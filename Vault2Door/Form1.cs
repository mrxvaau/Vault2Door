using System;
using System.Drawing;
using System.Windows.Forms;
//
namespace Vault2Door
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BuildDashboardUI();
        }

        private void BuildDashboardUI()
        {
            this.Text = "PreciousMetals";
            this.Size = new Size(1366, 768);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Sidebar
            Panel sidebar = new Panel
            {
                BackColor = Color.FromArgb(245, 247, 250),
                Dock = DockStyle.Left,
                Width = 200
            };
            this.Controls.Add(sidebar);

            string[] sidebarButtons = {
                "Dashboard", "Markets", "Holdings (4)", "Orders", "Payments", "Reports", "KYC Status: Verified", "Settings"
            };

            int yOffset = 20;
            foreach (string text in sidebarButtons)
            {
                Button btn = new Button
                {
                    Text = text,
                    Location = new Point(10, yOffset),
                    Width = 180,
                    Height = 35
                };
                sidebar.Controls.Add(btn);
                yOffset += 40;
            }

            // Main Panel
            Panel main = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            this.Controls.Add(main);

            Label header = new Label
            {
                Text = "Markets Open     Balance: $24,750.00",
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 10)
            };
            main.Controls.Add(header);

            // Info banner
            Panel banner = new Panel
            {
                BackColor = Color.FromArgb(29, 39, 55),
                Size = new Size(1000, 100),
                Location = new Point(10, 50)
            };
            main.Controls.Add(banner);

            Label bannerText = new Label
            {
                Text = "Trade Precious Metals with Confidence\nInvest in gold, silver, diamonds, and bronze with our secure vaulted storage and instant delivery options.",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(10, 10),
                Size = new Size(980, 80)
            };
            banner.Controls.Add(bannerText);

            // Summary Boxes
            string[] metrics = {
                "Total Portfolio:$24,750.00\n+1,250.50 (+5.3%)",
                "Cash Balance:$8,420.00\n-180.00 (-2.1%)",
                "Holdings Value:$16,330.00\n+1,430.50 (+9.6%)",
                "Today's P&L:+342.80 (+1.4%)"
            };

            int xOffset = 10;
            for (int i = 0; i < metrics.Length; i++)
            {
                GroupBox box = new GroupBox
                {
                    Text = "",
                    Size = new Size(220, 80),
                    Location = new Point(xOffset, 160)
                };
                Label l = new Label
                {
                    Text = metrics[i],
                    Location = new Point(10, 20),
                    Size = new Size(200, 50)
                };
                box.Controls.Add(l);
                main.Controls.Add(box);
                xOffset += 240;
            }

            // Assets
            Label assetsTitle = new Label
            {
                Text = "Available Assets",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 260),
                AutoSize = true
            };
            main.Controls.Add(assetsTitle);

            CreateAssetCard(main, "GOLD (24K)", "$2,048.50", "+$12.80 (+0.63%)", Color.LightGreen, 10, 290);
            CreateAssetCard(main, "SILVER (999)", "$24.85", "-$0.15 (-0.60%)", Color.LightCoral, 260, 290);
        }

        private void CreateAssetCard(Panel parent, string name, string price, string change, Color changeColor, int x, int y)
        {
            Panel card = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(230, 120),
                Location = new Point(x, y)
            };

            Label l1 = new Label { Text = name, Location = new Point(10, 10), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Label l2 = new Label { Text = price, Location = new Point(10, 35), Font = new Font("Segoe UI", 10) };
            Label l3 = new Label { Text = change, Location = new Point(10, 60), ForeColor = changeColor, Font = new Font("Segoe UI", 9) };

            Button buy = new Button { Text = "Buy", Location = new Point(10, 85), Width = 80 };
            Button sell = new Button { Text = "Sell", Location = new Point(100, 85), Width = 80 };

            card.Controls.Add(l1);
            card.Controls.Add(l2);
            card.Controls.Add(l3);
            card.Controls.Add(buy);
            card.Controls.Add(sell);

            parent.Controls.Add(card);
        }
    }
}
