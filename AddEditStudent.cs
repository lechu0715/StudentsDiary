using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary1
{
    public partial class AddEditStudent : Form
    {
        
        private int _studentId;
        private int _gruopId = 1;
        private Student _student;

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;
            GetStudentData();
            tbFirstName.Select();
        }

        private void AddGroups()
        {
            
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                {
                    throw new Exception("Brak użytkownika o podanym Id");
                }


                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            //var cbText = cbActivities.Checked ? "uczęszcza" : "nie uczęszcza";

            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            rtbComments.Text = _student.Comments;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPolishLang.Text = _student.PolishLang;
            tbPhysics.Text = _student.Physics;
            tbForeignLang.Text = _student.ForeignLang;
            cbActivities.Checked = _student.Activities;
            cmbGroup.SelectedItem = _student.GroupId;

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
            {
                AssignIdToNewStudent(students);
            }

            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddNewUserToList(List<Student> students)
        {
            //var cbText = cbActivities.Checked ? "uczęszcza" : "nie uczęszcza";
            //var cmbGroup = cmbGroup
            //
            //_gruopId = cmbGroup.SelectedIndex;

            var student = new Student
            {

                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                ForeignLang = tbForeignLang.Text,
                PolishLang = tbPolishLang.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                Activities = cbActivities.Checked,
                GroupId = Convert.ToInt32(cmbGroup.SelectedItem)
            };

            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId =
                students.OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ?
                1 : studentWithHighestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}
