using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Console;
using Newtonsoft.Json;

namespace chat_server.Hubs
{
	public class ListHub:Hub
	{
		public static IList<User> list = new ObservableCollection<User>();
		public async Task Login(string addr,string name)
		{
			Console.WriteLine($"{addr} connected");
			list.Add(new User());
			list[list.Count - 1].Addr = addr;
			list[list.Count - 1].UserName = name;
			await Clients.All.SendAsync("Update",JsonConvert.SerializeObject(list));
		}
		public async Task Quit(string addr, string name)
		{
			Console.WriteLine($"{addr} disconnected");
			for(int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].Addr == addr && list[i].UserName == name)
					list.Remove(list[i]);
			}
			await Clients.AllExcept(Context.ConnectionId).SendAsync("Update", JsonConvert.SerializeObject(list));
		}
	}
}
