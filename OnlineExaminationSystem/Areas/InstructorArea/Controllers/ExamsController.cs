using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OES.Data;
using OES.Model.Examination;
using OnlineExaminationSystem.Areas.InstructorArea.Models;
using OES.Modules.Instructor;
using OES.Modules.Common;

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class ExamsController : InstructorBaseController
    {
        private OESData db = new OESData();

        public ExamsController()
        {
            ViewBag.Page = "exams";
        }

        // GET: InstructorArea/Exams
        public ActionResult Index(string id = null)
        {
            var model = new ExamViewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model.SelectedRegistration = ExamModule.GetRegistrationForExams(id);
            }
            model.Registrations = Registrations;
            return View(model);
        }

        public ActionResult Select(string id)
        {
            return RedirectToAction("Index", new { id = id });
        }

        // GET: InstructorArea/Exams/Create
        public ActionResult Create(string id)
        {

            return View(new Exam() { RegistrationId = id });
        }

        // POST: InstructorArea/Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                var result = ExamModule.AddExam(exam);
                if (result.Success)
                {
                    return RedirectToAction("Index", new { id = exam.RegistrationId });
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                }

            }

            return View(exam);
        }

        public ActionResult Delete(string id)
        {
            Exam exam = db.Exams.Find(id);
            string regId = exam.RegistrationId;
            db.Exams.Remove(exam);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = regId });
        }

        public ActionResult Generate(string id)
        {
            GenerateExamModule module = new GenerateExamModule();
            var result = module.GenerateExamVersion(id);
            if (result.Success)
            {
                return View("Exam", result.ReturnObject);
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Key, err.Message);
                }
                var model = new ExamViewModel();
                model.SelectedRegistration = ExamModule.GetRegistrationForExams(result.ReturnObject.RegistrationId);
                model.Registrations = Registrations;
                model.SelectedExamId = id;
                return View("Index", model);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
