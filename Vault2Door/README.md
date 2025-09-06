# Vault2Door — Visual Studio–Ready WinForms Project

This solution is configured for **Visual Studio 2022+** and **.NET 8 (Windows)**. It keeps your working behavior intact and splits `Form1` into partials that are **nested under `Form1.cs`** in Solution Explorer for easy navigation.

## Open & Run
1. Open `Vault2Door.sln` in Visual Studio.
2. Press **F5** to run.

> The app builds with **.NET 8**. If you prefer another target (e.g., .NET 6 or later), change `<TargetFramework>` in `Vault2Door.csproj`.

## Where your code lives
- `Form1.cs` — anchor for partial class (no logic).
- `Form1.Fields.cs` — constants, paths/state, and UI fields.
- `Form1.Constructor.cs` — constructor + `Form1_Load` handler.
- `Form1.UI.Build.cs` — `BuildDashboardUI` + `CreateAssetCard`.
- `Form1.UI.Chart.cs` — `ShowChart`.
- `Form1.Theme.cs` — `ToggleTheme` + `ApplyTheme`.
- `Form1.Designer.cs` — standard WinForms designer scaffolding.
- `gif\` — put your `gold.gif`, `silver.gif`, etc. here if you want the **fallback** path to work.

## Nothing changed in behavior
All logic is **unchanged** and matches your original single-file code—only separated into partials and nested for IDE friendliness.

## Use your absolute GIF path or fallback
The app prefers your absolute path:
```
C:\Users\Qlurut\source\repos\PreciousMetalsTradingApp\PreciousMetalsTradingApp\gif\
```
If it doesn't exist, it falls back to `.\gif\` next to the EXE (this folder is included).

## Integrate into an existing project
If you just want to drop the partials into your current project:
1. Add the six `Form1.*.cs` files to your project.
2. Ensure your `.csproj` has these items to **nest them under `Form1.cs`**:

```xml
<ItemGroup>
  <Compile Update="Form1.Fields.cs"><DependentUpon>Form1.cs</DependentUpon></Compile>
  <Compile Update="Form1.Constructor.cs"><DependentUpon>Form1.cs</DependentUpon></Compile>
  <Compile Update="Form1.UI.Build.cs"><DependentUpon>Form1.cs</DependentUpon></Compile>
  <Compile Update="Form1.UI.Chart.cs"><DependentUpon>Form1.cs</DependentUpon></Compile>
  <Compile Update="Form1.Theme.cs"><DependentUpon>Form1.cs</DependentUpon></Compile>
  <Compile Update="Form1.Designer.cs"><DependentUpon>Form1.cs</DependentUpon></Compile>
</ItemGroup>
```
This is purely a **Visual Studio IDE** nesting convenience—no runtime effect.

## Targeting other frameworks
- For **.NET 6/7**, set `<TargetFramework>net6.0-windows</TargetFramework>` or `net7.0-windows`.
- If you are on **.NET Framework 4.8**, keep your existing project but you can still use these partials. Ensure your project uses C# 8+ (or remove the `?` and `!` nullable annotations).

## Tips
- Keep **Designer-generated code** minimal; your UI is built programmatically in `BuildDashboardUI`.
- Add new features in new partials, e.g., `Form1.Orders.cs`, `Form1.Reports.cs`.
