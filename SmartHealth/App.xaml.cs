using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace SmartHealth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        private User? _user = null;
        private bool _userLoggedAsAdmin = false;

        public User? User 
        {
            get 
            { 
                return _user; 
            }
            set 
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public static bool Good { get; set; }

        public bool UserLoggedAsAdmin 
        { 
            get
            {
                return _userLoggedAsAdmin;
            }
            set
            {
                _userLoggedAsAdmin = value;
                OnPropertyChanged(nameof(UserLoggedAsAdmin));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
