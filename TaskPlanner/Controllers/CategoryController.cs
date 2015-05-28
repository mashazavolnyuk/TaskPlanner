using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskPlanner.Models;

namespace TaskPlanner.Controllers
{
    public class CategoryController : Controller
    {
        TaskPlannerDbEntities db=new TaskPlannerDbEntities();

        public ActionResult Index()
        {
            return View(db.Category.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            db.Category.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
