using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private SchoolContext db = new SchoolContext(); 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(string sortOrder)
        {
            ViewBag.EnrolmentSortParam = (String.IsNullOrEmpty(sortOrder)) ? "enrollment_desc" : "";
            ViewBag.CountSortParam = sortOrder == "count" ? "count_desc" : "count";

            IQueryable<EnrollmentDateGroup> data = from student in db.Students
                                                    group student by student.EnrollmentDate into dateGroup
                                                    select new EnrollmentDateGroup()
                                                    {
                                                        EnrollmentDate = dateGroup.Key,
                                                        StudentCount = dateGroup.Count()

                                                    };
            switch(sortOrder)
            {
                case "count":
                    data = data.OrderBy(m => m.StudentCount);
                    break;
                case "count_desc":
                    data = data.OrderByDescending(m => m.StudentCount);
                    break;
                case "enrollment_desc":
                    data = data.OrderByDescending(m => m.EnrollmentDate);
                    break;
                default:
                    data = data.OrderBy(m => m.EnrollmentDate);
                    break;
            }

            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}