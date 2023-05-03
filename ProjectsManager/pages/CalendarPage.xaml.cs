using System;
using System.Collections.Generic;
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
using System.Data.SQLite;

namespace ProjectsManager.pages
{
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {

        string dataPath = AppDomain.CurrentDomain.BaseDirectory + @"..\\..\\\data\tasksdatabase.db";

        public CalendarPage()
        {
            InitializeComponent();
        }

        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = CalendarTask.SelectedDate ?? DateTime.MinValue;

            // Format date string to match format used in database
            string formattedDate = selectedDate.ToString("dd.MM.yyyy");

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dataPath};Version=3;"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand("SELECT * FROM Tasks WHERE Date=@Date", connection);
                command.Parameters.AddWithValue("@Date", formattedDate);

                SQLiteDataReader reader = command.ExecuteReader();

                // Check if there is a task assigned to the selected date
                if (reader.HasRows)
                {
                    NavigateButton.Visibility = Visibility.Visible;
                    // Display task name(s) for selected date
                    string taskNames = string.Empty;

                    // Read the first row to get Person, Desc, Date values
                    if (reader.Read())
                    {
                        string taskName = reader.GetValue(0).ToString();
                        string taskWorker = reader.GetValue(1).ToString();
                        string taskDesc = reader.GetValue(2).ToString();

                        DetailsName.Text = taskName;
                        DetailsWorker.Text = taskWorker;
                        DetailsDesc.Text = taskDesc;

                        taskNames = taskName;
                    }

                    // Read remaining rows and add task names to the taskNames string
                    while (reader.Read())
                    {
                        string taskName = reader.GetValue(0).ToString();
                        taskNames += $", {taskName}";
                    }

                    ExistingStatus.Text = "Tasks Existing";
                    DetailsDate.Text = formattedDate;
                    DetailsName.Text = taskNames;
                }
                else
                {
                    NavigateButton.Visibility = Visibility.Hidden;
                    ExistingStatus.Text = "No tasks for this day";
                    DetailsName.Text = String.Empty;
                    DetailsWorker.Text = String.Empty;
                    DetailsDesc.Text = String.Empty;
                    DetailsDate.Text = String.Empty;
                }

                connection.Close();
            }
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProjectsPage());
        }
    }
}
