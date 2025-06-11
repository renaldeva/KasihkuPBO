using System.Collections.Generic;
using KasihkuPBO.Model;

namespace KasihkuPBO.Controller
{
    public class RiwayatAdminController
    {
        private RiwayatTransaksiModel model = new();

        public List<Transaksi> AmbilSemuaRiwayat()
        {
            return model.GetRiwayatTransaksi();
        }
    }
}