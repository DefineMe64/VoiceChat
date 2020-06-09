using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace VoiceChat
{
	class UserView:INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}
		private ObservableCollection<User> list = new ObservableCollection<User>();
		public ObservableCollection<User> List
		{
			get { return list; }
			set
			{
				list = value;
				OnPropertyChanged(new PropertyChangedEventArgs("ListStatus"));
			}
		}
	}
}
