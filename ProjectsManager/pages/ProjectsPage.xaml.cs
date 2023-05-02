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
using System.IO;
using ButtonSpace;
using System.Runtime.Remoting.Contexts;
using System.Data.SQLite;

namespace ProjectsManager.pages
{
    /// <summary>
    /// Interaction logic for ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory + @"..\\..\\\data\testdata.txt";
        string dataPath = AppDomain.CurrentDomain.BaseDirectory + @"..\\..\\\data\tasksdata.db";

        public ProjectsPage()
        {
            InitializeComponent();

            TaskList.Items.Clear();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dataPath};Version=3;"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand("SELECT * FROM Tasks", connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string taskName = reader.GetValue(0).ToString();

                    TaskList.Items.Add(taskName);                 
                }

                connection.Close();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            ButtonMethods buttonMethods = new ButtonMethods();

            if (NameBox.Text.Length > 0)
            {
                // If same task already exist in ListBox
                if (TaskList.Items.Contains(NameBox.Text))
                {
                    // Asking user, if wants to add same task as existing one. 
                    MessageBoxResult result = MessageBox.Show("Exactly same tasks already exist. Are you sure you want to add it again?", "Action needed.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        string taskName = NameBox.Text;
                        string taskPerson = WorkerBox.Text;
                        string taskDesc = DescBox.Text;
                        string taskDate = DateBox.Text;

                        // If yes, adding name to ListBox
                        TaskList.Items.Add(NameBox.Text);

                        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dataPath};Version=3;"))
                        {
                            connection.Open();

                            string sql = "insert into Tasks VALUES(@Name, @Person, @Desc, @Date)";

                            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@Name", taskName);
                                command.Parameters.AddWithValue("@Person", taskPerson);
                                command.Parameters.AddWithValue("@Desc", taskDesc);
                                command.Parameters.AddWithValue("@Date", taskDate);

                                command.ExecuteNonQuery();
                            }

                            connection.Close();
                        }

                        NameBox.Clear();
                        WorkerBox.Clear();
                        DescBox.Clear();
                    }

                    else
                    {
                        // Clearing Input box when user do not want to add same task.
                        NameBox.Clear();
                        WorkerBox.Clear();
                        DescBox.Clear();
                        return;
                    }
                }

                // If same task does not exist in ListBox
                else
                {
                    string taskName = NameBox.Text;
                    string taskPerson = WorkerBox.Text;
                    string taskDesc = DescBox.Text;
                    string taskDate = DateBox.Text;

                    // If yes, adding name to ListBox
                    TaskList.Items.Add(NameBox.Text);

                    using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dataPath};Version=3;"))
                    {
                        connection.Open();

                        string sql = "insert into Tasks VALUES(@Name, @Person, @Desc, @Date)";

                        using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Name", taskName);
                            command.Parameters.AddWithValue("@Person", taskPerson);
                            command.Parameters.AddWithValue("@Desc", taskDesc);
                            command.Parameters.AddWithValue("@Date", taskDate);

                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }

                    NameBox.Clear();
                    WorkerBox.Clear();
                    DescBox.Clear();
                }
            }
        }

        private void DelTask_Click(object sender, RoutedEventArgs e)
        {
            // Getting name of selected task
            string selectedTask = this.TaskList.SelectedItem.ToString();

            // Removing selected task from the database
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dataPath};Version=3;"))
            {
                connection.Open();

                string sql = "DELETE FROM Tasks WHERE Name=@Name";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", selectedTask);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Removing selected task from ListBox
            if (this.TaskList.SelectedIndex != -1)
            {
                this.TaskList.Items.RemoveAt(this.TaskList.SelectedIndex);
            }

            // Clearing data from details section after deleting task
            DetailsName.Text = String.Empty;
            DetailsWorker.Text = String.Empty;
            DetailsDesc.Text = String.Empty;
            DetailsDate.Text = String.Empty;
        }



        // If task inside ListBox is checked, overwriting TextBlocks in Details
        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clearing data from details section, if there are not tasks left (after deleting every task)
            if (TaskList.Items.Count == 0)
            {
                DetailsName.Text = String.Empty;
                DetailsWorker.Text = String.Empty;
                DetailsDesc.Text = String.Empty;
                DetailsDate.Text = String.Empty;
            }

            // Reading from database by select query and splitting data.
            if (TaskList.SelectedIndex != -1)
            {
                string selectedTaskName = TaskList.SelectedItem.ToString();

                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dataPath};Version=3;"))
                {
                    connection.Open();

                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM Tasks WHERE Name=@Name", connection);
                    command.Parameters.AddWithValue("@Name", selectedTaskName);
                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string taskName = reader.GetValue(0).ToString();
                        string taskPerson = reader.GetValue(1).ToString();
                        string taskDesc = reader.GetValue(2).ToString();
                        string taskDate = reader.GetValue(3).ToString();

                        DetailsName.Text = taskName;
                        DetailsWorker.Text = taskPerson;
                        DetailsDesc.Text = taskDesc;
                        DetailsDate.Text = taskDate;
                    }

                    connection.Close();
                }
            }
        }
    }
}

