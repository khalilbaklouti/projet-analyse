using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace tracker
{
    /// <summary>
    /// Logique d'interaction pour meeting.xaml
    /// </summary>
    public partial class meeting : Page
    {
        public ObservableCollection<Employee> AvailableEmployees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<string> SelectedEmployees { get; set; } = new ObservableCollection<string>();

        public meeting()
        {
            InitializeComponent();

            InitializeComponent();
            availableEmployees.ItemsSource = AvailableEmployees;
            selectedEmployees.ItemsSource = SelectedEmployees;
        }

        private void CheckAvailability_Click(object sender, RoutedEventArgs e)
        {
            // Simulate checking availability based on selected date and time
            AvailableEmployees.Clear();
            var employees = new[] { "Alice", "Bob", "Charlie", "Dave" }; // Example employees, replace with actual data retrieval

            foreach (var employee in employees)
            {
                AvailableEmployees.Add(new Employee { Name = employee, SelectCommand = new RelayCommand(() => SelectEmployee(employee)) });
            }

            // Update meeting details
            detailsTitle.Text = "Title: " + meetingTitle.Text;
            detailsDate.Text = "Date: " + meetingDate.SelectedDate?.ToString("yyyy-MM-dd");
            detailsTime.Text = "Time: " + meetingTime.Value?.ToString(@"hh\:mm"); // Adjust format as needed
        }

        private void SelectEmployee(string name)
        {
            SelectedEmployees.Add(name);
            // Optionally, remove the employee from AvailableEmployees if they should not be reselected
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public RelayCommand SelectCommand { get; set; }
    }

    // Implementation of RelayCommand omitted for brevity, use your preferred ICommand implementation
}
