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

            /*return doku.DocumentationId.ToString; //k*//*uittaus Frontille, että päivitys meni oikein --> Frontti voi tsekata, että kontrolleri palauttaa saman id:n mitä käsitteli*/
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

            /*return doku.DocumentationId.ToString; //k*//*uittaus Frontille, että päivitys meni oikein --> Frontti voi tsekata, että kontrolleri palauttaa saman id:n mitä käsitteli*/
        }

        [HttpGet]
        [Route("R")]

        public IActionResult GetSomeTunnit(int offset, int limit, string oppilasid)
        {
            //if (oppilasid != null)
            //{
            //    WebApiDatabaseContext db = new WebApiDatabaseContext();
            //    List<Tunnit> leimaukset = db.Tunnit.Where(d => d.OppilasId == oppilasid).ToList();
            //    return Ok(leimaukset);
            //}

            //else
            {
                WebApiDatabaseContext db = new WebApiDatabaseContext();
                var model = (from c in db.Tunnit
                             join au in db.Users on c.UserId equals au.Id
                             join l in db.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
                             orderby c.TunnitId descending
                             select new
                             {
                                 c.TunnitId,
                                 LuokkahuoneNimi = l.LuokkaNimi,
                                 c.Sisaan,
                                 c.Ulos,
                                 c.UserId,
                                 OppilasName = au.FirstName + " " + au.LastName,
                             }).Skip(offset).Take(limit).ToList();

                return Ok(model);
            }

        }
    }
}