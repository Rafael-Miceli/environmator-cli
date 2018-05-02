using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ci_x_core
{
    public abstract class Command
    {
        public abstract string Verb { get; }
        public abstract Option[] Options { get; }
        public abstract string Help { get; }

        public abstract void CreateEnvironment();

        public async Task WriteConfig(Dictionary<Option, string> optionsAndValues)
        {
            Console.WriteLine("Indo escrever essas opções no Config");
            foreach (var option in optionsAndValues)
            {
                Console.WriteLine($"opcao: {option.Key.Name} valor: {option.Value}");
            }
        }
    }
}
