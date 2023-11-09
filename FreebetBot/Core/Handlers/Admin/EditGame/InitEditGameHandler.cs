using FreebetBot.Abstractions;
using FreebetBot.Core.Utils;
using FreebetBot.Data;
using FreebetBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreebetBot.Core.Handlers.Admin
{
    public class InitEditGameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public InitEditGameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.CallbackQuery != null &&
                   user.IsAdmin &&
                   update.CallbackQuery.Data.StartsWith("edit:");
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var id = update.CallbackQuery.Data.Split(':').LastOrDefault();
            if (!int.TryParse(id, out int gameId))
                return;
            var game = db.Games.FirstOrDefaultAsync(x => x.Id == gameId);
            if (game == null)
            {
                await Client.SendTextMessageAsync(user.Id, "Игры не существует");
                return;
            }
            var keyboard = KeyboardManager.SkipButton("editname");
            var message = await Client.SendTextMessageAsync(user.Id, "Введите новое название игры (short name): ", replyMarkup: keyboard);
            user.LastMsg = message.MessageId;
            user.Data = id;
        }
    }
}
