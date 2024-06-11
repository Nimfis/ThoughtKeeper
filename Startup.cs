using Microsoft.Extensions.DependencyInjection;
using ThoughtKeeper.Interfaces;
using ThoughtKeeper.Security;
using ThoughtKeeper.Service;
using ThoughtKeeper.Services;

namespace ThoughtKeeper
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPasswordManager, PasswordManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<INoteCryptoService, NoteCryptoService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}