namespace BRI_button_test
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.btnEnableBRI = new System.Windows.Forms.Button();
            this.btnDisableBRI = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGet = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.chkLeftUpper = new System.Windows.Forms.CheckBox();
            this.chkRightUpper = new System.Windows.Forms.CheckBox();
            this.chkLeftLower = new System.Windows.Forms.CheckBox();
            this.chkRightLower = new System.Windows.Forms.CheckBox();
            this.chkCenterScan = new System.Windows.Forms.CheckBox();
            this.chkButtonMiddle = new System.Windows.Forms.CheckBox();
            this.btnGetRFIDMapping = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEnableBRI
            // 
            this.btnEnableBRI.Location = new System.Drawing.Point(115, 10);
            this.btnEnableBRI.Name = "btnEnableBRI";
            this.btnEnableBRI.Size = new System.Drawing.Size(51, 19);
            this.btnEnableBRI.TabIndex = 0;
            this.btnEnableBRI.Text = "Enable";
            this.btnEnableBRI.Click += new System.EventHandler(this.btnEnableBRI_Click);
            // 
            // btnDisableBRI
            // 
            this.btnDisableBRI.Location = new System.Drawing.Point(179, 10);
            this.btnDisableBRI.Name = "btnDisableBRI";
            this.btnDisableBRI.Size = new System.Drawing.Size(58, 19);
            this.btnDisableBRI.TabIndex = 0;
            this.btnDisableBRI.Text = "Disable";
            this.btnDisableBRI.Click += new System.EventHandler(this.btnDisableBRI_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.Text = "Middle to Scan:";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(63, 35);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(46, 21);
            this.btnGet.TabIndex = 2;
            this.btnGet.Text = "get";
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(115, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(112, 21);
            this.textBox1.TabIndex = 3;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(5, 161);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(231, 104);
            this.txtLog.TabIndex = 4;
            // 
            // chkLeftUpper
            // 
            this.chkLeftUpper.Location = new System.Drawing.Point(13, 86);
            this.chkLeftUpper.Name = "chkLeftUpper";
            this.chkLeftUpper.Size = new System.Drawing.Size(65, 19);
            this.chkLeftUpper.TabIndex = 6;
            this.chkLeftUpper.Text = "upper";
            this.chkLeftUpper.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // chkRightUpper
            // 
            this.chkRightUpper.Location = new System.Drawing.Point(172, 86);
            this.chkRightUpper.Name = "chkRightUpper";
            this.chkRightUpper.Size = new System.Drawing.Size(64, 19);
            this.chkRightUpper.TabIndex = 6;
            this.chkRightUpper.Text = "upper";
            this.chkRightUpper.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // chkLeftLower
            // 
            this.chkLeftLower.Location = new System.Drawing.Point(13, 108);
            this.chkLeftLower.Name = "chkLeftLower";
            this.chkLeftLower.Size = new System.Drawing.Size(65, 19);
            this.chkLeftLower.TabIndex = 6;
            this.chkLeftLower.Text = "lower";
            this.chkLeftLower.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // chkRightLower
            // 
            this.chkRightLower.Location = new System.Drawing.Point(172, 108);
            this.chkRightLower.Name = "chkRightLower";
            this.chkRightLower.Size = new System.Drawing.Size(64, 19);
            this.chkRightLower.TabIndex = 6;
            this.chkRightLower.Text = "lower";
            this.chkRightLower.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // chkCenterScan
            // 
            this.chkCenterScan.Location = new System.Drawing.Point(84, 108);
            this.chkCenterScan.Name = "chkCenterScan";
            this.chkCenterScan.Size = new System.Drawing.Size(82, 15);
            this.chkCenterScan.TabIndex = 7;
            this.chkCenterScan.Text = "Center";
            this.chkCenterScan.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // chkButtonMiddle
            // 
            this.chkButtonMiddle.Location = new System.Drawing.Point(84, 86);
            this.chkButtonMiddle.Name = "chkButtonMiddle";
            this.chkButtonMiddle.Size = new System.Drawing.Size(82, 15);
            this.chkButtonMiddle.TabIndex = 7;
            this.chkButtonMiddle.Text = "Middle";
            this.chkButtonMiddle.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // btnGetRFIDMapping
            // 
            this.btnGetRFIDMapping.Location = new System.Drawing.Point(63, 133);
            this.btnGetRFIDMapping.Name = "btnGetRFIDMapping";
            this.btnGetRFIDMapping.Size = new System.Drawing.Size(46, 22);
            this.btnGetRFIDMapping.TabIndex = 9;
            this.btnGetRFIDMapping.Text = "get";
            this.btnGetRFIDMapping.Click += new System.EventHandler(this.btnGetRFIDMapping_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 19);
            this.label2.Text = "RFID Mapping:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGetRFIDMapping);
            this.Controls.Add(this.chkButtonMiddle);
            this.Controls.Add(this.chkCenterScan);
            this.Controls.Add(this.chkRightLower);
            this.Controls.Add(this.chkRightUpper);
            this.Controls.Add(this.chkLeftLower);
            this.Controls.Add(this.chkLeftUpper);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDisableBRI);
            this.Controls.Add(this.btnEnableBRI);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEnableBRI;
        private System.Windows.Forms.Button btnDisableBRI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.CheckBox chkLeftUpper;
        private System.Windows.Forms.CheckBox chkRightUpper;
        private System.Windows.Forms.CheckBox chkLeftLower;
        private System.Windows.Forms.CheckBox chkRightLower;
        private System.Windows.Forms.CheckBox chkCenterScan;
        private System.Windows.Forms.CheckBox chkButtonMiddle;
        private System.Windows.Forms.Button btnGetRFIDMapping;
        private System.Windows.Forms.Label label2;
    }
}

