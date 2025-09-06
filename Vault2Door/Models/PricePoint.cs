using System;

namespace Vault2Door.Models
{
    public readonly struct PricePoint
    {
        public DateTime Time { get; }
        public double Value { get; }
        public PricePoint(DateTime time, double value) { Time = time; Value = value; }
    }
}
