using GCS_LAB411.ViewModels;
using GCS_LAB411.ViewModels.SubViewsModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GCS_LAB411
{
    public partial class App : Application
    {
        private readonly IHost host;
        public static IServiceProvider ServiceProvider { get; private set; }
        public App()
        {
            InitializeComponent();
            host = Host.CreateDefaultBuilder()  // Use default settings
                                                //new HostBuilder()          // Initialize an empty HostBuilder
                    .ConfigureAppConfiguration((hostingContext, configuration) =>
                    {
                        // Add other configuration files...
                        configuration.Sources.Clear();

                        IHostEnvironment env = hostingContext.HostingEnvironment;

                        configuration
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    }).ConfigureServices((hostingContext, services) =>
                    {
                        ConfigureServices(hostingContext.Configuration, services);
                    })
                    .ConfigureLogging(logging =>
                    {
                        // Add other loggers...
                    })
                    .Build();

            ServiceProvider = host.Services;
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            //services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            //_mapDownload = new HttpTileDownload();
            //// Register all ViewModels.
            services.AddSingleton<MainPage>();
            services.AddSingleton<SettingViewModel>();
            services.AddSingleton<NavBarViewModel>();
            services.AddSingleton<VehicleManagerViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MapViewModel>();
            services.AddSingleton<SlideConfirmViewModel>();
            services.AddSingleton<CameraLiveViewModel>();
        }

        protected override void OnStart()
        {
            MainPage = App.ServiceProvider.GetRequiredService<MainPage>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
