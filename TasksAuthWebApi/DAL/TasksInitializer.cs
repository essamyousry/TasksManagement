using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TasksAuthWebApi.Models;

namespace TasksAuthWebApi.DAL
{
    public class TasksInitializer : System.Data.Entity. DropCreateDatabaseIfModelChanges<TasksContext>
    {
        protected override void Seed(TasksContext context)
        {
            var tasks = new List<TTask>
            {
                new TTask{ID = 0, TaskName = "TaskA"},
                new TTask{ID = 1, TaskName = "TaskB"},
                new TTask{ID = 2, TaskName = "TaskC"},
                new TTask{ID = 3, TaskName = "TaskD"},
                new TTask{ID = 4, TaskName = "TaskE"},
            };

            tasks.ForEach(t => context.TTasks.Add(t));
            context.SaveChanges();

            var users = new List<User>
            {
                new User{ID = 0, Username = "essam", Password = "Test_123"}
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}