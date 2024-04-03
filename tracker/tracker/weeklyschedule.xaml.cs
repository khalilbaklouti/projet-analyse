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
    /// Logique d'interaction pour weeklyschedule.xaml
    /// </summary>
    public partial class weeklyschedule : Page
    {
        public weeklyschedule()
        {
            InitializeComponent();
            EmployeeSchedules = new ObservableCollection<EmployeeSchedule>();
            scheduleListView.ItemsSource = EmployeeSchedules;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            // Show input form for new employee
        }

        private void ScheduleMeeting_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to scheduling functionality
        }

        private void FinalCalendar_Click(object sender, RoutedEventArgs e)
        {
            // Show final calendar view
        }

        // Additional methods to handle adding, editing, and marking availability
    }

    public class EmployeeSchedule
    {
        public string Name { get; set; }
        // Properties for each day's schedule
    }
}
