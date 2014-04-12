using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotRas;

namespace NetShuffler
{
    public partial class ProfileDetailForm : Form
    {
        public ProfileDetailForm()
        {
            InitializeComponent();
        }

        // Remove selected item from the profile actions list.
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if ((listBox1.SelectedIndex >= 0) && (listBox1.SelectedIndex < listBox1.Items.Count))
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        // Add a new mapped drive action.
        private void btnMapDrive_Click(object sender, EventArgs e)
        {
            var mdf = new MapDriveForm();
            if (mdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var pmd = new PAMapDrive();
                pmd.DriveLetter = mdf.comboBox1.Text;
                pmd.FolderPath = mdf.textBox1.Text;
                listBox1.Items.Add(pmd);
            }
        }

        // Add a new default printer action.
        private void btnSetPrinter_Click(object sender, EventArgs e)
        {
            var sis = new SingleItemSelectorForm();
            sis.SetText("Set Default Printer", "Printer:");

            // Populate the combo box drop-down with the list of installed printers.
            foreach (string s in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                sis.comboBox1.Items.Add(s);

            if (sis.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var pdp = new PADefaultPrinter();
                pdp.PrinterName = sis.comboBox1.Text;
                listBox1.Items.Add(pdp);
            }
        }

        // Add a new run program action.
        private void btnRunProgram_Click(object sender, EventArgs e)
        {
            var rpf = new RunProgramForm();
            if (rpf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var pdp = new PARunProgram();
                pdp.Target = rpf.textBox1.Text;
                pdp.Arguments = rpf.textBox2.Text;
                pdp.SuppressIfRunning = rpf.checkBox1.Checked;
                listBox1.Items.Add(pdp);
            }
        }

        // Add a new VPN connection action.
        private void btnConnectVPN_Click(object sender, EventArgs e)
        {
            var sis = new SingleItemSelectorForm();
            sis.SetText("Connect to VPN", "VPN Name:");

            // Populate the combo box drop-down with the list of known VPNs.
            var rpbk = new RasPhoneBook();
            rpbk.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers));
            foreach (var rent in rpbk.Entries)
                sis.comboBox1.Items.Add(rent.Name);
            rpbk.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
            foreach (var rent in rpbk.Entries)
                sis.comboBox1.Items.Add(rent.Name);

            if (sis.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var pcv = new PAConnectVPN();
                pcv.VPN_Name = sis.comboBox1.Text;
                listBox1.Items.Add(pcv);
            }
        }

    }
}
