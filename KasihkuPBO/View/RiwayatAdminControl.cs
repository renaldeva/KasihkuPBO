using System;
using System.Drawing;
using System.Windows.Forms;
using KasihkuPBO.Controller;
using KasihkuPBO.Model;

namespace KasihkuPBO.View
{
    public partial class RiwayatAdminControl : UserControl
    {
        private DataGridView dataGridViewRiwayat;
        private DateTimePicker datePicker;
        private Button btnKembali;
        private RiwayatAdminController controller;

        public event Action KembaliClicked;

        public RiwayatAdminControl()
        {
            InitializeComponent();
            controller = new RiwayatAdminController();
            SetupUI();
            MuatData();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;

            datePicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(643, 166), 
                Size = new Size(180, 45),
                CalendarFont = new Font("Segoe UI", 10),
                CalendarForeColor = Color.DarkSlateGray
            };
            datePicker.ValueChanged += (s, e) => MuatData(datePicker.Value.ToString("yyyy-MM-dd"));
            Controls.Add(datePicker);

            btnKembali = new Button()
            {
                Text = "⮌ Kembali", 
                Location = new Point(426, 166), 
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(33, 88, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnKembali.FlatAppearance.BorderSize = 0;
            btnKembali.Click += (s, e) => KembaliClicked?.Invoke();
            Controls.Add(btnKembali);

            dataGridViewRiwayat = new DataGridView()
            {
                Location = new Point(426, 214),
                Size = new Size(1438, 747),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ColumnHeadersHeight = 40,
                RowHeadersVisible = false
            };

            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 88, 64);
            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridViewRiwayat.DefaultCellStyle.Font = new Font("Segoe UI", 11);
            dataGridViewRiwayat.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewRiwayat.DefaultCellStyle.BackColor = Color.White;

            dataGridViewRiwayat.EnableHeadersVisualStyles = false;

            dataGridViewRiwayat.Columns.Add("tanggal", "Tanggal");
            dataGridViewRiwayat.Columns.Add("produk", "Daftar Produk");
            dataGridViewRiwayat.Columns.Add("total", "Total");
            dataGridViewRiwayat.Columns.Add("status", "Metode Pembayaran");

            Controls.Add(dataGridViewRiwayat);
            Controls.Add(dataGridViewRiwayat);
            dataGridViewRiwayat.BringToFront();
        }

        private void MuatData(string filterTanggal = "")
        {
            dataGridViewRiwayat.Rows.Clear();
            var data = controller.AmbilSemuaRiwayat();

            foreach (var item in data)
            {
                var tanggalTransaksi = item.Tanggal.Split(' ')[0];
                if (string.IsNullOrEmpty(filterTanggal) || tanggalTransaksi == filterTanggal)
                {
                    dataGridViewRiwayat.Rows.Add(item.Tanggal, item.DaftarProduk, $"Rp {item.Total:N0}", item.Status);
                }
            }
        }
    }
}