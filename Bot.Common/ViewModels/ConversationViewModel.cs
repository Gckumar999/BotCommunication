using Bot.Common.Interfaces;
using Bot.Portable.Common.ViewModels;
using Microsoft.Bot.Connector.DirectLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Botframework.Xamarin.ViewModels
{
    public class ConversationViewModel : BaseViewModel
    {
        private IBotService _botService;
        private string _conversationId;
        private ICommand _sendMessageCommand;
        private string _message;
        private bool _isSendButtonEnabled;
        private Activity _previousSentActivity { get; set; } = new Activity();

        public List<Activity> Messages { get; set; } = new List<Activity>();
       // public ICommand SendMessageCommand => _sendMessageCommand ?? (_sendMessageCommand = new Command(async (o) => await OnSendMessage(o)));


        public bool IsSendButtonEnabled
        {
            get { return _isSendButtonEnabled; }
            set { _isSendButtonEnabled = value;
                OnPropertyChanged("IsSendButtonEnabled");
                if (IsSendButtonEnabled)
                {
                    // inform the UI
                }
                else
                {
                    // inform the UI
                }
            }
        }
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
                if (_message != string.Empty)
                {
                    IsSendButtonEnabled = true;
                }
                else
                {
                    IsSendButtonEnabled = false;
                }
            }
        }

        //initiaste the conversation 
        public ConversationViewModel(IBotService botService)
        {
            _botService = botService;
            Task.Factory.StartNew(async () => _conversationId = await _botService.StartConversation());
            _botService.ActivityReceived += ActivityReceived;
            _botService.ActivitySent += ActivitySent;


        }

        //Add the message to loca list as soon as the message sent to bot
        private void ActivitySent(object sender, ActivityEventArgs e)
        {
            _previousSentActivity = e.Activity;
            Messages.Add(_previousSentActivity);
        }


        //On receiving the new message
        private void ActivityReceived(object sender, ActivityEventArgs e)
        {
            var listText = new List<string>();
        var activty=  JsonConvert.SerializeObject(e.Activity);
            if (string.Equals(e.Activity.Type, "message")) // only add messages to our list
            {
                Messages.Add(e.Activity);
            }

           // Messages = Messages.OrderBy(o=>o.Timestamp).ToList();
            foreach (var m in Messages)
            {
                listText.Add(m.Text);
            }
            JsonConvert.SerializeObject(listText);
        }


        // on sending new message 
        public async Task OnSendMessage(object obj, bool isAttachmentIncluded = false)
        {
            try
            {
                var currentMessage = Message;
                // Clear entry field after sending
                Message = string.Empty;

                var activityId = await _botService.SendMessage(currentMessage);

                if (activityId != null || activityId.Id != null || activityId.Id != "")
                {
                    // Add the id of the recently sent activity to local message list
                    var indexOfPreviouslySentActivity = Messages.IndexOf(_previousSentActivity);
                    var prevSentActivity = _previousSentActivity;
                    prevSentActivity.Id = activityId.Id;
                    JsonConvert.SerializeObject(Messages);
                    Messages.Insert(indexOfPreviouslySentActivity, prevSentActivity);
                    JsonConvert.SerializeObject(Messages);
                    Messages.Remove(_previousSentActivity);
                    JsonConvert.SerializeObject(Messages);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
