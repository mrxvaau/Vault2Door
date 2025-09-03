using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        private Panel graphPanel;

        public Form1()
        {
            InitializeComponent();
            this.Text = "PreciousMetals";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            BuildDashboardUI();
        }

        private void BuildDashboardUI()
        {
            // Sidebar
            Panel sidebar = new Panel
            {
                BackColor = Color.FromArgb(245, 247, 250),
                Dock = DockStyle.Left,
                Width = 200
            };
            this.Controls.Add(sidebar);

            string[] sidebarButtons =
            {
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
            Panel main = new Panel
            {
                Location = new Point(200, 0),
                Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height),
                BackColor = Color.White,
                Padding = new Padding(20),
                AutoScroll = true
            };
            this.Controls.Add(main);

            // Top Header Bar
            Panel topHeader = new Panel
            {
                Size = new Size(main.Width, 40),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            main.Controls.Add(topHeader);

            Label marketStatus = new Label
            {
                Text = "Markets Open",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Green
            };
            topHeader.Controls.Add(marketStatus);

            Label balanceLabel = new Label
            {
                Text = "Balance: $24,750.00",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(150, 10)
            };
            topHeader.Controls.Add(balanceLabel);

            // Info Banner
            Panel banner = new Panel
            {
                BackColor = Color.FromArgb(29, 39, 55),
                Size = new Size(main.Width - 40, 100),
                Location = new Point(0, 50),
                Padding = new Padding(10)
            };
            main.Controls.Add(banner);

            Label bannerText = new Label
            {
                Text = "Trade Precious Metals with Confidence\nInvest in gold, silver, diamonds, and bronze with our secure vaulted storage and instant delivery options.",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            banner.Controls.Add(bannerText);

            // Summary Cards Row
            int xOffset = 0;
            string[] metrics =
            {
                "Total Portfolio:$24,750.00\n+1,250.50 (+5.3%)",
                "Cash Balance:$8,420.00\n-180.00 (-2.1%)",
                "Holdings Value:$16,330.00\n+1,430.50 (+9.6%)",
                "Today's P&L:+342.80 (+1.4%)"
            };
            for (int i = 0; i < metrics.Length; i++)
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
                    Text = metrics[i],
                    Location = new Point(10, 10),
                    Size = new Size(200, 60)
                };
                card.Controls.Add(l);
                main.Controls.Add(card);
                xOffset += 240;
            }

            // Title for Assets Section
            Label assetsTitle = new Label
            {
                Text = "Available Assets",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 270),
                AutoSize = true
            };
            main.Controls.Add(assetsTitle);

            // Layout Panels
            Panel contentRow = new Panel
            {
                Location = new Point(0, 300),
                Size = new Size(main.Width - 40, 500),
                BackColor = Color.White
            };
            main.Controls.Add(contentRow);

            // Left Panel - Assets
            Panel assetListPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(300, 500),
                BackColor = Color.White
            };
            contentRow.Controls.Add(assetListPanel);

            // Right Panel - Graph
            graphPanel = new Panel
            {
                Name = "graphPanel",
                Location = new Point(310, 0),
                Size = new Size(800, 500),
                BackColor = Color.LightGray
            };
            contentRow.Controls.Add(graphPanel);

            // Add Asset Cards Vertically with Click Handlers
            int y = 0;
            CreateAssetCard(assetListPanel, "DIAMOND", "$4,500.00", "+$55.20", Color.LightGreen, 10, y += 10, () => ShowGraph("diamond"));
            CreateAssetCard(assetListPanel, "GOLD", "$2,048.50", "+$12.80", Color.LightGreen, 10, y += 130, () => ShowGraph("gold"));
            CreateAssetCard(assetListPanel, "SILVER", "$24.85", "-$0.15", Color.LightCoral, 10, y += 130, () => ShowGraph("silver"));
            CreateAssetCard(assetListPanel, "BRONZE", "$15.10", "+$0.50", Color.LightGreen, 10, y += 130, () => ShowGraph("bronze"));

            // Show initial graph
            ShowGraph("diamond");
        }

        private void CreateAssetCard(Panel parent, string name, string price, string change, Color changeColor, int x, int y, Action onClick)
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

            l1.Click += (s, e) => onClick();
            card.Click += (s, e) => onClick();

            card.Controls.Add(l1);
            card.Controls.Add(l2);
            card.Controls.Add(l3);
            card.Controls.Add(buy);
            card.Controls.Add(sell);

            parent.Controls.Add(card);
        }

        private void ShowGraph(string assetName)
        {
            graphPanel.Controls.Clear();

            Label title = new Label
            {
                Text = $"{assetName.ToUpper()} Price Trend",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            graphPanel.Controls.Add(title);

            string path = $@"C:\Users\Qlurut\source\repos\PreciousMetalsTradingApp\PreciousMetalsTradingApp\gif\{assetName}.gif";

            if (File.Exists(path))
            {
                PictureBox graphBox = new PictureBox
                {
                    Size = new Size(750, 400),
                    Location = new Point(10, 40),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = Image.FromFile(path)
                };
                graphPanel.Controls.Add(graphBox);
            }
            else
            {
                Label error = new Label
                {
                    Text = $"Graph image not found: {assetName}.gif",
                    ForeColor = Color.Red,
                    Location = new Point(10, 40),
                    AutoSize = true
                };
                graphPanel.Controls.Add(error);
            }
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}
