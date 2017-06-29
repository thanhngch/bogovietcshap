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
        private string vietIcon = "icon/vietnam.ico";
        private string engIcon = "icon/english.ico";
        public GotiengVietForm()
        {
            InitializeComponent();
            this.selectTypeInput.SelectedItem = "Telex";
            
            SubscribeGlobal();
            FormClosing += Form1_Closing;

            rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue("Gõ Tiếng Việt", Application.ExecutablePath.ToString());

            sim = new InputSimulator();
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            MinimizeBox = false;
            if (IsWindows7AndBelow())
            {
                vietIcon = "icon/vietnam2.ico";
                engIcon = "icon/english2.ico";
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                bogoviet.Icon = new Icon(baseDir + vietIcon);
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
                ChangeVietEng(this.bogoviet);
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

        private void ChangeVietEng(object sender)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            goEnglish = !goEnglish;
            tv.Reset();
            SystemSounds.Beep.Play();

            NotifyIcon notifyIcon = (NotifyIcon)sender;
            if (goEnglish)
            {
                notifyIcon.Icon = new Icon(baseDir + engIcon);
                notifyIcon.Text = "Bộ Gõ Việt (Tắt)";
            }
            else
            {
                notifyIcon.Icon = new Icon(baseDir + vietIcon);
                notifyIcon.Text = "Bộ Gõ Việt (Bật)";
            }
        }

        private void bogoviet_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ChangeVietEng(sender);
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
        private static bool IsAlreadyRunning()
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            bool bCreatedNew;

            Mutex mutex = new Mutex(true, "Global\\" + sExeName, out bCreatedNew);
            if (bCreatedNew)
            { 
                mutex.ReleaseMutex();
            }
            return !bCreatedNew;
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
        private bool IsWindows7AndBelow()
        {
            return (Environment.OSVersion.Version.Major == 6 &&
                Environment.OSVersion.Version.Minor <= 1) || 
                Environment.OSVersion.Version.Major < 6;
        }
        
    }
}
