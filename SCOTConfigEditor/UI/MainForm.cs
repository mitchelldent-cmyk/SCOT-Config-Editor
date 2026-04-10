using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SCOTConfigEditor.Helpers;

namespace SCOTConfigEditor.UI
{
    public partial class MainForm : Form
    {
        private string ConfigBase => txtConfigPath.Text.TrimEnd('\\', '/');

        private IniFile     _ini000;
        private IniFile     _iniDat;
        private IniFile     _iniTend;
        private StringsFile _stringsFile;
        private XmlConfigFile _xmlConfig;

        public MainForm()
        {
            InitializeComponent();
            LoadAll();
        }

        // ── Load all files ────────────────────────────────────────────────────────

        private void LoadAll()
        {
            _ini000      = new IniFile(Path.Combine(ConfigBase, "Scotopts.000"));
            _iniDat      = new IniFile(Path.Combine(ConfigBase, "Scotopts.dat"));
            _iniTend     = new IniFile(Path.Combine(ConfigBase, "SCOTTend.dat"));
            _stringsFile = new StringsFile(Path.Combine(ConfigBase, "SSCOStrings.en-US.custom.dat"));
            _xmlConfig   = new XmlConfigFile(Path.Combine(ConfigBase, "SSCOUI.exe.config"));

            LoadTab000();
            LoadTabDat();
            LoadTabTend();
            LoadTabStrings();
            LoadTabXml();
        }

        // ── Tab 1: Scotopts.000 ───────────────────────────────────────────────────

        private void LoadTab000()
        {
            SetFileStatus(lbl000Status, _ini000.FilePath, _ini000.Loaded, null);
            if (!_ini000.Loaded) return;

            txt000ReceiptHeader.Text        = _ini000.Get("SCOTSSF",    "ReceiptHeader");
            SetCombo(cbo000AllowAltID,        _ini000.Get("Operations",  "AllowAltIDEntry"));
            for (int i = 1; i <= 6; i++)
                txt000CashBack[i].Text      = _ini000.Get("Tender",      "CashBack" + i);
            SetCombo(cbo000MemberIDRequired,  _ini000.Get("AMPM",        "MemberIDRequired"));
            txt000DummyMemberID.Text        = _ini000.Get("AMPM",        "DummyMemberID");
        }

        private void btn000Save_Click(object sender, EventArgs e) => Save000();

        private void Save000()
        {
            if (!_ini000.Loaded) { ShowNotFound(_ini000.FilePath); return; }
            _ini000.Set("SCOTSSF",   "ReceiptHeader",    " \"" + txt000ReceiptHeader.Text + "\"");
            _ini000.Set("Operations","AllowAltIDEntry",  cbo000AllowAltID.SelectedItem?.ToString() ?? "N");
            for (int i = 1; i <= 6; i++)
                _ini000.Set("Tender","CashBack" + i,     txt000CashBack[i].Text);
            _ini000.Set("AMPM",      "MemberIDRequired", cbo000MemberIDRequired.SelectedItem?.ToString() ?? "N");
            _ini000.Set("AMPM",      "DummyMemberID",    txt000DummyMemberID.Text);
            _ini000.Save();
            SetFileStatus(lbl000Status, _ini000.FilePath, true, "Saved");
        }

        // ── Tab 2: Scotopts.dat ──────────────────────────────────────────────────

        private void LoadTabDat()
        {
            SetFileStatus(lblDatStatus, _iniDat.FilePath, _iniDat.Loaded, null);
            if (!_iniDat.Loaded) return;

            SetCombo(cboDatCash,    _iniDat.Get("Tender", "CashAllowed"));
            SetCombo(cboDatCredit,  _iniDat.Get("Tender", "CreditAllowed"));
            SetCombo(cboDatDebit,   _iniDat.Get("Tender", "DebitAllowed"));
            SetCombo(cboDatEBTFS,   _iniDat.Get("Tender", "EBTFSAllowed"));
            SetCombo(cboDatEBTCB,   _iniDat.Get("Tender", "EBTCBAllowed"));
            SetCombo(cboDatOther,   _iniDat.Get("Tender", "OtherPayment"));
            for (int i = 1; i <= 11; i++)
                txtDatCashBack[i].Text    = _iniDat.Get("Tender", "CashBack"    + i);
            for (int i = 1; i <= 11; i++)
                txtDatEBTCashBack[i].Text = _iniDat.Get("Tender", "EBTCashBack" + i);
        }

