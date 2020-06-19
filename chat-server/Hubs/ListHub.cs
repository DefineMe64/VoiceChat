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
		public async Task Login(string userJson)
		{
			User newUser = JsonConvert.DeserializeObject<User>(userJson);
			Console.WriteLine($"{newUser.Addr} connected");
			list.Add(newUser);
			await Clients.AllExcept(Context.ConnectionId).SendAsync("AddUser", userJson);
			await Clients.Client(Context.ConnectionId).SendAsync("UpdateList", JsonConvert.SerializeObject(list));
		}
		public async Task Quit(string userJson)
		{
			User deleteUser = JsonConvert.DeserializeObject<User>(userJson);
			Console.WriteLine($"{deleteUser.Addr} disconnected");
			for(int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].Addr == deleteUser.Addr && list[i].UserName == deleteUser.UserName)
					list.Remove(list[i]);
			}
			await Clients.AllExcept(Context.ConnectionId).SendAsync("DeleteUser", userJson);
		}
	}
}
