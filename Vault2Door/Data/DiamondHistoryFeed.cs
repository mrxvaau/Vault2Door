using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vault2Door.Models;

namespace Vault2Door.Data
{
    /// <summary>
    /// Local diamond "history" provider that reads a CSV (date,close_usd)
    /// and returns a SeriesResult(points-only) for Stable 2.6.
    /// If the CSV is missing, it auto-generates a smooth dataset
    /// into %AppData%\Vault2Door\data\diamond_history.csv so the app never breaks.
    /// </summary>
    public sealed class DiamondHistoryFeed : IPriceFeed
    {
        private static string AppDataDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vault2Door", "data");

        private static string AppDataCsv => Path.Combine(AppDataDir, "diamond_history.csv");

        public Task<SeriesResult> GetSeriesAsync(string symbol, CancellationToken ct)
        {
            string path = EnsureCsv(ct);

            var rows = new List<PricePoint>();
            using (var sr = new StreamReader(path))
            {
                string? line = sr.ReadLine(); // header
                while ((line = sr.ReadLine()) != null)
                {
                    ct.ThrowIfCancellationRequested();
                    var parts = line.Split(',');
                    if (parts.Length < 2) continue;
                    if (!DateTime.TryParse(parts[0], out var d)) continue;
                    if (!double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var c)) continue;
                    rows.Add(new PricePoint(d, c));
                }
            }

            if (rows.Count == 0)
                throw new InvalidOperationException("diamond_history.csv contained no rows.");

            return Task.FromResult(new SeriesResult(rows, "LocalHistory", "DIAMOND"));
        }

        private static string EnsureCsv(CancellationToken ct)
        {
            // 1) AppData first
            if (File.Exists(AppDataCsv)) return AppDataCsv;

            // 2) Executable folder /data (copied by csproj as Content)
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string exeCsv = Path.Combine(exeDir, "data", "diamond_history.csv");
            if (File.Exists(exeCsv)) return exeCsv;

            // 3) Dev relative path ..\..\..\data for F5 runs
            string devCsv = Path.GetFullPath(Path.Combine(exeDir, @"..\..\..\data\diamond_history.csv"));
            if (File.Exists(devCsv)) return devCsv;

            // 4) Not found → generate AppData file
            Directory.CreateDirectory(AppDataDir);
            using (var sw = new StreamWriter(AppDataCsv))
            {
                var start = DateTime.Today.AddDays(-365 * 3);
                double price = 8.0;
                var rnd = new Random(42);
                sw.WriteLine("date,close_usd");
                for (int i = 0; i < 365 * 3; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    double season = 0.2 * (1 + Math.Sin(i / 60.0));
                    double drift = (i / 365.0) * 0.05;
                    price = Math.Max(3.0, price + (rnd.NextDouble() - 0.5) * 0.08 + season * 0.01 + drift * 0.001);
                    var d = start.AddDays(i).ToString("yyyy-MM-dd");
                    sw.WriteLine($"{d},{price:F2}");
                }
            }
            return AppDataCsv;
        }
    }
}
