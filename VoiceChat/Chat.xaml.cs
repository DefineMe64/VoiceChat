using System;
using System.Collections.Generic;
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
		public Chat()
		{
			InitializeComponent();
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
	}
}
