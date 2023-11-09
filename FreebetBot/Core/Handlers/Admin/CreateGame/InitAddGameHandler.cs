using FreebetBot.Abstractions;
using FreebetBot.Data;
using FreebetBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreebetBot.Core.Handlers.Admin
{
    public class InitAddGameHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public InitAddGameHandler(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   update.Message.Text == "Добавить игру" &&
                   user.IsAdmin;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {
            var message = await Client.SendTextMessageAsync(user.Id, "Введите название игры (short name): ");
            user.LastMsg = message.MessageId;
        }
    }
}
