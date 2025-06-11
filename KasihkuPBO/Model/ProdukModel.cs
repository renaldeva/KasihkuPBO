using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

namespace KasihkuPBO.Model
{
    public class ProdukModel
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Dev@211104;Database=KASIHKU";

        public List<Produk> GetProduk(string keyword = "")
        {
            var listProduk = new List<Produk>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT p.id_produk, p.nama_produk, p.stok, p.deskripsi, p.gambar, p.harga, k.nama_kategori
                    FROM produk p
                    JOIN kategori_produk k ON p.id_kategori = k.id_kategori
                    WHERE LOWER(p.nama_produk) LIKE @keyword
                    ORDER BY p.id_produk";
                var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("keyword", $"%{keyword.ToLower()}%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listProduk.Add(new Produk
                        {
                            Id = reader.GetInt32(0),
                            Nama = reader.GetString(1),
                            Stok = reader.GetInt32(2),
                            Deskripsi = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            Gambar = reader.IsDBNull(4) ? null : (byte[])reader[4],
                            Harga = reader.GetDecimal(5),
                            Kategori = reader.GetString(6)
                        });
                    }
                }
            }

            return listProduk;
        }

        public List<string> GetRekomendasiProduk(List<string> preferensi)
        {
            var hasil = new List<string>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var conditions = string.Join(" OR ", preferensi.ConvertAll(p => $"LOWER(p.nama_produk) LIKE '%{p}%'"));
                var query = $@"
                    SELECT DISTINCT p.nama_produk
                    FROM produk p
                    JOIN kategori_produk k ON p.id_kategori = k.id_kategori
                    WHERE {conditions}
                    ORDER BY p.nama_produk";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        hasil.Add(reader.GetString(0));
                    }
                }
            }
            return hasil;
        }
    }

    public class Produk
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public int Stok { get; set; }
        public string Deskripsi { get; set; }
        public byte[] Gambar { get; set; }
        public decimal Harga { get; set; }
        public string Kategori { get; set; }
    }
}
