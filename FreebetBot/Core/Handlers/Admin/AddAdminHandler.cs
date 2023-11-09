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
    public class AddAdminHandler : IHandler
    {
        public ITelegramBotClient Client { get; set; }

        public AddAdminHandler(ITelegramBotClient client)
        {
            Client = client;
        }
        public bool IsMatch(Update update, TGUser user, AppDb db)
        {
            return update.Message != null &&
                   (update.Message.Text.StartsWith("/addadmin") ||
                   update.Message.Text.StartsWith("/removeadmin")) &&
                   user.IsAdmin;
        }

        public async Task Handle(Update update, TGUser user, AppDb db)
        {   
            bool isAddAdmin = update.Message.Text.StartsWith("/addadmin");
            var rawId = update.Message.Text.Replace("/addadmin", "")
                                          .Replace("/removeadmin", "");
            if (!long.TryParse(rawId, out long id))
            {
                await Client.SendTextMessageAsync(user.Id, "Некорректный Id");
                throw new InvalidOperationException("Invalid id");
            }
            var needUser = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (needUser == default)
            {
                await Client.SendTextMessageAsync(user.Id, "Пользователь не найден");
                throw new InvalidOperationException("User not found");
            }
            needUser.IsAdmin = isAddAdmin;
            await Client.SendTextMessageAsync(user.Id, $"Пользователь {rawId} админ: {isAddAdmin}");
            await Client.SendTextMessageAsync(needUser.Id, $"Права администратора: {isAddAdmin}");
        }
    }
}
