using System;
using System.Collections.Generic;
using System.Text;

namespace ci_x_core
{
    public class Option
    {
        public Option(
            string name,
            string commandShortName,
            string commandLongName = "",
            string help = "",
            bool isRequired = true)
        {
            Name = name;
            TerminalShortName = commandShortName;
            TerminalLongName = commandLongName;
            Help = help;
            IsRequired = isRequired;
        }

        public string Name { get; }
        public string TerminalShortName { get; }
        public string TerminalLongName { get; }
        public string Help { get; }
        public bool IsRequired { get; }
    }
}
