﻿using System;
using System.Windows.Forms;
using WebBrowserLaboratory.Forms;

namespace WebBrowserLaboratory
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
            //Application.Run(new MyWebBrowser());
            //Application.Run(new FlexCelAPI());
            Application.Run(new CheckFile());
        }
    }
}