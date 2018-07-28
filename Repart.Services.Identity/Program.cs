using Repart.Common.Services;

namespace Repart.Services.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .Build()
                .Run();
        }
    }
}
