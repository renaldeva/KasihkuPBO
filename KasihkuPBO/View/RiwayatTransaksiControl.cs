using System;
using System.Drawing;
using System.Windows.Forms;

namespace KasihkuPBO.View
{
    public partial class RiwayatTransaksiControl : UserControl
    {
        private DataGridView dataGridViewRiwayat;
        private Button btnKembali;

        public event Action KembaliClicked;  // event agar form utama bisa mendeteksi tombol kembali

        public RiwayatTransaksiControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.Controls.Clear();

            // Tombol Kembali
            btnKembali = new Button()
            {
                Text = "Kembali",
                Location = new Point(20, 10),
                Size = new Size(100, 30),
            };
            btnKembali.Click += BtnKembali_Click;
            this.Controls.Add(btnKembali);

            // DataGridView untuk riwayat transaksi
            dataGridViewRiwayat = new DataGridView()
            {
                Location = new Point(20, 50),
                Size = new Size(800, 370),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            dataGridViewRiwayat.Columns.Add("tanggal", "Tanggal");
            dataGridViewRiwayat.Columns.Add("produk", "Daftar Produk");
            dataGridViewRiwayat.Columns.Add("total", "Total");

            this.Controls.Add(dataGridViewRiwayat);
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            KembaliClicked?.Invoke(); // memicu event agar form utama bisa merespons
        }

        public void TambahRiwayat(string tanggal, string daftarProduk, decimal total)
        {
            dataGridViewRiwayat.Rows.Add(tanggal, daftarProduk, $"Rp {total:N0}");
        }

        // Optional: method untuk membersihkan riwayat
        public void ResetRiwayat()
        {
            dataGridViewRiwayat.Rows.Clear();
        }
    }
}
