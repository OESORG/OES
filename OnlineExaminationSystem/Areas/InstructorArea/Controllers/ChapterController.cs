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
    public class ChapterController : Controller
    {
        private OESData db = new OESData();

        // GET: InstructorArea/Chapter
        public ActionResult Index(string id = null)
        {
            IEnumerable<Chapter> chapters = null;
            if (string.IsNullOrWhiteSpace(id))
            {
                chapters = db.Chapters.Include(c => c.Registration);
            }
            else
            {
                chapters = db.Chapters.Include(c => c.Registration).Where(c => c.RegistrationId.Equals(id, StringComparison.OrdinalIgnoreCase));
            }

            return View(chapters.ToList());
        }

        // GET: InstructorArea/Chapter/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // GET: InstructorArea/Chapter/Create
        public ActionResult Create()
        {
            ViewBag.RegistrationId = new SelectList(db.Registrations, "RegistrationId", "SemesterId");
            return View();
        }

        // POST: InstructorArea/Chapter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChapterId,Title,RegistrationId")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegistrationId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", chapter.RegistrationId);
            return View(chapter);
        }

        // GET: InstructorArea/Chapter/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegistrationId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", chapter.RegistrationId);
            return View(chapter);
        }

        // POST: InstructorArea/Chapter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChapterId,Title,RegistrationId")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegistrationId = new SelectList(db.Registrations, "RegistrationId", "SemesterId", chapter.RegistrationId);
            return View(chapter);
        }

        // GET: InstructorArea/Chapter/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: InstructorArea/Chapter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Chapter chapter = db.Chapters.Find(id);
            db.Chapters.Remove(chapter);
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
