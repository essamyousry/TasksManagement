using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TasksAuthWebApi.AuthenticationService;
using TasksAuthWebApi.DAL;
using TasksAuthWebApi.Models;

namespace TasksAuthWebApi.Controllers
{
    [AuthorizationFilter]
    public class TasksController : ApiController
    {
        static TasksContext _db = new TasksContext();

        [Route("GetTaskByID/{id}")]
        public IHttpActionResult GetTaskByID(int id)
        {
            TTask t =_db.TTasks.Find(id);
            if (t == null) return NotFound();
            else
            {
                return Ok(t);
            }
        }

        [Route("GetAllTasks")]
        public IQueryable GetAllTasks()
        {
            return _db.TTasks;
        }
    }
}
