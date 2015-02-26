﻿using System;
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
    public class ChaptersController : InstructorBaseController
    {
        private OESData db = new OESData();

        public ChaptersController()
        {
            ViewBag.Page = "chapters";
        }
        // GET: InstructorArea/Chapters
        public ActionResult Index(string id = null)
        {
            ChapterViewModel model = new ChapterViewModel();
            model.Regisatrations = Registrations;
            if (!string.IsNullOrWhiteSpace(id))
            {
                model.SelectedRegisteration = Registrations.FirstOrDefault(r => r.RegistrationId.Equals(id, StringComparison.OrdinalIgnoreCase));
            }
            return View(model);
        }

        // GET: InstructorArea/Chapters/Details/5
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

        // GET: InstructorArea/Chapters/Create
        public ActionResult Create(string id)
        {
            return View(new Chapter() { RegistrationId = id });
        }

        // POST: InstructorArea/Chapters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Overview,RegistrationId")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = chapter.RegistrationId });
            }

            return View(chapter);
        }

        // GET: InstructorArea/Chapters/Edit/5
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

        // POST: InstructorArea/Chapters/Edit/5
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

        // GET: InstructorArea/Chapters/Delete/5
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

        // POST: InstructorArea/Chapters/Delete/5
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