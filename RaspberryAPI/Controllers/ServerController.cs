using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
/**/
using System.Web.Http;
using System.Web;
using System.Web.WebSockets;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Text;


namespace RaspberryAPI.Controllers
{
    public class ServerController : ApiController
    {
        public HttpResponseMessage Get()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessSocket);
                return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public async Task ProcessSocket(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

                /* espera asincrona para la llegada de un mensaje desde el cliente*/
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);


                /*Si el socket sigue abierto, envie de regreso el mensaje al cliente*/
                if (socket.State == WebSocketState.Open)
                {
                    string userMesage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    userMesage = "Usuario: " + context.UserHostName + " => " + userMesage + " at " + DateTime.Now.ToString();
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMesage));

                    /*envia mensaje al cliente*/
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }


    }
}
