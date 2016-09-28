using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaspberryAPI.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        [Required]
        public string NombreSensor { get; set; }
        public string TipoSensor { get; set; }
    }
}