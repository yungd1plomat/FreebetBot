using FreebetBot.Models;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreebetBot.Core.Utils
{
    public static class KeyboardManager
    {
        public static ReplyKeyboardMarkup AdminButtons()
        {
            return new(new[]
            {
                new KeyboardButton[] { "Добавить игру" },
                new KeyboardButton[] { "Все игры" },
                new KeyboardButton[] { "Рассылка" },
                new KeyboardButton[] { "Отмена"},
            })
            {
                ResizeKeyboard = true
            };
        }

        public static InlineKeyboardMarkup CreateGameButtons()
        {
            return new[]
            {
                InlineKeyboardButton.WithCallbackData("Создать", "creategame")
            };
        }

        public static InlineKeyboardMarkup SkipButton(string step)
        {
            return new[]
            {
                InlineKeyboardButton.WithCallbackData("Пропустить", $"skip:{step}")
            };
        }

        public static InlineKeyboardMarkup GameButtons(Game game)
        {
            return new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Сделать основным", $"main:{game.Id}") },
                new[] { InlineKeyboardButton.WithCallbackData("Редактировать", $"edit:{game.Id}") },
                new[] { InlineKeyboardButton.WithCallbackData("Удалить", $"delete:{game.Id}") },
            };
        }

        public static InlineKeyboardMarkup BuildGameButtons(string text)
        {
            return new[]
            {
                InlineKeyboardButton.WithCallBackGame(text),
            };
        }
    }
}
