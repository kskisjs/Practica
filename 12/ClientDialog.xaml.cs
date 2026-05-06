using System.Windows;

namespace CRMApp
{
    public partial class ClientDialog : Window
    {
        // Эти свойства будут хранить данные, которые ввёл пользователь
        public string FullName { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }

        /// <summary>
        /// Конструктор для ДОБАВЛЕНИЯ нового клиента
        /// </summary>
        public ClientDialog()
        {
            InitializeComponent();
            // Меняем заголовок кнопки для режима добавления
            btnOk.Content = "✅ Добавить";
        }

        /// <summary>
        /// Конструктор для РЕДАКТИРОВАНИЯ существующего клиента
        /// </summary>
        public ClientDialog(Client client)
        {
            InitializeComponent();

            // Меняем заголовок кнопки для режима редактирования
            btnOk.Content = "💾 Сохранить";

            // Заполняем поля текущими данными клиента
            txtFullName.Text = client.FullName;
            txtPhone.Text = client.Phone;
            txtEmail.Text = client.Email;
        }

        /// <summary>
        /// Кнопка "Добавить / Сохранить" - сохраняет данные и закрывает окно
        /// </summary>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // ПРОВЕРКА: ФИО не должно быть пустым
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("❌ Ошибка: Введите ФИО клиента!",
                                "Проверка данных",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return; // Выходим из метода, не закрываем окно
            }

            // ПРОВЕРКА: Телефон не должен быть пустым
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("❌ Ошибка: Введите номер телефона!",
                                "Проверка данных",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Сохраняем данные в свойства (убираем лишние пробелы по краям)
            FullName = txtFullName.Text.Trim();
            Phone = txtPhone.Text.Trim();
            Email = txtEmail.Text.Trim();

            // Устанавливаем DialogResult = true - это значит "пользователь нажал OK"
            // Главное окно получит true и поймёт, что нужно добавить клиента
            DialogResult = true;

            // Закрываем окно
            Close();
        }

        /// <summary>
        /// Кнопка "Отмена" - закрывает окно без сохранения
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // DialogResult = false - значит "пользователь отменил"
            DialogResult = false;
            Close();
        }
    }
}