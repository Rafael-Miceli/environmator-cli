using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ci_x_core
{
    public abstract class EnvironmentPluginService
    {
        public abstract Task CreateEnvironment(string projectName, string description = null);
        public abstract Task SetEnvironmentConfig<T>(T opts);
        public abstract Task<T> ReadEnvironmentConfig<T>();
    }
}
