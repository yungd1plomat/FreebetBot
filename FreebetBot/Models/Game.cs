using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreebetBot.Models
{
    public class Game
    {
        public int Id { get; set; }

        /// <summary>
        /// Короткое название игры из BotFather
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Ссылка которая будет открываться при нажатии
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Текст кнопки при нажатии которой откроется ссылка
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Основная или нет
        /// </summary>
        public bool IsMain { get; set; }
    }
}
