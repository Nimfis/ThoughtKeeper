using System;
using System.Windows;
using ThoughtKeeper.Database;
using ThoughtKeeper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ThoughtKeeper.Security;

namespace ThoughtKeeper
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseSetup();

            var startup = new Startup();
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
                serviceProvider.GetRequiredService<ICategoryService>(),
                serviceProvider.GetRequiredService<IPasswordManager>());
        }

        private void DatabaseSetup()
        {
            var databaseContext = new DatabaseContext();
            databaseContext.EnsureDatabaseCreated();
            databaseContext.EnsureSchemaCreated();
            databaseContext.SeedData();
        }
    }
}
