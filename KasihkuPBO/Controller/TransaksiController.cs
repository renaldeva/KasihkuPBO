using System;
using System.Collections.Generic;
using KasihkuPBO.Model;
using Npgsql;

namespace KasihkuPBO.Controller
{
    public class TransaksiController
    {
        private readonly string connectionString;
        private readonly TransaksiModel model;

        public TransaksiController(string connStr, TransaksiModel model)
        {
            this.connectionString = connStr;
            this.model = model;
        }

        public int SimpanTransaksi(out string tanggal, out string daftarProduk, string status)
        {
            tanggal = DateTime.Now.ToString("yyyy-MM-dd");
            daftarProduk = "";
            int idTransaksi = 0;

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();

            try
            {
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO transaksi (tanggal, total, status) VALUES (@tanggal, @total, @status) RETURNING id_transaksi", conn))
                {
                    cmd.Parameters.AddWithValue("@tanggal", DateTime.Now);
                    cmd.Parameters.AddWithValue("@total", model.Total);
                    cmd.Parameters.AddWithValue("@status", status); 
                    cmd.Transaction = trans;
                    idTransaksi = Convert.ToInt32(cmd.ExecuteScalar());
                }

                foreach (var item in model.Keranjang)
                {
                    var (nama, harga, jumlah) = item.Value;

                    using (var cmdDetail = new NpgsqlCommand(@"
                INSERT INTO detail_transaksi 
                (id_transaksi, id_produk, nama_produk, jumlah, harga, subtotal)
                VALUES (@id_transaksi, @id_produk, @nama_produk, @jumlah, @harga, @subtotal)", conn))
                    {
                        cmdDetail.Parameters.AddWithValue("@id_transaksi", idTransaksi);
                        cmdDetail.Parameters.AddWithValue("@id_produk", item.Key);
                        cmdDetail.Parameters.AddWithValue("@nama_produk", nama);
                        cmdDetail.Parameters.AddWithValue("@jumlah", jumlah);
                        cmdDetail.Parameters.AddWithValue("@harga", harga);
                        cmdDetail.Parameters.AddWithValue("@subtotal", harga * jumlah);
                        cmdDetail.Transaction = trans;
                        cmdDetail.ExecuteNonQuery();
                    }

                    using (var cmdUpdate = new NpgsqlCommand("UPDATE produk SET stok = stok - @jumlah WHERE id_produk = @id_produk", conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@jumlah", jumlah);
                        cmdUpdate.Parameters.AddWithValue("@id_produk", item.Key);
                        cmdUpdate.Transaction = trans;
                        cmdUpdate.ExecuteNonQuery();
                    }

                    daftarProduk += $"{nama} x{jumlah}, ";
                }

                trans.Commit();

                if (daftarProduk.EndsWith(", "))
                    daftarProduk = daftarProduk[..^2];
            }
            catch
            {
                trans.Rollback();
                throw;
            }

            return idTransaksi;
        }

    }
}
