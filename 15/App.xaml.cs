using System.Windows;
using System.Windows.Threading;

namespace CRMApp
{
    public partial class App : Application
    {
        public App()
        {
            // Глобальный обработчик необработанных исключений UI-потока
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                $"Необработанная ошибка:\n{e.Exception.Message}",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.Handled = true; // не крашить приложение
        }
    }
}
