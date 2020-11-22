using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using timeTrackingSystemBackend.Entities;

namespace timeTrackingSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OppilasController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Tunnit> GetAllLogins()
        {
            WebApiDatabaseContext db = new WebApiDatabaseContext();
            List<Tunnit> tunnit = db.Tunnit.ToList();
            return tunnit;
        }

        // POST: api/Leimaus
        [HttpPost]
        [Route("add")]
        public ActionResult PostTunti([FromBody] Tunnit tunti)
        {

            WebApiDatabaseContext db = new WebApiDatabaseContext();
            try
            {
                Tunnit dbItem = new Tunnit()

                {
                    TunnitId = tunti.TunnitId,
                    LuokkahuoneId = tunti.LuokkahuoneId,
                    UserId = tunti.UserId,
                    Sisaan = tunti.Sisaan,
                    Ulos = tunti.Ulos,
                    Tarkastettu = false
                };

                _ = db.Tunnit.Add(dbItem);
                _ = db.SaveChanges();

                return Ok(dbItem.TunnitId);
            }

            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen leimausta lisättäessä!?!?");
            }

            finally
            {

                db.Dispose();
            }

        }

        // POST: api/Leimaus
        [HttpPost]
        [Route("sisaan")]
        public ActionResult PostSisaan([FromBody] Tunnit tunti)
        {
            WebApiDatabaseContext db = new WebApiDatabaseContext();

            try
            {
                Tunnit dbItem = new Tunnit()

                {
                    TunnitId = tunti.TunnitId,
                    LuokkahuoneId = tunti.LuokkahuoneId,
                    OppilasId = tunti.OppilasId,
                    UserId = tunti.UserId,
                    Sisaan = DateTime.Now,
                    Tarkastettu = true
                };

                if (tunti.LuokkahuoneId == "1" || tunti.LuokkahuoneId == "2" || tunti.LuokkahuoneId == "3")
                {
                    _ = db.Tunnit.Add(dbItem);
                    _ = db.SaveChanges();               
                }
                return Ok(dbItem.TunnitId);
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen leimausta lisättäessä!?!?");
            }

            finally
            {
                db.Dispose();
            }        
        }

        // POST: api/Leimaus
        [HttpPost]
        [Route("ulos")]
        public ActionResult PostUlos([FromBody] Tunnit tunti)
        {
            WebApiDatabaseContext db = new WebApiDatabaseContext();

            try
            {

                Tunnit dbItem = (from p in db.Tunnit
                                 where p.OppilasId == tunti.OppilasId && p.LuokkahuoneId == tunti.LuokkahuoneId
                                 orderby p.TunnitId descending
                                 select p).First();
                {
                    dbItem.Ulos = DateTime.Now;

                    if (tunti.LuokkahuoneId == "1" || tunti.LuokkahuoneId == "2" || tunti.LuokkahuoneId == "3")
                        _ = db.SaveChanges();

                    return Ok(dbItem.TunnitId);
                }
            }

            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen leimausta lisättäessä!?!?");
            }

            finally
            {

                db.Dispose();
            }
        }

        [HttpGet]
        [Route("R")]

        public IActionResult GetTunnit(bool approvalStatus)
        {
            if (approvalStatus == true)
            {
                WebApiDatabaseContext db = new WebApiDatabaseContext();
                var model = (from c in db.Tunnit
                             join au in db.Users on c.UserId equals au.Id
                             join l in db.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
                             where c.Tarkastettu == false || c.Tarkastettu == null
                             orderby c.TunnitId descending
                             select new
                             {
                                 c.TunnitId,
                                 c.LuokkahuoneId,
                                 LuokkahuoneNimi = l.LuokkaNimi,
                                 c.Sisaan,
                                 c.Ulos,
                                 c.UserId,
                                 OppilasName = au.FirstName + " " + au.LastName,
                             }).ToList();
                return Ok(model);
            }
            else
            {
                WebApiDatabaseContext dt = new WebApiDatabaseContext();
                var model2 = (from c in dt.Tunnit
                             join au in dt.Users on c.UserId equals au.Id
                             join l in dt.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
                             where c.Tarkastettu == true
                             orderby c.TunnitId descending
                             select new
                             {
                                 c.TunnitId,
                                 c.LuokkahuoneId,
                                 LuokkahuoneNimi = l.LuokkaNimi,
                                 c.Sisaan,
                                 c.Ulos,
                                 c.UserId,
                                 OppilasName = au.FirstName + " " + au.LastName,
                             }).ToList();
                return Ok(model2);
            }
            
        }
    }
}