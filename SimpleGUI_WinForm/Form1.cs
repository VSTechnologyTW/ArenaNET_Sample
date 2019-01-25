using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenaNET;

namespace SimpleGUI_WinForm
{
    /// <summary>
    /// This example demonstrates the integration of a camera with a GUI application (Windows Form framework).
    /// </summary>
    public partial class Form1 : Form
    {

        public IDevice Device { get; set; }

        public ISystem ArenaNETSystem { get; set; }

        public CancellationTokenSource ImagePollingCancellationTokenSource { get; set; }

        public Task PollingTask { get; set; }

        public int FrameRate { get; set; } = 10;

        public int PollingImageTimeout => (1000 / FrameRate) + 200;

        public int FrameCount { get; set; } = 10;

        public Form1()
        {
            InitializeComponent();
            ImagePollingCancellationTokenSource = new CancellationTokenSource();
        }

        #region Methods

        private void SetUpCamera(IDevice device)
        {
            // Set AcquisitionMode
            (device.NodeMap.GetNode("AcquisitionMode") as IEnumeration).FromString("Continuous");
            (device.NodeMap.GetNode("TriggerMode") as IEnumeration).FromString("Off");

            // Set the FrameRate
            (device.NodeMap.GetNode("AcquisitionFrameRateEnable") as IBoolean).Value = true;
            (device.NodeMap.GetNode("AcquisitionFrameRate") as IFloat).Value = FrameRate;
        }

        private void UpdateConsoleView(object sender, string str)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<string>(UpdateConsoleView), sender, str);
                return;
            }

            var textbox = sender as TextBox;
            if (textbox != null)
            {
                if (string.IsNullOrEmpty(str))
                {
                    textbox.Text = string.Empty;
                }
                else
                {
                    textbox.Text += str;
                }
            }
        }

        private void UpdateImage(object sender, IImage image)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<IImage>(UpdateImage), sender, image);
                return;
            }

            UpdateConsoleView(ConsoleView, $"Image Grabbed({image.FrameId}). - {DateTime.Now}" + Environment.NewLine);

            // Release resources
            if (GrabbedImage.Image != null)
            {
                GrabbedImage.Image.Dispose();
                ArenaNET.ImageFactory.Destroy(GrabbedImage.Tag as IImage);
            }

            // Convert image to rgb24 format
            var bgr24Image = ArenaNET.ImageFactory.Convert(image, EPfncFormat.BGR8);

            // Set image
            GrabbedImage.Image = bgr24Image.Bitmap;
            GrabbedImage.Tag = bgr24Image;

            // Release source image
            ArenaNET.ImageFactory.Destroy(image);
        }

        private Task StartPollingImages(CancellationTokenSource cancellationTokenSource)
        {
            return Task.Run(() =>
            {
                var image_count = 0;
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        if (!Device.IsConnected)
                        {
                            UpdateConsoleView(ConsoleView, $"Camera Disconnected. - {DateTime.Now}" + Environment.NewLine);
                            break;
                        }


                        // Get Image
                        var image = Device.GetImage((ulong)PollingImageTimeout);

                        // Drop incompletely image
                        if (image.IsIncomplete)
                        {
                            Device.RequeueBuffer(image);
                            UpdateConsoleView(ConsoleView, $"Missing data. - {DateTime.Now}" + Environment.NewLine);
                            continue;
                        }

                        // Copy & RequeueBuffer
                        var copyImage = ArenaNET.ImageFactory.Copy(image);
                        Device.RequeueBuffer(image);

                        // ===========================================
                        // Do Something

                        // Show image
                        UpdateImage(GrabbedImage, copyImage);
                        image_count++;
                        if (image_count == FrameCount)
                            break;

                    }
                    catch (ArenaNET.GenericException)
                    {
                        // ignore
                    }
                    catch (Exception exception)
                    {
                        if (Device != null && Device.IsConnected)
                            MessageBox.Show(exception.Message);
                    }

                }
            }, cancellationTokenSource.Token);
        }

        #endregion

        #region UI Event

        private void InitializeButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear hte ConsoleView
                ConsoleView.Text = "";

                // Open system
                ArenaNETSystem = ArenaNET.Arena.OpenSystem();

                // Update information
                ConsoleView.Text += $"ArenaNETSystem already opened. - {DateTime.Now}" + Environment.NewLine;
                InitializeButton.Enabled = false;
                SearchButton.Enabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Update device list
                if (ArenaNETSystem.UpdateDevices(1000))
                {
                    if (ArenaNETSystem.Devices.Count != 0)
                    {
                        // if at least one device found, open it
                        Device = ArenaNETSystem.CreateDevice(ArenaNETSystem.Devices[0]);

                        // Update information
                        ConsoleView.Text += $"{ArenaNETSystem.Devices.Count } devices found. - {DateTime.Now}" + Environment.NewLine;
                        ConsoleView.Text += $"First camera opened. - {DateTime.Now}" + Environment.NewLine;

                        SearchButton.Enabled = false;
                        OpenButton.Enabled = true;
                    }
                    else
                    {
                        ConsoleView.Text += $"Devices not found. - {DateTime.Now}" + Environment.NewLine;
                    }
                }
                else
                {
                    ConsoleView.Text += $"Devices not found. - {DateTime.Now}" + Environment.NewLine;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Set up camera
                SetUpCamera(Device);

                // Start polling task, polling image from different thread
                ImagePollingCancellationTokenSource = new CancellationTokenSource();
                PollingTask = StartPollingImages(ImagePollingCancellationTokenSource);

                // Start stream
                Device.StartStream();

                OpenButton.Enabled = false;
                CloseButton.Enabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Stop polling task and wait it stopped
                ImagePollingCancellationTokenSource.Cancel(true);
                PollingTask.Wait(PollingImageTimeout);

                // Stop stream
                Device.StopStream();

                // Release resources
                ArenaNETSystem.DestroyDevice(Device);
                ArenaNET.Arena.CloseSystem(ArenaNETSystem);

                InitializeButton.Enabled = true;
                CloseButton.Enabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion

    }
}
