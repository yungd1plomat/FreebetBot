using FreebetBot.Abstractions;
using FreebetBot.Data;
using FreebetBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreebetBot.Core.Handlers.Admin
{
    public class InitSendHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public InitSendHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   update.Message.Text == "Рассылка" &&
                   user.IsAdmin;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            await Client.SendTextMessageAsync(user.Id, "Пришлите сообщение для рассылки");
        }
    }
}
