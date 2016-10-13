using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

/*importa los modelos*/
using RaspberryAPI.Models;

/*librerias para el socket*/
//using Microsoft.Web.WebSockets;
using System.Web.Script.Serialization;
//using System.Net.WebSockets;
//using Microsoft.AspNet.SignalR.WebSockets;
/*por documentar*/
using System.Threading;
using Microsoft.Web.WebSockets;

namespace RaspberryAPI.Controllers
{
    public class SocketController : ApiController
    {
        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Get()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest(new ProcessWebSocketHandler());
                return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


    }

    public class ProcessWebSocketHandler : WebSocketHandler
    {
        private static WebSocketCollection _processClient = new WebSocketCollection();
        public string userid { get; set; }
        //public ProcessWebSocketHandler()
        //{
        //}

        public override void OnClose()
        {
            //base.OnClose();
            var usuarios = string.Empty;
            _processClient.Remove(this);
            foreach (ProcessWebSocketHandler item in _processClient)
            {
                usuarios += (usuarios == string.Empty ? "" : ",") + item.userid;
            }
            _processClient.Broadcast("Usuario: " + usuarios);
        }

        public override void OnOpen()
        {
            //base.OnOpen();
            if (!_processClient.Contains(this))
            {
                this.userid = this.WebSocketContext.QueryString["userid"];
                _processClient.Add(this);

                var usuarios = string.Empty;
                foreach (ProcessWebSocketHandler item in _processClient)
                {
                    usuarios += (usuarios == string.Empty ? "" : ",") + item.userid;
                }
                _processClient.Broadcast("Usuario: " + usuarios);

                /*envia el token de conexión*/
                foreach (ProcessWebSocketHandler item in _processClient)
                {
                    item.Send("key:" + item.WebSocketContext.SecWebSocketKey);
                }
            }
        }

        public override void OnMessage(string json)
        {
            //base.OnMessage(message);
            var jsSerializado = new JavaScriptSerializer();
            MensajeCliente value = (MensajeCliente)jsSerializado.Deserialize(json, typeof(MensajeCliente));
            string strBody = jsSerializado.Serialize(value);

            foreach (ProcessWebSocketHandler item in _processClient)
            {
                if (item.userid == value.userid)
                {
                    item.Send("Procesando...");
                    Transaccion trans = new Transaccion();
                    trans.EnviarACliente += SendToClientMessage;
                    trans.StarProcess(item, value.tipoproceso);
                    trans.EnviarACliente -= SendToClientMessage;
                    item.Send("Listo ");
                }
            }
        }

        private void SendToClientMessage(object sender, Evento e)
        {
            e.evento.Send(e.mensaje);
        }

    }


    public class Transaccion
    {
        public event EventHandler<Evento> EnviarACliente;
        protected virtual void OnSendToClient(Evento e)
        {
            if (this.EnviarACliente != null)
            {
                this.EnviarACliente(this, e);
            }
        }

        public void StarProcess(ProcessWebSocketHandler handler, string mensaje)
        {
            Thread.Sleep(1000);
            for (int i = 0; i < 5; i++)
            {
                Evento e = new Evento()
                {
                    evento = handler,
                    mensaje = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss fffffff") + "=>" + mensaje + "Procesando " + (i * 20).ToString() + "%"
                };
                this.OnSendToClient(e);
                Thread.Sleep(200000 * ((i % 2) + 1));
            }
        }
    }



}
