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
using System.Windows.Threading;
using System.Data.SQLite;

namespace ProjectsManager.pages
{
    /// <summary>
    /// Interaction logic for InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {

        string dataPath = AppDomain.CurrentDomain.BaseDirectory + @"..\\..\\\data\tasksdatabase.db";

        public InfoPage()
        {
            InitializeComponent();
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += Timer_Tick;
            timer.Start();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dataPath};Version=3;"))
            {
                connection.Open();

                // Get the count of all tasks
                SQLiteCommand countCommand = new SQLiteCommand("SELECT COUNT(*) FROM Tasks", connection);
                int count = Convert.ToInt32(countCommand.ExecuteScalar());

                // Get the name of the closest task
                DateTime currentDate = DateTime.Now.Date;
                SQLiteCommand closestCommand = new SQLiteCommand("SELECT Name FROM Tasks WHERE Date >= @Date ORDER BY Date ASC LIMIT 1", connection);
                closestCommand.Parameters.AddWithValue("@Date", currentDate.ToString("dd.MM.yyyy"));
                string closestTaskName = closestCommand.ExecuteScalar()?.ToString();

                Amount.Text = count.ToString();
                Closest.Text = closestTaskName ?? "No upcoming tasks";

                connection.Close();
            }



        }

        // Timer for refreshing actual time
        private void Timer_Tick(object sender, EventArgs e)
        {
            StaticDate.Text = "Today is: " + DateTime.Now.ToString("g");
        }
    }
}
