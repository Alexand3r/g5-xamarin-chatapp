using App3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App3.View
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false); // Hide the annoying titlebar
          
        }


    }
}
