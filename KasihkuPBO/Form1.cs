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
                    string sql = "SELECT COUNT(*) FROM login WHERE username = @user AND kata_sandi = @pass";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("user", usernameInput);
                        cmd.Parameters.AddWithValue("pass", passwordInput);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Login berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // TODO: buka form selanjutnya
                            Form2 form2 = new Form2(LoginUsername.Text);
                            form2.Show();

                            this.Hide(); // Sembunyikan Form1 (form login)
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
