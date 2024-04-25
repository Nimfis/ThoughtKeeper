using System;
using System.Security.Cryptography;
using System.Text;


namespace ThoughtKeeper
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userManagementService)
        {
            _userService = userManagementService ?? throw new ArgumentNullException(nameof(userManagementService));
        }

        public bool Login(string username, string password)
        {
            var userHashedPassword = _userService.GetPasswordHash(username);
            return VerifyPassword(password, userHashedPassword);
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Konwertowanie podanego hasła na tablicę bajtów
            byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);

            // Inicjalizacja obiektu SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Obliczenie skrótu dla podanego hasła
                byte[] hashedBytes = sha256.ComputeHash(enteredPasswordBytes);

                // Konwersja zaszyfrowanego skrótu na postać tekstową (hexadecimal)
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                string hashedPassword = builder.ToString();

                // Porównanie obliczonego skrótu z zapisanym skrótem
                return string.Equals(storedHash, hashedPassword, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}