using System;using System.Threading;using System.Threading.Tasks;using Vault2Door.Data;using Vault2Door.Models;
namespace Vault2Door.Services{
  public sealed class RealtimeService{
    readonly IPriceFeed av=new AlphaVantageEquityFeed(); readonly YahooFinanceFeed yh=new YahooFinanceFeed(); readonly IPriceFeed sim=new SimulatedFeed(); readonly IPriceFeed diamond=new DiamondHistoryFeed();
    static (string av,string yh) MapSymbols(AssetKind a)=> a switch{ AssetKind.Gold=>("GLD","GLD"), AssetKind.Silver=>("SLV","SLV"), AssetKind.Bronze=>("CPER","CPER"), AssetKind.Diamond=>("",""), _=>("","") };
    public async Task<SeriesResult> GetSeriesAsync(AssetKind a, DataRange range, CancellationToken ct){
      if(a==AssetKind.Diamond) return await diamond.GetSeriesAsync("DIAMOND", ct); var (asym,yhsym)=MapSymbols(a);
      string r = range==DataRange.OneDay? "1d": range==DataRange.FiveDays? "5d":"1mo";
      string iv= range==DataRange.OneDay? "5m": range==DataRange.FiveDays? "15m":"60m";
      Exception? last=null;
      if(range==DataRange.OneDay && AppConfig.UseAlphaVantage && !string.IsNullOrWhiteSpace(AppConfig.AlphaVantageApiKey) && !string.IsNullOrWhiteSpace(asym)){ try{ return await av.GetSeriesAsync(asym,ct);}catch(Exception ex){ last=ex; } }
      try{ return await yh.GetSeriesAsync(yhsym,r,iv,ct);}catch(Exception ex){ last=ex; }
      var s=await sim.GetSeriesAsync(a.ToString().ToUpperInvariant()+"_SIM",ct); if(last!=null) return new SeriesResult(s.Points,"Simulated (after error: "+last.Message+")",s.Symbol); return s;
    }
  }}