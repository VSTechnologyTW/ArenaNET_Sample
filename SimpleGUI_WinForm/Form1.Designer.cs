namespace SimpleGUI_WinForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ConsoleView = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.InitializeButton = new System.Windows.Forms.Button();
            this.GrabbedImage = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrabbedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.GrabbedImage, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 403);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ConsoleView);
            this.panel1.Controls.Add(this.CloseButton);
            this.panel1.Controls.Add(this.OpenButton);
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.InitializeButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 397);
            this.panel1.TabIndex = 0;
            // 
            // ConsoleView
            // 
            this.ConsoleView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConsoleView.Location = new System.Drawing.Point(0, 124);
            this.ConsoleView.Multiline = true;
            this.ConsoleView.Name = "ConsoleView";
            this.ConsoleView.ReadOnly = true;
            this.ConsoleView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConsoleView.Size = new System.Drawing.Size(344, 273);
            this.ConsoleView.TabIndex = 4;
            // 
            // CloseButton
            // 
            this.CloseButton.AutoSize = true;
            this.CloseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CloseButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CloseButton.Enabled = false;
            this.CloseButton.Location = new System.Drawing.Point(0, 93);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(5);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Padding = new System.Windows.Forms.Padding(3);
            this.CloseButton.Size = new System.Drawing.Size(344, 31);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "4.Close Device";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.AutoSize = true;
            this.OpenButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OpenButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.OpenButton.Enabled = false;
            this.OpenButton.Location = new System.Drawing.Point(0, 62);
            this.OpenButton.Margin = new System.Windows.Forms.Padding(5);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Padding = new System.Windows.Forms.Padding(3);
            this.OpenButton.Size = new System.Drawing.Size(344, 31);
            this.OpenButton.TabIndex = 2;
            this.OpenButton.Text = "3.Open First Device, grab 10 photos";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // SearchButton
            // 
            this.SearchButton.AutoSize = true;
            this.SearchButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SearchButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.SearchButton.Enabled = false;
            this.SearchButton.Location = new System.Drawing.Point(0, 31);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(5);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Padding = new System.Windows.Forms.Padding(3);
            this.SearchButton.Size = new System.Drawing.Size(344, 31);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "2.Search Device";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // InitializeButton
            // 
            this.InitializeButton.AutoSize = true;
            this.InitializeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.InitializeButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.InitializeButton.Location = new System.Drawing.Point(0, 0);
            this.InitializeButton.Margin = new System.Windows.Forms.Padding(5);
            this.InitializeButton.Name = "InitializeButton";
            this.InitializeButton.Padding = new System.Windows.Forms.Padding(3);
            this.InitializeButton.Size = new System.Drawing.Size(344, 31);
            this.InitializeButton.TabIndex = 0;
            this.InitializeButton.Text = "1.Initialize";
            this.InitializeButton.UseVisualStyleBackColor = true;
            this.InitializeButton.Click += new System.EventHandler(this.InitializeButton_Click);
            // 
            // GrabbedImage
            // 
            this.GrabbedImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrabbedImage.Location = new System.Drawing.Point(353, 3);
            this.GrabbedImage.Name = "GrabbedImage";
            this.GrabbedImage.Size = new System.Drawing.Size(426, 397);
            this.GrabbedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.GrabbedImage.TabIndex = 1;
            this.GrabbedImage.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 403);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrabbedImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button InitializeButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TextBox ConsoleView;
        private System.Windows.Forms.PictureBox GrabbedImage;
    }
}

