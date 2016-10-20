namespace RaspberryAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistroSensors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaRegistro = c.DateTime(nullable: false),
                        Lectura = c.String(),
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sensors", t => t.SensorId, cascadeDelete: true)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreSensor = c.String(nullable: false),
                        TipoSensor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegistroSensors", "SensorId", "dbo.Sensors");
            DropIndex("dbo.RegistroSensors", new[] { "SensorId" });
            DropTable("dbo.Sensors");
            DropTable("dbo.RegistroSensors");
        }
    }
}
