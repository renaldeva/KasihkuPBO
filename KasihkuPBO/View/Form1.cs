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
using static System.Windows.Forms.DataFormats;


namespace KasihkuPBO
{
    public partial class Form1 : Form
    {
        private readonly AuthController authController;

        public Form1()
        {
            InitializeComponent();
            authController = new AuthController();
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
                        Form2 formAdmin = new Form2(user.Username);
                        formAdmin.Show();
                    }
                    else if (role.ToLower() == "kasir")
                    {
                        Form3 formKasir = new Form3(user.Username);
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
    }
}