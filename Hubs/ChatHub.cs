﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat
{
	public class ChatHub : Hub
	{
		private readonly SignalRContext signalRContext;

		public ChatHub(SignalRContext context)
		{
			this.signalRContext = context;
		}

		public override Task OnConnected()
		{
			Console.WriteLine("OnConnected " + Context.ConnectionId);

			if(!this.signalRContext.SignalRItemList.Any(item => item.ConnectionId == Context.ConnectionId)){
				this.signalRContext.Update(new SignalRItem{ConnectionId = Context.ConnectionId});
				this.signalRContext.SaveChanges();
			}

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Console.WriteLine("OnDisconnected " + Context.ConnectionId);

			SignalRItem signalRItem = this.signalRContext.SignalRItemList.FirstOrDefault(item => item.ConnectionId == Context.ConnectionId);
			if(signalRItem != null) {
				this.signalRContext.Remove(signalRItem);
				this.signalRContext.SaveChanges();
			}

			return base.OnDisconnected(stopCalled);
		}

		public void Send(string message)
		{
			Console.WriteLine(String.Format("[Called '{0}'] {1}", MethodBase.GetCurrentMethod().Name, message));
			Clients.All.Recieve(message);
		}
	}
}
