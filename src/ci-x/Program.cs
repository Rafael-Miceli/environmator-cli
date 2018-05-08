using System;
using System.Collections.Generic;
using ci_x_core;
using System.Linq;
using vsts_plugin;
using System.Reflection;
using System.IO;

namespace environmator_cli
{
    public class Program
    {
        private static List<Command> _commands = new List<Command>();

        static int Main(string[] args)
        {
            InitializeCommands();

            if (args.Length <= 0)
            {
                Console.WriteLine("Command not found try one of these: ");
                ShowCommands();
                return -1;
            }

            var commands = args.Select(c => c.ToLower()).ToArray();

            if (IsAskingHelp(commands[0]))
            {
                ShowCommands();
                return 0;
            }

            if (!_commands.Any())
            {
                Console.WriteLine("You don't have any plugin configured. Please add plugins in your ci-x config file!");
                return -1;
            }
            
            if (commands[0] == "new-project")
            {
                ManageNewProject(commands);
            }

            if (commands[0] == "config")
            {
                ManageConfig(commands);
            }

            

            return 0;
        }

        private static int ManageNewProject(string[] commands)
        {
            if (commands.Length <= 1 || IsAskingHelp(commands[1]))
            {
                Console.WriteLine($"new-project options:");
                Console.WriteLine($"   -n --name        Your project name.");
                return 0;
            }

            if (!(commands[1] == "-n" || commands[1] == "--name"))
            {
                Console.WriteLine("Specify your project name option.");
                return -1;
            }

            if (commands.Length <= 2)
            {
                Console.WriteLine("Specify your project name option.");
                return -1;
            }

            foreach (var command in _commands)
            {
                Console.WriteLine($"Creating your environment in {command.Verb}");

                try
                {
                    var options = command.ReadConfig().Result;
                    command.CreateEnvironment(commands[2], options);
                    Console.WriteLine($"Environment in {command.Verb} created!");
                }
                catch (Exception ex)
                {
                    //Logar exceção
                    Console.WriteLine($"Error when creating your environment in {command.Verb}, See the logs for more information.");
                }
                
            }

            Console.WriteLine($"Project created in all environments.");

            return 0;
        }

        private static int ManageConfig(string[] commands)
        {
            if (commands.Length <= 1 || IsAskingHelp(commands[1]))
            {
                ShowConfigCommands();
                return 0;
            }

            var selectedConfig = _commands.FirstOrDefault(c => c.Verb == commands[1]);

            if (selectedConfig == null)
            {
                Console.WriteLine($"Config `{commands[1]}` not found... =/ Here are the configs you have:");
                ShowConfigCommands();
                return 0;
            }

            if (commands.Length <= 2 || IsAskingHelp(commands[2]))
            {
                ShowConfigOptions(selectedConfig);
                return 0;
            }

            Dictionary<string, string> rawOptionsAndValues = GetOptionsAndItsValues(commands);

            List<string> requiredOptionsNotFilled = RequiredOptionsNotFilled(rawOptionsAndValues, selectedConfig);

            if (requiredOptionsNotFilled.Any())
            {
                Console.Write("Required options: ");
                foreach (var requiredOptionNotFilled in requiredOptionsNotFilled)
                    Console.Write($"{requiredOptionNotFilled},");
                Console.WriteLine(" not filled");

                return -1;
            }

            var optionsAndValues = TransformRawOptionsToObject(rawOptionsAndValues, selectedConfig);

            selectedConfig.WriteConfig(optionsAndValues).Wait();
            return 0;
        }




        private static Dictionary<Option, string> TransformRawOptionsToObject(Dictionary<string, string> rawOptionsAndValues, Command selectedConfig)
        {
            var optionsCommands = selectedConfig.Options;

            var optionsAndValues = new Dictionary<Option, string>();

            foreach (var option in optionsCommands)
            {
                var matchedRawOptionAndValue = rawOptionsAndValues.FirstOrDefault(op => op.Key == ("-" + option.TerminalShortName) || op.Key == ("--" + option.TerminalLongName));                
                optionsAndValues.Add(option, matchedRawOptionAndValue.Value);
            }

            return optionsAndValues;
        }

        private static List<string> RequiredOptionsNotFilled(Dictionary<string, string> optionsAndValues, Command selectedConfig)
        {
            var requiredOptionsCommands = selectedConfig.Options.Where(o => o.IsRequired);
            var optionsFromArgs = optionsAndValues.Select(o => o.Key);
            
            var requiredOptionsNotFilled = new List<string>();
            foreach (var requiredOption in requiredOptionsCommands)
            {                
                if (!optionsFromArgs.Any(op => op == ("-" + requiredOption.TerminalShortName) || op == ("--" + requiredOption.TerminalLongName)))
                    requiredOptionsNotFilled.Add(requiredOption.Name);
            }

            return requiredOptionsNotFilled;
        }

        private static Dictionary<string, string> GetOptionsAndItsValues(string[] commands)
        {
            var optionsAndVaues = new Dictionary<string, string>();

            for (int i = 2; i < commands.Length; i++)
            {
                var option = commands[i];
                i++;

                var value = "";
                if (i <= commands.Length)
                {
                    value = commands[i];
                }

                optionsAndVaues.Add(option, value);
            }

            return optionsAndVaues;
        }

        


        private static bool IsAskingHelp(string command)
        {
            return command == "-h" || command == "--help" || command == "help";
        }

        private static void ShowCommands()
        {
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("   new-project   Create a new project in all environments defined in default configuration or by the parameters.");
            Console.WriteLine("   config        Show the configuration to access you repositories, platforms, and C.I. builders.");
        }

        private static void ShowConfigCommands()
        {
            Console.WriteLine("");          
            
            Console.WriteLine("configs:");            
            foreach (var command in _commands)
            {
                Console.WriteLine($"   {command.Verb}   {command.Help}.");
            }            
        }

        private static void ShowConfigOptions(Command command)
        {
            Console.WriteLine("");
            Console.WriteLine($"config {command.Verb} options:");
            foreach (var option in command.Options)
            {
                Console.Write($"   -{option.TerminalShortName} ");
                if (!string.IsNullOrEmpty(option.TerminalLongName))
                    Console.Write($"--{option.TerminalLongName}");

                Console.WriteLine($"    {option.Help}");
            }
        }


        private static void InitializeCommands()
        {
            var plugins = GetPluginsToLoad();

            Type baseType = typeof(Command);

            foreach (var plugin in plugins)
            {
                string target = plugin; 
                Assembly assembly = Assembly.LoadFrom(target);

                var @type = assembly.GetTypes()
                    .Where(baseType.IsAssignableFrom)
                    .FirstOrDefault(t => baseType != t);

                var instance = Activator.CreateInstance(@type) as Command;
                _commands.Add(instance);
            }            
        }

        private static IEnumerable<string >GetPluginsToLoad()
        {
            var AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var envyxDir = Path.Combine(AppDataPath, "ci-x");
            Directory.CreateDirectory(envyxDir);

            var envyxConfigFile = Path.Combine(envyxDir, "config.");

            var pluginInConfig = File.ReadAllLines(envyxConfigFile);

            var plugins = pluginInConfig
             .Where(l => !string.IsNullOrEmpty(l))
             .SkipWhile(line => !line.Contains($"[plugins]"))
             .Skip(1)
             .TakeWhile(line => !line.Contains("["));

            return plugins;
        }
    }
}
