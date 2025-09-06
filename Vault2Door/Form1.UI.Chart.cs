using System;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Measure;

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

            // size of data
            int n = 60;

            if (key.Contains("gold"))
            {
                // GOLD: glowing area
                var values = RandSeries(n, 12, 0.6);
                var line = new LineSeries<double>
                {
                    Values = values,
                    GeometrySize = 0,
                    LineSmoothness = 0.7,
                    Stroke = new SolidColorPaint(new SKColor(218, 165, 32)) { StrokeThickness = 3 },
                    Fill = new LinearGradientPaint(
                        new[] {
                            new SKColor(255, 215, 0, 80),
                            new SKColor(255, 215, 0, 10)
                        },
                        new SKPoint(0, 0),
                        new SKPoint(0, 1))
                };
                chart.Series = new ISeries[] { line };
            }
            else if (key.Contains("diamond"))
            {
                // DIAMOND: crisp line + diamond markers (sparkle vibe)
                var values = RandSeries(n, 8, 1.0);
                var line = new LineSeries<double>
                {
                    Values = values,
                    GeometrySize = 10,
                    GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                    GeometryFill = new SolidColorPaint(new SKColor(135, 206, 250, 180)),
                    LineSmoothness = 0.5,
                    Stroke = new SolidColorPaint(new SKColor(135, 206, 250)) { StrokeThickness = 2.4f }
                };
                chart.Series = new ISeries[] { line };
            }
            else if (key.Contains("silver"))
            {
                // SILVER: smooth silver line
                var values = RandSeries(n, 6, 0.5);
                var line = new LineSeries<double>
                {
                    Values = values,
                    GeometrySize = 0,
                    LineSmoothness = 0.9,
                    Stroke = new SolidColorPaint(new SKColor(192, 192, 192)) { StrokeThickness = 2.6f },
                    Fill = null
                };
                chart.Series = new ISeries[] { line };
            }
            else if (key.Contains("bronze"))
            {
                // BRONZE: bronze columns
                var values = RandSeries(24, 7, 1.2);
                var cols = new ColumnSeries<double>
                {
                    Values = values,
                    MaxBarWidth = double.NaN, // responsive width
                    Fill = new SolidColorPaint(new SKColor(205, 127, 50, 170)),
                    Stroke = new SolidColorPaint(new SKColor(139, 69, 19)) { StrokeThickness = 1.5f }
                };
                chart.Series = new ISeries[] { cols };
            }
            else
            {
                // fallback
                var values = RandSeries(n, 10, 0.7);
                chart.Series = new ISeries[] { new LineSeries<double> { Values = values, GeometrySize = 0 } };
            }

            // Apply theme to axes each time
            ApplyChartAxesTheme();
        }

        private void ApplyChartAxesTheme()
        {
            var labels = isDarkMode ? SKColors.Gainsboro : SKColors.DimGray;
            var grid = isDarkMode ? new SKColor(255, 255, 255, 30) : new SKColor(0, 0, 0, 30);

            chart.XAxes = new[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(labels),
                    SeparatorsPaint = new SolidColorPaint(grid) { StrokeThickness = 1 },
                    TicksPaint = new SolidColorPaint(grid) { StrokeThickness = 1 },
                    TextSize = 12
                }
            };
            chart.YAxes = new[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(labels),
                    SeparatorsPaint = new SolidColorPaint(grid) { StrokeThickness = 1 },
                    TicksPaint = new SolidColorPaint(grid) { StrokeThickness = 1 },
                    TextSize = 12
                }
            };
        }
    }
}
