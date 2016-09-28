using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RaspberryAPI.Models;

namespace RaspberryAPI.Controllers
{
    public class SensorController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/Sensor
        public IQueryable<Sensor> GetSensors()
        {
            return db.Sensors;
        }

        // GET api/Sensor/5
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult GetSensor(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // PUT api/Sensor/5
        public IHttpActionResult PutSensor(int id, Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensor.Id)
            {
                return BadRequest();
            }

            db.Entry(sensor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorExists(id))
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

        // POST api/Sensor
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult PostSensor(Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sensors.Add(sensor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensor.Id }, sensor);
        }

        // DELETE api/Sensor/5
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult DeleteSensor(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            db.Sensors.Remove(sensor);
            db.SaveChanges();

            return Ok(sensor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SensorExists(int id)
        {
            return db.Sensors.Count(e => e.Id == id) > 0;
        }
    }
}