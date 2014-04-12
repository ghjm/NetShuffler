using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Xml.Serialization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using NETWORKLIST;

namespace NetShuffler
{
    public partial class NetShufflerForm : Form
    {

        // Application-level configuration
        private AppConfig _config = AppConfig.Load();
        public AppConfig config
        {
            get { return _config; }
        }

        // List of defined network profiles
        public BindingList<ProfileData> Profiles
        {
            get { return config.Profiles; }
        }

        // List of currently-active network profiles
        private BindingList<ProfileData> _ActiveProfiles = new BindingList<ProfileData>();
        public BindingList<ProfileData> ActiveProfiles
        {
            get { return _ActiveProfiles; }
        }

        // NetworkListManager is a .NET class for accessing Windows Network Location Awareness (NLA).
        NetworkListManager nlm = null;

        // Maintain the Win32 handle to the WiFi subsystem globally, to avoid triggering driver 
        // handle leaks by repeatedly opening/closing it.
        IntPtr wifi_handle = IntPtr.Zero;

        // We set this true whenever any network conditions have changed. We use multiple methods
        // for detecting network changes, but we don't want to re-enumerate the network list multiple
        // times if multiple methods each detected the change. So the change detectors just set this
        // to true, and a system timer periodically checks and re-enumerates if the flag is set.
        static bool netchanged = false;

        // Hack to avoid highlighting any row of the data grid until actually clicked.
        // When this is true, the grid should not show any selected rows.
        static bool suppress_grid_enter = true;

