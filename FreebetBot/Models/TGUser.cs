using FreebetBot.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FreebetBot.Models
{
    public class TGUser
    {
        /// <summary>
        /// ID чата
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Последняя команда которую использовал пользователь
        /// </summary>
        public Command? LastCommand { get; set; }

        /// <summary>
        /// Администратор пользователь или нет
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Id последнего отправленного ботом сообщения (для навигации)
        /// </summary>
        public int? LastMsg { get; set; }

        /// <summary>
        /// Полезная нагрузка
        /// </summary>
        public string? Data { get; set; }

        public TGUser() { }

        public TGUser(long chatId, bool isAdmin)
        {
            Id = chatId;
            IsAdmin = isAdmin;
        }
    }
}
