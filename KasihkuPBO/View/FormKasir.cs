using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace KasihkuPBO
{
    public partial class FormKasir : Form
    {
        public FormKasir(string username)
        {
            InitializeComponent();
            label1.Text = "Selamat datang di aplikasi Kasihku," + " " + username;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
