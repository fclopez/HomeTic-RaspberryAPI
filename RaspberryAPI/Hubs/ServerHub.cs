﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace RaspberryAPI.Hubs
{
    public class ServerHub : Hub
    {
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}