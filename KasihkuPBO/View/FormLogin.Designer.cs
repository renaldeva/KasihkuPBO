namespace KasihkuPBO
{
    partial class FormLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            button1 = new Button();
            LoginUsername = new TextBox();
            LoginPassword = new TextBox();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Book Antiqua", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Transparent;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.Location = new Point(614, 767);
            button1.Name = "button1";
            button1.Size = new Size(709, 90);
            button1.TabIndex = 1;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // LoginUsername
            // 
            LoginUsername.BackColor = SystemColors.Window;
            LoginUsername.BorderStyle = BorderStyle.None;
            LoginUsername.Cursor = Cursors.Hand;
            LoginUsername.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LoginUsername.Location = new Point(576, 456);
            LoginUsername.Name = "LoginUsername";
            LoginUsername.Size = new Size(747, 43);
            LoginUsername.TabIndex = 5;
            LoginUsername.TextChanged += LoginUsername_TextChanged;
            // 
            // LoginPassword
            // 
            LoginPassword.BorderStyle = BorderStyle.None;
            LoginPassword.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LoginPassword.Location = new Point(576, 636);
            LoginPassword.Name = "LoginPassword";
            LoginPassword.Size = new Size(747, 43);
            LoginPassword.TabIndex = 6;
            LoginPassword.TextChanged += LoginPassword_TextChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Location = new Point(1278, 738);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(134, 19);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "Tampilkan Password";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1904, 1041);
            Controls.Add(checkBox1);
            Controls.Add(LoginPassword);
            Controls.Add(LoginUsername);
            Controls.Add(button1);
            Margin = new Padding(2);
            Name = "FormLogin";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private TextBox LoginUsername;
        private TextBox LoginPassword;
        private CheckBox checkBox1;
    }
}
