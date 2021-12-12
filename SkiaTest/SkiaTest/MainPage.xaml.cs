using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaTest
{
	public partial class MainPage : ContentPage
	{

		ServerMsg svrMsg;
		WebSocketReceiveResult rcvResult;


		public class ServerMsg
		{
			public char id;
			public float level;
			public char snsrstate;
			public char p1state;
			public char p1pwr;
			public char p2state;
			public char p2pwr;
			public char automatic;
			public char bypass;
		}




		public MainPage()
		{
			InitializeComponent();
			connect();
			uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

			
		}
		TaskScheduler uiScheduler;

		async Task AnimateProgress(ServerMsg progress)
		{
			if (progress.level == GaugeControl.Val)
			{
				return;
			}
			if (progress.level <= GaugeControl.Val)
			{
				for (int i = (int)GaugeControl.Val; i >= progress.level; i--)
				{
					GaugeControl.Val = i;
					await Task.Delay(5);
				}
			}
			else
			{
				for (int i = (int)GaugeControl.Val; i <= progress.level; i++)
				{
					GaugeControl.Val = i;
					await Task.Delay(5);
				}
			}


			GaugeControl.SensorState = progress.snsrstate;
			//GaugeControl.PMP1State = progress.p1state;
			//GaugeControl.PMP2State = progress.p2state;
			//GaugeControl.P1PWRState = progress.p1pwr;
			//GaugeControl.P2PWRState = progress.p2pwr;

		}

		private async void connect()
		{
			ClientWebSocket socket = new ClientWebSocket();
			Uri uri = new Uri("ws://192.168.1.200:81");
			var cts = new CancellationTokenSource();
			await socket.ConnectAsync(uri, cts.Token);

			Console.WriteLine(socket.State);
			await Task.Factory.StartNew(
				async () =>
				{
					var rcvBytes = new byte[128];
					System.ArraySegment<byte> rcvBuffer = new System.ArraySegment<byte>(rcvBytes);
					while (true)
					{
						try
						{
							rcvResult = await socket.ReceiveAsync(rcvBuffer, cts.Token);
						}
						catch (Exception ex)
						{
							Console.WriteLine("...error in receive...");
						}
						

						byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
						string rcvMsg = Encoding.UTF8.GetString(msgBytes);
						Console.WriteLine("Received: {0}", rcvMsg);

						try
						{
							svrMsg = JsonConvert.DeserializeObject<ServerMsg>(rcvMsg);
							Console.WriteLine("level = " + svrMsg.level);
							Device.BeginInvokeOnMainThread(async () => await AnimateProgress(svrMsg));

						}
						catch (Exception ex)
						{
							Console.WriteLine("Exception:"+ex.Message);
						}
					}

				}, cts.Token, TaskCreationOptions.LongRunning, uiScheduler);

			/*			while (true)
						{
							var message = Console.ReadLine();
							if (message == "Bye")
							{
								cts.Cancel();
								return;
							}
							byte[] sendBytes = Encoding.UTF8.GetBytes(message);
							var sendBuffer = new ArraySegment<byte>(sendBytes);
							await
								socket.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true,
												 cancellationToken: cts.Token);
						}
			*/
		}
	}
}
