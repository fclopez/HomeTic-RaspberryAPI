using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RaspberryAPI.Models
{
    public class RegistroSensor
    {
        //autoincremental
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }
        public string Lectura { get; set; }
        //Foreign key sensor
        [ForeignKey("Sensor")]
        public int SensorId { get; set; }
        //Propiedad de navegación
        public virtual Sensor Sensor { get; set; }
    }
}