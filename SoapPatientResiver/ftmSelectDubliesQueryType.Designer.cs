namespace SoapPatientResiver
{
    partial class ftmSelectDubliesQueryType
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
            this.btnMakeDBF = new System.Windows.Forms.Button();
            this.rblastName = new System.Windows.Forms.RadioButton();
            this.rblastNameBirth = new System.Windows.Forms.RadioButton();
            this.rb2NamesBirt = new System.Windows.Forms.RadioButton();
            this.rbPussportNumb = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnMakeDBF
            // 
            this.btnMakeDBF.Location = new System.Drawing.Point(105, 103);
            this.btnMakeDBF.Name = "btnMakeDBF";
            this.btnMakeDBF.Size = new System.Drawing.Size(75, 23);
            this.btnMakeDBF.TabIndex = 0;
            this.btnMakeDBF.Text = "Make DBF";
            this.btnMakeDBF.UseVisualStyleBackColor = true;
            this.btnMakeDBF.Click += new System.EventHandler(this.MakeDBFClick);
            // 
            // rblastName
            // 
            this.rblastName.AutoSize = true;
            this.rblastName.Checked = true;
            this.rblastName.Location = new System.Drawing.Point(17, 12);
            this.rblastName.Name = "rblastName";
            this.rblastName.Size = new System.Drawing.Size(83, 17);
            this.rblastName.TabIndex = 1;
            this.rblastName.TabStop = true;
            this.rblastName.Text = "by lastName";
            this.rblastName.UseVisualStyleBackColor = true;
            // 
            // rblastNameBirth
            // 
            this.rblastNameBirth.AutoSize = true;
            this.rblastNameBirth.Location = new System.Drawing.Point(17, 35);
            this.rblastNameBirth.Name = "rblastNameBirth";
            this.rblastNameBirth.Size = new System.Drawing.Size(126, 17);
            this.rblastNameBirth.TabIndex = 2;
            this.rblastNameBirth.TabStop = true;
            this.rblastNameBirth.Text = "by lastName, birthday";
            this.rblastNameBirth.UseVisualStyleBackColor = true;
            // 
            // rb2NamesBirt
            // 
            this.rb2NamesBirt.AutoSize = true;
            this.rb2NamesBirt.Location = new System.Drawing.Point(17, 58);
            this.rb2NamesBirt.Name = "rb2NamesBirt";
            this.rb2NamesBirt.Size = new System.Drawing.Size(142, 17);
            this.rb2NamesBirt.TabIndex = 3;
            this.rb2NamesBirt.TabStop = true;
            this.rb2NamesBirt.Text = "by any 2 names, birthday";
            this.rb2NamesBirt.UseVisualStyleBackColor = true;
            // 
            // rbPussportNumb
            // 
            this.rbPussportNumb.AutoSize = true;
            this.rbPussportNumb.Location = new System.Drawing.Point(17, 82);
            this.rbPussportNumb.Name = "rbPussportNumb";
            this.rbPussportNumb.Size = new System.Drawing.Size(117, 17);
            this.rbPussportNumb.TabIndex = 4;
            this.rbPussportNumb.TabStop = true;
            this.rbPussportNumb.Text = "by passport number";
            this.rbPussportNumb.UseVisualStyleBackColor = true;
            // 
            // ftmSelectDubliesQueryType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 144);
            this.Controls.Add(this.rbPussportNumb);
            this.Controls.Add(this.rb2NamesBirt);
            this.Controls.Add(this.rblastNameBirth);
            this.Controls.Add(this.rblastName);
            this.Controls.Add(this.btnMakeDBF);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ftmSelectDubliesQueryType";
            this.Text = "ftmSelectDubliesQueryType";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMakeDBF;
        private System.Windows.Forms.RadioButton rblastName;
        private System.Windows.Forms.RadioButton rblastNameBirth;
        private System.Windows.Forms.RadioButton rb2NamesBirt;
        private System.Windows.Forms.RadioButton rbPussportNumb;
    }
}