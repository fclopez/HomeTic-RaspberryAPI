using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RaspberryAPI.Controllers;

namespace RaspberryAPI.Models
{
    public class Evento
    {
        public ProcessWebSocketHandler evento { get; set; }
        public string sesion { get; set; }
        public string mensaje { get; set; }

    }
}