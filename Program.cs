using System;

namespace environmator_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read configuration            
            // Console.WriteLine("Welcome to environmator!");
            // Console.WriteLine("Let's try to create the environment of all your application!");
            var commands = args[0];

            Console.WriteLine("");

            if(commands == "-h" || commands == "--help")
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("   new       Create a new project in all environments defined in default configuration or by the parameters.");
            }
        }
    }
}
