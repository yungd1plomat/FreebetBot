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
    public class CancelHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public CancelHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   update.Message.Text == "Отмена" &&
                   user.IsAdmin;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            user.LastMsg = null;
            user.Data = null;
            await Client.SendTextMessageAsync(user.Id, "Действие отменено");
        }
    }
}
