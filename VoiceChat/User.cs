using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat
{
	class User
	{
		public string Addr { get; set; }
		public string UserName { get; set; }
		public User(string addr,string userName)
		{
			Addr = addr;
			UserName = userName;
		}
	}
}
