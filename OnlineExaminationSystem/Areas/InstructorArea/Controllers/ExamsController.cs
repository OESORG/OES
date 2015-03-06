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
        public ActionResult Index(string id)
        {
            var model = new ExamViewModel();
            model.SelectedRegistration = ExamModule.GetRegistrationForExams(id);
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
        public ActionResult Create([Bind(Include = "StartDate,EndDate,NumberOfHighQuestion,HighQuestionScore,NumberOfMediumQuestion,MediumQuestionScore,NumberOfLowQuestion,LowQuestionScore,RegistrationId")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(exam);
                db.SaveChanges();
                return RedirectToAction("Index", new {id = exam.RegistrationId });
            }

            return View(exam);
        }

        // GET: InstructorArea/Exams/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegistrationId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", exam.RegistrationId);
            return View(exam);
        }

        // POST: InstructorArea/Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamId,StartDate,EndDate,NumberOfHighQuestion,HighQuestionScore,NumberOfMediumQuestion,MediumQuestionScore,NumberOfLowQuestion,LowQuestionScore,RegistrationId")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = exam.RegistrationId });
            }
            ViewBag.RegistrationId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", exam.RegistrationId);
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