        private void btnDatSave_Click(object sender, EventArgs e) => SaveDat();

        private void SaveDat()
        {
            if (!_iniDat.Loaded) { ShowNotFound(_iniDat.FilePath); return; }
            _iniDat.Set("Tender", "CashAllowed",   cboDatCash.SelectedItem?.ToString()   ?? "Y");
            _iniDat.Set("Tender", "CreditAllowed", cboDatCredit.SelectedItem?.ToString() ?? "Y");
            _iniDat.Set("Tender", "DebitAllowed",  cboDatDebit.SelectedItem?.ToString()  ?? "Y");
            _iniDat.Set("Tender", "EBTFSAllowed",  cboDatEBTFS.SelectedItem?.ToString()  ?? "Y");
            _iniDat.Set("Tender", "EBTCBAllowed",  cboDatEBTCB.SelectedItem?.ToString()  ?? "Y");
            _iniDat.Set("Tender", "OtherPayment",  cboDatOther.SelectedItem?.ToString()  ?? "N");
            for (int i = 1; i <= 11; i++)
                _iniDat.Set("Tender", "CashBack"    + i, txtDatCashBack[i].Text);
            for (int i = 1; i <= 11; i++)
                _iniDat.Set("Tender", "EBTCashBack" + i, txtDatEBTCashBack[i].Text);
            _iniDat.Save();
            SetFileStatus(lblDatStatus, _iniDat.FilePath, true, "Saved");
        }

        // ── Tab 3: SCOTTend.dat ──────────────────────────────────────────────────

        private static readonly int[] TenderButtons = { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12 };

        private void LoadTabTend()
        {
            SetFileStatus(lblTendStatus, _iniTend.FilePath, _iniTend.Loaded, null);
            if (!_iniTend.Loaded) return;

            foreach (int b in TenderButtons)
            {
                string sec = "button" + b;
                _tendText[b].Text     = _iniTend.Get(sec, "Text");
                _tendType[b].Text     = _iniTend.Get(sec, "TenderType");
                _tendCashBack[b].Text = _iniTend.Has(sec, "CashBack") ? _iniTend.Get(sec, "CashBack") : "";
                _tendNeedPin[b].Text  = _iniTend.Has(sec, "NeedPin")  ? _iniTend.Get(sec, "NeedPin")  : "";
            }
        }

        private void btnTendSave_Click(object sender, EventArgs e) => SaveTend();

        private void SaveTend()
        {
            if (!_iniTend.Loaded) { ShowNotFound(_iniTend.FilePath); return; }
            foreach (int b in TenderButtons)
            {
                string sec = "button" + b;
                _iniTend.Set(sec, "Text", " \"" + _tendText[b].Text + "\"");
                if (_iniTend.Has(sec, "CashBack") && _tendCashBack[b].Text != "")
                    _iniTend.Set(sec, "CashBack", " " + _tendCashBack[b].Text);
                if (_iniTend.Has(sec, "NeedPin") && _tendNeedPin[b].Text != "")
                    _iniTend.Set(sec, "NeedPin",  " " + _tendNeedPin[b].Text);
            }
            _iniTend.Save();
            SetFileStatus(lblTendStatus, _iniTend.FilePath, true, "Saved");
        }

        // ── Tab 4: SSCOStrings ───────────────────────────────────────────────────

