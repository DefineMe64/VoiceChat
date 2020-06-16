using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace chat_server.Hubs
{
	public class ListHub:Hub
	{
		public static IList<User> list = new ObservableCollection<User>();
		public async Task Login(string addr,string name)
		{
			list.Add(new User());
			list[list.Count - 1].Addr = addr;
			list[list.Count - 1].UserName = name;
			await Clients.All.SendAsync("Update",JsonConvert.SerializeObject(list));
		}
		public async Task Test()
		{
			await Clients.All.SendAsync("Hello");
		}
		public async Task Quit(string addr, string name)
		{
			User temp = new User();
			temp.Addr = addr;
			temp.UserName = name;
			list.Remove(temp);
			await Clients.All.SendAsync("Update", JsonConvert.SerializeObject(list));
		}
	}
}
