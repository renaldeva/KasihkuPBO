﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using KasihkuPBO.Controller;
using KasihkuPBO.Model;

namespace KasihkuPBO.View
{
    public partial class ProdukKasirControl : UserControl
    {
        private ProdukController produkController = new ProdukController();

        private FlowLayoutPanel panelProduk;
        private TextBox txtSearch;
        private Button btnRekomendasi, btnKeTransaksi, btnKembali;
        private Label lblTotalItem, lblTotalHarga;

        private Dictionary<int, int> jumlahProduk = new Dictionary<int, int>();
        private Dictionary<int, int> stokProduk = new Dictionary<int, int>();

        private decimal? lastHargaMin = null;
        private decimal? lastHargaMax = null;
        private string lastKategori = "None";

        public event Action NavigasiKeTransaksi;
        public event Action KembaliClicked;
        public event Action<int, string, decimal> ProdukDitambahkan;
        public event Action<int> ProdukDikurangkan;

        public ProdukKasirControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.Controls.Clear();

            panelProduk = new FlowLayoutPanel()
            {
                Location = new Point(454, 271),
                Size = new Size(1371, 662),
                AutoScroll = true,
                BackColor = Color.White
            };
            this.Controls.Add(panelProduk);

            txtSearch = new TextBox()
            {
                Location = new Point(601, 202),
                Width = 400,
                Size = new Size (396, 23),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            this.Controls.Add(txtSearch);

            btnRekomendasi = new Button()
            {
                Text = " Rekomendasi ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(188, 33),
                Location = new Point(1009, 202),
                BackColor = Color.FromArgb(33, 88, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRekomendasi.Click += BtnRekomendasi_Click;
            this.Controls.Add(btnRekomendasi);

            btnKeTransaksi = new Button()
            {
                Text = " Ke Transaksi ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(188, 33),
                Location = new Point(1243, 202),
                BackColor = Color.FromArgb(33, 88, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnKeTransaksi.Click += BtnKeTransaksi_Click;
            this.Controls.Add(btnKeTransaksi);

            btnKembali = new Button()
            {
                Text = " Kembali",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(454, 202),
                BackColor = Color.FromArgb(33, 88, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(5, 0, 0, 0),
            };
            btnKembali.Click += BtnKembali_Click;
            this.Controls.Add(btnKembali);
            btnKembali.BringToFront();

            lblTotalItem = new Label()
            {
                Text = "Total Item: 0",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(468, 975),
                Size = new Size(253, 48)
            };
            this.Controls.Add(lblTotalItem);

            lblTotalHarga = new Label()
            {
                Text = "Total Harga: Rp 0",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(745, 975),
                Size = new Size(253, 48)
            };
            this.Controls.Add(lblTotalHarga);


            LoadProduk();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduk(txtSearch.Text.Trim(), lastHargaMin, lastHargaMax, lastKategori);
        }

        private void BtnRekomendasi_Click(object sender, EventArgs e)
        {
            using (var form = new Form())
            {
                form.Text = "Filter Rekomendasi";
                form.Size = new Size(350, 250);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var labelKategori = new Label() { Text = "Kategori", Location = new Point(20, 20), Size = new Size(80, 25) };
                var cmbKategori = new ComboBox()
                {
                    Location = new Point(120, 20),
                    Width = 180,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 10)
                };
                cmbKategori.Items.AddRange(new string[] { "None", "Tanaman", "Pot", "Pupuk" });
                cmbKategori.SelectedItem = lastKategori ?? "None";

                var labelMin = new Label() { Text = "Harga Min", Location = new Point(20, 60), Size = new Size(80, 25) };
                var txtMin = new TextBox()
                {
                    Location = new Point(120, 60),
                    Width = 180,
                    Font = new Font("Segoe UI", 10),
                    Text = lastHargaMin?.ToString() ?? ""
                };

                var labelMax = new Label() { Text = "Harga Max", Location = new Point(20, 100), Size = new Size(80, 25) };
                var txtMax = new TextBox()
                {
                    Location = new Point(120, 100),
                    Width = 180,
                    Font = new Font("Segoe UI", 10),
                    Text = lastHargaMax?.ToString() ?? ""
                };

                var btnOK = new Button()
                {
                    Text = "Terapkan",
                    Location = new Point(120, 150),
                    Size = new Size(80, 35),
                    DialogResult = DialogResult.OK
                };

                var btnReset = new Button()
                {
                    Text = "Reset",
                    Location = new Point(220, 150),
                    Size = new Size(80, 35),
                    DialogResult = DialogResult.Retry
                };

                form.Controls.AddRange(new Control[]
                {
                labelKategori, cmbKategori,
                labelMin, txtMin,
                labelMax, txtMax,
                btnOK, btnReset
                });

                form.AcceptButton = btnOK;

                var result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    lastKategori = cmbKategori.SelectedItem.ToString();

                    string minText = txtMin.Text.Trim();
                    string maxText = txtMax.Text.Trim();

                    if (!string.IsNullOrEmpty(minText) && !decimal.TryParse(minText, out _))
                    {
                        MessageBox.Show("Harga minimum harus berupa angka!", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!string.IsNullOrEmpty(maxText) && !decimal.TryParse(maxText, out _))
                    {
                        MessageBox.Show("Harga maksimum harus berupa angka!", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bool isMinValid = decimal.TryParse(minText, out decimal min);
                    bool isMaxValid = decimal.TryParse(maxText, out decimal max);

                    if (isMinValid && min < 0)
                    {
                        MessageBox.Show("Harga minimum tidak boleh kurang dari 0.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (isMaxValid && max < 0)
                    {
                        MessageBox.Show("Harga maksimum tidak boleh kurang dari 0.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lastHargaMin = isMinValid ? min : null;
                    lastHargaMax = isMaxValid ? max : null;

                    LoadProduk(txtSearch.Text.Trim(), lastHargaMin, lastHargaMax, lastKategori);
                }

                else if (result == DialogResult.Retry)
                {
                    lastKategori = "None";
                    lastHargaMin = null;
                    lastHargaMax = null;
                    LoadProduk();
                }
            }
        }

        public void LoadProduk(string keyword = "", decimal? hargaMin = null, decimal? hargaMax = null, string kategori = "None")
        {
            panelProduk.Controls.Clear();
            var produkList = produkController.CariProduk(keyword);

            if (kategori != "None")
                produkList = produkList.Where(p => string.Equals(p.Kategori?.Trim(), kategori.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (hargaMin.HasValue)
                produkList = produkList.Where(p => p.Harga >= hargaMin.Value).ToList();
            if (hargaMax.HasValue && hargaMax.Value > 0)
                produkList = produkList.Where(p => p.Harga <= hargaMax.Value).ToList();

            jumlahProduk.Clear();
            stokProduk.Clear();

            foreach (var produk in produkList)
            {
                var panelItem = new Panel()
                {
                    Size = new Size(250, 320),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    BackColor = Color.White
                };

                var gambar = new PictureBox()
                {
                    Size = new Size(200, 150),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(25, 10)
                };
                if (produk.Gambar != null)
                {
                    using var ms = new MemoryStream(produk.Gambar);
                    gambar.Image = Image.FromStream(ms);
                }
                panelItem.Controls.Add(gambar);

                var lblNama = new Label()
                {
                    Text = produk.Nama,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(230, 25),
                    Location = new Point(10, 165)
                };
                panelItem.Controls.Add(lblNama);

                var lblHarga = new Label()
                {
                    Text = "Rp " + produk.Harga.ToString("N"),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(230, 20),
                    Location = new Point(10, 190)
                };
                panelItem.Controls.Add(lblHarga);

                var lblStok = new Label()
                {
                    Text = $"Stok: {produk.Stok}",
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(230, 20),
                    Location = new Point(10, 215)
                };

                var btnKurang = new Button()
                {
                    Text = "-",
                    Size = new Size(30, 30),
                    Location = new Point(40, 250)
                };

                var txtJumlah = new TextBox()
                {
                    Text = "0",
                    Location = new Point(80, 255),
                    Size = new Size(30, 25),
                    TextAlign = HorizontalAlignment.Center,
                    MaxLength = 3
                };

                var btnTambah = new Button()
                {
                    Text = "+",
                    Size = new Size(30, 30),
                    Location = new Point(120, 250)
                };

                var btnDeskripsi = new Button()
                {
                    Text = "Deskripsi",
                    Size = new Size(60, 30),
                    Location = new Point(160, 250),
                    BackColor = Color.LightBlue
                };

                int id = produk.Id;
                string nama = produk.Nama;
                decimal harga = produk.Harga;
                int stok = produk.Stok;

                jumlahProduk[id] = 0;
                stokProduk[id] = stok;
                int jumlahSebelumnya = jumlahProduk[id];

                txtJumlah.TextChanged += (s, e) =>
                {
                    string input = txtJumlah.Text.Trim();

                    if (string.IsNullOrEmpty(input))
                        return;

                    int jumlahSebelumnya = jumlahProduk[id];

                    if (int.TryParse(input, out int jumlahBaru))
                    {
                        if (jumlahBaru < 0)
                        {
                            MessageBox.Show("Jumlah tidak boleh negatif!\nMasukkan angka minimal 0.", "Input Minus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            jumlahProduk[id] = 0;
                            txtJumlah.Text = "0";
                        }
                        else if (jumlahBaru > stokProduk[id])
                        {
                            MessageBox.Show("Jumlah melebihi stok tersedia!", "Stok Tidak Cukup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            jumlahProduk[id] = 0;
                            txtJumlah.Text = "0";
                        }
                        else
                        {
                            jumlahProduk[id] = jumlahBaru;

                            int selisih = jumlahBaru - jumlahSebelumnya;

                            if (selisih > 0)
                            {
                                for (int i = 0; i < selisih; i++)
                                    ProdukDitambahkan?.Invoke(id, nama, harga);
                            }
                            else if (selisih < 0)
                            {
                                for (int i = 0; i < -selisih; i++)
                                    ProdukDikurangkan?.Invoke(id);
                            }
                        }

                        txtJumlah.SelectionStart = txtJumlah.Text.Length;
                        UpdateTotal();
                    }
                    else
                    {
                        MessageBox.Show("Masukkan hanya angka!", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        jumlahProduk[id] = 0;
                        txtJumlah.Text = "0";
                        txtJumlah.SelectionStart = txtJumlah.Text.Length;
                    }
                };

                btnTambah.Click += (s, e) =>
                {
                    if (jumlahProduk[id] + 1 <= stokProduk[id])
                    {
                        jumlahProduk[id]++;
                        txtJumlah.Text = jumlahProduk[id].ToString();
                        ProdukDitambahkan?.Invoke(id, nama, harga);
                        UpdateTotal();
                    }
                    else
                    {
                        MessageBox.Show("Stok produk tidak mencukupi!", "Stok Habis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                btnKurang.Click += (s, e) =>
                {
                    if (jumlahProduk[id] > 0)
                    {
                        jumlahProduk[id]--;
                        txtJumlah.Text = jumlahProduk[id].ToString();
                        ProdukDikurangkan?.Invoke(id);
                        UpdateTotal();
                    }
                    else
                    {
                        jumlahProduk[id] = 0;
                        txtJumlah.Text = "0";
                        SystemSounds.Beep.Play();
                    }
                };


                btnDeskripsi.Click += (s, e) =>
                {
                    MessageBox.Show(produk.Deskripsi, "Deskripsi Produk", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                panelItem.Controls.Add(btnKurang);
                panelItem.Controls.Add(txtJumlah);
                panelItem.Controls.Add(btnTambah);
                panelItem.Controls.Add(btnDeskripsi);
                panelProduk.Controls.Add(panelItem);
                panelItem.Controls.Add(lblStok);

                if (!jumlahProduk.ContainsKey(id)) jumlahProduk[id] = 0;
                if (!stokProduk.ContainsKey(id)) stokProduk[id] = produk.Stok;

            }


            UpdateTotal();
        }

        private void UpdateTotal()
        {
            int totalItem = jumlahProduk.Values.Sum();
            decimal totalHarga = 0;
            foreach (var pair in jumlahProduk)
            {
                var produk = produkController.CariProduk("").FirstOrDefault(p => p.Id == pair.Key);
                if (produk != null)
                    totalHarga += produk.Harga * pair.Value;
            }

            lblTotalItem.Text = $"Total Item: {totalItem}";
            lblTotalHarga.Text = $"Total Harga: Rp {totalHarga:N}";
        }

        private void BtnKeTransaksi_Click(object sender, EventArgs e)
        {
            NavigasiKeTransaksi?.Invoke();
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            KembaliClicked?.Invoke();
        }

        public void SembunyikanProduk()
        {
            this.Visible = false;
        }

        public void TampilkanProduk()
        {
            this.Visible = true;
        }

        public void ReloadProdukList()
        {
            jumlahProduk.Clear();
            stokProduk.Clear();
            LoadProduk(txtSearch.Text.Trim(), lastHargaMin, lastHargaMax, lastKategori);
            UpdateTotal();
        }
    }
}