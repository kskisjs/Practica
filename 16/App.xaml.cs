using System.Windows;
using System.Windows.Threading;
using CRMApp.Services;
using CRMApp.Views;

namespace CRMApp
{
    public partial class App : Application
    {
        // Синглтоны сервисов — живут всё время жизни приложения
        public static IAuthService AuthService { get; } = new AuthService();
        public static ClientService ClientService { get; } = new ClientService();
        public static NotificationService NotificationService { get; } = new NotificationService();
        public static FileWatcherService FileWatcher { get; } = new FileWatcherService();
        public static PipeMessenger PipeMessenger { get; } = new PipeMessenger();
        public static ChatService ChatService { get; } = new ChatService();

        public App()
        {
            DispatcherUnhandledException += OnUnhandled;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Не завершать приложение автоматически при закрытии любого окна
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            FileWatcher.Start();

            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (!AuthService.IsAuthenticated)
            {
                Shutdown();
                return;
            }

            // Теперь можно переключиться на обычный режим
            ShutdownMode = ShutdownMode.OnLastWindowClose;

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            FileWatcher.Dispose();
            PipeMessenger.Dispose();
            base.OnExit(e);
        }

        private void OnUnhandled(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                $"Необработанная ошибка:\n{e.Exception.Message}",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
