using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineExaminationSystem.Extensions;
using OES.Model.Users;
using OES.Model.Examination;
using OES.Data;
using OES.Modules.Instructor;

namespace OnlineExaminationSystem.Areas.StudentArea.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentBaseController : Controller
    {
        private Student _Student;
        public Student Student
        {
            get
            {
                _Student = _Student ?? (User.GetDbUser() as Student);
                return _Student;
            }
        }

        public List<Registration> Registrations
        {
            get
            {
                OESData db = new OESData();
                var studentRegistrations = db.Students.Include(s=> s.Registrations)
                    .Include(s => s.Registrations.Select(sr => sr.Registration.Semester))
                    .Include(s => s.Registrations.Select(sr => sr.Registration.Course))
                    .Include(s => s.Registrations.Select(sr => sr.Registration.Instructor))
                    .FirstOrDefault(s => s.UserId.Equals(Student.UserId, StringComparison.OrdinalIgnoreCase)).Registrations;
                    
                List<Registration> regs = new List<Registration>();
                foreach (var reg in studentRegistrations)
                {
                    regs.Add(reg.Registration);
                }
                return regs;
            }
        }


        private ExamModule _ExamModule;
        public ExamModule ExamModule
        {
            get
            {
                _ExamModule = _ExamModule ?? new ExamModule();
                return _ExamModule;
            }
        }

    }
}