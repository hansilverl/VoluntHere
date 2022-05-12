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


        }

    }


}
