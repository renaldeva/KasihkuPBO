using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Npgsql;

namespace KasihkuPBO.View
{
    public partial class ProdukKasirControl : UserControl
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=fahrezaadam1784;Database=KASIHKU";

        private Panel panelProduk, panelRekomendasi;
        private TextBox txtSearch;
        private DataGridView dataGridViewProduk;
        private Button btnRekomendasi, btnKeTransaksi;

        private ComboBox comboLokasi, comboTujuan, comboPengalaman, comboWaktu;
        private CheckBox chkTanaman, chkPot, chkPupuk;
        private Button btnProsesRekomendasi;
        private ListBox lstRekomendasi;

        private Dictionary<int, int> jumlahProduk = new Dictionary<int, int>();

        // Event untuk berpindah ke halaman transaksi
        public event Action NavigasiKeTransaksi;
        public event Action<int, string, decimal> ProdukDitambahkan;
        public event Action<int> ProdukDikurangkan;

        public ProdukKasirControl()
        {
            InitializeComponent();
            SetupUI();

            // Sembunyikan seluruh UI saat control dibuat
            txtSearch.Visible = false;
            btnRekomendasi.Visible = false;
            btnKeTransaksi.Visible = false;
            panelProduk.Visible = false;
            panelRekomendasi.Visible = false;
            this.Visible = false;
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.Controls.Clear();

            // Search textbox
            txtSearch = new TextBox()
            {
                PlaceholderText = "Cari nama produk...",
                Location = new Point(50, 20),
                Width = 400,
                Font = new Font("Segoe UI", 14),
                Visible = false
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            this.Controls.Add(txtSearch);

            // Tombol rekomendasi
            btnRekomendasi = new Button()
            {
                Text = "Tampilkan Rekomendasi",
                Location = new Point(470, 20),
                Width = 200,
                Font = new Font("Segoe UI", 10),
                Visible = false
            };
            btnRekomendasi.Click += BtnRekomendasi_Click_ShowPanel;
            this.Controls.Add(btnRekomendasi);

            // Tombol ke transaksi
            btnKeTransaksi = new Button()
            {
                Text = "Ke Transaksi",
                Location = new Point(690, 20),
                Width = 150,
                Font = new Font("Segoe UI", 10),
                Visible = false
            };
            btnKeTransaksi.Click += BtnKeTransaksi_Click;
            this.Controls.Add(btnKeTransaksi);

            // Panel produk
            panelProduk = new Panel() { Location = new Point(0, 70), Size = new Size(1400, 700), AutoScroll = true, Visible = false };
            dataGridViewProduk = CreateProdukGridView(Point.Empty);
            panelProduk.Controls.Add(dataGridViewProduk);
            this.Controls.Add(panelProduk);

            // Panel rekomendasi
            panelRekomendasi = new Panel() { Location = new Point(0, 70), Size = new Size(1400, 700), AutoScroll = true, Visible = false };
            InitRekomendasiPanel();
            this.Controls.Add(panelRekomendasi);
        }

        private void InitRekomendasiPanel()
        {
            Label lblJudul = new Label() { Text = "Masukkan Kebutuhan:", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(50, 20) };

            comboLokasi = new ComboBox() { Location = new Point(50, 60), Width = 200 };
            comboLokasi.Items.AddRange(new[] { "Indoor", "Outdoor" });

            comboTujuan = new ComboBox() { Location = new Point(300, 60), Width = 200 };
            comboTujuan.Items.AddRange(new[] { "Dekorasi", "Hobi", "Produksi" });

            comboPengalaman = new ComboBox() { Location = new Point(50, 100), Width = 200 };
            comboPengalaman.Items.AddRange(new[] { "Pemula", "Menengah", "Ahli" });

            comboWaktu = new ComboBox() { Location = new Point(300, 100), Width = 200 };
            comboWaktu.Items.AddRange(new[] { "Sedikit", "Sedang", "Banyak" });

            chkTanaman = new CheckBox() { Text = "Tanaman", Location = new Point(50, 140) };
            chkPot = new CheckBox() { Text = "Pot", Location = new Point(150, 140) };
            chkPupuk = new CheckBox() { Text = "Pupuk", Location = new Point(250, 140) };

            btnProsesRekomendasi = new Button() { Text = "Cari Rekomendasi", Location = new Point(50, 180), Width = 250 };
            btnProsesRekomendasi.Click += BtnRekomendasi_Click;

            lstRekomendasi = new ListBox() { Location = new Point(50, 230), Size = new Size(1000, 400) };

            Button btnKembali = new Button() { Text = "Kembali", Location = new Point(50, 650) };
            btnKembali.Click += (s, e) => { panelRekomendasi.Visible = false; panelProduk.Visible = true; };

            panelRekomendasi.Controls.AddRange(new Control[]
            {
                lblJudul, comboLokasi, comboTujuan, comboPengalaman, comboWaktu,
                chkTanaman, chkPot, chkPupuk, btnProsesRekomendasi, lstRekomendasi, btnKembali
            });
        }

        private DataGridView CreateProdukGridView(Point location)
        {
            var grid = new DataGridView()
            {
                Location = location,
                Size = new Size(1300, 600),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowTemplate = { Height = 100 },
                RowHeadersVisible = false
            };

            grid.Columns.Add("no", "No"); 
            grid.Columns.Add("id_produk", "ID");
            grid.Columns["id_produk"].Visible = false;
            grid.Columns.Add("nama_produk", "Nama Produk");
            grid.Columns.Add("stok", "Stok");
            grid.Columns.Add("deskripsi", "Deskripsi");

            var imgCol = new DataGridViewImageColumn();
            imgCol.Name = "Gambar";
            imgCol.HeaderText = "Gambar";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom; 
            grid.Columns.Add(imgCol);

            grid.Columns.Add("harga", "Harga");
            grid.Columns.Add("kategori", "Kategori");
            grid.Columns.Add("jumlah", "Jumlah");

            grid.Columns.Add(new DataGridViewButtonColumn { Name = "tambah", HeaderText = "Tambah", Text = "+", UseColumnTextForButtonValue = true });
            grid.Columns.Add(new DataGridViewButtonColumn { Name = "kurang", HeaderText = "Kurang", Text = "-", UseColumnTextForButtonValue = true });

            grid.CellClick += DataGridViewProduk_CellClick;

            grid.DataError += (s, e) =>
            {
                MessageBox.Show("Terjadi kesalahan saat menampilkan data produk:\n\n" + e.Exception.Message,
                    "Kesalahan Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.ThrowException = false;
            };

            return grid;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduk(txtSearch.Text.Trim());
        }

        private void LoadProduk(string keyword = "")
        {
            dataGridViewProduk.Rows.Clear();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
            SELECT p.id_produk, p.nama_produk, p.stok, p.deskripsi, p.gambar, p.harga, k.nama_kategori
            FROM produk p
            JOIN kategori_produk k ON p.id_kategori = k.id_kategori
            WHERE LOWER(p.nama_produk) LIKE @keyword
            ORDER BY p.id_produk";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("keyword", $"%{keyword.ToLower()}%");

                using (var reader = cmd.ExecuteReader())
                {
                    int no = 1;
                    while (reader.Read())
                    {
                        AddProdukRow(dataGridViewProduk, reader, no);
                        no++;
                    }
                }
            }
        }

        private void AddProdukRow(DataGridView grid, NpgsqlDataReader reader, int no)
        {
            int id = reader.GetInt32(0);
            string nama = reader.GetString(1);
            int stok = reader.GetInt32(2);
            string deskripsi = reader.IsDBNull(3) ? "" : reader.GetString(3);
            byte[] gambarBytes = reader.IsDBNull(4) ? null : (byte[])reader[4];
            decimal harga = reader.GetDecimal(5);
            string kategori = reader.GetString(6);

            Image img = null;
            if (gambarBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(gambarBytes))
                {
                    img = Image.FromStream(ms);
                }
            }

            grid.Rows.Add(no, id, nama, stok, deskripsi, img, harga.ToString("N0"), kategori, 0);
        }

