using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

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

        // ✅ Load data dari database PostgreSQL
        public void LoadRiwayatDariDatabase()
        {
            dataGridViewRiwayat.Rows.Clear();

            string connString = "Host=localhost;Username=postgres;Password=fahrezaadam1784;Database=KASIHKU";
            string query = @"
                SELECT t.tanggal, 
                   STRING_AGG(d.nama_produk || ' x' || d.jumlah, ', ') AS daftar_produk, 
                   t.total
                   FROM transaksi t
                   JOIN detail_transaksi d ON t.id_transaksi = d.id_detail
                   GROUP BY t.id_transaksi, t.tanggal, t.total
                   ORDER BY t.tanggal DESC
            ";

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tanggal = reader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm");
                            string daftarProduk = reader.GetString(1);
                            decimal total = reader.GetDecimal(2);

                            dataGridViewRiwayat.Rows.Add(tanggal, daftarProduk, $"Rp {total:N0}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat riwayat transaksi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}