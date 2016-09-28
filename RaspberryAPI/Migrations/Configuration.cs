namespace RaspberryAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    //Se importan los modelos para el migrations
    using RaspberryAPI.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<RaspberryAPI.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RaspberryAPI.Models.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Sensors.AddOrUpdate( p => p.Id,
              new Sensor { Id = 1, TipoSensor = "PIR", NombreSensor = "PIR-Sala1" },
              new Sensor { Id = 2, TipoSensor = "PIR", NombreSensor = "PIR-Sala2" },
              new Sensor { Id = 3, TipoSensor = "PIR", NombreSensor = "PIR-Sala3" }
            );

            context.RegistroSensors.AddOrUpdate(p => p.Id,
                new RegistroSensor {Id = 1, FechaRegistro = DateTime.Now,Lectura = "Movimiento detectado", SensorId = 2 },
                new RegistroSensor {Id = 2, FechaRegistro = DateTime.Now,Lectura = "Movimiento detectado", SensorId = 2 },
                new RegistroSensor {Id = 3, FechaRegistro = DateTime.Now,Lectura = "Movimiento detectado", SensorId = 3 }
                );
        }
    }
}
