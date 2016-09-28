using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RaspberryAPI.Models;

namespace RaspberryAPI.Controllers
{
    public class RegistroController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/Registro
        public IQueryable<RegistroSensor> GetRegistroSensors()
        {
            return db.RegistroSensors;
        }

        // GET api/Registro/5
        [ResponseType(typeof(RegistroSensor))]
        public async Task<IHttpActionResult> GetRegistroSensor(string id)
        {
            RegistroSensor registrosensor = await db.RegistroSensors.FindAsync(id);
            if (registrosensor == null)
            {
                return NotFound();
            }

            return Ok(registrosensor);
        }

        // PUT api/Registro/5
        public async Task<IHttpActionResult> PutRegistroSensor(int id, RegistroSensor registrosensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registrosensor.Id)
            {
                return BadRequest();
            }

            db.Entry(registrosensor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroSensorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Registro
        [ResponseType(typeof(RegistroSensor))]
        public async Task<IHttpActionResult> PostRegistroSensor(RegistroSensor registrosensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RegistroSensors.Add(registrosensor);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RegistroSensorExists(registrosensor.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = registrosensor.Id }, registrosensor);
        }

        // DELETE api/Registro/5
        [ResponseType(typeof(RegistroSensor))]
        public async Task<IHttpActionResult> DeleteRegistroSensor(int id)
        {
            RegistroSensor registrosensor = await db.RegistroSensors.FindAsync(id);
            if (registrosensor == null)
            {
                return NotFound();
            }

            db.RegistroSensors.Remove(registrosensor);
            await db.SaveChangesAsync();

            return Ok(registrosensor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegistroSensorExists(int id)
        {
            return db.RegistroSensors.Count(e => e.Id == id) > 0;
        }
    }
}