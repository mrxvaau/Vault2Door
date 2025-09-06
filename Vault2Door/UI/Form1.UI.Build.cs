using System;
using System.Drawing;
using System.Windows.Forms;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using Vault2Door.Models;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        private void BuildDashboardUI()
        {
            sidebar = new Panel { BackColor = Color.FromArgb(245, 247, 250), Dock = DockStyle.Left, Width = 200 };
            this.Controls.Add(sidebar);
            string[] sidebarButtons = { "Dashboard", "Markets", "Holdings (4)", "Orders", "Payments", "Reports", "KYC Status: Verified", "Settings" };
            int yOffset = 20;
            foreach (string text in sidebarButtons)
            {
                var btn = new Button { Text = text, Location = new Point(10, yOffset), Width = 180, Height = 35, FlatStyle = FlatStyle.Flat };
                btn.FlatAppearance.BorderSize = 1; sidebar.Controls.Add(btn); yOffset += 40;
            }

            mainPanel = new Panel { Location = new Point(200, 0), Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height), BackColor = Color.White, Padding = new Padding(20), AutoScroll = false, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            this.Controls.Add(mainPanel);

            topHeader = new Panel { Size = new Size(mainPanel.Width, 56), Location = new Point(0, 0), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            mainPanel.Controls.Add(topHeader);

            marketStatus = new Label { Text = "Markets Open", Location = new Point(10, 18), AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = Color.Green };
            topHeader.Controls.Add(marketStatus);
            balanceLabel = new Label { Text = "Balance: $24,750.00", Location = new Point(150, 18), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            topHeader.Controls.Add(balanceLabel);

            var rightBar = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 360, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Padding = new Padding(0, 10, 12, 0), BackColor = Color.Transparent };
            topHeader.Controls.Add(rightBar);

            btnTheme = new Button { Text = "🌙", Width = 44, Height = 32, FlatStyle = FlatStyle.Flat, Margin = new Padding(10, 0, 0, 0) };
            btnTheme.FlatAppearance.BorderSize = 1; btnTheme.Click += (s, e) => ToggleTheme(); tip.SetToolTip(btnTheme, "Toggle dark mode (Ctrl + D)");
            rightBar.Controls.Add(btnTheme);

            btnUser = new Button { Text = "👤", Width = 40, Height = 32, FlatStyle = FlatStyle.Flat, Margin = new Padding(8, 0, 0, 0) };
            btnUser.FlatAppearance.BorderSize = 1; btnUser.Click += (s, e) => MessageBox.Show($"{AppName}\nVersion: {AppVersion}\n\n© {DateTime.Now:yyyy} Vault2Door", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            rightBar.Controls.Add(btnUser);

            btnBell = new Button { Text = "🔔", Width = 40, Height = 32, FlatStyle = FlatStyle.Flat, Margin = new Padding(8, 0, 0, 0) };
            btnBell.FlatAppearance.BorderSize = 1; btnBell.Click += (s, e) => MessageBox.Show("No new notifications (stub).");
            rightBar.Controls.Add(btnBell);

            chkRealtime = new CheckBox { Text = "Realtime", AutoSize = true, Margin = new Padding(10, 6, 0, 0) };
            chkRealtime.CheckedChanged += async (s, e) => await ToggleRealtimeAsync();
            tip.SetToolTip(chkRealtime, "Switch between demo data and live market data");
            rightBar.Controls.Add(chkRealtime);

            banner = new Panel { BackColor = Color.FromArgb(29, 39, 55), Size = new Size(mainPanel.Width - 40, 120), Location = new Point(0, 64), Padding = new Padding(12), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            mainPanel.Controls.Add(banner);
            bannerText = new Label { Text = "Trade Precious Metals with Confidence\nInvest in gold, silver, diamonds, and bronze with our secure vaulted storage and instant delivery options.", ForeColor = Color.White, Font = new Font("Segoe UI", 10), AutoSize = true, Location = new Point(10, 12) };
            banner.Controls.Add(bannerText);

            versionBadge = new Label { Text = $"v{AppVersion}", AutoSize = true, ForeColor = Color.Gray, BackColor = Color.Transparent, Anchor = AnchorStyles.Bottom | AnchorStyles.Right, Location = new Point(mainPanel.Width - 80, mainPanel.Height - 30) };
            mainPanel.Controls.Add(versionBadge);

            metricsTable = new TableLayoutPanel { Location = new Point(0, 192), Size = new Size(mainPanel.Width - 40, 100), ColumnCount = 4, RowCount = 1, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.Transparent, Padding = new Padding(0), Margin = new Padding(0) };
            for (int i = 0; i < 4; i++) metricsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            metricsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            mainPanel.Controls.Add(metricsTable);

            string[] metrics = { "Total Portfolio:$24,750.00\n+1,250.50 (+5.3%)", "Cash Balance:$8,420.00\n-180.00 (-2.1%)", "Holdings Value:$16,330.00\n+1,430.50 (+9.6%)", "Today's P&L:+342.80 (+1.4%)" };
            for (int i = 0; i < 4; i++) { var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(18, 0, 18, 0) }; var l = new Label { Text = metrics[i], Location = new Point(10, 10), Size = new Size(1000, 70) }; card.Controls.Add(l); metricsTable.Controls.Add(card, i, 0); }

            assetsTitle = new Label { Text = "Available Assets", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true };
            mainPanel.Controls.Add(assetsTitle);

            contentRow = new Panel { BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            mainPanel.Controls.Add(contentRow);

            assetListPanel = new Panel { BackColor = Color.White, AutoScroll = false, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, Width = 320 };
            contentRow.Controls.Add(assetListPanel);
            assetContent = new Panel { Location = new Point(0, 0), Size = new Size(assetListPanel.Width - 14, assetListPanel.Height), BackColor = Color.White };
            assetListPanel.Controls.Add(assetContent);
            assetScrollTrack = new Panel { Width = 12, Dock = DockStyle.Right, BackColor = Color.FromArgb(230, 230, 230) };
            assetListPanel.Controls.Add(assetScrollTrack);
            assetScrollThumb = new Panel { Width = 10, Height = 80, Left = 1, Top = 0, BackColor = Color.FromArgb(160, 160, 160), Cursor = Cursors.Hand };
            assetScrollTrack.Controls.Add(assetScrollThumb);

            assetListPanel.MouseWheel += AssetViewport_MouseWheel;
            assetScrollTrack.MouseDown += (s, e) => ScrollTrackClick(e.Y);
            assetScrollThumb.MouseDown += (s, e) => { assetThumbDragging = true; assetThumbDragStartY = e.Y; assetThumbStartTop = assetScrollThumb.Top; };
            assetScrollThumb.MouseUp += (s, e) => assetThumbDragging = false;
            assetScrollThumb.MouseMove += (s, e) => { if (!assetThumbDragging) return; MoveThumbTo(assetThumbStartTop + (e.Y - assetThumbDragStartY)); };

            graphPanel = new Panel { BackColor = Color.Transparent, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            contentRow.Controls.Add(graphPanel);
            chart = new CartesianChart { Dock = DockStyle.Fill, BackColor = Color.Transparent, LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden };
            chart.XAxes = new[] { new Axis() }; chart.YAxes = new[] { new Axis() };
            graphPanel.Padding = new Padding(10);
            graphPanel.Controls.Add(chart);

            // provider badge (top-right over chart)
            providerBadge = new Label { Text = "DEMO", AutoSize = true, BackColor = Color.FromArgb(50, 50, 50, 100), ForeColor = Color.White, Padding = new Padding(6, 2, 6, 2) };
            providerBadge.Parent = graphPanel;
            providerBadge.BringToFront();
            providerBadge.Location = new Point(graphPanel.Width - 80, 8);
            providerBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            int y = 0;
            CreateAssetCard(assetContent, "DIAMOND", "$4,500.00", "+$55.20", Color.LightGreen, 10, y += 10, "diamond");
            CreateAssetCard(assetContent, "GOLD (GLD)", "$2,048.50", "+$12.80", Color.LightGreen, 10, y += 140, "gold");
            CreateAssetCard(assetContent, "SILVER (SLV)", "$24.85", "-$0.15", Color.LightCoral, 10, y += 140, "silver");
            CreateAssetCard(assetContent, "BRONZE (CPER)", "$15.10", "+$0.50", Color.LightGreen, 10, y += 140, "bronze");
            assetContent.Height = y + 150;

            AlignVerticalLayout();
            UpdateAssetScrollMetrics();
        }

        private void CreateAssetCard(Panel parent, string name, string price, string change, Color changeColor, int x, int y, string key)
        {
            var card = new Panel { BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle, Size = new Size(290, 130), Location = new Point(x, y), Cursor = Cursors.Hand };
            var l1 = new Label { Text = name, Location = new Point(12, 12), Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true };
            var l2 = new Label { Text = price, Location = new Point(12, 38), Font = new Font("Segoe UI", 10), AutoSize = true };
            var l3 = new Label { Text = change, Location = new Point(12, 64), ForeColor = changeColor, Font = new Font("Segoe UI", 9), AutoSize = true };
            var buy = new Button { Text = "Buy", Location = new Point(12, 92), Width = 90, Height = 28, FlatStyle = FlatStyle.Flat };
            buy.FlatAppearance.BorderSize = 1;
            var sell = new Button { Text = "Sell", Location = new Point(110, 92), Width = 90, Height = 28, FlatStyle = FlatStyle.Flat };
            sell.FlatAppearance.BorderSize = 1;

            void clickHandler(object s, EventArgs e)
            {
                currentAsset = key.Contains("gold") ? AssetKind.Gold : key.Contains("silver") ? AssetKind.Silver : key.Contains("bronze") ? AssetKind.Bronze : AssetKind.Diamond;
                if (realtimeEnabled) _ = LoadRealtimeAsync(); else ShowChart(key);
            }
            card.Click += clickHandler; l1.Click += clickHandler; l2.Click += clickHandler; l3.Click += clickHandler; buy.Click += clickHandler; sell.Click += clickHandler;

            card.Controls.Add(l1); card.Controls.Add(l2); card.Controls.Add(l3); card.Controls.Add(buy); card.Controls.Add(sell);
            parent.Controls.Add(card);
        }
    }
}
