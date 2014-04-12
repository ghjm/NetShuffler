using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net;
using NETWORKLIST;
using DotRas;
using Microsoft.Win32;

namespace NetShuffler
{

    // Enumeration for types of network recognizers.
    public enum RecognizerMethod : int
    {
        None = 0,
        DNS_Suffix = 1,
        WiFi_SSID = 2,
        Subnet = 3,
        NLA_Description = 4,
        NLA_Category = 5,
    }

    // Base class for all ProfileActions. The fundamental feature of a ProfileAction is that it can provide
    // a thread-safe Action to be called when the action is to be performed.

    // We add attributes for all the defined subclasses here so that the XML serializer will be able to identify
    // and serialize each subclass, even if it is only given a reference to the abstract base class.
    [XmlInclude(typeof(PAMapDrive))]
    [XmlInclude(typeof(PADefaultPrinter))]
    [XmlInclude(typeof(PARunProgram))]
    [XmlInclude(typeof(PAConnectVPN))]
    public abstract class ProfileAction
    {
        public abstract Action GetAction();
    }

    // ProfileAction for mapping a drive letter.
    public class PAMapDrive : ProfileAction
    {
        public string DriveLetter { get; set; }
        public string FolderPath { get; set; }

        public override Action GetAction()
        {
            // Create a "runner" as an entirely distinct class instance.
            var pmdr = new PAMapDriveRunner();
            pmdr.DriveLetter = DriveLetter;
            pmdr.FolderPath = FolderPath;
            return new Action(() => pmdr.MapDrive());
        }

        public override string ToString()
        {
            return "Map drive " + DriveLetter + " to folder " + FolderPath;
        }
    }

    // Runner class for mapping a drive letter. This is distinct from
    // the ProfileAction class for thread safety.
    public class PAMapDriveRunner
    {
        public string DriveLetter;
        public string FolderPath;
        public void MapDrive()
        {
            // Retrieve the existing drive mapping for this drive letter, if any.
            int capacity = 255;
            var sb = new StringBuilder(capacity);
            if (Win32API.WNetGetConnection(DriveLetter, sb, ref capacity) == 0)
            {
                // If the drive is already mapped to the desired folder, we do not need to do anything.
                if (sb.ToString().ToLower() == FolderPath.ToLower())
                    return;
                else
                {
                    // The drive is already mapped to a different folder, so we unmap it here.
                    Win32API.WNetCancelConnection(DriveLetter, true);
                    // On some networks, immediately re-mapping after disconnecting will fail.
                    // Note: This is running on a background thread, not the GUI thread.
                    Thread.Sleep(1000);
                }
            }

            // Create a NETRESOURCE structure describing the desired drive mapping.
            var nr = new Win32API.NETRESOURCE()
            {
                dwScope = Win32API.ResourceScope.GlobalNet,
                dwType = Win32API.ResourceType.Disk,
                dwDisplayType = Win32API.ResourceDisplayType.Share,
                lpRemoteName = FolderPath,
                lpLocalName = DriveLetter,
            };

            // Map the drive letter.
            if (Win32API.WNetAddConnection2(nr, null, null, Win32API.CONNECT_TEMPORARY) != 0)
                Win32API.ThrowException();

        }
    }

    // ProfileAction for changing the default printer.
    public class PADefaultPrinter : ProfileAction
    {
        public string PrinterName { get; set; }

        public override Action GetAction()
        {
            // Create a "runner" as an entirely distinct class instance.
            var pmdr = new PADefaultPrinterRunner();
            pmdr.PrinterName = PrinterName;
            return new Action(() => pmdr.SetDefaultPrinter());
        }

        public override string ToString()
        {
            return "Set default printer to " + PrinterName;
        }
    }

