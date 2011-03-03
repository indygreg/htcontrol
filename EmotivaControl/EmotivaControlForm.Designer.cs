namespace EmotivaControl
{
    partial class EmotivaControlForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmotivaControlForm));
            this.btnPowerOn = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnPowerOff = new System.Windows.Forms.Button();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.radioButtonCD = new System.Windows.Forms.RadioButton();
            this.radioButtonSAT = new System.Windows.Forms.RadioButton();
            this.radioButtonDVD = new System.Windows.Forms.RadioButton();
            this.radioButtonPhono = new System.Windows.Forms.RadioButton();
            this.radioButtonTuner = new System.Windows.Forms.RadioButton();
            this.radioButton8Channel = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonTape = new System.Windows.Forms.RadioButton();
            this.radioButtonVCR = new System.Windows.Forms.RadioButton();
            this.radioButtonVID2 = new System.Windows.Forms.RadioButton();
            this.radioButtonVID1 = new System.Windows.Forms.RadioButton();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPowerOn
            // 
            this.btnPowerOn.Location = new System.Drawing.Point(76, 57);
            this.btnPowerOn.Name = "btnPowerOn";
            this.btnPowerOn.Size = new System.Drawing.Size(75, 23);
            this.btnPowerOn.TabIndex = 0;
            this.btnPowerOn.Text = "On";
            this.btnPowerOn.UseVisualStyleBackColor = true;
            this.btnPowerOn.Click += new System.EventHandler(this.btnPowerOn_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Emotiva Controller";
            this.notifyIcon1.Visible = true;
            // 
            // btnPowerOff
            // 
            this.btnPowerOff.Location = new System.Drawing.Point(171, 57);
            this.btnPowerOff.Name = "btnPowerOff";
            this.btnPowerOff.Size = new System.Drawing.Size(75, 23);
            this.btnPowerOff.TabIndex = 2;
            this.btnPowerOff.Text = "Off";
            this.btnPowerOff.UseVisualStyleBackColor = true;
            this.btnPowerOff.Click += new System.EventHandler(this.btnPowerOff_Click);
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Location = new System.Drawing.Point(12, 57);
            this.trackBarVolume.Maximum = 99;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarVolume.Size = new System.Drawing.Size(45, 104);
            this.trackBarVolume.SmallChange = 5;
            this.trackBarVolume.TabIndex = 3;
            this.trackBarVolume.TickFrequency = 10;
            this.trackBarVolume.Scroll += new System.EventHandler(this.trackBarVolume_Scroll);
            // 
            // radioButtonCD
            // 
            this.radioButtonCD.AutoSize = true;
            this.radioButtonCD.Location = new System.Drawing.Point(16, 19);
            this.radioButtonCD.Name = "radioButtonCD";
            this.radioButtonCD.Size = new System.Drawing.Size(40, 17);
            this.radioButtonCD.TabIndex = 4;
            this.radioButtonCD.TabStop = true;
            this.radioButtonCD.Text = "CD";
            this.radioButtonCD.UseVisualStyleBackColor = true;
            this.radioButtonCD.CheckedChanged += new System.EventHandler(this.radioButtonCD_CheckedChanged);
            // 
            // radioButtonSAT
            // 
            this.radioButtonSAT.AutoSize = true;
            this.radioButtonSAT.Location = new System.Drawing.Point(16, 45);
            this.radioButtonSAT.Name = "radioButtonSAT";
            this.radioButtonSAT.Size = new System.Drawing.Size(46, 17);
            this.radioButtonSAT.TabIndex = 5;
            this.radioButtonSAT.TabStop = true;
            this.radioButtonSAT.Text = "SAT";
            this.radioButtonSAT.UseVisualStyleBackColor = true;
            this.radioButtonSAT.CheckedChanged += new System.EventHandler(this.radioButtonSAT_CheckedChanged);
            // 
            // radioButtonDVD
            // 
            this.radioButtonDVD.AutoSize = true;
            this.radioButtonDVD.Location = new System.Drawing.Point(16, 68);
            this.radioButtonDVD.Name = "radioButtonDVD";
            this.radioButtonDVD.Size = new System.Drawing.Size(48, 17);
            this.radioButtonDVD.TabIndex = 6;
            this.radioButtonDVD.TabStop = true;
            this.radioButtonDVD.Text = "DVD";
            this.radioButtonDVD.UseVisualStyleBackColor = true;
            this.radioButtonDVD.CheckedChanged += new System.EventHandler(this.radioButtonDVD_CheckedChanged);
            // 
            // radioButtonPhono
            // 
            this.radioButtonPhono.AutoSize = true;
            this.radioButtonPhono.Location = new System.Drawing.Point(16, 91);
            this.radioButtonPhono.Name = "radioButtonPhono";
            this.radioButtonPhono.Size = new System.Drawing.Size(56, 17);
            this.radioButtonPhono.TabIndex = 7;
            this.radioButtonPhono.TabStop = true;
            this.radioButtonPhono.Text = "Phono";
            this.radioButtonPhono.UseVisualStyleBackColor = true;
            this.radioButtonPhono.CheckedChanged += new System.EventHandler(this.radioButtonPhono_CheckedChanged);
            // 
            // radioButtonTuner
            // 
            this.radioButtonTuner.AutoSize = true;
            this.radioButtonTuner.Location = new System.Drawing.Point(16, 114);
            this.radioButtonTuner.Name = "radioButtonTuner";
            this.radioButtonTuner.Size = new System.Drawing.Size(53, 17);
            this.radioButtonTuner.TabIndex = 8;
            this.radioButtonTuner.TabStop = true;
            this.radioButtonTuner.Text = "Tuner";
            this.radioButtonTuner.UseVisualStyleBackColor = true;
            this.radioButtonTuner.CheckedChanged += new System.EventHandler(this.radioButtonTuner_CheckedChanged);
            // 
            // radioButton8Channel
            // 
            this.radioButton8Channel.AutoSize = true;
            this.radioButton8Channel.Location = new System.Drawing.Point(16, 137);
            this.radioButton8Channel.Name = "radioButton8Channel";
            this.radioButton8Channel.Size = new System.Drawing.Size(73, 17);
            this.radioButton8Channel.TabIndex = 9;
            this.radioButton8Channel.TabStop = true;
            this.radioButton8Channel.Text = "8-Channel";
            this.radioButton8Channel.UseVisualStyleBackColor = true;
            this.radioButton8Channel.CheckedChanged += new System.EventHandler(this.radioButton8Channel_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonTape);
            this.groupBox1.Controls.Add(this.radioButtonVCR);
            this.groupBox1.Controls.Add(this.radioButtonVID2);
            this.groupBox1.Controls.Add(this.radioButtonVID1);
            this.groupBox1.Controls.Add(this.radioButton8Channel);
            this.groupBox1.Controls.Add(this.radioButtonTuner);
            this.groupBox1.Controls.Add(this.radioButtonPhono);
            this.groupBox1.Controls.Add(this.radioButtonDVD);
            this.groupBox1.Controls.Add(this.radioButtonSAT);
            this.groupBox1.Controls.Add(this.radioButtonCD);
            this.groupBox1.Location = new System.Drawing.Point(271, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 264);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // radioButtonTape
            // 
            this.radioButtonTape.AutoSize = true;
            this.radioButtonTape.Location = new System.Drawing.Point(16, 233);
            this.radioButtonTape.Name = "radioButtonTape";
            this.radioButtonTape.Size = new System.Drawing.Size(50, 17);
            this.radioButtonTape.TabIndex = 13;
            this.radioButtonTape.TabStop = true;
            this.radioButtonTape.Text = "Tape";
            this.radioButtonTape.UseVisualStyleBackColor = true;
            this.radioButtonTape.CheckedChanged += new System.EventHandler(this.radioButtonTape_CheckedChanged);
            // 
            // radioButtonVCR
            // 
            this.radioButtonVCR.AutoSize = true;
            this.radioButtonVCR.Location = new System.Drawing.Point(16, 209);
            this.radioButtonVCR.Name = "radioButtonVCR";
            this.radioButtonVCR.Size = new System.Drawing.Size(47, 17);
            this.radioButtonVCR.TabIndex = 12;
            this.radioButtonVCR.TabStop = true;
            this.radioButtonVCR.Text = "VCR";
            this.radioButtonVCR.UseVisualStyleBackColor = true;
            this.radioButtonVCR.CheckedChanged += new System.EventHandler(this.radioButtonVCR_CheckedChanged);
            // 
            // radioButtonVID2
            // 
            this.radioButtonVID2.AutoSize = true;
            this.radioButtonVID2.Location = new System.Drawing.Point(16, 185);
            this.radioButtonVID2.Name = "radioButtonVID2";
            this.radioButtonVID2.Size = new System.Drawing.Size(49, 17);
            this.radioButtonVID2.TabIndex = 11;
            this.radioButtonVID2.TabStop = true;
            this.radioButtonVID2.Text = "VID2";
            this.radioButtonVID2.UseVisualStyleBackColor = true;
            this.radioButtonVID2.CheckedChanged += new System.EventHandler(this.radioButtonVID2_CheckedChanged);
            // 
            // radioButtonVID1
            // 
            this.radioButtonVID1.AutoSize = true;
            this.radioButtonVID1.Location = new System.Drawing.Point(16, 161);
            this.radioButtonVID1.Name = "radioButtonVID1";
            this.radioButtonVID1.Size = new System.Drawing.Size(49, 17);
            this.radioButtonVID1.TabIndex = 10;
            this.radioButtonVID1.TabStop = true;
            this.radioButtonVID1.Text = "VID1";
            this.radioButtonVID1.UseVisualStyleBackColor = true;
            this.radioButtonVID1.CheckedChanged += new System.EventHandler(this.radioButtonVID1_CheckedChanged);
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(41, 12);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(76, 21);
            this.comboBoxPorts.TabIndex = 12;
            this.comboBoxPorts.SelectedIndexChanged += new System.EventHandler(this.comboBoxPorts_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Port";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(76, 197);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(131, 79);
            this.textBox1.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(76, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(76, 303);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save Settings";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 388);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxPorts);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.trackBarVolume);
            this.Controls.Add(this.btnPowerOff);
            this.Controls.Add(this.btnPowerOn);
            this.Name = "Form1";
            this.Text = "Emotiva Processor Controller";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPowerOn;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnPowerOff;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.RadioButton radioButtonCD;
        private System.Windows.Forms.RadioButton radioButtonSAT;
        private System.Windows.Forms.RadioButton radioButtonDVD;
        private System.Windows.Forms.RadioButton radioButtonPhono;
        private System.Windows.Forms.RadioButton radioButtonTuner;
        private System.Windows.Forms.RadioButton radioButton8Channel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonTape;
        private System.Windows.Forms.RadioButton radioButtonVCR;
        private System.Windows.Forms.RadioButton radioButtonVID2;
        private System.Windows.Forms.RadioButton radioButtonVID1;
        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSave;
    }
}

