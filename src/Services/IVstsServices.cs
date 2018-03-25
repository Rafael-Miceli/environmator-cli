using System.Threading.Tasks;

namespace environmator_cli.Services
{
    public interface IVstsService
    {
        Task CreateRepository(string repositoryName);
    }
}