using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace GotiengVietApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major <= 6)
                SetProcessDPIAware();

            string currentUser = Environment.UserName;
            string processName = Process.GetCurrentProcess().ProcessName;
            Mutex mutex = new Mutex(true, @"Global\" + currentUser + "-" + processName);
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new GotiengVietForm());
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                MessageBox.Show("Ứng dụng Gõ Tiếng Việt đang chạy");
            }
            
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
