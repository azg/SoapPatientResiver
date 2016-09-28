namespace SoapPatientResiver
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
            this.cmbMis = new System.Windows.Forms.ComboBox();
            this.btnDoDownloadPatients = new System.Windows.Forms.Button();
            this.dbfpath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblmis = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnDownloadTalons = new System.Windows.Forms.Button();
            this.btnDubliesToDBF = new System.Windows.Forms.Button();
            this.btn_csv_export = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.rbMonday = new System.Windows.Forms.RadioButton();
            this.rbVoluntaryDay = new System.Windows.Forms.RadioButton();
            this.gbCSV = new System.Windows.Forms.GroupBox();
            this.csvto2 = new System.Windows.Forms.RadioButton();
            this.btnHQLExp = new System.Windows.Forms.Button();
            this.gbIntervalExport = new System.Windows.Forms.GroupBox();
            this.txtFromDate = new System.Windows.Forms.MaskedTextBox();
            this.btMakeDBF = new System.Windows.Forms.Button();
            this.csvto5 = new System.Windows.Forms.RadioButton();
            this.gbCSV.SuspendLayout();
            this.gbIntervalExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbMis
            // 
            this.cmbMis.FormattingEnabled = true;
            this.cmbMis.Items.AddRange(new object[] {
            "172.20.0.213:8080",
            "172.20.0.211:8080",
            "109.109.217.25:8080",
            "109.109.217.23:8080",
            "82.162.57.113:8388",
            "82.162.57.113:8188",
            "192.168.1.210:8080",
            "192.168.1.209:8080"});
            this.cmbMis.Location = new System.Drawing.Point(133, 34);
            this.cmbMis.Name = "cmbMis";
            this.cmbMis.Size = new System.Drawing.Size(235, 21);
            this.cmbMis.TabIndex = 0;
            this.cmbMis.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnDoDownloadPatients
            // 
            this.btnDoDownloadPatients.Location = new System.Drawing.Point(26, 189);
            this.btnDoDownloadPatients.Name = "btnDoDownloadPatients";
            this.btnDoDownloadPatients.Size = new System.Drawing.Size(65, 23);
            this.btnDoDownloadPatients.TabIndex = 1;
            this.btnDoDownloadPatients.Text = "PatientsDbfExp";
            this.btnDoDownloadPatients.UseVisualStyleBackColor = true;
            this.btnDoDownloadPatients.Click += new System.EventHandler(this.btnDoDownload_Click);
            // 
            // dbfpath
            // 
            this.dbfpath.Location = new System.Drawing.Point(133, 86);
            this.dbfpath.Name = "dbfpath";
            this.dbfpath.ReadOnly = true;
            this.dbfpath.Size = new System.Drawing.Size(235, 20);
            this.dbfpath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(26, 86);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblmis
            // 
            this.lblmis.AutoSize = true;
            this.lblmis.Location = new System.Drawing.Point(23, 37);
            this.lblmis.Name = "lblmis";
            this.lblmis.Size = new System.Drawing.Size(36, 13);
            this.lblmis.TabIndex = 4;
            this.lblmis.Text = "Mis IP";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(26, 143);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(342, 23);
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // btnDownloadTalons
            // 
            this.btnDownloadTalons.Location = new System.Drawing.Point(97, 189);
            this.btnDownloadTalons.Name = "btnDownloadTalons";
            this.btnDownloadTalons.Size = new System.Drawing.Size(59, 23);
            this.btnDownloadTalons.TabIndex = 6;
            this.btnDownloadTalons.Text = "DBF_165_order";
            this.btnDownloadTalons.UseVisualStyleBackColor = true;
            this.btnDownloadTalons.Click += new System.EventHandler(this.btnDownloadTalons_Click);
            // 
            // btnDubliesToDBF
            // 
            this.btnDubliesToDBF.Location = new System.Drawing.Point(26, 218);
            this.btnDubliesToDBF.Name = "btnDubliesToDBF";
            this.btnDubliesToDBF.Size = new System.Drawing.Size(65, 23);
            this.btnDubliesToDBF.TabIndex = 7;
            this.btnDubliesToDBF.Text = "DubliesToDBF";
            this.btnDubliesToDBF.UseVisualStyleBackColor = true;
            this.btnDubliesToDBF.Click += new System.EventHandler(this.btnDubliesToDBF_Click);
            // 
            // btn_csv_export
            // 
            this.btn_csv_export.Location = new System.Drawing.Point(17, 18);
            this.btn_csv_export.Name = "btn_csv_export";
            this.btn_csv_export.Size = new System.Drawing.Size(159, 23);
            this.btn_csv_export.TabIndex = 8;
            this.btn_csv_export.Text = "Make csv";
            this.btn_csv_export.UseVisualStyleBackColor = true;
            this.btn_csv_export.Click += new System.EventHandler(this.btn_csv_export_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(512, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // rbMonday
            // 
            this.rbMonday.AutoSize = true;
            this.rbMonday.Location = new System.Drawing.Point(17, 47);
            this.rbMonday.Name = "rbMonday";
            this.rbMonday.Size = new System.Drawing.Size(83, 17);
            this.rbMonday.TabIndex = 10;
            this.rbMonday.TabStop = true;
            this.rbMonday.Text = "Do 3 reports";
            this.rbMonday.UseVisualStyleBackColor = true;
            this.rbMonday.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnRb3RDown);
            this.rbMonday.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnRb3RUp);
            // 
            // rbVoluntaryDay
            // 
            this.rbVoluntaryDay.AutoSize = true;
            this.rbVoluntaryDay.Location = new System.Drawing.Point(17, 67);
            this.rbVoluntaryDay.Name = "rbVoluntaryDay";
            this.rbVoluntaryDay.Size = new System.Drawing.Size(80, 17);
            this.rbVoluntaryDay.TabIndex = 11;
            this.rbVoluntaryDay.TabStop = true;
            this.rbVoluntaryDay.Text = "Проз. дата";
            this.rbVoluntaryDay.UseVisualStyleBackColor = true;
            this.rbVoluntaryDay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnRbVoluntaryDown);
            this.rbVoluntaryDay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnRbVoluntaryUp);
            // 
            // gbCSV
            // 
            this.gbCSV.Controls.Add(this.csvto5);
            this.gbCSV.Controls.Add(this.csvto2);
            this.gbCSV.Controls.Add(this.btn_csv_export);
            this.gbCSV.Controls.Add(this.rbVoluntaryDay);
            this.gbCSV.Controls.Add(this.rbMonday);
            this.gbCSV.Location = new System.Drawing.Point(175, 170);
            this.gbCSV.Name = "gbCSV";
            this.gbCSV.Size = new System.Drawing.Size(193, 84);
            this.gbCSV.TabIndex = 12;
            this.gbCSV.TabStop = false;
            this.gbCSV.Text = "CSV";
            // 
            // csvto2
            // 
            this.csvto2.AutoSize = true;
            this.csvto2.Location = new System.Drawing.Point(106, 47);
            this.csvto2.Name = "csvto2";
            this.csvto2.Size = new System.Drawing.Size(70, 17);
            this.csvto2.TabIndex = 12;
            this.csvto2.TabStop = true;
            this.csvto2.Text = "CSV за 2";
            this.csvto2.UseVisualStyleBackColor = true;
            // 
            // btnHQLExp
            // 
            this.btnHQLExp.Location = new System.Drawing.Point(97, 217);
            this.btnHQLExp.Name = "btnHQLExp";
            this.btnHQLExp.Size = new System.Drawing.Size(59, 23);
            this.btnHQLExp.TabIndex = 13;
            this.btnHQLExp.Text = "HQLPatientExp";
            this.btnHQLExp.UseVisualStyleBackColor = true;
            this.btnHQLExp.Click += new System.EventHandler(this.btnHQLExp_Click);
            // 
            // gbIntervalExport
            // 
            this.gbIntervalExport.Controls.Add(this.txtFromDate);
            this.gbIntervalExport.Controls.Add(this.btMakeDBF);
            this.gbIntervalExport.Location = new System.Drawing.Point(380, 170);
            this.gbIntervalExport.Name = "gbIntervalExport";
            this.gbIntervalExport.Size = new System.Drawing.Size(120, 84);
            this.gbIntervalExport.TabIndex = 14;
            this.gbIntervalExport.TabStop = false;
            this.gbIntervalExport.Text = "IntervalExport";
            // 
            // txtFromDate
            // 
            this.txtFromDate.Location = new System.Drawing.Point(4, 50);
            this.txtFromDate.Mask = "00.00.0000 90:00";
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(100, 20);
            this.txtFromDate.TabIndex = 1;
            this.txtFromDate.Text = " 12";
            // 
            // btMakeDBF
            // 
            this.btMakeDBF.Location = new System.Drawing.Point(6, 19);
            this.btMakeDBF.Name = "btMakeDBF";
            this.btMakeDBF.Size = new System.Drawing.Size(98, 23);
            this.btMakeDBF.TabIndex = 0;
            this.btMakeDBF.Text = "makeDBF";
            this.btMakeDBF.UseVisualStyleBackColor = true;
            this.btMakeDBF.Click += new System.EventHandler(this.MakeDBFbyInterval);
            // 
            // csvto5
            // 
            this.csvto5.AutoSize = true;
            this.csvto5.Location = new System.Drawing.Point(106, 67);
            this.csvto5.Name = "csvto5";
            this.csvto5.Size = new System.Drawing.Size(70, 17);
            this.csvto5.TabIndex = 13;
            this.csvto5.TabStop = true;
            this.csvto5.Text = "CSV за 5";
            this.csvto5.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 266);
            this.Controls.Add(this.gbIntervalExport);
            this.Controls.Add(this.btnHQLExp);
            this.Controls.Add(this.gbCSV);
            this.Controls.Add(this.btnDubliesToDBF);
            this.Controls.Add(this.btnDownloadTalons);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblmis);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.dbfpath);
            this.Controls.Add(this.btnDoDownloadPatients);
            this.Controls.Add(this.cmbMis);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(528, 305);
            this.MinimumSize = new System.Drawing.Size(528, 263);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.gbCSV.ResumeLayout(false);
            this.gbCSV.PerformLayout();
            this.gbIntervalExport.ResumeLayout(false);
            this.gbIntervalExport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMis;
        private System.Windows.Forms.Button btnDoDownloadPatients;
        private System.Windows.Forms.TextBox dbfpath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblmis;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnDownloadTalons;
        private System.Windows.Forms.Button btnDubliesToDBF;
        private System.Windows.Forms.Button btn_csv_export;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.RadioButton rbMonday;
        private System.Windows.Forms.RadioButton rbVoluntaryDay;
        private System.Windows.Forms.GroupBox gbCSV;
        private System.Windows.Forms.Button btnHQLExp;
        private System.Windows.Forms.GroupBox gbIntervalExport;
        private System.Windows.Forms.MaskedTextBox txtFromDate;
        private System.Windows.Forms.Button btMakeDBF;
        private System.Windows.Forms.RadioButton csvto2;
        private System.Windows.Forms.RadioButton csvto5;
    }
}

