using System;
using System.Drawing;
using System.Windows.Forms;
using KasihkuPBO.View;

namespace KasihkuPBO
{
    public partial class FormAdmin : Form
    {
        private Panel panelContent;
        private Label lblWelcome;
        private RiwayatAdminControl riwayatControl;

        private Button btnProduk, btnTransaksi, btnRiwayat, btnManajemen, btnLogout;

        public FormAdmin(string username)
        {
            InitializeComponent();

            lblWelcome = label1;
            btnProduk = btnAdminProduk;
            btnTransaksi = btnAdminTransaksi;
            btnRiwayat = btnAdminRiwayat;
            btnManajemen = btnAdminManajemen;
            btnLogout = button1;

            lblWelcome.Text = "Selamat datang di aplikasi Kasihku, " + username;

            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                Size = new Size(1920, 1080),
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(panelContent);

            riwayatControl = new RiwayatAdminControl();
            riwayatControl.Dock = DockStyle.Fill;
            this.Controls.Add(riwayatControl);

            panelContent.Visible = false;
            riwayatControl.Visible = false;

            riwayatControl.KembaliClicked += () =>
            {
                panelContent.Visible = false;
                riwayatControl.Visible = false;
            };
        }


        private void TampilkanKontrol(ProdukAdminControl produkControl)
        {
            ToggleMainUI(false);

            panelContent.Controls.Clear();

            Button btnKembali = new Button
            {
                Text = "⮌ Kembali", 
                Location = new Point(413, 180),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Size = new Size(265, 53),
                BackColor = Color.FromArgb(33, 88, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnKembali.Click += (s, e) =>
            {
                panelContent.Visible = false;
                ToggleMainUI(true);
            };
            panelContent.Controls.Add(btnKembali);

            produkControl.ShowGridOnly();

            produkControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(produkControl);

            panelContent.Visible = true;
            riwayatControl.Visible = false;
            panelContent.BringToFront();
        }

        private void ShowRiwayatControl()
        {
            panelContent.Visible = false;
            riwayatControl.Visible = true;

            riwayatControl.BringToFront();
        }

        private void ToggleMainUI(bool visible)
        {
            lblWelcome.Visible = visible;
            btnProduk.Visible = visible;
            btnTransaksi.Visible = visible;
            btnRiwayat.Visible = visible;
            btnManajemen.Visible = visible;
            btnLogout.Visible = visible;
        }

        private void btnAdminManajemen_Click(object sender, EventArgs e)
        {
            TampilkanKontrol(new ProdukAdminControl());
        }

        private void btnAdminProduk_Click(object sender, EventArgs e)
        {
            TampilkanKontrol(new ProdukAdminControl());
        }

        private void btnAdminTransaksi_Click(object sender, EventArgs e)
        {
        }

        private void btnAdminRiwayat_Click(object sender, EventArgs e)
        {
            ShowRiwayatControl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormLogin().Show();
        }

        private void FormAdmin_Load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }
    }
}