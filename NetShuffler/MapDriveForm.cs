using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetShuffler
{
    public partial class MapDriveForm : Form
    {
        public MapDriveForm()
        {
            InitializeComponent();
        }

        private void button1_Validating(object sender, CancelEventArgs e)
        {
            // Only allow an OK result if a drive has been selected and something typed into the text field.
            if ((comboBox1.SelectedIndex < 0) || (textBox1.TextLength <= 0))
                e.Cancel = true;
        }
    }
}
