using System.Drawing;
namespace Vault2Door
{
    public partial class Form1
    {
        private void AlignVerticalLayout()
        {
            if (mainPanel == null || metricsTable == null) return;
            metricsTable.Width = mainPanel.Width - 40;
            int belowMetrics = metricsTable.Bottom + 10;
            if (assetsTitle != null) assetsTitle.Location = new Point(10, belowMetrics);
            if (contentRow != null)
            {
                int top = (assetsTitle?.Bottom ?? belowMetrics + 28) + 4;
                contentRow.Location = new Point(0, top);
                contentRow.Size = new Size(mainPanel.Width - 40, mainPanel.Height - top - 20);
            }
            if (assetListPanel != null && graphPanel != null && contentRow != null)
            {
                assetListPanel.Location = new Point(0, 0);
                assetListPanel.Height = contentRow.Height;
                graphPanel.Location = new Point(assetListPanel.Right + 12, 0);
                graphPanel.Size = new Size(contentRow.Width - (assetListPanel.Width + 12), contentRow.Height);
            }
        }
    }
}
