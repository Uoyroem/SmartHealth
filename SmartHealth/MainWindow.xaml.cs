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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHealth
{
    public class MainWindowViewModel : ViewModel
    {
        public User? User 
        { 
            get { return _user;  }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            } 
        }

        public bool UserLoggedAsAdmin 
        {
            get { return _userLoggedAsAdmin; }
            set 
            {
                _userLoggedAsAdmin = value;
                OnPropertyChanged(nameof(UserLoggedAsAdmin));
            }
        }

        private User? _user = null;
        private bool _userLoggedAsAdmin = false;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; init; }

        public readonly Uri homePageUri = new("Pages/HomePage.xaml", UriKind.Relative);
        public readonly Uri specialistsPageUri = new("Pages/SpecialistsPage.xaml", UriKind.Relative);
        public readonly Uri servicesPageUri = new("Pages/ServicesPage.xaml", UriKind.Relative);

        public void NavigateHomePage() => Frame.Navigate(homePageUri);
        public void NavigateSpecialistsPage() => Frame.Navigate(specialistsPageUri);
        public void NavigateServicesPage() => Frame.Navigate(servicesPageUri);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new MainWindowViewModel();
        }

        private void SpecialistsNavigationButton_Click(object sender, RoutedEventArgs e) => NavigateSpecialistsPage();

        private void SmartHealthImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => NavigateHomePage();

        private void ServicesNavigationButton_Click(object sender, RoutedEventArgs e) => NavigateServicesPage();

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (OwnedWindows.OfType<SignUpWindow>().Any())
            {
                return;
            }

            SignUpWindow signUpWindow = new()
            {
                Owner = this
            };
            signUpWindow.Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (OwnedWindows.OfType<LoginWindow>().Any())
            {
                return;
            }

            LoginWindow loginWindow = new()
            {
                Owner = this
            };
            loginWindow.Show();
        }
    }
}
