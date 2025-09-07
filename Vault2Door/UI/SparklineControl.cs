using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Vault2Door.UI
{
    /// <summary>
    /// Lightweight sparkline renderer with true transparent background (double-buffered).
    /// Great for small KPI cards. No external packages required.
    /// </summary>
    public class SparklineControl : Control
    {
        private double[] _values = Array.Empty<double>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double[] Values
        {
            get => _values;
            set { _values = value ?? Array.Empty<double>(); Invalidate(); }
        }

        public float LineThickness { get; set; } = 3f;
        public float Smoothness    { get; set; } = 0.65f; // 0..1 used as tension in DrawCurve
        public Color LineColor     { get; set; } = Color.RoyalBlue;
        public Color FillTop       { get; set; } = Color.FromArgb(64, Color.RoyalBlue);
        public Color FillBottom    { get; set; } = Color.Transparent;
        public Padding PlotPadding { get; set; } = new Padding(8, 6, 8, 6);

        public SparklineControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_values == null || _values.Length < 2)
            {
                // draw a subtle baseline when no data
                using var pen = new Pen(Color.FromArgb(40, ForeColor), 1f);
                int y = Height - PlotPadding.Bottom - 1;
                g.DrawLine(pen, PlotPadding.Left, y, Width - PlotPadding.Right, y);
                return;
            }

            var rect = Rectangle.Inflate(new Rectangle(Point.Empty, Size), -PlotPadding.Left - PlotPadding.Right, -PlotPadding.Top - PlotPadding.Bottom);
            rect.X = PlotPadding.Left; rect.Y = PlotPadding.Top; rect.Width = Width - PlotPadding.Left - PlotPadding.Right; rect.Height = Height - PlotPadding.Top - PlotPadding.Bottom;
            if (rect.Width <= 2 || rect.Height <= 2) return;

            double min = _values.Min();
            double max = _values.Max();
            if (Math.Abs(max - min) < 1e-6) { max = min + 1.0; } // avoid div by zero

            // Map values to points
            var n = _values.Length;
            PointF[] pts = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                float x = rect.Left + (rect.Width * i) / Math.Max(1, n - 1f);
                float t = (float)((_values[i] - min) / (max - min));
                float y = rect.Bottom - t * rect.Height;
                pts[i] = new PointF(x, y);
            }

            // Fill area under curve (construct polygon: pts + baseline)
            using (var path = new GraphicsPath())
            {
                // smooth curve path
                path.AddCurve(pts, Smoothness);
                // close to baseline
                path.AddLine(pts[^1], new PointF(pts[^1].X, rect.Bottom));
                path.AddLine(new PointF(pts[^1].X, rect.Bottom), new PointF(pts[0].X, rect.Bottom));
                path.AddLine(new PointF(pts[0].X, rect.Bottom), pts[0]);
                using var lg = new LinearGradientBrush(rect, FillTop, FillBottom, LinearGradientMode.Vertical);
                g.FillPath(lg, path);
            }

            // Stroke
            using var pen2 = new Pen(LineColor, LineThickness) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawCurve(pen2, pts, Smoothness);
        }
    }
}
