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
    public class SetMainGameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public SetMainGameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.CallbackQuery != null &&
                   user.IsAdmin &&
                   update.CallbackQuery.Data.StartsWith("main:");
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var id = update.CallbackQuery.Data.Split(':').LastOrDefault();
            if (!int.TryParse(id, out int gameId))
                return;
            var allGames = await db.Games.ToListAsync();
            foreach (var game in allGames)
            {
                game.IsMain = game.Id == gameId;
            }
            await Client.SendTextMessageAsync(user.Id, $"Игра {gameId} установлена по умолчанию!");
        }
    }
}
