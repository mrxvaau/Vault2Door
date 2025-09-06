using System;using System.Windows.Forms;
namespace Vault2Door.Settings{ public partial class SettingsForm:Form{ public SettingsForm(){ InitializeComponent(); txtKey.Text=AppConfig.AlphaVantageApiKey; }
private void btnSave_Click(object? s, EventArgs e){ AppConfig.AlphaVantageApiKey=txtKey.Text?.Trim()??""; ConfigService.Save(); this.DialogResult=DialogResult.OK; Close(); }
private void btnCancel_Click(object? s, EventArgs e){ this.DialogResult=DialogResult.Cancel; Close(); } } }