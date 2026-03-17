using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace TeacherAccounting
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadDepartments();
            LoadTeachers();
        }

        private void LoadDepartments()
        {
            cboDepartment.Items.Clear();
            cboDepartment.Items.Add("Все кафедры");
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT name FROM departments ORDER BY name";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    cboDepartment.Items.Add(reader.GetString(0));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке кафедр: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cboDepartment.SelectedIndex = 0;
        }

        private void LoadTeachers()
        {
            string searchText = txtSearch.Text.Trim();
            string selectedDept = cboDepartment.SelectedIndex > 0
                ? cboDepartment.SelectedItem!.ToString()!
                : string.Empty;

            var sql = new StringBuilder(@"
                SELECT t.id, t.full_name, d.name AS department, p.name AS position, t.phone, t.email
                FROM teachers t
                LEFT JOIN departments d ON t.department_id = d.id
                LEFT JOIN positions   p ON t.position_id   = p.id
                WHERE 1=1");

            if (!string.IsNullOrEmpty(searchText))
                sql.Append(" AND LOWER(t.full_name) LIKE LOWER(@search)");

            if (!string.IsNullOrEmpty(selectedDept))
                sql.Append(" AND d.name = @dept");

            sql.Append(" ORDER BY t.full_name");

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql.ToString();

                if (!string.IsNullOrEmpty(searchText))
                    cmd.Parameters.AddWithValue("search", "%" + searchText + "%");

                if (!string.IsNullOrEmpty(selectedDept))
                    cmd.Parameters.AddWithValue("dept", selectedDept);

                using var adapter = new NpgsqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

                dgvTeachers.DataSource = table;

                if (dgvTeachers.Columns.Count > 0)
                {
                    dgvTeachers.Columns["id"]!.Visible = false;
                    SetColumnHeader("full_name",  "ФИО");
                    SetColumnHeader("department", "Кафедра");
                    SetColumnHeader("position",   "Должность");
                    SetColumnHeader("phone",      "Телефон");
                    SetColumnHeader("email",      "E-mail");
                }

                statusLabel.Text = $"Всего преподавателей: {table.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Ошибка загрузки данных.";
            }
        }

        private void SetColumnHeader(string colName, string header)
        {
            if (dgvTeachers.Columns.Contains(colName))
                dgvTeachers.Columns[colName]!.HeaderText = header;
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            LoadTeachers();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var form = new TeacherEditForm();
            if (form.ShowDialog(this) == DialogResult.OK)
                LoadTeachers();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.CurrentRow == null) return;
            if (dgvTeachers.CurrentRow.DataBoundItem is not DataRowView row) return;
            int id = Convert.ToInt32(row["id"]);
            using var form = new TeacherEditForm(id);
            if (form.ShowDialog(this) == DialogResult.OK)
                LoadTeachers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.CurrentRow == null) return;
            if (dgvTeachers.CurrentRow.DataBoundItem is not DataRowView row) return;
            int id = Convert.ToInt32(row["id"]);
            string name = row["full_name"]?.ToString() ?? string.Empty;

            var result = MessageBox.Show(
                $"Удалить преподавателя «{name}»?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM teachers WHERE id = @id";
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                LoadTeachers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTeachers();
        }

        private void btnTeachingLoad_Click(object sender, EventArgs e)
        {
            using var form = new TeachingLoadForm();
            form.ShowDialog(this);
        }

        private void MenuExport_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.DataSource is not DataTable table || table.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта.", "Экспорт",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new SaveFileDialog();
            dlg.Filter = "CSV файлы (*.csv)|*.csv|Текстовые файлы (*.txt)|*.txt";
            dlg.FileName = "Преподаватели";
            dlg.DefaultExt = "csv";

            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                using var writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
                writer.WriteLine("ФИО;Кафедра;Должность;Телефон;E-mail");

                foreach (DataRow row in table.Rows)
                {
                    string fio   = row["full_name"]?.ToString()  ?? string.Empty;
                    string dept  = row["department"]?.ToString() ?? string.Empty;
                    string pos   = row["position"]?.ToString()   ?? string.Empty;
                    string phone = row["phone"]?.ToString()      ?? string.Empty;
                    string email = row["email"]?.ToString()      ?? string.Empty;
                    writer.WriteLine($"{Escape(fio)};{Escape(dept)};{Escape(pos)};{Escape(phone)};{Escape(email)}");
                }

                MessageBox.Show($"Файл сохранён:\n{dlg.FileName}", "Экспорт выполнен",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Escape(string value)
        {
            if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }

        private void MenuStats_Click(object sender, EventArgs e)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT d.name AS department, COUNT(t.id) AS cnt
                    FROM departments d
                    LEFT JOIN teachers t ON t.department_id = d.id
                    GROUP BY d.name
                    ORDER BY d.name";

                using var reader = cmd.ExecuteReader();
                var sb = new StringBuilder();
                sb.AppendLine("Статистика по кафедрам:");
                sb.AppendLine(new string('─', 40));
                while (reader.Read())
                {
                    string dept = reader.GetString(0);
                    long count  = reader.GetInt64(1);
                    sb.AppendLine($"{dept}: {count} чел.");
                }

                MessageBox.Show(sb.ToString(), "Статистика кафедр",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении статистики: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
