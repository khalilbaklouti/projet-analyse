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
using tracker.Properties;

namespace tracker
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Meetings = new ObservableCollection<Meeting>
            {
                new Meeting { Title = "Project Kickoff", Attendees = 5, TimeDate = "Monday 10:00 AM" },
                new Meeting { Title = "Weekly Sync", Attendees = 3, TimeDate = "Wednesday 2:00 PM" },
                new Meeting { Title = "Client Q&A", Attendees = 4, TimeDate = "Friday 1:00 PM" }
            };
            // Assume Schedule is similar and populated accordingly
            Schedules = new ObservableCollection<Schedule>();
            // Populate Schedules as necessary

            this.DataContext = this;
            scheduleDataGrid.ItemsSource = Schedules;
            meetingSummaryListBox.ItemsSource = Meetings;
        }
    }

    public class Meeting
    {
        public string Title { get; set; }
        public int Attendees { get; set; }
        public string TimeDate { get; set; }
    }

    public class Schedule
    {
        // Define properties for Schedule
    }
}