    // Runner class for changing the default printer. This is distinct from
    // the ProfileAction class for thread safety.
    public class PADefaultPrinterRunner
    {
        public string PrinterName;
        public void SetDefaultPrinter()
        {
            // Check to make sure that the named printer actually exists as an installed printer.
            // We are given a nice .NET class for listing installed printers.
            bool found = false;
            foreach (var p in PrinterSettings.InstalledPrinters)
            {
                if (p.ToString().ToLower() == PrinterName.ToLower())
                    found = true;
            }

            // If it exists, set it as the default. Oddly, this is not a function of the .NET
            // class, so we have to call the Win32 API to make this happen.
            if (found)
                Win32API.SetDefaultPrinter(PrinterName);
        }
    }

    // ProfileAction for running an external program.
    public class PARunProgram : ProfileAction
    {
        public string Target { get; set; }
        public string Arguments { get; set; }
        public bool SuppressIfRunning { get; set; }

        public override Action GetAction()
        {
            // Create a "runner" as an entirely distinct class instance.
            var prpr = new PARunProgramRunner();
            prpr.Target = Target;
            prpr.Arguments = Arguments;
            prpr.SuppressIfRunning = SuppressIfRunning;
            return new Action(() => prpr.RunProgram());
        }

        public override string ToString()
        {
            return "Run program " + Target;
        }
    }

