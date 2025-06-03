using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KasihkuPBO.View
{
    public partial class TransaksiControl : UserControl
    {
        public RiwayatTransaksiControl RiwayatPanel { get; set; }

        private DataGridView dataGridViewTransaksi;
        private Button btnBayar;
        private Button btnKembali;
        private Label lblTotal;
        public event Action KembaliClicked;

        private decimal total = 0;
        // key = id produk, value = tuple (nama, harga, jumlah)
        private Dictionary<int, (string nama, decimal harga, int jumlah)> keranjang = new();

        public TransaksiControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.Controls.Clear();

            dataGridViewTransaksi = new DataGridView()
            {
                Location = new Point(30, 20),
                Size = new Size(800, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            dataGridViewTransaksi.Columns.Add("id", "ID Produk");
            dataGridViewTransaksi.Columns.Add("nama", "Nama Produk");
            dataGridViewTransaksi.Columns.Add("qty", "Qty");
            dataGridViewTransaksi.Columns.Add("harga", "Harga Satuan");
            dataGridViewTransaksi.Columns.Add("subtotal", "Subtotal");

            this.Controls.Add(dataGridViewTransaksi);

            lblTotal = new Label()
            {
                Location = new Point(30, 430),
                Font = new Font("Segoe UI", 14),
                Size = new Size(400, 40),
                Text = "Total Bayar: Rp 0"
            };
            this.Controls.Add(lblTotal);

            btnBayar = new Button()
            {
                Text = "Konfirmasi Pembayaran",
                Location = new Point(650, 430),
                Size = new Size(180, 40)
            };
            btnBayar.Click += BtnBayar_Click;
            this.Controls.Add(btnBayar);

            btnKembali = new Button()
            {
                Text = "Kembali",
                Location = new Point(450, 430),
                Size = new Size(180, 40)
            };
            btnKembali.Click += BtnKembali_Click;
            this.Controls.Add(btnKembali);
        }

        public void TambahProduk(int id, string nama, decimal harga)
        {
            if (keranjang.ContainsKey(id))
            {
                var item = keranjang[id];
                keranjang[id] = (item.nama, item.harga, item.jumlah + 1);
            }
            else
            {
                keranjang[id] = (nama, harga, 1);
            }
            RenderKeranjang();
        }

        public void KurangiProduk(int id)
        {
            if (keranjang.ContainsKey(id))
            {
                var item = keranjang[id];
                int jumlahBaru = item.jumlah - 1;
                if (jumlahBaru <= 0)
                    keranjang.Remove(id);
                else
                    keranjang[id] = (item.nama, item.harga, jumlahBaru);

                RenderKeranjang();
            }
        }

        private void RenderKeranjang()
        {
            dataGridViewTransaksi.Rows.Clear();
            total = 0;
            foreach (var item in keranjang)
            {
                decimal subtotal = item.Value.harga * item.Value.jumlah;
                dataGridViewTransaksi.Rows.Add(
                    item.Key,
                    item.Value.nama,
                    item.Value.jumlah.ToString(),
                    item.Value.harga.ToString("N0"),
                    subtotal.ToString("N0")
                );
                total += subtotal;
            }
            lblTotal.Text = $"Total Bayar: Rp {total:N0}";
        }

        public void Tampilkan()
        {
            this.Visible = true;
            this.BringToFront();
        }

        private void BtnBayar_Click(object sender, EventArgs e)
        {
            if (keranjang.Count == 0)
            {
                MessageBox.Show("Keranjang kosong!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tanggal = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string daftarProduk = "";

            foreach (var item in keranjang)
            {
                daftarProduk += $"{item.Value.nama} x{item.Value.jumlah}, ";
            }

            daftarProduk = daftarProduk.TrimEnd(',', ' ');

            // Tambahkan ke panel riwayat transaksi jika sudah diset
            RiwayatPanel?.TambahRiwayat(tanggal, daftarProduk, total);

            MessageBox.Show("Transaksi berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            keranjang.Clear();
            RenderKeranjang();
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            KembaliClicked?.Invoke();
        }

        public void ResetKeranjang()
        {
            keranjang.Clear();
            RenderKeranjang();
        }
    }
}
