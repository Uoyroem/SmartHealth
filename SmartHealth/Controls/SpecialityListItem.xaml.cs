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

namespace SmartHealth.Controls
{
    public class SpecialistListItemViewModel : MainViewModel
    {
        private Speciality? _speciality;

        public Speciality? Speciality
        {
            get { return _speciality; }
            set { _speciality = value; OnPropertyChanged(); }
        }

    }
    
    /// <summary>
    /// Логика взаимодействия для SpecialistListItem.xaml
    /// </summary>
    public partial class SpecialityListItem : UserControl, IDeleteable<Speciality>
    {
        private readonly Speciality _speciality;

        public SpecialityListItem(Speciality speciality)
        {
            _speciality = speciality;
            InitializeComponent();

            var mainViewModel = ((App)Application.Current).MainViewModel;
            var viewModel = new SpecialistListItemViewModel()
            {
                User = mainViewModel.User,
                UserLoggedAsAdmin = mainViewModel.UserLoggedAsAdmin,
                Speciality = speciality
            };
            mainViewModel.PropertyChanged += viewModel.Redirector();
            DataContext = viewModel;
        }

        public event Action<Speciality>? OnDelete;

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke(_speciality);
        }
    }
}
