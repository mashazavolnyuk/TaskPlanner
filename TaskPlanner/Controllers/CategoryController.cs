using System.Linq;
using System.Web.Mvc;
using TaskPlanner.Models;

namespace TaskPlanner.Controllers
{
    public class CategoryController : Controller
    {
        private readonly TaskPlannerDbEntities _db = new TaskPlannerDbEntities();

        public ActionResult Index()
        {
            return View(_db.Category.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            _db.Category.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}