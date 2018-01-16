using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace bitboard
{
    class SysTrayApp : Form
    {
        public string SysRoot = Application.StartupPath;
        public bitboardWorld world = new bitboardWorld();
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SysTrayApp());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public SysTrayApp()
        {
            //move this to XMPP module
            xmppManager.world = this.world;


            this.world.SysRoot = Application.StartupPath;
            this.world.agi.mount(0,"sysfs","/local/system", new string[] {this.world.SysRoot });
            this.world.agi.mount(0, "homefs", "/local/home", new string[] { "" });
            fileSystem proc = this.world.agi.mount(0, "procfs", "/local/home", new string[] { "" });
            proc.init(this.world);

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Programs", OnPrograms);
            trayMenu.MenuItems.Add("Shell", OnShell);
            trayMenu.MenuItems.Add("BEdit", OnEdit);
            trayMenu.MenuItems.Add("Login", OnLogin);
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "BitBoard";
            trayIcon.Icon = new Icon("Resources/softsurve.ico", 32,32);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnEdit(object sender, EventArgs e)
        {
            BEdit editor = new BEdit(this.world, "");
            editor.Show();
        }

        private void OnPrograms(object sender, EventArgs e)
        {
 
        }

        private void OnLogin(object sender, EventArgs e)
        {
            Form form3= new Form3();
            form3.Show();
        }

        private void OnShell(object sender, EventArgs e)
        {
            Form shellForm = new ShellForm(this.world);
            shellForm.Show();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (this.world.agi.Shutdown() == 0)
                {
                    trayIcon.Dispose();
                    base.Dispose(isDisposing);
                }
                else
                {
                    Console.WriteLine("AgI returned error on Shutdown");
                }
            }
        }
    }
}
