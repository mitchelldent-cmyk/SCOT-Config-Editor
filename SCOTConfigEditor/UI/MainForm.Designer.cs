using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SCOTConfigEditor.UI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Colour palette (matches SCO Lane app) ─────────────────────────────────
        private static readonly Color ClrBack     = Color.FromArgb( 13,  17,  23);
        private static readonly Color ClrSurface  = Color.FromArgb( 22,  27,  34);
        private static readonly Color ClrSurface2 = Color.FromArgb( 33,  38,  45);
        private static readonly Color ClrInput    = Color.FromArgb( 22,  27,  34);
        private static readonly Color ClrInputFg  = Color.FromArgb(201, 209, 217);
        private static readonly Color ClrText     = Color.FromArgb(230, 237, 243);
        private static readonly Color ClrMuted    = Color.FromArgb(139, 148, 158);
        private static readonly Color ClrAccent   = Color.FromArgb( 88, 166, 255);
        private static readonly Color ClrBorder   = Color.FromArgb( 48,  54,  61);

        // ── Navigation ────────────────────────────────────────────────────────────
        private readonly List<Panel>  _navPanels  = new List<Panel>();
        private readonly List<Panel>  _navAccents = new List<Panel>();
        private readonly List<Label>  _navLabels  = new List<Label>();
        private readonly List<Panel>  _contentPanels = new List<Panel>();

        // ── Tab 1: Scotopts.000 ───────────────────────────────────────────────────
        private Label   lbl000Status;
        private TextBox txt000ReceiptHeader;
        private ComboBox cbo000AllowAltID;
        private readonly Dictionary<int, TextBox> txt000CashBack = new Dictionary<int, TextBox>();
        private ComboBox cbo000MemberIDRequired;
        private TextBox  txt000DummyMemberID;

        // ── Tab 2: Scotopts.dat ───────────────────────────────────────────────────
        private Label    lblDatStatus;
        private ComboBox cboDatCash, cboDatCredit, cboDatDebit;
        private ComboBox cboDatEBTFS, cboDatEBTCB, cboDatOther;
        private readonly Dictionary<int, TextBox> txtDatCashBack    = new Dictionary<int, TextBox>();
        private readonly Dictionary<int, TextBox> txtDatEBTCashBack = new Dictionary<int, TextBox>();

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
            this.ClientSize    = new Size(880, 720);
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
                Size      = new Size(880, 55),
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
                Size      = new Size(880, 2),
                BackColor = ClrAccent,
                Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // ── Left nav (160px) ─────────────────────────────────────────────────
            var pnlNav = new Panel
            {
                Location  = new Point(0, 57),
                Size      = new Size(160, 663),
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
                int idx = i;
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
                Location  = new Point(160, 57),
                Size      = new Size(720, 663),
                BackColor = ClrBack,
                Anchor    = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Build all 5 content panels
            for (int i = 0; i < 5; i++)
            {
                var cp = new Panel
                {
                    Location   = new Point(0, 0),
                    Size       = new Size(720, 663),
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

            this.Controls.AddRange(new Control[] { pnlHeader, pnlAccentLine, pnlNav, pnlContentArea });
            this.ResumeLayout(false);
        }

        // ── Navigation ────────────────────────────────────────────────────────────

        private void ActivateNav(int index)
        {
            for (int i = 0; i < _navPanels.Count; i++)
            {
                bool active = (i == index);
                _navAccents[i].BackColor = active ? ClrAccent : Color.Transparent;
                _navLabels[i].ForeColor  = active ? ClrText   : ClrMuted;
                _navLabels[i].Font       = new Font("Segoe UI", 8.5f,
                    active ? FontStyle.Bold : FontStyle.Regular);
                _contentPanels[i].Visible = active;
            }
        }

        // ── Content 0: Scotopts.000 ───────────────────────────────────────────────

        private void BuildContent000(Panel p)
        {
            int fy = 8;

            lbl000Status = MakeStatusLabel(12, fy, 680);
            p.Controls.Add(lbl000Status);
            fy += 24;

            // Card: Receipt & Operations
            var c1 = MakeCard("RECEIPT & OPERATIONS", 12, fy, 686, 76);
            fy += 84;

            txt000ReceiptHeader = MakeTextBox(148, 30, 480);
            cbo000AllowAltID    = MakeYNCombo(148, 54, 70);

            c1.Controls.AddRange(new Control[]
            {
                MakeFieldLabel("Receipt Header:",   12, 32, 130), txt000ReceiptHeader,
                MakeFieldLabel("Allow Alt ID Entry:", 12, 56, 130), cbo000AllowAltID,
            });

            // Card: CashBack Amounts
            var c2 = MakeCard("CASHBACK AMOUNTS  (Scotopts.000 [Tender])", 12, fy, 686, 96);
            fy += 104;

            int[] cbRows = { 30, 54, 78 };
            int[][] cbCols = { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
            int[] colX = { 12, 190, 368 };

            for (int row = 0; row < cbRows.Length; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    int n = cbCols[row][col];
                    int x = colX[col * 1 == 0 ? 0 : col == 1 ? 1 : 2];
                    x = col == 0 ? 12 : 190;
                    var tb = MakeTextBox(x + 80, cbRows[row], 72);
                    txt000CashBack[n] = tb;
                    c2.Controls.Add(MakeFieldLabel("CashBack" + n + ":", x, cbRows[row] + 2, 78));
                    c2.Controls.Add(tb);
                }
            }

            // Card: AMPM / Loyalty
            var c3 = MakeCard("AMPM / LOYALTY", 12, fy, 686, 76);
            fy += 84;

            cbo000MemberIDRequired = MakeYNCombo(148, 30, 70);
            txt000DummyMemberID    = MakeTextBox(148, 54, 120);

            c3.Controls.AddRange(new Control[]
            {
                MakeFieldLabel("Member ID Required:", 12, 32, 130), cbo000MemberIDRequired,
                MakeFieldLabel("Dummy Member ID:",    12, 56, 130), txt000DummyMemberID,
            });

            var btn = MakeSaveButton("Save  Scotopts.000", 12, fy);
            btn.Click += btn000Save_Click;

            p.Controls.AddRange(new Control[] { c1, c2, c3, btn });
        }

        // ── Content 1: Scotopts.dat ───────────────────────────────────────────────

        private void BuildContentDat(Panel p)
        {
            int fy = 8;

            lblDatStatus = MakeStatusLabel(12, fy, 680);
            p.Controls.Add(lblDatStatus);
            fy += 24;

            // Card: Tender Allowed
            var cAllow = MakeCard("TENDER ALLOWED", 12, fy, 686, 96);
            fy += 104;

            cboDatCash    = MakeYNCombo(120, 30, 60);
            cboDatCredit  = MakeYNCombo(358, 30, 60);
            cboDatDebit   = MakeYNCombo(596, 30, 60);
            cboDatEBTFS   = MakeYNCombo(120, 58, 60);
            cboDatEBTCB   = MakeYNCombo(358, 58, 60);
            cboDatOther   = MakeYNCombo(596, 58, 60);

            cAllow.Controls.AddRange(new Control[]
            {
                MakeFieldLabel("Cash Allowed:",    12,  32, 106), cboDatCash,
                MakeFieldLabel("Credit Allowed:", 248,  32, 108), cboDatCredit,
                MakeFieldLabel("Debit Allowed:",  486,  32, 108), cboDatDebit,
                MakeFieldLabel("EBT FS Allowed:",  12,  60, 106), cboDatEBTFS,
                MakeFieldLabel("EBT CB Allowed:", 248,  60, 108), cboDatEBTCB,
                MakeFieldLabel("Other Payment:",  486,  60, 108), cboDatOther,
            });

            // Card: CashBack (regular)
            var cCB = MakeCard("CASHBACK AMOUNTS  (Regular, [Tender])", 12, fy, 686, 106);
            fy += 114;

            AddCashBackGrid(cCB, txtDatCashBack, "CashBack", 1, 11);

            // Card: EBT CashBack
            var cEBT = MakeCard("CASHBACK AMOUNTS  (EBT, [Tender])", 12, fy, 686, 106);
            fy += 114;

            AddCashBackGrid(cEBT, txtDatEBTCashBack, "EBTCashBack", 1, 11);

            var btn = MakeSaveButton("Save  Scotopts.dat", 12, fy);
            btn.Click += btnDatSave_Click;

            p.Controls.AddRange(new Control[] { cAllow, cCB, cEBT, btn });
        }

        private static void AddCashBackGrid(Panel card, Dictionary<int, TextBox> dict,
            string prefix, int start, int end)
        {
            // 4 per row layout
            int[] xs = { 12, 180, 348, 516 };
            int labelW = 84, inputW = 64, rowH = 26;
            int row = 0, col = 0;

            for (int i = start; i <= end; i++)
            {
                int y = 30 + row * rowH;
                int x = xs[col];
                var tb = MakeTextBox(x + labelW + 2, y, inputW);
                dict[i] = tb;
                card.Controls.Add(MakeFieldLabel(prefix + i + ":", x, y + 2, labelW));
                card.Controls.Add(tb);
                col++;
                if (col >= 4) { col = 0; row++; }
            }
        }

        // ── Content 2: SCOTTend.dat ───────────────────────────────────────────────

        private void BuildContentTend(Panel p)
        {
            int fy = 8;

            lblTendStatus = MakeStatusLabel(12, fy, 680);
            p.Controls.Add(lblTendStatus);
            fy += 24;

            // Table header
            int[] colX2   = { 12, 60, 240, 318, 398, 476 };
            int[] colW2   = { 44, 178, 76, 78, 76, 76 };
            string[] hdrs = { "Btn", "Text", "Type", "Cash Back", "Need PIN", "" };

            var hdrPanel = new Panel
            {
                Location  = new Point(12, fy),
                Size      = new Size(686, 22),
                BackColor = ClrSurface2
            };
            for (int h = 0; h < hdrs.Length - 1; h++)
                hdrPanel.Controls.Add(new Label
                {
                    Text      = hdrs[h],
                    Location  = new Point(colX2[h], 0),
                    Size      = new Size(colW2[h], 22),
                    Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = ClrMuted,
                    BackColor = Color.Transparent,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                });
            p.Controls.Add(hdrPanel);
            fy += 24;

            var card = MakeCard("TENDER BUTTONS", 12, fy, 686, 11 * 28 + 32);
            fy += 11 * 28 + 40;

            int[] buttons = { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12 };
            for (int r = 0; r < buttons.Length; r++)
            {
                int b  = buttons[r];
                int ry = 30 + r * 28;

                var lblBtn = new Label
                {
                    Text      = b.ToString(),
                    Location  = new Point(colX2[0], ry + 2),
                    Size      = new Size(colW2[0], 22),
                    Font      = new Font("Segoe UI", 8.5f),
                    ForeColor = ClrAccent,
                    BackColor = Color.Transparent,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                };

                var txtTxt  = MakeTextBox(colX2[1], ry, colW2[1] - 4);
                var txtType = MakeTextBox(colX2[2], ry, colW2[2] - 4);
                var txtCB   = MakeTextBox(colX2[3], ry, colW2[3] - 4);
                var txtPin  = MakeTextBox(colX2[4], ry, colW2[4] - 4);

                _tendText[b]     = txtTxt;
                _tendType[b]     = txtType;
                _tendCashBack[b] = txtCB;
                _tendNeedPin[b]  = txtPin;

                card.Controls.AddRange(new Control[] { lblBtn, txtTxt, txtType, txtCB, txtPin });
            }

            var btn = MakeSaveButton("Save  SCOTTend.dat", 12, fy);
            btn.Click += btnTendSave_Click;
            p.Controls.AddRange(new Control[] { card, btn });
        }

        // ── Content 3: SSCOStrings ────────────────────────────────────────────────

        private void BuildContentStrings(Panel p)
        {
            int fy = 8;

            lblStrStatus = MakeStatusLabel(12, fy, 680);
            p.Controls.Add(lblStrStatus);
            fy += 24;

            // All string keys in the file
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

            int cardHeight = strKeys.Length * 26 + 34;
            var card = MakeCard("CUSTOM STRINGS  (SSCOStrings.en-US.custom.dat)", 12, fy, 686, cardHeight);
            fy += cardHeight + 8;

            for (int i = 0; i < strKeys.Length; i++)
            {
                int ry = 30 + i * 26;
                var tb = MakeTextBox(148, ry, 520);
                _stringBoxes[strKeys[i]] = tb;
                card.Controls.Add(MakeFieldLabel(strKeys[i] + ":", 12, ry + 2, 134));
                card.Controls.Add(tb);
            }

            var btn = MakeSaveButton("Save  SSCOStrings", 12, fy);
            btn.Click += btnStrSave_Click;
            p.Controls.AddRange(new Control[] { card, btn });
        }

        // ── Content 4: SSCOUI.exe.config ──────────────────────────────────────────

        private void BuildContentXml(Panel p)
        {
            int fy = 8;

            lblXmlStatus = MakeStatusLabel(12, fy, 680);
            p.Controls.Add(lblXmlStatus);
            fy += 24;

            // Card: Auto Restart
            var cRestart = MakeCard("AUTO RESTART", 12, fy, 686, 76);
            fy += 84;

            txtXmlAutoRestart  = MakeTextBox(160, 30, 506);
            txtXmlRestartStates = MakeTextBox(160, 54, 506);

            cRestart.Controls.AddRange(new Control[]
            {
                MakeFieldLabel("AutoRestartOptions:",    12, 32, 146), txtXmlAutoRestart,
                MakeFieldLabel("AutoRestartStates:",     12, 56, 146), txtXmlRestartStates,
            });

            // Card: Display
            var cDisplay = MakeCard("DISPLAY SETTINGS", 12, fy, 686, 96);
            fy += 104;

            cboXmlShowGoToPOS  = MakeTFCombo(160, 30, 80);
            cboXmlShowCursor   = MakeTFCombo(160, 54, 80);
            cboXmlLiveVideo    = MakeTFCombo(160, 78, 80);

            cDisplay.Controls.AddRange(new Control[]
            {
                MakeFieldLabel("Show GoToPOS Widget:", 12, 32, 146), cboXmlShowGoToPOS,
                MakeFieldLabel("Show Cursor:",          12, 56, 146), cboXmlShowCursor,
                MakeFieldLabel("Live Video (NG UI):",   12, 80, 146), cboXmlLiveVideo,
            });

            // Card: Images
            var cImages = MakeCard("IMAGE PATHS  (relative or absolute)", 12, fy, 686, 120);
            fy += 128;

            txtXmlLogoImage   = MakeTextBox(160, 30, 506);
            txtXmlBgImage     = MakeTextBox(160, 54, 506);
            txtXmlWelcomeImage = MakeTextBox(160, 78, 506);
            txtXmlFinishImage  = MakeTextBox(160, 102, 506);

            cImages.Controls.AddRange(new Control[]
            {
                MakeFieldLabel("Logo Image:",       12, 32,  146), txtXmlLogoImage,
                MakeFieldLabel("Background Image:", 12, 56,  146), txtXmlBgImage,
                MakeFieldLabel("Welcome Image:",    12, 80,  146), txtXmlWelcomeImage,
                MakeFieldLabel("Finish Image:",     12, 104, 146), txtXmlFinishImage,
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
            panel.Controls.Add(new Label
            {
                Text      = title,
                Location  = new Point(12, 5),
                Size      = new Size(width - 20, 16),
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = ClrMuted,
                BackColor = Color.Transparent,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            });
            panel.Controls.Add(new Panel
            {
                Location  = new Point(12, 22),
                Size      = new Size(width - 15, 1),
                BackColor = ClrBorder
            });
            return panel;
        }

        private static Label MakeFieldLabel(string text, int x, int y, int width)
            => new Label
            {
                Text      = text,
                Location  = new Point(x, y),
                Size      = new Size(width, 20),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = ClrMuted,
                BackColor = Color.Transparent
            };

        private static Label MakeStatusLabel(int x, int y, int width)
            => new Label
            {
                Text      = "",
                Location  = new Point(x, y),
                Size      = new Size(width, 16),
                Font      = new Font("Segoe UI", 7.5f),
                ForeColor = ClrMuted,
                BackColor = Color.Transparent
            };

        private static TextBox MakeTextBox(int x, int y, int width)
            => new TextBox
            {
                Location    = new Point(x, y),
                Size        = new Size(width, 22),
                BackColor   = ClrInput,
                ForeColor   = ClrInputFg,
                BorderStyle = BorderStyle.FixedSingle
            };

        private static ComboBox MakeYNCombo(int x, int y, int width)
        {
            var cbo = new ComboBox
            {
                Location      = new Point(x, y),
                Size          = new Size(width, 22),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = ClrInput,
                ForeColor     = ClrInputFg
            };
            cbo.Items.AddRange(new object[] { "Y", "N" });
            return cbo;
        }

        private static ComboBox MakeTFCombo(int x, int y, int width)
        {
            var cbo = new ComboBox
            {
                Location      = new Point(x, y),
                Size          = new Size(width, 22),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = ClrInput,
                ForeColor     = ClrInputFg
            };
            cbo.Items.AddRange(new object[] { "True", "False" });
            return cbo;
        }

        private static Button MakeSaveButton(string text, int x, int y)
            => new Button
            {
                Text      = text,
                Location  = new Point(x, y),
                Size      = new Size(200, 36),
                BackColor = Color.FromArgb(31, 111, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
    }
}
