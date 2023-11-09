using FreebetBot.Abstractions;
using FreebetBot.Data;
using FreebetBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreebetBot.Core.Handlers
{
    public class OpenGameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public OpenGameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.CallbackQuery != null &&
                   update.CallbackQuery.IsGameQuery &&
                   db.Games.Any(x => x.ShortName == update.CallbackQuery.GameShortName);
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var gameShortName = update.CallbackQuery.GameShortName;
            var game = await db.Games.FirstAsync(x => x.ShortName == gameShortName);
            await Client.AnswerCallbackQueryAsync(update.CallbackQuery.Id, null, false, game.Url);
        }
    }
}
