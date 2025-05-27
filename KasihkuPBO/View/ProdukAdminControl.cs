using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Npgsql;

namespace KasihkuPBO.View
{
    public partial class ProdukAdminControl : UserControl
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Rafif0205,;Database=project";

        private Panel panelGrid;
        private Panel panelFormInput;

        private DataGridView dataGridView1;

        private TextBox txtNama, txtStok, txtDeskripsi, txtHarga;
        private ComboBox cmbKategori;
        private PictureBox pictureBoxGambar;
        private Button btnPilihGambar, btnSimpan, btnKembali;

        private Button btnTambahProduk;

        private bool isEditMode = false;
        private int produkIdEdit = -1;
        internal Control? PanelGrid;

        public ProdukAdminControl()
        {
            InitializeComponent();
            SetupUI();
            LoadKategori();
            LoadProduk();
            ShowGridOnly();
        }

        public void ShowGridOnly()
        {
            panelGrid.Visible = true;
            panelFormInput.Visible = false;
        }

        public void ShowInputForm()
        {
            panelGrid.Visible = false;
            panelFormInput.Visible = true;
        }

        public void SetupUI()
        {
            // Panel Grid
            panelGrid = new Panel() { Dock = DockStyle.Fill };
            panelGrid.BackgroundImage = Image.FromFile(@"C:\Users\Rafif Ahmad H\Downloads\MJ.png");
            panelGrid.BackgroundImageLayout = ImageLayout.Stretch;
            this.Controls.Add(panelGrid);

            btnTambahProduk = new Button() { Text = "Tambah", Location = new Point(1550, 825), Size = new Size(200,100), BackColor = Color.FromArgb(33, 88, 64), ForeColor = Color.White, Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0) };
            btnTambahProduk.Click += BtnTambahProduk_Click;
            panelGrid.Controls.Add(btnTambahProduk);

            dataGridView1 = new DataGridView()
            {
                Location = new Point(450, 325),
                Size = new Size(1300, 500),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowTemplate = { Height = 100 }
            };

            dataGridView1.Columns.Add("id_produk", "ID");
            dataGridView1.Columns.Add("nama_produk", "Nama Produk");
            dataGridView1.Columns.Add("stok", "Stok");
            dataGridView1.Columns.Add("deskripsi", "Deskripsi");

            var imgCol = new DataGridViewImageColumn
            {
                Name = "gambar",
                HeaderText = "Gambar",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridView1.Columns.Add(imgCol);

            dataGridView1.Columns.Add("harga", "Harga");
            dataGridView1.Columns.Add("kategori", "Kategori");

            var aksiCol = new DataGridViewButtonColumn()
            {
                Name = "aksi",
                HeaderText = "Aksi",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                Width = 60
            };
            dataGridView1.Columns.Add(aksiCol);

            var hapusCol = new DataGridViewButtonColumn()
            {
                Name = "hapus",
                HeaderText = "Hapus",
                Text = "Hapus",
                UseColumnTextForButtonValue = true,
                Width = 60
            };
            dataGridView1.Columns.Add(hapusCol);

            dataGridView1.CellClick += DataGridView1_CellClick;
            panelGrid.Controls.Add(dataGridView1);

            // Panel Form Input
            panelFormInput = new Panel() { Dock = DockStyle.Fill, Visible = false };
            panelFormInput.BackgroundImage = Image.FromFile(@"C:\Users\Rafif Ahmad H\Downloads\MJ.png");
            panelFormInput.BackgroundImageLayout = ImageLayout.Stretch;
            this.Controls.Add(panelFormInput);

            panelFormInput.Controls.Add(new Label() { Text = "Tambah Dan Edit Produk :", Location = new Point(450, 300), BackColor = Color.Transparent, Size = new Size(900, 100), ForeColor = Color.DarkGreen, Font = new Font("Times New Roman", 36F, FontStyle.Bold, GraphicsUnit.Point, 0) });


            int labelX = 500, controlX = 800, currentY = 400, jarakY = 35;

            panelFormInput.Controls.Add(new Label() { Text = "Nama Produk:", Location = new Point(labelX, currentY), BackColor = Color.Transparent, Size = new Size(200, 35), Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0) });
            txtNama = new TextBox() { Location = new Point(controlX, currentY), Width = 800 };
            panelFormInput.Controls.Add(txtNama);
            currentY += jarakY;

