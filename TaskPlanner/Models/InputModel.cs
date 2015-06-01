using System;
using System.Collections.Generic;

namespace TaskPlanner.Models
{
    public class TaskInputModel
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public List<SubTaskInputModel> SubTasks { get; set; }
    }

    public class SubTaskInputModel
    {
        public int idTask { get; set; }
        public string Text { get; set; }
    }
}