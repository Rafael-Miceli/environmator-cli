using ci_x_core;
using System;
using System.Threading.Tasks;

namespace vsts_plugin
{
    public class VstsService : EnvironmentPluginService
    {
        public override Task CreateEnvironment(string name, string description = null)
        {
            throw new NotImplementedException();
        }

        public override Task<T> ReadEnvironmentConfig<T>()
        {
            throw new NotImplementedException();
        }

        public override Task SetEnvironmentConfig<T>(T opts)
        {
            throw new NotImplementedException();
        }
    }
}
