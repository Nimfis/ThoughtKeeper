using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ThoughtKeeper.Service;

namespace ThoughtKeeper
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var startup = new Startup("Data Source=AR-ACER-NITRO\\SQLEXPRESS;Initial Catalog=Thought_Keeper;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
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
                serviceProvider.GetRequiredService<INoteService>());
        }
    }
}
