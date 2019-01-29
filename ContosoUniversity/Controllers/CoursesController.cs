﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Courses
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.TitleSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.NumberSortparam = sortOrder == "courseid" ? "courseid_desc" : "courseid";
            ViewBag.CreditsSortParam = sortOrder == "credits" ? "credits_desc" : "credits";
            ViewBag.DepartmentSortParam = sortOrder == "department" ? "department_desc" : "department";

            var courses = db.Courses.Include(c => c.Department);

            if (searchString != null)
            {
                page = 1;
            }
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;



            switch (sortOrder)
            {
                case "courseid":
                    courses = courses.OrderBy(c => c.CourseID);
                    break;
                case "courseid_desc":
                    courses = courses.OrderByDescending(c => c.CourseID);
                    break;
                case "credits":
                    courses = courses.OrderBy(c => c.Credits);
                    break;
                case "credits_desc":
                    courses = courses.OrderByDescending(c => c.Credits);
                    break;
                case "department":
                    courses = courses.OrderBy(c => c.DepartmentID);
                    break;
                case "department_desc":
                    courses = courses.OrderByDescending(c => c.DepartmentID);
                    break;
                case "title_desc":
                    courses = courses.OrderByDescending(c => c.Title);
                    break;
                default:
                    courses = courses.OrderBy(c => c.Title);
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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