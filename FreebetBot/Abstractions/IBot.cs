using FreebetBot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreebetBot.Abstractions
{
    public interface IBot
    {
        /// <summary>
        /// Запуск поллинга
        /// </summary>
        Task Start();

        /// <summary>
        /// Обработчики событий
        /// </summary>
        IDictionary<Command, IHandler> Handlers { get; set; }

        /// <summary>
        /// Обработка событый
        /// </summary>
        /// <param name="botClient">Клиент телеграма</param>
        /// <param name="update">Событие (обновление)</param>
        /// <param name="cancellationToken"></param>
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

        /// <summary>
        /// Обработка ошибок
        /// </summary>
        /// <param name="botClient">Клиент телеграма</param>
        /// <param name="exception">Ошибка</param>
        /// <param name="cancellationToken"></param>
        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}
