using Android.Content.PM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Xamarin.Forms;

namespace VoluntHere
{

    public partial class MainPage : ContentPage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Task _task { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private void getHelp(object sender, EventArgs e)
        {

            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            DisplayAlert("Done!", "Help is on the way!", "OK");
            IPAddress ipAddr = IPAddress.Parse("192.168.1.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
            Socket server = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to server...");
            server.Connect(localEndPoint);
            Console.WriteLine("Connected");
            Console.WriteLine("Connected");
            //Send message

            //location

            byte[] messageSent = Encoding.ASCII.GetBytes("HELP$1$2");
            int byteSent = server.Send(messageSent);
        }

        [Obsolete]
        private void Button_Click_Register(object sender, EventArgs e)
        {
            App.Current.MainPage = new REGISTER(UpdateDet);

        }

        private void handle_data(string data)
        {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                string[] newdata = data.Split('$');
                if (newdata[0] == "Help")
                {
                    DisplayAlert("", "Help needed from" + Id, "OK");
                }
            });
        }

        private void startListening()
        {
            IPAddress ipAddr = new IPAddress(0x00000000);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 600);
            try
            {

                // Create a Socket that will use Tcp protocol
                Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                listener.Listen(10);


                // Incoming data from the client.
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    // Incoming data from the client.
                    data = null;
                    bytes = null;
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    handle_data(data);

                }
            }
            catch { }
        }
        public static int GetRandFromServer()
        {
            IPAddress ipAddr = new IPAddress(0x0100007F); //כתובת 
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
            Socket sender = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to server...");
            sender.Connect(localEndPoint);
            Console.WriteLine("Connected");
            //Send message

            //location

            byte[] messageSent = Encoding.ASCII.GetBytes("GET RAND");
            int byteSent = sender.Send(messageSent);


            // Data buffer
            byte[] messageReceived = new byte[1024];

            // We receive the message using
            // the method Receive(). This
            // method returns number of bytes
            // received, that we'll use to
            // convert them to string
            int byteRecv = sender.Receive(messageReceived);
            Console.WriteLine("Message from Server -> {0}",
                  Encoding.ASCII.GetString(messageReceived,
                                             0, byteRecv));
            int RandNum;
            int.TryParse(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), out RandNum);
            return RandNum;


            //            private WithEvents watcher As GeoCoordinateWatcher
            //            public Sub GetLocationDataEvent()
            //            watcher = New System.Device.Location.GeoCoordinateWatcher()
            //            AddHandler watcher.PositionChanged, AddressOf watcher_PositionChanged
            //            watcher.Start()
            //            End Sub

            //Private Sub watcher_PositionChanged(ByVal sender As Object, ByVal e As GeoPositionChangedEventArgs(Of GeoCoordinate))
            //    MsgBox(e.Position.Location.Latitude.ToString & ", " & _
            //           e.Position.Location.Longitude.ToString)
            //    ' Stop receiving updates after the first one.
            //    watcher.Stop()
            //End Sub
        }

        [Obsolete]
        public void UpdateDet(int Id, string name)
        {
            ID = Id;
            Name = name;
            (new Thread(() =>
            {
                IPAddress ipAddr = IPAddress.Parse("192.168.1.1");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
                Socket sender = new Socket(ipAddr.AddressFamily,
                       SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Connecting to server...");
                sender.Connect(localEndPoint);
                Console.WriteLine("Connected");
                //Send message

                //location
                string IP;
                string hostName;

                while (true)
                {
                    Thread.Sleep(3000);
                    hostName = Dns.GetHostName();
                    Console.WriteLine(hostName);
                    // Get the IP from GetHostByName method of dns class.
                    IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                    Console.WriteLine("IP Address is : " + IP);
                    String str = "UPD" + "$" + Id + "$" + name + "$" + IP + "$1$2";/////////

                    byte[] messageSent = Encoding.ASCII.GetBytes(str);
                    try
                    {
                        int byteSent = sender.Send(messageSent);

                    }
                    catch
                    {

                    }
                }
            })).Start();
            _task = Task.Run(() =>
            {
                startListening();
            });
        }


    }
}
