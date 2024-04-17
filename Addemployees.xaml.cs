using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient; // Make sure to use MySql.Data.MySqlClient

namespace meetingtracker
{
    public partial class Addemployees : Page
    {
        public ObservableCollection<ScheduleEntry> ScheduleEntries { get; } = new ObservableCollection<ScheduleEntry>();

        public Addemployees()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadScheduleEntries();
        }
        public static class DatabaseManager
        {
            private static string connectionString = "Server=127.0.0.1;Database=test;Uid=root;Pwd=;";

            public static MySqlConnection GetConnection()
            {
                try
                {
                    MySqlConnection connection = new MySqlConnection(connectionString);
                    return connection;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("An error occurred connecting to the database: " + ex.Message);
                    return null;
                }
            }
        }

        private void LoadScheduleEntries()
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Name, Day, StartTime, EndTime, BreakTime, EndBreakTime, IsAvailable FROM availability";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            ScheduleEntries.Clear(); // Clear existing entries before loading new ones.
                            while (reader.Read())
                            {
                                var scheduleEntry = new ScheduleEntry
                                {
                                    Name = reader["Name"].ToString(),
                                    Day = reader["Day"].ToString(),
                                    StartTime = reader["StartTime"] == DBNull.Value ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                                    EndTime = reader["EndTime"] == DBNull.Value ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                                    BreakTime = reader["BreakTime"] == DBNull.Value ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("BreakTime")),
                                    EndBreakTime = reader["EndBreakTime"] == DBNull.Value ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("EndBreakTime")),
                                    IsAvailable = reader["IsAvailable"] == DBNull.Value ? false : reader.GetBoolean(reader.GetOrdinal("IsAvailable"))
                                };

                                ScheduleEntries.Add(scheduleEntry);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading schedule entries: " + ex.Message);
                }
            }
        }
        private void AddEmployeeAvailability(string name, string day, TimeSpan startTime, TimeSpan endTime, TimeSpan breakTime, TimeSpan endBreakTime)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO availability 
                        (Name, Day, StartTime, EndTime, BreakTime, EndBreakTime, IsAvailable) 
                        VALUES 
                        (@Name, @Day, @StartTime, @EndTime, @BreakTime, @EndBreakTime, @IsAvailable)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Day", day);
                        command.Parameters.AddWithValue("@StartTime", startTime);
                        command.Parameters.AddWithValue("@EndTime", endTime);
                        command.Parameters.AddWithValue("@BreakTime", breakTime);
                        command.Parameters.AddWithValue("@EndBreakTime", endBreakTime);
                        command.Parameters.AddWithValue("@IsAvailable", true); // Assuming default availability is true when added

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            LoadScheduleEntries();
        }

       


        private void RemoveEmployeeAvailabilityFromDatabase(ScheduleEntry entry)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    var query = @"
                DELETE FROM availability 
                WHERE Name = @Name AND Day = @Day AND StartTime = @StartTime AND EndTime = @EndTime
                AND BreakTime = @BreakTime AND EndBreakTime = @EndBreakTime";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", entry.Name);
                        command.Parameters.AddWithValue("@Day", entry.Day);
                        command.Parameters.AddWithValue("@StartTime", entry.StartTime);
                        command.Parameters.AddWithValue("@EndTime", entry.EndTime);
                        command.Parameters.AddWithValue("@BreakTime", entry.BreakTime);
                        command.Parameters.AddWithValue("@EndBreakTime", entry.EndBreakTime);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when removing employee availability: " + ex.Message);
                }
            }
        }

       
        public class ScheduleEntry
        {
            public string Name { get; set; }
            public string Day { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public TimeSpan BreakTime { get; set; }
            public TimeSpan EndBreakTime { get; set; }
            public bool IsAvailable { get; set; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = employeeNameTextBox.Text;
            string day = (dayComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(day))
            {
                MessageBox.Show("Please enter the employee's name and select a day.");
                return;
            }

            MarkEmployeeAsNotAvailable(name, day);
        }

        private void MarkEmployeeAsNotAvailable(string name, string day)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    var query = @"
                UPDATE availability 
                SET IsAvailable = 0 
                WHERE Name = @Name AND Day = @Day";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Day", day);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("No matching employee availability found to update.");
                        }
                        else
                        {
                            MessageBox.Show("The employee has been marked as not available.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when updating employee availability: " + ex.Message);
                }
            }
            LoadScheduleEntries();
        }
    

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            // Add employee availability logic
            string name = employeeNameTextBox.Text;
            string day = (dayComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty;

            if (!TimeSpan.TryParse(startTimeTextBox.Text, out var startTime) ||
                !TimeSpan.TryParse(endTimeTextBox.Text, out var endTime) ||
                !TimeSpan.TryParse(breakStartTimeTextBox.Text, out var breakTime) ||
                !TimeSpan.TryParse(breakEndTimeTextBox.Text, out var endBreakTime))
            {
                MessageBox.Show("Please enter valid times in HH:MM format.");
                return;
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(day))
            {
                MessageBox.Show("Please enter a name and select a day.");
                return;
            }

            AddEmployeeAvailability(name, day, startTime, endTime, breakTime, endBreakTime);
        
    }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            // Remove selected employee(s) logic
            var selectedEntries = dataGrid.SelectedItems.Cast<ScheduleEntry>().ToList();
            if (selectedEntries.Any())
            {
                foreach (var entry in selectedEntries)
                {
                    ScheduleEntries.Remove(entry);
                    RemoveEmployeeAvailabilityFromDatabase(entry);
                }
            }
            else
            {
                MessageBox.Show("Please select one or more employees to remove.");
            }
        }
    }
}