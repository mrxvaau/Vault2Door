using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        private void BuildDashboardUI()
        {
            // ===== Sidebar =====
            sidebar = new Panel
            {
                BackColor = Color.FromArgb(245, 247, 250),
                Dock = DockStyle.Left,
                Width = 200
            };
            this.Controls.Add(sidebar);

            string[] sidebarButtons = {
                "Dashboard", "Markets", "Holdings (4)", "Orders",
                "Payments", "Reports", "KYC Status: Verified", "Settings"
            };

            int yOffset = 20;
            foreach (string text in sidebarButtons)
            {
                var btn = new Button
                {
                    Text = text,
                    Location = new Point(10, yOffset),
                    Width = 180,
                    Height = 35,
                    FlatStyle = FlatStyle.Flat
                };
                btn.FlatAppearance.BorderSize = 1;
                sidebar.Controls.Add(btn);
                yOffset += 40;
            }

            // ===== Main panel =====
            mainPanel = new Panel
            {
                Location = new Point(200, 0),
                Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height),
                BackColor = Color.White,
                Padding = new Padding(20),
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            this.Controls.Add(mainPanel);

            // ===== Top header =====
            topHeader = new Panel
            {
                Size = new Size(mainPanel.Width, 56),
                Location = new Point(0, 0),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            mainPanel.Controls.Add(topHeader);

            marketStatus = new Label
            {
                Text = "Markets Open",
                Location = new Point(10, 18),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Green
            };
            topHeader.Controls.Add(marketStatus);

            balanceLabel = new Label
            {
                Text = "Balance: $24,750.00",
                Location = new Point(150, 18),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            topHeader.Controls.Add(balanceLabel);

            // Right header controls (theme toggle, user, bell)
            var rightBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 200,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 10, 12, 0),
                BackColor = Color.Transparent
            };
            topHeader.Controls.Add(rightBar);

            btnTheme = new Button
            {
                Text = "🌙", // will flip to ☀️ when dark
                Width = 44,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10, 0, 0, 0)
            };
            btnTheme.FlatAppearance.BorderSize = 1;
            btnTheme.Click += (s, e) => ToggleTheme();
            tip.SetToolTip(btnTheme, "Toggle dark mode (Ctrl + D)");
            rightBar.Controls.Add(btnTheme);

            btnUser = new Button
            {
                Text = "👤",
                Width = 40,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(8, 0, 0, 0)
            };
            btnUser.FlatAppearance.BorderSize = 1;
            btnUser.Click += (s, e) =>
            {
                MessageBox.Show(
                    $"{AppName}\nVersion: {AppVersion}\n\n© {DateTime.Now:yyyy} Vault2Door",
                    "About",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            };
            tip.SetToolTip(btnUser, "About");
            rightBar.Controls.Add(btnUser);

            btnBell = new Button
            {
                Text = "🔔",
                Width = 40,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(8, 0, 0, 0)
            };
            btnBell.FlatAppearance.BorderSize = 1;
            btnBell.Click += (s, e) => MessageBox.Show("No new notifications (stub).");
            tip.SetToolTip(btnBell, "Notifications");
            rightBar.Controls.Add(btnBell);

            // ===== Banner =====
            banner = new Panel
            {
                BackColor = Color.FromArgb(29, 39, 55),
                Size = new Size(mainPanel.Width - 40, 120),
                Location = new Point(0, 64),
                Padding = new Padding(12),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            mainPanel.Controls.Add(banner);

            bannerText = new Label
            {
                Text = "Trade Precious Metals with Confidence\nInvest in gold, silver, diamonds, and bronze with our secure vaulted storage and instant delivery options.",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(10, 12)
            };
            banner.Controls.Add(bannerText);

            // Version badge (bottom-right of main)
            versionBadge = new Label
            {
                Text = $"v{AppVersion}",
                AutoSize = true,
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(mainPanel.Width - 80, mainPanel.Height - 30)
            };
            mainPanel.Controls.Add(versionBadge);

            // ===== Summary cards =====
            string[] metrics =
            {
                "Total Portfolio:$24,750.00\n+1,250.50 (+5.3%)",
                "Cash Balance:$8,420.00\n-180.00 (-2.1%)",
                "Holdings Value:$16,330.00\n+1,430.50 (+9.6%)",
                "Today's P&L:+342.80 (+1.4%)"
            };

            int xOffset = 0;
            foreach (string metric in metrics)
            {
                Panel card = new Panel
                {
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Size = new Size(260, 90),
                    Location = new Point(xOffset, 196)
                };
                var l = new Label
                {
                    Text = metric,
                    Location = new Point(10, 10),
                    Size = new Size(240, 70)
                };
                card.Controls.Add(l);
                mainPanel.Controls.Add(card);
                xOffset += 280;
            }

            // ===== Assets title =====
            var assetsTitle = new Label
            {
                Text = "Available Assets",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 302),
                AutoSize = true
            };
            mainPanel.Controls.Add(assetsTitle);

            // ===== Content row (assets + graph) =====
            contentRow = new Panel
            {
                Location = new Point(0, 334),
                Size = new Size(mainPanel.Width - 40, 560),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            mainPanel.Controls.Add(contentRow);

            // Left (assets) with scroll
            assetListPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(320, contentRow.Height),
                BackColor = Color.White,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom
            };
            contentRow.Controls.Add(assetListPanel);

            // Right (graph) - fills space
            graphPanel = new Panel
            {
                Location = new Point(assetListPanel.Right + 12, 0),
                Size = new Size(contentRow.Width - assetListPanel.Width - 12, contentRow.Height),
                BackColor = Color.LightGray,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            contentRow.Controls.Add(graphPanel);

            chartBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.LightGray,
                Margin = new Padding(0)
            };
            graphPanel.Padding = new Padding(10);
            graphPanel.Controls.Add(chartBox);

            // Asset cards (clickable to switch GIF)
            int y = 0;
            CreateAssetCard(assetListPanel, "DIAMOND", "$4,500.00", "+$55.20", Color.LightGreen, 10, y += 10, "diamond.gif");
            CreateAssetCard(assetListPanel, "GOLD (24K)", "$2,048.50", "+$12.80", Color.LightGreen, 10, y += 140, "gold.gif");
            CreateAssetCard(assetListPanel, "SILVER (999)", "$24.85", "-$0.15", Color.LightCoral, 10, y += 140, "silver.gif");
            CreateAssetCard(assetListPanel, "BRONZE", "$15.10", "+$0.50", Color.LightGreen, 10, y += 140, "bronze.gif");

            // Default chart
            ShowChart("gold.gif");
        }

        private void CreateAssetCard(Panel parent, string name, string price, string change, Color changeColor, int x, int y, string gifFile)
        {
            var card = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(290, 130),
                Location = new Point(x, y),
                Cursor = Cursors.Hand
            };

            var l1 = new Label { Text = name, Location = new Point(12, 12), Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true };
            var l2 = new Label { Text = price, Location = new Point(12, 38), Font = new Font("Segoe UI", 10), AutoSize = true };
            var l3 = new Label { Text = change, Location = new Point(12, 64), ForeColor = changeColor, Font = new Font("Segoe UI", 9), AutoSize = true };

            var buy = new Button { Text = "Buy", Location = new Point(12, 92), Width = 90, Height = 28, FlatStyle = FlatStyle.Flat };
            buy.FlatAppearance.BorderSize = 1;
            var sell = new Button { Text = "Sell", Location = new Point(110, 92), Width = 90, Height = 28, FlatStyle = FlatStyle.Flat };
            sell.FlatAppearance.BorderSize = 1;

            void clickHandler(object s, EventArgs e) => ShowChart(gifFile);
            card.Click += clickHandler; l1.Click += clickHandler; l2.Click += clickHandler; l3.Click += clickHandler;
            buy.Click += clickHandler; sell.Click += clickHandler;

            card.Controls.Add(l1);
            card.Controls.Add(l2);
            card.Controls.Add(l3);
            card.Controls.Add(buy);
            card.Controls.Add(sell);

            parent.Controls.Add(card);
        }
    }
}
