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

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    public class InstructorBaseController : Controller
    {
        private Instructor _Instructor;
        public Instructor Instructor
        {
            get
            {
                _Instructor = _Instructor ?? (User.GetDbUser() as Instructor);
                return _Instructor;
            }
        }

        public List<Registration> Registrations
        {
            get
            {
                OESData db = new OESData();
                var registerations = db.Registrations.Include(r => r.Semester)
                    .Include(r => r.Course)
                    .Include(r => r.Chapters)
                    .Where(r => r.InstructorId.Equals(Instructor.UserId, StringComparison.OrdinalIgnoreCase)).ToList();
                db.Dispose();
                return registerations;
            }
        }

    }
}