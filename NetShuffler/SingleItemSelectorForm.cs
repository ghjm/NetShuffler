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
    public partial class SingleItemSelectorForm : Form
    {
        public SingleItemSelectorForm()
        {
            InitializeComponent();
        }

        public void SetText(string DialogTitle, string SelectorName)
        {
            this.Text = DialogTitle;
            lblSelectorName.Text = SelectorName;
        }

        private void button1_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                e.Cancel = true;
        }
    }
}
