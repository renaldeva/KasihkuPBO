
namespace KasihkuPBO.View
{
    partial class ProdukKasirControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnTransaksiKasir = new Button();
            btnRiwayatKasir = new Button();
            btnDashboard = new Button();
            SuspendLayout();
            // 
            // btnTransaksiKasir
            // 
            btnTransaksiKasir.Location = new Point(0, 0);
            btnTransaksiKasir.Name = "btnTransaksiKasir";
            btnTransaksiKasir.Size = new Size(75, 23);
            btnTransaksiKasir.TabIndex = 2;
            // 
            // btnRiwayatKasir
            // 
            btnRiwayatKasir.Location = new Point(0, 0);
            btnRiwayatKasir.Name = "btnRiwayatKasir";
            btnRiwayatKasir.Size = new Size(75, 23);
            btnRiwayatKasir.TabIndex = 1;
            // 
            // btnDashboard
            // 
            btnDashboard.Location = new Point(0, 0);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(75, 23);
            btnDashboard.TabIndex = 0;
            // 
            // ProdukKasirControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Produk;
            Controls.Add(btnDashboard);
            Controls.Add(btnRiwayatKasir);
            Controls.Add(btnTransaksiKasir);
            Name = "ProdukKasirControl";
            Size = new Size(1920, 1080);
            Load += ProdukKasirControl_Load;
            ResumeLayout(false);
        }

        private void ProdukKasirControl_Load(object sender, EventArgs e)
        {

        }

        #endregion

        private Button btnTransaksiKasir;
        private Button btnRiwayatKasir;
        private Button btnDashboard;
    }
}
