using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        private PictureBox chartBox;
        private string gifPathRoot;
        private bool isDarkMode = false;

        private Panel mainPanel;
        private Panel topHeader;
        private Panel sidebar;
        private Label bannerText;
        private Panel banner;
        private Label balanceLabel, marketStatus;
        private CheckBox toggleSwitch;

        public Form1()
        {
            InitializeComponent();
            this.Text = "PreciousMetals";
            this.Size = new Size(1400, 800);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            gifPathRoot = @"C:\Users\Qlurut\source\repos\PreciousMetalsTradingApp\PreciousMetalsTradingApp\gif\";

            BuildDashboardUI();
        }

        private void BuildDashboardUI()
        {
            // Sidebar
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
                Button btn = new Button
                {
                    Text = text,
                    Location = new Point(10, yOffset),
                    Width = 180,
                    Height = 35,
                    FlatStyle = FlatStyle.Flat
                };
                sidebar.Controls.Add(btn);
                yOffset += 40;
            }

            // Main Panel
            mainPanel = new Panel
            {
                Location = new Point(200, 0),
                Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height),
                BackColor = Color.White,
                Padding = new Padding(20),
                AutoScroll = true
            };
            this.Controls.Add(mainPanel);

            // Top Header
            topHeader = new Panel
            {
                Size = new Size(mainPanel.Width, 50),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            mainPanel.Controls.Add(topHeader);

            marketStatus = new Label
            {
                Text = "Markets Open",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Green
            };
            topHeader.Controls.Add(marketStatus);

            balanceLabel = new Label
            {
                Text = "Balance: $24,750.00",
                Location = new Point(150, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            topHeader.Controls.Add(balanceLabel);

            // Top Right Icons + Dark Mode Toggle
            Label notificationIcon = new Label
            {
                Text = "🔔",
                Font = new Font("Segoe UI", 12),
                Location = new Point(topHeader.Width - 120, 12),
                AutoSize = true
            };
            topHeader.Controls.Add(notificationIcon);

            Label userIcon = new Label
            {
                Text = "👤",
                Font = new Font("Segoe UI", 12),
                Location = new Point(topHeader.Width - 90, 12),
                AutoSize = true
            };
            topHeader.Controls.Add(userIcon);

            toggleSwitch = new CheckBox
            {
                Text = "🌙",
                Font = new Font("Segoe UI", 12),
                Appearance = Appearance.Button,
                Location = new Point(topHeader.Width - 60, 8),
                Width = 40,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };
            toggleSwitch.CheckedChanged += ToggleSwitch_CheckedChanged;
            topHeader.Controls.Add(toggleSwitch);

            // Info Banner
            banner = new Panel
            {
                BackColor = Color.FromArgb(29, 39, 55),
                Size = new Size(mainPanel.Width - 40, 100),
                Location = new Point(0, 60),
                Padding = new Padding(10)
            };
            mainPanel.Controls.Add(banner);

            bannerText = new Label
            {
                Text = "Trade Precious Metals with Confidence\nInvest in gold, silver, diamonds, and bronze with our secure vaulted storage and instant delivery options.",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            banner.Controls.Add(bannerText);

            // Summary Cards
            string[] metrics = {
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
                    Size = new Size(220, 80),
                    Location = new Point(xOffset, 170)
                };
                Label l = new Label
                {
                    Text = metric,
                    Location = new Point(10, 10),
                    Size = new Size(200, 60)
                };
                card.Controls.Add(l);
                mainPanel.Controls.Add(card);
                xOffset += 240;
            }

            // Section Title
            Label assetsTitle = new Label
            {
                Text = "Available Assets",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 270),
                AutoSize = true
            };
            mainPanel.Controls.Add(assetsTitle);

            // Content Row Panel
            Panel contentRow = new Panel
            {
                Location = new Point(0, 300),
                Size = new Size(mainPanel.Width - 40, 400),
                BackColor = Color.White
            };
            mainPanel.Controls.Add(contentRow);

            // Left - Assets
            Panel assetListPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(300, 400),
                BackColor = Color.White
            };
            contentRow.Controls.Add(assetListPanel);

            // Right - Graph
            Panel graphPanel = new Panel
            {
                Location = new Point(310, 0),
                Size = new Size(600, 400),
                BackColor = Color.LightGray
            };
            contentRow.Controls.Add(graphPanel);

            // Chart Placeholder
            chartBox = new PictureBox
            {
                Size = new Size(580, 380),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            graphPanel.Controls.Add(chartBox);

            // Add Asset Cards
            int y = 0;
            CreateAssetCard(assetListPanel, "DIAMOND", "$4,500.00", "+$55.20", Color.LightGreen, 10, y += 10, "diamond.gif");
            CreateAssetCard(assetListPanel, "GOLD (24K)", "$2,048.50", "+$12.80", Color.LightGreen, 10, y += 130, "gold.gif");
            CreateAssetCard(assetListPanel, "SILVER (999)", "$24.85", "-$0.15", Color.LightCoral, 10, y += 130, "silver.gif");
            CreateAssetCard(assetListPanel, "BRONZE", "$15.10", "+$0.50", Color.LightGreen, 10, y += 130, "bronze.gif");

            // Default Chart
            ShowChart("diamond.gif");
        }

        private void CreateAssetCard(Panel parent, string name, string price, string change, Color changeColor, int x, int y, string gifFile)
        {
            Panel card = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(260, 120),
                Location = new Point(x, y)
            };

            Label l1 = new Label { Text = name, Location = new Point(10, 10), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Label l2 = new Label { Text = price, Location = new Point(10, 35), Font = new Font("Segoe UI", 10) };
            Label l3 = new Label { Text = change, Location = new Point(10, 60), ForeColor = changeColor, Font = new Font("Segoe UI", 9) };

            Button buy = new Button { Text = "Buy", Location = new Point(10, 85), Width = 80 };
            Button sell = new Button { Text = "Sell", Location = new Point(100, 85), Width = 80 };

            buy.Click += (s, e) => ShowChart(gifFile);
            sell.Click += (s, e) => ShowChart(gifFile);

            card.Controls.Add(l1);
            card.Controls.Add(l2);
            card.Controls.Add(l3);
            card.Controls.Add(buy);
            card.Controls.Add(sell);

            parent.Controls.Add(card);
        }

        private void ShowChart(string gifFileName)
        {
            string fullPath = Path.Combine(gifPathRoot, gifFileName);
            if (File.Exists(fullPath))
            {
                chartBox.Image = null;
                chartBox.ImageLocation = fullPath;
            }
            else
            {
                chartBox.Image = null;
                MessageBox.Show($"Chart not found: {gifFileName}", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            isDarkMode = toggleSwitch.Checked;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            Color bgColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
            Color fgColor = isDarkMode ? Color.White : Color.Black;

            mainPanel.BackColor = bgColor;
            topHeader.BackColor = bgColor;
            banner.BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.FromArgb(29, 39, 55);
            bannerText.ForeColor = fgColor;
            balanceLabel.ForeColor = fgColor;
            marketStatus.ForeColor = isDarkMode ? Color.LightGreen : Color.Green;
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}
