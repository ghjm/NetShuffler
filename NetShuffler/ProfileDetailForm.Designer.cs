namespace NetShuffler
{
    partial class ProfileDetailForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMapDrive = new System.Windows.Forms.Button();
            this.btnSetPrinter = new System.Windows.Forms.Button();
            this.btnRunProgram = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnConnectVPN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 35);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(477, 134);
            this.listBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Actions";
            // 
            // btnMapDrive
            // 
            this.btnMapDrive.Location = new System.Drawing.Point(23, 175);
            this.btnMapDrive.Name = "btnMapDrive";
            this.btnMapDrive.Size = new System.Drawing.Size(80, 23);
            this.btnMapDrive.TabIndex = 2;
            this.btnMapDrive.Text = "Map Drive";
            this.btnMapDrive.UseVisualStyleBackColor = true;
            this.btnMapDrive.Click += new System.EventHandler(this.btnMapDrive_Click);
            // 
            // btnSetPrinter
            // 
            this.btnSetPrinter.Location = new System.Drawing.Point(109, 175);
            this.btnSetPrinter.Name = "btnSetPrinter";
            this.btnSetPrinter.Size = new System.Drawing.Size(80, 23);
            this.btnSetPrinter.TabIndex = 3;
            this.btnSetPrinter.Text = "Set Printer";
            this.btnSetPrinter.UseVisualStyleBackColor = true;
            this.btnSetPrinter.Click += new System.EventHandler(this.btnSetPrinter_Click);
            // 
            // btnRunProgram
            // 
            this.btnRunProgram.Location = new System.Drawing.Point(195, 175);
            this.btnRunProgram.Name = "btnRunProgram";
            this.btnRunProgram.Size = new System.Drawing.Size(80, 23);
            this.btnRunProgram.TabIndex = 4;
            this.btnRunProgram.Text = "Run Program";
            this.btnRunProgram.UseVisualStyleBackColor = true;
            this.btnRunProgram.Click += new System.EventHandler(this.btnRunProgram_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(250, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(331, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(404, 175);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnConnectVPN
            // 
            this.btnConnectVPN.Location = new System.Drawing.Point(281, 175);
            this.btnConnectVPN.Name = "btnConnectVPN";
            this.btnConnectVPN.Size = new System.Drawing.Size(80, 23);
            this.btnConnectVPN.TabIndex = 9;
            this.btnConnectVPN.Text = "Connect VPN";
            this.btnConnectVPN.UseVisualStyleBackColor = true;
            this.btnConnectVPN.Click += new System.EventHandler(this.btnConnectVPN_Click);
            // 
            // ProfileDetailForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(501, 263);
            this.Controls.Add(this.btnConnectVPN);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRunProgram);
            this.Controls.Add(this.btnSetPrinter);
            this.Controls.Add(this.btnMapDrive);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileDetailForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profile Detail";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMapDrive;
        private System.Windows.Forms.Button btnSetPrinter;
        private System.Windows.Forms.Button btnRunProgram;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnConnectVPN;
    }
}