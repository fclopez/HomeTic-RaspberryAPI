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
using System.Web.Http.Cors;

namespace RaspberryAPI.Controllers
{

     [EnableCors(origins: "http://localhost:59720, http://localhost:3289,http://localhost:3288,http://localhost:20216,http://localhost:20193,http://localhost:22372", headers: "*", methods: "*")]
    public class RegistroController : ApiController
    {
        private DataContext contextDb = new DataContext();

        // GET api/Registro
        public IQueryable<dynamic> GetRegistroSensors()
        {
            var listaRegistros = (from reg in contextDb.RegistroSensors
                                  select new { 
                                      reg.Id,
                                      reg.FechaRegistro,
                                      reg.Lectura,
                                      reg.SensorId                                 
                                  });
            return listaRegistros;
        }

        // GET api/Registro/5
        [ResponseType(typeof(RegistroSensor))]
        public async Task<IHttpActionResult> GetRegistroSensor(string id)
        {
            RegistroSensor registrosensor = await contextDb.RegistroSensors.FindAsync(id);
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

            contextDb.Entry(registrosensor).State = EntityState.Modified;

            try
            {
                await contextDb.SaveChangesAsync();
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

            contextDb.RegistroSensors.Add(registrosensor);

            try
            {
                await contextDb.SaveChangesAsync();
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
            RegistroSensor registrosensor = await contextDb.RegistroSensors.FindAsync(id);
            if (registrosensor == null)
            {
                return NotFound();
            }

            contextDb.RegistroSensors.Remove(registrosensor);
            await contextDb.SaveChangesAsync();

            return Ok(registrosensor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contextDb.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegistroSensorExists(int id)
        {
            return contextDb.RegistroSensors.Count(e => e.Id == id) > 0;
        }
    }
}