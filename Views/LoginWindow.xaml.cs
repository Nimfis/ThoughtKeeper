using System;
using System.Windows;
using System.Windows.Input;
using ThoughtKeeper.Interfaces;
using ThoughtKeeper.Security;

namespace ThoughtKeeper
{

    public partial class LoginWindow : Window
    {
        private readonly IUserService _userService;
        private readonly INoteService _noteService;
        private readonly ICategoryService _categoryService;
        private readonly IPasswordManager _passwordManager;

        public LoginWindow(IUserService userService, INoteService noteService, ICategoryService categoryService, IPasswordManager passwordManager)
        {
            InitializeComponent();
            _userService = userService;
            _noteService = noteService;
            _categoryService = categoryService;
            _passwordManager = passwordManager;
        }
    

    private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            try
            {
                bool isAuthenticated = _passwordManager.VerifyPassword(username, password);

                if (isAuthenticated)
                {
                    UserDTO userDTO = _userService.GetUserByUsername(username);

                    if (userDTO != null)
                    {
                        MainWindow mainWindow = new MainWindow(userDTO, _noteService, _userService, _categoryService, _passwordManager);
                        mainWindow.Closed += (s, args) => this.Close();
                        mainWindow.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show($"Użytkownik o podanej nazwie '{username}' nie został odnaleziony", "Użytkownik nie istnieje", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Wprowadzono nieprawidłowy login lub hasło. Spróbuj ponownie", "Błędne dane logowania", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił nieoczekiwany błąd", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewUser_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow(_userService, _passwordManager);
            registrationWindow.Show();
        }

    }
}
