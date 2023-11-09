using FreebetBot.Data;
using FreebetBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace FreebetBot.Abstractions
{
    /// <summary>
    /// Обработчик определенной команды
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        ITelegramBotClient Client { get; set; }

        /// <summary>
        /// Проверяем соответствует ли объект сообщения данной команде
        /// </summary>
        /// <param name="update">Событие (обновление)</param>
        /// <param name="user">Пользователь</param>
        /// <param name="db">Контекст базы данных</param>
        bool IsMatch(Update update, TGUser user, AppDb db);

        /// <summary>
        /// Обрабатываем данное сообщение (команду)
        /// </summary>
        /// <param name="update">Событие (обновление)</param>
        /// <param name="user">Пользователь</param>
        /// <param name="db">Контекст базы данных</param>
        Task Handle(Update update, TGUser user, AppDb db);
    }
}
