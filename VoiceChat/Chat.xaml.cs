using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VoiceChat
{
	/// <summary>
	/// Chat.xaml 的交互逻辑
	/// </summary>
	public partial class Chat : Window
	{
		private User local;
		private UserView userView;
		private List<History> Historys;
		public Chat(string localUserName,string localAddr)
		{
			InitializeComponent();
			//记录登录的用户名和本地IP
			local = new User(localAddr, localUserName);
			//连接联系人列表数据源
			userView = new UserView();
			listStackPanel.DataContext = userView;
			//消息历史集合
			Historys = new List<History>();
		}
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
		private void Close_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
		private void Min_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}
		private void Record_Click(object sender, RoutedEventArgs e)
		{
			userView.List.Add(new User("127.0.0.2", "李四"));
			Historys.Add(new History(local.UserName, local.Addr, "李四", "127.0.0.2"));
			foreach (History history in Historys)
			{
				if (history.UserAddr == "127.0.0.2")
				{
					history.ChatHistory.Add(new Message("你不好", DateTime.Now, false));
				}
			}
		}
		//Test
		private void AddTest_Click(object sender, RoutedEventArgs e)
		{
			userView.List.Add(new User("127.0.0.1","张三"));
			Historys.Add(new History(local.UserName,local.Addr,"张三", "127.0.0.1"));
			foreach(History history in Historys)
			{
				if(history.UserAddr== "127.0.0.1")
				{
					history.ChatHistory.Add(new Message("你好",DateTime.Now,false));
				}
			}
		}
		private void SelectUser_Change(object sender, RoutedEventArgs e)
		{
			foreach(History temp in Historys)
			{
				if (temp.UserAddr == ((User)userList.SelectedItem).Addr)
					messageStackPanel.DataContext = temp;
			}
		}
	}
}
