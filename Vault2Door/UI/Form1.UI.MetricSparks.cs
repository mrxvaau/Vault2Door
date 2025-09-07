using System;
using System.Drawing;
using System.Windows.Forms;
using Vault2Door.UI; // SparklineControl

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        // Builds four metric cards with polished, fully transparent sparklines (dummy fixed data).
        private void BuildMetricCards()
        {
            metricsTable = new TableLayoutPanel
            {
                Location = new Point(0, 192),
                Size = new Size(mainPanel.Width, 120),
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            for (int i = 0; i < 4; i++) metricsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            metricsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            mainPanel.Controls.Add(metricsTable);

            // Fixed, curated dummy series (pleasant waves; not real data)
            double[] s1 = { 10.2, 10.6, 11.1, 10.9, 11.6, 11.9, 12.1, 12.0, 12.3, 12.7 };
            double[] s2 = { 8.5, 8.42, 8.38, 8.33, 8.30, 8.28, 8.31, 8.26, 8.24, 8.22 };
            double[] s3 = { 14.1, 14.6, 14.9, 15.0, 15.3, 15.6, 15.9, 16.0, 16.15, 16.4 };
            double[] s4 = { 0.10, 0.22, 0.35, 0.28, 0.46, 0.62, 0.58, 0.74, 0.92, 1.05 };

            var cards = new (string title, string sub, double[] vals, bool negative)[]
            {
                ("Total Portfolio:$24,750.00", "+1,250.50 (+5.3%)", s1, false),
                ("Cash Balance:$8,420.00", "-180.00 (-2.1%)", s2, true),
                ("Holdings Value:$16,330.00", "+1,430.50 (+9.6%)", s3, false),
                ("Today's P&L:+342.80 (+1.4%)", "", s4, false),
            };

            for (int i = 0; i < 4; i++)
            {
                var card = CreateMetricCard(cards[i].title, cards[i].sub, cards[i].vals, cards[i].negative);
                card.Dock = DockStyle.Fill;
                card.Margin = new Padding(20, 0, 20, 0);
                metricsTable.Controls.Add(card, i, 0);
            }
        }

        private Panel CreateMetricCard(string title, string sub, double[] values, bool negative)
        {
            var panelBg = isDarkMode ? Color.FromArgb(28, 31, 36) : Color.White;
            var border   = isDarkMode ? Color.FromArgb(60, 66, 78) : Color.FromArgb(210, 214, 220);
            var text     = isDarkMode ? Color.Gainsboro : Color.FromArgb(30, 30, 30);

            var card = new Panel
            {
                BackColor = panelBg,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lbl = new Label
            {
                Text = string.IsNullOrWhiteSpace(sub) ? title : (title + "\n" + sub),
                ForeColor = text,
                AutoSize = false,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(240, 40),
                Location = new Point(10, 8)
            };
            card.Controls.Add(lbl);

            // Sparkline (transparent, no packages)
            var lineColor = negative ? Color.FromArgb(220, 77, 65) : Color.FromArgb(58, 130, 247);
            var fillTop   = Color.FromArgb(isDarkMode ? 64 : 48, lineColor);
            var spark = new SparklineControl
            {
                Values = values,
                Location = new Point(10, 56),
                Size = new Size(240, 48),
                LineColor = lineColor,
                FillTop = fillTop,
                FillBottom = Color.FromArgb(0, lineColor),
                LineThickness = 3f,
                Smoothness = 0.65f,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };
            card.Controls.Add(spark);

            // Subtle 1px border
            card.Padding = new Padding(0);
            card.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(border), new Rectangle(0, 0, card.Width - 1, card.Height - 1));

            return card;
        }
    }
}
