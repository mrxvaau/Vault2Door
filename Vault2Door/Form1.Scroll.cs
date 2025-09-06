using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        private void AssetViewport_MouseWheel(object? sender, MouseEventArgs e)
        {
            int delta = -Math.Sign(e.Delta) * 60; // scroll speed
            ScrollAssetsBy(delta);
        }

        private void ScrollTrackClick(int clickY)
        {
            // page up/down
            int page = assetListPanel.Height - 40;
            int thumbTop = assetScrollThumb.Top;
            if (clickY < thumbTop) ScrollAssetsBy(-page);
            else if (clickY > thumbTop + assetScrollThumb.Height) ScrollAssetsBy(page);
        }

        private void MoveThumbTo(int newTop)
        {
            int minTop = 0;
            int maxTop = Math.Max(0, assetScrollTrack.Height - assetScrollThumb.Height);
            int clamped = Math.Max(minTop, Math.Min(maxTop, newTop));
            assetScrollThumb.Top = clamped;

            int contentRange = Math.Max(0, assetContent.Height - assetListPanel.Height);
            if (contentRange <= 0) { assetScrollOffset = 0; assetContent.Top = 0; return; }

            float frac = (float)clamped / Math.Max(1, maxTop);
            assetScrollOffset = (int)(frac * contentRange);
            assetContent.Top = -assetScrollOffset;
        }

        private void ScrollAssetsBy(int dy)
        {
            int contentRange = Math.Max(0, assetContent.Height - assetListPanel.Height);
            int newOffset = Math.Max(0, Math.Min(contentRange, assetScrollOffset + dy));
            assetScrollOffset = newOffset;

            assetContent.Top = -assetScrollOffset;

            // sync thumb
            int maxTop = Math.Max(0, assetScrollTrack.Height - assetScrollThumb.Height);
            float frac = contentRange == 0 ? 0 : (float)newOffset / contentRange;
            assetScrollThumb.Top = (int)(frac * maxTop);
        }

        private void UpdateAssetScrollMetrics()
        {
            if (assetListPanel == null || assetScrollTrack == null || assetScrollThumb == null || assetContent == null) return;

            // ensure content width
            assetContent.Width = assetListPanel.Width - assetScrollTrack.Width;

            int contentRange = Math.Max(0, assetContent.Height - assetListPanel.Height);
            if (contentRange <= 0)
            {
                assetScrollThumb.Visible = false;
                assetScrollOffset = 0;
                assetContent.Top = 0;
            }
            else
            {
                assetScrollThumb.Visible = true;
                // thumb size proportional
                float ratio = (float)assetListPanel.Height / assetContent.Height;
                int thumbMin = 24;
                int thumbSize = Math.Max(thumbMin, (int)(assetScrollTrack.Height * ratio));
                assetScrollThumb.Height = Math.Min(thumbSize, assetScrollTrack.Height - 4);
                ScrollAssetsBy(0); // re-sync position
            }
        }
    }
}
