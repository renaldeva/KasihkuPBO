﻿namespace KasihkuPBO.View
{
    partial class RiwayatTransaksiControl
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
            SuspendLayout();
            // 
            // RiwayatTransaksiControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "RiwayatTransaksiControl";
            Size = new Size(1920, 1080);
            Load += RiwayatTransaksiControl_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnTransaksiKasir;
        private Button btnProdukKasir;
        private Button btnDashboard;
    }
}
