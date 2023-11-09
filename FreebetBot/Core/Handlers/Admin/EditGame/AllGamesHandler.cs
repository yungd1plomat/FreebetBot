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
    public class AllGamesHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public AllGamesHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   update.Message.Text == "Все игры" &&
                   user.IsAdmin;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var games = db.Games.ToList();
            foreach (var game in games)
            {
                var info = $"Id: {game.Id}\nНазвание: {game.ShortName}\nСсылка: {game.Url}\nТекст кнопки: {game.ButtonText}\nОсновное: {game.IsMain}\n\nДействие: ";
                var keyboard = KeyboardManager.GameButtons(game);
                await Client.SendTextMessageAsync(user.Id, info, replyMarkup: keyboard);
            }
        }
    }
}