    // Runner class for executing an external program. This is distinct from
    // the ProfileAction class for thread safety.
    public class PARunProgramRunner
    {
        public string Target;
        public string Arguments;
        public bool SuppressIfRunning;
        public void RunProgram()
        {
            // The "suppress if running" feature lets the user prevent us from starting
            // a new instance of the program if it was already active. For example, if
            // we are launching Outlook when connecting to the VPN, we don't want a
            // second copy to launch if Outlook was already loaded when the VPN connected.
            if (SuppressIfRunning)
            {
                // Use .NET to search the list of processes for one that matches our filename.
                var plist = Process.GetProcesses();
                foreach (var p in plist)
                {
                    bool found = false;
                    try
                    {
                        found = (p.MainModule.FileName.ToLower() == Target.ToLower());
                    }
                    catch
                    {
                        // on error, assume it wasn't a match
                    }
                    // If we get here and found is true, then we should suppress the action.
                    // It's easier to just return here rather than add more boolean logic below.
                    if (found)
                        return;
                }
            }
            // Launch the program as requested.
            try
            {
                var prog = new Process();
                prog.StartInfo.FileName = Target;
                prog.StartInfo.Arguments = Arguments;
                prog.Start();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error launching program." + Environment.NewLine +
                    "Program: " + Target + Environment.NewLine +
                    "Arguments: " + Arguments + Environment.NewLine +
                    "Error: " + e.Message); 
            }
        }
    }

    // ProfileAction for mapping a drive letter.
    public class PAConnectVPN : ProfileAction
    {
        public string VPN_Name { get; set; }

        public override Action GetAction()
        {
            // Create a "runner" as an entirely distinct class instance.
            var pcvr = new PAConnectVPNRunner();
            pcvr.VPN_Name = VPN_Name;
            return new Action(() => pcvr.ConnectVPN());
        }

        public override string ToString()
        {
            return "Connect VPN " + VPN_Name;
        }
    }

    // Runner class for connecting a VPN. This is distinct from
    // the ProfileAction class for thread safety.
    public class PAConnectVPNRunner
    {
        public string VPN_Name;
        public void ConnectVPN()
        {
            // To connect the VPN, we need to know which phonebook it is in.
            string pbpath = null;
            var rpbk = new RasPhoneBook();
            rpbk.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers));
            foreach (var re in rpbk.Entries)
                if (re.Name == VPN_Name)
                {
                    pbpath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
                    break;
                }
            if (pbpath == null)
            {
                rpbk.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
                foreach (var re in rpbk.Entries)
                    if (re.Name == VPN_Name)
                    {
                        pbpath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
                        break;
                    }
            }
            if (pbpath != null)
            {
                // We found it, now dial it.
                var dialer = new RasDialer();
                dialer.PhoneBookPath = pbpath;
                dialer.EntryName = VPN_Name;
                dialer.AllowUseStoredCredentials = true;
                dialer.Dial();
            }
        }
    }

    // Class for storing configuration information related to a specific profile.
    // All member fields must support XML serialization.
    public class ProfileData
    {
        // Which recognition method will trigger this profile and what should it recognize?
        public RecognizerMethod RecMethod = RecognizerMethod.None;
        public string RecData = "";

        // Human-readable description of the recognition method and data.
        public string RecognizeBy
        {
            get
            {
                switch (RecMethod)
                {
                    case RecognizerMethod.DNS_Suffix:
                        return "DNS Suffix = " + RecData;
                    case RecognizerMethod.WiFi_SSID:
                        return "WiFi_SSID = " + RecData;
                    case RecognizerMethod.Subnet:
                        return "Subnet = " + RecData;
                    case RecognizerMethod.NLA_Description:
                        return "NLA Description = " + RecData;
                    case RecognizerMethod.NLA_Category:
                        return "NLA Category = " + RecData;
                    default:
                        return "";
                }
            }
        }

        // List of actions to run when this profile is activated.
        // This is a BindingList so it can be bound to data views in the GUI.
        private BindingList<ProfileAction> _ActionList = new BindingList<ProfileAction>();
        public BindingList<ProfileAction> ActionList { get { return _ActionList; } }

        // Run-time-only property indicating whether this profile is currently active or not.
        [XmlIgnore]
        public bool Active { get; set; }

        // Method to execute all the configured actions in this profile.
        public void RunActions()
        {
            foreach (var ali in ActionList)
            {
                var ts = new ThreadStart(ali.GetAction());
                var thr = new Thread(ts);
                thr.Start();
            }
        }
    }

    // Class to hold application-level configuration settings.
    // All member fields must support XML serialization.
    public class AppConfig
    {
        // Global enable/disable for considering IPV6 addresses.
        public bool IPV6Enabled { get; set; }
        // Global setting for showing/hiding inactive profiles.
        public bool ShowInactiveProfiles { get; set; }

        // List of adapters which we will ignore for detection purposes.
        // This is a BindingList so it can be bound to data view controls in the GUI.
        private BindingList<string> _AdapterIgnoreList = new BindingList<string>();
        public BindingList<string> AdapterIgnoreList
        {
            get { return _AdapterIgnoreList; }
        }

        // List of network profiles. Profiles are containers for actions.
        // This is a BindingList so it can be bound to data view controls in the GUI.
        private BindingList<ProfileData> _Profiles = new BindingList<ProfileData>();
        public BindingList<ProfileData> Profiles
        {
            get { return _Profiles; }
        }

        // Write the current configuration information out to a file in the Windows user profile.
        public void Save()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForAssembly())
            using (var stream = store.CreateFile(@"NetShuffler.xml"))
            using (var sw = new StreamWriter(stream))
            {
                XmlSerializer ser = new XmlSerializer(typeof(AppConfig));
                ser.Serialize(sw, this);
            }
        }

        // Read configuration information from a file in the Windows user profile.
        public static AppConfig Load()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForAssembly())
                using (var stream = store.OpenFile(@"NetShuffler.xml", FileMode.Open))
                using (var sr = new StreamReader(stream))
                {
                    var ser = new XmlSerializer(typeof(AppConfig));
                    return (AppConfig)ser.Deserialize(sr);
                }
            }
            catch
            {
                return new AppConfig();
            }
        }
    }

    // Handy exenstion method for creating comma-delimited strings.
    public static class Extensions
    {
        public static StringBuilder AppendWithComma(this StringBuilder builder, string s)
        {
            if (builder.Length == 0)
                return builder.Append(s);
            else
                return builder.Append(", ").Append(s);
        }
    }

    // Class to handle registry configuration for starting the app at login.
    public static class StartAtLoginConfig
    {
        public static bool CheckStartupConfig()
        {
            var rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            if (!rk.GetSubKeyNames().Contains(@"NetShuffler"))
                return false;
            else
                return true;
        }
        public static void SetStartupConfig(bool StartAtLogin)
        {
            if (StartAtLogin)
            {
                var rk = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk.SetValue(@"NetShuffler", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"", RegistryValueKind.String);
            }
            else
            {
                var rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                rk.DeleteValue(@"NetShuffler");
            }
        }
    }

}