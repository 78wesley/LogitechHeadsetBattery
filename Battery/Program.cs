using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Battery
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (processes.Length > 1)
            {
                MessageBox.Show("Sorry, this program is already running!");
                return;
            }
            else
                Application.Run(new Form1());
        }

           
        }
    }

