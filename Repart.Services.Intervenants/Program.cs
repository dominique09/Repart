using Repart.Common.Services;

namespace Repart.Services.Intervenants
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
