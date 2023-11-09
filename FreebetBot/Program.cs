using FreebetBot.Abstractions;
using FreebetBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreebetBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Enter token as argument, Example dotnet FreebetBot.dll 123456:asdhsadsadhkjhwejquiahdak");
                return;
            }
            string token = args[0];
            IBot bot = new Bot(token);
            await bot.Start();
            Console.ReadLine();
        }
    }
}
