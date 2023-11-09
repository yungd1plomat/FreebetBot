using FreebetBot.Abstractions;
using FreebetBot.Data;
using FreebetBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Game = FreebetBot.Models.Game;
using FreebetBot.Models.Enums;

namespace FreebetBot.Core.Handlers.Admin
{
    public class CreateGameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public CreateGameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.CallbackQuery != null &&
                   user.LastCommand == Command.SetGameButtonText &&
                   user.IsAdmin &&
                   user.LastMsg != null &&
                   user.Data != null &&
                   update.CallbackQuery.Data == "creategame";
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var game = JsonSerializer.Deserialize<Game>(user.Data);
            await db.Games.AddAsync(game);
            await Client.EditMessageTextAsync(user.Id, (int)user.LastMsg, "Игра создана!");
            user.LastMsg = null;
            user.Data = null;
        }
    }
}
