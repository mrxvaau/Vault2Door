using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Vault2Door.Models;

namespace Vault2Door.Data
{
    /// <summary>
    /// Alpha Vantage TIME_SERIES_INTRADAY for equities/ETFs (e.g., GLD, SLV, CPER).
    /// Free key: https://www.alphavantage.co
    /// </summary>
    public sealed class AlphaVantageEquityFeed : IPriceFeed
    {
        private static readonly HttpClient _http = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All });

        public async Task<SeriesResult> GetSeriesAsync(string symbol, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(AppConfig.AlphaVantageApiKey))
                throw new InvalidOperationException("Alpha Vantage API key is missing. Set AppConfig.AlphaVantageApiKey.");

            string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={Uri.EscapeDataString(symbol)}&interval=5min&outputsize=compact&apikey={AppConfig.AlphaVantageApiKey}";
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.UserAgent.ParseAdd("Vault2Door/2.5 (+https://example.local)");
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            resp.EnsureSuccessStatusCode();

            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            // Find "Time Series (5min)"
            JsonElement series = default;
            bool found = false;
            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                if (prop.Name.StartsWith("Time Series (", StringComparison.OrdinalIgnoreCase))
                {
                    series = prop.Value; found = true; break;
                }
            }
            if (!found)
            {
                if (doc.RootElement.TryGetProperty("Information", out var info))
                    throw new InvalidOperationException("Alpha Vantage info: " + info.GetString());
                if (doc.RootElement.TryGetProperty("Note", out var note))
                    throw new InvalidOperationException("Alpha Vantage note: " + note.GetString());
                if (doc.RootElement.TryGetProperty("Error Message", out var err))
                    throw new InvalidOperationException("Alpha Vantage error: " + err.GetString());
                throw new InvalidOperationException("Unexpected Alpha Vantage response for TIME_SERIES_INTRADAY.");
            }

            var points = new List<PricePoint>(120);
            foreach (var kv in series.EnumerateObject())
            {
                if (DateTime.TryParse(kv.Name, out DateTime t))
                {
                    if (kv.Value.TryGetProperty("4. close", out var closeProp) &&
                        double.TryParse(closeProp.GetString(), out double v))
                    {
                        points.Add(new PricePoint(t, v));
                    }
                }
            }
            points.Sort((a, b) => a.Time.CompareTo(b.Time));
            return new SeriesResult(points, "AlphaVantage", symbol);
        }
    }
}
