using Newtonsoft.Json;
using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetStudent(string filter, string FilterBy)
        {
            StudentDbContext db = new StudentDbContext();
            try
            {
                List<StudentDTO> studentWithMarks = new List<StudentDTO>();
                studentWithMarks = db.Students
                           .Include(x => x.StudentClass)
                           .Select(x => new StudentDTO
                           {
                               FirstName = x.FirstName,
                               ID = x.ID,
                               LastName = x.LastName,
                               ClassName = x.StudentClass.ClassName
                           })
                           .ToList();
                if (!string.IsNullOrEmpty(filter))
                {
                    switch (FilterBy)
                    {
                        case "FirstName":
                            studentWithMarks = studentWithMarks.Where(x => x.FirstName.Contains(filter)).ToList();
                            break;
                        case "LastName":
                            studentWithMarks = studentWithMarks.Where(x => x.LastName.Contains(filter)).ToList();
                            break;
                        case "ClassName":
                            studentWithMarks = studentWithMarks.Where(x => x.ClassName.Contains(filter)).ToList();
                            break;
                        default:
                            studentWithMarks = studentWithMarks;
                            break;
                    }
                }
                return Json(new { Result = "OK", Records = studentWithMarks }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Create(StudentDTO Model)
        {
            StudentDbContext db = new StudentDbContext();
            try
            {
               
                StudentSubject studentSubject = new StudentSubject()
                {
                    Marks = Model.Marks,
                    SubjectID = Model.SubjectID
                };
                Student student = new Student()
                {
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    StudentClassID = Model.StudentClassID,

                };
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Enter Valid Value" });
                }
                student.StudentSubject = new List<StudentSubject>();
                student.StudentSubject.Add(studentSubject);
                db.Students.Add(student);
                db.SaveChanges();
                var studentWithMarks = db.Students
                             .Include(x => x.StudentClass).Where(x => x.ID == student.ID)
                             .Select(x => new StudentDTO
                             {
                                 FirstName = x.FirstName,
                                 ID = x.ID,
                                 LastName = x.LastName,
                                 ClassName = x.StudentClass.ClassName
                             })
                             .FirstOrDefault();
                return Json(new { Result = "OK", Record = studentWithMarks }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Edit(Student Model)
        {
            StudentDbContext db = new StudentDbContext();
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Enter Valid Value" });
                }
                db.Entry(Model).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Delete(String ID)
        {
            StudentDbContext db = new StudentDbContext();
            try
            {
                Student marks = db.Students.Find(Convert.ToInt32(ID));
                db.Students.Remove(marks);
                db.SaveChanges();
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GetStudentClass()
        {
            try
            {
                StudentDbContext db = new StudentDbContext();
                var classList = db.StudentClasses
                                   .Select(c => new { DisplayText = c.ClassName, Value = c.ID }).OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = classList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GetSubject()
        {
            try
            {
                StudentDbContext db = new StudentDbContext();
                var classList = db.Subjects
                                   .Select(c => new { DisplayText = c.SubjectName, Value = c.ID })
                                   .OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = classList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult AddMarks(StudentDTO Model)
        {
            StudentDbContext db = new StudentDbContext();
            try
            {

                var isExist = db.StudentSubjectWithMarks.Any(x => x.SubjectID == Model.SubjectID && x.StudentID == Model.ID);
                if (isExist)
                {
                    return Json(new { Result = "ERROR", Message = "Subject is Allrady in database." });
                }
                StudentSubject studentSubject = new StudentSubject()
                {
                    Marks = Model.Marks,
                    SubjectID = Model.SubjectID,
                    StudentID = Model.ID
                };
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Enter Valid Value" });
                }
                db.StudentSubjectWithMarks.Add(studentSubject);
                db.SaveChanges();
                var studentWithMarks = db.StudentSubjectWithMarks
                             .Include(x => x.Student).Include(x => x.Subject).Where(x => x.ID == studentSubject.ID).ToList()
                             .Select(x => new StudentDTO
                             {
                                 ID = x.ID,
                                 SubjectName = x.Subject.SubjectName,
                                 Marks = x.Marks
                             })
                             .FirstOrDefault();
                return Json(new { Result = "OK", Record = studentWithMarks }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GetStudentmarks(int id, string filter, string FilterBy)
        {
            try
            {
                StudentDbContext db = new StudentDbContext();
                var markList = db.StudentSubjectWithMarks
                                   .Where(x => x.StudentID == id).Include(x => x.Subject)
                                   .Select(x => new StudentDTO { Marks = x.Marks, ID = x.StudentID, StudentSubjectMarksID = x.ID, SubjectName = x.Subject.SubjectName }).ToList();

                if (!string.IsNullOrEmpty(filter))
                {
                    switch (FilterBy)
                    {
                        case "Marks":
                            markList = markList.Where(x => x.Marks == Convert.ToInt32(filter)).ToList();
                            break;
                        case "SubjectName":
                            markList = markList.Where(x => x.SubjectName.Contains(filter)).ToList();
                            break;

                        default:
                            markList = markList;
                            break;
                    }
                }

                return Json(new { Result = "OK", Records = markList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult EditMarks(StudentDTO Model)
        {
            StudentDbContext db = new StudentDbContext();
            try
            {

                StudentSubject studentSubject = new StudentSubject()
                {
                    Marks = Model.Marks,
                    SubjectID = Model.SubjectID,
                    StudentID = Model.ID,
                    ID = Model.StudentSubjectMarksID
                };
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Enter Valid Value" });
                }
                db.Entry(studentSubject).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult DeleteMarks(String StudentSubjectMarksID)
        {
            StudentDbContext db = new StudentDbContext();
            try
            {
                int ID = Convert.ToInt32(StudentSubjectMarksID);
                StudentSubject marks = db.StudentSubjectWithMarks.Find(ID);
                db.StudentSubjectWithMarks.Remove(marks);
                db.SaveChanges();
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}