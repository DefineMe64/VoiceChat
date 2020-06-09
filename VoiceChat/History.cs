using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace VoiceChat
{
	class History:INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}
		private ObservableCollection<Message> chatHistory=new ObservableCollection<Message>();
		public ObservableCollection<Message> ChatHistory
		{
			get { return chatHistory; }
			set
			{
				chatHistory = value;
				OnPropertyChanged(new PropertyChangedEventArgs("ChatHistory"));
			}
		}
		public string LocalName { get; set; }
		public string LocalAddr { get; set; }
		public string UserName { get; set; }
		public string UserAddr { get; set; }
		public History(string localName,string localAddr,string userName,string userAddr)
		{
			LocalName = localName;
			LocalAddr = localAddr;
			UserName = userName;
			UserAddr = userAddr;
		}
	}
}
