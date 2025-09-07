using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        // Overlay pages (fill mainPanel). Dashboard is the base layer (existing dashboard controls).
        private Panel pageMarkets, pageHoldings, pageOrders, pagePayments, pageReports, pageKyc, pageSettings;

        private void InitializeNavigation()
        {
            if (mainPanel == null || sidebar == null) return;

            BuildPages();
            WireSidebarButtons();
            NavigateTo("Dashboard");
        }

        private void BuildPages()
        {
            pageMarkets  = CreatePlaceholderPage("Markets");
            pageHoldings = CreatePlaceholderPage("Holdings");
            pageOrders   = CreatePlaceholderPage("Orders");
            pagePayments = CreatePlaceholderPage("Payments");
            pageReports  = CreatePlaceholderPage("Reports");
            pageKyc      = CreatePlaceholderPage("KYC Status");
            pageSettings = CreatePlaceholderPage("Settings");

            foreach (var p in new[] { pageMarkets, pageHoldings, pageOrders, pagePayments, pageReports, pageKyc, pageSettings })
            {
                p.Visible = false;
                // (no Dock) host uses explicit Location/Size to avoid covering header/banner
                mainPanel.Controls.Add(p);
                p.BringToFront();
            }
        }

        private Panel CreatePlaceholderPage(string title)
        {
            // keep top header & banner visible; small trim from top; small side gutters
            int topOffset = 0;
            if (topHeader != null) topOffset = Math.Max(topOffset, topHeader.Bottom);
            if (banner != null)    topOffset = Math.Max(topOffset, banner.Bottom);
            int trimTop = 3;
            int sideMargin = 6;

            var bg   = isDarkMode ? Color.FromArgb(24, 27, 32) : Color.White;
            var text = isDarkMode ? Color.Gainsboro : Color.FromArgb(25, 25, 25);

            var host = new Panel
            {
                BackColor = bg,
                Location  = new Point(0, topOffset + trimTop),
                Size      = new Size(mainPanel.Width, Math.Max(0, mainPanel.Height - topOffset - trimTop)),
                Anchor    = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            };

            var content = new Panel
            {
                BackColor = Color.Transparent,
                Location  = new Point(sideMargin, 0),
                Size      = new Size(host.Width - sideMargin * 2, host.Height),
                Anchor    = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            };
            host.Controls.Add(content);

            var lbl = new Label
            {
                Text = title + " — page",
                AutoSize = true,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = text,
                Location = new Point(8, 8)
            };
            content.Controls.Add(lbl);

            return host;
        }

        private void WireSidebarButtons()
        {
            foreach (var b in sidebar.Controls.OfType<Button>())
            {
                var t = (b.Text ?? string.Empty).Trim();
                string key = t;
                if (t.StartsWith("Holdings")) key = "Holdings";
                if (t.StartsWith("KYC"))      key = "KYC Status";

                b.Click -= SidebarButton_Click;
                b.Click += SidebarButton_Click;
                b.Tag = key;
            }
        }

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            if (sender is Button b && b.Tag is string key)
                NavigateTo(key);
        }

        private void NavigateTo(string key)
        {
            // Hide overlay pages first
            foreach (var p in new[] { pageMarkets, pageHoldings, pageOrders, pagePayments, pageReports, pageKyc, pageSettings })
                p.Visible = false;

            // Highlight sidebar
            foreach (var b in sidebar.Controls.OfType<Button>())
            {
                bool active = (b.Tag as string) == key || (key == "Dashboard" && (b.Tag as string) == null && b.Text.StartsWith("Dashboard"));
                b.BackColor = active
                    ? (isDarkMode ? Color.FromArgb(36, 40, 48) : Color.FromArgb(235, 238, 242))
                    : (isDarkMode ? Color.FromArgb(28, 31, 36) : Color.FromArgb(250, 251, 252));
            }

            // Select page
            Panel target = null;
            switch (key)
            {
                case "Markets":     target = pageMarkets; break;
                case "Holdings":    target = pageHoldings; break;
                case "Orders":      target = pageOrders; break;
                case "Payments":    target = pagePayments; break;
                case "Reports":     target = pageReports; break;
                case "KYC Status":  target = pageKyc; break;
                case "Settings":    target = pageSettings; break;
                default: target = null; break; // Dashboard
            }

            if (target != null)
            {
                target.BackColor = isDarkMode ? Color.FromArgb(24, 27, 32) : Color.White;
                target.Visible = true;
                target.BringToFront();
            }
        }
    }
}
