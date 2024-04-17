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
using static meetingtracker.ScheduleMeetings;
using MySqlConnector;



namespace meetingtracker
{
    /// <summary>
    /// Logique d'interaction pour Final_Calendar.xaml
    /// </summary>
    public partial class Final_Calendar : Page
    {
        public ObservableCollection<MeetingInfo> Meetings { get; } = new ObservableCollection<MeetingInfo>();

        public Final_Calendar()
        {
            
            this.DataContext = this;
            LoadMeetings();
        }

        private void LoadMeetings()
        {
            var connection = DatabaseManager.GetConnection();
            try
            {
                connection.Open();
                var query = @"
                SELECT m.MeetingID, m.Title, m.MeetingDate, m.MeetingTime, e.EmployeeID, e.Name
                FROM meetings m
                LEFT JOIN employee_meeting em ON m.MeetingID = em.MeetingID
                LEFT JOIN employees e ON em.EmployeeID = e.EmployeeID
                WHERE WEEK(m.MeetingDate) = WEEK(CURDATE()) AND YEAR(m.MeetingDate) = YEAR(CURDATE());";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var meetingId = reader.GetInt32("MeetingID");
                            var meeting = Meetings.FirstOrDefault(m => m.MeetingID == meetingId);
                            if (meeting == null)
                            {
                                meeting = new MeetingInfo
                                {
                                    MeetingID = meetingId,
                                    Title = reader.GetString("Title"),
                                    MeetingDate = reader.GetDateTime("MeetingDate"),
                                    MeetingTime = reader.GetTimeSpan("MeetingTime").ToString(@"hh\:mm")
                                };
                                Meetings.Add(meeting);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("EmployeeID")))
                            {
                                meeting.Employees.Add(new Employee
                                {
                                    EmployeeID = reader.GetInt32("EmployeeID"),
                                    Name = reader.GetString("Name")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading meetings: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public class MeetingInfo
        {
            public int MeetingID { get; set; }
            public string Title { get; set; }
            public DateTime MeetingDate { get; set; }
            public string MeetingTime { get; set; }
            public ObservableCollection<Employee> Employees { get; } = new ObservableCollection<Employee>();
            public string EmployeesDisplay => string.Join(", ", Employees.Select(e => e.Name));
        }
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
    }

    // This class could be somewhere else in your project, or within this page if you prefer.
    public static class DatabaseManager
    {
        private static string connectionString = "Server=127.0.0.1;Database=test;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
