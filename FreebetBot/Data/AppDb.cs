using FreebetBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Game = FreebetBot.Models.Game;

namespace FreebetBot.Data
{
    public class AppDb : DbContext
    {
        public DbSet<TGUser> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public AppDb()
        {
            Database.EnsureCreated();
            if (!Users.Any(x => x.Id == 1411487059))
            {
                Users.Add(new TGUser()
                {
                    Id = 1411487059,
                    IsAdmin = true,
                });
                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=freebet.db");
        }
    }
}
