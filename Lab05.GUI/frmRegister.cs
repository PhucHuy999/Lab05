using Lab05.BUS;
using Lab05.DAL.Connect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05.GUI
{
    public partial class frmRegister : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();
        public frmRegister()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            try
            {
                var listFacultys = facultyService.GetAll();
                FillFalcultyCombobox(listFacultys);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        //Hàm binding list dữ liệu khoa vào combobox có tên hiện thị là tên khoa, giá trị là Mã khoa
        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void cmbFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Faculty selectedFaculty = cmbFaculty.SelectedItem as Faculty;
            //if (selectedFaculty != null)
            //{
            //    var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID);
            //    FillMajorCombobox(listMajor);
            //    var listStudents = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
            //    BindGrid(listStudents);

            //}
        }
        private void BindGrid(List<Student> listStudent)
        {
            //dgvStudent.Rows.Clear();
            //foreach (var item in listStudent)
            //{
            //    int index = dgvStudent.Rows.Add();
            //    dgvStudent.Rows[index].Cells[1].Value = item.StudentID;
            //    dgvStudent.Rows[index].Cells[2].Value = item.FullName;
            //    if (item.Faculty != null)
            //        dgvStudent.Rows[index].Cells[3].Value = item.Faculty.FacultyName;
            //        dgvStudent.Rows[index].Cells[4].Value = item.AverageScore + "";
            //    if (item.MajorID != null)
            //        dgvStudent.Rows[index].Cells[5].Value = item.Major.Name + "";
            //}

        }
    }
}