        private void LoadTabStrings()
        {
            SetFileStatus(lblStrStatus, _stringsFile.FilePath, _stringsFile.Loaded, null);
            if (!_stringsFile.Loaded) return;

            foreach (var kv in _stringBoxes)
                kv.Value.Text = _stringsFile.Get(kv.Key);
        }

        private void btnStrSave_Click(object sender, EventArgs e) => SaveStrings();

        private void SaveStrings()
        {
            if (!_stringsFile.Loaded) { ShowNotFound(_stringsFile.FilePath); return; }
            foreach (var kv in _stringBoxes)
                _stringsFile.Set(kv.Key, kv.Value.Text);
            _stringsFile.Save();
            SetFileStatus(lblStrStatus, _stringsFile.FilePath, true, "Saved");
        }

        // ── Tab 5: SSCOUI.exe.config ─────────────────────────────────────────────

        private void LoadTabXml()
        {
            SetFileStatus(lblXmlStatus, _xmlConfig.FilePath, _xmlConfig.Loaded, null);
            if (!_xmlConfig.Loaded) return;

            txtXmlAutoRestart.Text       = _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "AutoRestartOptions");
            txtXmlRestartStates.Text     = _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "AutoRestartStateParamList");
            SetCombo(cboXmlShowGoToPOS,    _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "ShowGoToPOSWidget"));
            SetCombo(cboXmlShowCursor,     _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "ShowCursor"));
            SetCombo(cboXmlLiveVideo,      _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "NextGenUILiveVideo"));
            txtXmlLogoImage.Text         = _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "LogoImage");
            txtXmlBgImage.Text           = _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "BackgroundImage");
            txtXmlWelcomeImage.Text      = _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "WelcomeImage");
            txtXmlFinishImage.Text       = _xmlConfig.GetSetting("SSCOUI.Properties.Settings", "FinishImage");
        }

        private void btnXmlSave_Click(object sender, EventArgs e) => SaveXml();

        private void SaveXml()
        {
            if (!_xmlConfig.Loaded) { ShowNotFound(_xmlConfig.FilePath); return; }
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "AutoRestartOptions",       txtXmlAutoRestart.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "AutoRestartStateParamList", txtXmlRestartStates.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "ShowGoToPOSWidget",        cboXmlShowGoToPOS.SelectedItem?.ToString() ?? "True");
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "ShowCursor",               cboXmlShowCursor.SelectedItem?.ToString()  ?? "False");
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "NextGenUILiveVideo",       cboXmlLiveVideo.SelectedItem?.ToString()   ?? "True");
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "LogoImage",                txtXmlLogoImage.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "BackgroundImage",          txtXmlBgImage.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "WelcomeImage",             txtXmlWelcomeImage.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "FinishImage",              txtXmlFinishImage.Text);
            _xmlConfig.Save();
            SetFileStatus(lblXmlStatus, _xmlConfig.FilePath, true, "Saved");
        }

        // ── Helpers ──────────────────────────────────────────────────────────────

        private static readonly Color ClrSuccess = Color.FromArgb( 63, 185,  80);
        private static readonly Color ClrError   = Color.FromArgb(248,  81,  73);
        private static readonly Color _clrMuted  = Color.FromArgb(139, 148, 158);

        private void SetFileStatus(Label lbl, string path, bool loaded, string extra)
        {
            if (extra != null)
            {
                lbl.Text      = path + "  ·  " + extra + " " + DateTime.Now.ToString("HH:mm:ss");
                lbl.ForeColor = ClrSuccess;
            }
            else if (!loaded)
            {
                lbl.Text      = path + "  ·  File not found";
                lbl.ForeColor = ClrError;
            }
            else
            {
                lbl.Text      = path;
                lbl.ForeColor = _clrMuted;
            }
        }

        private static void SetCombo(ComboBox cbo, string value)
        {
            foreach (var item in cbo.Items)
            {
                if (string.Equals(item.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    cbo.SelectedItem = item;
                    return;
                }
            }
        }

        private static void ShowNotFound(string path) =>
            MessageBox.Show("File not found:\n" + path, "File Missing",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
