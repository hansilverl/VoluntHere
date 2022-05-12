using Android.Content.PM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Page Page1 = new Page();
            App.Current.MainPage = new Page1();


        }

    }


}
