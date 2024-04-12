using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ScheduleApp
{
    public partial class MainWindow : Window
    {
        // Assume you have a ScheduleEntry class that represents an entry in the DataGrid
        public ObservableCollection<ScheduleEntry> ScheduleEntries { get; } = new ObservableCollection<ScheduleEntry>();

        public MainWindow()
        {
            InitializeComponent();
            // Set the DataContext for the window to allow for data binding
            DataContext = this;
        }

        // Event handler for the 'Add Availability' button
        private void AddAvailability_Click(object sender, RoutedEventArgs e)
        {
            // Get the input values from the form
            string employeeName = employeeNameTextBox.Text;
            string selectedDay = ((ComboBoxItem)dayComboBox.SelectedItem)?.Content?.ToString();
            string startTime = startTimeTextBox.Text;
            string endTime = endTimeTextBox.Text;

            // Validate inputs (omitted for brevity)

            // Add a new entry to the collection which will update the DataGrid
            ScheduleEntries.Add(new ScheduleEntry
            {
                Name = employeeName,
                Day = selectedDay,
                StartTime = startTime,
                EndTime = endTime
            });

            // Clear the form inputs
            ClearFormInputs();
        }

        // Event handler for the 'Not Available' button
        private void MarkNotAvailable_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to mark an employee as not available
            // This is typically based on the selected row in the DataGrid
        }

        // Helper method to clear the input fields
        private void ClearFormInputs()
        {
            employeeNameTextBox.Clear();
            dayComboBox.SelectedIndex = -1; // Reset the ComboBox selection
            startTimeTextBox.Clear();
            endTimeTextBox.Clear();
        }

        // Define the ScheduleEntry class
        public class ScheduleEntry
        {
            public string Name { get; set; }
            public string Day { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            // You can add additional properties for other days of the week
        }
    }
}
