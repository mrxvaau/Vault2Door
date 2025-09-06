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

            // Regular app behavior: resizable, min & max buttons
            this.Size = new Size(1280, 800);
            this.MinimumSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            // Keyboard theme toggle (Ctrl + D)
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.D)
                {
                    ToggleTheme();
                    e.Handled = true;
                }
            };

            // Path fallback (compat; not used by charts anymore)
            if (!Directory.Exists(gifPathRoot))
            {
                var localGif = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gif");
                if (Directory.Exists(localGif))
                    gifPathRoot = localGif + System.IO.Path.DirectorySeparatorChar;
            }

            BuildDashboardUI();
            ApplyTheme(); // start in chosen theme

            // default chart preset
            ShowChart("gold");

            // keep scrollbar metrics fresh on resize
            this.Resize += (s, e) => UpdateAssetScrollMetrics();
        }

        // Designer wires this; keep it to avoid CS0103
        private void Form1_Load(object? sender, EventArgs e) { }
    }
}
