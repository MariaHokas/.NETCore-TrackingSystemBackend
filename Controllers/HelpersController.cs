using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timeTrackingSystemBackend.Entities;

namespace timeTrackingSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpersController : ControllerBase
    {
        [HttpGet]
        [Route("user")]

        public IActionResult GetUsersByRole()
        {
            {
                WebApiDatabaseContext db = new WebApiDatabaseContext();
                var users = (from u in db.Users
                             where u.Role == "User"
                             select new
                             {
                                 u.Id,
                                 Name = u.FirstName + " " + u.LastName,
                             }).ToList();

                return Ok(users);
            }

        }
    }
}