using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Connector;
using System.Diagnostics;

namespace TwitterStreamEventBot.Dialogs
{
    [Serializable]
    public class NotificationDialog : IDialog<object>
    {
        public NotificationDialog(string test)
        {
            Debug.WriteLine("it worked");
        }
        public async Task StartAsync(IDialogContext context)
        {

            Task.Run(() => Test(context));
            //context.Wait(StartNotificationsAsync);
        }

        private static void Test(IDialogContext context)
        {
            context.PostAsync("test");
        }

        public async Task StartNotificationsAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

           /* var message = await argument;
            await context.PostAsync("You said started a new confo");
            context.Wait(StartNotificationsAsync);
            */
        }

    }
}