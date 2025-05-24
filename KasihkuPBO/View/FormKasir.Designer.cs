namespace KasihkuPBO
{
    partial class FormKasir
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKasir));
            label1 = new Label();
            button1 = new Button();
            btnRiwayatKasir = new Button();
            btnProdukKasir = new Button();
            btnTransaksiKasir = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.DarkGreen;
            label1.Location = new Point(450, 161);
            label1.Name = "label1";
            label1.Size = new Size(166, 65);
            label1.TabIndex = 0;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Red;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(100, 899);
            button1.Name = "button1";
            button1.Size = new Size(170, 54);
            button1.TabIndex = 2;
            button1.Text = "LOGOUT";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // btnRiwayatKasir
            // 
            btnRiwayatKasir.ForeColor = Color.Transparent;
            btnRiwayatKasir.Image = (Image)resources.GetObject("btnRiwayatKasir.Image");
            btnRiwayatKasir.Location = new Point(12, 424);
            btnRiwayatKasir.Name = "btnRiwayatKasir";
            btnRiwayatKasir.Size = new Size(372, 73);
            btnRiwayatKasir.TabIndex = 5;
            btnRiwayatKasir.UseVisualStyleBackColor = true;
            btnRiwayatKasir.Click += btnRiwayatKasir_Click;
            // 
            // btnProdukKasir
            // 
            btnProdukKasir.BackColor = Color.Transparent;
            btnProdukKasir.ForeColor = Color.Transparent;
            btnProdukKasir.Image = (Image)resources.GetObject("btnProdukKasir.Image");
            btnProdukKasir.Location = new Point(12, 280);
            btnProdukKasir.Name = "btnProdukKasir";
            btnProdukKasir.Size = new Size(372, 70);
            btnProdukKasir.TabIndex = 6;
            btnProdukKasir.UseVisualStyleBackColor = false;
            btnProdukKasir.Click += btnProdukKasir_Click;
            // 
            // btnTransaksiKasir
            // 
            btnTransaksiKasir.ForeColor = Color.Transparent;
            btnTransaksiKasir.Image = (Image)resources.GetObject("btnTransaksiKasir.Image");
            btnTransaksiKasir.Location = new Point(12, 352);
            btnTransaksiKasir.Name = "btnTransaksiKasir";
            btnTransaksiKasir.Size = new Size(372, 70);
            btnTransaksiKasir.TabIndex = 7;
            btnTransaksiKasir.UseVisualStyleBackColor = true;
            btnTransaksiKasir.Click += btnTransaksiKasir_Click;
            // 
            // FormKasir
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1904, 1041);
            Controls.Add(btnTransaksiKasir);
            Controls.Add(btnProdukKasir);
            Controls.Add(btnRiwayatKasir);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "FormKasir";
            Text = "Form3";
            Load += Form3_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Button btnRiwayatKasir;
        private Button btnProdukKasir;
        private Button btnTransaksiKasir;
    }
}