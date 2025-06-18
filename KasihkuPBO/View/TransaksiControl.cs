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
using QRCoder;
using Image = System.Drawing.Image;
using HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;

namespace KasihkuPBO.View
{
    public partial class TransaksiControl : UserControl
    {
        private TransaksiModel model = new();
        private TransaksiController controller;

        private DataGridView dataGridViewTransaksi;
        private Dictionary<int, (string nama, decimal harga, int jumlah)> lastKeranjang;
        private Button btnBayar, btnKembali, btnCetakNota, btnQRIS;
        private Label lblTotal;
        private Panel panelGrid;

        private int lastIdTransaksi = -1;
        private string lastTanggal = "", lastDaftarProduk = "";
        private decimal lastTotal = 0;

        public event Action KembaliClicked;
        public ProdukKasirControl ProdukPanel { get; set; }
        public RiwayatTransaksiControl RiwayatPanel { get; set; }

        public TransaksiControl()
        {
            InitializeComponent();
            controller = new TransaksiController("Host=localhost;Username=postgres;Password=fahrezaadam1784;Database=KASIHKU", model);
            SetupUI();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            panelGrid = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Image.FromFile(@"C:\Users\Reza\Downloads\Transaksi.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(panelGrid);

            dataGridViewTransaksi = new DataGridView()
            {
                Size = new Size(1371, 662),
                Location = new Point(454, 271),
                BackColor = Color.FromArgb(33, 88, 64),
                ForeColor = Color.Green,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };


            dataGridViewTransaksi.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 88, 64);
            dataGridViewTransaksi.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewTransaksi.EnableHeadersVisualStyles = false;

            dataGridViewTransaksi.Columns.Add("id_produk", "ID Produk");
            dataGridViewTransaksi.Columns.Add("nama", "Nama Produk");
            dataGridViewTransaksi.Columns.Add("qty", "Qty");
            dataGridViewTransaksi.Columns.Add("harga", "Harga Satuan");
            dataGridViewTransaksi.Columns.Add("subtotal", "Subtotal");
            this.Controls.Add(dataGridViewTransaksi);
            dataGridViewTransaksi.BringToFront();

            lblTotal = new Label()
            {
                Location = new Point(454, 962),
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
                Size = new Size(220, 40),
                Location = new Point(1374, 948),
                BackColor = Color.FromArgb(33, 88, 64),
                ForeColor = Color.White
            };
            btnBayar.Click += BtnBayar_Click;
            this.Controls.Add(btnBayar);
            btnBayar.BringToFront();

            btnQRIS = new Button()
            {
                Text = "Bayar via QRIS",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point(1126, 948),
                BackColor = Color.OrangeRed,
                ForeColor = Color.White
            };
            btnQRIS.Click += BtnQRIS_Click;
            this.Controls.Add(btnQRIS);
            btnQRIS.BringToFront();

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
            panelGrid.Controls.Add(btnKembali);
            btnKembali.BringToFront();

            btnCetakNota = new Button()
            {
                Text = "Cetak Nota (PDF)",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 40),
                Location = new Point(1614, 948),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White
            };
            btnCetakNota.Click += BtnCetakNota_Click;
            this.Controls.Add(btnCetakNota);
            btnCetakNota.BringToFront();
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

                lastIdTransaksi = idTransaksi;
                lastTanggal = tanggal;
                lastDaftarProduk = daftarProduk;
                lastTotal = model.Total;

                lastKeranjang = new Dictionary<int, (string, decimal, int)>(model.Keranjang);

                MessageBox.Show("Transaksi berhasil! Anda bisa mencetak nota.");
                ResetKeranjang();

                if (ProdukPanel != null)
                {
                    ProdukPanel.ReloadProdukList();
                    ProdukPanel.TampilkanProduk();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan transaksi: " + ex.Message);
            }
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

        private void BtnCetakNota_Click(object sender, EventArgs e)
        {
            if (lastIdTransaksi == -1)
            {
                MessageBox.Show("Belum ada transaksi yang bisa dicetak.");
                return;
            }

            try
            {
                CetakNotaPdf(lastIdTransaksi, lastTanggal, lastDaftarProduk, lastTotal);
                MessageBox.Show("Nota berhasil dicetak.");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Gagal mencetak nota: " + ex.Message);
            }
        }

        private void BtnQRIS_Click(object sender, EventArgs e)
        {
            if (model.Total == 0)
            {
                MessageBox.Show("Total pembayaran masih Rp 0. Silakan tambahkan produk terlebih dahulu.");
                return;
            }

            string qrisData = $"PAYMENT|TOKO_KASIHKU|TOTAL|{model.Total}";
            ShowQRCode(qrisData, model.Total);
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            KembaliClicked?.Invoke();
        }

        private void ShowQRCode(string data, decimal total)
        {
            using (var qrForm = new Form())
            {
                qrForm.Text = "Pembayaran via QRIS";
                qrForm.Size = new Size(400, 500);

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrImage = qrCode.GetGraphic(10);

                var pictureBox = new PictureBox()
                {
                    Image = qrImage,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Size = new Size(300, 300),
                    Location = new Point(40, 30)
                };

                var label = new Label()
                {
                    Text = $"Silakan scan QR untuk membayar\nJumlah: Rp {total:N0}",
                    Size = new Size(350, 80),
                    Location = new Point(20, 350),
                    Font = new Font("Segoe UI", 10),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                qrForm.Controls.Add(pictureBox);
                qrForm.Controls.Add(label);

                qrForm.StartPosition = FormStartPosition.CenterParent;
                qrForm.ShowDialog();
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

            string logoPath = @"C:\Users\Reza\Downloads\LOGO hitam.png";
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

            foreach (var item in lastKeranjang)
            {
                var (nama, harga, jumlah) = item.Value;
                string namaPendek = nama.Length > 16 ? nama.Substring(0, 16) : nama;
                string line = $"{jumlah.ToString().PadRight(4)} {namaPendek.PadRight(18)} Rp {(harga * jumlah):N0}";
                doc.Add(new Paragraph(line).SetFont(normalFont).SetFontSize(10));
            }

            doc.Add(new Paragraph("========================================================================"));
            doc.Add(new Paragraph($"TOTAL BAYAR:     Rp {total:N0}").SetFont(boldFont).SetFontSize(11));
            doc.Add(new Paragraph("========================================================================"));
            doc.Add(new Paragraph("Terima kasih telah berbelanja!").SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Toko Kasihku - 2025").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8));

            System.Diagnostics.Process.Start("explorer", filePath);
        }
    }
}