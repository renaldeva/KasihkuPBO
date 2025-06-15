using System;
using System.Collections.Generic;

namespace KasihkuPBO.Model
{
    public class TransaksiModel
    {
        // Keranjang menyimpan data produk dengan jumlah
        public Dictionary<int, (string nama, decimal harga, int jumlah)> Keranjang { get; private set; } = new();

        // Total harga transaksi
        public decimal Total { get; private set; }

        // Stok produk yang tersedia, id_produk sebagai key
        private Dictionary<int, int> StokProduk { get; set; } = new();

        // Menambahkan produk ke keranjang dan mengurangi stok
        public void TambahProduk(int id, string nama, decimal harga)
        {
            if (StokProduk.ContainsKey(id) && StokProduk[id] > 0) // Pastikan stok masih tersedia
            {
                if (Keranjang.ContainsKey(id))
                {
                    Keranjang[id] = (nama, harga, Keranjang[id].jumlah + 1);
                }
                else
                {
                    Keranjang[id] = (nama, harga, 1);
                }

                // Kurangi stok setelah produk ditambahkan ke keranjang
                StokProduk[id]--;

                HitungTotal();
            }
            else
            {
                // Jika stok habis, tampilkan pesan
                Console.WriteLine($"Stok produk {nama} tidak mencukupi.");
            }
        }

        // Mengurangi produk dari keranjang dan menambah stok
        public void KurangiProduk(int id)
        {
            if (!Keranjang.ContainsKey(id)) return;

            var item = Keranjang[id];
            if (item.jumlah <= 1)
            {
                Keranjang.Remove(id);
                // Tambahkan stok kembali jika produk dihapus dari keranjang
                StokProduk[id]++;
            }
            else
            {
                Keranjang[id] = (item.nama, item.harga, item.jumlah - 1);
                // Tambahkan stok kembali
                StokProduk[id]++;
            }

            HitungTotal();
        }

        // Reset keranjang dan stok produk
        public void Reset()
        {
            Keranjang.Clear();
            Total = 0;
        }

        // Menambahkan stok produk (misal untuk admin yang menambah stok)
        public void TambahStok(int id, int jumlah)
        {
            if (StokProduk.ContainsKey(id))
                StokProduk[id] += jumlah;
            else
                StokProduk[id] = jumlah;
        }

        // Menghitung total harga dari keranjang
        private void HitungTotal()
        {
            decimal total = 0;
            foreach (var item in Keranjang.Values)
                total += item.harga * item.jumlah;

            Total = total;
        }

        // Mengatur stok produk yang tersedia (untuk di-load dari database atau sumber lain)
        public void SetStokProduk(Dictionary<int, int> stok)
        {
            StokProduk = stok;
        }
    }
}
