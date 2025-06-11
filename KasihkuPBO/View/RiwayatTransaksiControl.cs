using KasihkuPBO.Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KasihkuPBO.View
{
    public partial class RiwayatTransaksiControl : UserControl
    {
        private DataGridView dataGridViewRiwayat;
        private Button btnKembali;
        private Panel overlayPanel;
        private Panel panelGrid;
        private DateTimePicker datePickerTanggal;

        public event EventHandler RefreshRequested;
        public event Action KembaliClicked;

        // Menyimpan data sementara agar bisa difilter ulang
        private List<Tuple<string, string, decimal>> semuaRiwayat = new List<Tuple<string, string, decimal>>();

        public RiwayatTransaksiControl()
        {
            InitializeComponent();
            SetupUI();
            new RiwayatTransaksiController(this);
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            panelGrid = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Image.FromFile(@"C:\Users\Reza\Downloads\Riwayat.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(panelGrid);

            overlayPanel = new Panel()
            {
                BackColor = Color.FromArgb(180, 255, 255, 255),
                Location = new Point(400, 220),
                Size = new Size(900, 450),
                BorderStyle = BorderStyle.FixedSingle
            };
            panelGrid.Controls.Add(overlayPanel);

            btnKembali = new Button()
            {
                Text = " Kembali",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(140, 45),
                Location = new Point(400, 165),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(5, 0, 0, 0),
            };
            btnKembali.Click += BtnKembali_Click;
            panelGrid.Controls.Add(btnKembali);

            Label lblJudul = new Label()
            {
                Text = "Riwayat Transaksi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(900, 450),
                AutoSize = true
            };
            panelGrid.Controls.Add(lblJudul);

            datePickerTanggal = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(560, 165),
                Width = 140
            };
            datePickerTanggal.ValueChanged += (s, e) => TampilkanDataSesuaiTanggal();
            panelGrid.Controls.Add(datePickerTanggal);

            dataGridViewRiwayat = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
            };

            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewRiwayat.EnableHeadersVisualStyles = false;

            dataGridViewRiwayat.Columns.Add("tanggal", "Tanggal");
            dataGridViewRiwayat.Columns.Add("produk", "Daftar Produk");
            dataGridViewRiwayat.Columns.Add("total", "Total");

            overlayPanel.Controls.Add(dataGridViewRiwayat);

            this.Load += (s, e) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            KembaliClicked?.Invoke();
        }

        public void Tampilkan()
        {
            this.Visible = true;
            this.BringToFront();
        }

        public void TambahRiwayat(string tanggal, string daftarProduk, decimal total)
        {
            // Simpan semua riwayat ke list agar bisa difilter ulang
            semuaRiwayat.Add(new Tuple<string, string, decimal>(tanggal, daftarProduk, total));

            // Hanya tampilkan jika tanggal sesuai yang dipilih
            string selectedDate = datePickerTanggal.Value.ToString("yyyy-MM-dd");
            if (tanggal.StartsWith(selectedDate))
            {
                dataGridViewRiwayat.Rows.Add(tanggal, daftarProduk, $"Rp {total:N0}");
            }
        }

        public void ResetRiwayat()
        {
            dataGridViewRiwayat.Rows.Clear();
            semuaRiwayat.Clear();
        }

        public void RefreshData()
        {
            ResetRiwayat();
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        public void TampilkanDataSesuaiTanggal()
        {
            dataGridViewRiwayat.Rows.Clear();
            string selectedDate = datePickerTanggal.Value.ToString("yyyy-MM-dd");

            foreach (var item in semuaRiwayat)
            {
                if (item.Item1.StartsWith(selectedDate))
                {
                    dataGridViewRiwayat.Rows.Add(item.Item1, item.Item2, $"Rp {item.Item3:N0}");
                }
            }
        }

        public void TampilkanError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RiwayatTransaksiControl_Load(object sender, EventArgs e)
        {

        }
    }
}