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
using System.Runtime.InteropServices;
using System.IO;

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
			recordBtn.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(Record_MouseLeftButtonDown), true);
			recordBtn.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(Record_MouseLeftButtonUp), true);
			
			//创建用于记录录音文件的文件夹
			if (!System.IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory+"\\record"))
				System.IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\\record");
			//创建服务器连接
			connection = new HubConnectionBuilder()
				.WithUrl("https://localhost:5001/listhub")
				.Build();
			connection.Closed += async (error) =>
			{
				await Task.Delay(new Random().Next(0, 5) * 1000);
				await connection.StartAsync();
			};
			//往hub中添加客户端方法
			connection.On<string>("Update",(listJson)=> {
				IList<User> users = JsonConvert.DeserializeObject<ObservableCollection<User>>(listJson);
				userView.List.Clear();
				foreach (User user in users)
				{
					userView.List.Add(user);
				}

			});
			//当有用户登录，其他用户添加该用户到聊天列表，并创建对应聊天历史
			connection.On<string>("AddUser", (userJson) => {
				User newUser = JsonConvert.DeserializeObject<User>(userJson);
				userView.List.Add(newUser);
				Historys.Add(new History(local.UserName, local.Addr, newUser.UserName, newUser.Addr));
			});
			//当新登录用户将所有用户更新到聊天列表，并创建对应聊天历史
			connection.On<string>("UpdateList", (usersJson) => {
				IList<User> users = JsonConvert.DeserializeObject<ObservableCollection<User>>(usersJson);
				userView.List.Clear();
				foreach (User user in users)
				{
					if (user.Addr == local.Addr && user.UserName == local.UserName)
						continue;
					userView.List.Add(user);
					Historys.Add(new History(local.UserName, local.Addr, user.UserName, user.Addr));
				}
			});
			//当有用户退出，从聊天列表中删除该用户，并删除对应聊天历史
			connection.On<string>("DeleteUser", (userJson) => {
				User deleteUser = JsonConvert.DeserializeObject<User>(userJson);
				for (int i = userView.List.Count - 1; i >= 0; i--)
				{
					if (userView.List[i].Addr == deleteUser.Addr && userView.List[i].UserName == deleteUser.UserName)
						userView.List.Remove(userView.List[i]);
				}
				for (int i = Historys.Count - 1; i >= 0; i--)
				{
					if (Historys[i].UserAddr == deleteUser.Addr && Historys[i].UserName == deleteUser.UserName)
						Historys.Remove(Historys[i]);
				}
			});
			//建立连接
			Start();
			//登录
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
								string voicePath= System.AppDomain.CurrentDomain.BaseDirectory + "\\record\\" + result.RemoteEndPoint.Address.ToString().Replace(".","") + (new Random()).Next() + ".wav";
								history.ChatHistory.Add(new Message(voicePath, DateTime.Now, false));
								try
								{
									FileStream fs = new FileStream(voicePath, FileMode.OpenOrCreate, FileAccess.Write);
									fs.Write(result.Buffer, 0, result.Buffer.Length);
									fs.Close();
								}
								catch(Exception ex)
								{
									MessageBox.Show(ex.Message);
								}
							}));
						}
					}
				}
			});
		}
		[DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
		public static extern int mciSendString(
		 string lpstrCommand,
		 string lpstrReturnString,
		 int uReturnLength,
		 int hwndCallback
		);
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
				await connection.InvokeAsync("Login",JsonConvert.SerializeObject(local));
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
				await connection.InvokeAsync("Quit", JsonConvert.SerializeObject(local));
				
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
		private void Record_MouseLeftButtonDown(object sender, RoutedEventArgs e)
		{
			mciSendString("close movie", "", 0, 0);
			mciSendString("open new type WAVEAudio alias movie", "", 0, 0);
			mciSendString("record movie", "", 0, 0);
		}
		private void Message_MouseLeftButtonUp(object sender, RoutedEventArgs e)
		{
			Message message=(Message)((Border)sender).DataContext;
			mciSendString("close movie", "", 0, 0);
			string command = "open " + message.ChatPath + " alias aa";
			mciSendString(command, null, 0, 0);
			mciSendString("play aa wait", null, 0, 0);
			mciSendString("close aa", "", 0, 0);
			mciSendString("close movie", "", 0, 0);
		}
		private void Record_MouseLeftButtonUp(object sender, RoutedEventArgs e)
		{
			//
			string selectAddr = ((User)userList.SelectedItem).Addr;
			string selectName = ((User)userList.SelectedItem).UserName;
			DateTime voiceTime = DateTime.Now;
			string voicePath = System.AppDomain.CurrentDomain.BaseDirectory + "record\\"+selectAddr.Replace(".","")+(new Random()).Next()+".wav";
			mciSendString("stop movie", "", 0, 0);
			mciSendString("save movie " + voicePath, "", 0, 0);
			mciSendString("close movie", "", 0, 0);
			Message message = new Message(voicePath, voiceTime, true);
			FileStream fs;
			byte[] sendVoice=null;
			try
			{
				fs= new FileStream(voicePath, FileMode.Open, FileAccess.Read);
				sendVoice = new byte[fs.Length];
				fs.Read(sendVoice, 0, (int)fs.Length);
				fs.Close();
			}
			catch(Exception ex)
			{
				OutputError(ex.Message);
			}
			//byte[] sendByte = Encoding.ASCII.GetBytes(sendString);
			udpClient.SendAsync(sendVoice, sendVoice.Length, selectAddr, 8889);
			foreach (History history in Historys)
			{
				if (history.UserName == selectName && history.UserAddr == selectAddr)
				{
					history.ChatHistory.Add(message);
				}
			}
			//
			
		}
		private void Close_Click(object sender, RoutedEventArgs e)
		{
			End();
			DeleteDir(System.AppDomain.CurrentDomain.BaseDirectory + "record");
			Application.Current.Shutdown();
		}
		private void DeleteDir(string file)
		{
			try
			{
				//去除文件夹和子文件的只读属性
				//去除文件夹的只读属性
				System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
				fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
				//去除文件的只读属性
				System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
				//判断文件夹是否还存在
				if (Directory.Exists(file))
				{
					foreach (string f in Directory.GetFileSystemEntries(file))
					{
						if (File.Exists(f))
						{
							//如果有子文件删除文件
							File.Delete(f);
							MessageBox.Show(f);
						}
						else
						{
							//循环递归删除子文件夹
							DeleteDir(f);
						}
					}
					//删除空文件夹
					Directory.Delete(file);
					MessageBox.Show(file);
				}
			}
			catch (Exception ex) // 异常处理
			{
				MessageBox.Show(ex.Message.ToString());// 异常信息
			}
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
			udpClient.SendAsync(sendByte,sendByte.Length ,selectAddr,8889);
			foreach(History history in Historys)
			{
				if (history.UserName == selectName && history.UserAddr == selectAddr)
				{
					history.ChatHistory.Add(new Message(sendString, DateTime.Now, true));
				}
			}
		}
		private void SelectUser_Change(object sender, RoutedEventArgs e)
		{
			if (userList.SelectedItem == null)
				return;
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
