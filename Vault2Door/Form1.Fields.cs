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
        private const string AppName = "Vault2Door – PreciousMetals";
        private const string AppVersion = "2.5 (Stable)";

        private string gifPathRoot = @"C:\Users\Qlurut\source\repos\PreciousMetalsTradingApp\PreciousMetalsTradingApp\gif\";
        private bool isDarkMode = true;

        private Panel sidebar = null!;
        private Panel mainPanel = null!;
        private Panel topHeader = null!;
        private Panel banner = null!;
        private Label bannerText = null!;
        private Label marketStatus = null!;
        private Label balanceLabel = null!;
        private TableLayoutPanel metricsTable = null!;
        private CartesianChart chart = null!;
        private Button btnTheme = null!;
        private Button btnBell = null!;
        private Button btnUser = null!;
        private Label versionBadge = null!;
        private ToolTip tip = new ToolTip();

        private Panel contentRow = null!;
        private Panel assetListPanel = null!;
        private Panel assetContent = null!;
        private Panel assetScrollTrack = null!;
        private Panel assetScrollThumb = null!;
        private Panel graphPanel = null!;
        private Label assetsTitle = null!;

        private int assetScrollOffset = 0;
        private bool assetThumbDragging = false;
        private int assetThumbDragStartY = 0;
        private int assetThumbStartTop = 0;
    }
}
