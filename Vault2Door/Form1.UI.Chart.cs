using System;
using System.Drawing;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;

namespace Vault2Door
{
    public partial class Form1
    {
        private readonly Random _rng = new Random();

        private double[] RandSeries(int n, double start = 10, double vol = 0.8)
        {
            var vals = new double[n];
            double v = start;
            for (int i = 0; i < n; i++)
            {
                v += (_rng.NextDouble() - 0.5) * vol;
                if (v < 0.1) v = 0.1;
                vals[i] = Math.Round(v, 2);
            }
            return vals;
        }

        private void ShowChart(string key)
        {
            if (chart == null) return;
            key = key.ToLowerInvariant();

            int n = 60;

            if (key.Contains("gold"))
            {
                var values = RandSeries(n, 12, 0.6);
                var line = new LineSeries<double>
                {
                    Values = values,
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                    GeometryFill = new SolidColorPaint(new SKColor(255, 255, 255, 120)),
                    LineSmoothness = 0.7,
                    Stroke = new SolidColorPaint(new SKColor(120, 180, 255)) { StrokeThickness = 3 },
                    Fill = new LinearGradientPaint(
                        new[] { new SKColor(255, 215, 0, 80), new SKColor(255, 215, 0, 10) },
                        new SKPoint(0, 0), new SKPoint(0, 1))
                };
                chart.Series = new ISeries[] { line };
            }
            else if (key.Contains("diamond"))
            {
                var values = RandSeries(n, 8, 1.0);
                var line = new LineSeries<double>
                {
                    Values = values,
                    GeometrySize = 10,
                    GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                    GeometryFill = new SolidColorPaint(new SKColor(160, 230, 255, 200)),
                    LineSmoothness = 0.5,
                    Stroke = new SolidColorPaint(new SKColor(135, 206, 250)) { StrokeThickness = 2.4f }
                };
                chart.Series = new ISeries[] { line };
            }
            else if (key.Contains("silver"))
            {
                var values = RandSeries(n, 6, 0.5);
                var line = new LineSeries<double>
                {
                    Values = values,
                    GeometrySize = 6,
                    GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 1.8f },
                    GeometryFill = new SolidColorPaint(new SKColor(230, 230, 240, 180)),
                    LineSmoothness = 0.9,
                    Stroke = new SolidColorPaint(new SKColor(192, 192, 192)) { StrokeThickness = 2.6f },
                    Fill = null
                };
                chart.Series = new ISeries[] { line };
            }
            else if (key.Contains("bronze"))
            {
                var values = RandSeries(24, 7, 1.2);
                var cols = new ColumnSeries<double>
                {
                    Values = values,
                    MaxBarWidth = double.NaN,
                    Fill = new SolidColorPaint(new SKColor(205, 127, 50, 170)),
                    Stroke = new SolidColorPaint(new SKColor(139, 69, 19)) { StrokeThickness = 1.5f }
                };
                chart.Series = new ISeries[] { cols };
            }
            else
            {
                var values = RandSeries(n, 10, 0.7);
                chart.Series = new ISeries[] { new LineSeries<double> { Values = values, GeometrySize = 0 } };
            }

            ApplyChartAxesTheme();
        }

        private void ApplyChartAxesTheme()
        {
            var bg = isDarkMode ? Color.FromArgb(36, 40, 48) : Color.FromArgb(248, 248, 248);
            chart.BackColor = bg;

            var labels = isDarkMode ? SKColors.Gainsboro : new SKColor(60, 60, 60);
            var grid = isDarkMode ? new SKColor(255, 255, 255, 30) : new SKColor(0, 0, 0, 30);

            chart.XAxes = new[] { new Axis { LabelsPaint = new SolidColorPaint(labels), SeparatorsPaint = new SolidColorPaint(grid) { StrokeThickness = 1 }, TicksPaint = new SolidColorPaint(grid) { StrokeThickness = 1 }, TextSize = 12 } };
            chart.YAxes = new[] { new Axis { LabelsPaint = new SolidColorPaint(labels), SeparatorsPaint = new SolidColorPaint(grid) { StrokeThickness = 1 }, TicksPaint = new SolidColorPaint(grid) { StrokeThickness = 1 }, TextSize = 12 } };
        }
    }
}
