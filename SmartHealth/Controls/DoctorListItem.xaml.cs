using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Логика взаимодействия для Doctor.xaml
    /// </summary>
    

    public class DoctorViewModel : MainViewModel
    {
        public Doctor? Doctor { get; set; } = null;
        public BitmapImage? Photo { get; set; } = null;
    }

    public partial class DoctorListItem : UserControl
    {
        public event Action<Doctor>? OnDelete;

        private readonly Doctor _doctor;

        public DoctorListItem(Doctor doctor)
        {
            _doctor = doctor;

            InitializeComponent();
            
            using var memoryStream = new MemoryStream(Convert.FromBase64String(doctor.Photo));

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            var mainViewModel = ((App)Application.Current).MainViewModel;

            DataContext = new DoctorViewModel() 
            {
                User = mainViewModel.User, 
                Doctor = doctor,
                UserLoggedAsAdmin = mainViewModel.UserLoggedAsAdmin,
                Photo = bitmapImage
            };
        }

        private void MakeAnAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavigateMakeAnAppointmentPage();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke(_doctor);
        }
    }
}
