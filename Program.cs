using System;
using hsr_museum.src.main;

namespace hsr_museum
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}