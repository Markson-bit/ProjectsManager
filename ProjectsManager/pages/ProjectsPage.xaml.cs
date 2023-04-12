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

namespace ProjectsManager.pages
{
    /// <summary>
    /// Interaction logic for ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory + @"..\\..\\\data\testdata.txt";

        public ProjectsPage()
        {
            InitializeComponent();

            TaskList.Items.Clear();

            // Reading tasks from file, splitting by ';' char and appending task name to ListBox.

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    string foundName = parts[0];
                    TaskList.Items.Add(foundName);
                }
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
                        // If yes, adding name to ListBox
                        TaskList.Items.Add(NameBox.Text);

                        string content = NameBox.Text + ";" + WorkerBox.Text + ";" + DescBox.Text + ";" + DateBox.Text + Environment.NewLine;
                        File.AppendAllText(filePath, content);

                        NameBox.Clear();
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
                    TaskList.Items.Add(NameBox.Text);
                    string content = NameBox.Text + ";" + WorkerBox.Text + ";" + DescBox.Text + ";" + DateBox.Text + Environment.NewLine;
                    File.AppendAllText(filePath, content);
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

            // Reading data from file
            string[] lines = File.ReadAllLines(filePath);
            List<string> linesList = lines.ToList();

            // Finding line where name exists
            string lineToRemove = linesList.FirstOrDefault(line => line.StartsWith(selectedTask + ";"));

            // Deleting specific line from file and overwriting it without deleted line
            if (lineToRemove != null)
            {
                linesList.Remove(lineToRemove);
                File.WriteAllLines(filePath, linesList.ToArray());
            }

            // Deleting from ListBox
            if (this.TaskList.SelectedIndex != -1)
            {
                this.TaskList.Items.RemoveAt(this.TaskList.SelectedIndex);
            }
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

            // Reading file and splitting data by ';' char. Name is content behind first ';' char, worker is after first... - Filling details section
            if (TaskList.SelectedIndex != -1)
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    for (int i = 0; i <= TaskList.SelectedIndex; i++)
                    {
                        string line = sr.ReadLine();

                        if (i == TaskList.SelectedIndex)
                        {
                            string[] parts = line.Split(';');
                            string foundName = parts[0];
                            string foundPerson = parts[1];
                            string foundDesc = parts[2];
                            string foundDate = parts[3];

                            DetailsName.Text = foundName;
                            DetailsWorker.Text = foundPerson;
                            DetailsDesc.Text = foundDesc;
                            DetailsDate.Text = foundDate;
                            break;
                        }
                    }
                }


            }
        }
    }
}

