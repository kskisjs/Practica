using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CRMApp.ViewModels;

namespace CRMApp.Views
{
    public partial class MainWindow : Window
    {
        // Флаг: карточка клиента развёрнута или свёрнута
        private bool _clientCardExpanded = true;

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

            // Слушаем изменения количества уведомлений для пульсации
            mainVm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(mainVm.UnreadCount))
                    UpdateBadgePulse(mainVm.UnreadCount);

                // Встряска статус-бара при появлении ошибки в StatusMessage
                if (e.PropertyName == nameof(mainVm.StatusMessage) &&
                    mainVm.StatusMessage?.StartsWith("❌") == true)
                {
                    var shake = (Storyboard)Resources["StatusShake"];
                    shake.Begin(this, true);
                }
            };
        }

        // ── FadeIn окна при загрузке ─────────────────────────────────
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var sb = (Storyboard)Resources["WindowFadeIn"];
            sb.Begin(this, true);

            // Запускаем пульс бейджа если уже есть непрочитанные
            if (DataContext is MainWindowViewModel vm)
                UpdateBadgePulse(vm.UnreadCount);
        }

        // ── Пульсация бейджа уведомлений ────────────────────────────
        private void UpdateBadgePulse(int count)
        {
            var pulse = (Storyboard)Resources["BadgePulse"];
            if (count > 0)
                pulse.Begin(this, true);
            else
                pulse.Stop(this);
        }

        // ── SlideIn карточки при выборе клиента ─────────────────────
        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // Небольшая задержка даёт WPF время отрисовать Visibility
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, () =>
                {
                    var sb1 = (Storyboard)Resources["ClientCardSlideIn"];
                    sb1.Begin(this, true);

                    var sb2 = (Storyboard)Resources["OrdersExpandIn"];
                    sb2.Begin(this, true);
                });

                // Сбрасываем состояние свёрнутости карточки
                _clientCardExpanded = true;
                if (CardToggleArrow != null) CardToggleArrow.Text = " ▲";
                if (ClientCardBody  != null) ClientCardBody.Visibility  = Visibility.Visible;
                if (ClientCardBody2 != null) ClientCardBody2.Visibility = Visibility.Visible;
            }
        }

        // ── Разворачивание/сворачивание карточки клиента (клик на заголовок) ──
        private void ToggleClientCard(object sender, MouseButtonEventArgs e)
        {
            if (_clientCardExpanded)
            {
                // Сворачиваем
                var collapse = new Storyboard();
                var heightAnim = new DoubleAnimation(0, new Duration(System.TimeSpan.FromSeconds(0.3)))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
                };
                var opacityAnim = new DoubleAnimation(0, new Duration(System.TimeSpan.FromSeconds(0.2)));

                Storyboard.SetTarget(heightAnim,    ClientCardBody);
                Storyboard.SetTarget(opacityAnim,   ClientCardBody);
                Storyboard.SetTargetProperty(heightAnim,  new PropertyPath("MaxHeight"));
                Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("Opacity"));

                // Устанавливаем начальный MaxHeight перед анимацией
                ClientCardBody.MaxHeight  = ClientCardBody.ActualHeight;
                ClientCardBody2.MaxHeight = ClientCardBody2.ActualHeight;

                collapse.Children.Add(heightAnim);
                collapse.Children.Add(opacityAnim);
                collapse.Completed += (_, _) =>
                {
                    ClientCardBody.Visibility  = Visibility.Collapsed;
                    ClientCardBody2.Visibility = Visibility.Collapsed;
                };
                collapse.Begin();

                CardToggleArrow.Text = " ▼";
                _clientCardExpanded  = false;
            }
            else
            {
                // Разворачиваем
                ClientCardBody.Visibility  = Visibility.Visible;
                ClientCardBody2.Visibility = Visibility.Visible;
                ClientCardBody.MaxHeight   = 0;
                ClientCardBody2.MaxHeight  = 0;
                ClientCardBody.Opacity     = 0;

                var expand = new Storyboard();

                var heightAnim = new DoubleAnimation(400, new Duration(System.TimeSpan.FromSeconds(0.4)))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                var opacityAnim = new DoubleAnimation(1, new Duration(System.TimeSpan.FromSeconds(0.3)));

                Storyboard.SetTarget(heightAnim,   ClientCardBody);
                Storyboard.SetTarget(opacityAnim,  ClientCardBody);
                Storyboard.SetTargetProperty(heightAnim,  new PropertyPath("MaxHeight"));
                Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("Opacity"));

                // Синхронно анимируем второй столбец
                var heightAnim2 = new DoubleAnimation(400, new Duration(System.TimeSpan.FromSeconds(0.4)))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(heightAnim2, ClientCardBody2);
                Storyboard.SetTargetProperty(heightAnim2, new PropertyPath("MaxHeight"));

                expand.Children.Add(heightAnim);
                expand.Children.Add(opacityAnim);
                expand.Children.Add(heightAnim2);
                expand.Begin();

                CardToggleArrow.Text = " ▲";
                _clientCardExpanded  = true;
            }
        }

        // ── FadeIn при переключении вкладок ─────────────────────────
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is not TabControl) return;

            // Определяем какая вкладка активна и проигрываем FadeIn её содержимого
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, () =>
            {
                FrameworkElement? panel = null;
                if (DataContext is MainWindowViewModel vm)
                {
                    panel = vm.SelectedTab switch
                    {
                        0 => ClientsTabContent,
                        1 => ChatTabContent,
                        2 => NotifTabContent,
                        _ => null
                    };
                }

                if (panel == null) return;

                panel.Opacity = 0;
                var fadeAnim = new DoubleAnimation(0, 1, new Duration(System.TimeSpan.FromSeconds(0.3)))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                panel.BeginAnimation(OpacityProperty, fadeAnim);

                var marginAnim = new ThicknessAnimation(
                    new Thickness(-20, 0, 0, 0),
                    new Thickness(0),
                    new Duration(System.TimeSpan.FromSeconds(0.3)))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                panel.BeginAnimation(MarginProperty, marginAnim);
            });
        }

        // ── Logout ──────────────────────────────────────────────────
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

            var chatVm = new ChatViewModel(App.ChatService, App.FileWatcher, App.AuthService);
            DataContext = new MainWindowViewModel(
                new CRMViewModel(App.ClientService, App.AuthService,
                    App.NotificationService, App.FileWatcher, App.PipeMessenger),
                chatVm);
        }

        // ── Enter в чате ────────────────────────────────────────────
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

    // ════════════════════════════════════════════════════════════════
    //  Объединяющий ViewModel
    // ════════════════════════════════════════════════════════════════
    public class MainWindowViewModel : BaseViewModel
    {
        public CRMViewModel  Crm  { get; }
        public ChatViewModel Chat { get; }

        public CRMApp.Models.UserModel? CurrentUser  => Crm.CurrentUser;
        public bool    IsAdmin      => Crm.IsAdmin;
        public int     SelectedTab  { get => Crm.SelectedTab; set => Crm.SelectedTab = value; }
        public bool    IsLoading    => Crm.IsLoading;
        public string  StatusMessage => Crm.StatusMessage;
        public int     UnreadCount  => Crm.UnreadCount;

        public System.Collections.ObjectModel.ObservableCollection<CRMApp.Models.ClientModel>  FilteredClients => Crm.FilteredClients;
        public CRMApp.Models.ClientModel?  SelectedClient { get => Crm.SelectedClient; set => Crm.SelectedClient = value; }
        public string  SearchText     { get => Crm.SearchText;     set => Crm.SearchText     = value; }
        public string  EditFullName   { get => Crm.EditFullName;   set => Crm.EditFullName   = value; }
        public string  EditPhone      { get => Crm.EditPhone;      set => Crm.EditPhone      = value; }
        public string  EditEmail      { get => Crm.EditEmail;      set => Crm.EditEmail      = value; }
        public string  EditCompany    { get => Crm.EditCompany;    set => Crm.EditCompany    = value; }
        public string  EditNotes      { get => Crm.EditNotes;      set => Crm.EditNotes      = value; }
        public CRMApp.Models.ClientType EditClientType { get => Crm.EditClientType; set => Crm.EditClientType = value; }
        public System.Array ClientTypeValues => Crm.ClientTypeValues;

        public System.Collections.ObjectModel.ObservableCollection<CRMApp.Models.OrderModel> ClientOrders => Crm.ClientOrders;
        public CRMApp.Models.OrderModel?  SelectedOrder      { get => Crm.SelectedOrder;      set => Crm.SelectedOrder      = value; }
        public string    EditOrderDesc   { get => Crm.EditOrderDesc;   set => Crm.EditOrderDesc   = value; }
        public decimal   EditOrderAmount { get => Crm.EditOrderAmount; set => Crm.EditOrderAmount = value; }
        public CRMApp.Models.OrderStatus EditOrderStatus { get => Crm.EditOrderStatus; set => Crm.EditOrderStatus = value; }
        public System.Array OrderStatusValues => Crm.OrderStatusValues;

        public System.Collections.ObjectModel.ObservableCollection<CRMApp.Models.AppNotification> Notifications => Crm.Notifications;

        public ICommand LoadClientsCommand          => Crm.LoadClientsCommand;
        public ICommand AddClientCommand            => Crm.AddClientCommand;
        public ICommand SaveClientCommand           => Crm.SaveClientCommand;
        public ICommand DeleteClientCommand         => Crm.DeleteClientCommand;
        public ICommand AddOrderCommand             => Crm.AddOrderCommand;
        public ICommand SaveOrderCommand            => Crm.SaveOrderCommand;
        public ICommand DeleteOrderCommand          => Crm.DeleteOrderCommand;
        public ICommand MarkNotificationsReadCommand => Crm.MarkNotificationsReadCommand;
        public ICommand RefreshNotificationsCommand => Crm.RefreshNotificationsCommand;

        public MainWindowViewModel(CRMViewModel crm, ChatViewModel chat)
        {
            Crm  = crm;
            Chat = chat;
            Crm.PropertyChanged  += (_, e) => OnPropertyChanged(e.PropertyName);
            Chat.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        }
    }
}
