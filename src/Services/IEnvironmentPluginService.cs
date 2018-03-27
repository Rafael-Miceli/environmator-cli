using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace environmator_cli.Services
{
    interface IEnvironmentPluginService
    {
        Task CreateEnvironment(string name, string description = null);
        void SetEnvironmentConfig<T>(T opts);
    }
}
