using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace StudentsDiary1
{
    public partial class Main : Form
    {

        private FileHelper<List<Student>> _fileHelper = 
            new FileHelper<List<Student>>(Program.FilePath);

        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeader();
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            dvgDiary.DataSource = students;
        }

        private void SetColumnsHeader()
        {
            dvgDiary.Columns[0].HeaderText = "Numer";
            dvgDiary.Columns[1].HeaderText = "Imię";
            dvgDiary.Columns[2].HeaderText = "Nazwisko";
            dvgDiary.Columns[3].HeaderText = "Uwagi";
            dvgDiary.Columns[4].HeaderText = "Matematyka";
            dvgDiary.Columns[5].HeaderText = "Technologia";
            dvgDiary.Columns[6].HeaderText = "Fizyka";
            dvgDiary.Columns[7].HeaderText = "Język Polski";
            dvgDiary.Columns[8].HeaderText = "Język Obcy";
            dvgDiary.Columns[9].HeaderText = "Zajęcia Dod.";
            dvgDiary.Columns[10].HeaderText = "Klasa";
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dvgDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prosze zaznacz ucznia którego dane chcesz edytować");
                return;
            }

            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dvgDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dvgDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prosze zaznacz ucznia którego dane chcesz usunąć");
                return;
            }

            var selectedStudent = dvgDiary.SelectedRows[0];

            var confirmDelete = 
                MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}",
                "Usuwanie ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void cbmGroupM_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cbmGroupM.SelectedItem.ToString() == "Wszyscy")
            {
                RefreshDiary();
            }
            else
            {
                var index = Convert.ToInt32(cbmGroupM.SelectedItem);
            
                var students = _fileHelper.DeserializeFromFile();
                var _students = students.Where(x => x.GroupId == index);
                dvgDiary.DataSource = _students;
            }
        }


    }
}
