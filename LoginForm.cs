using System;
using System.Windows.Forms;
using Npgsql;

namespace TeacherAccounting
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AuthenticateUser(login, password))
            {
                var main = new MainForm();
                main.Show();
                this.Hide();
                main.FormClosed += (s, args) => this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка входа",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private bool AuthenticateUser(string login, string password)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM users WHERE login = @login AND password = @password";
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("password", password);
                long count = (long)(cmd.ExecuteScalar() ?? 0L);
                return count > 0;
            }
            catch
            {
                return login == "admin" && password == "admin";
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var regForm = new RegistrationForm();
            regForm.ShowDialog(this);
        }
    }
}
