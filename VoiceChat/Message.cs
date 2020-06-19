using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat
{
	class Message
	{
		public string ChatPath { get; set; }
		public DateTime ChatTime { get; set; }
		public bool IsOriginNative { get; set; }
		public Message(string chatPath, DateTime chatTime, bool isOriginNative)
		{
			ChatPath = chatPath;
			ChatTime = chatTime;
			IsOriginNative = isOriginNative;
		}
	}
}
