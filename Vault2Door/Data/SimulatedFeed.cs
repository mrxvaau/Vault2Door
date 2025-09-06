using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vault2Door.Models;

namespace Vault2Door.Data
{
    public sealed class SimulatedFeed : IPriceFeed
    {
        private readonly Random _rng = new Random();
        public Task<SeriesResult> GetSeriesAsync(string symbol, CancellationToken ct)
        {
            var list = new List<PricePoint>();
            double v = 8.0;
            var start = DateTime.Now.AddHours(-8);
            for (int i = 0; i < 100; i++)
            {
                v += (_rng.NextDouble() - 0.5) * 0.3;
                if (v < 0.1) v = 0.1;
                list.Add(new PricePoint(start.AddMinutes(i * 5), Math.Round(v, 2)));
            }
            return Task.FromResult(new SeriesResult(list, "Simulated", symbol));
        }
    }
}
