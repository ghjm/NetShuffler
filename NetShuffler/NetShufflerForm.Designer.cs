namespace NetShuffler
{
    partial class NetShufflerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetShufflerForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TaskTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TaskTrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblNetworks = new System.Windows.Forms.Label();
            this.dgNetworks = new System.Windows.Forms.DataGridView();
            this.Adapter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GUID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP_Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subnet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WiFi_SSID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DNS_Suffix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NLA_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NLA_Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Internet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetworkMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreThisAdapterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreAllIgnoredAdaptersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbxIPV6 = new System.Windows.Forms.CheckBox();
            this.dgProfiles = new System.Windows.Forms.DataGridView();
            this.Active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfileMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbxProfiles = new System.Windows.Forms.CheckBox();
            this.lblProfiles = new System.Windows.Forms.Label();
            this.startWithWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recognizeByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfigProfilesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ConfigBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TaskTrayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgNetworks)).BeginInit();
            this.NetworkMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProfiles)).BeginInit();
            this.ProfileMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConfigProfilesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConfigBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TaskTrayIcon
            // 
            this.TaskTrayIcon.ContextMenuStrip = this.TaskTrayMenu;
            this.TaskTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TaskTrayIcon.Icon")));
            this.TaskTrayIcon.Text = "NetShuffler";
            this.TaskTrayIcon.Visible = true;
            this.TaskTrayIcon.DoubleClick += new System.EventHandler(this.TaskTrayIcon_DoubleClick);
            // 
            // TaskTrayMenu
            // 
            this.TaskTrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.startWithWindowsToolStripMenuItem,
            this.toolStripMenuItem1});
            this.TaskTrayMenu.Name = "TaskTrayMenu";
            this.TaskTrayMenu.Size = new System.Drawing.Size(177, 92);
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.configureToolStripMenuItem.Text = "&Configure...";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(176, 22);
            this.toolStripMenuItem1.Text = "E&xit";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblNetworks
            // 
            this.lblNetworks.AutoSize = true;
            this.lblNetworks.Location = new System.Drawing.Point(13, 12);
            this.lblNetworks.Name = "lblNetworks";
            this.lblNetworks.Size = new System.Drawing.Size(85, 13);
            this.lblNetworks.TabIndex = 0;
            this.lblNetworks.Text = "Active Networks";
            // 
            // dgNetworks
            // 
            this.dgNetworks.AllowUserToAddRows = false;
            this.dgNetworks.AllowUserToDeleteRows = false;
            this.dgNetworks.AllowUserToOrderColumns = true;
            this.dgNetworks.AllowUserToResizeRows = false;
            this.dgNetworks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgNetworks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Adapter,
            this.GUID,
            this.IP_Address,
            this.Subnet,
            this.WiFi_SSID,
            this.DNS_Suffix,
            this.NLA_Description,
            this.NLA_Category,
            this.Internet});
            this.dgNetworks.ContextMenuStrip = this.NetworkMenu;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgNetworks.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgNetworks.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgNetworks.Location = new System.Drawing.Point(16, 29);
            this.dgNetworks.MultiSelect = false;
            this.dgNetworks.Name = "dgNetworks";
            this.dgNetworks.ReadOnly = true;
            this.dgNetworks.RowHeadersVisible = false;
            this.dgNetworks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgNetworks.Size = new System.Drawing.Size(783, 126);
            this.dgNetworks.StandardTab = true;
            this.dgNetworks.TabIndex = 1;
            this.dgNetworks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgNetworks_CellDoubleClick);
            this.dgNetworks.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgNetworks_CellMouseDown);
            this.dgNetworks.Enter += new System.EventHandler(this.DataGrid_Enter);
            this.dgNetworks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGrid_KeyDown);
            this.dgNetworks.Leave += new System.EventHandler(this.DataGrid_Leave);
            this.dgNetworks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgNetworks_MouseClick);
            // 
            // Adapter
            // 
            this.Adapter.HeaderText = "Adapter";
            this.Adapter.Name = "Adapter";
            this.Adapter.ReadOnly = true;
            this.Adapter.Width = 200;
            // 
            // GUID
            // 
            this.GUID.HeaderText = "GUID";
            this.GUID.Name = "GUID";
            this.GUID.ReadOnly = true;
            this.GUID.Visible = false;
            // 
            // IP_Address
            // 
            this.IP_Address.HeaderText = "IP Address";
            this.IP_Address.Name = "IP_Address";
            this.IP_Address.ReadOnly = true;
            this.IP_Address.Width = 90;
            // 
            // Subnet
            // 
            this.Subnet.HeaderText = "Subnet";
            this.Subnet.Name = "Subnet";
            this.Subnet.ReadOnly = true;
            this.Subnet.Width = 110;
            // 
            // WiFi_SSID
            // 
            this.WiFi_SSID.HeaderText = "WiFi SSID";
            this.WiFi_SSID.Name = "WiFi_SSID";
            this.WiFi_SSID.ReadOnly = true;
            this.WiFi_SSID.Width = 80;
            // 
            // DNS_Suffix
            // 
            this.DNS_Suffix.HeaderText = "DNS Suffix";
            this.DNS_Suffix.Name = "DNS_Suffix";
            this.DNS_Suffix.ReadOnly = true;
            this.DNS_Suffix.Width = 80;
            // 
            // NLA_Description
            // 
            this.NLA_Description.HeaderText = "NLA Description";
            this.NLA_Description.Name = "NLA_Description";
            this.NLA_Description.ReadOnly = true;
            this.NLA_Description.Width = 80;
            // 
            // NLA_Category
            // 
            this.NLA_Category.HeaderText = "NLA Category";
            this.NLA_Category.Name = "NLA_Category";
            this.NLA_Category.ReadOnly = true;
            this.NLA_Category.Width = 60;
            // 
            // Internet
            // 
            this.Internet.HeaderText = "Internet Status";
            this.Internet.Name = "Internet";
            this.Internet.ReadOnly = true;
            this.Internet.Width = 60;
            // 
            // NetworkMenu
            // 
            this.NetworkMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProfileToolStripMenuItem,
            this.ignoreThisAdapterToolStripMenuItem,
            this.restoreAllIgnoredAdaptersToolStripMenuItem});
            this.NetworkMenu.Name = "NetworkMenu";
            this.NetworkMenu.Size = new System.Drawing.Size(225, 70);
            this.NetworkMenu.Opening += new System.ComponentModel.CancelEventHandler(this.NetworkMenu_Opening);
            // 
            // addProfileToolStripMenuItem
            // 
            this.addProfileToolStripMenuItem.Name = "addProfileToolStripMenuItem";
            this.addProfileToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.addProfileToolStripMenuItem.Text = "Add Profile";
            this.addProfileToolStripMenuItem.Click += new System.EventHandler(this.addProfileToolStripMenuItem_Click);
            // 
            // ignoreThisAdapterToolStripMenuItem
            // 
            this.ignoreThisAdapterToolStripMenuItem.Name = "ignoreThisAdapterToolStripMenuItem";
            this.ignoreThisAdapterToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.ignoreThisAdapterToolStripMenuItem.Text = "Ignore This Adapter";
            this.ignoreThisAdapterToolStripMenuItem.Click += new System.EventHandler(this.ignoreThisAdapterToolStripMenuItem_Click);
            // 
            // restoreAllIgnoredAdaptersToolStripMenuItem
            // 
            this.restoreAllIgnoredAdaptersToolStripMenuItem.Name = "restoreAllIgnoredAdaptersToolStripMenuItem";
            this.restoreAllIgnoredAdaptersToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.restoreAllIgnoredAdaptersToolStripMenuItem.Text = "Restore All Ignored Adapters";
            this.restoreAllIgnoredAdaptersToolStripMenuItem.Click += new System.EventHandler(this.restoreAllIgnoredAdaptersToolStripMenuItem_Click);
            // 
            // cbxIPV6
            // 
            this.cbxIPV6.AutoSize = true;
            this.cbxIPV6.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.ConfigBindingSource, "IPV6Enabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbxIPV6.Location = new System.Drawing.Point(16, 161);
            this.cbxIPV6.Name = "cbxIPV6";
            this.cbxIPV6.Size = new System.Drawing.Size(133, 17);
            this.cbxIPV6.TabIndex = 2;
            this.cbxIPV6.Text = "Enable IPV6 Networks";
            this.cbxIPV6.UseVisualStyleBackColor = true;
            this.cbxIPV6.CheckedChanged += new System.EventHandler(this.cbxIPV6_CheckedChanged);
            // 
            // dgProfiles
            // 
            this.dgProfiles.AllowUserToAddRows = false;
            this.dgProfiles.AllowUserToDeleteRows = false;
            this.dgProfiles.AllowUserToOrderColumns = true;
            this.dgProfiles.AllowUserToResizeRows = false;
            this.dgProfiles.AutoGenerateColumns = false;
            this.dgProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProfiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.recognizeByDataGridViewTextBoxColumn,
            this.Active});
            this.dgProfiles.ContextMenuStrip = this.ProfileMenu;
            this.dgProfiles.DataSource = this.ConfigProfilesBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgProfiles.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgProfiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgProfiles.Location = new System.Drawing.Point(16, 209);
            this.dgProfiles.MultiSelect = false;
            this.dgProfiles.Name = "dgProfiles";
            this.dgProfiles.ReadOnly = true;
            this.dgProfiles.RowHeadersVisible = false;
            this.dgProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgProfiles.Size = new System.Drawing.Size(422, 126);
            this.dgProfiles.StandardTab = true;
            this.dgProfiles.TabIndex = 4;
            this.dgProfiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProfiles_CellDoubleClick);
            this.dgProfiles.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgProfiles_CellMouseDown);
            this.dgProfiles.Enter += new System.EventHandler(this.DataGrid_Enter);
            this.dgProfiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGrid_KeyDown);
            this.dgProfiles.Leave += new System.EventHandler(this.DataGrid_Leave);
            this.dgProfiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgProfiles_MouseClick);
            // 
            // Active
            // 
            this.Active.DataPropertyName = "Active";
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.ReadOnly = true;
            // 
            // ProfileMenu
            // 
            this.ProfileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editProfileToolStripMenuItem,
            this.deleteProfileToolStripMenuItem,
            this.executeProfileToolStripMenuItem});
            this.ProfileMenu.Name = "ProfileMenu";
            this.ProfileMenu.Size = new System.Drawing.Size(176, 70);
            this.ProfileMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ProfileMenu_Opening);
            // 
            // editProfileToolStripMenuItem
            // 
            this.editProfileToolStripMenuItem.Name = "editProfileToolStripMenuItem";
            this.editProfileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.editProfileToolStripMenuItem.Text = "Edit Profile";
            this.editProfileToolStripMenuItem.Click += new System.EventHandler(this.editProfileToolStripMenuItem_Click);
            // 
            // deleteProfileToolStripMenuItem
            // 
            this.deleteProfileToolStripMenuItem.Name = "deleteProfileToolStripMenuItem";
            this.deleteProfileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.deleteProfileToolStripMenuItem.Text = "Remove Profile";
            this.deleteProfileToolStripMenuItem.Click += new System.EventHandler(this.deleteProfileToolStripMenuItem_Click);
            // 
            // executeProfileToolStripMenuItem
            // 
            this.executeProfileToolStripMenuItem.Name = "executeProfileToolStripMenuItem";
            this.executeProfileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.executeProfileToolStripMenuItem.Text = "Run Profile Actions";
            this.executeProfileToolStripMenuItem.Click += new System.EventHandler(this.executeProfileToolStripMenuItem_Click);
            // 
            // cbxProfiles
            // 
            this.cbxProfiles.AutoSize = true;
            this.cbxProfiles.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.ConfigBindingSource, "ShowInactiveProfiles", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbxProfiles.Location = new System.Drawing.Point(16, 341);
            this.cbxProfiles.Name = "cbxProfiles";
            this.cbxProfiles.Size = new System.Drawing.Size(131, 17);
            this.cbxProfiles.TabIndex = 5;
            this.cbxProfiles.Text = "Show Inactive Profiles";
            this.cbxProfiles.UseVisualStyleBackColor = true;
            this.cbxProfiles.CheckedChanged += new System.EventHandler(this.cbxProfiles_CheckedChanged);
            // 
            // lblProfiles
            // 
            this.lblProfiles.AutoSize = true;
            this.lblProfiles.Location = new System.Drawing.Point(13, 192);
            this.lblProfiles.Name = "lblProfiles";
            this.lblProfiles.Size = new System.Drawing.Size(74, 13);
            this.lblProfiles.TabIndex = 3;
            this.lblProfiles.Text = "Active Profiles";
            // 
            // startWithWindowsToolStripMenuItem
            // 
            this.startWithWindowsToolStripMenuItem.Checked = true;
            this.startWithWindowsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startWithWindowsToolStripMenuItem.Name = "startWithWindowsToolStripMenuItem";
            this.startWithWindowsToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.startWithWindowsToolStripMenuItem.Text = "&Start with Windows";
            this.startWithWindowsToolStripMenuItem.Click += new System.EventHandler(this.startWithWindowsToolStripMenuItem_Click);
            // 
            // recognizeByDataGridViewTextBoxColumn
            // 
            this.recognizeByDataGridViewTextBoxColumn.DataPropertyName = "RecognizeBy";
            this.recognizeByDataGridViewTextBoxColumn.HeaderText = "Recognize By";
            this.recognizeByDataGridViewTextBoxColumn.Name = "recognizeByDataGridViewTextBoxColumn";
            this.recognizeByDataGridViewTextBoxColumn.ReadOnly = true;
            this.recognizeByDataGridViewTextBoxColumn.Width = 200;
            // 
            // ConfigProfilesBindingSource
            // 
            this.ConfigProfilesBindingSource.AllowNew = false;
            this.ConfigProfilesBindingSource.DataMember = "Profiles";
            this.ConfigProfilesBindingSource.DataSource = typeof(NetShuffler.NetShufflerForm);
            // 
            // ConfigBindingSource
            // 
            this.ConfigBindingSource.DataSource = typeof(NetShuffler.AppConfig);
            this.ConfigBindingSource.Filter = "Active = True";
            // 
            // NetShufflerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 388);
            this.Controls.Add(this.dgProfiles);
            this.Controls.Add(this.cbxProfiles);
            this.Controls.Add(this.lblProfiles);
            this.Controls.Add(this.dgNetworks);
            this.Controls.Add(this.cbxIPV6);
            this.Controls.Add(this.lblNetworks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "NetShufflerForm";
            this.ShowInTaskbar = false;
            this.Text = "NetShuffler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetShufflerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NetShufflerForm_FormClosed);
            this.Load += new System.EventHandler(this.NetShufflerForm_Load);
            this.Shown += new System.EventHandler(this.NetShufflerForm_Shown);
            this.SizeChanged += new System.EventHandler(this.NetShufflerForm_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NetShufflerForm_KeyDown);
            this.TaskTrayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgNetworks)).EndInit();
            this.NetworkMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgProfiles)).EndInit();
            this.ProfileMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConfigProfilesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConfigBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon TaskTrayIcon;
        private System.Windows.Forms.ContextMenuStrip TaskTrayMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblNetworks;
        private System.Windows.Forms.DataGridView dgNetworks;
        private System.Windows.Forms.CheckBox cbxIPV6;
        private System.Windows.Forms.ContextMenuStrip NetworkMenu;
        private System.Windows.Forms.ToolStripMenuItem ignoreThisAdapterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreAllIgnoredAdaptersToolStripMenuItem;
        private System.Windows.Forms.DataGridView dgProfiles;
        private System.Windows.Forms.CheckBox cbxProfiles;
        private System.Windows.Forms.Label lblProfiles;
        private System.Windows.Forms.ToolStripMenuItem addProfileToolStripMenuItem;
        private System.Windows.Forms.BindingSource ConfigBindingSource;
        private System.Windows.Forms.BindingSource ConfigProfilesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Adapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn GUID;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP_Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subnet;
        private System.Windows.Forms.DataGridViewTextBoxColumn WiFi_SSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DNS_Suffix;
        private System.Windows.Forms.DataGridViewTextBoxColumn NLA_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn NLA_Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Internet;
        private System.Windows.Forms.DataGridViewTextBoxColumn recognizeByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Active;
        private System.Windows.Forms.ContextMenuStrip ProfileMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startWithWindowsToolStripMenuItem;
    }
}

