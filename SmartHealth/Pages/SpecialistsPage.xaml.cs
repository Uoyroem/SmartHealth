using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHealth.Pages
{
    /// <summary>
    /// Логика взаимодействия для SpecialistsPage.xaml
    /// </summary>
    public partial class SpecialistsPage : Page
    {
        public SpecialistsPage()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).MainViewModel;
            RefreshDatas();
        }

        public void RefreshDatas()
        {
            using var dbContext = new SmartHealthDbContext();

            DoctorUserComboBox.ItemsSource = dbContext.Users.ToList();
            DoctorSpecialityComboBox.ItemsSource = dbContext.Specialities.ToList();

            foreach (var doctor in dbContext.Doctors.ToList())
            {
                using var memoryStream = new MemoryStream(Convert.FromBase64String(doctor.Photo));

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                var image = new Image()
                {
                    Source = bitmapImage,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 200,
                };

                var textBox = new TextBlock()
                {
                    Text = doctor.User.Username
                };

                textBox.SetValue(Grid.ColumnProperty, 1);

                var grid = new Grid()
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(),
                        new ColumnDefinition(),
                        new ColumnDefinition(),
                    },
                    Children =
                    {
                        image, textBox
                    },
                };
                DoctorsListBox.Items.Add(grid);
            }
        }

        private void ShowLogs(StackPanel stackPanel, List<string>? errors = null, List<string>? infos = null)
        {
            stackPanel.Children.Clear();
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    var textBox = new TextBox()
                    {
                        Text = error,
                        Foreground = Brushes.Red,
                        Margin = new Thickness(0, 0, 0, 5)
                    };
                    stackPanel.Children.Add(textBox);
                    var doubleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(1.5));
                    doubleAnimation.BeginTime = TimeSpan.FromSeconds(3);

                    textBox.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
                }
            }

            if (infos != null)
            {
                foreach (var info in infos)
                {
                    var textBox = new TextBox()
                    {
                        Text = info,
                        Foreground = Brushes.Green,
                        Margin = new Thickness(0, 0, 0, 5)
                    };
                    stackPanel.Children.Add(textBox);
                    var doubleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(1.5));
                    doubleAnimation.BeginTime = TimeSpan.FromSeconds(3);

                    textBox.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
                }
            }

        }

        private void SpecialityCreationButton_Click(object sender, RoutedEventArgs e)
        {
            var name = SpecialityNameTextBox.Text;
            using var dbContext = new SmartHealthDbContext();
            dbContext.Specialities.Add(new Speciality() { Name = name });
            dbContext.SaveChanges();

            SpecialityNameTextBox.Text = "";

            ShowLogs(SpecialityCreationLogsStackPanel, infos: new List<string>() { $"'Специальность - {name}' успешно создана." });

            RefreshDatas();
        }

        private void DoctorPhotoPathSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DoctorPhotoPath.Text = openFileDialog.FileName;
            }
        }

        private bool CheckDoctorCreationDatas(out List<string> errors, out UIElement elementToFocus)
        {
            errors = new();
            elementToFocus = null!;
            if (!File.Exists(DoctorPhotoPath.Text))
            {
                errors.Add($"Фотографий по пути {DoctorPhotoPath.Text} не существует.");
                elementToFocus = DoctorPhotoPath;
            }
            if (DoctorUserComboBox.SelectedItem == null)
            {
                errors.Add($"Укажите пользователя.");
                elementToFocus = DoctorUserComboBox;
            }
            if (DoctorSpecialityComboBox.SelectedItem == null)
            {
                errors.Add($"Укажите специальность.");
                elementToFocus = DoctorSpecialityComboBox;
            }
            if (DoctorWorkExperienceTextBox.Text == "")
            {
                errors.Add($"Укажите опыт работы.");
                elementToFocus = DoctorWorkExperienceTextBox;
            }
            if (DoctorAcademicDegreeTextBox.Text == "")
            {
                errors.Add($"Укажите ученую степень.");
                elementToFocus = DoctorWorkExperienceTextBox;
            }

            return elementToFocus == null;
        }

        private void DoctorCreationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckDoctorCreationDatas(out var errors, out var elementToFocus))
            {
                elementToFocus.Focus();
                ShowLogs(DoctorCreationLogsStackPanel, errors);
                return;
            }

            var user = (User)DoctorUserComboBox.SelectedItem;
            var speciality = (Speciality)DoctorSpecialityComboBox.SelectedItem;
            var workExperience = int.Parse(DoctorWorkExperienceTextBox.Text);
            string photo = Convert.ToBase64String(File.ReadAllBytes(DoctorPhotoPath.Text));
            var academicDegree = DoctorAcademicDegreeTextBox.Text;

            using var dbContext = new SmartHealthDbContext();
            dbContext.Doctors.Add(new Doctor()
            {
                UserId = user.Id,
                SpecialityId = speciality.Id,
                WorkExperience = workExperience,
                Photo = photo,
                AcademicDegree = academicDegree
            });
            dbContext.SaveChanges();

            ShowLogs(DoctorCreationLogsStackPanel, infos: new List<string>() { "Врач создан!" });
            DoctorUserComboBox.SelectedItem = DoctorSpecialityComboBox.SelectedItem = null;
            DoctorWorkExperienceTextBox.Text = DoctorPhotoPath.Text = DoctorAcademicDegreeTextBox.Text = "";
            RefreshDatas();
        }
    }
}
