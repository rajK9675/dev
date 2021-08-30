using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int StudentClassID { get; set; }
        public StudentClass StudentClass { get; set; }
        public List<StudentSubject> StudentSubject { get; set; }

    } 
}