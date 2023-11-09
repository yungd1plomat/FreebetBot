using FreebetBot.Abstractions;
using FreebetBot.Data;
using FreebetBot.Models;
using FreebetBot.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreebetBot.Core.Handlers.Admin
{
    public class SendHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public SendHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null && 
                   user.LastCommand == Command.InitSend && 
                   user.IsAdmin;
        }

        private async Task SendMessage(IEnumerable<TGUser> users, long fromId, int messageId)
        {
            foreach (var user in users)
            {
                try
                {
                    await Client.CopyMessageAsync(user.Id, fromId, messageId);
                } catch { }
                await Task.Delay(100);
            }
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var users = db.Users.ToList();
            await Task.Factory.StartNew(async () => await SendMessage(users, update.Message.Chat.Id, update.Message.MessageId), TaskCreationOptions.LongRunning);
            await Client.SendTextMessageAsync(user.Id, "Рассылка запущена!");
        }
    }
}
