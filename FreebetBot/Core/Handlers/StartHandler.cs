using FreebetBot.Abstractions;
using FreebetBot.Core.Utils;
using FreebetBot.Data;
using FreebetBot.Models;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreebetBot.Core.Handlers
{
    public class StartHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public StartHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return true;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var game = db.Games.FirstOrDefault(x => x.IsMain);
            if (game is null)
                return;
            var keyboard = KeyboardManager.BuildGameButtons(game.ButtonText);
            await Client.SendGameAsync(user.Id, game.ShortName, replyMarkup: keyboard);
        }
    }
}
