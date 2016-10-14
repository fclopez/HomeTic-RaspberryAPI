using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

/*importa los modelos*/
using RaspberryAPI.Models;

/*por documentar*/
/*librerias para el socket*/
using Microsoft.Web.WebSockets;
using System.Web.Script.Serialization;
using System.Threading;

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
                //if(item.userid != value.userid)
                if (true)
                {
                    Transaccion trans = new Transaccion();
                    trans.SendToClient += SendToClientMessage;
                    trans.StarProcess(item, value.tipoproceso);
                    trans.SendToClient -= SendToClientMessage;
                    item.Send("Listo ");
                }
            }
        }

        private void SendToClientMessage(object sender, Evento e)
        {
            e.eventos.Send(e.mensaje);
        }

    }

    public class Transaccion
    {
        public event EventHandler<Evento> SendToClient;
        protected virtual void OnSendToClient(Evento e)
        {
            if (this.SendToClient != null)
            {
                this.SendToClient(this, e);
            }
        }
        public void StarProcess(ProcessWebSocketHandler evento, string mensaje)
        {
            Evento evnt = new Evento();
            evnt.eventos = evento;
            evnt.mensaje = mensaje;
            this.OnSendToClient(evnt);
        }

    }

}
