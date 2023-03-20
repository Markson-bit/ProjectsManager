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
using ButtonSpace;

namespace ProjectsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frame.Source = new Uri("/pages/DefaultPage.xaml", UriKind.Relative);
        }

        // Method that allows moving window

        private void Window_mousedown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Exit button that closes program

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Auxiliary method for xaml code

        private void DragOverExit(object sender, DragEventArgs e)
        {
        }

        // Minimize button that is changing state to minimized 

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // --------MENU BUTTONS CODE--------

        private void Infobutton_Click(object sender, RoutedEventArgs e)
        {
            
            frame.Source = new Uri("/pages/InfoPage.xaml", UriKind.Relative);
            ButtonMethods buttonMethods = new ButtonMethods();
        }

        private void Calendarbutton_Click(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("/pages/CalendarPage.xaml", UriKind.Relative);
            ButtonMethods buttonMethods = new ButtonMethods();
        }

        private void Projectsbutton_Click(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("/pages/ProjectsPage.xaml", UriKind.Relative);
            ButtonMethods buttonMethods = new ButtonMethods();
        }    

        private void Aboutbutton_Click(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("/pages/AboutUsPage.xaml", UriKind.Relative);
            ButtonMethods buttonMethods = new ButtonMethods();
        }
    }
}
