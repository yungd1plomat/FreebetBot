using FreebetBot.Abstractions;
using FreebetBot.Core.Utils;
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
    public class EditGameNameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public EditGameNameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return ((update.Message != null && user.LastCommand == Command.InitEditGame) ||
                   (update.CallbackQuery != null && update.CallbackQuery.Data == "skip:editname")) &&
                   user.IsAdmin &&
                   user.Data != null &&
                   user.LastMsg != null;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var keyboard = KeyboardManager.SkipButton("editurl");
            await Client.EditMessageTextAsync(user.Id, (int)user.LastMsg, "Введите новую ссылку: ", replyMarkup: keyboard);
            if (update.CallbackQuery?.Data == "skip:editname")
                return;
            await Client.DeleteMessageAsync(user.Id, update.Message.MessageId);
            var gameName = update.Message.Text;
            var rawId = user.Data;
            if (!int.TryParse(rawId, out int gameId))
                return;
            var game = db.Games.FirstOrDefault(x => x.Id == gameId);
            game.ShortName = gameName;

        }
    }
}
