﻿using Lab05.DAL.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class MajorService
    {
        public List<Major> GetAllByFaculty(int facultyID)
        {
            Model1 context = new Model1();
            return context.Major.Where(p=>p.FacultyID == facultyID).ToList();

        }
    }
}