        public NetShufflerForm()
        {
            InitializeComponent();

            // Check whether start-on-login is set
            startWithWindowsToolStripMenuItem.Checked = StartAtLoginConfig.CheckStartupConfig();

            // Bind data grid data source
            ConfigBindingSource.DataSource = config;
            dgProfiles.DataSource = config.Profiles;
            cbxProfiles_CheckedChanged(this, null);

            // Open a Win32 handle to the WiFi subsystem.
            uint wifi_ver;
            if (Win32API.WlanOpenHandle((uint)2, IntPtr.Zero, out wifi_ver, out wifi_handle) != 0)
                Win32API.ThrowException();

            // Set up detection of network stauts changes using .NET.

            // Method #1: NetworkListManager
            nlm = new NetworkListManager();
            try
            {
                nlm.NetworkAdded += new INetworkEvents_NetworkAddedEventHandler(OnNetworkAdded);
                nlm.NetworkConnectivityChanged += new INetworkEvents_NetworkConnectivityChangedEventHandler(OnNetworkConnectivityChanged);
                nlm.NetworkDeleted += new INetworkEvents_NetworkDeletedEventHandler(OnNetworkDeleted);
                nlm.NetworkPropertyChanged += new INetworkEvents_NetworkPropertyChangedEventHandler(OnNetworkPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Method #2: System.Net.NetworkInformation.NetworkChange
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(OnNetworkChange);

            // Initially populate the active network list.
            UpdateNetworkList();
        }


        private void NetShufflerForm_Load(object sender, EventArgs e)
        {
            // Retrieve a Win32 WINDOWPLACEMENT struct with details of this window.
            var wp = Win32API.WINDOWPLACEMENT.Default;
            if (!Win32API.GetWindowPlacement(this.Handle, out wp))
                Win32API.ThrowException();

            // Set the Win32 minimize position so that our minimization animation will
            // be aimed towards the toolbar tray area.
            // Todo: Interrogate Windows Explorer to find the actual location of the
            // toolbar tray. (We assume it is in the default location at the bottom right,
            // but it's possible that the user may have moved it.)
            var ScreenBounds = Screen.GetWorkingArea(this);
            wp.Flags = Win32API.WPF_SETMINPOSITION;
            wp.MinPosition.X = ScreenBounds.Width - 400;
            wp.MinPosition.Y = ScreenBounds.Height;
            if (!Win32API.SetWindowPlacement(this.Handle, ref wp))
                Win32API.ThrowException();
        }

        void OnNetworkPropertyChanged(Guid networkId, NLM_NETWORK_PROPERTY_CHANGE Flags)
        {
            // Just flag that something has changed. Changes will be enumerated
            // when timer1 ticks.
            netchanged = true;
        }

        void OnNetworkConnectivityChanged(Guid networkId, NLM_CONNECTIVITY newConnectivity)
        {
            // Just flag that something has changed. Changes will be enumerated
            // when timer1 ticks.
            netchanged = true;
        }

        void OnNetworkAdded(Guid networkId)
        {
            // Just flag that something has changed. Changes will be enumerated
            // when timer1 ticks.
            netchanged = true;
        }

        void OnNetworkDeleted(Guid networkId)
        {
            // Just flag that something has changed. Changes will be enumerated
            // when timer1 ticks.
            netchanged = true;
        }
        
        private static void OnNetworkChange(object sender, EventArgs e)
        {
            // Just flag that something has changed. Changes will be enumerated
            // when timer1 ticks.
            netchanged = true;
        }

        static int changed_ticks = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (netchanged)
            {
                // Something has changed somewhere, so we want to re-enumerate network connection status.
                // But we don't necessarily want to re-enumerate immediately, because multiple facilities
                // will tell us about the network change. So we wait for four timer ticks (2 seconds at
                // 500ms per tick) before actually doing the enumeration.
                changed_ticks++;
                if (changed_ticks >= 4)
                {
                    netchanged = false;
                    changed_ticks = 0;
                    UpdateNetworkList();
                }
            }
        }

        // Retrieve available information for a given network id (nid) from the .NET NetworkListManager.
        private void GetNLMInfo(Guid nid, DataGridViewRow row)
        {
            INetwork nlm_net = null;
            foreach (INetworkConnection nc in nlm.GetNetworkConnections())
            {
                if (nc.GetAdapterId().Equals(nid))
                {
                    nlm_net = nc.GetNetwork();
                    break; // foreach
                }
            }
            // If we didn't find any information, these fields in the grid
            // will just remain empty.
            if (nlm_net != null)
            {
                // Fill in the appropriate cells in the data grid based on
                // information from the NetworkListManager.
                row.Cells["NLA_Description"].Value = nlm_net.GetDescription();
                switch (nlm_net.GetCategory())
                {
                    case NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PUBLIC:
                        row.Cells["NLA_Category"].Value = "Public";
                        break;
                    case NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PRIVATE:
                        row.Cells["NLA_Category"].Value = "Private";
                        break;
                    case NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_DOMAIN_AUTHENTICATED:
                        row.Cells["NLA_Category"].Value = "Domain";
                        break;
                }
                if (nlm_net.IsConnectedToInternet)
                    row.Cells["Internet"].Value = "Connected";
            }
        }

        // Retrieve available information for a given network id (nid) from the Win32 WiFi subsystem.
        private void GetWifiInfo(Guid nid, DataGridViewRow row)
        {
            // Get the list of WiFi network interfaces known to Win32.
            IntPtr ptr = new IntPtr();
            if (Win32API.WlanEnumInterfaces(wifi_handle, IntPtr.Zero, ref ptr) != 0)
                Win32API.ThrowException();

            // Free temporary memory while preserving the list itself.
            Win32API.WLAN_INTERFACE_INFO_LIST infoList = new Win32API.WLAN_INTERFACE_INFO_LIST(ptr);
            Win32API.WlanFreeMemory(ptr);

            // Search for the desired network id (nid) in the list of returned networks.
            for (int i = 0; i < infoList.dwNumberOfItems; i++)
            {
                if (infoList.InterfaceInfo[i].InterfaceGuid.Equals(nid))
                {
                    // We found the desired network, so interrogate Win32 for its WiFi information.
                    uint DataSize;
                    if (Win32API.WlanQueryInterface(wifi_handle, ref nid,
                        Win32API.WLAN_INTF_OPCODE.wlan_intf_opcode_current_connection, IntPtr.Zero,
                        out DataSize, ref ptr, IntPtr.Zero) != 0)
                        Win32API.ThrowException();
                    var wifi_conn = (Win32API.WLAN_CONNECTION_ATTRIBUTES)
                        System.Runtime.InteropServices.Marshal.PtrToStructure(ptr, typeof(Win32API.WLAN_CONNECTION_ATTRIBUTES));
                    row.Cells["WiFi_SSID"].Value = wifi_conn.wlanAssociationAttributes.dot11Ssid.ucSSID;
                    Win32API.WlanFreeMemory(ptr);
                }
            }
        }

        // APIPA is the system for assigning unique addresses to adapters configured for DHCP
        // which do not receive a reply from a DHCP server. All such addresses will be within
        // the below range, and the below range is supposed to be used only for such addresses.
        private IPAddress APIPA_Network = IPAddress.Parse("169.254.0.0");
        private IPAddress ClassB = IPAddress.Parse("255.255.0.0");

        // Retrieve all available network information for a specified interface.
        private void GetNetworkInfo(NetworkInterface ni)
        {
            // Check whether this interface is in the user-specified ignore list.
            bool ignore = false;
            foreach (string a in config.AdapterIgnoreList)
                if (ni.Id == a)
                    ignore = true;
            // Check whether this interface should be examined.
            if ((ni.OperationalStatus == OperationalStatus.Up) &&
                (ni.NetworkInterfaceType != NetworkInterfaceType.Loopback) &&
                (!ignore))
            {
                // Get the network id (nid) for the interface.
                var nid = Guid.Parse(ni.Id);
                // Get the list of IP addresses for the interface.
                var ipp = ni.GetIPProperties();
                foreach (var addr in ipp.UnicastAddresses)
                {
                    if ((addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) &&
                        (addr.Address.IsInSameSubnet(APIPA_Network, ClassB)))
                    {
                        // This is a network adapter configured for DHCP that hasn't received an address - ignore.
                        // (We have to ignore it so cached DNS names don't trigger actions on the wrong network.)
                        break;
                    }
                    // Check if this is an IPV4 address OR we are allowed to look at IPV6 addresses.
                    if ((addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) || (config.IPV6Enabled))
                    {
                        // Fill in the basic information for this network interface.
                        var row = dgNetworks.Rows[dgNetworks.Rows.Add()];
                        row.Cells["Adapter"].Value = ni.Description;
                        row.Cells["GUID"].Value = ni.Id;
                        row.Cells["DNS_Suffix"].Value = ipp.DnsSuffix;
                        row.Cells["IP_Address"].Value = addr.Address.ToString();
                        if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            row.Cells["Subnet"].Value =
                            addr.Address.NetworkAddress(addr.IPv4Mask).ToString() + "/" +
                            addr.IPv4Mask.NetmaskToBits().ToString();

                        // Get additional information, if any, available from the NetworkListManager.
                        try
                        {
                            GetNLMInfo(nid, row);
                        }
                        catch
                        {
                            // On exception, we'll just do without the NLM data.
                        }

                        // Get additional information, if any, available from the Win32 WiFi subsystem.
                        try
                        {
                            GetWifiInfo(nid, row);
                        }
                        catch
                        {
                            // On exception, we'll just do without the WiFi data.
                        }

                    }
                }

            }
        }

        // Main method to re-enumerate all network information.
        private void UpdateNetworkList()
        {
            // Suspend drawing so we don't repeatedly update the GUI while changing bound data values.
            dgNetworks.SuspendDrawing();
            try
            {
                // Rather than try to compare each row to the current status, we just
                // re-build the network list each time.
                dgNetworks.Rows.Clear();
                // Initially populate the list with all known network interfaces.
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                // Add values to informational colums, where possible.
                foreach (var ni in adapters)
                    GetNetworkInfo(ni);
                // Re-check each profile to see if it has become active/inactive based on the
                // newly available network information.
                UpdateProfileStatus();
            }
            finally
            {
                // Resume drawing - the changes we've made will now appear to the user.
                dgNetworks.ResumeDrawing();
            }
        }

        // Compare two strings, returning false if either of them is null.
        // If both strings are null, the comparison should fail.
        private bool NullSafeCompare(object a, string b, bool CaseInsensitive = false)
        {
            if ((a == null) || (b == null))
                return false;
            if (CaseInsensitive)
                return (a.ToString().ToLower() == b.ToLower());
            else
                return (a.ToString() == b);
        }

        // Updates the active/inactive status of all configured profiles, based on current network status.
        private void UpdateProfileStatus(bool force = false)
        {
            // Track whether anything changed, to avoid unnecessary GUI updates when nothing has changed.
            bool anything_changed = false;

            // Search all profiles
            foreach (var p in Profiles)
            {
                // Search all network interfaces
                bool found = false;
                for (int i = 0; (!found) && (i < dgNetworks.RowCount); i++)
                {
                    // Check whether the current network interface matches the configured
                    // recognition method for the profile.
                    switch (p.RecMethod)
                    {
                        case RecognizerMethod.DNS_Suffix:
                            if (NullSafeCompare(dgNetworks.Rows[i].Cells["DNS_Suffix"].Value, p.RecData, CaseInsensitive: true))
                                found = true;
                            break;
                        case RecognizerMethod.WiFi_SSID:
                            if (NullSafeCompare(dgNetworks.Rows[i].Cells["WiFi_SSID"].Value, p.RecData))
                                found = true;
                            break;
                        case RecognizerMethod.Subnet:
                            string[] tokens = p.RecData.Split(new[] { "/" }, StringSplitOptions.None);
                            if ((tokens.Length == 2) && (dgNetworks.Rows[i].Cells["IP_Address"].Value != null))
                            {
                                // Check if the interface's IP address is within a given CIDR subnet
                                IPAddress aip = IPAddress.Parse(dgNetworks.Rows[i].Cells["IP_Address"].Value.ToString());
                                if (aip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    IPAddress pip = IPAddress.Parse(tokens[0]);
                                    IPAddress mask = pip.BitsToNetmask(Convert.ToInt32(tokens[1]));
                                    if (pip.IsInSameSubnet(aip, mask))
                                        found = true;
                                }
                            }
                            break;
                        case RecognizerMethod.NLA_Description:
                            if (NullSafeCompare(dgNetworks.Rows[i].Cells["NLA_Description"].Value, p.RecData))
                                found = true;
                            break;
                        case RecognizerMethod.NLA_Category:
                            if (NullSafeCompare(dgNetworks.Rows[i].Cells["NLA_Category"].Value, p.RecData))
                                found = true;
                            break;
                    }
                }
                // "found" is true if and only if p.Active should now become true.
                if (p.Active != found)
                {
                    anything_changed = true;
                    p.Active = found;
                    // If we got here and p.Active is now true, then this was a false-to-true transition,
                    // and profile actions should now be run.
                    if (p.Active)
                        p.RunActions();
                }
            }
            // Only if anything changed, regenerate the active profiles list and re-draw the list in the GUI.
            if (anything_changed || force)
            {
                dgProfiles.SuspendDrawing();
                ActiveProfiles.Clear();
                foreach (var p in Profiles)
                    if (p.Active)
                        ActiveProfiles.Add(p);
                dgProfiles.ResumeDrawing();
            }
        }

        // Make main window visible, even if currently minimized to tray.
        public void ShowMainWindow()
        {
            if (!this.Visible)
                this.Show();
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            this.Focus();
        }

        // Double clicking on the taskbar icon should bring up the main form.
        private void TaskTrayIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        // The user can also bring up the main form through the right-click context menu.
        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        // If the user changes whether or not IPV6 should be looked at, we need to re-detect the network list.
        private void cbxIPV6_CheckedChanged(object sender, EventArgs e)
        {
            netchanged = true;
        }

        // User chose to ignore an adapter. Run a re-detect, while ignoring this adapter.
        private void ignoreThisAdapterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config.AdapterIgnoreList.Add(dgNetworks.CurrentRow.Cells["GUID"].Value.ToString());
            netchanged = true;
        }

