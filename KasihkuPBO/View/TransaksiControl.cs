using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using KasihkuPBO.Model;
using KasihkuPBO.Controller;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Properties;
using Image = System.Drawing.Image;
using HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;

namespace KasihkuPBO.View
{
    public partial class TransaksiControl : UserControl
    {
        private TransaksiModel model = new();
        private TransaksiController controller;

        private DataGridView dataGridViewTransaksi;
        private Button btnBayar, btnKembali;
        private Label lblTotal;
        private Panel panelGrid;

        public event Action KembaliClicked;
        public RiwayatTransaksiControl RiwayatPanel { get; set; }

        public TransaksiControl()
        {
            InitializeComponent();
            controller = new TransaksiController("Host=localhost;Username=postgres;Password=Dev@211104;Database=KASIHKU", model);
            SetupUI();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            panelGrid = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Image.FromFile(@"C:\Users\User\Downloads\Transaksi.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(panelGrid);

            dataGridViewTransaksi = new DataGridView()
            {
                Size = new Size(800, 400),
                Location = new Point(400, 165),
                BackColor = Color.Green,
                ForeColor = Color.Green,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dataGridViewTransaksi.Columns.Add("id_produk", "ID Produk");
            dataGridViewTransaksi.Columns.Add("nama", "Nama Produk");
            dataGridViewTransaksi.Columns.Add("qty", "Qty");
            dataGridViewTransaksi.Columns.Add("harga", "Harga Satuan");
            dataGridViewTransaksi.Columns.Add("subtotal", "Subtotal");
            this.Controls.Add(dataGridViewTransaksi);
            dataGridViewTransaksi.BringToFront();

            lblTotal = new Label()
            {
                Location = new Point(400, 565),
                Font = new Font("Segoe UI", 14),
                Size = new Size(400, 40),
                Text = "Total Bayar: Rp 0",
                BackColor = Color.White
            };
            this.Controls.Add(lblTotal);
            lblTotal.BringToFront();

            btnBayar = new Button()
            {
                Text = " Konfirmasi Pembayaran ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(250, 40),
                Location = new Point(950, 570),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnBayar.Click += BtnBayar_Click;
            this.Controls.Add(btnBayar);
            btnBayar.BringToFront();

            btnKembali = new Button()
            {
                Text = "Kembali",
                Location = new Point(450, 430),
                Size = new Size(180, 40)
            };
            btnKembali.Click += (s, e) => { this.Visible = false; KembaliClicked?.Invoke(); };
            this.Controls.Add(btnKembali);
        }

        public void Tampilkan()
        {
            this.Visible = true;
            this.BringToFront();
        }

        public void TambahProduk(int id, string nama, decimal harga)
        {
            model.TambahProduk(id, nama, harga);
            RenderKeranjang();
        }

        public void KurangiProduk(int id)
        {
            model.KurangiProduk(id);
            RenderKeranjang();
        }

        public void ResetKeranjang()
        {
            model.Reset();
            RenderKeranjang();
        }

        private void RenderKeranjang()
        {
            dataGridViewTransaksi.Rows.Clear();
            foreach (var item in model.Keranjang)
            {
                decimal subtotal = item.Value.harga * item.Value.jumlah;
                dataGridViewTransaksi.Rows.Add(item.Key, item.Value.nama, item.Value.jumlah, item.Value.harga.ToString("N0"), subtotal.ToString("N0"));
            }
            lblTotal.Text = $"Total Bayar: Rp {model.Total:N0}";
        }

        private void BtnBayar_Click(object sender, EventArgs e)
        {
            if (model.Keranjang.Count == 0)
            {
                MessageBox.Show("Keranjang kosong!");
                return;
            }

            try
            {
                int idTransaksi = controller.SimpanTransaksi(out string tanggal, out string daftarProduk);
                RiwayatPanel?.TambahRiwayat(tanggal, daftarProduk, model.Total);
                CetakNotaPdf(idTransaksi, tanggal, daftarProduk, model.Total);
                MessageBox.Show("Transaksi berhasil!");
                ResetKeranjang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan transaksi: " + ex.Message);
            }
        }

        private void CetakNotaPdf(int idTransaksi, string tanggal, string daftarProduk, decimal total)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NotaKasihku");
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, $"Nota_Transaksi_{idTransaksi}.pdf");

            using var writer = new PdfWriter(filePath);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            var boldFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);
            var normalFont = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            string logoPath = @"C:\Users\User\Downloads\LOGO hitam.png";
            if (File.Exists(logoPath))
            {
                var img = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath)).ScaleToFit(100, 100);
                img.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                doc.Add(img);
            }

            doc.Add(new Paragraph("TOKO KASIHKU").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Jl. Mawar No. 123, Banyuwangi").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));
            doc.Add(new Paragraph("========================================================================"));
            doc.Add(new Paragraph($"ID Transaksi : {idTransaksi}"));
            doc.Add(new Paragraph($"Tanggal      : {tanggal}"));
            doc.Add(new Paragraph());
            doc.Add(new Paragraph("QTY  ITEM                SUBTOTAL").SetFont(boldFont));

            foreach (var item in model.Keranjang)
            {
                var (nama, harga, jumlah) = item.Value;
                string namaPendek = nama.Length > 16 ? nama.Substring(0, 16) : nama;
                string line = $"{jumlah.ToString().PadRight(4)} {namaPendek.PadRight(18)} Rp {(harga * jumlah):N0}";
                doc.Add(new Paragraph(line).SetFont(normalFont).SetFontSize(10));
            }

            doc.Add(new Paragraph("------------------------------------------------------------------------"));
            doc.Add(new Paragraph($"TOTAL BAYAR:     Rp {total:N0}").SetFont(boldFont).SetFontSize(11));
            doc.Add(new Paragraph("========================================================================"));
            doc.Add(new Paragraph("Terima kasih telah berbelanja!").SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Toko Kasihku - 2025").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8));

            System.Diagnostics.Process.Start("explorer", filePath);
        }
    }
}
