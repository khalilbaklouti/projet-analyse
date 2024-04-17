using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;


namespace meetingtracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
            public MainWindow()
            {
                InitializeComponent();
            }
        public static class DatabaseManager
        {
            private static string connectionString = "Server=127.0.0.1;Database=test;Uid=root;Pwd=;";


            public static MySqlConnection GetConnection()
            {
                return new MySqlConnection(connectionString);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Créer une instance de la fenêtre MainWindow
            MainWindow mainWindow = new MainWindow();

            // Ouvrir la fenêtre MainWindow
            mainWindow.Show();

            // Optionnel: Fermer la fenêtre courante si nécessaire
            this.Close();
        }

       

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            MainContentFrame.Navigate(new Addemployees());
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            // Assume that ScheduleMeetingPage is the x:Class of your scheduling Page
            MainContentFrame.Navigate(new ScheduleMeetings());

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new Final_Calendar());
        }
    }
    }
