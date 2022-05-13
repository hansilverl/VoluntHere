using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoluntHere
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class REGISTER : ContentPage
    {
        public string Name { get; set; }
        public int Lat { get; set; }
        public int Lon { get; set; }
        public int ID;
        public Action<int,string> Func { get; set; }

        public REGISTER(Action<int,string> func)
        {
            InitializeComponent();
            Func = func;
        }

        private void Button_Click_Submit(object sender, EventArgs e)
        {
            Name = entry1.Text;
            Lat = Convert.ToInt16(entry2.Text);
            Lon = Convert.ToInt16(entry3.Text);

            IPAddress ipAddr = IPAddress.Parse("192.168.1.1"); 
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
            Socket server = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to server...");
            server.Connect(localEndPoint);
            Console.WriteLine("Connected");
            //Send message


            string hostName = Dns.GetHostName();
            Console.WriteLine(hostName);

            // Get the IP from GetHostByName method of dns class.
            string IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            Console.WriteLine("IP Address is : " + IP);

            String str = "REG" + "$" + Name + "$" + IP + "$" + Lat + "$" + Lon + "\n";

            //location

            byte[] messageSent = Encoding.ASCII.GetBytes(str);
            int byteSent = server.Send(messageSent);

            byte[] messageReceived = new byte[1024];

            // We receive the message using
            // the method Receive(). This
            // method returns number of bytes
            // received, that we'll use to
            // convert them to string
            int byteRecv = server.Receive(messageReceived);
            Console.WriteLine("Message from Server -> {0}",
                  Encoding.ASCII.GetString(messageReceived,
                                             0, byteRecv));

            int.TryParse(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), out ID);
            //Close();

            Func(ID, Name);
        }
    }
}