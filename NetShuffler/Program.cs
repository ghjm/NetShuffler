using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetShuffler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // If we are running within Visual Studio, then exit when the main form closes.
            // Otherwise, the application runs with or without the main form (with a taskbar tray icon).
            if (System.Diagnostics.Debugger.IsAttached)
                Application.Run(new NetShufflerForm());
            else
            {
                var nsf = new NetShufflerForm();

                // Process command line
                foreach (string arg in Environment.GetCommandLineArgs())
                {
                    if (arg.ToLower() == @"/show")
                        nsf.Show();
                }

                Application.Run();
            }
        }
    }
}
