using Lab05.DAL.Connect;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab05.BUS
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            Model1 context = new Model1();  
            return context.Student.ToList();   
        }
        public List<Student> GetAllHasNoMajor()
        {
            Model1 context = new Model1();
            return context.Student.Where(p=>p.MajorID == null).ToList();
        }
        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            Model1 context = new Model1();
            return context.Student.Where(p => p.MajorID == null && p.FacultyID == facultyID).ToList();
        }
        public Student FinById(string studentId)
        {
            Model1 context = new Model1();
            return context.Student.FirstOrDefault(p => p.StudentID == studentId);
        }
        public void InsertUpdate(Student student)
        {
            Model1 context = new Model1();
            context.Student.AddOrUpdate(student);
            context.SaveChanges();
        }
        public void Delete(string studentId)
        {
            Model1 context = new Model1();
            var student = context.Student.FirstOrDefault(p => p.StudentID == studentId);
            if (student != null)
            {
                context.Student.Remove(student);
                context.SaveChanges();
            }
        }
    }
}
