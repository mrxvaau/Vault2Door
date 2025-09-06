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
            this.Size = new Size(1500, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

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

            // Path fallback if absolute path doesn't exist
            if (!Directory.Exists(gifPathRoot))
            {
                var localGif = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gif");
                if (Directory.Exists(localGif))
                    gifPathRoot = localGif + Path.DirectorySeparatorChar;
            }

            BuildDashboardUI();
            ApplyTheme(); // start in chosen theme
        }

        // Designer wires this; keep it to avoid CS0103
        private void Form1_Load(object? sender, EventArgs e) { }
    }
}
