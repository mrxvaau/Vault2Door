# Vault2Door – Precious Metals Trading Platform

A sophisticated Windows Forms application for trading precious metals with real-time price feeds, advanced charting capabilities, and secure portfolio management.

![Version](https://img.shields.io/badge/version-2.6.1c-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)

## 🌟 Features

### Core Functionality
- **Multi-Asset Support**: Trade Gold (GLD), Silver (SLV), Bronze (CPER), and Diamonds
- **Real-time Price Feeds**: Integration with Alpha Vantage and Yahoo Finance APIs
- **Interactive Charts**: Advanced charting with LiveCharts2 and SkiaSharp
- **Multiple Timeframes**: 1 Day, 5 Days, and 1 Month data ranges
- **Portfolio Management**: Track holdings, P&L, and cash balance

### User Interface
- **Modern Dashboard**: Clean, professional interface with dark/light theme support
- **Custom Sparklines**: Lightweight performance indicators for KPI cards
- **Responsive Design**: Adaptive layout with smooth scrolling and navigation
- **Context Menus**: Export charts as PNG or CSV
- **Navigation System**: Multi-page interface (Dashboard, Markets, Holdings, etc.)

### Data Management
- **Fallback System**: Graceful degradation from real-time → simulated data
- **Local Storage**: Diamond price history with auto-generation
- **Configuration**: Persistent settings in AppData folder
- **Error Handling**: Robust error management with user-friendly messages

## 🚀 Quick Start

### Prerequisites
- Windows 10/11
- .NET 8.0 Runtime
- Internet connection (for real-time data)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/Vault2Door.git
   cd Vault2Door
   ```

2. **Build the project**
   ```bash
   dotnet build --configuration Release
   ```

3. **Run the application**
   ```bash
   dotnet run --project Vault2Door
   ```

### First Launch Setup

1. **API Configuration** (Optional)
   - Click the settings button (⚙️) in the top toolbar
   - Enter your Alpha Vantage API key for enhanced real-time data
   - Free API keys available at [Alpha Vantage](https://www.alphavantage.co/support/#api-key)

2. **Theme Selection**
   - Press `Ctrl+D` or click the theme button (🌙/☀️) to toggle dark/light mode

3. **Real-time Data**
   - Click the clock button (🕒) to enable real-time price feeds
   - Data refreshes every 60 seconds when enabled

## 📊 Usage Guide

### Dashboard Overview
- **Portfolio Metrics**: View total portfolio value, cash balance, holdings, and P&L
- **Asset Cards**: Click any asset card to view detailed charts
- **Market Status**: Real-time market status indicator
- **Quick Actions**: Access settings, notifications, and user profile

### Chart Features
- **Interactive Charts**: Hover for data points, zoom, and pan
- **Multiple Chart Types**: Line charts for precious metals, column charts for bronze
- **Export Options**: Right-click charts to export as PNG or CSV
- **Time Ranges**: Switch between 1D, 5D, and 1M views

### Navigation
- **Sidebar Menu**: Navigate between Dashboard, Markets, Holdings, Orders, Payments, Reports, and KYC Status
- **Breadcrumb Navigation**: Clear indication of current page location

### Asset Management
- **Asset Selection**: Click asset cards to switch between Gold, Silver, Bronze, and Diamond views
- **Buy/Sell Actions**: Quick action buttons on each asset card
- **Price Monitoring**: Real-time price updates with change indicators

## 🔧 Configuration

### API Keys
The application stores configuration in `%AppData%\Vault2Door\config.json`:

```json
{
  "AlphaVantageApiKey": "your-api-key-here"
}
```

### Data Sources
1. **Alpha Vantage**: Primary source for intraday equity data (requires API key)
2. **Yahoo Finance**: Fallback source for all timeframes
3. **Local History**: Diamond prices from CSV file
4. **Simulated Data**: Demo data when APIs are unavailable

### Data Storage
- **Configuration**: `%AppData%\Vault2Door\config.json`
- **Diamond History**: `%AppData%\Vault2Door\data\diamond_history.csv`
- **Application Data**: Embedded resources and generated content

## 🏗️ Architecture

### Project Structure
```
Vault2Door/
├── Data/                     # Data providers and feeds
│   ├── AlphaVantageEquityFeed.cs
│   ├── YahooFinanceFeed.cs
│   ├── DiamondHistoryFeed.cs
│   ├── SimulatedFeed.cs
│   └── IPriceFeed.cs
├── Models/                   # Data models
│   ├── AssetKind.cs
│   ├── DataRange.cs
│   ├── PricePoint.cs
│   └── SeriesResult.cs
├── Services/                 # Business logic
│   └── RealtimeService.cs
├── Settings/                 # Configuration UI
│   ├── SettingsForm.cs
│   └── SettingsForm.Designer.cs
├── UI/                       # User interface
│   ├── Form1.*.cs           # Main form (partial classes)
│   └── SparklineControl.cs  # Custom sparkline control
├── AppConfig.cs             # Application configuration
└── Program.cs               # Entry point
```

### Key Components

#### Data Layer
- **IPriceFeed**: Interface for all data providers
- **RealtimeService**: Orchestrates data retrieval with fallback logic
- **Multiple Providers**: Alpha Vantage, Yahoo Finance, local CSV, simulated data

#### UI Layer
- **Modular Design**: Partial classes for organized code structure
- **Custom Controls**: SparklineControl for performance indicators
- **Theme System**: Dark/light mode with consistent styling
- **Responsive Layout**: Adaptive sizing and scrolling

#### Models
- **AssetKind**: Enumeration of supported precious metals
- **DataRange**: Time range options (1D, 5D, 1M)
- **PricePoint**: Time-stamped price data structure
- **SeriesResult**: Complete data series with metadata

## 🔌 Dependencies

### NuGet Packages
```xml
<PackageReference Include="LiveChartsCore.SkiaSharpView.WinForms" Version="2.0.0-rc5.4" />
```

### System Requirements
- **.NET 8.0**: Modern .NET runtime with performance improvements
- **Windows Forms**: Native Windows UI framework
- **SkiaSharp**: Cross-platform 2D graphics API for chart rendering
- **System.Text.Json**: Built-in JSON serialization
- **HttpClient**: HTTP communications for API calls

## 🛠️ Development

### Building from Source
```bash
# Restore dependencies
dotnet restore

# Build in debug mode
dotnet build

# Build in release mode
dotnet build --configuration Release

# Run with hot reload during development
dotnet watch run --project Vault2Door
```

### Development Features
- **Hot Reload**: Automatic rebuilding during development
- **Partial Classes**: Organized code structure with separation of concerns
- **Designer Support**: Full Windows Forms designer integration
- **Resource Management**: Automatic handling of embedded resources

### Debugging
- Set `AppConfig.UseAlphaVantage = false` to force Yahoo Finance usage
- Diamond data auto-generates if CSV is missing
- Simulated data available when all APIs fail
- Comprehensive error handling with user-friendly messages

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Style
- Follow C# naming conventions
- Use partial classes for large forms
- Implement proper error handling
- Add XML documentation for public APIs
- Maintain consistent indentation and formatting

## 📈 Roadmap

### Planned Features
- [ ] WebSocket real-time feeds for instant updates
- [ ] Advanced technical indicators (RSI, MACD, Bollinger Bands)
- [ ] Portfolio optimization algorithms
- [ ] Mobile companion app
- [ ] Multi-currency support
- [ ] Advanced order types (stop-loss, limit orders)
- [ ] Historical data analysis tools
- [ ] Custom alert system

### Performance Improvements
- [ ] Async chart rendering
- [ ] Data caching and compression
- [ ] Memory usage optimization
- [ ] Startup time improvements

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **LiveCharts2**: Powerful charting library for .NET
- **SkiaSharp**: Cross-platform 2D graphics
- **Alpha Vantage**: Financial data API provider
- **Yahoo Finance**: Alternative financial data source
- **.NET Team**: Excellent framework and tooling

## 📞 Support

- **Issues**: Report bugs and request features via GitHub Issues
- **Discussions**: Join community discussions in GitHub Discussions
- **Documentation**: Additional docs available in the `/docs` folder
- **Email**: themrxavu.com (if applicable)

---

**Vault2Door** - Professional precious metals trading platform built with modern .NET technologies.

*Made with ❤️ for precious metals traders and investors worldwide.*
