using System;
using Xunit;
using environmator_cli.Configuration;
using environmator_cli;

namespace environmator.tests
{
    public class ConfigTests
    {
        [Fact]
        public void Should_Create_Config_When_Doesnt_Exists()
        {
            var configVstsVerb = new ConfigVerb.ConfigVstsVerb()
            {
                Instance = "rafael-miceli",
                Project = "Envyx"
            };

            var sut = new ConfigRepository();            
            sut.SetVstsConfig(configVstsVerb);
        }

        [Fact]
        public void Should_Read_Config_When_Exists()
        {
            var configVstsVerb = new ConfigVerb.ConfigVstsVerb()
            {
                Instance = "rafael-miceli",
                Project = "Envyx",
                Token = "teste"
            };

            var sut = new ConfigRepository();
            var result = sut.ReadVstsConfig();

            Assert.NotNull(result);
        }

        [Fact]
        public void When_RepositoryName_Already_Exists_Then_Return_True()
        {
            var configVstsVerb = new ConfigVerb.ConfigVstsVerb()
            {
                Instance = "rafa-miceli",
                Project = "Teste",
                Token = "llhqarunnxc7gsb6yik34j3zcddlmgei5kzmebvexs6bcsick2ca"
            };

            Program._vstsConfig = configVstsVerb;
            var result = Program.RepositoryExists("Teste");

            Assert.False(result);
        }
    }
}
