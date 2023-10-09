using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace EindprojectProgrammeren2
{
    /// <summary>
    /// Interaction logic for succes.xaml
    /// </summary>
    public partial class succes : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        public succes(int intSeconds)
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(10); // Timer is 10 seconds
            timer.Tick += CloseWindow; // Run close window when timer ticks after 10 seconds
            timer.Start(); // Start timer

            tbWin.Text = $"You have won the game in {intSeconds} seconds"; // Change Text to how long it took to win the game
        }

        private void CloseWindow(object? sender, EventArgs e)
        {
            this.Close(); // Close succes window
            timer.Stop(); // Stop timer
        }
    }
}
