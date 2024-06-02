using System;
using System.Windows;
using System.Configuration;
using ThoughtKeeper.Database;
using ThoughtKeeper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ThoughtKeeper
{
    public partial class App : Application
    {
        private readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["ThoughtKeeperDB"].ConnectionString;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseSetup();

            var startup = new Startup(CONNECTION_STRING);
            var serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var loginWindow = CreateLoginWindow(serviceProvider);
            loginWindow.Show();
        }

        private LoginWindow CreateLoginWindow(IServiceProvider serviceProvider)
        {
            return new LoginWindow(
                serviceProvider.GetRequiredService<IUserService>(),
                serviceProvider.GetRequiredService<INoteService>(),
                serviceProvider.GetRequiredService<ICategoryService>());
        }

        private void DatabaseSetup()
        {
            var databaseContext = new DatabaseContext(CONNECTION_STRING);
            databaseContext.EnsureDatabaseCreated();
            databaseContext.EnsureSchemaCreated();
            databaseContext.SeedData();
        }
    }
}
