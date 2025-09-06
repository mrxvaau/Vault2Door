# Vault2Door — LiveChartsCore Upgrade (WinForms, .NET 8)

- Resizable window (min/max), better default size, minimum size.
- Professional, **transparent** and responsive charts powered by **LiveChartsCore.SkiaSharpView.WinForms v2.0.0-rc5.4**.
- Different chart model for each asset:
  - **Gold:** glowing area chart
  - **Diamond:** line with diamond markers
  - **Silver:** smooth silver line
  - **Bronze:** bronze columns
- Dark/Light theme integration.

## How to run
1. Open `Vault2Door.sln` in **Visual Studio 2022+**.
2. Restore NuGet packages if prompted.
3. Press **F5**.

## Where the changes are
- `Vault2Door.csproj` — adds the LiveChartsCore package.
- `Form1.Fields.cs` — replaces `PictureBox` with `CartesianChart` (LiveCharts).
- `Form1.UI.Build.cs` — adds the chart control (transparent, Dock=Fill).
- `Form1.UI.Chart.cs` — defines series per asset & axes styling.
- `Form1.Theme.cs` — forwards theme to chart and axes.
