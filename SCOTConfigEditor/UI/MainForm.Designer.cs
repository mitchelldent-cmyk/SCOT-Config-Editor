using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SCOTConfigEditor.UI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Colour palette ────────────────────────────────────────────────────────
        private static readonly Color ClrBack     = Color.FromArgb( 13,  17,  23);
        private static readonly Color ClrSurface  = Color.FromArgb( 22,  27,  34);
        private static readonly Color ClrSurface2 = Color.FromArgb( 33,  38,  45);
        private static readonly Color ClrInput    = Color.FromArgb( 22,  27,  34);
        private static readonly Color ClrInputFg  = Color.FromArgb(201, 209, 217);
        private static readonly Color ClrText     = Color.FromArgb(230, 237, 243);
        private static readonly Color ClrMuted    = Color.FromArgb(139, 148, 158);
        private static readonly Color ClrAccent   = Color.FromArgb( 88, 166, 255);
        private static readonly Color ClrBorder   = Color.FromArgb( 48,  54,  61);

        // ── Path bar ─────────────────────────────────────────────────────────────
        private TextBox txtConfigPath;

        // ── Navigation ────────────────────────────────────────────────────────────
        private readonly List<Panel> _navPanels   = new List<Panel>();
        private readonly List<Panel> _navAccents  = new List<Panel>();
        private readonly List<Label> _navLabels   = new List<Label>();
        private readonly List<Panel> _contentPanels = new List<Panel>();

        // ── Tab 1: Scotopts.000 ───────────────────────────────────────────────────
        private Label    lbl000Status;
        // [SCOTSSF]
        private TextBox  txt000ReceiptHeader;
        private ComboBox cbo000Enable194Receipt, cbo000AllowRounding;
        private ComboBox cbo000HeadCashierRequired, cbo000TenderReversalPrompt;
        // [Operations]
        private ComboBox cbo000AllowAltID, cbo000PartialIntegratedPINPad;
        private ComboBox cbo000AllowContainerBtn, cbo000CustomerBagAllowed, cbo000SellBagsAtFinalize;
        // [State]
        private ComboBox cbo000StartupToLaneClosed, cbo000DualLanguage;
        private ComboBox cbo000SayPrices, cbo000DisplayWeightsMeasures;
        private ComboBox cbo000AllowLoyaltyCard, cbo000CouponSensor, cbo000AssistMode;
        // [Tender]
        private readonly Dictionary<int, TextBox> txt000CashBack = new Dictionary<int, TextBox>();
        private ComboBox cbo000OtherPayment;
        // [AMPM]
        private ComboBox cbo000MemberIDRequired;
        private TextBox  txt000DummyMemberID;

        // ── Tab 2: Scotopts.dat ───────────────────────────────────────────────────
        private Label    lblDatStatus;
        // [Tender]
        private ComboBox cboDatCash, cboDatCredit, cboDatDebit, cboDatEBTFS, cboDatEBTCB, cboDatOther;
        private ComboBox cboDatAllowCoupons, cboDatShowUseCoupons;
        private ComboBox cboDatIsMotorizedMSR, cboDatCancelPartialEBT;
        private TextBox  txtDatPrintReceiptOver, txtDatDenominations;
        private readonly Dictionary<int, TextBox> txtDatCashBack    = new Dictionary<int, TextBox>();
        private readonly Dictionary<int, TextBox> txtDatEBTCashBack = new Dictionary<int, TextBox>();
        // [Operations]
        private ComboBox cboDatAllowDegradedMode, cboDatAllowSkipBagging;
        private ComboBox cboDatAllowContainerBtn, cboDatSellBagsAtFinalize;
        private ComboBox cboDatGiftCardRedemption, cboDatDisplayGiftCard, cboDatMethodEmptyGiftCard;
        private ComboBox cboDatShowVolumeControl, cboDatAllowCancelAll, cboDatSendSignature;
        private TextBox  txtDatMaxCoupon;
        // [State]
        private ComboBox cboDatSayPrices, cboDatSayPhrases, cboDatLiveVideo;
        private ComboBox cboDatAllowLoyaltyCard, cboDatCouponSensor;
        private ComboBox cboDatStartupClosed, cboDatAssistMode;
        private TextBox  txtDatMovieOnAttract;
        // [Tracing]
        private TextBox  txtDatTraceLevel;
        // [TimeOut]
        private TextBox txtDatTimeoutTender, txtDatTimeoutAttract, txtDatTimeoutThankYou;
        private TextBox txtDatTimeoutItemSale, txtDatTimeoutTakeReceipt;

        // ── Tab 3: SCOTTend.dat ───────────────────────────────────────────────────
        private Label lblTendStatus;
        private readonly Dictionary<int, TextBox> _tendText     = new Dictionary<int, TextBox>();
        private readonly Dictionary<int, TextBox> _tendType     = new Dictionary<int, TextBox>();
        private readonly Dictionary<int, TextBox> _tendCashBack = new Dictionary<int, TextBox>();
        private readonly Dictionary<int, TextBox> _tendNeedPin  = new Dictionary<int, TextBox>();

        // ── Tab 4: SSCOStrings ────────────────────────────────────────────────────
        private Label lblStrStatus;
        private readonly Dictionary<string, TextBox> _stringBoxes = new Dictionary<string, TextBox>();

        // ── Tab 5: SSCOUI.exe.config ──────────────────────────────────────────────
        private Label    lblXmlStatus;
        private TextBox  txtXmlAutoRestart, txtXmlRestartStates;
        private ComboBox cboXmlShowGoToPOS, cboXmlShowCursor, cboXmlLiveVideo;
        private TextBox  txtXmlLogoImage, txtXmlBgImage, txtXmlWelcomeImage, txtXmlFinishImage;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text          = "SCOT Config Editor";
            this.Icon          = new System.Drawing.Icon(
                System.Reflection.Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("SCOTConfigEditor.app.ico")
                ?? System.Reflection.Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("app.ico"));
            this.ClientSize    = new Size(920, 720);
            this.MinimumSize   = new Size(700, 500);
            this.MaximizeBox   = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font          = new Font("Segoe UI", 9f);
            this.BackColor     = ClrBack;
            this.ForeColor     = ClrText;

            // ── Header ───────────────────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(920, 55),
                BackColor = ClrSurface2,
                Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlHeader.Controls.Add(new Label
            {
                Text      = "SCOT Config Editor",
                Location  = new Point(16, 6),
                Size      = new Size(600, 26),
                Font      = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = ClrText,
                BackColor = Color.Transparent
            });
            pnlHeader.Controls.Add(new Label
            {
                Text      = "NCR ENCOR  ·  SCO Terminal Configuration Files",
                Location  = new Point(18, 33),
                Size      = new Size(600, 16),
                Font      = new Font("Segoe UI", 8f),
                ForeColor = ClrMuted,
                BackColor = Color.Transparent
            });

            var pnlAccentLine = new Panel
            {
                Location  = new Point(0, 55),
                Size      = new Size(920, 2),
                BackColor = ClrAccent,
                Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // ── Path bar ─────────────────────────────────────────────────────────
            var pnlPathBar = new Panel
            {
                Location  = new Point(0, 57),
                Size      = new Size(920, 34),
                BackColor = ClrSurface,
                Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlPathBar.Controls.Add(new Label
            {
                Text      = "Config Path:",
                Location  = new Point(12, 0),
                Size      = new Size(82, 34),
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = ClrMuted,
                BackColor = Color.Transparent,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            });
            txtConfigPath = new TextBox
            {
                Location    = new Point(98, 7),
                Size        = new Size(700, 22),
                BackColor   = ClrInput,
                ForeColor   = ClrInputFg,
                BorderStyle = BorderStyle.FixedSingle,
                Text        = @"C:\scot\config",
                Anchor      = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            var btnReload = new Button
            {
                Text      = "Reload",
                Location  = new Point(806, 6),
                Size      = new Size(90, 22),
                BackColor = ClrSurface2,
                ForeColor = ClrMuted,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 8f),
                Cursor    = Cursors.Hand,
                Anchor    = AnchorStyles.Top | AnchorStyles.Right
            };
            btnReload.FlatAppearance.BorderColor = ClrBorder;
            btnReload.Click   += (s, e) => LoadAll();
            txtConfigPath.KeyDown += (s, e) => { if (e.KeyCode == Keys.Return) LoadAll(); };
            pnlPathBar.Controls.Add(txtConfigPath);
            pnlPathBar.Controls.Add(btnReload);

            // ── Left nav ─────────────────────────────────────────────────────────
            var pnlNav = new Panel
            {
                Location  = new Point(0, 91),
                Size      = new Size(160, 629),
                BackColor = ClrSurface2,
                Anchor    = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            pnlNav.Controls.Add(new Label
            {
                Text      = "FILES",
                Location  = new Point(12, 10),
                Size      = new Size(136, 14),
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = ClrMuted,
                BackColor = Color.Transparent
            });

            string[] navTitles = { "Scotopts.000", "Scotopts.dat", "SCOTTend.dat", "SSCOStrings", "SSCOUI Config" };
            for (int i = 0; i < navTitles.Length; i++)
            {
                int idx      = i;
                var navPanel  = new Panel { Location = new Point(0, 30 + i * 40), Size = new Size(160, 38), BackColor = ClrSurface2, Cursor = Cursors.Hand };
                var navAccent = new Panel { Location = new Point(0, 0), Size = new Size(3, 38), BackColor = Color.Transparent };
                var navLabel  = new Label
                {
                    Text      = navTitles[i],
                    Location  = new Point(14, 0),
                    Size      = new Size(144, 38),
                    Font      = new Font("Segoe UI", 8.5f),
                    ForeColor = ClrMuted,
                    BackColor = Color.Transparent,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                };
                navPanel.Controls.Add(navAccent);
                navPanel.Controls.Add(navLabel);
                navPanel.Click  += (s, e) => ActivateNav(idx);
                navLabel.Click  += (s, e) => ActivateNav(idx);
                navAccent.Click += (s, e) => ActivateNav(idx);
                _navPanels.Add(navPanel);
                _navAccents.Add(navAccent);
                _navLabels.Add(navLabel);
                pnlNav.Controls.Add(navPanel);
            }

            // ── Content area ─────────────────────────────────────────────────────
            var pnlContentArea = new Panel
            {
                Location  = new Point(160, 91),
                Size      = new Size(760, 629),
                BackColor = ClrBack,
                Anchor    = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            for (int i = 0; i < 5; i++)
            {
                var cp = new Panel
                {
                    Location   = new Point(0, 0),
                    Size       = new Size(760, 629),
                    AutoScroll = true,
                    BackColor  = ClrBack,
                    Anchor     = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    Visible    = (i == 0)
                };
                _contentPanels.Add(cp);
                pnlContentArea.Controls.Add(cp);
            }

            BuildContent000(_contentPanels[0]);
            BuildContentDat(_contentPanels[1]);
            BuildContentTend(_contentPanels[2]);
            BuildContentStrings(_contentPanels[3]);
            BuildContentXml(_contentPanels[4]);
            ActivateNav(0);

            this.Controls.AddRange(new Control[] { pnlHeader, pnlAccentLine, pnlPathBar, pnlNav, pnlContentArea });
            this.ResumeLayout(false);
        }

        private void ActivateNav(int index)
        {
            for (int i = 0; i < _navPanels.Count; i++)
            {
                bool active = (i == index);
                _navAccents[i].BackColor = active ? ClrAccent : Color.Transparent;
                _navLabels[i].ForeColor  = active ? ClrText   : ClrMuted;
                _navLabels[i].Font       = new Font("Segoe UI", 8.5f, active ? FontStyle.Bold : FontStyle.Regular);
                _contentPanels[i].Visible = active;
            }
        }

        // ── Content 0: Scotopts.000 ───────────────────────────────────────────────

        private void BuildContent000(Panel p)
        {
            const int cw = 726;   // card width
            const int lw = 220;   // label width
            const int ix = 224;   // input x (label + 4)
            const int iw = 70;    // Y/N combo width
            const int cx = 400;   // col-2 label x
            const int ix2 = cx + lw + 4;  // col-2 input x (~624)
            const int rh = 26;    // row height

            int fy = 8;
            lbl000Status = MakeStatusLabel(12, fy, 720);
            p.Controls.Add(lbl000Status);
            fy += 24;

            // ── Card: SCOTSSF ─────────────────────────────────────────────────────
            var cSSF = MakeCard("RECEIPT  [SCOTSSF]", 12, fy, cw, 106);
            fy += 114;

            txt000ReceiptHeader          = MakeTextBox(ix, 30, 490);
            cbo000Enable194Receipt       = MakeYNCombo(ix, 30 + rh, iw);
            cbo000AllowRounding          = MakeYNCombo(ix2, 30 + rh, iw);
            cbo000HeadCashierRequired    = MakeYNCombo(ix, 30 + rh * 2, iw);
            cbo000TenderReversalPrompt   = MakeYNCombo(ix2, 30 + rh * 2, iw);

            cSSF.Controls.AddRange(new Control[]
            {
                FL("Receipt Header:",                       12,  32, lw), txt000ReceiptHeader,
                FL("Enable 194 Receipt:",                   12,  56 + 2, lw), cbo000Enable194Receipt,
                FL("Allow Rounding:",                      cx,  56 + 2, lw), cbo000AllowRounding,
                FL("Head Cashier Req. for Cash Mgmt:",     12,  82 + 2, lw), cbo000HeadCashierRequired,
                FL("Tender Reversal Prompt:",              cx,  82 + 2, lw), cbo000TenderReversalPrompt,
            });

            // ── Card: Operations ──────────────────────────────────────────────────
            var cOps = MakeCard("OPERATIONS  [Operations]", 12, fy, cw, 106);
            fy += 114;

            cbo000AllowAltID             = MakeYNCombo(ix,  30, iw);
            cbo000PartialIntegratedPINPad = MakeYNCombo(ix2, 30, iw);
            cbo000AllowContainerBtn      = MakeYNCombo(ix,  30 + rh, iw);
            cbo000CustomerBagAllowed     = MakeYNCombo(ix2, 30 + rh, iw);
            cbo000SellBagsAtFinalize     = MakeYNCombo(ix,  30 + rh * 2, iw);

            cOps.Controls.AddRange(new Control[]
            {
                FL("Allow Alt ID Entry:",                  12,  32, lw), cbo000AllowAltID,
                FL("Partial Integrated PINPad (PIP):",    cx,  32, lw), cbo000PartialIntegratedPINPad,
                FL("Allow Bag/Box Btn (weight alert):",   12,  58, lw), cbo000AllowContainerBtn,
                FL("Allow Customer Own Bag:",             cx,  58, lw), cbo000CustomerBagAllowed,
                FL("Sell Bags at Finalize:",              12,  84, lw), cbo000SellBagsAtFinalize,
            });

            // ── Card: State ───────────────────────────────────────────────────────
            var cState = MakeCard("STATE / DISPLAY  [State]", 12, fy, cw, 132);
            fy += 140;

            cbo000StartupToLaneClosed   = MakeYNCombo(ix,  30, iw);
            cbo000DualLanguage          = MakeYNCombo(ix2, 30, iw);
            cbo000SayPrices             = MakeYNCombo(ix,  30 + rh, iw);
            cbo000DisplayWeightsMeasures = MakeYNCombo(ix2, 30 + rh, iw);
            cbo000AllowLoyaltyCard      = MakeYNCombo(ix,  30 + rh * 2, iw);
            cbo000CouponSensor          = MakeYNCombo(ix2, 30 + rh * 2, iw);
            cbo000AssistMode            = MakeYNCombo(ix,  30 + rh * 3, iw);

            cState.Controls.AddRange(new Control[]
            {
                FL("Startup to Lane Closed:",             12,  32, lw), cbo000StartupToLaneClosed,
                FL("Dual Language:",                     cx,  32, lw), cbo000DualLanguage,
                FL("Say Prices (audio):",                12,  58, lw), cbo000SayPrices,
                FL("Display Weights & Measures:",        cx,  58, lw), cbo000DisplayWeightsMeasures,
                FL("Allow Loyalty Card:",                12,  84, lw), cbo000AllowLoyaltyCard,
                FL("Coupon Sensor (show insert screen):", cx, 84, lw), cbo000CouponSensor,
                FL("Assist Mode:",                       12, 110, lw), cbo000AssistMode,
            });

            // ── Card: CashBack ────────────────────────────────────────────────────
            var cCB = MakeCard("CASHBACK AMOUNTS  [Tender]", 12, fy, cw, 96);
            fy += 104;

            // 2 columns of 3 rows
            int[][] cbPairs = { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            int[] rys = { 30, 54, 78 };
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    int n  = cbPairs[r][c];
                    int bx = c == 0 ? 12 : 370;
                    var tb = MakeTextBox(bx + 86, rys[r], 70);
                    txt000CashBack[n] = tb;
                    cCB.Controls.Add(FL("CashBack" + n + ":", bx, rys[r] + 2, 84));
                    cCB.Controls.Add(tb);
                }
            }

            // Other Payment alongside CashBack card
            cCB.Controls.Add(FL("Other Payment:", 580, 32, 100));
            cbo000OtherPayment = MakeYNCombo(686, 30, iw);
            cCB.Controls.Add(cbo000OtherPayment);

            // ── Card: AMPM ────────────────────────────────────────────────────────
            var cAMPM = MakeCard("AMPM / LOYALTY  [AMPM]", 12, fy, cw, 56);
            fy += 64;

            cbo000MemberIDRequired = MakeYNCombo(ix, 30, iw);
            txt000DummyMemberID    = MakeTextBox(ix2, 30, 120);

            cAMPM.Controls.AddRange(new Control[]
            {
                FL("Member ID Required:",    12, 32, lw), cbo000MemberIDRequired,
                FL("Dummy Member ID:",      cx, 32, lw), txt000DummyMemberID,
            });

            var btn = MakeSaveButton("Save  Scotopts.000", 12, fy);
            btn.Click += btn000Save_Click;
            p.Controls.AddRange(new Control[] { cSSF, cOps, cState, cCB, cAMPM, btn });
        }

        // ── Content 1: Scotopts.dat ───────────────────────────────────────────────

        private void BuildContentDat(Panel p)
        {
            const int cw  = 726;
            const int lw  = 210;
            const int ix  = 214;
            const int iw  = 70;
            const int cx  = 390;
            const int ix2 = cx + lw + 4;

            int fy = 8;
            lblDatStatus = MakeStatusLabel(12, fy, 720);
            p.Controls.Add(lblDatStatus);
            fy += 24;

            // ── Card: Tender Allowed ──────────────────────────────────────────────
            var cAllow = MakeCard("TENDER ALLOWED  [Tender]", 12, fy, cw, 132);
            fy += 140;

            cboDatCash    = MakeYNCombo(ix,  30, iw);
            cboDatCredit  = MakeYNCombo(ix2 - 50, 30, iw);
            cboDatDebit   = MakeYNCombo(ix2 + 100, 30, iw);
            cboDatEBTFS   = MakeYNCombo(ix,  56, iw);
            cboDatEBTCB   = MakeYNCombo(ix2 - 50, 56, iw);
            cboDatOther   = MakeYNCombo(ix2 + 100, 56, iw);

            cboDatAllowCoupons   = MakeAONCombo(ix, 82, 70);
            cboDatShowUseCoupons = MakeYNCombo(ix2, 82, iw);
            cboDatIsMotorizedMSR = MakeYNCombo(ix,  108, iw);
            cboDatCancelPartialEBT = MakeYNCombo(ix2, 108, iw);

            cAllow.Controls.AddRange(new Control[]
            {
                FL("Cash Allowed:",           12,  32, lw - 20), cboDatCash,
                FL("Credit Allowed:",        265,  32, lw - 20), cboDatCredit,
                FL("Debit Allowed:",         505,  32, lw - 20), cboDatDebit,
                FL("EBT FS Allowed:",         12,  58, lw - 20), cboDatEBTFS,
                FL("EBT Cash Benefit:",      265,  58, lw - 20), cboDatEBTCB,
                FL("Other Payment:",         505,  58, lw - 20), cboDatOther,
                FL("Allow Coupons (A=All, O=Assist, N=None):", 12,  84, 200), cboDatAllowCoupons,
                FL("Show Use Coupons btn:",  cx,  84, lw), cboDatShowUseCoupons,
                FL("Motorized Card Reader:", 12, 110, lw), cboDatIsMotorizedMSR,
                FL("Cancel With Partial EBT:", cx, 110, lw), cboDatCancelPartialEBT,
            });

            // ── Card: Tender Settings ─────────────────────────────────────────────
            var cTSet = MakeCard("TENDER SETTINGS  [Tender]", 12, fy, cw, 56);
            fy += 64;

            txtDatPrintReceiptOver = MakeTextBox(ix, 30, 80);
            txtDatDenominations    = MakeTextBox(ix2, 30, 200);

            cTSet.Controls.AddRange(new Control[]
            {
                FL("Print Receipt Over $ (0=always):", 12, 32, lw), txtDatPrintReceiptOver,
                FL("Denominations:",                  cx, 32, lw), txtDatDenominations,
            });

            // ── Card: CashBack (regular) ──────────────────────────────────────────
            var cCB = MakeCard("CASHBACK AMOUNTS  [Tender]", 12, fy, cw, 106);
            fy += 114;
            AddCashBackGrid(cCB, txtDatCashBack, "CashBack", 1, 11);

            // ── Card: EBT CashBack ────────────────────────────────────────────────
            var cEBT = MakeCard("EBT CASHBACK AMOUNTS  [Tender]", 12, fy, cw, 106);
            fy += 114;
            AddCashBackGrid(cEBT, txtDatEBTCashBack, "EBTCashBack", 1, 11);

            // ── Card: Operations ──────────────────────────────────────────────────
            var cOps = MakeCard("OPERATIONS  [Operations]", 12, fy, cw, 184);
            fy += 192;

            cboDatAllowDegradedMode  = MakeYNCombo(ix, 30, iw);
            cboDatAllowSkipBagging   = MakeYNCombo(ix2, 30, iw);
            cboDatAllowContainerBtn  = MakeYNCombo(ix, 56, iw);
            cboDatSellBagsAtFinalize = MakeYNCombo(ix2, 56, iw);
            cboDatShowVolumeControl  = MakeYNCombo(ix, 82, iw);
            cboDatAllowCancelAll     = MakeYNCombo(ix2, 82, iw);
            cboDatSendSignature      = MakeYNCombo(ix, 108, iw);
            txtDatMaxCoupon          = MakeTextBox(ix2, 108, 80);
            // Gift card row
            cboDatGiftCardRedemption  = Make012Combo(ix, 134, 120);
            cboDatDisplayGiftCard     = MakeYNCombo(ix2, 134, iw);
            cboDatMethodEmptyGiftCard = Make012Combo(ix, 160, 120);

            cOps.Controls.AddRange(new Control[]
            {
                FL("Allow Degraded Mode:",               12,  32, lw), cboDatAllowDegradedMode,
                FL("Allow Skip Bagging Button:",         cx,  32, lw), cboDatAllowSkipBagging,
                FL("Allow Bag/Box Btn (weight alert):",  12,  58, lw), cboDatAllowContainerBtn,
                FL("Sell Bags at Finalize:",             cx,  58, lw), cboDatSellBagsAtFinalize,
                FL("Show Volume Control (attract):",     12,  84, lw), cboDatShowVolumeControl,
                FL("Allow Cancel All:",                  cx,  84, lw), cboDatAllowCancelAll,
                FL("Send Signature Data to TB:",         12, 110, lw), cboDatSendSignature,
                FL("Max Coupon Value (100=$1):",         cx, 110, lw), txtDatMaxCoupon,
                FL("Gift Card (0=Off,1=Scan,2=Swipe):", 12, 136, lw), cboDatGiftCardRedemption,
                FL("Display Gift Card Balance:",         cx, 136, lw), cboDatDisplayGiftCard,
                FL("Empty Gift Card: 0=No,1=Slot,2=Cashier:", 12, 162, lw), cboDatMethodEmptyGiftCard,
            });

            // ── Card: State / Display ─────────────────────────────────────────────
            var cState = MakeCard("STATE / DISPLAY  [State]", 12, fy, cw, 132);
            fy += 140;

            cboDatSayPrices        = MakeYNCombo(ix, 30, iw);
            cboDatSayPhrases       = MakeYNCombo(ix2, 30, iw);
            cboDatAllowLoyaltyCard = MakeYNCombo(ix, 56, iw);
            cboDatCouponSensor     = MakeYNCombo(ix2, 56, iw);
            cboDatStartupClosed    = MakeYNCombo(ix, 82, iw);
            cboDatAssistMode       = MakeYNCombo(ix2, 82, iw);
            cboDatLiveVideo        = MakeYNCombo(ix, 108, iw);
            txtDatMovieOnAttract   = MakeTextBox(ix2, 108, 200);

            cState.Controls.AddRange(new Control[]
            {
                FL("Say Prices (audio):",            12,  32, lw), cboDatSayPrices,
                FL("Say Phrases (audio):",           cx,  32, lw), cboDatSayPhrases,
                FL("Allow Loyalty Card:",            12,  58, lw), cboDatAllowLoyaltyCard,
                FL("Coupon Sensor (insert screen):", cx,  58, lw), cboDatCouponSensor,
                FL("Startup to Lane Closed:",        12,  84, lw), cboDatStartupClosed,
                FL("Assist Mode:",                   cx,  84, lw), cboDatAssistMode,
                FL("Live Video:",                    12, 110, lw), cboDatLiveVideo,
                FL("Attract Screen Video File:",     cx, 110, lw), txtDatMovieOnAttract,
            });

            // ── Card: Tracing + Timeouts ──────────────────────────────────────────
            var cMisc = MakeCard("TRACING & TIMEOUTS  [Tracing] [TimeOut]", 12, fy, cw, 106);
            fy += 114;

            txtDatTraceLevel       = MakeTextBox(ix,  30, 60);
            txtDatTimeoutTender    = MakeTextBox(ix,  56, 60);
            txtDatTimeoutAttract   = MakeTextBox(ix2, 56, 60);
            txtDatTimeoutThankYou  = MakeTextBox(ix,  82, 60);
            txtDatTimeoutItemSale  = MakeTextBox(ix2, 82, 60);
            txtDatTimeoutTakeReceipt = MakeTextBox(cx + lw + 4, 30, 60);

            cMisc.Controls.AddRange(new Control[]
            {
                FL("Trace Level (0-7, 6=verbose):", 12, 32, lw), txtDatTraceLevel,
                FL("Take Receipt timeout (sec):",   cx, 32, lw), txtDatTimeoutTakeReceipt,
                FL("Tender timeout (sec):",         12, 58, lw), txtDatTimeoutTender,
                FL("Attract timeout (sec):",        cx, 58, lw), txtDatTimeoutAttract,
                FL("Thank You timeout (sec):",      12, 84, lw), txtDatTimeoutThankYou,
                FL("Item Sale In Progress (sec, 0=no timeout):", cx, 84, lw - 10), txtDatTimeoutItemSale,
            });

            var btn = MakeSaveButton("Save  Scotopts.dat", 12, fy);
            btn.Click += btnDatSave_Click;
            p.Controls.AddRange(new Control[] { cAllow, cTSet, cCB, cEBT, cOps, cState, cMisc, btn });
        }

        // ── Content 2: SCOTTend.dat ───────────────────────────────────────────────

        private void BuildContentTend(Panel p)
        {
            int fy = 8;
            lblTendStatus = MakeStatusLabel(12, fy, 720);
            p.Controls.Add(lblTendStatus);
            fy += 28;

            // Column headers
            int[] colX = { 12, 58, 248, 328, 412, 494 };
            int[] colW = { 42, 188, 78,  82,  80,  80  };
            string[] hdrs = { "Btn", "Text", "Type", "Cash Back", "Need PIN" };
            var hdr = new Panel { Location = new Point(12, fy), Size = new Size(726, 22), BackColor = ClrSurface2 };
            for (int h = 0; h < hdrs.Length; h++)
                hdr.Controls.Add(new Label
                {
                    Text      = hdrs[h],
                    Location  = new Point(colX[h], 0),
                    Size      = new Size(colW[h], 22),
                    Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = ClrMuted,
                    BackColor = Color.Transparent,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                });
            p.Controls.Add(hdr);
            fy += 24;

            int[] buttons = { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12 };
            var card = MakeCard("TENDER BUTTONS", 12, fy, 726, buttons.Length * 28 + 32);
            fy += buttons.Length * 28 + 40;

            for (int r = 0; r < buttons.Length; r++)
            {
                int b  = buttons[r];
                int ry = 30 + r * 28;
                var lbl = new Label { Text = b.ToString(), Location = new Point(colX[0], ry + 2), Size = new Size(colW[0], 22), Font = new Font("Segoe UI", 8.5f), ForeColor = ClrAccent, BackColor = Color.Transparent, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
                var tTxt  = MakeTextBox(colX[1], ry, colW[1] - 4);
                var tType = MakeTextBox(colX[2], ry, colW[2] - 4);
                var tCB   = MakeTextBox(colX[3], ry, colW[3] - 4);
                var tPin  = MakeTextBox(colX[4], ry, colW[4] - 4);
                _tendText[b] = tTxt; _tendType[b] = tType; _tendCashBack[b] = tCB; _tendNeedPin[b] = tPin;
                card.Controls.AddRange(new Control[] { lbl, tTxt, tType, tCB, tPin });
            }

            var btn = MakeSaveButton("Save  SCOTTend.dat", 12, fy);
            btn.Click += btnTendSave_Click;
            p.Controls.AddRange(new Control[] { card, btn });
        }

        // ── Content 3: SSCOStrings ────────────────────────────────────────────────

        private void BuildContentStrings(Panel p)
        {
            int fy = 8;
            lblStrStatus = MakeStatusLabel(12, fy, 720);
            p.Controls.Add(lblStrStatus);
            fy += 24;

            string[] strKeys =
            {
                "ScotApp_3", "ScotApp_4", "ScotApp_13", "ScotApp_22",
                "ScotApp_48", "ScotApp_49", "ScotApp_92", "ScotApp_329",
                "ScotApp_421", "ScotApp_489", "ScotApp_490", "ScotApp_523",
                "ScotApp_664", "ScotApp_674", "ScotApp_733", "ScotApp_773",
                "ScotApp_774", "ScotApp_775", "ScotApp_790", "ScotApp_812",
                "ScotApp_965", "ScotApp_966", "ScotApp_1108", "ScotApp_1109",
                "ScotApp_1112", "ScotApp_1113", "ScotApp_1244", "ScotApp_1245",
                "ScotApp_1260",
                "ScotApp_9001", "ScotApp_9002", "ScotApp_9003", "ScotApp_9004",
                "ScotApp_9005", "ScotApp_9006", "ScotApp_9007", "ScotApp_9008",
                "ScotApp_9009", "ScotApp_9010", "ScotApp_9011", "ScotApp_9012",
                "ScotApp_9013", "ScotApp_9014", "ScotApp_9015", "ScotApp_9016",
                "ScotApp_9017", "ScotApp_9018", "ScotApp_9019", "ScotApp_9020",
                "ScotApp_9021", "ScotApp_9022", "ScotApp_9023", "ScotApp_9024",
                "ScotApp_9025", "ScotApp_9026", "ScotApp_9027", "ScotApp_9028",
                "ScotApp_9029", "ScotApp_9030", "ScotApp_9031", "ScotApp_9032",
                "ScotApp_9033", "ScotApp_9034", "ScotApp_9035", "ScotApp_9036",
                "ScotApp_9037",
                "TransDiscount1ButtonText", "TransDiscount2ButtonText"
            };

            int cardH = strKeys.Length * 26 + 34;
            var card = MakeCard("CUSTOM STRINGS  (SSCOStrings.en-US.custom.dat)", 12, fy, 726, cardH);
            fy += cardH + 8;

            for (int i = 0; i < strKeys.Length; i++)
            {
                int ry = 30 + i * 26;
                var tb = MakeTextBox(148, ry, 560);
                _stringBoxes[strKeys[i]] = tb;
                card.Controls.Add(FL(strKeys[i] + ":", 12, ry + 2, 134));
                card.Controls.Add(tb);
            }

            var btn = MakeSaveButton("Save  SSCOStrings", 12, fy);
            btn.Click += btnStrSave_Click;
            p.Controls.AddRange(new Control[] { card, btn });
        }

        // ── Content 4: SSCOUI.exe.config ──────────────────────────────────────────

        private void BuildContentXml(Panel p)
        {
            const int lw = 200;
            const int ix = 204;

            int fy = 8;
            lblXmlStatus = MakeStatusLabel(12, fy, 720);
            p.Controls.Add(lblXmlStatus);
            fy += 24;

            var cRestart = MakeCard("AUTO RESTART  [SSCOUI.Properties.Settings]", 12, fy, 726, 76);
            fy += 84;

            txtXmlAutoRestart  = MakeTextBox(ix, 30, 506);
            txtXmlRestartStates = MakeTextBox(ix, 54, 506);
            cRestart.Controls.AddRange(new Control[]
            {
                FL("AutoRestartOptions:", 12, 32, lw), txtXmlAutoRestart,
                FL("AutoRestart States:", 12, 56, lw), txtXmlRestartStates,
            });

            var cDisplay = MakeCard("DISPLAY  [SSCOUI.Properties.Settings]", 12, fy, 726, 96);
            fy += 104;

            cboXmlShowGoToPOS = MakeTFCombo(ix, 30, 80);
            cboXmlShowCursor  = MakeTFCombo(ix, 54, 80);
            cboXmlLiveVideo   = MakeTFCombo(ix, 78, 80);
            cDisplay.Controls.AddRange(new Control[]
            {
                FL("Show Go-To-POS Widget:", 12, 32, lw), cboXmlShowGoToPOS,
                FL("Show Cursor:",           12, 56, lw), cboXmlShowCursor,
                FL("Next Gen UI Live Video:", 12, 80, lw), cboXmlLiveVideo,
            });

            var cImages = MakeCard("IMAGE PATHS  [SSCOUI.Properties.Settings]", 12, fy, 726, 120);
            fy += 128;

            txtXmlLogoImage    = MakeTextBox(ix, 30,  506);
            txtXmlBgImage      = MakeTextBox(ix, 54,  506);
            txtXmlWelcomeImage = MakeTextBox(ix, 78,  506);
            txtXmlFinishImage  = MakeTextBox(ix, 102, 506);
            cImages.Controls.AddRange(new Control[]
            {
                FL("Logo Image:",       12, 32, lw), txtXmlLogoImage,
                FL("Background Image:", 12, 56, lw), txtXmlBgImage,
                FL("Welcome Image:",    12, 80, lw), txtXmlWelcomeImage,
                FL("Finish Image:",     12, 104, lw), txtXmlFinishImage,
            });

            var btn = MakeSaveButton("Save  SSCOUI.exe.config", 12, fy);
            btn.Click += btnXmlSave_Click;
            p.Controls.AddRange(new Control[] { cRestart, cDisplay, cImages, btn });
        }

        // ── UI helpers ────────────────────────────────────────────────────────────

        private static Panel MakeCard(string title, int x, int y, int width, int height)
        {
            var panel = new Panel { Location = new Point(x, y), Size = new Size(width, height), BackColor = ClrSurface };
            panel.Controls.Add(new Panel { Location = new Point(0, 0), Size = new Size(3, height), BackColor = ClrAccent });
            panel.Controls.Add(new Label { Text = title, Location = new Point(12, 5), Size = new Size(width - 20, 16), Font = new Font("Segoe UI", 7f, FontStyle.Bold), ForeColor = ClrMuted, BackColor = Color.Transparent, TextAlign = System.Drawing.ContentAlignment.MiddleLeft });
            panel.Controls.Add(new Panel { Location = new Point(12, 22), Size = new Size(width - 15, 1), BackColor = ClrBorder });
            return panel;
        }

        private static Label FL(string text, int x, int y, int width)
            => new Label { Text = text, Location = new Point(x, y), Size = new Size(width, 20), TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8.5f), ForeColor = ClrMuted, BackColor = Color.Transparent };

        private static Label MakeStatusLabel(int x, int y, int width)
            => new Label { Text = "", Location = new Point(x, y), Size = new Size(width, 16), Font = new Font("Segoe UI", 7.5f), ForeColor = ClrMuted, BackColor = Color.Transparent };

        private static TextBox MakeTextBox(int x, int y, int width)
            => new TextBox { Location = new Point(x, y), Size = new Size(width, 22), BackColor = ClrInput, ForeColor = ClrInputFg, BorderStyle = BorderStyle.FixedSingle };

        private static ComboBox MakeYNCombo(int x, int y, int width)
        {
            var c = new ComboBox { Location = new Point(x, y), Size = new Size(width, 22), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = ClrInput, ForeColor = ClrInputFg };
            c.Items.AddRange(new object[] { "Y", "N" });
            return c;
        }

        private static ComboBox MakeAONCombo(int x, int y, int width)
        {
            var c = new ComboBox { Location = new Point(x, y), Size = new Size(width, 22), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = ClrInput, ForeColor = ClrInputFg };
            c.Items.AddRange(new object[] { "A", "O", "N" });
            return c;
        }

        private static ComboBox Make012Combo(int x, int y, int width)
        {
            var c = new ComboBox { Location = new Point(x, y), Size = new Size(width, 22), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = ClrInput, ForeColor = ClrInputFg };
            c.Items.AddRange(new object[] { "0", "1", "2" });
            return c;
        }

        private static ComboBox MakeTFCombo(int x, int y, int width)
        {
            var c = new ComboBox { Location = new Point(x, y), Size = new Size(width, 22), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = ClrInput, ForeColor = ClrInputFg };
            c.Items.AddRange(new object[] { "True", "False" });
            return c;
        }

        private static Button MakeSaveButton(string text, int x, int y)
            => new Button { Text = text, Location = new Point(x, y), Size = new Size(220, 36), BackColor = Color.FromArgb(31, 111, 235), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), Cursor = Cursors.Hand };

        private static void AddCashBackGrid(Panel card, Dictionary<int, TextBox> dict, string prefix, int start, int end)
        {
            int[] xs = { 12, 190, 368, 546 };
            int labelW = 96, inputW = 58, rowH = 26;
            int row = 0, col = 0;
            for (int i = start; i <= end; i++)
            {
                int y  = 30 + row * rowH;
                int x  = xs[col];
                var tb = MakeTextBox(x + labelW + 2, y, inputW);
                dict[i] = tb;
                card.Controls.Add(FL(prefix + i + ":", x, y + 2, labelW));
                card.Controls.Add(tb);
                col++;
                if (col >= 4) { col = 0; row++; }
            }
        }
    }
}
