using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CRMApp.Models;
using CRMApp.Services;

namespace CRMApp.ViewModels
{
    public class CRMViewModel : BaseViewModel
    {
        private readonly IClientService _clientService;

        // ──────────────────────────────────────────
        // Коллекция клиентов (ObservableCollection)
        // ──────────────────────────────────────────
        public ObservableCollection<ClientModel> Clients { get; } = new();

        // ──────────────────────────────────────────
        // Выбранный клиент (TwoWay-привязки в форме)
        // ──────────────────────────────────────────
        private ClientModel? _selectedClient;
        public ClientModel? SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (SetField(ref _selectedClient, value))
                    // При смене выбора копируем данные во временную форму редактирования
                    LoadEditForm(value);
            }
        }

        // ──────────────────────────────────────────
        // Поля формы редактирования (TwoWay-привязка)
        // ──────────────────────────────────────────
        private string _editFullName = string.Empty;
        public string EditFullName
        {
            get => _editFullName;
            set => SetField(ref _editFullName, value);
        }

        private string _editPhone = string.Empty;
        public string EditPhone
        {
            get => _editPhone;
            set => SetField(ref _editPhone, value);
        }

        private string _editEmail = string.Empty;
        public string EditEmail
        {
            get => _editEmail;
            set => SetField(ref _editEmail, value);
        }

        private ClientType _editClientType;
        public ClientType EditClientType
        {
            get => _editClientType;
            set => SetField(ref _editClientType, value);
        }

        // Список типов клиента для ComboBox (OneWay-привязка)
        public Array ClientTypeValues => Enum.GetValues(typeof(ClientType));

        // ──────────────────────────────────────────
        // Индикатор загрузки
        // ──────────────────────────────────────────
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        // ──────────────────────────────────────────
        // Статусное сообщение
        // ──────────────────────────────────────────
        private string _statusMessage = "Готово";
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        // ──────────────────────────────────────────
        // Команды
        // ──────────────────────────────────────────
        public ICommand LoadClientsCommand  { get; }
        public ICommand AddClientCommand    { get; }
        public ICommand SaveClientCommand   { get; }
        public ICommand DeleteClientCommand { get; }

        // ──────────────────────────────────────────
        // Конструктор
        // ──────────────────────────────────────────
        public CRMViewModel() : this(new ClientService()) { }

        public CRMViewModel(IClientService clientService)
        {
            _clientService = clientService;

            LoadClientsCommand  = new AsyncRelayCommand(LoadClientsAsync);
            AddClientCommand    = new RelayCommand(AddClient);
            SaveClientCommand   = new AsyncRelayCommand(SaveClientAsync,
                                    () => SelectedClient != null);
            DeleteClientCommand = new AsyncRelayCommand(DeleteClientAsync,
                                    () => SelectedClient != null);

            // Загрузка при старте
            _ = LoadClientsAsync();
        }

        // ──────────────────────────────────────────
        // Методы
        // ──────────────────────────────────────────

        private async Task LoadClientsAsync()
        {
            IsLoading = true;
            StatusMessage = "Загрузка клиентов...";
            try
            {
                var clients = await Task.Run(() => _clientService.GetAllClientsAsync());

                // Обновляем ObservableCollection в UI-потоке
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Clear();
                    foreach (var c in clients)
                        Clients.Add(c);
                });

                StatusMessage = $"Загружено клиентов: {Clients.Count}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddClient()
        {
            var newClient = new ClientModel
            {
                FullName   = "Новый клиент",
                Phone      = string.Empty,
                Email      = string.Empty,
                ClientType = ClientType.Regular
            };
            Clients.Add(newClient);
            SelectedClient = newClient;
            StatusMessage = "Добавлен новый клиент. Заполните данные и сохраните.";
        }

        private async Task SaveClientAsync()
        {
            if (SelectedClient is null) return;

            IsLoading = true;
            StatusMessage = "Сохранение...";
            try
            {
                // Переносим данные из формы в модель
                SelectedClient.FullName   = EditFullName;
                SelectedClient.Phone      = EditPhone;
                SelectedClient.Email      = EditEmail;
                SelectedClient.ClientType = EditClientType;

                if (SelectedClient.Id == 0)
                    await _clientService.AddClientAsync(SelectedClient);
                else
                    await _clientService.UpdateClientAsync(SelectedClient);

                StatusMessage = "Изменения сохранены.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка сохранения: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteClientAsync()
        {
            if (SelectedClient is null) return;

            IsLoading = true;
            StatusMessage = "Удаление...";
            try
            {
                var toDelete = SelectedClient;
                if (toDelete.Id != 0)
                    await _clientService.DeleteClientAsync(toDelete.Id);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Remove(toDelete);
                    SelectedClient = Clients.FirstOrDefault();
                });

                StatusMessage = "Клиент удалён.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка удаления: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LoadEditForm(ClientModel? client)
        {
            if (client is null)
            {
                EditFullName   = string.Empty;
                EditPhone      = string.Empty;
                EditEmail      = string.Empty;
                EditClientType = ClientType.Regular;
                return;
            }
            EditFullName   = client.FullName;
            EditPhone      = client.Phone;
            EditEmail      = client.Email;
            EditClientType = client.ClientType;
        }
    }
}
