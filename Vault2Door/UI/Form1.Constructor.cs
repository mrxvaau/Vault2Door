using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = $"{AppName} v{AppVersion}";
            this.Size = new Size(1280, 800);
            this.MinimumSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.D) { ToggleTheme(); e.Handled = true; }
            };

            if (!Directory.Exists(gifPathRoot))
            {
                var localGif = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gif");
                if (Directory.Exists(localGif)) gifPathRoot = localGif + Path.DirectorySeparatorChar;
            }

            BuildDashboardUI();
            ApplyTheme();
            ShowChart("gold");

            this.Resize += (s, e) => { UpdateAssetScrollMetrics(); AlignVerticalLayout(); };

            realTimer.Interval = AppConfig.PollIntervalMs;  // 60s default
            realTimer.Tick += async (s, e) => await LoadRealtimeAsync();
        }

        private void Form1_Load(object? sender, EventArgs e) { }
    }
}
