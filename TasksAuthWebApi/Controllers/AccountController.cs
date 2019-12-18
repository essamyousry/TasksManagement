using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TasksAuthWebApi.AuthenticationService;
using TasksAuthWebApi.DAL;
using TasksAuthWebApi.DTOs;
using TasksAuthWebApi.Models;

namespace TasksAuthWebApi.Controllers
{
    public class AccountController : ApiController
    {
        TasksContext _db = new TasksContext();

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(UserLoginDTO _user)
        {
            var user = _db.Users.Where(u => u.Username == _user.Username && u.Password == _user.Password).ToList();

            if (user != null && user.Count == 1)
            {
                AuthenticationModule auth = new AuthenticationModule();
                var token = auth.GenerateTokenForUser(user[0].Username, user[0].ID);

                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(RegisterDTO _user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var users = _db.Users.ToList().Find(u => u.Username == _user.Username);
                if (users != null)
                {
                    return BadRequest("Username already Exists");
                }
                else
                {
                    User u = new User();
                    u.Username = _user.Username;
                    u.Password = _user.Password;
                    _db.Users.Add(u);
                    _db.SaveChanges();

                    return Ok("User Added");
                }
            }
        }
    }
}
