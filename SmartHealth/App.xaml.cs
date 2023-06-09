using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace SmartHealth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class MainViewModel : ViewModel
    {
        public User? User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public bool UserLoggedAsAdmin
        {
            get { return _userLoggedAsAdmin; }
            set
            {
                _userLoggedAsAdmin = value;
                OnPropertyChanged();
            }
        }

        private User? _user;
        private bool _userLoggedAsAdmin = false;
    }
    public partial class App : Application
    {
        public MainViewModel MainViewModel { get; init; }

        public App() 
        {
            MainViewModel = new();
        }
    }
}
