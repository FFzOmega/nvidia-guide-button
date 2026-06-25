using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GuideToF20
{
    public partial class MainForm : Form
    {
        private readonly ControllerManager controller;
        private readonly System.Windows.Forms.Timer timer;
        private readonly NotifyIcon trayIcon;

        private readonly ToolStripMenuItem controllerItem;
        private readonly ToolStripMenuItem enabledItem;
        private readonly ToolStripMenuItem startupItem;

        public MainForm()
        {
            InitializeComponent();

            controller = new ControllerManager();
            controller.StatusChanged += Controller_StatusChanged;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 5;
            timer.Tick += Timer_Tick;
            timer.Start();

            var trayMenu = new ContextMenuStrip();

            var titleItem = new ToolStripMenuItem("Guide Nvidia");
            titleItem.Enabled = false;

            controllerItem = new ToolStripMenuItem("Connected: " + controller.ConnectedControllerName);
            controllerItem.Enabled = false;

            enabledItem = new ToolStripMenuItem("Enabled");
            enabledItem.Checked = true;
            enabledItem.CheckOnClick = true;
            enabledItem.Click += Enabled_Click;

            startupItem = new ToolStripMenuItem("Start with Windows");
            startupItem.Checked = StartupManager.IsEnabled();
            startupItem.CheckOnClick = true;
            startupItem.Click += Startup_Click;

            trayMenu.Items.Add(titleItem);
            trayMenu.Items.Add(controllerItem);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add(enabledItem);
            trayMenu.Items.Add(startupItem);
            trayMenu.Items.Add("Reload Controller", null, Reload_Click);
            trayMenu.Items.Add("About...", null, About_Click);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add("Exit", null, Exit_Click);

            trayIcon = new NotifyIcon();
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            trayIcon.Text = "Guide Nvidia";
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;

            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.Shown += MainForm_Shown;
            this.FormClosing += MainForm_FormClosing;
        }

        private void MainForm_Shown(object? sender, EventArgs e)
        {
            this.Hide();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            controller.Update();
        }

        private void Controller_StatusChanged(string text)
        {
            controllerItem.Text = "Connected: " + controller.ConnectedControllerName;

            string trayText = "Guide Nvidia - " + text;
            trayIcon.Text = trayText.Length > 63 ? trayText.Substring(0, 63) : trayText;
        }

        private void Enabled_Click(object? sender, EventArgs e)
        {
            controller.Enabled = enabledItem.Checked;
            trayIcon.Text = enabledItem.Checked ? "Guide Nvidia - Enabled" : "Guide Nvidia - Disabled";
        }

        private void Startup_Click(object? sender, EventArgs e)
        {
            StartupManager.SetEnabled(startupItem.Checked);
        }

        private void Reload_Click(object? sender, EventArgs e)
        {
            controller.ReloadController();
            controllerItem.Text = "Connected: " + controller.ConnectedControllerName;
        }

        private void About_Click(object? sender, EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            MessageBox.Show(
                $"Guide Nvidia\n\n" +
                $"Version {version}\n\n" +
                $"Guide button remapper for DirectInput controllers.\n\n" +
                $"Single Tap  → Ctrl+Home\n" +
                $"Double Tap  → F20\n" +
                $"Hold (2s)   → F17\n\n" +
                $"Created by ChatGPT 5-5 and guided by Abner H. Braga",
                "About Guide Nvidia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Exit_Click(object? sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            trayIcon.Visible = false;
        }
    }
}