using KasihkuPBO.Model;
using Npgsql;
using System;

namespace KasihkuPBO.Controller
{
    public class AuthController
    {
        private readonly string connString = "Host=localhost;Port=5432;Username=postgres;Password=Dev@211104;Database=KASIHKU";


        public string Login(UserModel user)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT role_job FROM login WHERE username = @user AND kata_sandi = @pass";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("user", user.Username);
                    cmd.Parameters.AddWithValue("pass", user.Password);

                    var roleObj = cmd.ExecuteScalar();
                    return roleObj?.ToString();
                }
            }
        }
    }
}
