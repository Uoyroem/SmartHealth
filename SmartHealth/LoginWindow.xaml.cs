using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartHealth
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;
            var loginAsAdmin = LoginAsAdminCheckBox.IsChecked == true;

            var errors = new List<string>();

            using (var dbContext = new SmartHealthDbContext())
            {
                var user = dbContext.Users.FirstOrDefault((User user) => user.Username == username && user.Password == password);

                if (user != null)
                {
                    if (!user.IsAdmin && loginAsAdmin)
                    {
                        errors.Add($"{username} не является админом. Так что вы не можете войти как админ.");
                    }
                    else
                    {
                        var app = (App)Application.Current;
                        app.User = user;
                        app.UserLoggedAsAdmin = loginAsAdmin;
                        Close();
                    }
                } 
                else
                {
                    errors.Add($"Имя пользователя или пароль не верен.");
                }

                ErrorsStackPanel.Children.Clear();

                foreach (var error in errors)
                {
                    ErrorsStackPanel.Children.Add(new TextBlock()
                    {
                        Text = error,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 8),
                        Foreground = Brushes.Red
                    });
                }
            }
        }
    }
}
