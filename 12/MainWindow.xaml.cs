using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CRMApp
{
    public partial class MainWindow : Window
    {
        // Список всех клиентов. ObservableCollection сам обновляет ListView
        private ObservableCollection<Client> clients;

        // Текущий выбранный клиент в ListView
        private Client selectedClient;

        public MainWindow()
        {
            InitializeComponent(); // Эта строчка загружает дизайн из XAML

            // Загружаем тестовые данные
            LoadSampleData();

            // Привязываем список клиентов к ListView
            lvClients.ItemsSource = clients;
        }

        /// <summary>
        /// Загружает примеры клиентов для демонстрации
        /// </summary>
        private void LoadSampleData()
        {
            clients = new ObservableCollection<Client>
            {
                new Client
                {
                    Id = 1,
                    FullName = "Иванов Иван Иванович",
                    Phone = "+7 (999) 123-45-67",
                    Email = "ivanov@mail.ru"
                },
                new Client
                {
                    Id = 2,
                    FullName = "Петрова Анна Сергеевна",
                    Phone = "+7 (888) 765-43-21",
                    Email = "petrova@mail.ru"
                },
                new Client
                {
                    Id = 3,
                    FullName = "Сидоров Алексей Владимирович",
                    Phone = "+7 (777) 555-33-11",
                    Email = "sidorov@mail.ru"
                }
            };
        }

        /// <summary>
        /// Срабатывает, когда пользователь выбрал другого клиента в списке
        /// </summary>
        private void LvClients_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Получаем выбранного клиента
            selectedClient = lvClients.SelectedItem as Client;

            if (selectedClient != null)
            {
                // Показываем информацию о выбранном клиенте
                tbSelectedClient.Text = $"{selectedClient.FullName}\n{selectedClient.Phone}\n{selectedClient.Email}";
            }
            else
            {
                tbSelectedClient.Text = "(не выбран)";
            }
        }

        /// <summary>
        /// Кнопка "Добавить" - открывает окно для создания нового клиента
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Создаём окно добавления
            var dialog = new ClientDialog();

            // ShowDialog() показывает окно и ЖДЁТ, пока его закроют
            // Если пользователь нажал "Добавить" (DialogResult = true) - добавляем
            if (dialog.ShowDialog() == true)
            {
                // Генерируем новый ID
                int newId = clients.Count > 0 ? clients.Max(c => c.Id) + 1 : 1;

                // Создаём нового клиента
                var newClient = new Client
                {
                    Id = newId,
                    FullName = dialog.FullName,  // Берём из формы
                    Phone = dialog.Phone,         // Берём из формы
                    Email = dialog.Email          // Берём из формы
                };

                // Добавляем в список
                clients.Add(newClient);

                MessageBox.Show($"✅ Клиент {newClient.FullName} добавлен!",
                                "Успех",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Кнопка "Редактировать" - изменяет данные выбранного клиента
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли клиент
            if (selectedClient == null)
            {
                MessageBox.Show("Сначала выберите клиента в списке!",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Открываем окно редактирования, передаём текущего клиента
            var dialog = new ClientDialog(selectedClient);

            if (dialog.ShowDialog() == true)
            {
                // Обновляем данные у выбранного клиента
                selectedClient.FullName = dialog.FullName;
                selectedClient.Phone = dialog.Phone;
                selectedClient.Email = dialog.Email;

                // Обновляем отображение в ListView (Refresh заставляет перерисовать)
                lvClients.Items.Refresh();

                // Обновляем информацию в правой панели
                tbSelectedClient.Text = $"{selectedClient.FullName}\n{selectedClient.Phone}\n{selectedClient.Email}";

                MessageBox.Show("Данные клиента обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Кнопка "Удалить" - удаляет выбранного клиента
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли клиент
            if (selectedClient == null)
            {
                MessageBox.Show("Сначала выберите клиента в списке!",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Спрашиваем подтверждение
            var result = MessageBox.Show($"Вы действительно хотите удалить клиента:\n{selectedClient.FullName}?",
                                          "Подтверждение удаления",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                clients.Remove(selectedClient);
                selectedClient = null;
                tbSelectedClient.Text = "(не выбран)";

                MessageBox.Show("Клиент удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}