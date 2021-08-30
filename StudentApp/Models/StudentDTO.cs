using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class StudentDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public int StudentClassID { get; set; }
        public string ClassName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }        
        public int Marks { get; set; }
        public int StudentSubjectMarksID { get; set; }

    }
}