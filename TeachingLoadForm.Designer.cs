namespace TeacherAccounting
{
    partial class TeachingLoadForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvLoads = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpNewLoad = new System.Windows.Forms.GroupBox();
            this.lblTeacher = new System.Windows.Forms.Label();
            this.cboTeacher = new System.Windows.Forms.ComboBox();
            this.lblDiscipline = new System.Windows.Forms.Label();
            this.cboDiscipline = new System.Windows.Forms.ComboBox();
            this.lblHours = new System.Windows.Forms.Label();
            this.numHours = new System.Windows.Forms.NumericUpDown();
            this.lblSemester = new System.Windows.Forms.Label();
            this.cboSemester = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoads)).BeginInit();
            this.grpNewLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHours)).BeginInit();
            this.SuspendLayout();

            this.dgvLoads.AllowUserToAddRows = false;
            this.dgvLoads.AllowUserToDeleteRows = false;
            this.dgvLoads.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLoads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoads.Location = new System.Drawing.Point(12, 12);
            this.dgvLoads.MultiSelect = false;
            this.dgvLoads.Name = "dgvLoads";
            this.dgvLoads.ReadOnly = true;
            this.dgvLoads.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLoads.Size = new System.Drawing.Size(760, 300);
            this.dgvLoads.TabIndex = 0;

            this.btnDelete.Location = new System.Drawing.Point(672, 318);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.grpNewLoad.Controls.Add(this.lblTeacher);
            this.grpNewLoad.Controls.Add(this.cboTeacher);
            this.grpNewLoad.Controls.Add(this.lblDiscipline);
            this.grpNewLoad.Controls.Add(this.cboDiscipline);
            this.grpNewLoad.Controls.Add(this.lblHours);
            this.grpNewLoad.Controls.Add(this.numHours);
            this.grpNewLoad.Controls.Add(this.lblSemester);
            this.grpNewLoad.Controls.Add(this.cboSemester);
            this.grpNewLoad.Controls.Add(this.btnSave);
            this.grpNewLoad.Location = new System.Drawing.Point(12, 350);
            this.grpNewLoad.Name = "grpNewLoad";
            this.grpNewLoad.Size = new System.Drawing.Size(760, 150);
            this.grpNewLoad.TabIndex = 2;
            this.grpNewLoad.TabStop = false;
            this.grpNewLoad.Text = "Назначить новую нагрузку";

            this.lblTeacher.AutoSize = true;
            this.lblTeacher.Location = new System.Drawing.Point(20, 25);
            this.lblTeacher.Name = "lblTeacher";
            this.lblTeacher.Size = new System.Drawing.Size(89, 13);
            this.lblTeacher.TabIndex = 0;
            this.lblTeacher.Text = "Преподаватель:";

            this.cboTeacher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTeacher.FormattingEnabled = true;
            this.cboTeacher.Location = new System.Drawing.Point(23, 41);
            this.cboTeacher.Name = "cboTeacher";
            this.cboTeacher.Size = new System.Drawing.Size(300, 21);
            this.cboTeacher.TabIndex = 1;

            this.lblDiscipline.AutoSize = true;
            this.lblDiscipline.Location = new System.Drawing.Point(20, 75);
            this.lblDiscipline.Name = "lblDiscipline";
            this.lblDiscipline.Size = new System.Drawing.Size(73, 13);
            this.lblDiscipline.TabIndex = 2;
            this.lblDiscipline.Text = "Дисциплина:";

            this.cboDiscipline.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiscipline.FormattingEnabled = true;
            this.cboDiscipline.Location = new System.Drawing.Point(23, 91);
            this.cboDiscipline.Name = "cboDiscipline";
            this.cboDiscipline.Size = new System.Drawing.Size(300, 21);
            this.cboDiscipline.TabIndex = 3;

            this.lblHours.AutoSize = true;
            this.lblHours.Location = new System.Drawing.Point(350, 25);
            this.lblHours.Name = "lblHours";
            this.lblHours.Size = new System.Drawing.Size(100, 13);
            this.lblHours.TabIndex = 4;
            this.lblHours.Text = "Количество часов:";

            this.numHours.Location = new System.Drawing.Point(353, 41);
            this.numHours.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numHours.Name = "numHours";
            this.numHours.Size = new System.Drawing.Size(100, 20);
            this.numHours.TabIndex = 5;

            this.lblSemester.AutoSize = true;
            this.lblSemester.Location = new System.Drawing.Point(470, 25);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(54, 13);
            this.lblSemester.TabIndex = 6;
            this.lblSemester.Text = "Семестр:";

            this.cboSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSemester.FormattingEnabled = true;
            this.cboSemester.Items.AddRange(new object[] { "Осенний", "Весенний" });
            this.cboSemester.Location = new System.Drawing.Point(473, 41);
            this.cboSemester.Name = "cboSemester";
            this.cboSemester.Size = new System.Drawing.Size(180, 21);
            this.cboSemester.TabIndex = 7;

            this.btnSave.Location = new System.Drawing.Point(353, 91);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(300, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(this.grpNewLoad);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dgvLoads);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeachingLoadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Учебная нагрузка";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoads)).EndInit();
            this.grpNewLoad.ResumeLayout(false);
            this.grpNewLoad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHours)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvLoads;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox grpNewLoad;
        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.ComboBox cboTeacher;
        private System.Windows.Forms.Label lblDiscipline;
        private System.Windows.Forms.ComboBox cboDiscipline;
        private System.Windows.Forms.Label lblHours;
        private System.Windows.Forms.NumericUpDown numHours;
        private System.Windows.Forms.Label lblSemester;
        private System.Windows.Forms.ComboBox cboSemester;
        private System.Windows.Forms.Button btnSave;

    }
}