        private void DataGridViewProduk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.RowIndex < 0 || grid.Rows[e.RowIndex].IsNewRow) return;

            var row = grid.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["id_produk"].Value);
            string nama = row.Cells["nama_produk"].Value.ToString();
            decimal harga = decimal.Parse(row.Cells["harga"].Value.ToString().Replace(",", ""));

            var jumlahCell = row.Cells["jumlah"];

            if (!jumlahProduk.ContainsKey(id))
                jumlahProduk[id] = 0;

            if (grid.Columns[e.ColumnIndex].Name == "tambah")
            {
                jumlahProduk[id]++;
                jumlahCell.Value = jumlahProduk[id];
                ProdukDitambahkan?.Invoke(id, nama, harga);
            }
            else if (grid.Columns[e.ColumnIndex].Name == "kurang")
            {
                if (jumlahProduk[id] > 0)
                {
                    jumlahProduk[id]--;
                    jumlahCell.Value = jumlahProduk[id];
                    ProdukDikurangkan?.Invoke(id);
                }
            }

        }

        private void BtnRekomendasi_Click_ShowPanel(object sender, EventArgs e)
        {
            panelProduk.Visible = false;
            panelRekomendasi.Visible = true;
        }

        private void BtnRekomendasi_Click(object sender, EventArgs e)
        {
            List<string> preferensi = new();
            if (chkTanaman.Checked) preferensi.Add("tanaman");
            if (chkPot.Checked) preferensi.Add("pot");
            if (chkPupuk.Checked) preferensi.Add("pupuk");

            lstRekomendasi.Items.Clear();

            if (preferensi.Count == 0)
            {
                lstRekomendasi.Items.Add("Pilih minimal satu kategori rekomendasi!");
                return;
            }

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var conditions = preferensi.Select(p => $"LOWER(p.nama_produk) LIKE '%{p}%'");
                var query = @"
                    SELECT DISTINCT p.nama_produk
                    FROM produk p
                    JOIN kategori_produk k ON p.id_kategori = k.id_kategori
                    WHERE " + string.Join(" OR ", conditions) + @"
                    ORDER BY p.nama_produk";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstRekomendasi.Items.Add(reader.GetString(0));
                    }
                }
            }
        }

        private void BtnKeTransaksi_Click(object sender, EventArgs e)
        {
            NavigasiKeTransaksi?.Invoke();
        }

        // Method untuk tampilkan panel produk
        public void TampilkanProduk()
        {
            this.Visible = true;

            txtSearch.Visible = true;
            btnRekomendasi.Visible = true;
            btnKeTransaksi.Visible = true;
            panelProduk.Visible = true;
            panelRekomendasi.Visible = false;

            LoadProduk();
        }

        // Method untuk sembunyikan semua panel produk
        public void SembunyikanProduk()
        {
            this.Visible = false;

            txtSearch.Visible = false;
            btnRekomendasi.Visible = false;
            btnKeTransaksi.Visible = false;
            panelProduk.Visible = false;
            panelRekomendasi.Visible = false;
        }
    }
}
