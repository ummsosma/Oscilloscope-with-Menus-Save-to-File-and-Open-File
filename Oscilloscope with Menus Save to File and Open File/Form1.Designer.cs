namespace Lab12
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.grpBox = new System.Windows.Forms.GroupBox();
            this.updSamplesPerChannel = new System.Windows.Forms.NumericUpDown();
            this.updSampleRate = new System.Windows.Forms.NumericUpDown();
            this.updHighChannel = new System.Windows.Forms.NumericUpDown();
            this.updLowChannel = new System.Windows.Forms.NumericUpDown();
            this.txtADRate = new System.Windows.Forms.TextBox();
            this.txtAcqTime = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboVoltageRange = new System.Windows.Forms.ComboBox();
            this.cboTerminal = new System.Windows.Forms.ComboBox();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.chtData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acquireToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSamplesPerChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updSampleRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updHighChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updLowChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox
            // 
            this.grpBox.Controls.Add(this.updSamplesPerChannel);
            this.grpBox.Controls.Add(this.updSampleRate);
            this.grpBox.Controls.Add(this.updHighChannel);
            this.grpBox.Controls.Add(this.updLowChannel);
            this.grpBox.Controls.Add(this.txtADRate);
            this.grpBox.Controls.Add(this.txtAcqTime);
            this.grpBox.Controls.Add(this.label10);
            this.grpBox.Controls.Add(this.label9);
            this.grpBox.Controls.Add(this.label8);
            this.grpBox.Controls.Add(this.label7);
            this.grpBox.Controls.Add(this.label6);
            this.grpBox.Controls.Add(this.label5);
            this.grpBox.Controls.Add(this.label4);
            this.grpBox.Controls.Add(this.label3);
            this.grpBox.Controls.Add(this.label2);
            this.grpBox.Controls.Add(this.label1);
            this.grpBox.Controls.Add(this.cboVoltageRange);
            this.grpBox.Controls.Add(this.cboTerminal);
            this.grpBox.Controls.Add(this.cboPorts);
            this.grpBox.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBox.Location = new System.Drawing.Point(18, 61);
            this.grpBox.Name = "grpBox";
            this.grpBox.Size = new System.Drawing.Size(540, 410);
            this.grpBox.TabIndex = 0;
            this.grpBox.TabStop = false;
            this.grpBox.Text = "DAQ Configuration";
            // 
            // updSamplesPerChannel
            // 
            this.updSamplesPerChannel.Location = new System.Drawing.Point(27, 251);
            this.updSamplesPerChannel.Maximum = new decimal(new int[] {
            250000,
            0,
            0,
            0});
            this.updSamplesPerChannel.Name = "updSamplesPerChannel";
            this.updSamplesPerChannel.Size = new System.Drawing.Size(221, 24);
            this.updSamplesPerChannel.TabIndex = 26;
            this.updSamplesPerChannel.ValueChanged += new System.EventHandler(this.updSamplesPerChannel_ValueChanged);
            // 
            // updSampleRate
            // 
            this.updSampleRate.Location = new System.Drawing.Point(27, 194);
            this.updSampleRate.Maximum = new decimal(new int[] {
            250000,
            0,
            0,
            0});
            this.updSampleRate.Name = "updSampleRate";
            this.updSampleRate.Size = new System.Drawing.Size(221, 24);
            this.updSampleRate.TabIndex = 25;
            this.updSampleRate.ValueChanged += new System.EventHandler(this.updSampleRate_ValueChanged);
            // 
            // updHighChannel
            // 
            this.updHighChannel.Location = new System.Drawing.Point(116, 139);
            this.updHighChannel.Name = "updHighChannel";
            this.updHighChannel.Size = new System.Drawing.Size(83, 24);
            this.updHighChannel.TabIndex = 24;
            this.updHighChannel.ValueChanged += new System.EventHandler(this.updHighChannel_ValueChanged);
            // 
            // updLowChannel
            // 
            this.updLowChannel.Location = new System.Drawing.Point(27, 139);
            this.updLowChannel.Name = "updLowChannel";
            this.updLowChannel.Size = new System.Drawing.Size(83, 24);
            this.updLowChannel.TabIndex = 23;
            this.updLowChannel.ValueChanged += new System.EventHandler(this.updLowChannel_ValueChanged);
            // 
            // txtADRate
            // 
            this.txtADRate.Location = new System.Drawing.Point(286, 250);
            this.txtADRate.Name = "txtADRate";
            this.txtADRate.ReadOnly = true;
            this.txtADRate.Size = new System.Drawing.Size(215, 24);
            this.txtADRate.TabIndex = 21;
            // 
            // txtAcqTime
            // 
            this.txtAcqTime.Location = new System.Drawing.Point(286, 193);
            this.txtAcqTime.Name = "txtAcqTime";
            this.txtAcqTime.ReadOnly = true;
            this.txtAcqTime.Size = new System.Drawing.Size(215, 24);
            this.txtAcqTime.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(283, 230);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "A/D Rate (S/s)\r\n";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(283, 174);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 17);
            this.label9.TabIndex = 18;
            this.label9.Text = "Acquisition Time (s):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Channel Range:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 231);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(174, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Number Samples / Channel:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Channel Sample Rate:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(282, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Voltage Range:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "High Channel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Low Channel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Terminal Configuration:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Device:";
            // 
            // cboVoltageRange
            // 
            this.cboVoltageRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVoltageRange.FormattingEnabled = true;
            this.cboVoltageRange.Items.AddRange(new object[] {
            "±0.2 V",
            "±1 V ",
            "±5 V ",
            "±10 V"});
            this.cboVoltageRange.Location = new System.Drawing.Point(285, 138);
            this.cboVoltageRange.Name = "cboVoltageRange";
            this.cboVoltageRange.Size = new System.Drawing.Size(91, 25);
            this.cboVoltageRange.TabIndex = 8;
            // 
            // cboTerminal
            // 
            this.cboTerminal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTerminal.FormattingEnabled = true;
            this.cboTerminal.Items.AddRange(new object[] {
            "Nrse",
            "Rse",
            "Differential"});
            this.cboTerminal.Location = new System.Drawing.Point(285, 46);
            this.cboTerminal.Name = "cboTerminal";
            this.cboTerminal.Size = new System.Drawing.Size(248, 25);
            this.cboTerminal.TabIndex = 1;
            // 
            // cboPorts
            // 
            this.cboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(26, 46);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(248, 25);
            this.cboPorts.TabIndex = 0;
            // 
            // chtData
            // 
            chartArea2.Name = "ChartArea1";
            this.chtData.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chtData.Legends.Add(legend2);
            this.chtData.Location = new System.Drawing.Point(564, 61);
            this.chtData.Name = "chtData";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chtData.Series.Add(series2);
            this.chtData.Size = new System.Drawing.Size(603, 410);
            this.chtData.TabIndex = 1;
            this.chtData.Text = "chart1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.acquireToolStripMenuItem,
            this.clearChartToolStripMenuItem,
            this.configToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1191, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.appendToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // appendToolStripMenuItem
            // 
            this.appendToolStripMenuItem.Name = "appendToolStripMenuItem";
            this.appendToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.appendToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.appendToolStripMenuItem.Text = "Append";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // acquireToolStripMenuItem
            // 
            this.acquireToolStripMenuItem.Name = "acquireToolStripMenuItem";
            this.acquireToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.acquireToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.acquireToolStripMenuItem.Text = "Acquire";
            this.acquireToolStripMenuItem.Click += new System.EventHandler(this.acquireToolStripMenuItem_Click);
            // 
            // clearChartToolStripMenuItem
            // 
            this.clearChartToolStripMenuItem.Name = "clearChartToolStripMenuItem";
            this.clearChartToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.clearChartToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.clearChartToolStripMenuItem.Text = "Clear Chart";
            this.clearChartToolStripMenuItem.Click += new System.EventHandler(this.clearChartToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
            this.configToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.configToolStripMenuItem.Text = "Help";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 501);
            this.Controls.Add(this.chtData);
            this.Controls.Add(this.grpBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Oscilloscope with Menus, Save to File and Open File";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpBox.ResumeLayout(false);
            this.grpBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSamplesPerChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updSampleRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updHighChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updLowChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboVoltageRange;
        private System.Windows.Forms.ComboBox cboTerminal;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtData;
        private System.Windows.Forms.TextBox txtADRate;
        private System.Windows.Forms.TextBox txtAcqTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown updSamplesPerChannel;
        private System.Windows.Forms.NumericUpDown updSampleRate;
        private System.Windows.Forms.NumericUpDown updHighChannel;
        private System.Windows.Forms.NumericUpDown updLowChannel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acquireToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearChartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appendToolStripMenuItem;
    }
}

