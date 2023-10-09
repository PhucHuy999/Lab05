using Lab05.BUS;
using Lab05.DAL.Connect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05.GUI
{
    public partial class frmStudent : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();

        public frmStudent()
        {
            InitializeComponent();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
                //// Xóa hình ảnh trong điều khiển picAvatar khi biểu mẫu được nạp
                //picAvatar.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            
        }



        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }
        //Hàm binding gridView từ list sinh viên 
        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name + "";
                ShowAvatar(item.Avatar);
            }
        }
        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                picAvatar.Image = null;
            }
            else
            {
                string parentDirectory =Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images",ImageName);
                picAvatar.Image = Image.FromFile(imagePath);
                picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
                picAvatar.Refresh();
            }
        }


        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle =
           DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.checkBox1.Checked)
                listStudents = studentService.GetAllHasNoMajor();
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
            //// Xóa hình ảnh trong điều khiển picAvatar khi biểu mẫu được nạp
            //picAvatar.Image = null;

        }

        private void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ giao diện và tạo một đối tượng Student
            Student student = new Student
            {
                StudentID = txtStudentID.Text,
                FullName = txtFullName.Text,
                FacultyID = cmbFaculty.SelectedIndex,

            };

            // Thử chuyển đổi giá trị từ chuỗi thành kiểu double
            if (double.TryParse(txtAvgScore.Text, out double avgScore))
            {
                student.AverageScore = avgScore;
            }
            else
            {
                // Xử lý trường hợp không thể chuyển đổi thành công (thích hợp với thông báo lỗi)
                MessageBox.Show("Vui lòng nhập điểm số hợp lệ.");
                return; // Không thực hiện thêm hoặc cập nhật nếu không thể chuyển đổi thành công
            }

            //////////////////////
            // Lưu hình ảnh và cập nhật tên hình ảnh trong cơ sở dữ liệu
            if (picAvatar.Image != null)
            {
                string imageExtension = GetImageExtension(picAvatar.Image);
                string imageFileName = $"{student.StudentID}.{imageExtension}";

                // Lưu hình ảnh vào thư mục ứng dụng
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images", imageFileName);

                // Lưu hình ảnh
                picAvatar.Image.Save(imagePath);

                // Cập nhật tên hình ảnh vào thông tin sinh viên
                student.Avatar = imageFileName;
            }
            //////////////////



            // Gọi phương thức InsertUpdate để thêm hoặc cập nhật Student
            studentService.InsertUpdate(student);

            // Sau khi thêm hoặc cập nhật thành công, gọi lại BindGrid để cập nhật DataGridView
            var listStudents = studentService.GetAll();
            BindGrid(listStudents);


        }
        private string GetImageExtension(Image image)
        {
            if (ImageFormat.Jpeg.Equals(image.RawFormat))
            {
                return "jpg";
            }
            else if (ImageFormat.Png.Equals(image.RawFormat))
            {
                return "png";
            }
            else if (ImageFormat.Gif.Equals(image.RawFormat))
            {
                return "gif";
            }
            else
            {
                return "jpg"; // Mặc định là jpg nếu không xác định được định dạng
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudent.SelectedRows.Count > 0)
            {
                // Hiển thị hộp thoại xác nhận trước khi xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string studentId = dgvStudent.SelectedRows[0].Cells[0].Value.ToString();
                    studentService.Delete(studentId);

                    // Sau khi xóa thành công, cập nhật DataGridView
                    var listStudents = studentService.GetAll();
                    BindGrid(listStudents);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Chọn ảnh";
                openFileDialog.Filter = "Tất cả các tệp|*.*|Ảnh|*.jpg;*.jpeg;*.png;*.gif";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName;
                    picAvatar.Image = Image.FromFile(imagePath);
                    picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }

        }

        
    }
}


//if (e.RowIndex >= 0)
//{
//    DataGridViewRow row = dgvStudent.Rows[e.RowIndex];

//    Lấy thông tin từ DataGridView và hiển thị lên các TextBox
//    txtStudentID.Text = row.Cells[0].Value.ToString();
//    txtFullName.Text = row.Cells[1].Value.ToString();
//    cmbFaculty.Text = row.Cells[2].ToString();
//    txtAvgScore.Text = row.Cells[3].Value.ToString();

//    Hiển thị hình ảnh sinh viên lên PictureBox(picAvatar)
//    string imageName = row.Cells[0].Value.ToString() + ".jpg";
//    ShowAvatar(imageName);
//}