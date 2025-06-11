using KasihkuPBO.Controller;
using System;
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

        // Events for controller
        public event EventHandler RefreshRequested;

        // Existing public interface remains the same
        public event Action KembaliClicked;

        public RiwayatTransaksiControl()
        {
            InitializeComponent();
            SetupUI();

            // Initialize controller
            new RiwayatTransaksiController(this);
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Panel untuk background tambahan jika diperlukan
            panelGrid = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Image.FromFile(@"C:\Users\User\Downloads\Riwayat1.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(panelGrid);

            // Panel semi-transparan untuk konten
            overlayPanel = new Panel()
            {
                BackColor = Color.FromArgb(180, 255, 255, 255),
                Location = new Point(400, 220),
                Size = new Size(900, 450),
                BorderStyle = BorderStyle.FixedSingle
            };
            panelGrid.Controls.Add(overlayPanel);

            // Tombol kembali
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

            // Label judul
            Label lblJudul = new Label()
            {
                Text = "Riwayat Transaksi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(900, 450),
                AutoSize = true
            };
            panelGrid.Controls.Add(lblJudul);

            // DataGridView
            dataGridViewRiwayat = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(900, 450)
            };

            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            dataGridViewRiwayat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewRiwayat.EnableHeadersVisualStyles = false;

            dataGridViewRiwayat.Columns.Add("tanggal", "Tanggal");
            dataGridViewRiwayat.Columns.Add("produk", "Daftar Produk");
            dataGridViewRiwayat.Columns.Add("total", "Total");

            overlayPanel.Controls.Add(dataGridViewRiwayat);

            // Trigger initial load
            this.Load += (s, e) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            KembaliClicked?.Invoke();
        }

        // Public methods remain unchanged
        public void Tampilkan()
        {
            this.Visible = true;
            this.BringToFront();
        }

        public void TambahRiwayat(string tanggal, string daftarProduk, decimal total)
        {
            dataGridViewRiwayat.Rows.Add(tanggal, daftarProduk, $"Rp {total:N0}");
        }

        public void ResetRiwayat()
        {
            dataGridViewRiwayat.Rows.Clear();
        }

        public void RefreshData()
        {
            RefreshRequested?.Invoke(this, EventArgs.Empty);
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