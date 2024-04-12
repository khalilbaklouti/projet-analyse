using System.Collections.ObjectModel;
using System.Windows;

namespace MeetingScheduler
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<WeeklySchedule> WeeklySchedules { get; set; }
        public ObservableCollection<MeetingSummary> MeetingSummaries { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            WeeklySchedules = new ObservableCollection<WeeklySchedule>();
            MeetingSummaries = new ObservableCollection<MeetingSummary>();

            // Fill the collections with initial data
            PopulateWeeklySchedules();
            PopulateMeetingSummaries();

            // Set the DataContext for data binding
            DataContext = this;
        }

        private void PopulateWeeklySchedules()
        {
            // Placeholder for real data
            WeeklySchedules.Add(new WeeklySchedule { TimeDate = "John" });
            WeeklySchedules.Add(new WeeklySchedule { TimeDate = "Jane" });
            WeeklySchedules.Add(new WeeklySchedule { TimeDate = "Pamela" });
            WeeklySchedules.Add(new WeeklySchedule { TimeDate = "Corey" });
            WeeklySchedules.Add(new WeeklySchedule { TimeDate = "Ginny" });

            // Ideally, you'd have properties for each day of the week in your WeeklySchedule class
            // and you would set them here if you have initial data
        }

        private void PopulateMeetingSummaries()
        {
            // Placeholder for real data
            MeetingSummaries.Add(new MeetingSummary { Project = "Project Kickoff", DayAndTime = "Monday at 10:00 AM", Attendees = "5 Attendees" });
            MeetingSummaries.Add(new MeetingSummary { Project = "Weekly Sync", DayAndTime = "Wednesday at 2:00 PM", Attendees = "3 Attendees" });
            MeetingSummaries.Add(new MeetingSummary { Project = "Client Q&A", DayAndTime = "Friday at 1:00 PM", Attendees = "4 Attendees" });

            // You would bind this collection to your ListView in XAML
            // e.g., <ListView ItemsSource="{Binding MeetingSummaries}">
        }
    }

    public class WeeklySchedule
    {
        public string TimeDate { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        // Add more properties for other days if necessary
    }

    public class MeetingSummary
    {
        public string Project { get; set; }
        public string DayAndTime { get; set; }
        public string Attendees { get; set; }
    }
}
