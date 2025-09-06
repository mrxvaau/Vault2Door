using System;using System.IO;using System.Text.Json;
namespace Vault2Door{
  public static class AppConfig{ public static bool UseAlphaVantage=true; public static string AlphaVantageApiKey="EK5AZXERBF7VPC88"; public const int PollIntervalMs=60000; }
  public static class ConfigService{
    static string Dir=>Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Vault2Door");
    static string FilePath=>Path.Combine(Dir,"config.json");
    public static void Load(){ try{ if(File.Exists(FilePath)){ var cfg=JsonSerializer.Deserialize<ConfigModel>(File.ReadAllText(FilePath)); if(cfg!=null && !string.IsNullOrWhiteSpace(cfg.AlphaVantageApiKey)) AppConfig.AlphaVantageApiKey=cfg.AlphaVantageApiKey!; } }catch{} }
    public static void Save(){ try{ Directory.CreateDirectory(Dir); var json=JsonSerializer.Serialize(new ConfigModel{AlphaVantageApiKey=AppConfig.AlphaVantageApiKey}, new JsonSerializerOptions{WriteIndented=true}); File.WriteAllText(FilePath,json);}catch{} }
    class ConfigModel{ public string? AlphaVantageApiKey{get;set;} }
  }}