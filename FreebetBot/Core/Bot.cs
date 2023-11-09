using FreebetBot.Abstractions;
using FreebetBot.Core.Handlers;
using FreebetBot.Core.Handlers.Admin;
using FreebetBot.Data;
using FreebetBot.Models;
using FreebetBot.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FreebetBot.Core
{
    public class Bot : IBot
    {
        public IDictionary<Command, IHandler> Handlers { get; set; }

        private readonly ITelegramBotClient _client;

        private readonly AppDb _database;

        public Bot(string token) 
        {
            _client = new TelegramBotClient(token);
            _database = new AppDb();
            Handlers = new Dictionary<Command, IHandler>()
            {
                { Command.Cancel, new CancelHandler(_client) },
                { Command.AdminMenu, new AdminMenuHandler(_client) },
                { Command.InitAddGame, new InitAddGameHandler(_client) },
                { Command.SetGameName, new SetGameNameHandler(_client) },
                { Command.SetGameUrl, new SetGameUrlHandler(_client) },
                { Command.SetGameButtonText, new SetGameBtnTextHandler(_client) },
                { Command.CreateGame, new CreateGameHandler(_client) },
                { Command.AllGamesButton, new AllGamesHandler(_client) },
                { Command.OpenGame, new OpenGameHandler(_client) },
                { Command.AddRemoveAdmin, new AddAdminHandler(_client) },
                { Command.InitEditGame, new InitEditGameHandler(_client) },
                { Command.EditGameName, new EditGameNameHandler(_client) },
                { Command.EditGameUrl, new EditGameUrlHandler(_client) },
                { Command.EditGameButtonText, new EditBtnTextHandler(_client) },
                { Command.DeleteGame, new DeleteGameHandler(_client) },
                { Command.SetMainGame, new SetMainGameHandler(_client) },
                { Command.InitSend, new InitSendHandler(_client) },
                { Command.SendMessages, new SendHandler(_client) },
                { Command.Start, new StartHandler(_client) },
            };
        }

        public Task Start()
        {
            _client.StartReceiving(updateHandler: HandleUpdateAsync,
                                   pollingErrorHandler: HandlePollingErrorAsync);
            return Task.CompletedTask;
        }

        public async Task<TGUser> Register(long chatId)
        {
            var user = await _database.Users.FirstOrDefaultAsync(x => x.Id == chatId);
            if (user == default)
            {
                user = new TGUser(chatId, false);
                await _database.Users.AddAsync(user);
                await _database.SaveChangesAsync();
            }
            return user;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            long userId = update.Type switch
            {
                UpdateType.Message => update.Message.From.Id,
                UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
            };
            var user = await Register(userId);
            try
            {
                foreach (var handler in Handlers)
                {
                    if (handler.Value.IsMatch(update, user, _database))
                    {
                        await handler.Value.Handle(update, user, _database);
                        user.LastCommand = handler.Key;
                        break;
                    }
                }
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
