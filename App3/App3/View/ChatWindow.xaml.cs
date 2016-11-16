using App3.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App3.View
{
    public partial class ChatWindow : CarouselPage
    {
       
        public ChatWindow() : base ()
        {
            
            InitializeComponent();
            //??For some reason, XAML Binding does not work if the Binding is not initialized here
            this.BindingContext = new ChatWindowVM();

            base.Disappearing += (object sender, EventArgs e) => {
                MessagingCenter.Send((ChatWindowVM)this.BindingContext, "OnDisappearing");
            };
            

        }

      
    }
}
