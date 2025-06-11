using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KasihkuPBO.Controller;
using KasihkuPBO.Model;

namespace KasihkuPBO.View
{
    public partial class ProdukKasirControl : UserControl
    {
        private ProdukController produkController = new ProdukController();

        private Panel panelProduk, panelRekomendasi;
        private TextBox txtSearch;
        private DataGridView dataGridViewProduk;
        private Button btnRekomendasi, btnKeTransaksi;

        private ComboBox comboLokasi, comboTujuan, comboPengalaman, comboWaktu;
        private CheckBox chkTanaman, chkPot, chkPupuk;
        private Button btnProsesRekomendasi;
        private ListBox lstRekomendasi;

        private Dictionary<int, int> jumlahProduk = new Dictionary<int, int>();
        private Panel panelGrid;
        private Control? overlayPanel;

        // Event untuk navigasi dan transaksi
        public event Action NavigasiKeTransaksi;
        public event Action<int, string, decimal> ProdukDitambahkan;
        public event Action<int> ProdukDikurangkan;

        public ProdukKasirControl()
        {
            InitializeComponent();
            SetupUI();

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

            panelGrid = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Image.FromFile(@"C:\Users\User\Downloads\Produk.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(panelGrid);

            txtSearch = new TextBox()
            {
                PlaceholderText = "Cari nama produk...",
                Location = new Point(400, 165),
                Width = 400,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Visible = false,
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            this.Controls.Add(txtSearch);
            txtSearch.BringToFront();

            btnRekomendasi = new Button()
            {
                Text = " Rekomendasi ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(820, 165),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(5, 0, 0, 0),
            };
            btnRekomendasi.Click += BtnRekomendasi_Click_ShowPanel;
            this.Controls.Add(btnRekomendasi);
            btnRekomendasi.BringToFront();

            btnKeTransaksi = new Button()
            {
                Text = " Ke Transaksi ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(960, 165),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(5, 0, 0, 0),
            };
            btnKeTransaksi.Click += BtnKeTransaksi_Click;
            this.Controls.Add(btnKeTransaksi);
            btnKeTransaksi.BringToFront();

            panelProduk = new Panel()
            {
                Location = new Point(400, 220),
                Size = new Size(1400, 700),
                AutoScroll = true,
                Visible = false,
                BackColor = Color.White
            };

            dataGridViewProduk = CreateProdukGridView(Point.Empty);
            panelProduk.Controls.Add(dataGridViewProduk);
            this.Controls.Add(panelProduk);
            panelProduk.BringToFront();

            panelRekomendasi = new Panel()
            {
                Location = new Point(0, 70),
                Size = new Size(1400, 700),
                AutoScroll = true,
                Visible = false
            };

            // Tambahkan checkbox dan listbox rekomendasi
            chkTanaman = new CheckBox() { Text = "Tanaman", Location = new Point(50, 20), AutoSize = true };
            chkPot = new CheckBox() { Text = "Pot", Location = new Point(50, 50), AutoSize = true };
            chkPupuk = new CheckBox() { Text = "Pupuk", Location = new Point(50, 80), AutoSize = true };

            btnProsesRekomendasi = new Button()
            {
                Text = "Tampilkan Rekomendasi",
                Location = new Point(50, 120),
                Size = new Size(180, 30)
            };
            btnProsesRekomendasi.Click += BtnRekomendasi_Click;

            lstRekomendasi = new ListBox()
            {
                Location = new Point(50, 170),
                Size = new Size(400, 400)
            };

            panelRekomendasi.Controls.Add(chkTanaman);
            panelRekomendasi.Controls.Add(chkPot);
            panelRekomendasi.Controls.Add(chkPupuk);
            panelRekomendasi.Controls.Add(btnProsesRekomendasi);
            panelRekomendasi.Controls.Add(lstRekomendasi);
            this.Controls.Add(panelRekomendasi);
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
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.RowHeadersVisible = false;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.Black;

            grid.Columns.Add("id_produk", "ID");
            grid.Columns.Add("nama_produk", "Nama Produk");
            grid.Columns.Add("stok", "Stok");
            grid.Columns.Add("deskripsi", "Deskripsi");

            var imgCol = new DataGridViewImageColumn
            {
                Name = "gambar",
                HeaderText = "Gambar",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            grid.Columns.Add(imgCol);

            grid.Columns.Add("harga", "Harga");
            grid.Columns.Add("kategori", "Kategori");
            grid.Columns.Add("jumlah", "Jumlah");

            grid.Columns.Add(new DataGridViewButtonColumn { Name = "tambah", HeaderText = "Tambah", Text = "+", UseColumnTextForButtonValue = true });
            grid.Columns.Add(new DataGridViewButtonColumn { Name = "kurang", HeaderText = "Kurang", Text = "-", UseColumnTextForButtonValue = true });

            grid.CellClick += DataGridViewProduk_CellClick;

            return grid;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduk(txtSearch.Text.Trim());
        }

        private void LoadProduk(string keyword = "")
        {
            dataGridViewProduk.Rows.Clear();
            var produkList = produkController.CariProduk(keyword);

            foreach (var produk in produkList)
            {
                Image img = null;
                if (produk.Gambar != null)
                {
                    using (var ms = new MemoryStream(produk.Gambar))
                        img = Image.FromStream(ms);
                }

                dataGridViewProduk.Rows.Add(
                    produk.Id, produk.Nama, produk.Stok, produk.Deskripsi,
                    img, produk.Harga.ToString("N0"), produk.Kategori, 0
                );
            }
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

            var hasil = produkController.RekomendasiProduk(preferensi);
            foreach (var nama in hasil)
                lstRekomendasi.Items.Add(nama);
        }

        private void BtnKeTransaksi_Click(object sender, EventArgs e)
        {
            NavigasiKeTransaksi?.Invoke();
        }

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
