using System;using System.Collections.Generic;using System.Linq;using System.Net.Http;using System.Text.Json;using System.Threading;using System.Threading.Tasks;using Vault2Door.Models;
namespace Vault2Door.Data{
  public sealed class YahooFinanceFeed:IPriceFeed{
    static readonly HttpClient http=new HttpClient(new HttpClientHandler{AutomaticDecompression=System.Net.DecompressionMethods.All});
    public Task<SeriesResult> GetSeriesAsync(string symbol,CancellationToken ct)=>GetSeriesAsync(symbol,"1d","5m",ct);
    public async Task<SeriesResult> GetSeriesAsync(string symbol,string range,string interval,CancellationToken ct){
      string url=$"https://query1.finance.yahoo.com/v8/finance/chart/{Uri.EscapeDataString(symbol)}?range={range}&interval={interval}";
      using var req=new HttpRequestMessage(HttpMethod.Get,url); req.Headers.UserAgent.ParseAdd("Mozilla/5.0 Vault2Door/2.6");
      using var resp=await http.SendAsync(req,HttpCompletionOption.ResponseHeadersRead,ct); if((int)resp.StatusCode==429) throw new InvalidOperationException("Yahoo rate limit (429)."); resp.EnsureSuccessStatusCode();
      using var stream=await resp.Content.ReadAsStreamAsync(ct); using var doc=await JsonDocument.ParseAsync(stream,cancellationToken:ct);
      var res=doc.RootElement.GetProperty("chart").GetProperty("result"); if(res.GetArrayLength()==0) throw new InvalidOperationException("Empty Yahoo result.");
      var r=res[0]; var ts=r.GetProperty("timestamp").EnumerateArray().Select(e=>(long)e.GetDouble()).ToArray();
      var closes=r.GetProperty("indicators").GetProperty("quote")[0].GetProperty("close").EnumerateArray();
      var pts=new List<PricePoint>(); int i=0; foreach(var el in closes){ if(i>=ts.Length) break; if(el.ValueKind==JsonValueKind.Null){ i+=1; continue; } if(el.TryGetDouble(out double v)){ var t=DateTimeOffset.FromUnixTimeSeconds(ts[i]).LocalDateTime; pts.Add(new PricePoint(t,v)); } i+=1; }
      return new SeriesResult(pts,"Yahoo",symbol);
    }
  }}