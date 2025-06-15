
namespace KasihkuPBO
{
    partial class FormAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdmin));
            label1 = new Label();
            button1 = new Button();
            btnAdminProduk = new Button();
            btnAdminTransaksi = new Button();
            btnAdminRiwayat = new Button();
            btnAdminManajemen = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.DarkGreen;
            label1.Location = new Point(450, 177);
            label1.Name = "label1";
            label1.Size = new Size(383, 65);
            label1.TabIndex = 0;
            label1.Text = "Selamat datang";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Red;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(113, 889);
            button1.Name = "button1";
            button1.Size = new Size(170, 54);
            button1.TabIndex = 1;
            button1.Text = "LOGOUT";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // btnAdminProduk
            // 
            btnAdminProduk.BackColor = Color.Transparent;
            btnAdminProduk.BackgroundImageLayout = ImageLayout.Zoom;
            btnAdminProduk.FlatStyle = FlatStyle.Popup;
            btnAdminProduk.ForeColor = Color.Transparent;
            btnAdminProduk.Location = new Point(12, 279);
            btnAdminProduk.Name = "btnAdminProduk";
            btnAdminProduk.Size = new Size(378, 67);
            btnAdminProduk.TabIndex = 3;
            btnAdminProduk.UseVisualStyleBackColor = false;
            btnAdminProduk.Click += btnAdminProduk_Click;
            // 
            // btnAdminTransaksi
            // 
            btnAdminTransaksi.BackColor = Color.Transparent;
            btnAdminTransaksi.FlatStyle = FlatStyle.Popup;
            btnAdminTransaksi.ForeColor = Color.Transparent;
            btnAdminTransaksi.Location = new Point(12, 346);
            btnAdminTransaksi.Name = "btnAdminTransaksi";
            btnAdminTransaksi.Size = new Size(378, 77);
            btnAdminTransaksi.TabIndex = 4;
            btnAdminTransaksi.UseVisualStyleBackColor = false;
            btnAdminTransaksi.Click += btnAdminTransaksi_Click;
            // 
            // btnAdminRiwayat
            // 
            btnAdminRiwayat.BackColor = Color.Transparent;
            btnAdminRiwayat.BackgroundImage = (Image)resources.GetObject("btnAdminRiwayat.BackgroundImage");
            btnAdminRiwayat.BackgroundImageLayout = ImageLayout.Zoom;
            btnAdminRiwayat.FlatStyle = FlatStyle.Popup;
            btnAdminRiwayat.ForeColor = Color.Transparent;
            btnAdminRiwayat.Location = new Point(9, 294);
            btnAdminRiwayat.Name = "btnAdminRiwayat";
            btnAdminRiwayat.Size = new Size(378, 77);
            btnAdminRiwayat.TabIndex = 5;
            btnAdminRiwayat.UseVisualStyleBackColor = false;
            btnAdminRiwayat.Click += btnAdminRiwayat_Click;
            // 
            // btnAdminManajemen
            // 
            btnAdminManajemen.BackColor = Color.Transparent;
            btnAdminManajemen.BackgroundImage = (Image)resources.GetObject("btnAdminManajemen.BackgroundImage");
            btnAdminManajemen.BackgroundImageLayout = ImageLayout.Zoom;
            btnAdminManajemen.FlatStyle = FlatStyle.Popup;
            btnAdminManajemen.ForeColor = Color.Transparent;
            btnAdminManajemen.Location = new Point(9, 371);
            btnAdminManajemen.Name = "btnAdminManajemen";
            btnAdminManajemen.Size = new Size(378, 77);
            btnAdminManajemen.TabIndex = 6;
            btnAdminManajemen.UseVisualStyleBackColor = false;
            btnAdminManajemen.Click += btnAdminManajemen_Click;
            // 
            // FormAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1904, 1041);
            Controls.Add(btnAdminManajemen);
            Controls.Add(btnAdminRiwayat);
            Controls.Add(btnAdminTransaksi);
            Controls.Add(btnAdminProduk);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "FormAdmin";
            Text = "Form2";
            Load += FormAdmin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Button btnAdminProduk;
        private Button btnAdminTransaksi;
        private Button btnAdminRiwayat;
        private Button btnAdminManajemen;
    }
}