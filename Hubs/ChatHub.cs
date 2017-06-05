using System;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat
{
	public class ChatHub : Hub
	{
		public void Send(string message)
		{
			Console.WriteLine(String.Format("[Called '{0}'] {1}", MethodBase.GetCurrentMethod().Name, message));
			Clients.All.Recieve(message);
		}
	}
}
