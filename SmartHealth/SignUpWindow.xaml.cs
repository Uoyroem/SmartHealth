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
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private bool СheckUsername(string username, List<string> errors)
        { 
            if (username.Length < 2) 
            {
                errors.Add("Длина имени пользователя должно быть больше 2 символов.");
            }
            return errors.Count == 0;
        }

        private bool CheckPassword(string password, List<string> errors)
        {
            if (password.Length < 8) 
            {
                errors.Add("Длина пароля должна быть больше 8 символов.");
            }
            return errors.Count == 0;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        { 

            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;
            var isAdmin = RegisterAsAdminCheckBox.IsChecked == true;

            var errors = new List<string>();

            if (!CheckPassword(password, errors) || !СheckUsername(username, errors)) 
            {
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

                return;
            }

            using (var dbContext = new SmartHealthDbContext())
            {
                

                var user = new User()
                {
                    Username = username,
                    Password = password,
                    IsAdmin = isAdmin
                };

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
            
            Close();

        }
    }
}
