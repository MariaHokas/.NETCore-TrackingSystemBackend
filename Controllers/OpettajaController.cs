﻿using System;
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
                    UserId = tunti.UserId,
                    Sisaan = tunti.Sisaan,
                    Ulos = tunti.Ulos,
                    Tarkastettu = true
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

        // POST: api/approve
        [HttpPost]
        [Route("op/{key}")]
        public ActionResult ApproveStudentTimestamp(int key)
        {
            WebApiDatabaseContext db = new WebApiDatabaseContext();
            try
            {
                Tunnit dbItem = (from p in db.Tunnit
                                 where p.TunnitId == key
                                 select p).First();
                {
                    dbItem.TunnitId = key;
                    dbItem.Tarkastettu = true;
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
        public void testiMetodi()
        {
            WebApiDatabaseContext webdb = new WebApiDatabaseContext();
            var modelNoLimits = (from c in webdb.Tunnit
                                 join au in webdb.Users on c.UserId equals au.Id
                                 join l in webdb.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
                                 orderby c.TunnitId descending
                                 where c.Tarkastettu == true
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
           
        }
        [HttpGet]
        [Route("R")]

        public IActionResult GetSomeTunnit( string lastName, int offset, int limit)
        {
            if (lastName != null)
            {
                WebApiDatabaseContext db = new WebApiDatabaseContext();
                List<Tunnit> leimaukset = db.Tunnit.Where(d => d.LuokkahuoneId == lastName).ToList();
                return Ok(leimaukset);
            }

            else 
            {
                if (limit == 10)
                {
                    WebApiDatabaseContext webdb = new WebApiDatabaseContext();
                    var modelNoLimits = (from c in webdb.Tunnit
                                 join au in webdb.Users on c.UserId equals au.Id
                                 join l in webdb.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
                                 orderby c.TunnitId descending
                                 where c.Tarkastettu == true
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
                    return Ok(modelNoLimits);
                }
                else if (limit == 11)
                {
                    WebApiDatabaseContext tb = new WebApiDatabaseContext();
                    var sentByOppilas = (from c in tb.Tunnit
                                 join au in tb.Users on c.UserId equals au.Id
                                 join l in tb.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
                                 orderby c.TunnitId descending
                                 where c.Tarkastettu == false || c.Tarkastettu == null
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
                    return Ok(sentByOppilas);

                }
                else { 
                WebApiDatabaseContext db = new WebApiDatabaseContext();
                var model = (from c in db.Tunnit
                             join au in db.Users on c.UserId equals au.Id
                             join l in db.Luokat on c.LuokkahuoneId equals l.LuokkahuoneId.ToString()
                             orderby c.TunnitId descending
                             where c.Tarkastettu == true
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
}