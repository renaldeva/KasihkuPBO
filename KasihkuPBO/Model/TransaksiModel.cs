using System;
using System.Collections.Generic;

namespace KasihkuPBO.Model
{
    public class TransaksiModel
    {
        public Dictionary<int, (string nama, decimal harga, int jumlah)> Keranjang { get; private set; } = new();
        public decimal Total { get; private set; }

        public void TambahProduk(int id, string nama, decimal harga)
        {
            if (Keranjang.ContainsKey(id))
                Keranjang[id] = (nama, harga, Keranjang[id].jumlah + 1);
            else
                Keranjang[id] = (nama, harga, 1);

            HitungTotal();
        }

        public void KurangiProduk(int id)
        {
            if (!Keranjang.ContainsKey(id)) return;

            var item = Keranjang[id];
            if (item.jumlah <= 1)
                Keranjang.Remove(id);
            else
                Keranjang[id] = (item.nama, item.harga, item.jumlah - 1);

            HitungTotal();
        }

        public void Reset()
        {
            Keranjang.Clear();
            Total = 0;
        }

        private void HitungTotal()
        {
            decimal total = 0;
            foreach (var item in Keranjang.Values)
                total += item.harga * item.jumlah;

            Total = total;
        }
    }
}
