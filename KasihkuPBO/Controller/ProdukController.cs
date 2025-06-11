using KasihkuPBO.Model;
using System.Collections.Generic;

namespace KasihkuPBO.Controller
{
    public class ProdukController
    {
        private ProdukModel produkModel = new ProdukModel();

        public List<Produk> CariProduk(string keyword)
        {
            return produkModel.GetProduk(keyword);
        }

        public List<string> RekomendasiProduk(List<string> preferensi)
        {
            return produkModel.GetRekomendasiProduk(preferensi);
        }
    }
}
