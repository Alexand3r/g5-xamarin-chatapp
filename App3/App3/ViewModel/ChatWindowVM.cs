using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Windows.Input;
using System.ComponentModel;
using System;
using App3.Model;

namespace App3.ViewModel
{
    class ChatWindowVM :INotifyPropertyChanged
    {
        //TODO:
        /*
        
            1.Generalize the user into ONE object and stop creating multiple User objects for requests
            2.OnDestroy() Call logout() as OnDissapearing is not enough, if it does not call LogOut() the User will not be able to log back in
            (Temporary workaround call logout() while logging in)
            3.Implement a device Key generator for each user/device for improved security.
            (Currently you can send a message on behalf of someone else who is logged in with a "home made" POST request)
            4.Auto Scroll down <- This is annoying
            5. !! Try Catch for each HTTP Request !!
            6.Implement the Connected users List
            7.Timer on the Server and on the client(in ChatWindowVM) (~every 5 mins?) for Log Out due timeout
            8.A friendly "Insert Server IP address" entry instead of a hardcoded string.
            9. A lot of tweaking and organizing.
        */
        

        private ObservableCollection<Message> _messageList = new ObservableCollection<Message>();
        public ObservableCollection<Message> messageList
        {
            get
            {
                return _messageList;
            }
            set
            {
                _messageList = value;
                OnPropertyChanged("messageList");
            }
        }
        private string _username;
        public string username
        {
            get { return _username; }

            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("username");
                }
            }
        }
        private string _message;
        public string message
        {
            get { return _message; }

            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("message");
                }
            }
        }
        public ICommand SendButtonCMD{ get; set; }
        private static ChatWindowVM cwVM;
        private string _newMessage;
        public string newMessage
        {
            get { return _newMessage; }
            set {
                if (_newMessage != value)
                {
                    _newMessage = value;
                    OnPropertyChanged("newMessage");
                }
            } }

        private bool loggedOUT = false;


        public ChatWindowVM()
        {
            //TODO: Create an User object instead of juggling with a string
            this.username = App.username;
            cwVM = this;
            getAllMessages();
            startTimer();
            //Content = listView;
            MessagingCenter.Subscribe<ChatWindowVM>(this, "OnDisappearing", (sender) => {
                this.OnDisappearing();
            });
            SendButtonCMD = new Command(SendMessage);

        }

        private void OnDisappearing()
        {
            loggedOUT = true;
            logout();
        }

        private static void startTimer()
        {
            //Device.StartTimer(TimeSpan.FromSeconds(5), TimerTick);
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (!cwVM.loggedOUT)
                {
                    TimerTick(); 
                    return true;
                }
                return false;
            });
        }

        private static bool TimerTick()
        {
            cwVM.checkNewMessages(cwVM.getMsgId());
            return true;
        }

        private async void SendMessage()
        {
           
            var client = new RestClient(App.ChatServer);
            var request = new RestRequest("sendmessage", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", new Message { username = username, message = newMessage }, ParameterType.RequestBody);
           
            var queryResult = await client.Execute(request);
            newMessage = "";
            checkNewMessages(getMsgId());
           
        }
        //Not implemented yet
        public void getusers()
        { }
        public async void getAllMessages()
        {
            List<Message> msgs = null;
            var client = new RestClient(App.ChatServer);
            var request = new RestRequest("getmessages", Method.GET);
            var queryResult = await client.Execute(request);
            if (queryResult != null)
            {
                string json = queryResult.Content;
                msgs = JsonConvert.DeserializeObject<List<Message>>(json);
                foreach (Message m in msgs)
                {
                    messageList.Add(m);
                }
            }
           
        }


      
        public async void checkNewMessages(int id)
        {
            List<Message> msgs = null;
            var client = new RestClient(App.ChatServer);
            var request = new RestRequest("getnewmessages?id="+id, Method.GET);
            var queryResult = await client.Execute(request);
            if (queryResult != null)
            {
                string json = queryResult.Content;
                msgs = JsonConvert.DeserializeObject<List<Message>>(json);
                foreach (Message m in msgs)
                {
                    messageList.Add(m);
                }
            }
           
        }

        public async void logout()
        {
            
            var client = new RestClient(App.ChatServer);
            var request = new RestRequest("logout", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", new User { username = username }, ParameterType.RequestBody);
            var queryResult = await client.Execute(request);
            if (queryResult != null)
            {

                string json = queryResult.Content;
                bool success = JsonConvert.DeserializeObject<bool>(json);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Status", "Logged out!", "OK");
                }

            }
        }
        private int getMsgId()
        { return messageList.Count-1; }
       

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
