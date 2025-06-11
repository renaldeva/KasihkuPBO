using KasihkuPBO.Model;
using KasihkuPBO.View;
using System;

namespace KasihkuPBO.Controller
{
    public class RiwayatTransaksiController
    {
        private readonly RiwayatTransaksiModel _model;
        private readonly RiwayatTransaksiControl _view;

        public RiwayatTransaksiController(RiwayatTransaksiControl view)
        {
            _view = view;
            _model = new RiwayatTransaksiModel();

            // Wire up view events
            _view.Load += (s, e) => LoadData();
            _view.RefreshRequested += (s, e) => LoadData();
        }

        private void LoadData()
        {
            try
            {
                _view.ResetRiwayat();
                var transaksiList = _model.GetRiwayatTransaksi();

                foreach (var transaksi in transaksiList)
                {
                    _view.TambahRiwayat(transaksi.Tanggal, transaksi.DaftarProduk, transaksi.Total);
                }
            }
            catch (Exception ex)
            {
                _view.TampilkanError("Gagal memuat riwayat transaksi: " + ex.Message);
            }
        }
    }
}