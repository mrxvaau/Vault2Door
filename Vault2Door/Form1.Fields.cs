using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;

namespace Vault2Door
{
    public partial class Form1 : Form
    {
        // ===== Versioning =====
        private const string AppName = "Vault2Door – PreciousMetals";
        private const string AppVersion = "2.0 (Stable)";

        // ===== Paths & state (path retained for backwards-compat; no longer used for charts) =====
        private string gifPathRoot = @"C:\Users\Qlurut\source\repos\PreciousMetalsTradingApp\PreciousMetalsTradingApp\gif\";
        private bool isDarkMode = true; // start in dark

        // ===== UI references =====
        private Panel sidebar = null!;
        private Panel mainPanel = null!;
        private Panel topHeader = null!;
        private Panel banner = null!;
        private Label bannerText = null!;
        private Label marketStatus = null!;
        private Label balanceLabel = null!;
        private CartesianChart chart = null!; // LiveCharts chart
        private Button btnTheme = null!;
        private Button btnBell = null!;
        private Button btnUser = null!;
        private Label versionBadge = null!;
        private ToolTip tip = new ToolTip();

        // Helps layout the graph
        private Panel contentRow = null!;
        private Panel assetListPanel = null!;
        private Panel graphPanel = null!;
    }
}
