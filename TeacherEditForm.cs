using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Npgsql;

namespace TeacherAccounting
{
    public partial class TeacherEditForm : Form
    {
        public int? TeacherId { get; private set; }

        public TeacherEditForm(int? teacherId = null)
        {
            InitializeComponent();
            TeacherId = teacherId;
            LoadDepartments();
            LoadPositions();

            if (teacherId.HasValue)
            {
                this.Text = "Изменение данных преподавателя";
                LoadTeacherData(teacherId.Value);
            }
            else
            {
                this.Text = "Добавление преподавателя";
            }
        }

        private void LoadDepartments()
        {
            try
            {
                cmbDepartment.Items.Clear();
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, name FROM departments ORDER BY name";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    cmbDepartment.Items.Add(new ComboItem(reader.GetInt32(0), reader.GetString(1)));
                cmbDepartment.DisplayMember = "Name";
                cmbDepartment.ValueMember = "Id";
            }
            catch
            {
                cmbDepartment.Items.Add(new ComboItem(1, "Кафедра математики"));
                cmbDepartment.Items.Add(new ComboItem(2, "Кафедра информатики"));
                cmbDepartment.Items.Add(new ComboItem(3, "Кафедра физики"));
                cmbDepartment.DisplayMember = "Name";
                cmbDepartment.ValueMember = "Id";
            }
        }

        private void LoadPositions()
        {
            try
            {
                cmbPosition.Items.Clear();
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, name FROM positions ORDER BY name";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    cmbPosition.Items.Add(new ComboItem(reader.GetInt32(0), reader.GetString(1)));
                cmbPosition.DisplayMember = "Name";
                cmbPosition.ValueMember = "Id";
            }
            catch
            {
                cmbPosition.Items.Add(new ComboItem(1, "Профессор"));
                cmbPosition.Items.Add(new ComboItem(2, "Доцент"));
                cmbPosition.Items.Add(new ComboItem(3, "Старший преподаватель"));
                cmbPosition.Items.Add(new ComboItem(4, "Преподаватель"));
                cmbPosition.Items.Add(new ComboItem(5, "Ассистент"));
                cmbPosition.DisplayMember = "Name";
                cmbPosition.ValueMember = "Id";
            }
        }

        private void LoadTeacherData(int id)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT full_name, department_id, position_id, phone, email FROM teachers WHERE id = @id";
                cmd.Parameters.AddWithValue("id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtFullName.Text = reader.GetString(0);
                    SelectComboById(cmbDepartment, reader.GetInt32(1));
                    SelectComboById(cmbPosition, reader.GetInt32(2));
                    txtPhone.Text = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    txtEmail.Text = reader.IsDBNull(4) ? "" : reader.GetString(4);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectComboById(System.Windows.Forms.ComboBox combo, int id)
        {
            foreach (ComboItem item in combo.Items)
            {
                if (item.Id == id)
                {
                    combo.SelectedItem = item;
                    return;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Поле 'ФИО' обязательно для заполнения.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (cmbDepartment.SelectedItem == null)
            {
                MessageBox.Show("Выберите кафедру.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPosition.SelectedItem == null)
            {
                MessageBox.Show("Выберите должность.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var deptItem = (ComboItem)cmbDepartment.SelectedItem;
                var posItem = (ComboItem)cmbPosition.SelectedItem;
                string fullName = txtFullName.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string email = txtEmail.Text.Trim();

                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();

                if (TeacherId.HasValue)
                {
                    cmd.CommandText = @"UPDATE teachers
                        SET full_name = @name, department_id = @dept, position_id = @pos,
                            phone = @phone, email = @email
                        WHERE id = @id";
                    cmd.Parameters.AddWithValue("id", TeacherId.Value);
                }
                else
                {
                    cmd.CommandText = @"INSERT INTO teachers (full_name, department_id, position_id, phone, email)
                        VALUES (@name, @dept, @pos, @phone, @email)";
                }

                cmd.Parameters.AddWithValue("name", fullName);
                cmd.Parameters.AddWithValue("dept", deptItem.Id);
                cmd.Parameters.AddWithValue("pos", posItem.Id);
                cmd.Parameters.AddWithValue("phone", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);
                cmd.Parameters.AddWithValue("email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                cmd.ExecuteNonQuery();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }

    public class ComboItem
    {
        public int Id { get; }
        public string Name { get; }

        public ComboItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;
    }
}
