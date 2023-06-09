using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SmartHealth.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
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

            DoctorSpecialityComboBox.ItemsSource = dbContext.Specialities.ToList();

            DoctorsListBox.Items.Clear();
            foreach (var doctor in dbContext.Doctors)
            {
                var doctorListItem = new DoctorListItem(doctor);
                doctorListItem.OnDelete += DoctorListItem_OnDelete;
                DoctorsListBox.Items.Add(doctorListItem);
            }

            SpecialistsListBox.Items.Clear();
            foreach (var speciality in dbContext.Specialities)
            {
                var specialityListItem = new SpecialityListItem(speciality);
                specialityListItem.OnDelete += SpecialityListItem_OnDelete;
                SpecialistsListBox.Items.Add(specialityListItem);
            }
        }

        private void SpecialityListItem_OnDelete(Speciality speciality)
        {
            using var dbContext = new SmartHealthDbContext();
            dbContext.Specialities.Remove(speciality);
            dbContext.SaveChanges();
            RefreshDatas();
        }

        private void DoctorListItem_OnDelete(Doctor doctor)
        {
            using var dbContext = new SmartHealthDbContext();
            dbContext.Doctors.Remove(doctor);
            dbContext.SaveChanges();
            RefreshDatas();
        }

        private static void ShowLogs(StackPanel stackPanel, List<string>? errors = null, List<string>? infos = null)
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
            if (DoctorNameTextBox.Text == "")
            {
                errors.Add($"Укажите пользователя.");
                elementToFocus = DoctorNameTextBox;
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

            var name = DoctorNameTextBox.Text;
            var speciality = (Speciality)DoctorSpecialityComboBox.SelectedItem;
            var workExperience = int.Parse(DoctorWorkExperienceTextBox.Text);
            string photo = Convert.ToBase64String(File.ReadAllBytes(DoctorPhotoPath.Text));
            var academicDegree = DoctorAcademicDegreeTextBox.Text;

            using var dbContext = new SmartHealthDbContext();
            dbContext.Doctors.Add(new Doctor()
            {
                Name = name,
                SpecialityId = speciality.Id,
                WorkExperience = workExperience,
                Photo = photo,
                AcademicDegree = academicDegree
            });
            dbContext.SaveChanges();

            ShowLogs(DoctorCreationLogsStackPanel, infos: new List<string>() { "Врач создан!" });
            DoctorSpecialityComboBox.SelectedItem = null;
            DoctorNameTextBox.Text = DoctorWorkExperienceTextBox.Text = DoctorPhotoPath.Text = DoctorAcademicDegreeTextBox.Text = "";
            RefreshDatas();
        }
    }
}
