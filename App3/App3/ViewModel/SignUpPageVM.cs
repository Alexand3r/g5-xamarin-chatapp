using App3.Model;
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
   public class SignUpPageVM : INotifyPropertyChanged
    {
        public ICommand SignUpButtonCommand { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public SignUpPageVM()
        {

            SignUpButtonCommand = new Command(SignUp);
        }

      
        private async void SignUp(object obj)
        {
            var client = new RestClient(App.ChatServer);
            var request = new RestRequest("createuser", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", new User { username = username, password = password }, ParameterType.RequestBody);
            var queryResult = await client.Execute(request);
            if (queryResult != null)
            {
                
                string json = queryResult.Content;
                bool a = JsonConvert.DeserializeObject<bool>(json);
                if (a)
                { await Application.Current.MainPage.DisplayAlert("Sign Up", "Success!", "OK"); }
                else
                { await Application.Current.MainPage.DisplayAlert("Sign Up", "Failed!", "OK"); }
                }
            }
       
        //not used 
        //TODO: Clean the forms after registering, doable but not mandatory
        #region OnPC
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }


}

