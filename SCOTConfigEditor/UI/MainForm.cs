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

        private IniFile       _ini000;
        private IniFile       _iniDat;
        private IniFile       _iniTend;
        private StringsFile   _stringsFile;
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

            // [SCOTSSF]
            txt000ReceiptHeader.Text           = _ini000.Get("SCOTSSF", "ReceiptHeader");
            SetCombo(cbo000Enable194Receipt,     _ini000.Get("SCOTSSF", "Enable194Receipt"));
            SetCombo(cbo000AllowRounding,        _ini000.Get("SCOTSSF", "AllowRounding"));
            SetCombo(cbo000HeadCashierRequired,  _ini000.Get("SCOTSSF", "HeadCashierRequiredforCashManagement"));
            SetCombo(cbo000TenderReversalPrompt, _ini000.Get("SCOTSSF", "TenderReversalPrompt"));

            // [Operations]
            SetCombo(cbo000AllowAltID,              _ini000.Get("Operations", "AllowAltIDEntry"));
            SetCombo(cbo000PartialIntegratedPINPad,  _ini000.Get("Operations", "PartialIntegratedPINPad"));
            SetCombo(cbo000AllowContainerBtn,        _ini000.Get("Operations", "AllowContainerBtn"));
            SetCombo(cbo000CustomerBagAllowed,       _ini000.Get("Operations", "CustomerBagAllowed"));
            SetCombo(cbo000SellBagsAtFinalize,       _ini000.Get("Operations", "SellBagsAtFinalize"));

            // [State]
            SetCombo(cbo000StartupToLaneClosed,    _ini000.Get("State", "StartupToLaneClosed"));
            SetCombo(cbo000DualLanguage,           _ini000.Get("State", "DualLanguage"));
            SetCombo(cbo000SayPrices,              _ini000.Get("State", "SayPrices"));
            SetCombo(cbo000DisplayWeightsMeasures, _ini000.Get("State", "DisplayWeightsAndMeasures"));
            SetCombo(cbo000AllowLoyaltyCard,       _ini000.Get("State", "AllowLoyaltyCard"));
            SetCombo(cbo000CouponSensor,           _ini000.Get("State", "CouponSensor"));
            SetCombo(cbo000AssistMode,             _ini000.Get("State", "AssistMode"));

            // [Tender]
            for (int i = 1; i <= 6; i++)
                txt000CashBack[i].Text = _ini000.Get("Tender", "CashBack" + i);
            SetCombo(cbo000OtherPayment, _ini000.Get("Tender", "OtherPayment"));

            // [AMPM]
            SetCombo(cbo000MemberIDRequired, _ini000.Get("AMPM", "MemberIDRequired"));
            txt000DummyMemberID.Text        = _ini000.Get("AMPM", "DummyMemberID");
        }

        private void btn000Save_Click(object sender, EventArgs e) => Save000();

        private void Save000()
        {
            if (!_ini000.Loaded) { ShowNotFound(_ini000.FilePath); return; }

            // [SCOTSSF]
            _ini000.Set("SCOTSSF", "ReceiptHeader",                        " \"" + txt000ReceiptHeader.Text + "\"");
            _ini000.Set("SCOTSSF", "Enable194Receipt",                     YN(cbo000Enable194Receipt));
            _ini000.Set("SCOTSSF", "AllowRounding",                        YN(cbo000AllowRounding));
            _ini000.Set("SCOTSSF", "HeadCashierRequiredforCashManagement", YN(cbo000HeadCashierRequired));
            _ini000.Set("SCOTSSF", "TenderReversalPrompt",                  YN(cbo000TenderReversalPrompt));

            // [Operations]
            _ini000.Set("Operations", "AllowAltIDEntry",         YN(cbo000AllowAltID));
            _ini000.Set("Operations", "PartialIntegratedPINPad", YN(cbo000PartialIntegratedPINPad));
            _ini000.Set("Operations", "AllowContainerBtn",       YN(cbo000AllowContainerBtn));
            _ini000.Set("Operations", "CustomerBagAllowed",      YN(cbo000CustomerBagAllowed));
            _ini000.Set("Operations", "SellBagsAtFinalize",      YN(cbo000SellBagsAtFinalize));

            // [State]
            _ini000.Set("State", "StartupToLaneClosed",      YN(cbo000StartupToLaneClosed));
            _ini000.Set("State", "DualLanguage",              YN(cbo000DualLanguage));
            _ini000.Set("State", "SayPrices",                 YN(cbo000SayPrices));
            _ini000.Set("State", "DisplayWeightsAndMeasures", YN(cbo000DisplayWeightsMeasures));
            _ini000.Set("State", "AllowLoyaltyCard",          YN(cbo000AllowLoyaltyCard));
            _ini000.Set("State", "CouponSensor",              YN(cbo000CouponSensor));
            _ini000.Set("State", "AssistMode",                YN(cbo000AssistMode));

            // [Tender]
            for (int i = 1; i <= 6; i++)
                _ini000.Set("Tender", "CashBack" + i, txt000CashBack[i].Text);
            _ini000.Set("Tender", "OtherPayment", YN(cbo000OtherPayment));

            // [AMPM]
            _ini000.Set("AMPM", "MemberIDRequired", YN(cbo000MemberIDRequired));
            _ini000.Set("AMPM", "DummyMemberID",    txt000DummyMemberID.Text);

            _ini000.Save();
            SetFileStatus(lbl000Status, _ini000.FilePath, true, "Saved");
        }

        // ── Tab 2: Scotopts.dat ──────────────────────────────────────────────────

        private void LoadTabDat()
        {
            SetFileStatus(lblDatStatus, _iniDat.FilePath, _iniDat.Loaded, null);
            if (!_iniDat.Loaded) return;

            // [Tender]
            SetCombo(cboDatCash,              _iniDat.Get("Tender", "CashAllowed"));
            SetCombo(cboDatCredit,            _iniDat.Get("Tender", "CreditAllowed"));
            SetCombo(cboDatDebit,             _iniDat.Get("Tender", "DebitAllowed"));
            SetCombo(cboDatEBTFS,             _iniDat.Get("Tender", "EBTFSAllowed"));
            SetCombo(cboDatEBTCB,             _iniDat.Get("Tender", "EBTCBAllowed"));
            SetCombo(cboDatOther,             _iniDat.Get("Tender", "OtherPayment"));
            SetCombo(cboDatAllowCoupons,      _iniDat.Get("Tender", "AllowCoupons"));
            SetCombo(cboDatShowUseCoupons,    _iniDat.Get("Tender", "ShowUseCoupons"));
            SetCombo(cboDatIsMotorizedMSR,    _iniDat.Get("Tender", "IsMotorizedMSR"));
            SetCombo(cboDatCancelPartialEBT,  _iniDat.Get("Tender", "AllowCancelWithPartialEBTTender"));
            txtDatPrintReceiptOver.Text      = _iniDat.Get("Tender", "PrintReceiptTrxOverAmount");
            txtDatDenominations.Text         = _iniDat.Get("Tender", "Denominations");
            for (int i = 1; i <= 11; i++)
                txtDatCashBack[i].Text    = _iniDat.Get("Tender", "CashBack" + i);
            for (int i = 1; i <= 11; i++)
                txtDatEBTCashBack[i].Text = _iniDat.Get("Tender", "EBTCashBack" + i);

            // [Operations]
            SetCombo(cboDatAllowDegradedMode,  _iniDat.Get("Operations", "AllowDegradedMode"));
            SetCombo(cboDatAllowSkipBagging,   _iniDat.Get("Operations", "AllowSkipBaggingBtn"));
            SetCombo(cboDatAllowContainerBtn,  _iniDat.Get("Operations", "AllowContainerBtn"));
            SetCombo(cboDatSellBagsAtFinalize, _iniDat.Get("Operations", "SellBagsAtFinalize"));
            SetCombo(cboDatGiftCardRedemption, _iniDat.Get("Operations", "GiftCardRedemption"));
            SetCombo(cboDatDisplayGiftCard,    _iniDat.Get("Operations", "DisplayGiftCardBalance"));
            SetCombo(cboDatMethodEmptyGiftCard,_iniDat.Get("Operations", "MethodOfCollectionForEmptyGiftCard"));
            SetCombo(cboDatShowVolumeControl,  _iniDat.Get("Operations", "ShowVolumeControl"));
            SetCombo(cboDatAllowCancelAll,     _iniDat.Get("Operations", "AllowCancelAll"));
            SetCombo(cboDatSendSignature,      _iniDat.Get("Operations", "SendSignatureDataToTB"));
            txtDatMaxCoupon.Text             = _iniDat.Get("Operations", "MaxCoupon");

            // [State]
            SetCombo(cboDatSayPrices,       _iniDat.Get("State", "SayPrices"));
            SetCombo(cboDatSayPhrases,      _iniDat.Get("State", "SayPhrases"));
            SetCombo(cboDatLiveVideo,       _iniDat.Get("State", "LiveVideo"));
            SetCombo(cboDatAllowLoyaltyCard,_iniDat.Get("State", "AllowLoyaltyCard"));
            SetCombo(cboDatCouponSensor,    _iniDat.Get("State", "CouponSensor"));
            SetCombo(cboDatStartupClosed,   _iniDat.Get("State", "StartupToLaneClosed"));
            SetCombo(cboDatAssistMode,      _iniDat.Get("State", "AssistMode"));
            txtDatMovieOnAttract.Text      = _iniDat.Get("State", "MoviePlayOnAttract");

            // [Tracing]
            txtDatTraceLevel.Text = _iniDat.Get("Tracing", "CurrentLevel");

            // [TimeOut]
            txtDatTimeoutTender.Text    = _iniDat.Get("TimeOut", "Tender");
            txtDatTimeoutAttract.Text   = _iniDat.Get("TimeOut", "Attract");
            txtDatTimeoutThankYou.Text  = _iniDat.Get("TimeOut", "ThankYou");
            txtDatTimeoutItemSale.Text  = _iniDat.Get("TimeOut", "ItemSaleInProgress");
            txtDatTimeoutTakeReceipt.Text = _iniDat.Get("TimeOut", "TakeReceipt");
        }

        private void btnDatSave_Click(object sender, EventArgs e) => SaveDat();

        private void SaveDat()
        {
            if (!_iniDat.Loaded) { ShowNotFound(_iniDat.FilePath); return; }

            // [Tender]
            _iniDat.Set("Tender", "CashAllowed",                      YN(cboDatCash));
            _iniDat.Set("Tender", "CreditAllowed",                    YN(cboDatCredit));
            _iniDat.Set("Tender", "DebitAllowed",                     YN(cboDatDebit));
            _iniDat.Set("Tender", "EBTFSAllowed",                     YN(cboDatEBTFS));
            _iniDat.Set("Tender", "EBTCBAllowed",                     YN(cboDatEBTCB));
            _iniDat.Set("Tender", "OtherPayment",                     YN(cboDatOther));
            _iniDat.Set("Tender", "AllowCoupons",                     Val(cboDatAllowCoupons));
            _iniDat.Set("Tender", "ShowUseCoupons",                   YN(cboDatShowUseCoupons));
            _iniDat.Set("Tender", "IsMotorizedMSR",                   YN(cboDatIsMotorizedMSR));
            _iniDat.Set("Tender", "AllowCancelWithPartialEBTTender",  YN(cboDatCancelPartialEBT));
            _iniDat.Set("Tender", "PrintReceiptTrxOverAmount",        txtDatPrintReceiptOver.Text);
            _iniDat.Set("Tender", "Denominations",                    txtDatDenominations.Text);
            for (int i = 1; i <= 11; i++)
                _iniDat.Set("Tender", "CashBack" + i,    txtDatCashBack[i].Text);
            for (int i = 1; i <= 11; i++)
                _iniDat.Set("Tender", "EBTCashBack" + i, txtDatEBTCashBack[i].Text);

            // [Operations]
            _iniDat.Set("Operations", "AllowDegradedMode",                    YN(cboDatAllowDegradedMode));
            _iniDat.Set("Operations", "AllowSkipBaggingBtn",                  YN(cboDatAllowSkipBagging));
            _iniDat.Set("Operations", "AllowContainerBtn",                    YN(cboDatAllowContainerBtn));
            _iniDat.Set("Operations", "SellBagsAtFinalize",                   YN(cboDatSellBagsAtFinalize));
            _iniDat.Set("Operations", "GiftCardRedemption",                   Val(cboDatGiftCardRedemption));
            _iniDat.Set("Operations", "DisplayGiftCardBalance",               YN(cboDatDisplayGiftCard));
            _iniDat.Set("Operations", "MethodOfCollectionForEmptyGiftCard",   Val(cboDatMethodEmptyGiftCard));
            _iniDat.Set("Operations", "ShowVolumeControl",                    YN(cboDatShowVolumeControl));
            _iniDat.Set("Operations", "AllowCancelAll",                       YN(cboDatAllowCancelAll));
            _iniDat.Set("Operations", "SendSignatureDataToTB",                YN(cboDatSendSignature));
            _iniDat.Set("Operations", "MaxCoupon",                            txtDatMaxCoupon.Text);

            // [State]
            _iniDat.Set("State", "SayPrices",             YN(cboDatSayPrices));
            _iniDat.Set("State", "SayPhrases",            YN(cboDatSayPhrases));
            _iniDat.Set("State", "LiveVideo",             YN(cboDatLiveVideo));
            _iniDat.Set("State", "AllowLoyaltyCard",      YN(cboDatAllowLoyaltyCard));
            _iniDat.Set("State", "CouponSensor",          YN(cboDatCouponSensor));
            _iniDat.Set("State", "StartupToLaneClosed",   YN(cboDatStartupClosed));
            _iniDat.Set("State", "AssistMode",            YN(cboDatAssistMode));
            _iniDat.Set("State", "MoviePlayOnAttract",    txtDatMovieOnAttract.Text);

            // [Tracing]
            _iniDat.Set("Tracing", "CurrentLevel", txtDatTraceLevel.Text);

            // [TimeOut]
            _iniDat.Set("TimeOut", "Tender",              txtDatTimeoutTender.Text);
            _iniDat.Set("TimeOut", "Attract",             txtDatTimeoutAttract.Text);
            _iniDat.Set("TimeOut", "ThankYou",            txtDatTimeoutThankYou.Text);
            _iniDat.Set("TimeOut", "ItemSaleInProgress",  txtDatTimeoutItemSale.Text);
            _iniDat.Set("TimeOut", "TakeReceipt",         txtDatTimeoutTakeReceipt.Text);

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
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "AutoRestartOptions",        txtXmlAutoRestart.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "AutoRestartStateParamList", txtXmlRestartStates.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "ShowGoToPOSWidget",         cboXmlShowGoToPOS.SelectedItem?.ToString() ?? "True");
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "ShowCursor",                cboXmlShowCursor.SelectedItem?.ToString()  ?? "False");
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "NextGenUILiveVideo",        cboXmlLiveVideo.SelectedItem?.ToString()   ?? "True");
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "LogoImage",                 txtXmlLogoImage.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "BackgroundImage",           txtXmlBgImage.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "WelcomeImage",              txtXmlWelcomeImage.Text);
            _xmlConfig.SetSetting("SSCOUI.Properties.Settings", "FinishImage",               txtXmlFinishImage.Text);
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
            if (string.IsNullOrEmpty(value)) return;
            foreach (var item in cbo.Items)
            {
                if (string.Equals(item.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    cbo.SelectedItem = item;
                    return;
                }
            }
        }

        private static string YN(ComboBox cbo)  => cbo.SelectedItem?.ToString() ?? "N";
        private static string Val(ComboBox cbo) => cbo.SelectedItem?.ToString() ?? "";

        private static void ShowNotFound(string path) =>
            MessageBox.Show("File not found:\n" + path, "File Missing",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
