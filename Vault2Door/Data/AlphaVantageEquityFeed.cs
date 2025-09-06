using System;using System.Collections.Generic;using System.Net.Http;using System.Text.Json;using System.Threading;using System.Threading.Tasks;using Vault2Door.Models;
namespace Vault2Door.Data{
  public sealed class AlphaVantageEquityFeed:IPriceFeed{
    static readonly HttpClient http=new HttpClient(new HttpClientHandler{AutomaticDecompression=System.Net.DecompressionMethods.All});
    public async Task<SeriesResult> GetSeriesAsync(string symbol,CancellationToken ct){
      if(string.IsNullOrWhiteSpace(AppConfig.AlphaVantageApiKey)) throw new InvalidOperationException("Alpha Vantage API key is missing.");
      string url=$"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={Uri.EscapeDataString(symbol)}&interval=5min&outputsize=compact&apikey={AppConfig.AlphaVantageApiKey}";
      using var req=new HttpRequestMessage(HttpMethod.Get,url); req.Headers.UserAgent.ParseAdd("Vault2Door/2.6");
      using var resp=await http.SendAsync(req,HttpCompletionOption.ResponseHeadersRead,ct); resp.EnsureSuccessStatusCode();
      using var stream=await resp.Content.ReadAsStreamAsync(ct); using var doc=await JsonDocument.ParseAsync(stream,cancellationToken:ct);
      System.Text.Json.JsonElement series=default; bool found=false; foreach(var p in doc.RootElement.EnumerateObject()){ if(p.Name.StartsWith("Time Series (",StringComparison.OrdinalIgnoreCase)){ series=p.Value; found=true; break; } }
      if(!found){ if(doc.RootElement.TryGetProperty("Information",out var i)) throw new InvalidOperationException("Alpha Vantage info: "+i.GetString()); if(doc.RootElement.TryGetProperty("Note",out var n)) throw new InvalidOperationException("Alpha Vantage note: "+n.GetString()); if(doc.RootElement.TryGetProperty("Error Message",out var e)) throw new InvalidOperationException("Alpha Vantage error: "+e.GetString()); throw new InvalidOperationException("Unexpected Alpha Vantage response."); }
      var pts=new List<PricePoint>(120); foreach(var kv in series.EnumerateObject()){ if(DateTime.TryParse(kv.Name,out var t)){ if(kv.Value.TryGetProperty("4. close",out var c) && double.TryParse(c.GetString(),out double v)) pts.Add(new PricePoint(t,v)); } }
      pts.Sort((a,b)=>a.Time.CompareTo(b.Time)); return new SeriesResult(pts,"AlphaVantage",symbol);
    }
  }}