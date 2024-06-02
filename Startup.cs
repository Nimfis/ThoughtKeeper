using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Windows;
using ThoughtKeeper.Interfaces;
using ThoughtKeeper.Service;
using ThoughtKeeper.Services;

namespace ThoughtKeeper
{
    public class Startup
    {
        private readonly string _connectionString;

        public Startup(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService>(provider => new UserService(_connectionString));

            byte[] key;

            using (var aes = Aes.Create())
            {
                key = aes.Key;
            }

            byte[] iv;
            using (var aes = Aes.Create())
            {
                iv = aes.IV;
            }

            services.AddScoped<INoteCryptoService>(provider => new NoteCryptoService(key, iv));
            services.AddScoped<INoteService, NoteService>(_ => new NoteService(_connectionString, _.GetService<INoteCryptoService>(), _.GetService<IUserService>()));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICategoryService>(provider => new CategoryService(_connectionString));
        }
    }
}