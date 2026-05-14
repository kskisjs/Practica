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
        private readonly IAuthService _authService;
        private readonly NotificationService _notificationService;
        private readonly FileWatcherService _watcher;
        private readonly PipeMessenger _pipeMessenger;

        // ── Текущий пользователь ────────────────────────────────────────────
        public UserModel? CurrentUser => _authService.CurrentUser;
        public bool IsAdmin => CurrentUser?.IsAdmin ?? false;

        // ── Вкладки ────────────────────────────────────────────────────────
        private int _selectedTab = 0;
        public int SelectedTab
        {
            get => _selectedTab;
            set => SetField(ref _selectedTab, value);
        }

        // ── Клиенты ────────────────────────────────────────────────────────
        public ObservableCollection<ClientModel> Clients { get; } = new();

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetField(ref _searchText, value); FilterClients(); }
        }

        public ObservableCollection<ClientModel> FilteredClients { get; } = new();

        private ClientModel? _selectedClient;
        public ClientModel? SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (SetField(ref _selectedClient, value))
                {
                    LoadEditForm(value);
                    _ = LoadOrdersForClientAsync(value?.Id);
                }
            }
        }

        // ── Поля редактирования клиента ────────────────────────────────────
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

        private string _editCompany = string.Empty;
        public string EditCompany
        {
            get => _editCompany;
            set => SetField(ref _editCompany, value);
        }

        private string _editNotes = string.Empty;
        public string EditNotes
        {
            get => _editNotes;
            set => SetField(ref _editNotes, value);
        }

        private ClientType _editClientType;
        public ClientType EditClientType
        {
            get => _editClientType;
            set => SetField(ref _editClientType, value);
        }

        public Array ClientTypeValues => Enum.GetValues(typeof(ClientType));

        // ── Заказы ─────────────────────────────────────────────────────────
        public ObservableCollection<OrderModel> ClientOrders { get; } = new();

        private OrderModel? _selectedOrder;
        public OrderModel? SelectedOrder
        {
            get => _selectedOrder;
            set { SetField(ref _selectedOrder, value); LoadOrderEditForm(value); }
        }

        private string _editOrderDesc = string.Empty;
        public string EditOrderDesc
        {
            get => _editOrderDesc;
            set => SetField(ref _editOrderDesc, value);
        }

        private decimal _editOrderAmount;
        public decimal EditOrderAmount
        {
            get => _editOrderAmount;
            set => SetField(ref _editOrderAmount, value);
        }

        private OrderStatus _editOrderStatus;
        public OrderStatus EditOrderStatus
        {
            get => _editOrderStatus;
            set => SetField(ref _editOrderStatus, value);
        }

        public Array OrderStatusValues => Enum.GetValues(typeof(OrderStatus));

        // ── Уведомления ────────────────────────────────────────────────────
        public ObservableCollection<AppNotification> Notifications { get; } = new();

        private int _unreadCount;
        public int UnreadCount
        {
            get => _unreadCount;
            set => SetField(ref _unreadCount, value);
        }

        // ── Статус / загрузка ──────────────────────────────────────────────
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        private string _statusMessage = "Готово";
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        // ── Команды ────────────────────────────────────────────────────────
        public ICommand LoadClientsCommand   { get; }
        public ICommand AddClientCommand     { get; }
        public ICommand SaveClientCommand    { get; }
        public ICommand DeleteClientCommand  { get; }

        public ICommand AddOrderCommand      { get; }
        public ICommand SaveOrderCommand     { get; }
        public ICommand DeleteOrderCommand   { get; }

        public ICommand MarkNotificationsReadCommand { get; }
        public ICommand RefreshNotificationsCommand  { get; }

        // ── Конструктор ────────────────────────────────────────────────────
        public CRMViewModel(
            IClientService clientService,
            IAuthService authService,
            NotificationService notificationService,
            FileWatcherService watcher,
            PipeMessenger pipeMessenger)
        {
            _clientService       = clientService;
            _authService         = authService;
            _notificationService = notificationService;
            _watcher             = watcher;
            _pipeMessenger       = pipeMessenger;

            LoadClientsCommand   = new AsyncRelayCommand(LoadClientsAsync);
            AddClientCommand     = new RelayCommand(AddClient);
            SaveClientCommand    = new AsyncRelayCommand(SaveClientAsync,  () => SelectedClient != null);
            DeleteClientCommand  = new AsyncRelayCommand(DeleteClientAsync, () => SelectedClient != null);

            AddOrderCommand     = new RelayCommand(AddOrder,     () => SelectedClient != null);
            SaveOrderCommand    = new AsyncRelayCommand(SaveOrderAsync,   () => SelectedOrder != null);
            DeleteOrderCommand  = new AsyncRelayCommand(DeleteOrderAsync, () => SelectedOrder != null);

            MarkNotificationsReadCommand = new RelayCommand(MarkAllRead);
            RefreshNotificationsCommand  = new RelayCommand(ReloadNotifications);

            // FileSystemWatcher: авто-обновление при изменении crm_data.json
            _watcher.CrmDataChanged += () =>
                Application.Current?.Dispatcher.Invoke(() => _ = LoadClientsAsync());

            // Named Pipe: входящие сообщения от других пользователей
            _pipeMessenger.MessageReceived += OnPipeMessage;
            _pipeMessenger.StartListening();

            _ = LoadClientsAsync();
            ReloadNotifications();
        }

        // ── Клиенты ────────────────────────────────────────────────────────

        private async Task LoadClientsAsync()
        {
            IsLoading = true;
            StatusMessage = "Загрузка клиентов...";
            try
            {
                var clients = await Task.Run(() => _clientService.GetAllClientsAsync());
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Clear();
                    foreach (var c in clients) Clients.Add(c);
                    FilterClients();
                });
                StatusMessage = $"Клиентов: {Clients.Count}";
            }
            catch (Exception ex) { StatusMessage = $"Ошибка: {ex.Message}"; }
            finally { IsLoading = false; }
        }

        private void FilterClients()
        {
            FilteredClients.Clear();
            var query = string.IsNullOrWhiteSpace(SearchText)
                ? Clients
                : Clients.Where(c =>
                    c.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Company.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var c in query) FilteredClients.Add(c);
        }

        private void AddClient()
        {
            var newClient = new ClientModel
            {
                FullName = "Новый клиент",
                ClientType = ClientType.Regular
            };
            Clients.Add(newClient);
            FilterClients();
            SelectedClient = newClient;

            // Уведомление + pipe
            AddNotification(NotificationType.NewClient, "Новый клиент", $"Добавлен новый клиент.");
            PipeMessenger.Send(new PipeMessage { Type = "NewClient", Sender = CurrentUser?.Username ?? "", Payload = "Новый клиент" });

            StatusMessage = "Новый клиент. Заполните данные и сохраните.";
        }

        private async Task SaveClientAsync()
        {
            if (SelectedClient is null) return;
            IsLoading = true;
            StatusMessage = "Сохранение...";
            try
            {
                SelectedClient.FullName   = EditFullName;
                SelectedClient.Phone      = EditPhone;
                SelectedClient.Email      = EditEmail;
                SelectedClient.Company    = EditCompany;
                SelectedClient.Notes      = EditNotes;
                SelectedClient.ClientType = EditClientType;

                if (SelectedClient.Id == 0)
                    await _clientService.AddClientAsync(SelectedClient);
                else
                    await _clientService.UpdateClientAsync(SelectedClient);

                FilterClients();
                StatusMessage = "Сохранено.";
            }
            catch (Exception ex) { StatusMessage = $"Ошибка: {ex.Message}"; }
            finally { IsLoading = false; }
        }

        private async Task DeleteClientAsync()
        {
            if (SelectedClient is null) return;
            IsLoading = true;
            try
            {
                var toDelete = SelectedClient;
                if (toDelete.Id != 0)
                    await _clientService.DeleteClientAsync(toDelete.Id);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Remove(toDelete);
                    FilterClients();
                    SelectedClient = FilteredClients.FirstOrDefault();
                });
                StatusMessage = "Клиент удалён.";
            }
            catch (Exception ex) { StatusMessage = $"Ошибка: {ex.Message}"; }
            finally { IsLoading = false; }
        }

        private void LoadEditForm(ClientModel? c)
        {
            if (c is null) { EditFullName = EditPhone = EditEmail = EditCompany = EditNotes = string.Empty; return; }
            EditFullName   = c.FullName;
            EditPhone      = c.Phone;
            EditEmail      = c.Email;
            EditCompany    = c.Company;
            EditNotes      = c.Notes;
            EditClientType = c.ClientType;
        }

        // ── Заказы ─────────────────────────────────────────────────────────

        private async Task LoadOrdersForClientAsync(int? clientId)
        {
            ClientOrders.Clear();
            if (clientId == null || clientId == 0) return;
            var orders = await _clientService.GetOrdersByClientAsync(clientId.Value);
            foreach (var o in orders) ClientOrders.Add(o);
            SelectedOrder = ClientOrders.FirstOrDefault();
        }

        private void AddOrder()
        {
            if (SelectedClient == null) return;
            var order = new OrderModel
            {
                ClientId    = SelectedClient.Id,
                Description = "Новый заказ",
                Amount      = 0,
                Status      = OrderStatus.New,
                CreatedAt   = DateTime.Now
            };
            ClientOrders.Add(order);
            SelectedOrder = order;

            AddNotification(NotificationType.NewOrder, "Новая заявка",
                $"Новый заказ для {SelectedClient.FullName}");
            PipeMessenger.Send(new PipeMessage
            {
                Type    = "NewOrder",
                Sender  = CurrentUser?.Username ?? "",
                Payload = $"Новый заказ: {SelectedClient.FullName}"
            });

            StatusMessage = "Новый заказ. Заполните и сохраните.";
        }

        private async Task SaveOrderAsync()
        {
            if (SelectedOrder is null || SelectedClient is null) return;
            IsLoading = true;
            try
            {
                SelectedOrder.Description = EditOrderDesc;
                SelectedOrder.Amount      = EditOrderAmount;

                var oldStatus = SelectedOrder.Status;
                SelectedOrder.Status = EditOrderStatus;

                if (SelectedOrder.Id == 0)
                {
                    SelectedOrder.ClientId = SelectedClient.Id;
                    await _clientService.AddOrderAsync(SelectedOrder);
                }
                else
                {
                    await _clientService.UpdateOrderAsync(SelectedOrder);
                }

                if (oldStatus != EditOrderStatus)
                {
                    AddNotification(NotificationType.OrderStatusChanged, "Статус изменён",
                        $"Заказ #{SelectedOrder.Id}: {oldStatus} → {EditOrderStatus}");
                    PipeMessenger.Send(new PipeMessage
                    {
                        Type    = "OrderStatusChanged",
                        Sender  = CurrentUser?.Username ?? "",
                        Payload = $"Заказ #{SelectedOrder.Id} → {EditOrderStatus}"
                    });
                }

                await LoadOrdersForClientAsync(SelectedClient.Id);
                StatusMessage = "Заказ сохранён.";
            }
            catch (Exception ex) { StatusMessage = $"Ошибка: {ex.Message}"; }
            finally { IsLoading = false; }
        }

        private async Task DeleteOrderAsync()
        {
            if (SelectedOrder is null) return;
            try
            {
                if (SelectedOrder.Id != 0)
                    await _clientService.DeleteOrderAsync(SelectedOrder.Id);
                await LoadOrdersForClientAsync(SelectedClient?.Id);
                StatusMessage = "Заказ удалён.";
            }
            catch (Exception ex) { StatusMessage = $"Ошибка: {ex.Message}"; }
        }

        private void LoadOrderEditForm(OrderModel? o)
        {
            if (o is null) { EditOrderDesc = string.Empty; EditOrderAmount = 0; return; }
            EditOrderDesc   = o.Description;
            EditOrderAmount = o.Amount;
            EditOrderStatus = o.Status;
        }

        // ── Уведомления ────────────────────────────────────────────────────

        private void AddNotification(NotificationType type, string title, string msg)
        {
            var n = new AppNotification { Type = type, Title = title, Message = msg };
            _notificationService.Add(n);
            ReloadNotifications();
        }

        private void ReloadNotifications()
        {
            var list = _notificationService.GetAll();
            Notifications.Clear();
            foreach (var n in list) Notifications.Add(n);
            UnreadCount = list.Count(n => !n.IsRead);
        }

        private void MarkAllRead()
        {
            _notificationService.MarkAllRead();
            ReloadNotifications();
        }

        // ── Named Pipe входящие ────────────────────────────────────────────

        private void OnPipeMessage(PipeMessage msg)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                var type = msg.Type switch
                {
                    "NewOrder"           => NotificationType.NewOrder,
                    "OrderStatusChanged" => NotificationType.OrderStatusChanged,
                    "NewClient"          => NotificationType.NewClient,
                    _                    => NotificationType.ChatMessage
                };

                AddNotification(type, $"От: {msg.Sender}", msg.Payload);
                StatusMessage = $"[{msg.Sender}]: {msg.Payload}";
            });
        }
    }
}
