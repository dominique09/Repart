using System.Threading.Tasks;

namespace Repart.Common.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
