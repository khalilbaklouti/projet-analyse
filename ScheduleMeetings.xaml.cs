using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace meetingtracker
{
    /// <summary>
    /// Logique d'interaction pour ScheduleMeetings.xaml
    /// </summary>
    public partial class ScheduleMeetings : Page
    {
        public ScheduleMeetings()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static class DatabaseManager
        {
            private static string connectionString = "Server=127.0.0.1;Database=test;Uid=root;Pwd=;";


            public static MySqlConnection GetConnection()
            {
                return new MySqlConnection(connectionString);
            }
        }

        private void CheckAvailability_Click(object sender, RoutedEventArgs e)
        {
            if (!meetingDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a valid date.");
                return;
            }

            DateTime selectedDate = meetingDate.SelectedDate.Value;

            TimeSpan selectedTime;
            if (!TimeSpan.TryParse(meetingTime.Text, out selectedTime))
            {
                MessageBox.Show("Please enter a valid time in HH:MM format.");
                return;
            }
            InsertMeetingDetails(selectedDate, selectedTime, meetingTitle.Text);
            DisplayMeetingDetails(selectedDate, selectedTime);
            UpdateEmployeeAvailability(selectedDate);
        }

        private void DisplayMeetingDetails(DateTime selectedDate, TimeSpan selectedTime)
        {
            detailsTitle.Text = $"Title: {meetingTitle.Text}";
            detailsDate.Text = $"Date: {selectedDate:yyyy-MM-dd}";
            detailsTime.Text = $"Time: {selectedTime:hh\\:mm}"; // Use HH:mm for 24-hour or hh:mm for 12-hour format
        }
        private void InsertMeetingDetails(DateTime selectedDate, TimeSpan selectedTime, string title)
        {
            var connection = DatabaseManager.GetConnection();
            try
            {
                connection.Open();
                var query = "INSERT INTO meetings (Title, MeetingDate, MeetingTime) VALUES (@Title, @MeetingDate, @MeetingTime)";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@MeetingDate", selectedDate);
                    cmd.Parameters.AddWithValue("@MeetingTime", selectedTime);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when inserting meeting details: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateEmployeeAvailability(DateTime selectedDate)
        {
            availableEmployees.Items.Clear();

            var connection = DatabaseManager.GetConnection();
            try
            {
                using (var MySqlconnection = DatabaseManager.GetConnection())
                {
                    connection.Open();
                    var query = @"
            SELECT a.Name, 
                   CASE WHEN a.IsAvailable = 0 THEN 'Not Available' ELSE 'Available' END as AvailabilityStatus
            FROM availability a
            WHERE a.Day = @DayOfWeek";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@DayOfWeek", selectedDate.DayOfWeek.ToString());

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new EmployeeWithAvailability
                                {
                                    Name = reader.GetString("Name"),
                                    Availability = reader.GetString("AvailabilityStatus")
                                };
                                availableEmployees.Items.Add(employee);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error when checking availability: " + ex.Message);
            }

        }


        public class EmployeeWithAvailability
        {
            public int EmployeeID { get; set; } // Ensure this property matches your database employee ID.
            public string Name { get; set; }
            public string Availability { get; set; }
        }

        private void availableEmployees_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // This is assuming you have a DatePicker control named 'meetingDate'.
            if (meetingDate.SelectedDate.HasValue)
            {
                var selectedDay = meetingDate.SelectedDate.Value.DayOfWeek.ToString();

                var connection = DatabaseManager.GetConnection();
                if (connection == null) return; // Connection failed

                try
                {
                    var query = @"SELECT name 
                          FROM availability 
                          WHERE day = @DayOfWeek AND is_available = 1";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@DayOfWeek", selectedDay);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                availableEmployees.Items.Add(reader["name"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when checking availability: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Please select a date first.");
            }
            // Assuming you want to add the selected employee to the meeting
            selectedEmployees.Items.Clear();
            foreach (var item in availableEmployees.SelectedItems)
            {
                selectedEmployees.Items.Add(item); // This adds the entire EmployeeWithAvailability object, so ensure the DataTemplate is set correctly
            }
            selectedEmployees.Items.Clear();
            foreach (EmployeeWithAvailability employee in availableEmployees.SelectedItems)
            {
                selectedEmployees.Items.Add(employee.Name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { // Validate input first
            if (string.IsNullOrWhiteSpace(meetingTitle.Text) || meetingDate.SelectedDate == null || string.IsNullOrWhiteSpace(meetingTime.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            using (MySqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                // Start a transaction to ensure data integrity
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertMeetingSql = "INSERT INTO meetings (Title, MeetingDate, MeetingTime) VALUES (@Title, @Date, @Time);";
                    MySqlCommand insertMeetingCmd = new MySqlCommand(insertMeetingSql, conn, transaction);
                    insertMeetingCmd.Parameters.AddWithValue("@Title", meetingTitle.Text);
                    insertMeetingCmd.Parameters.AddWithValue("@Date", meetingDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    insertMeetingCmd.Parameters.AddWithValue("@Time", meetingTime.Text);
                    insertMeetingCmd.ExecuteNonQuery();

                    // Get the last inserted meeting ID
                    long meetingId = insertMeetingCmd.LastInsertedId;

                    // Prepare SQL for inserting into employee_meeting
                    string insertEmployeeMeetingSql = "INSERT INTO employee_meeting (EmployeeID, MeetingID) VALUES (@EmployeeID, @MeetingID);";
                    MySqlCommand insertEmployeeMeetingCmd = new MySqlCommand(insertEmployeeMeetingSql, conn, transaction);

                    // Insert the selected employees for the meeting
                    foreach (var item in availableEmployees.SelectedItems)
                    {
                        if (item is DataRowView row)
                        {
                            int employeeId = Convert.ToInt32(row["EmployeeID"]);

                            insertEmployeeMeetingCmd.Parameters.Clear();
                            insertEmployeeMeetingCmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                            insertEmployeeMeetingCmd.Parameters.AddWithValue("@MeetingID", meetingId);
                            insertEmployeeMeetingCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            // Handle the unexpected type or log it for debugging
                            Debug.WriteLine($"Unexpected item type: {item?.GetType().FullName ?? "null"}");
                        }
                    }

                    // Commit the transaction only after all employees have been inserted successfully
                    transaction.Commit();
                    MessageBox.Show("Meeting and attendees have been saved.");
                }
                catch (Exception ex)
                {
                    // If an error occurs, rollback the transaction
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    // Ensure connection is always closed
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
    }