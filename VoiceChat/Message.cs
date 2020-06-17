using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat
{
	class Message
	{
		public string ChatContent { get; set; }
		public DateTime ChatTime { get; set; }
		public bool IsOriginNative { get; set; }
		public Message(string chatContent, DateTime chatTime, bool isOriginNative)
		{
			ChatContent = chatContent;
			ChatTime = chatTime;
			IsOriginNative = isOriginNative;
		}
	}
}
