using System;
using System.Drawing;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;


using System.Media;
using Microsoft.Win32;
using BoGoViet.TiengViet;
using WindowsInput;
using BoGoViet;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Threading;

namespace GotiengVietApplication
{

    public partial class GotiengVietForm : Form
    {
        private IKeyboardMouseEvents m_Events;
        // che do go tieng anh
        private bool goEnglish = false;
        private RegistryKey rk;
        private TiengViet tv = new TiengViet();
        private bool isAltPress = false;
        private InputSimulator sim;
        About about = null;
        private string vietIcon = "icon/vietnam2.ico";
        private string engIcon = "icon/english2.ico";
        public GotiengVietForm()
        {
            InitializeComponent();
            // this.selectTypeInput.SelectedItem = "Telex";
            
            SubscribeGlobal();
            FormClosing += Form1_Closing;
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);

            rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue("Gõ Tiếng Việt", Application.ExecutablePath.ToString());

            sim = new InputSimulator();
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            MinimizeBox = false;
            
            if (BoGoViet.Properties.Settings.Default.useNewIcon) {
                vietIcon = "icon/vietnam.ico";
                engIcon = "icon/english.ico";
                sửDụngIconHiệnĐạiToolStripMenuItem.Checked = true;
            }
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            bogoviet.Icon = new Icon(baseDir + vietIcon);
            tv.SetKieuGo(BoGoViet.Properties.Settings.Default.bogo);
            this.selectTypeInput.SelectedItem = BoGoViet.Properties.Settings.Default.bogo;
        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                // I left my desk
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                // I returned to my desk
                // ChangeVietEng();
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void SubscribeApplication()
        {
            Unsubscribe();
            Subscribe(Hook.AppEvents());
        }

        private void SubscribeGlobal()
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
        }

        private void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.KeyDown += OnKeyDown;
            m_Events.KeyUp += OnKeyUp;
            m_Events.MouseClick += OnMouseClick;
        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.KeyDown -= OnKeyDown;
            m_Events.KeyUp -= OnKeyUp;
            m_Events.MouseClick -= OnMouseClick;

            m_Events.Dispose();
            m_Events = null;
        }

        

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            tv.OnMouseClick(ref sender, ref e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (this.special.Checked)
            {
                if (e.KeyCode == Keys.Capital)
                {
                    e.Handled = true;
                    return;
                }
            }
            
            if (e.KeyCode == Keys.LShiftKey)
            {
                pressLeftShift = true;
            }
            */
            if (e.KeyCode == Keys.LMenu)
            {
                isAltPress = true;
            }

            if (isAltPress && e.KeyCode == Keys.Z)
            {
                ChangeVietEng();
                return;
            }

            m_Events.KeyDown -= OnKeyDown;
            m_Events.KeyUp -= OnKeyUp;
            if (!goEnglish) { 
                tv.OnKeyDown(ref sender, ref e);
            }
            /*
            if (this.special.Checked)
            {
                if (e.KeyCode == Keys.PageUp)
                {
                    e.Handled = true;
                    sim.Keyboard.KeyPress(VirtualKeyCode.INSERT);
                }
                else if (pressLeftShift && e.KeyCode == Keys.PageDown)
                {
                    e.Handled = true;

                    sim.Keyboard.KeyUp(VirtualKeyCode.LSHIFT);
                    sim.Keyboard.KeyPress(VirtualKeyCode.PRIOR);
                    sim.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                }
            }
            */
            m_Events.KeyDown += OnKeyDown;
            m_Events.KeyUp += OnKeyUp;
        }

        
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (this.special.Checked)
            {
                if (e.KeyCode == Keys.Capital)
                {
                    e.Handled = true;
                    return;
                }
            }
            
            if (e.KeyCode == Keys.LShiftKey)
            {
                pressLeftShift = false;
            }
            */
            if (e.KeyCode == Keys.LMenu)
            {
                isAltPress = false;
            }
            tv.OnKeyUp(ref sender, ref e);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }
        void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            

        }

        private void ChangeVietEng()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            goEnglish = !goEnglish;
            tv.Reset();
            SystemSounds.Beep.Play();
            
            if (goEnglish)
            {
                bogoviet.Icon = new Icon(baseDir + engIcon);
                bogoviet.Text = "Bộ Gõ Việt (Tắt)";
            }
            else
            {
                bogoviet.Icon = new Icon(baseDir + vietIcon);
                bogoviet.Text = "Bộ Gõ Việt (Bật)";
            }
        }

        private void bogoviet_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ChangeVietEng();
            }
        }

        

        private void chkStartUp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartUp.Checked) { 
                rk.SetValue("Gõ Tiếng Việt", Application.ExecutablePath.ToString());
            }
            else { 
                rk.DeleteValue("Gõ Tiếng Việt", false);
            }
        }


        private void thoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void selectTypeInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kieuGo = (string) this.selectTypeInput.SelectedItem;
            tv.SetKieuGo(kieuGo);
            BoGoViet.Properties.Settings.Default.bogo = kieuGo;
            BoGoViet.Properties.Settings.Default.Save();
        }

        private void giớiThiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void giớiThiệuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // buttonGioiThieu_Click(sender, e);
            if (about == null)
            {
                about = new About();
            }
            if (!about.Visible)
            {
                about.ShowDialog();
            }
        }

        public static bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsAlreadyRunning()
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            string currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Process thisProc = Process.GetCurrentProcess();
            if (IsProcessOpen(sExeName) == true)
            {
                if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
                {
                    return true;
                }
            }
            return false;
        }

        private void GotiengVietForm_Load(object sender, EventArgs e)
        {
            if (IsAlreadyRunning())
            {
                MessageBox.Show("Ứng dụng Gõ Tiếng Việt đang chạy");
                Application.Exit();
            }
        }

        private void buttonDong_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void sửDụngIconHiệnĐạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sửDụngIconHiệnĐạiToolStripMenuItem.Checked = !sửDụngIconHiệnĐạiToolStripMenuItem.Checked;
            if (sửDụngIconHiệnĐạiToolStripMenuItem.Checked) {
                BoGoViet.Properties.Settings.Default.useNewIcon = true;
                vietIcon = "icon/vietnam.ico";
                engIcon = "icon/english.ico";
            } else
            {
                BoGoViet.Properties.Settings.Default.useNewIcon = false;
                vietIcon = "icon/vietnam2.ico";
                engIcon = "icon/english2.ico";
            }
            BoGoViet.Properties.Settings.Default.Save();
            goEnglish = !goEnglish;
            ChangeVietEng();
        }

        public bool IsCurrentOSContains(string name)
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");

            return productName.Contains(name);
        }

        /// Check if it's Windows 8.1
        public bool IsWindows8Dot1()
        {
            return IsCurrentOSContains("Windows 8.1");
        }

        /// Check if it's Windows 10
        public bool IsWindows10()
        {
            return IsCurrentOSContains("Windows 10");
        }
    }
}
