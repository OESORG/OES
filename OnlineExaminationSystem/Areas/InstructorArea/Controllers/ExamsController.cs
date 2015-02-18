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

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    public class ExamsController : Controller
    {
        private OESData db = new OESData();

        // GET: InstructorArea/Exams
        public ActionResult Index(string id = null)
        {
            IEnumerable<Exam> exams = null;
            if (id != null)
            {
                exams = db.Exams.Where(e => e.Course.RegistrationId.Equals(id, StringComparison.OrdinalIgnoreCase)).Include(e => e.Course);
            }
            else { exams = db.Exams.Include(e => e.Course); }
            return View(exams.ToList());
        }

        // GET: InstructorArea/Exams/Details/5
        public ActionResult Details(string id)
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
            return View(exam);
        }

        // GET: InstructorArea/Exams/Create
        public ActionResult Create()
        {
            ViewBag.CourseStudentId = new SelectList(db.Registrations, "RegistrationId", "SemesterId");
            return View();
        }

        // POST: InstructorArea/Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExamId,StartDate,EndDate,CourseStudentId")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(exam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseStudentId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", exam.CourseStudentId);
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
            ViewBag.CourseStudentId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", exam.CourseStudentId);
            return View(exam);
        }

        // POST: InstructorArea/Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamId,StartDate,EndDate,CourseStudentId")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseStudentId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", exam.CourseStudentId);
            return View(exam);
        }

        // GET: InstructorArea/Exams/Delete/5
        public ActionResult Delete(string id)
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
            return View(exam);
        }

        // POST: InstructorArea/Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Exam exam = db.Exams.Find(id);
            db.Exams.Remove(exam);
            db.SaveChanges();
            return RedirectToAction("Index");
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
