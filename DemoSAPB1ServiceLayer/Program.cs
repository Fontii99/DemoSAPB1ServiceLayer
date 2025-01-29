using B1SLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoSAPB1ServiceLayer
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var host = CreateHost();
            var services = host.Services;
            var mainForm = services.GetRequiredService<MainView>();
            Application.Run(mainForm);
        }

        static IHost CreateHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                    builder.SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                .ConfigureServices((context, services) =>
                {

                    services.AddSingleton(sp => new SLConnection(
                        context.Configuration.GetSection("ServiceLayer").GetValue<string>("serviceLayerRoot"),
                        context.Configuration.GetSection("ServiceLayer").GetValue<string>("companyDB"),
                        context.Configuration.GetSection("ServiceLayer").GetValue<string>("userName"),
                        context.Configuration.GetSection("ServiceLayer").GetValue<string>("password")));

                    services.AddSingleton<MainView>();

                }).
                Build();
        }
    }
}
