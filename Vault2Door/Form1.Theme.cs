using System.Drawing;
using System.Windows.Forms;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        private void ToggleTheme()
        {
            isDarkMode = !isDarkMode;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            // Colors tuned for contrast
            Color formBg = isDarkMode ? Color.FromArgb(18, 20, 23) : Color.FromArgb(240, 242, 245);
            Color panelBg = isDarkMode ? Color.FromArgb(24, 27, 32) : Color.White;
            Color sidebarBg = isDarkMode ? Color.FromArgb(28, 31, 37) : Color.FromArgb(245, 247, 250);
            Color textColor = isDarkMode ? Color.Gainsboro : Color.Black;
            Color btnBg = isDarkMode ? Color.FromArgb(43, 47, 54) : Color.White;
            Color borderColor = isDarkMode ? Color.FromArgb(90, 95, 105) : Color.FromArgb(200, 200, 200);

            this.BackColor = formBg;

            // Sidebar theme (buttons + bg + borders)
            if (sidebar != null)
            {
                sidebar.BackColor = sidebarBg;
                foreach (Control c in sidebar.Controls)
                {
                    if (c is Button b)
                    {
                        b.ForeColor = textColor;
                        b.BackColor = btnBg;
                        b.FlatStyle = FlatStyle.Flat;
                        b.FlatAppearance.BorderSize = 1;
                        b.FlatAppearance.BorderColor = borderColor;
                        b.UseVisualStyleBackColor = false;
                    }
                }
            }

            // Main area panels/labels/buttons (exclude banner)
            void Recurse(Control c)
            {
                if (c == banner) return; // handle banner separately
                if (c is Panel p) p.BackColor = panelBg;
                if (c is Label lb) lb.ForeColor = textColor;
                if (c is Button b)
                {
                    b.ForeColor = textColor;
                    b.BackColor = btnBg;
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderSize = 1;
                    b.FlatAppearance.BorderColor = borderColor;
                    b.UseVisualStyleBackColor = false;
                }
                foreach (Control child in c.Controls) Recurse(child);
            }
            if (mainPanel != null) Recurse(mainPanel);

            // Header + banner
            if (topHeader != null) topHeader.BackColor = panelBg;
            if (marketStatus != null) marketStatus.ForeColor = isDarkMode ? Color.LightGreen : Color.Green;
            if (balanceLabel != null) balanceLabel.ForeColor = textColor;

            if (banner != null)
            {
                banner.BackColor = isDarkMode ? Color.FromArgb(40, 44, 52) : Color.FromArgb(29, 39, 55);
                if (bannerText != null) bannerText.ForeColor = Color.White;
            }

            // Chart transparency + axes theme
            if (chart != null)
            {
                chart.BackColor = Color.Transparent;
                ApplyChartAxesTheme();
            }

            if (btnTheme != null) btnTheme.Text = isDarkMode ? "☀️" : "🌙";

            // Version badge stays subtle
            if (versionBadge != null)
                versionBadge.ForeColor = isDarkMode ? Color.Gray : Color.DimGray;
        }
    }
}
