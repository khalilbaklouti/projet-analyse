using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MeetingScheduler
{
    public partial class MainWindow : Window
    {
        // Using "readonly" to indicate that the dictionary is initialized only once
        private readonly Dictionary<DateTime, List<string>> _employeeAvailability = new Dictionary<DateTime, List<string>>()
        {
            { new DateTime(2024, 3, 10), new List<string> { "Alice", "Bob" } },
            { new DateTime(2024, 3, 11), new List<string> { "Charlie", "Dave" } }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckAvailability_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousData(); // Refactored clearing logic into its method for better readability

            DateTime? selectedDate = meetingDate.SelectedDate;
            string selectedTimeString = meetingTime.Text;
            if (selectedDate.HasValue && TimeSpan.TryParse(selectedTimeString, out TimeSpan selectedTime))
            {
                DisplayMeetingDetails(selectedDate.Value, selectedTime); // Refactored display logic into its method
                UpdateEmployeeAvailability(selectedDate.Value);
            }
            else
            {
                MessageBox.Show("Please enter a valid date and time.");
            }
        }

        // New method to clear previous selections and details
        private void ClearPreviousData()
        {
            selectedEmployees.Items.Clear();
            availableEmployees.Items.Clear();
        }

        // New method to display meeting details
        private void DisplayMeetingDetails(DateTime selectedDate, TimeSpan selectedTime)
        {
            detailsTitle.Text = $"Title: {meetingTitle.Text}";
            detailsDate.Text = $"Date: {selectedDate:yyyy-MM-dd}";
            detailsTime.Text = $"Time: {selectedTime}";
        }

        // New method to update employee availability based on the selected date
        private void UpdateEmployeeAvailability(DateTime selectedDate)
        {
            if (_employeeAvailability.TryGetValue(selectedDate, out List<string> employees))
            {
                foreach (var employeeName in employees)
                {
                    availableEmployees.Items.Add(new Employee { Name = employeeName });
                }
            }
            else
            {
                availableEmployees.Items.Add(new Employee { Name = "No employees available" });
            }
        }

        private void AvailableEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Employee employee in e.AddedItems)
            {
                selectedEmployees.Items.Add(employee);
            }
        }
    }

    public class Employee
    {
        public string Name { get; set; }
    }
}
