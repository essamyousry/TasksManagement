using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TasksWebAPI.Models
{
    public class TaskModel
    {
        public int TaskID { get; set; }
        [Required]
        public string TaskName { get; set; }

        [Required]
        public string TaskType { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int QuoteNumber { get; set; }

        public string QuoteType { get; set; }

        public string Contact { get; set; }

    }

    public class ConvertTask
    {
        public static T_Tasks T_Tasks(TaskModel task)
        {
            T_Tasks t = new T_Tasks();
            t.TaskID = task.TaskID;
            t.TaskName = task.TaskName;
            t.TaskType = task.TaskType;
            t.QuoteType = task.QuoteType;
            t.QuoteNumber = task.QuoteNumber;
            t.DueDate = task.DueDate;
            t.Contact = task.Contact;

            return t;
        }
    }

}