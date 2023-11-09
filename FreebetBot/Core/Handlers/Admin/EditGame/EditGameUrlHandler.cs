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
    public class EditGameUrlHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public EditGameUrlHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return ((update.Message != null && user.LastCommand == Command.EditGameName) ||
                   (update.CallbackQuery != null && update.CallbackQuery.Data == "skip:editurl")) &&
                   user.IsAdmin &&
                   user.LastMsg != null &&
                   user.Data != null;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var keyboard = KeyboardManager.SkipButton("editbtntext");
            await Client.EditMessageTextAsync(user.Id, (int)user.LastMsg, "Введите новый текст кнопки: ", replyMarkup: keyboard);
            if (update.CallbackQuery?.Data == "skip:editurl")
                return;
            if (update.Message?.MessageId != null)
                await Client.DeleteMessageAsync(user.Id, update.Message.MessageId);
            var gameUrl = update.Message.Text;
            var rawId = user.Data;
            if (!int.TryParse(rawId, out int gameId))
                return;
            var game = db.Games.FirstOrDefault(x => x.Id == gameId);
            game.Url = gameUrl;
        }
    }
}
