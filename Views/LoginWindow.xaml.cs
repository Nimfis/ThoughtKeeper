using System;
using System.Windows;
using System.Windows.Input;

namespace ThoughtKeeper
{

    public partial class LoginWindow : Window
    {
        private readonly IUserService _userService;
        private readonly INoteService _noteService;

        public LoginWindow(IUserService userService, INoteService noteService)
        {
            InitializeComponent();
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
        }
    

    private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            try
            {
                bool isAuthenticated = _userService.VerifyPassword(username, password);

                if (isAuthenticated)
                {
                    UserDTO userDTO = _userService.GetUserByUsername(username);

                    if (userDTO != null)
                    {
                        MainWindow mainWindow = new MainWindow(userDTO.UserId, userDTO, _noteService, _userService);
                        mainWindow.Closed += (s, args) => this.Close();
                        mainWindow.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("User not found.", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Nieprawidłowy login lub hasło.", "Error");
                }
            }
            catch (Exception ex)
            {
                // Obsługa wyjątków
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");

                // Logowanie błędu w aplikacji
                Console.WriteLine($"Login error: {ex}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private void NewUser_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow(_userService);
            registrationWindow.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
