namespace environmator_cli.Configuration
{
    public interface IConfigRepository
    {
        void ClearFileContent(string envyxConfigFile);
        //ConfigVerb.ConfigVstsVerb ReadVstsConfig();
        //void SetVstsConfig(ConfigVerb.ConfigVstsVerb opts);
    }
}