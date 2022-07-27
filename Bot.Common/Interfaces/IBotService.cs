
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;

namespace Bot.Common.Interfaces
{
    public interface IBotService
    {
        Task<string> StartConversation(bool isDefault = true);
        Task<ResourceResponse> SendMessage(string message, string conversationId = null);

        event EventHandler<ActivityEventArgs> ActivitySent;
        event EventHandler<ActivityEventArgs> ActivityReceived;
        event EventHandler<List<Activity>> BotActivitiesReceived;
    }

    public class ActivityEventArgs : EventArgs
    {
        public Activity Activity;
    }
}
