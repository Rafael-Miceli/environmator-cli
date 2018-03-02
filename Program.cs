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
                Console.WriteLine("");
                Console.WriteLine("'config' still command under construction!");
                Console.WriteLine("Today we only support set your VSTS code repo environment");

                if(commands.Length == 1) 
                {
                    ShowConfigSubCommandsAndOptions();
                    return;
                }

                if(commands[1] == "vsts")
                {
                    if(commands.Length == 2)
                    {
                        ShowVstsOptions();
                        return;
                    }
                    
                                            
                }

                return;
            }

            
            Console.WriteLine("Command not found try one of these: ");
            ShowCommands();
        }

        private static void ShowVstsOptions()
        {
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("   -i --instance      your vsts instance.");
        }

        private static void ShowCommands()
        {
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("   new       Create a new project in all environments defined in default configuration or by the parameters.");
            Console.WriteLine("   config    Show the configuration to access you repositories, platforms, and C.I. builders.");
        }

        private static void ShowConfigSubCommandsAndOptions()
        {
            Console.WriteLine("");
            Console.WriteLine("SubCommands:");
            Console.WriteLine("   vsts      Set the options to use your VSTS environment.");
        }
    }
}
