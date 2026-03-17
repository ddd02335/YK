using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Npgsql;

namespace TeacherAccounting
{
    public partial class TeachingLoadForm : Form
    {
        public TeachingLoadForm()
        {
            InitializeComponent();
            LoadTeachers();
            LoadDisciplines();
            LoadData();
            cboSemester.SelectedIndex = 0;
        }

        private void LoadData()
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = "SELECT tl.load_id, t.full_name, d.discipline_name, tl.hours_count, tl.semester_type " +
                           "FROM teaching_load tl " +
                           "JOIN teachers t ON tl.teacher_id = t.id " +
                           "JOIN disciplines d ON tl.discipline_id = d.discipline_id " +
                           "ORDER BY t.full_name";
                
                using var cmd = new NpgsqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                
                var dt = new System.Data.DataTable();
                dt.Load(reader);
                dgvLoads.DataSource = dt;

                if (dgvLoads.Columns["load_id"] != null) dgvLoads.Columns["load_id"].Visible = false;
                if (dgvLoads.Columns["full_name"] != null) dgvLoads.Columns["full_name"].HeaderText = "ФИО";
                if (dgvLoads.Columns["discipline_name"] != null) dgvLoads.Columns["discipline_name"].HeaderText = "Дисциплина";
                if (dgvLoads.Columns["hours_count"] != null) dgvLoads.Columns["hours_count"].HeaderText = "Часы";
                if (dgvLoads.Columns["semester_type"] != null) dgvLoads.Columns["semester_type"].HeaderText = "Семестр";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void LoadTeachers()
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = "SELECT id, full_name FROM teachers ORDER BY full_name";
                using var cmd = new NpgsqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                
                var teachers = new List<ComboBoxItem>();
                while (reader.Read())
                {
                    teachers.Add(new ComboBoxItem 
                    { 
                        Id = reader.GetInt32(0), 
                        Name = reader.GetString(1) 
                    });
                }
                
                cboTeacher.DataSource = teachers;
                cboTeacher.DisplayMember = "Name";
                cboTeacher.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки преподавателей: " + ex.Message);
            }
        }

        private void LoadDisciplines()
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = "SELECT discipline_id, discipline_name FROM disciplines ORDER BY discipline_name";
                using var cmd = new NpgsqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                
                var disciplines = new List<ComboBoxItem>();
                while (reader.Read())
                {
                    disciplines.Add(new ComboBoxItem 
                    { 
                        Id = reader.GetInt32(0), 
                        Name = reader.GetString(1) 
                    });
                }
                
                cboDiscipline.DataSource = disciplines;
                cboDiscipline.DisplayMember = "Name";
                cboDiscipline.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки дисциплин: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cboTeacher.SelectedValue == null || cboDiscipline.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, выберите преподавателя и дисциплину.");
                return;
            }

            if (numHours.Value <= 0)
            {
                MessageBox.Show("Количество часов должно быть больше нуля.");
                return;
            }

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = "INSERT INTO teaching_load (teacher_id, discipline_id, hours_count, semester_type) " +
                           "VALUES (@teacherId, @disciplineId, @hours, @semester)";
                
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("teacherId", (int)cboTeacher.SelectedValue);
                cmd.Parameters.AddWithValue("disciplineId", (int)cboDiscipline.SelectedValue);
                cmd.Parameters.AddWithValue("hours", (int)numHours.Value);
                cmd.Parameters.AddWithValue("semester", cboSemester.SelectedItem.ToString());
                
                cmd.ExecuteNonQuery();
                MessageBox.Show("Нагрузка успешно сохранена.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLoads.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.");
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить выбранную нагрузку?",
                                     "Подтверждение удаления",
                                     MessageBoxButtons.YesNo);
            if (confirmResult != DialogResult.Yes) return;

            try
            {
                int loadId = Convert.ToInt32(dgvLoads.SelectedRows[0].Cells["load_id"].Value);
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                string sql = "DELETE FROM teaching_load WHERE load_id = @id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id", loadId);
                cmd.ExecuteNonQuery();
                
                MessageBox.Show("Запись удалена.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }


        private class ComboBoxItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
