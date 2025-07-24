using System.Diagnostics;
using System.IO;
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

namespace Airport_FlightTracker2
{
    public partial class MainWindow : Window
    {
        private readonly string filePath;
        public MainWindow()
        {
            InitializeComponent();

            /* sets the path to create the text file in the application domain on load of the app */
            filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Flight_Details.txt");
            
        }
        private void btnBoarded_Click(object sender, RoutedEventArgs e) //Synchronous method writes to file 
        {
            string name = txtPassengerName.Text.Trim();
            string seat = txtSeatNumber.Text.Trim();
        }

        //Async Method Demonstrates how the UI thread is still accessible even when a process is pending
        private async void btnBoardedAsync_Click(object sender, RoutedEventArgs e)
        {
            string name = txtPassengerName.Text.Trim();
            string seat = txtSeatNumber.Text.Trim();

            await WriteToFileAsync(name, seat, DateTime.Now, "Booked");
        }

        /* Event handlers have to use 'void' return type when using async (.NET rule) 
           Any other method using async has to have a return type of 'Task' to allow awaiting the completion of a task 
           And to also do proper exception handling */
        private async void btnCanceled_Click(object sender, RoutedEventArgs e)
        {
            string name = txtPassengerName.Text.Trim();
            string seat = txtSeatNumber.Text.Trim();

            // Awaiting the async Task method to handle cancellation logging properly.
            await WriteToFileAsync(name, seat, "Cancelled");
        }

        /* Use async Task for methods that you need to wait for response or handle exceptions properly */
        private async Task WriteToFileAsync(string name, string seat, DateTime time, string status)
        {
            string line = $"{name}, {seat}, {time}, {status}";

            labelLoggingPending.Text = "Logging boarded passenger data to the text file";
            await Task.Delay(5000);
            await File.AppendAllTextAsync(filePath, line + Environment.NewLine);
            labelLoggingPending.Text = "Logging Complete!";
        }

        //Overloaded method for cancellation, without timestamp
        private async Task WriteToFileAsync(string name, string seat, string status)
        {
            string line = $"{name},{seat},{status}";
            labelLoggingPending.Text = "Logging Cancelled passenger data to the text file";
            await Task.Delay(5000);
            await File.AppendAllTextAsync(filePath, line + Environment.NewLine);
            labelLoggingPending.Text = "Logging Complete!";

        }
        //opening external process (notepad, with a specific filepath to it) 
        private void btnViewFlight_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo("notepad.exe", filePath) 
                { UseShellExecute = false });
                Process.Start("notepad.exe");
            }
            else
            {
                MessageBox.Show("Flight details file not found.");
            }
        }

        //This method opens a website link 
        private void btnOpenWebsite_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(
            "https://www.emirates.com/za/english/before-you-fly/visa-passport-information/")
            { UseShellExecute = true });
        }
    }
}