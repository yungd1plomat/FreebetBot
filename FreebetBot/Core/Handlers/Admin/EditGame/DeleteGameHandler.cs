using FreebetBot.Abstractions;
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
    public class DeleteGameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public DeleteGameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.CallbackQuery != null &&
                   user.IsAdmin &&
                   update.CallbackQuery.Data.StartsWith("delete:");
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var id = update.CallbackQuery.Data.Split(':').LastOrDefault();
            if (!int.TryParse(id, out int gameId))
                return;
            var game = await db.Games.FirstOrDefaultAsync(x => x.Id == gameId);
            if (game == null)
            {
                await Client.SendTextMessageAsync(user.Id, $"Игра не найдена!");
                return;
            }
            db.Games.Remove(game);
            await Client.DeleteMessageAsync(user.Id, update.CallbackQuery.Message.MessageId);
            await Client.SendTextMessageAsync(user.Id, $"Игра {gameId} удалена!");
        }
    }
}
