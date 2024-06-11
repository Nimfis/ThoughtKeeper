using System;
using System.Windows;
using ThoughtKeeper.Security;

namespace ThoughtKeeper
{
    public partial class RegistrationWindow : Window
    {
        private readonly IUserService _userService;
        private readonly IPasswordManager _passwordManager;

        public RegistrationWindow(IUserService userService, IPasswordManager passwordManager)
        {
            InitializeComponent();
            _userService = userService;
            _passwordManager = passwordManager;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Visibility == Visibility.Visible ? PasswordBox.Password : VisiblePasswordTextBox.Text;
            var repeatedPassword = RepeatPasswordBox.Visibility == Visibility.Visible ? RepeatPasswordBox.Password : VisibleRepeatPasswordTextBox.Text;

            if (_userService.DoesUsernameExist(username))
            {
                MessageBox.Show("Nazwa użytkownika jest już zajęta. Wybierz inną.", "Użytkownik o podanej nazwie istnieje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!_passwordManager.IsPasswordValid(password))
            {
                MessageBox.Show("Hasło powinno zawierać minimum osiem znaków, co najmniej jedna litera i jedną cyfrę.", "Hasło nie spełnia wymagań", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != repeatedPassword)
            {
                MessageBox.Show("Podane hasła nie są identyczne. Sprawdź wpisane hasła i spróbuj ponownie.", "Podane hasła nie są identyczne", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Clear();
                RepeatPasswordBox.Clear();
                return;
            }

            try
            {
                _userService.CreateUser(username, password);
                MessageBox.Show("Rejestracja przebiegła pomyślnie!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Rejestracja nie powiodła się. Wystąpił nieoczekiwany błąd.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            VisiblePasswordTextBox.Text = PasswordBox.Password;
            VisiblePasswordTextBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;

            VisibleRepeatPasswordTextBox.Text = RepeatPasswordBox.Password;
            VisibleRepeatPasswordTextBox.Visibility = Visibility.Visible;
            RepeatPasswordBox.Visibility = Visibility.Collapsed;
        }

        private void CheckBox_ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = VisiblePasswordTextBox.Text;
            VisiblePasswordTextBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;

            RepeatPasswordBox.Password = VisibleRepeatPasswordTextBox.Text;
            VisibleRepeatPasswordTextBox.Visibility = Visibility.Collapsed;
            RepeatPasswordBox.Visibility = Visibility.Visible;
        }
    }
}
