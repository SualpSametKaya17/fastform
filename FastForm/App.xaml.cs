using System.Windows;
using FastForm.Data;
using FastForm.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FastForm
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/fastform-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("FastForm application starting...");

            // Configure Services
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Initialize Database
            try
            {
                var dbContext = ServiceProvider.GetRequiredService<FastFormDbContext>();
                dbContext.Database.EnsureCreated();
                Log.Information("Database initialized successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to initialize database");
                MessageBox.Show($"Veritabanı bağlantısı kurulamadı:\n{ex.Message}",
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // DbContext
            services.AddDbContext<FastFormDbContext>();

            // Services
            services.AddScoped<IFormTemplateService, FormTemplateService>();
            services.AddScoped<IFieldConfigurationService, FieldConfigurationService>();
            services.AddScoped<IFormDataService, FormDataService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IAuditService, AuditService>();

            // ViewModels
            services.AddTransient<ViewModels.MainViewModel>();
            services.AddTransient<ViewModels.FormEditorViewModel>();
            services.AddTransient<ViewModels.FormFillViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("FastForm application shutting down...");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}
