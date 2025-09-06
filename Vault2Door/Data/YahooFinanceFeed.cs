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
    /// <summary>Yahoo Finance chart endpoint (unofficial, no key).</summary>
    public sealed class YahooFinanceFeed : IPriceFeed
    {
        private static readonly HttpClient _http = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All });

        public async Task<SeriesResult> GetSeriesAsync(string symbol, CancellationToken ct)
        {
            string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{Uri.EscapeDataString(symbol)}?range=1d&interval=5m";
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) Vault2Door/2.5");
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);

            if ((int)resp.StatusCode == 429)
                throw new InvalidOperationException("Yahoo rate limit (429). Please increase PollIntervalMs or try later.");
            resp.EnsureSuccessStatusCode();

            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            var root = doc.RootElement;
            var chart = root.GetProperty("chart");
            var resultArr = chart.GetProperty("result");
            if (resultArr.GetArrayLength() == 0) throw new InvalidOperationException("Empty Yahoo Finance result.");

            var result = resultArr[0];
            var timestamps = result.GetProperty("timestamp").EnumerateArray().Select(e => (long)e.GetDouble()).ToArray();
            var quoteArr = result.GetProperty("indicators").GetProperty("quote")[0];
            var closesEnum = quoteArr.GetProperty("close").EnumerateArray();

            var points = new List<PricePoint>();
            int i = 0;
            foreach (var vEl in closesEnum)
            {
                if (i >= timestamps.Length) break;
                if (vEl.ValueKind == JsonValueKind.Null) { i++; continue; }
                if (vEl.TryGetDouble(out double v))
                {
                    var t = DateTimeOffset.FromUnixTimeSeconds(timestamps[i]).LocalDateTime;
                    points.Add(new PricePoint(t, v));
                }
                i++;
            }
            return new SeriesResult(points, "Yahoo", symbol);
        }
    }
}
