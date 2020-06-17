using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

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
		private HubConnection connection;
		private UdpClient udpClient;
		private Task receiveTask;
		public Chat(string localUserName,string localAddr)
		{
			InitializeComponent();
			//记录登录的用户名和本地IP
			local = new User();
			local.Addr = localAddr;
			local.UserName = localUserName;
			//连接联系人列表数据源
			userView = new UserView();
			listStackPanel.DataContext = userView;
			//消息历史集合
			Historys = new List<History>();
			//初始化UI
			recordBtn.IsEnabled = false;
			//创建服务器连接
			connection = new HubConnectionBuilder()
				.WithUrl("https://localhost:5001/listhub")
				.Build();
			connection.Closed += async (error) =>
			{
				await Task.Delay(new Random().Next(0, 5) * 1000);
				await connection.StartAsync();
			};
			connection.On<string>("Update",(listJson)=> {
				IList<User> users = JsonConvert.DeserializeObject<ObservableCollection<User>>(listJson);
				userView.List.Clear();
				foreach (User user in users)
				{
					userView.List.Add(user);
				}
			});
			Start();
			StartLogin();
			//开放udp通讯8888端口
			udpClient = new UdpClient(8888);
			//开启监听任务
			receiveTask = Task.Run(() =>
			{
				while (true)
				{
					Task<UdpReceiveResult> udpReceiveResult = udpClient.ReceiveAsync();
					UdpReceiveResult result = udpReceiveResult.Result;
					foreach (History history in Historys)
					{
						if (history.UserAddr == result.RemoteEndPoint.Address.ToString())
						{
							//history.ChatHistory.Add(new Message(Encoding.ASCII.GetString(result.Buffer), DateTime.Now, false));
							Application.Current.Dispatcher.Invoke(new Action(() => {
								history.ChatHistory.Add(new Message(Encoding.ASCII.GetString(result.Buffer), DateTime.Now, false));
							}));
						}
					}
				}
			});
		}
		private void OutputError(string message)
		{
			MessageBox.Show(message);
		}
		private async void Start()
		{
			try
			{
				await connection.StartAsync();
			}
			catch (Exception ex)
			{
				OutputError(ex.Message);
			}
		}
		private async void StartLogin()
		{
			try
			{
				await connection.InvokeAsync("Login", local.Addr,local.UserName);
			}
			catch (Exception ex)
			{
				OutputError(ex.Message);
			}
		}
		private async void End()
		{
			try
			{
				await connection.InvokeAsync("Quit", local.Addr,local.UserName);
				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
		private void Close_Click(object sender, RoutedEventArgs e)
		{
			End();
			Application.Current.Shutdown();
		}
		private void Min_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}
		private void Record_Click(object sender, RoutedEventArgs e)
		{
			string selectAddr = ((User)userList.SelectedItem).Addr;
			string selectName = ((User)userList.SelectedItem).UserName;
			string sendString = "hello," + selectName;
			byte[] sendByte = Encoding.ASCII.GetBytes(sendString);
			udpClient.SendAsync(sendByte,sendByte.Length ,selectAddr,8888);
			foreach(History history in Historys)
			{
				if (history.UserName == selectName && history.UserAddr == selectAddr)
				{
					history.ChatHistory.Add(new Message(sendString, DateTime.Now, true));
				}
			}
		}
		//Test
		private void AddTest_Click(object sender, RoutedEventArgs e)
		{
			
		}
		private void SelectUser_Change(object sender, RoutedEventArgs e)
		{
			recordBtn.IsEnabled = true;
			string selectAddr = ((User)userList.SelectedItem).Addr;
			string selectName = ((User)userList.SelectedItem).UserName;
			bool flag = false;
			foreach (History history in Historys)
			{
				if (history.UserName == selectName && history.UserAddr == selectAddr)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Historys.Add(new History(local.UserName, local.Addr, selectName, selectAddr));
			}
			foreach (History temp in Historys)
			{
				if (temp.UserAddr == selectAddr)
					messageStackPanel.DataContext = temp;
			}
		}
	}
}
