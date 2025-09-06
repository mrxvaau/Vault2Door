using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Vault2Door.Models;

namespace Vault2Door
{
    public partial class Form1
    {
        private async Task ToggleRealtimeAsync()
        {
            if (chkRealtime.Checked)
            {
                if (AppConfig.UseAlphaVantage && string.IsNullOrWhiteSpace(AppConfig.AlphaVantageApiKey))
                {
                    chkRealtime.Checked = false;
                    MessageBox.Show(
                        "Realtime mode can use Alpha Vantage (free key) or Yahoo fallback.\n" +
                        "You did not set an Alpha Vantage key – we will still use Yahoo without a key.\n" +
                        "Tip: set the key in AppConfig.AlphaVantageApiKey for better resiliency.",
                        "No API key",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                realtimeEnabled = true;
                await LoadRealtimeAsync();
                realTimer.Start();
            }
            else
            {
                realtimeEnabled = false;
                realTimer.Stop();
                ShowChart(currentAsset.ToString().ToLowerInvariant());
            }
        }

        private async Task LoadRealtimeAsync()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                var result = await realtime.GetSeriesAsync(currentAsset, cts.Token);
                var vals = result.Points.Select(p => p.Value).ToArray();

                if (currentAsset == AssetKind.Bronze)
                    chart.Series = new ISeries[] { new ColumnSeries<double> { Values = vals, MaxBarWidth = double.NaN } };
                else
                    chart.Series = new ISeries[] { new LineSeries<double> { Values = vals, GeometrySize = 0, LineSmoothness = 0.5 } };

                providerBadge.Text = $"{result.Provider}:{result.Symbol}".ToUpperInvariant();
                ApplyChartAxesTheme();
            }
            catch (Exception ex)
            {
                realTimer.Stop();
                realtimeEnabled = false;
                chkRealtime.Checked = false;
                MessageBox.Show("Failed to load realtime data. Falling back to demo.\n\n" + ex.Message, "Realtime error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                providerBadge.Text = "DEMO";
                ShowChart(currentAsset.ToString().ToLowerInvariant());
            }
        }
    }
}
