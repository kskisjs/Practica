using System.Windows;
using CRMApp.ViewModels;

namespace CRMApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _vm;

        public LoginWindow()
        {
            InitializeComponent();
            _vm = new LoginViewModel(App.AuthService);
            DataContext = _vm;

            _vm.LoginSucceeded += () =>
            {
                DialogResult = true;
                Close();
            };
        }

        // Кнопка «Войти» — Click в XAML, не Command, чтобы забрать пароль из PasswordBox
        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            _vm.DoLoginPublic();
        }

        // Кнопка «Зарегистрироваться»
        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            _vm.DoRegisterPublic();
        }
    }
}
