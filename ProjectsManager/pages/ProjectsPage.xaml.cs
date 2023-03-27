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

namespace ProjectsManager.pages
{
    /// <summary>
    /// Interaction logic for ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory + @"..\\..\\\data\testdata.txt";
        //string filePath = AppDomain.CurrentDomain.BaseDirectory + @"/bin/ProjectsManager\data\testdata.txt";
        public ProjectsPage()
        {
            InitializeComponent();
            // Reading tasks from file and appending them to List box.
            TaskList.Items.Clear();
            string[] content = File.ReadAllLines(filePath);
            foreach (string inside in content)
            {
                TaskList.Items.Add(inside);
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
                        string content = NameBox.Text + Environment.NewLine;
                        File.AppendAllText(filePath, content);
                        NameBox.Clear();
                    }

                    else
                    {
                        // Clearing Input box when user do not want to add same task.
                        NameBox.Clear();
                        return;
                    }
                }

                // If same task does not exist in ListBox
                else
                {
                    TaskList.Items.Add(NameBox.Text);
                    string content = NameBox.Text + Environment.NewLine;
                    File.AppendAllText(filePath, content);
                    NameBox.Clear();
                }

            }
        }

        private void DelTask_Click(object sender, RoutedEventArgs e)
        {
            // Making an array from lines of file.
            string[] inventoryData = File.ReadAllLines(filePath);
            // Making a list from components of array.
            List<string> inventoryDataList = inventoryData.ToList();

            // Removing from the file specific task that is converted to string.
            if (inventoryDataList.Remove(Convert.ToString(this.TaskList.SelectedItem)))
            {
                File.WriteAllLines(filePath, inventoryDataList.ToArray());
            }

            // Removing selected task from List box.
            if (this.TaskList.SelectedIndex != -1)
            {
                this.TaskList.Items.RemoveAt(this.TaskList.SelectedIndex);
            }
        }

        // If task inside ListBox is checked, overwriting TextBlocks in Details
        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList.Items.Count == 0)
            {
                DetailsName.Text = String.Empty;
                DetailsWorker.Text = String.Empty;
                DetailsDesc.Text = String.Empty;
                DetailsDate.Text = String.Empty;
            }
            if (TaskList.SelectedIndex != -1)
            {
                DetailsName.Text = TaskList.SelectedItem.ToString();
                DetailsWorker.Text = "Example Work no: " + (TaskList.SelectedIndex+1).ToString();
                DetailsDesc.Text = "Example Desc no: " + (TaskList.SelectedIndex + 1).ToString();
                DetailsDate.Text = "Example Date no: " + (TaskList.SelectedIndex + 1).ToString();
            }
        }
    }
}

