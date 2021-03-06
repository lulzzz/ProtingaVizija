﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsForms.FormControl;
using Helpers;
using FaceAnalysis;

namespace WindowsForms
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
            Application.Run(new FormFaceDetection());
        }
    }
}
