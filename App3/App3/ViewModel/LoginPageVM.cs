
using App3.Model;
using App3.View;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App3.ViewModel
{
    public class LoginPageVM : INotifyPropertyChanged
    {
        public string username { get; set; }
        public string password { get; set; }
        public ICommand LoginButtonCommand { get; set; }
        public ICommand SignUpButtonCommand { get; set; }
        public LoginPageVM()
        {
 
            LoginButtonCommand = new Command(Login);
            SignUpButtonCommand = new Command(SignUp);
         
        }

        private async void SignUp()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignUpPage());
        }

        public async void Login()
        {
            var client = new RestClient(App.ChatServer);
            var request = new RestRequest("login", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", new User { username = username, password = password }, ParameterType.RequestBody);
            var queryResult = await client.Execute(request);
            if(queryResult!=null)
            {

                string json = queryResult.Content;
                bool success = JsonConvert.DeserializeObject<bool>(json);
                if (success)
                {
                    App.username = username;
                    await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ChatWindow());
                }
                else
                { await Application.Current.MainPage.DisplayAlert("Login", "Failed! Username or Password is wrong or username doesn't exist", "OK"); }
            }
        
        }
        //not used
        //TODO: Clean the forms after each attempt to login
        #region ONPC
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
