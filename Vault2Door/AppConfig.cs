namespace Vault2Door
{
    public static class AppConfig
    {
        // Use Alpha Vantage equities intraday (TIME_SERIES_INTRADAY) first, then Yahoo fallback.
        public static bool UseAlphaVantage = true;

        // PLACE YOUR FREE KEY FROM https://www.alphavantage.co/support/#api-key
        public static string AlphaVantageApiKey = "EK5AZXERBF7VPC88"; // <-- set your key here

        // Polling interval (ms). Yahoo/AV both have rate limits; be gentle.
        public const int PollIntervalMs = 60000; // 60s
    }
}
