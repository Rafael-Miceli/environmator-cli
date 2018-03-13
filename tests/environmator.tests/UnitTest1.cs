using System;
using Xunit;
using environmator_cli.Configuration;
using environmator_cli;

namespace environmator.tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var configVstsVerb = new ConfigVerb.ConfigVstsVerb()
            {
                Instance = "rafael-miceli",
                Project = "PriceStore"
            };

            var sut = new ConfigRepository();            
            sut.SetVstsConfig(configVstsVerb);
        }
    }
}
