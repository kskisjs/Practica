using System.Windows.Input;
using CRMApp.Services;

namespace CRMApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        // Пароль не биндим напрямую — получаем из PasswordBox в code-behind
        public string Password { get; set; } = string.Empty;

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        private bool _isRegisterMode;
        public bool IsRegisterMode
        {
            get => _isRegisterMode;
            set { SetField(ref _isRegisterMode, value); OnPropertyChanged(nameof(FormTitle)); }
        }

        public string FormTitle => IsRegisterMode ? "Регистрация" : "Вход в систему";

        private string _fullName = string.Empty;
        public string FullName
        {
            get => _fullName;
            set => SetField(ref _fullName, value);
        }

        public bool LoginSuccess { get; private set; }

        public ICommand LoginCommand   { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleModeCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand    = new RelayCommand(DoLogin);
            RegisterCommand = new RelayCommand(DoRegister);
            ToggleModeCommand = new RelayCommand(() => { IsRegisterMode = !IsRegisterMode; ErrorMessage = string.Empty; });
        }

        private void DoLogin()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введите логин и пароль.";
                return;
            }

            if (!_authService.Login(Username.Trim(), Password))
            {
                ErrorMessage = "Неверный логин или пароль.";
                return;
            }

            LoginSuccess = true;
            LoginSucceeded?.Invoke();
        }

        private void DoRegister()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(FullName))
            {
                ErrorMessage = "Заполните все поля.";
                return;
            }

            if (!_authService.Register(Username.Trim(), Password, FullName.Trim()))
            {
                ErrorMessage = "Пользователь с таким логином уже существует.";
                return;
            }

            ErrorMessage = "Регистрация прошла успешно! Теперь войдите.";
            IsRegisterMode = false;
        }

        // Публичные методы для вызова из code-behind (PasswordBox передаёт пароль до вызова)
        public void DoLoginPublic()    => DoLogin();
        public void DoRegisterPublic() => DoRegister();

        public event System.Action? LoginSucceeded;
    }
}
