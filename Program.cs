using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace br.com.waltercoan.azfuncisolated;
public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<FuncDbContext>();
            })
            .Build();

        host.Run();
    }
}
