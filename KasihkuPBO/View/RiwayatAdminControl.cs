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
            MuatData(); // tampilkan semua saat awal
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;

            datePicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, 30),
                Width = 200
            };
            datePicker.ValueChanged += (s, e) => MuatData(datePicker.Value.ToString("yyyy-MM-dd"));
            Controls.Add(datePicker);

            btnKembali = new Button()
            {
                Text = "Kembali",
                Location = new Point(250, 30),
                Width = 100,
                Height = 30
            };
            btnKembali.Click += (s, e) => KembaliClicked?.Invoke();
            Controls.Add(btnKembali);

            dataGridViewRiwayat = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(700, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dataGridViewRiwayat.Columns.Add("tanggal", "Tanggal");
            dataGridViewRiwayat.Columns.Add("produk", "Daftar Produk");
            dataGridViewRiwayat.Columns.Add("total", "Total");

            Controls.Add(dataGridViewRiwayat);
        }

        private void MuatData(string filterTanggal = "")
        {
            dataGridViewRiwayat.Rows.Clear();
            var data = controller.AmbilSemuaRiwayat();

            foreach (var item in data)
            {
                // Cocokkan hanya bagian tanggal (tanpa jam)
                var tanggalTransaksi = item.Tanggal.Split(' ')[0];
                if (string.IsNullOrEmpty(filterTanggal) || tanggalTransaksi == filterTanggal)
                {
                    dataGridViewRiwayat.Rows.Add(item.Tanggal, item.DaftarProduk, $"Rp {item.Total:N0}");
                }
            }
        }
    }
}