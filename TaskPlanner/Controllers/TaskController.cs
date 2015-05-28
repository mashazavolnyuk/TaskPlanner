using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskPlanner.Models;

namespace TaskPlanner.Controllers
{
    public class TaskController : Controller
    {
        private TaskPlannerDbEntities db = new TaskPlannerDbEntities();
        private Task newTask = new Task();
        private Subtask sb = new Subtask();

        public ActionResult Index()
        {
            return View(db.Task.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(from c in db.Category select new {Id = c.Id, Name = c.Name}, "Id",
                "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(TaskInputModel model)
        {

            newTask.CategoryId = model.CategoryId;

            newTask.date = model.Date;
            newTask.text = model.Text;

            foreach (var st in model.SubTasks)
            {

                sb.text = st.Text;
                newTask.Subtask.Add(sb);
            }

            db.Task.Add(newTask);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Delete()
        {
            return View("Index");

        }

        [HttpPost]
        public ActionResult Delete(Task model)
        {

            newTask.isFinish = model.isFinish;
            if (newTask.isFinish)
            {
                db.Task.Remove(newTask);
                db.SaveChanges();
            }
            return RedirectToAction("Index");

        }


        [HttpPost]
        public ActionResult UpdateTaskState(int taskId)
        {
            Task task = db.Task.Find(taskId);
            if (task != null)
            {
                task.isFinish = !task.isFinish;
                if (task.isFinish)
                {
                    foreach (Subtask subtask in task.Subtask)
                    {
                        subtask.isFinish = true;
                    }
                }
                db.SaveChanges();
                var subtasks = task.Subtask.Select(x => new {x.Id, x.isFinish}).ToList();
                return Json(new {task.Id, task.isFinish, subtasks}, JsonRequestBehavior.AllowGet);
            }

            return Json("fail");
        }


        [HttpPost]
        public ActionResult UpdateSubTaskState(int subTaskId)
        {
            Subtask subtask = db.Subtask.Find(subTaskId);
            if (subtask != null)
            {
                subtask.isFinish = !subtask.isFinish;

                Task parentTask = subtask.Task;
                if (parentTask.Subtask.Count(x => x.isFinish) == parentTask.Subtask.Count)
                {
                    parentTask.isFinish = true;
                }
                else
                {
                    parentTask.isFinish = false;
                }

                db.SaveChanges();
                var subtasks = parentTask.Subtask.Select(x => new {x.Id, x.isFinish}).ToList();
                return Json(new {parentTask.Id, parentTask.isFinish, subtasks}, JsonRequestBehavior.AllowGet);
            }

            return Json("fail");
        }



        [HttpPost]
        public ActionResult CreateSubTask(int taskId, string text)
        {
            Task parent = db.Task.Find(taskId);
            if (parent != null)
            {
                var newSubtask = new Subtask() {text = text};
                parent.Subtask.Add(newSubtask);
                db.SaveChanges();
                return PartialView("SubTaskView", newSubtask);
            }

            return null;
        }
    }
}
