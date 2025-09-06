using System;
using System.Threading;
using System.Threading.Tasks;
using Vault2Door.Data;
using Vault2Door.Models;

namespace Vault2Door.Services
{
    public sealed class RealtimeService
    {
        private readonly IPriceFeed _av = new AlphaVantageEquityFeed();
        private readonly IPriceFeed _yahoo = new YahooFinanceFeed();
        private readonly IPriceFeed _sim = new SimulatedFeed();

        // Map each asset to AV symbol and Yahoo symbol (ETFs as proxies)
        private static (string av, string yh) MapSymbols(AssetKind a) => a switch
        {
            AssetKind.Gold   => ("GLD", "GLD"),
            AssetKind.Silver => ("SLV", "SLV"),
            AssetKind.Bronze => ("CPER", "CPER"), // copper ETF proxy
            AssetKind.Diamond=> ("", ""), // no AV/Yahoo free realtime - use sim
            _ => ("", "")
        };

        public async Task<SeriesResult> GetSeriesAsync(AssetKind a, CancellationToken ct)
        {
            var (av, yh) = MapSymbols(a);
            if (a == AssetKind.Diamond) return await _sim.GetSeriesAsync("DIAMOND_SIM", ct);

            Exception? last = null;

            if (AppConfig.UseAlphaVantage && !string.IsNullOrWhiteSpace(AppConfig.AlphaVantageApiKey) && !string.IsNullOrWhiteSpace(av))
            {
                try { return await _av.GetSeriesAsync(av, ct); }
                catch (Exception ex) { last = ex; /* try Yahoo */ }
            }

            if (!string.IsNullOrWhiteSpace(yh))
            {
                try { return await _yahoo.GetSeriesAsync(yh, ct); }
                catch (Exception ex) { last = ex; }
            }

            // fallback to simulated if both fail
            var sim = await _sim.GetSeriesAsync(a.ToString().ToUpperInvariant() + "_SIM", ct);
            if (last != null) return new SeriesResult(sim.Points, "Simulated (after error: " + last.Message + ")", sim.Symbol);
            return sim;
        }
    }
}
