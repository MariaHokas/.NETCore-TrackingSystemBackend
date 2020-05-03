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
    public class LuokatController : ControllerBase
    {
        public List<Luokat> GetAllLuokat()
        {
            WebApiDatabaseContext db = new WebApiDatabaseContext();
            List<Luokat> luokat = db.Luokat.ToList();
            return luokat;
        }
    }
}