            panelFormInput.Controls.Add(new Label() { Text = "Stok:", Location = new Point(labelX, currentY), BackColor = Color.Transparent, Size = new Size(200, 35), Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0) });
            txtStok = new TextBox() { Location = new Point(controlX, currentY), Width = 800 };
            panelFormInput.Controls.Add(txtStok);
            currentY += jarakY;

            panelFormInput.Controls.Add(new Label() { Text = "Deskripsi:", Location = new Point(labelX, currentY), BackColor = Color.Transparent, Size = new Size(200, 35), Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0) });
            txtDeskripsi = new TextBox() { Location = new Point(controlX, currentY), Width = 800 };
            panelFormInput.Controls.Add(txtDeskripsi);
            currentY += jarakY;

            panelFormInput.Controls.Add(new Label() { Text = "Harga:", Location = new Point(labelX, currentY), BackColor = Color.Transparent, Size = new Size(200, 35), Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0) });
            txtHarga = new TextBox() { Location = new Point(controlX, currentY), Width = 800 };
            panelFormInput.Controls.Add(txtHarga);
            currentY += jarakY;

            panelFormInput.Controls.Add(new Label() { Text = "Kategori:", Location = new Point(labelX, currentY), BackColor = Color.Transparent, Size = new Size(200, 35), Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0) });
            cmbKategori = new ComboBox() { Location = new Point(controlX, currentY), Width = 800, DropDownStyle = ComboBoxStyle.DropDownList };
            panelFormInput.Controls.Add(cmbKategori);
            currentY += jarakY;

            panelFormInput.Controls.Add(new Label() { Text = "Gambar:", Location = new Point(labelX, currentY), BackColor = Color.Transparent, Size = new Size(200, 35), Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0) });
            pictureBoxGambar = new PictureBox() { Location = new Point(controlX, currentY), Size = new Size(150, 150), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            panelFormInput.Controls.Add(pictureBoxGambar);

            btnPilihGambar = new Button() { Text = "Pilih Gambar", Location = new Point(controlX + 160, currentY + 60), Width = 100 };
            btnPilihGambar.Click += BtnPilihGambar_Click;
            panelFormInput.Controls.Add(btnPilihGambar);

            currentY += 170;

            btnSimpan = new Button() { Text = "Simpan", Location = new Point(controlX, currentY), Width = 100 };
            btnSimpan.Click += BtnSimpan_Click;
            panelFormInput.Controls.Add(btnSimpan);

            btnKembali = new Button() { Text = "Kembali", Location = new Point(controlX + 110, currentY), Width = 100 };
            btnKembali.Click += BtnKembali_Click;
            panelFormInput.Controls.Add(btnKembali);
        }

        private void BtnTambahProduk_Click(object sender, EventArgs e)
        {
            ClearForm();
            isEditMode = false;
            produkIdEdit = -1;
            btnSimpan.Text = "Tambah";
            ShowInputForm();
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            ShowGridOnly();
        }

        private void BtnPilihGambar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pictureBoxGambar.Image = Image.FromFile(dlg.FileName);
            }
        }

        private void BtnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show("Nama produk harus diisi!");
                return;
            }
            if (!int.TryParse(txtStok.Text, out int stok))
            {
                MessageBox.Show("Stok harus angka!");
                return;
            }
            if (!decimal.TryParse(txtHarga.Text, out decimal harga))
            {
                MessageBox.Show("Harga harus angka!");
                return;
            }
            if (cmbKategori.SelectedValue == null)
            {
                MessageBox.Show("Kategori harus dipilih!");
                return;
            }
            if (pictureBoxGambar.Image == null)
            {
                MessageBox.Show("Gambar harus dipilih!");
                return;
            }

            byte[] gambarBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBoxGambar.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                gambarBytes = ms.ToArray();
            }

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand cmd;
                if (isEditMode)
                {

                    cmd = new NpgsqlCommand(@"
                        UPDATE produk SET 
                            nama_produk=@nama, stok=@stok, deskripsi=@deskripsi, 
                            gambar=@gambar, harga=@harga, id_kategori=@kategori 
                        WHERE id_produk=@id", conn);

                    cmd.Parameters.AddWithValue("id", produkIdEdit);
                }
                else
                {
                    cmd = new NpgsqlCommand(@"
                        INSERT INTO produk (nama_produk, stok, deskripsi, gambar, harga, id_kategori) 
                        VALUES (@nama, @stok, @deskripsi, @gambar, @harga, @kategori)", conn);
                }

                cmd.Parameters.AddWithValue("nama", txtNama.Text);
                cmd.Parameters.AddWithValue("stok", stok);
                cmd.Parameters.AddWithValue("deskripsi", txtDeskripsi.Text);
                cmd.Parameters.AddWithValue("gambar", gambarBytes);
                cmd.Parameters.AddWithValue("harga", harga);
                cmd.Parameters.AddWithValue("kategori", (int)cmbKategori.SelectedValue);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(isEditMode ? "Data produk berhasil diupdate" : "Data produk berhasil ditambah");
                    LoadProduk();
                    ShowGridOnly();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saat menyimpan data: " + ex.Message);
                }
            }
        }

        private void LoadKategori()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT id_kategori, nama_kategori FROM kategori_produk", conn);
                var reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                cmbKategori.DataSource = dt;
                cmbKategori.ValueMember = "id_kategori";
                cmbKategori.DisplayMember = "nama_kategori";
            }
        }

        private void LoadProduk()
        {
            dataGridView1.Rows.Clear();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
                    SELECT p.id_produk, p.nama_produk, p.stok, p.deskripsi, p.gambar, p.harga, k.nama_kategori
                    FROM produk p
                    JOIN kategori_produk k ON p.id_kategori = k.id_kategori
                    ORDER BY p.id_produk", conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
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

                    dataGridView1.Rows.Add(id, nama, stok, deskripsi, img, harga.ToString("N0"), kategori);
                }
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "aksi")
            {
                // Edit mode
                int idProduk = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id_produk"].Value);
                LoadDataToForm(idProduk);
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "hapus")
            {
                int idProduk = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id_produk"].Value);
                if (MessageBox.Show("Yakin ingin menghapus produk ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteProduk(idProduk);
                }
            }
        }

        private void LoadDataToForm(int idProduk)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT * FROM produk WHERE id_produk=@id", conn);
                cmd.Parameters.AddWithValue("id", idProduk);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    produkIdEdit = idProduk;
                    isEditMode = true;
                    txtNama.Text = reader["nama_produk"].ToString();
                    txtStok.Text = reader["stok"].ToString();
                    txtDeskripsi.Text = reader["deskripsi"].ToString();
                    txtHarga.Text = Convert.ToDecimal(reader["harga"]).ToString();

                    cmbKategori.SelectedValue = Convert.ToInt32(reader["id_kategori"]);

                    if (!(reader["gambar"] is DBNull))
                    {
                        byte[] imgBytes = (byte[])reader["gambar"];
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            pictureBoxGambar.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBoxGambar.Image = null;
                    }
                }
            }
            btnSimpan.Text = "Update";
            ShowInputForm();
        }

        private void DeleteProduk(int idProduk)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("DELETE FROM produk WHERE id_produk=@id", conn);
                cmd.Parameters.AddWithValue("id", idProduk);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Produk berhasil dihapus");
                    LoadProduk();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus produk: " + ex.Message);
                }
            }
        }

        private void ClearForm()
        {
            txtNama.Text = "";
            txtStok.Text = "";
            txtDeskripsi.Text = "";
            txtHarga.Text = "";
            cmbKategori.SelectedIndex = -1;
            pictureBoxGambar.Image = null;
        }
    }
}
