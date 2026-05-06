using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CRMApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // Список всех клиентов
        private ObservableCollection<Client> clients;

        // Выбранный клиент в списке
        private Client selectedClient;
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                OnPropertyChanged();
                if (value != null && !IsEditMode)
                {
                    // Заполняем поля данными выбранного клиента
                    FullName = value.FullName;
                    Phone = value.Phone;
                    Email = value.Email;
                }
            }
        }

        // Поля для привязки к TextBox (режим редактирования)
        private string fullName;
        public string FullName
        {
            get => fullName;
            set { fullName = value; OnPropertyChanged(); }
        }

        private string phone;
        public string Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged(); }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        // Режим редактирования (включены/выключены поля)
        private bool isEditMode = false;
        public bool IsEditMode
        {
            get => isEditMode;
            set { isEditMode = value; OnPropertyChanged(); }
        }

        // ID для нового клиента (временный)
        private int? editingId = null;

        // КОМАНДЫ
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            LoadSampleData();
            lvClients.ItemsSource = clients;

            // Инициализация команд
            AddCommand = new RelayCommand(_ => StartAddMode());
            EditCommand = new RelayCommand(_ => StartEditMode(), _ => SelectedClient != null && !IsEditMode);
            DeleteCommand = new RelayCommand(_ => DeleteClient(), _ => SelectedClient != null && !IsEditMode);
            SaveCommand = new RelayCommand(_ => SaveClient(), _ => IsEditMode && !string.IsNullOrWhiteSpace(FullName));
            CancelCommand = new RelayCommand(_ => CancelEdit(), _ => IsEditMode);

            IsEditMode = false;
            ClearForm();
        }

        private void LoadSampleData()
        {
            clients = new ObservableCollection<Client>
            {
                new Client { Id = 1, FullName = "Иванов Иван Иванович", Phone = "+7 (999) 123-45-67", Email = "ivanov@mail.ru" },
                new Client { Id = 2, FullName = "Петрова Анна Сергеевна", Phone = "+7 (888) 765-43-21", Email = "petrova@mail.ru" },
                new Client { Id = 3, FullName = "Сидоров Алексей Владимирович", Phone = "+7 (777) 555-33-11", Email = "sidorov@mail.ru" }
            };
        }

        private void ClearForm()
        {
            FullName = "";
            Phone = "";
            Email = "";
            editingId = null;
        }

        private void StartAddMode()
        {
            ClearForm();
            IsEditMode = true;
            editingId = null;
            tbStatus.Text = "✏️ Режим: добавление нового клиента";
        }

        private void StartEditMode()
        {
            if (SelectedClient == null) return;

            FullName = SelectedClient.FullName;
            Phone = SelectedClient.Phone;
            Email = SelectedClient.Email;
            editingId = SelectedClient.Id;
            IsEditMode = true;
            tbStatus.Text = $"✏️ Режим: редактирование клиента ID={SelectedClient.Id}";
        }

        private void SaveClient()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Введите ФИО клиента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Введите номер телефона!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (editingId == null)
            {
                // ДОБАВЛЕНИЕ
                int newId = clients.Count > 0 ? clients.Max(c => c.Id) + 1 : 1;
                clients.Add(new Client
                {
                    Id = newId,
                    FullName = FullName.Trim(),
                    Phone = Phone.Trim(),
                    Email = Email.Trim()
                });
                MessageBox.Show("✅ Клиент добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // РЕДАКТИРОВАНИЕ
                var client = clients.FirstOrDefault(c => c.Id == editingId);
                if (client != null)
                {
                    client.FullName = FullName.Trim();
                    client.Phone = Phone.Trim();
                    client.Email = Email.Trim();
                    lvClients.Items.Refresh();
                    MessageBox.Show("✅ Данные обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            CancelEdit();
        }

        private void CancelEdit()
        {
            IsEditMode = false;
            ClearForm();
            tbStatus.Text = "⚡ Режим: просмотр";

            if (SelectedClient != null)
            {
                FullName = SelectedClient.FullName;
                Phone = SelectedClient.Phone;
                Email = SelectedClient.Email;
            }
        }

        private void DeleteClient()
        {
            if (SelectedClient == null) return;

            var result = MessageBox.Show($"Удалить клиента:\n{SelectedClient.FullName}?",
                                          "Подтверждение удаления",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                clients.Remove(SelectedClient);
                if (IsEditMode) CancelEdit();
                MessageBox.Show("🗑️ Клиент удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

        private void About_Click(object sender, RoutedEventArgs e) =>
            MessageBox.Show("CRM для небольшого бизнеса\nВерсия 2.0\nВсё на одной форме", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}