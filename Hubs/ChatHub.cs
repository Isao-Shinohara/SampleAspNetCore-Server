using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat
{
	public class ChatHub : Hub
	{
		public static HashSet<string> ConnectedIds = new HashSet<string>();

		public override Task OnConnected()
		{
			Console.WriteLine("OnConnected");
			ConnectedIds.Add(Context.ConnectionId);
			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Console.WriteLine("OnDisconnected");
			ConnectedIds.Remove(Context.ConnectionId);
			return base.OnDisconnected(stopCalled);
		}

		public void Send(string message)
		{
			Console.WriteLine(String.Format("[Called '{0}'] {1}", MethodBase.GetCurrentMethod().Name, message));
			Clients.All.Recieve(message);
		}
	}
}
