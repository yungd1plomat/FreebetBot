using FreebetBot.Abstractions;
using FreebetBot.Core.Utils;
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
    public class AdminMenuHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public AdminMenuHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   update.Message.Text == "/admin" &&
                   user.IsAdmin;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var keyboard = KeyboardManager.AdminButtons();
            await Client.SendTextMessageAsync(user.Id, "Выдано админское меню", replyMarkup: keyboard);
        }
    }
}
