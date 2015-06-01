using System.Linq;
using System.Web.Mvc;
using TaskPlanner.Models;

namespace TaskPlanner.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskPlannerDbEntities _db = new TaskPlannerDbEntities();

        public ActionResult Index()
        {
            return View(_db.Task.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(from c in _db.Category select new {c.Id, c.Name}, "Id",
                "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(TaskInputModel model)
        {
            var newTask = new Task
            {
                CategoryId = model.CategoryId,
                date = model.Date,
                text = model.Text
            };


            _db.Task.Add(newTask);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete()
        {
            var finishedTask = (from task in _db.Task where task.isFinish select task).ToList();

            for (var i = finishedTask.Count - 1; i >= 0; i--)
            {
                var task = finishedTask[i];
                if (task.isFinish)
                {
                    var finishedSubTask = task.Subtask.ToList();

                    for (var j = finishedSubTask.Count - 1; j >= 0; j--)
                    {
                        _db.Subtask.Remove(finishedSubTask[j]);
                    }
                    _db.Task.Remove(task);
                }
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateTaskState(int taskId)
        {
            var task = _db.Task.Find(taskId);
            if (task != null)
            {
                task.isFinish = !task.isFinish;
                if (task.isFinish)
                {
                    foreach (var subtask in task.Subtask)
                    {
                        subtask.isFinish = true;
                    }
                }
                _db.SaveChanges();
                var subtasks = task.Subtask.Select(x => new {x.Id, x.isFinish}).ToList();
                return Json(new {task.Id, task.isFinish, subtasks}, JsonRequestBehavior.AllowGet);
            }
            return Json("fail");
        }

        [HttpPost]
        public ActionResult UpdateSubTaskState(int subTaskId)
        {
            var subtask = _db.Subtask.Find(subTaskId);
            if (subtask != null)
            {
                subtask.isFinish = !subtask.isFinish;

                var parentTask = subtask.Task;
                parentTask.isFinish = parentTask.Subtask.Count(x => x.isFinish) == parentTask.Subtask.Count;

                _db.SaveChanges();
                var subtasks = parentTask.Subtask.Select(x => new {x.Id, x.isFinish}).ToList();
                return Json(new {parentTask.Id, parentTask.isFinish, subtasks}, JsonRequestBehavior.AllowGet);
            }

            return Json("fail");
        }

        [HttpPost]
        public ActionResult CreateSubTask(int taskId, string text)
        {
            var parent = _db.Task.Find(taskId);
            if (parent != null)
            {
                var newSubtask = new Subtask {text = text};
                parent.Subtask.Add(newSubtask);
                parent.isFinish = false;
                _db.SaveChanges();
                return PartialView("SubTaskView", newSubtask);
            }
            return null;
        }
    }
}