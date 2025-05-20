using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;


namespace KasihkuPBO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoginUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connString = "Host=localhost;Port=5432;Username=postgres;Password=Dev@211104;Database=KASIHKU;";
            string usernameInput = LoginUsername.Text.Trim();
            string passwordInput = LoginPassword.Text.Trim();

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT role_job FROM login WHERE username = @user AND kata_sandi = @pass";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("user", usernameInput);
                        cmd.Parameters.AddWithValue("pass", passwordInput);

                        var roleObj = cmd.ExecuteScalar();

                        if (roleObj != null)
                        {
                            string role = roleObj.ToString().ToLower();

                            MessageBox.Show("Login berhasil sebagai " + role, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (role == "admin")
                            {
                                Form2 formAdmin = new Form2(LoginUsername.Text); // Form untuk admin
                                formAdmin.Show();
                            }
                            else if (role == "kasir")
                            {
                                Form3 formKasir = new Form3(LoginUsername.Text); // Form untuk kasir
                                formKasir.Show();
                            }
                            else
                            {
                                MessageBox.Show("Role tidak dikenali.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            this.Hide(); // Sembunyikan form login
                        }
                        else
                        {
                            MessageBox.Show("Login gagal. Username atau password salah.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);

            }
        }
    }
}
