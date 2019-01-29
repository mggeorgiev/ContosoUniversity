using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Enrollments
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GradeSortParam = sortOrder == "grade" ? "grade_desc" : "grade";
            ViewBag.TitleSortParam = sortOrder == "title" ? "date_desc" : "title";

            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentSearchString = searchString;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);

            if (!String.IsNullOrEmpty(searchString))
            {
                enrollments = enrollments.Where(e => e.Course.Title.Contains(searchString) ||
                                            e.Student.LastName.Contains(searchString) ||
                                            e.Student.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title":
                    enrollments = enrollments.OrderBy(e => e.Course.Title);
                    break;
                case "title_desc":
                    enrollments = enrollments.OrderByDescending(е => е.Course.Title);
                    break;
                case "grade":
                    enrollments = enrollments.OrderBy(s => s.Grade);
                    break;
                case "grade_desc":
                    enrollments = enrollments.OrderByDescending(s => s.Grade);
                    break;
                case "name_desc":
                    enrollments = enrollments.OrderByDescending(s => s.Student.LastName);
                    break;
                default:
                    enrollments = enrollments.OrderBy(s => s.Student.LastName);
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(enrollments.ToPagedList(pageNumber, pageSize));
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students.OrderBy(s => s.LastName), "ID", "LastName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses.OrderBy(x => x.Title), "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students.OrderBy(s => s.LastName), "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses.OrderBy(x => x.Title), "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students.OrderBy(s => s.LastName), "ID", "Fullname", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses.OrderBy(x => x.Title), "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students.OrderBy(s => s.LastName), "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
