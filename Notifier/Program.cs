﻿using System;
using System.Configuration;
using System.Windows.Forms;
using Notifier.Model;
using Notifier.View;

namespace Notifier
{
    static class Program
    {
        const string LogPath = @"C:\TEMP";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    RunApplication();
                }
                else
                {
                    if (!AttachConsole(-1))
                    {
                        // Attach to an parent process console
                        AllocConsole(); // Alloc a new console
                    }
                    HandleArgs(args);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WriteException(ex);
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        private static void HandleArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                switch (arg)
                {
                    case "encrypt":
                        if (args.Length > i + 1)
                        {
                            string text = args[i + 1];
                            string encrypt = Configuration.ConfigurationManager.EncryptConnectionString(text);
                            i = i + 1;
                            Console.WriteLine(encrypt);
                        }
                        break;
                }
            }
        }

        private static void model_ExceptionThrown(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
                WriteException(ex);
        }

        private static void RunApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NotifierApplicationContext context = new NotifierApplicationContext();

            try
            {
                string viewName = ConfigurationManager.AppSettings["ViewName"];
                Type viewType = Type.GetType(viewName);
                context.View = Activator.CreateInstance(viewType) as INotificationView;
            }
            catch
            {
                context.View = new NotifyIconBalloon();
            }

            double second;
            try
            {
                string interval = ConfigurationManager.AppSettings["Interval"] ?? "60";
                second = Convert.ToDouble(interval);
            }
            catch
            {
                second = 60;
            }
            context.Interval = second * 1000;

            INotificationModel model = Configuration.ConfigurationManager.FromXml("settings.xml");
            model.ExceptionThrown += model_ExceptionThrown;
            context.Model = model;
            context.Start();
            Application.Run(context);
        }

        private static void WriteException(Exception ex)
        {
            if (!System.IO.Directory.Exists(LogPath))
                System.IO.Directory.CreateDirectory(LogPath);
            System.IO.File.AppendAllText(
                string.Format("{0}\\{1}", LogPath, @"Notifier.log"),
                string.Format("\n{0}\n{1}", ex.Message, ex.StackTrace)
            );
        }
    }
}
