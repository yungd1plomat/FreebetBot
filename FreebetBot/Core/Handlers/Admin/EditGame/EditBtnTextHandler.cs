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
    public class EditBtnTextHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public EditBtnTextHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return ((update.Message != null && user.LastCommand == Command.EditGameUrl) ||
                   (update.CallbackQuery != null && update.CallbackQuery.Data == "skip:editbtntext")) &&
                   user.IsAdmin &&
                   user.LastMsg != null &&
                   user.Data != null;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            await Client.EditMessageTextAsync(user.Id, (int)user.LastMsg, "Игра отредактирована!");
            if (update.CallbackQuery?.Data == "skip:editbtntext")
            {
                user.LastMsg = null;
                user.Data = null;
                return;
            }
            await Client.DeleteMessageAsync(user.Id, update.Message.MessageId);

            var gameBtnText = update.Message.Text;
            var rawId = user.Data;
            if (!int.TryParse(rawId, out int gameId))
                return;
            var game = db.Games.FirstOrDefault(x => x.Id == gameId);
            game.ButtonText = gameBtnText;
            user.LastMsg = null;
            user.Data = null;
        }
    }
}
