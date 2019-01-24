using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using ArenaNET;
using Arena = ArenaNET.Arena;
using TimeoutException = ArenaNET.TimeoutException;

namespace SimpleGUI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IDevice Device { get; set; }
        public ISystem ArenaNETSystem { get; set; }

        public CancellationTokenSource ImagePollingCancellationTokenSource { get; set; }
        public Task PollingTask { get; set; }
        public int FrameRate { get; set; } = 10;
        public int PollingImageTimeout
        {
            get
            {
                return (1000 / FrameRate) + 200;
            }
        }

        public int FrameCount { get; set; } = 10;

        public MainWindow()
        {
            InitializeComponent();
            GrabbedImage.Source = new WriteableBitmap(800, 600, 96, 96, PixelFormats.Bgr24, null);
            ImagePollingCancellationTokenSource = new CancellationTokenSource();
        }

        #region Methods

        private void SetUpCamera(IDevice device)
        {
            (device.NodeMap.GetNode("AcquisitionMode") as IEnumeration).FromString("Continuous");
            (device.NodeMap.GetNode("TriggerMode") as IEnumeration).FromString("Off");
            (device.NodeMap.GetNode("AcquisitionFrameRateEnable") as IBoolean).Value = true;
            (device.NodeMap.GetNode("AcquisitionFrameRate") as IFloat).Value = FrameRate;
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
                            Dispatcher.InvokeAsync(() =>
                            {
                                ConsoleView.Text += $"Camera Disconnected. - {DateTime.Now}" + Environment.NewLine;
                            });
                            break;
                        }


                        // Get Image
                        var image = Device.GetImage((ulong)PollingImageTimeout);

                        // Drop incompletely image
                        if (image.IsIncomplete)
                        {
                            Device.RequeueBuffer(image);
                            Dispatcher.InvokeAsync(() =>
                            {
                                ConsoleView.Text += $"Missing data. - {DateTime.Now}" + Environment.NewLine;
                            });
                            continue;
                        }

                        // Copy & RequeueBuffer
                        var copyImage = ArenaNET.ImageFactory.Copy(image);
                        Device.RequeueBuffer(image);

                        // ===========================================
                        // Do Something

                        // Show image , using BGR8 (BGR24)
                        Dispatcher.InvokeAsync(() =>
                       {
                           ConsoleView.Text += $"Image Grabbed({copyImage.FrameId}). - {DateTime.Now}" +
                                               Environment.NewLine;
                           var wb = GrabbedImage.Source as WriteableBitmap;
                           if (wb == null || wb.PixelWidth != copyImage.Width || wb.PixelHeight != copyImage.Height)
                           {
                               GrabbedImage.Source = new WriteableBitmap((int)copyImage.Width, (int)copyImage.Height,
                                   96, 96, PixelFormats.Bgr24, null);
                               wb = GrabbedImage.Source as WriteableBitmap;
                           }

                           var bgr24Image = ArenaNET.ImageFactory.Convert(copyImage, EPfncFormat.BGR8);
                           wb.Lock();
                           Marshal.Copy(bgr24Image.DataArray, 0, wb.BackBuffer, bgr24Image.DataArray.Length);
                           wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
                           wb.Unlock();

                           // Remember to release the resources
                           ArenaNET.ImageFactory.Destroy(copyImage);
                           ArenaNET.ImageFactory.Destroy(bgr24Image);
                       });
                        image_count++;
                        if (image_count == FrameCount)
                            break;

                    }
                    catch (TimeoutException)
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

        private void InitializeButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ConsoleView.Text = "";
                ArenaNETSystem = ArenaNET.Arena.OpenSystem();
                ConsoleView.Text += $"ArenaNETSystem already opened. - {DateTime.Now}" + Environment.NewLine;
                InitializeButton.IsEnabled = false;
                SearchButton.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ArenaNETSystem.UpdateDevices(1000))
                {
                    if (ArenaNETSystem.Devices.Count != 0)
                    {
                        Device = ArenaNETSystem.CreateDevice(ArenaNETSystem.Devices[0]);
                        ConsoleView.Text += $"{ArenaNETSystem.Devices.Count } devices found. - {DateTime.Now}" + Environment.NewLine;
                        ConsoleView.Text += $"First camera opened. - {DateTime.Now}" + Environment.NewLine;
                        SearchButton.IsEnabled = false;
                        OpenButton.IsEnabled = true;
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

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SetUpCamera(Device);
                ImagePollingCancellationTokenSource = new CancellationTokenSource();
                Device.StartStream();
                OpenButton.IsEnabled = false;
                CloseButton.IsEnabled = true;
                PollingTask = StartPollingImages(ImagePollingCancellationTokenSource);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ImagePollingCancellationTokenSource.Cancel(true);
                PollingTask.Wait(PollingImageTimeout);
                Device.StopStream();
                ArenaNETSystem.DestroyDevice(Device);
                ArenaNET.Arena.CloseSystem(ArenaNETSystem);
                InitializeButton.IsEnabled = true;
                CloseButton.IsEnabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion

    }
}
