using System;
using System.Linq;

namespace environmator_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ler default config?           

            if (args.Length <= 0) 
            {
                Console.WriteLine("Command not found try one of these: ");
                ShowCommands();
                return;
            }

            var commands = args.Select(c => c.ToLower()).ToArray();

            if(commands[0] == "-h" || commands[0] == "--help")
            {
                ShowCommands();
                return;
            }

            if(commands[0] == "config")
            {
                Console.WriteLine("'config' command under construction!");
                return;
            }

            
            Console.WriteLine("Command not found try one of these: ");
            ShowCommands();
        }

        private static void ShowCommands()
        {
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("   new       Create a new project in all environments defined in default configuration or by the parameters.");
            Console.WriteLine("   config    Show the configuration to access you repositories, platforms, and C.I. builders.");
        }
    }
}
