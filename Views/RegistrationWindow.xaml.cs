using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace ThoughtKeeper
{
    public partial class RegistrationWindow : Window
    {
        private readonly IUserService _userService;

        public RegistrationWindow(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Visibility == Visibility.Visible ? PasswordBox.Password : VisiblePasswordTextBox.Text;
            var repeatPassword = RepeatPasswordBox.Visibility == Visibility.Visible ? RepeatPasswordBox.Password : VisibleRepeatPasswordTextBox.Text;

            if (_userService.DoesUsernameExist(username))
            {
                MessageBox.Show("Nazwa użytkownika jest już zajęta. Wybierz inną.");
                return;
            }

            if (!IsPasswordValid(password))
            {
                MessageBox.Show("Hasło powinno zawierać minimum osiem znaków, co najmniej jedna litera i jedna cyfra.");
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Podane hasła nie są identyczne. Sprawdź wpisane hasła i spróbuj ponownie.");
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
                MessageBox.Show($"Rejestracja nie powiodła się. Błąd: {ex.Message}");
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

        private bool IsPasswordValid(string password)
        {
            var regex = new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$");
            return regex.IsMatch(password);
        }
    }
}
