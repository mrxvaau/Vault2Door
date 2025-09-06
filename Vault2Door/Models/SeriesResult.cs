using System.Collections.Generic;

namespace Vault2Door.Models
{
    public sealed class SeriesResult
    {
        public IReadOnlyList<PricePoint> Points { get; }
        public string Provider { get; }
        public string Symbol { get; }

        public SeriesResult(IReadOnlyList<PricePoint> points, string provider, string symbol)
        {
            Points = points;
            Provider = provider;
            Symbol = symbol;
        }
    }
}
