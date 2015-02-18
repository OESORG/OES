using OES.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineExaminationSystem.Extensions;

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class CourseController : Controller
    {
        private OESData db = new OESData();
        // GET: InstructorArea/Course
        public ActionResult Index()
        {
            var instructor = User.GetDbUser();
            var registerations = db.Registrations.Include(r => r.Semester).Include(r => r.Course).Where(r => r.InstructorId.Equals(instructor.UserId, StringComparison.OrdinalIgnoreCase)).ToList();
            return View(registerations);
        }
    }
}