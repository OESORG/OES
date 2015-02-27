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

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    public class QuestionsController : InstructorBaseController
    {
        public QuestionsController()
        {
            ViewBag.Page = "questions";
        }
        private OESData db = new OESData();

        // GET: InstructorArea/Questions
        public ActionResult Index(string id)
        {
            ExamModule module = new ExamModule();
            var chapter = module.GetChapterById(id);
            QuestionViewModel model = new QuestionViewModel()
            {
                Registrations = Registrations,
                SelectedChapter = chapter,
                SelectedRegistration = (chapter != null) ? chapter.Registration : null

            };
            return View(model);
        }

        // GET: InstructorArea/Questions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: InstructorArea/Questions/Create
        public ActionResult Create(string id)
        {
            return View(new Question() { ChapterId = id });
        }

        // POST: InstructorArea/Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionText,ChapterId,Type,Difficulty")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = question.ChapterId });
            }

            return View(question);
        }

        // GET: InstructorArea/Questions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: InstructorArea/Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionText,ChapterId,Type,Difficulty")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = question.ChapterId });
            }
            return View(question);
        }

        // GET: InstructorArea/Questions/Delete/5
        public ActionResult Delete(string id)
        {
            Question question = db.Questions.Find(id);
            string chapterId = question.ChapterId;
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = chapterId });
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