        // Allow the user to regain visibility of network adapters they previously chose to ignore.
        private void restoreAllIgnoredAdaptersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config.AdapterIgnoreList.Clear();
            netchanged = true;
        }

        // Show all or only active profiles based on user selection of a checkbox.
        private void cbxProfiles_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxProfiles.Checked)
            {
                lblProfiles.Text = "Profiles";
                dgProfiles.DataSource = Profiles;
                dgProfiles.Columns["Active"].Visible = true;
            }
            else
            {
                lblProfiles.Text = "Active Profiles";
                dgProfiles.DataSource = ActiveProfiles;
                dgProfiles.Columns["Active"].Visible = false;
            }
        }

        // Determine whether an object has a non-empty value.
        private bool HasValue(object Value)
        {
            if (Value == null)
                return false;
            if (Value.ToString().Length == 0)
                return false;
            return true;
        }

        // User requested to add a new profile.
        private void DoAddProfile(int RowIndex)
        {
            // Create an AddProfileForm
            var apf = new AddProfileForm();
            // Populate the detection method drop-down on the AddProfileForm
            var row = dgNetworks.Rows[RowIndex];
            if (HasValue(row.Cells["DNS_Suffix"].Value))
                apf.cbRecMethod.Items.Add("DNS Suffix = " + row.Cells["DNS_Suffix"].Value);
            if (HasValue(row.Cells["WiFi_SSID"].Value))
                apf.cbRecMethod.Items.Add("WiFi SSID = " + row.Cells["WiFi_SSID"].Value);
            if (HasValue(row.Cells["Subnet"].Value))
                apf.cbRecMethod.Items.Add("Subnet = " + row.Cells["Subnet"].Value);
            if (HasValue(row.Cells["NLA_Description"].Value))
                apf.cbRecMethod.Items.Add("NLA Description = " + row.Cells["NLA_Description"].Value);
            if (HasValue(row.Cells["NLA_Category"].Value))
                apf.cbRecMethod.Items.Add("NLA Category = " + row.Cells["NLA_Category"].Value);
            apf.cbRecMethod.SelectedIndex = 0;
            // Show the form
            if (apf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // If the user clicked OK, then actually create the profile based on their choices.
                var pd = new ProfileData();
                if (apf.cbRecMethod.Text.StartsWith("DNS Suffix"))
                    pd.RecMethod = RecognizerMethod.DNS_Suffix;
                else if (apf.cbRecMethod.Text.StartsWith("WiFi SSID"))
                    pd.RecMethod = RecognizerMethod.WiFi_SSID;
                else if (apf.cbRecMethod.Text.StartsWith("Subnet"))
                    pd.RecMethod = RecognizerMethod.Subnet;
                else if (apf.cbRecMethod.Text.StartsWith("NLA Description"))
                    pd.RecMethod = RecognizerMethod.NLA_Description;
                else if (apf.cbRecMethod.Text.StartsWith("NLA Category"))
                    pd.RecMethod = RecognizerMethod.NLA_Category;
                pd.RecData = String.Join("", apf.cbRecMethod.Text.Split(new[] { " = " }, StringSplitOptions.None).Skip(1));

                // Add the profile
                config.Profiles.Add(pd);
                UpdateProfileStatus();

                // Ensure the profile gets detected while it still has no actions
                netchanged = true;
                timer1_Tick(this, null);

                // Allow the user to add actions to this profile
                if (DoEditProfile(pd))
                {
                    // If the user added some actions, see if they want them to run now
                    if (pd.ActionList.Count > 0)
                    {
                        if (MessageBox.Show(this, "Run profile actions now?", "New Profile", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            pd.RunActions();
                    }
                }
            }
        }

        // Double-clicking on an active network lets us add a profile.
        private void dgNetworks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DoAddProfile(e.RowIndex);
            }
        }

        // Adding a profile is also available as a right-click context meho.
        private void addProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoAddProfile(dgNetworks.CurrentCell.RowIndex);
        }

        // When the form is minimized, we insert a delay so the minimize animation has time to run.
        private void NetShufflerForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                var ai = new Win32API.ANIMATIONINFO(0);
                Win32API.SystemParametersInfo(Win32API.SPI.SPI_GETANIMATION, ai.cbSize, ref ai, Win32API.SPIF.None);
                if (ai.iMinAnimate > 0)
                    System.Threading.Thread.Sleep(250);
                this.Hide();
            }
        }

        // User chose to delete a profile from the right-click context menu.
        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profile = dgProfiles.Rows[dgProfiles.CurrentCell.RowIndex].DataBoundItem as ProfileData;
            Profiles.Remove(profile);
            UpdateProfileStatus(true);
        }

        // User chose to run profile actions from the right-click context menu.
        private void executeProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (dgProfiles.Rows[dgProfiles.CurrentCell.RowIndex].DataBoundItem as ProfileData).RunActions();
        }

        // The following code allows the right mouse button to pull up a context menu for the
        // selected network, while avoiding null reference exceptions when the user right clicks 
        // on empty areas of the screen.
        bool SuppressNetworkMenu = false;

        private void dgNetworks_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                try
                {
                    dgNetworks.CurrentCell = null;
                    dgNetworks.CurrentCell = dgNetworks.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    SuppressNetworkMenu = false;
                }
                catch
                {
                    SuppressNetworkMenu = true;
                }
            }
        }

        private void NetworkMenu_Opening(object sender, CancelEventArgs e)
        {
            if (SuppressNetworkMenu)
                e.Cancel = true;
        }

        // The following code allows the right mouse button to pull up a context menu for the
        // selected profile, while avoiding null reference exceptions when the user right clicks 
        // on empty areas of the screen.
        bool SuppressProfileMenu = false;

        private void dgProfiles_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                try
                {
                    dgProfiles.CurrentCell = null;
                    dgProfiles.CurrentCell = dgProfiles.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    SuppressProfileMenu = false;
                }
                catch
                {
                    SuppressProfileMenu = true;
                }
            }
        }

        private void ProfileMenu_Opening(object sender, CancelEventArgs e)
        {
            if (SuppressProfileMenu)
                e.Cancel = true;
        }

        // For avoidance of duplication, we have a method to do all the things we
        // need to do when the user wants to edit the content of a profile.
        // Returns true if the user clicked OK or false if Cancel.
        private bool DoEditProfile(ProfileData pd)
        {
            // Create and populate the ProfileDetailForm
            var pdf = new ProfileDetailForm();
            foreach (var a in pd.ActionList)
                pdf.listBox1.Items.Add(a);

            // Show it to the user
            if (pdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // If the user clicked OK, then update the ProfileData
                pd.ActionList.Clear();
                foreach (var a in pdf.listBox1.Items)
                    if (a is ProfileAction)
                        pd.ActionList.Add(a as ProfileAction);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Overloaded method so we can call DoEditProfile with a row index
        private bool DoEditProfile(int RowIndex)
        {
            if (RowIndex >= 0)
                return DoEditProfile(dgProfiles.Rows[RowIndex].DataBoundItem as ProfileData);
            else
                return false;
        }

        // Double clicking on a profile should open the editor
        private void dgProfiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DoEditProfile(e.RowIndex);
        }

        // User right-clicked and requested to edit a profile
        private void editProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoEditProfile(dgProfiles.CurrentCell.RowIndex);
        }

        // We only set the highlighted row colors when focus is actually in the DataGrid, and then only
        // if suppress_grid_enter is false.
        private void DataGrid_Enter(object sender, EventArgs e)
        {
            if (!suppress_grid_enter)
            {
                (sender as DataGridView).DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                (sender as DataGridView).DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            }
        }

        // On leaving the DataGrid, stop showing the highlighted row.
        private void DataGrid_Leave(object sender, EventArgs e)
        {
            (sender as DataGridView).DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            (sender as DataGridView).DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
        }

        // When the main form is shown, then it's OK to enter the DataGrid.
        private void NetShufflerForm_Shown(object sender, EventArgs e)
        {
            suppress_grid_enter = false;
        }

        // When actually clicking on the networks DataGrid, including right clicking, show highlighted row.
        private void dgNetworks_MouseClick(object sender, MouseEventArgs e)
        {
            suppress_grid_enter = false;
            DataGrid_Enter(dgNetworks, e);
        }

        // When actually clicking on the networks DataGrid, including right clicking, show highlighted row.
        private void dgProfiles_MouseClick(object sender, MouseEventArgs e)
        {
            suppress_grid_enter = false;
            DataGrid_Enter(dgProfiles, e);
        }

        // When using the keyboard to navigate the DataGrid, show the highlighted row. Also, show the
        // context menu if the user hits Enter.
        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            suppress_grid_enter = false;
            DataGrid_Enter(dgNetworks, e);
            if (e.KeyCode == Keys.Enter)
            {
                Rectangle dr = dgNetworks.GetCellDisplayRectangle(dgNetworks.CurrentCellAddress.X, dgNetworks.CurrentCellAddress.Y, true);
                if (sender == dgNetworks)
                    NetworkMenu.Show(dgNetworks, dr.X + dr.Width / 2, dr.Y + dr.Height / 2);
                if (sender == dgProfiles)
                    ProfileMenu.Show(dgNetworks, dr.X + dr.Width / 2, dr.Y + dr.Height / 2);
                e.Handled = true;
            }
        }

        // If the user hits Escape, then the form should minimize to the system tray.
        private void NetShufflerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Handled = true;
            }
        }

        // When the user closes the main form, we want to go back to being
        // minimized to the task tray, so we have to override the normal
        // close behavior. However, when the user right-clicks and chooses
        // exit, we really want to close.
        bool ReallyClose = false;

        // User requested exit - set "ReallyClose" so FormClosing won't block it.
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReallyClose = true;
            Application.Exit();
        }

        // Someone (user or system) closed the main form. Either minimize or exit.
        private void NetShufflerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                config.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if ((e.CloseReason == CloseReason.UserClosing) && (!ReallyClose) && (!System.Diagnostics.Debugger.IsAttached))
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                suppress_grid_enter = true;
            }
        }

        // If the main form actually closes (which means ReallyClose was true), exit the app.
        private void NetShufflerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Turn on or off automatic startup at login.
        private void startWithWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startWithWindowsToolStripMenuItem.Checked = !startWithWindowsToolStripMenuItem.Checked;
            StartAtLoginConfig.SetStartupConfig(startWithWindowsToolStripMenuItem.Checked);
        }

    }

}
