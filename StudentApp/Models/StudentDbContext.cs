using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext() : base("StudentDB")
        { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSubject> StudentSubjectWithMarks { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; } 


    }
}