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
    public partial class RunProgramForm : Form
    {
        public RunProgramForm()
        {
            InitializeComponent();
        }

        private void button1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.TextLength <= 0)
                e.Cancel = true;
        }
    }
}
