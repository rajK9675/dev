using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class StudentSubject
    {
        public int ID { get; set; }
        [Required]
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }
        [Required]
        public int StudentID { get; set; }
        public Student Student { get; set; }
        [Required]
        public int Marks { get; set; }
    }
}