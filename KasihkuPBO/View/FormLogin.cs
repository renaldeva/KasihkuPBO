using KasihkuPBO.Controller;
using KasihkuPBO.Model;
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
    public partial class FormLogin : Form
    {
        private readonly AuthController authController;

        public FormLogin()
        {
            InitializeComponent();
            authController = new AuthController();

            // Tambahkan handler KeyDown untuk menangkap tombol Enter
            LoginUsername.KeyDown += LoginUsername_KeyDown;
            LoginPassword.KeyDown += LoginPassword_KeyDown;
        }

        private void LoginUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = new UserModel
            {
                Username = LoginUsername.Text.Trim(),
                Password = LoginPassword.Text.Trim()
            };

            try
            {
                string role = authController.Login(user);

                if (role != null)
                {
                    MessageBox.Show("Login berhasil sebagai " + role, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (role.ToLower() == "admin")
                    {
                        FormAdmin formAdmin = new FormAdmin(user.Username);
                        formAdmin.Show();
                    }
                    else if (role.ToLower() == "kasir")
                    {
                        FormKasir formKasir = new FormKasir(user.Username);
                        formKasir.Show();
                    }
                    else
                    {
                        MessageBox.Show("Role tidak dikenali.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login gagal. Username atau password salah.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoginPassword.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoginPassword.UseSystemPasswordChar = !checkBox1.Checked!;
        }

        // Fitur tambahan: tekan Enter untuk login
        private void LoginUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginPassword.Focus(); // Pindah fokus ke password
            }
        }

        private void LoginPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick(); // Tekan tombol login secara otomatis
            }
        }
    }
}
