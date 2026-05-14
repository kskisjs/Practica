using System.Windows;
using System.Windows.Input;
using CRMApp.ViewModels;

namespace CRMApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var chatVm = new ChatViewModel(
                App.ChatService,
                App.FileWatcher,
                App.AuthService);

            var mainVm = new MainWindowViewModel(
                new CRMViewModel(
                    App.ClientService,
                    App.AuthService,
                    App.NotificationService,
                    App.FileWatcher,
                    App.PipeMessenger),
                chatVm);

            DataContext = mainVm;
        }

        private void OnLogout(object sender, RoutedEventArgs e)
        {
            App.AuthService.Logout();
            var login = new LoginWindow();
            login.ShowDialog();

            if (!App.AuthService.IsAuthenticated)
            {
                Close();
                return;
            }

            // Пересоздаём DataContext с новым пользователем
            var chatVm = new ChatViewModel(App.ChatService, App.FileWatcher, App.AuthService);
            DataContext = new MainWindowViewModel(
                new CRMViewModel(App.ClientService, App.AuthService,
                    App.NotificationService, App.FileWatcher, App.PipeMessenger),
                chatVm);
        }

        private void OnChatKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (DataContext is MainWindowViewModel vm)
                    vm.Chat.SendCommand.Execute(null);
                e.Handled = true;
            }
        }
    }

    /// <summary>
    /// Объединяющий ViewModel для MainWindow — содержит CRM и Chat
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        public CRMViewModel Crm { get; }
        public ChatViewModel Chat { get; }

        // Проброс свойств CRM для биндинга
        public CRMApp.Models.UserModel? CurrentUser => Crm.CurrentUser;
        public bool IsAdmin => Crm.IsAdmin;
        public int SelectedTab { get => Crm.SelectedTab; set => Crm.SelectedTab = value; }
        public bool IsLoading => Crm.IsLoading;
        public string StatusMessage => Crm.StatusMessage;
        public int UnreadCount => Crm.UnreadCount;

        public System.Collections.ObjectModel.ObservableCollection<CRMApp.Models.ClientModel> FilteredClients => Crm.FilteredClients;
        public CRMApp.Models.ClientModel? SelectedClient { get => Crm.SelectedClient; set => Crm.SelectedClient = value; }
        public string SearchText { get => Crm.SearchText; set => Crm.SearchText = value; }
        public string EditFullName { get => Crm.EditFullName; set => Crm.EditFullName = value; }
        public string EditPhone { get => Crm.EditPhone; set => Crm.EditPhone = value; }
        public string EditEmail { get => Crm.EditEmail; set => Crm.EditEmail = value; }
        public string EditCompany { get => Crm.EditCompany; set => Crm.EditCompany = value; }
        public string EditNotes { get => Crm.EditNotes; set => Crm.EditNotes = value; }
        public CRMApp.Models.ClientType EditClientType { get => Crm.EditClientType; set => Crm.EditClientType = value; }
        public System.Array ClientTypeValues => Crm.ClientTypeValues;

        public System.Collections.ObjectModel.ObservableCollection<CRMApp.Models.OrderModel> ClientOrders => Crm.ClientOrders;
        public CRMApp.Models.OrderModel? SelectedOrder { get => Crm.SelectedOrder; set => Crm.SelectedOrder = value; }
        public string EditOrderDesc { get => Crm.EditOrderDesc; set => Crm.EditOrderDesc = value; }
        public decimal EditOrderAmount { get => Crm.EditOrderAmount; set => Crm.EditOrderAmount = value; }
        public CRMApp.Models.OrderStatus EditOrderStatus { get => Crm.EditOrderStatus; set => Crm.EditOrderStatus = value; }
        public System.Array OrderStatusValues => Crm.OrderStatusValues;

        public System.Collections.ObjectModel.ObservableCollection<CRMApp.Models.AppNotification> Notifications => Crm.Notifications;

        public ICommand LoadClientsCommand   => Crm.LoadClientsCommand;
        public ICommand AddClientCommand     => Crm.AddClientCommand;
        public ICommand SaveClientCommand    => Crm.SaveClientCommand;
        public ICommand DeleteClientCommand  => Crm.DeleteClientCommand;
        public ICommand AddOrderCommand      => Crm.AddOrderCommand;
        public ICommand SaveOrderCommand     => Crm.SaveOrderCommand;
        public ICommand DeleteOrderCommand   => Crm.DeleteOrderCommand;
        public ICommand MarkNotificationsReadCommand => Crm.MarkNotificationsReadCommand;
        public ICommand RefreshNotificationsCommand  => Crm.RefreshNotificationsCommand;

        public MainWindowViewModel(CRMViewModel crm, ChatViewModel chat)
        {
            Crm  = crm;
            Chat = chat;

            Crm.PropertyChanged  += (_, e) => OnPropertyChanged(e.PropertyName);
            Chat.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        }
    }
}
