using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TasksWebAPI.Models;

namespace TasksWebAPI.Controllers
{
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [Authorize]
    public class TasksController : ApiController
    {
        private TasksDatabaseEntities _db = new TasksDatabaseEntities();

        [Route("api/GetAllTasks")]
        public IQueryable GetAllTasks()
        {
            return _db.T_Tasks;
        }

        [Route("api/GetTask/{id}")]
        public IHttpActionResult GetTaskByID(int id)
        {
            T_Tasks t = _db.T_Tasks.Find(id);
            if (t == null)
            {
                return NotFound();
            }
            return Ok(t);
        }

        [Route("api/UpdateTask/{id}")]
        public IHttpActionResult PutTaskByID(int id, [FromBody] TaskModel task)
        {
            T_Tasks t = _db.T_Tasks.Find(id);

            if (t == null)
            {
                return BadRequest("No Task with id " + id);
            }

            if (task == null)
            {
                return BadRequest("Task cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            t.TaskName = task.TaskName;
            t.TaskType = task.TaskType;
            t.QuoteType = task.QuoteType;
            t.QuoteNumber = task.QuoteNumber;
            t.DueDate = task.DueDate;
            t.Contact = task.Contact;

            _db.SaveChanges();
            return Ok("Task Updated");

        }

        [Route("api/AddTask")]
        public IHttpActionResult PostTask([FromBody] TaskModel task)
        {
            if (task == null)
            {
                return BadRequest("Task cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.T_Tasks.Add(ConvertTask.T_Tasks(task));
            _db.SaveChanges();
            return Ok("Task Added");
        }

        [Route("api/DeleteTask/{id}")]
        public IHttpActionResult DeleteTask(int id)
        {
            T_Tasks t = _db.T_Tasks.Find(id);
            if (t == null)
            {
                return NotFound();
            }
            _db.T_Tasks.Remove(t);
            _db.SaveChanges();
            return Ok("Task Deleted");
        }
    }
}
