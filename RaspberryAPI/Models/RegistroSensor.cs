using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaspberryAPI.Models
{
    public class RegistroSensor
    {
        public int Id { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        public string Lectura { get; set; }
        //Foreign key sensor
        public int SensorId { get; set; }
        //Propiedad de navegación
        public Sensor Sensor { get; set; }
    }
}