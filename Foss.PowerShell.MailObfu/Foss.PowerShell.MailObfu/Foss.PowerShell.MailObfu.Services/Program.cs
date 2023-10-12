

namespace DesensitizeMailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            var host = builder.ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
            }).Build();

            host.Run();
        }
    }
}