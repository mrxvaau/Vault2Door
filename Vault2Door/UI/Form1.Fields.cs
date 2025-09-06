using System.Drawing;using System.IO;using System.Windows.Forms;using LiveChartsCore;using LiveChartsCore.SkiaSharpView;using LiveChartsCore.SkiaSharpView.WinForms;using Vault2Door.Models;using Vault2Door.Services;
namespace Vault2Door{ public partial class Form1:Form{
const string AppName="Vault2Door – PreciousMetals"; const string AppVersion="2.6 (Stable)";
string gifPathRoot=@"C:\\gif\\"; bool isDarkMode=true; bool realtimeEnabled=false; DataRange currentRange=DataRange.OneDay;
Panel sidebar=null!, mainPanel=null!, topHeader=null!, banner=null!, contentRow=null!, assetListPanel=null!, assetContent=null!, assetScrollTrack=null!, assetScrollThumb=null!, graphPanel=null!;
Label bannerText=null!, marketStatus=null!, balanceLabel=null!, assetsTitle=null!, providerBadge=null!, versionBadge=null!, loadingBadge=null!;
TableLayoutPanel metricsTable=null!; CartesianChart chart=null!; Button btnTheme=null!, btnBell=null!, btnUser=null!, btnRealtime=null!, btnSettings=null!, btn1D=null!, btn5D=null!, btn1M=null!;
ToolTip tip=new ToolTip(); int assetScrollOffset=0; bool assetThumbDragging=false; int assetThumbDragStartY=0, assetThumbStartTop=0;
readonly RealtimeService realtime=new RealtimeService(); readonly System.Windows.Forms.Timer realTimer=new System.Windows.Forms.Timer(); AssetKind currentAsset=AssetKind.Gold; } }