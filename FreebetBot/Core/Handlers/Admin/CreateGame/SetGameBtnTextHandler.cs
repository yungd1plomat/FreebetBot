using FreebetBot.Abstractions;
using FreebetBot.Data;
using FreebetBot.Models;
using FreebetBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Game = FreebetBot.Models.Game;
using FreebetBot.Core.Utils;

namespace FreebetBot.Core.Handlers.Admin
{
    public class SetGameBtnTextHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public SetGameBtnTextHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   user.LastCommand == Command.SetGameUrl &&
                   user.IsAdmin &&
                   user.LastMsg != null;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var game = JsonSerializer.Deserialize<Game>(user.Data);
            var buttonText = update.Message.Text;
            game.ButtonText = buttonText;
            var json = JsonSerializer.Serialize(game);
            var text = $"Короткое название: {game.ShortName}\nСсылка: {game.Url}\nТекст кнопки: {game.ButtonText}\n\nПодтвердите действие:";
            await Client.DeleteMessageAsync(user.Id, update.Message.MessageId);
            var keyboard = KeyboardManager.CreateGameButtons();
            var message = await Client.EditMessageTextAsync(user.Id, (int)user.LastMsg, text, replyMarkup: keyboard);
            user.LastMsg = message.MessageId;
            user.Data = json;
        }
    }
}
