using System.Collections.Generic;
using Npgsql;

namespace KasihkuPBO.Model
{
    public class Transaksi
    {
        public string Tanggal { get; set; }
        public string DaftarProduk { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }

    public class RiwayatTransaksiModel
    {
        private const string ConnString = "Host=localhost;Username=postgres;Password=Dev@211104;Database=KASIHKU";

        public List<Transaksi> GetRiwayatTransaksi()
        {
            var riwayat = new List<Transaksi>();
            string query = @"
            SELECT t.tanggal, 
                   STRING_AGG(d.nama_produk || ' x' || d.jumlah, ', ') AS daftar_produk, 
                   t.total,
                   t.status -- 🟢 Tambahkan kolom status
            FROM transaksi t
            JOIN detail_transaksi d ON t.id_transaksi = d.id_transaksi
            GROUP BY t.id_transaksi, t.tanggal, t.total, t.status
            ORDER BY t.tanggal DESC
        ";

            using (var conn = new NpgsqlConnection(ConnString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        riwayat.Add(new Transaksi
                        {
                            Tanggal = reader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm"),
                            DaftarProduk = reader.GetString(1),
                            Total = reader.GetDecimal(2),
                            Status = reader.GetString(3) // 🟢 Ambil status dari kolom ke-4
                        });
                    }
                }
            }

            return riwayat;
        }
    }
}