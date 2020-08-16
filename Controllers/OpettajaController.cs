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
    public class OpettajaController : ControllerBase
    {
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
                    UserId = tunti.UserId.GetValueOrDefault(),
                    Sisaan = tunti.Sisaan,
                    Ulos = tunti.Ulos,
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

        [HttpPut]
        [Route("{key}")]
        public ActionResult PutEdit(int key, [FromBody] Tunnit tunti)
        {
            WebApiDatabaseContext db = new WebApiDatabaseContext();
            try
            {
                Tunnit leimaus = db.Tunnit.Find(key);
                if (tunti != null)
                {
                    leimaus.LuokkahuoneId = tunti.LuokkahuoneId;
                    leimaus.Sisaan = tunti.Sisaan;
                    leimaus.Ulos = tunti.Ulos;
                    leimaus.UserId = tunti.UserId;


                    db.SaveChanges();
                    return Ok(leimaus.TunnitId);
                }

                else
                {
                    return NotFound("Päivitettävää leimausta ei löytynyt!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuntia päivittäessä");
            }

            finally
            {
                db.Dispose();
            }
        }
        [HttpDelete]
        [Route("{key}")]
        public ActionResult Deleteleimaus(int key) //
        {
            WebApiDatabaseContext db = new WebApiDatabaseContext();
            Tunnit tunti = db.Tunnit.Find(key);
            if (tunti != null)
            {
                db.Tunnit.Remove(tunti);
                db.SaveChanges();
                return Ok("tunti " + key + " poistettiin");
            }
            else
            {
                return NotFound("tuntita " + key + " ei löydy");
            }
        }

        [HttpGet]
        [Route("R")]

        public IActionResult GetSomeTunnit(int offset, int limit, string lastName)
        {
            if (lastName != null)
            {
                WebApiDatabaseContext db = new WebApiDatabaseContext();
                List<Tunnit> leimaukset = db.Tunnit.Where(d => d.LuokkahuoneId == lastName).ToList();
                return Ok(leimaukset);
            }

            else
            {
                WebApiDatabaseContext db = new WebApiDatabaseContext();
                var model = (from c in db.Tunnit
                             join au in db.Users on c.UserId equals au.Id
                             join l in db.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
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
                             }).Skip(offset).Take(limit).ToList();

                return Ok(model);
            }

        }
    }
}