using FreebetBot.Abstractions;
using FreebetBot.Data;
using FreebetBot.Models;
using FreebetBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Game = FreebetBot.Models.Game;
using System.Threading.Tasks;

namespace FreebetBot.Core.Handlers.Admin
{
    public class SetGameUrlHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public SetGameUrlHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   user.LastCommand == Command.SetGameName &&
                   user.IsAdmin &&
                   user.LastMsg != null &&
                   user.Data != null;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var game = JsonSerializer.Deserialize<Game>(user.Data);
            var url = update.Message.Text;
            game.Url = url;
            var json = JsonSerializer.Serialize(game);
            await Client.DeleteMessageAsync(user.Id, update.Message.MessageId);
            var message = await Client.EditMessageTextAsync(user.Id, (int)user.LastMsg, "Введите текст кнопки: ");
            user.LastMsg = message.MessageId;
            user.Data = json;
        }
    }
}
