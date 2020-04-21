using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timeTrackingSystemBackend.Entities;

namespace timeTrackingSystemBackend.Controllers
{
    [Route("[controller]")]
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


    }
}