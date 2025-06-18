using KasihkuPBO.View;
using System;
using System.Windows.Forms;

namespace KasihkuPBO
{
    public partial class FormKasir : Form
    {
        private ProdukKasirControl produkKasirControl;
        private TransaksiControl transaksiControl;
        private RiwayatTransaksiControl riwayatControl;

        public FormKasir(string username)
        {
            InitializeComponent();

            label1.Text = "Selamat datang di aplikasi Kasihku, " + username;

            produkKasirControl = new ProdukKasirControl();
            produkKasirControl.Dock = DockStyle.Fill;

            produkKasirControl.ProdukDitambahkan += ProdukDitambahkanHandler;
            produkKasirControl.ProdukDikurangkan += ProdukDikurangkanHandler;

            transaksiControl = new TransaksiControl();
            transaksiControl.Dock = DockStyle.Fill;

            riwayatControl = new RiwayatTransaksiControl();
            riwayatControl.Dock = DockStyle.Fill;

            transaksiControl.ProdukPanel = produkKasirControl;
            transaksiControl.RiwayatPanel = riwayatControl;

            this.Controls.Add(produkKasirControl);
            this.Controls.Add(transaksiControl);
            this.Controls.Add(riwayatControl);

            produkKasirControl.Visible = true;
            transaksiControl.Visible = false;
            riwayatControl.Visible = false;

            produkKasirControl.SembunyikanProduk();

            produkKasirControl.KembaliClicked += () =>
            {
                produkKasirControl.Visible = false;
                transaksiControl.Visible = false;
                riwayatControl.Visible = false;
            };

            transaksiControl.KembaliClicked += () =>
            {
                produkKasirControl.Visible = false;
                transaksiControl.Visible = false;
                riwayatControl.Visible = false;
            };

            produkKasirControl.NavigasiKeTransaksi += () =>
            {
                produkKasirControl.SembunyikanProduk();
                transaksiControl.Tampilkan();
            };

            riwayatControl.KembaliClicked += () =>
            {
                    produkKasirControl.Visible = false;
                    transaksiControl.Visible = false;
                    riwayatControl.Visible = false;
            };

        }

        private void ShowProdukControl()
        {
            produkKasirControl.Visible = true;
            transaksiControl.Visible = false;
            riwayatControl.Visible = false;

            produkKasirControl.TampilkanProduk();
            produkKasirControl.BringToFront();
        }

        private void ShowTransaksiControl()
        {
            produkKasirControl.Visible = false;
            transaksiControl.Visible = true;
            riwayatControl.Visible = false;

            transaksiControl.BringToFront();
        }

        private void ShowRiwayatControl()
        {
            produkKasirControl.Visible = false;
            transaksiControl.Visible = false;
            riwayatControl.Visible = true;

            riwayatControl.BringToFront();
        }

        private void btnProdukKasir_Click(object sender, EventArgs e)
        {
            ShowProdukControl();
        }

        private void btnTransaksiKasir_Click(object sender, EventArgs e)
        {
            ShowTransaksiControl();
        }

        private void btnRiwayatKasir_Click(object sender, EventArgs e)
        {
            ShowRiwayatControl();
        }

        private void ProdukDitambahkanHandler(int id, string nama, decimal harga)
        {
            transaksiControl.TambahProduk(id, nama, harga);
        }

        private void ProdukDikurangkanHandler(int id)
        {
            transaksiControl.KurangiProduk(id);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void Form3_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

    }
}
