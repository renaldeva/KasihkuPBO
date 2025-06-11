namespace KasihkuPBO.View
{
    partial class TransaksiControl
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
            btnProdukKasir = new Button();
            btnRiwayatKasir = new Button();
            btnDashboard = new Button();
            SuspendLayout();
            // TransaksiControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnDashboard);
            Controls.Add(btnRiwayatKasir);
            Controls.Add(btnProdukKasir);
            Name = "TransaksiControl";
            Size = new Size(1920, 1080);
            ResumeLayout(false);
        }

        #endregion

        private Button btnProdukKasir;
        private Button btnRiwayatKasir;
        private Button btnDashboard;
    }
}
