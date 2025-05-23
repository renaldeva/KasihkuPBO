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
    public partial class FormAdmin : Form
    {
        public FormAdmin(string username)
        {
            InitializeComponent();
            label1.Text = "Selamat datang di aplikasi Kasihku," + " " + username;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide(); // Sembunyikan form admin
            FormLogin loginForm = new FormLogin(); // Kembali ke login
            loginForm.Show();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
