using System.Threading.Tasks;

namespace Repart.Common.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
