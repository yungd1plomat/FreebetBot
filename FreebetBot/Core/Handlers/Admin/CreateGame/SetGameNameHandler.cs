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
    public class SetGameNameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public SetGameNameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   user.LastCommand == Command.InitAddGame &&
                   user.IsAdmin &&
                   user.LastMsg != null;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var name = update.Message.Text;
            var data = new Game()
            {
                ShortName = name,
            };
            var json = JsonSerializer.Serialize(data);
            await Client.DeleteMessageAsync(user.Id, update.Message.MessageId);
            var message = await Client.EditMessageTextAsync(user.Id, (int)user.LastMsg, "Введите ссылку: ");
            user.LastMsg = message.MessageId;
            user.Data = json;
        }
    }
}
