using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EindprojectProgrammeren2
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

        // Speeds for avatar movement
        int speedSuperSlow = 750;
        int speedSlow = 600;
        int speedNormal = 500;
        int speedFast = 400;
        int speedSuperFast = 300;

        int currentSpeed = 500;

        // Timers
        DispatcherTimer _moveTimer = new DispatcherTimer(); // Timer to move avatar
        DispatcherTimer _timeTimer = new DispatcherTimer(); // Timer to count time
        DispatcherTimer _countdownTimer = new DispatcherTimer(); // Countdown timer for party mode

        // Randoms for margins of avatar image
        Random _randomLeft = new Random(); // Left margin
        Random _randomTop = new Random(); // Top margin

        int counterSeconds = 0; // Int for timer

        // Functionality to move the avatar image
        private void MoveOnTick(object? sender, EventArgs e)
        {
            try
            {
                int currentHeight = Convert.ToInt32(gridRunner.ActualHeight); // Get height of yellow grid 
                int currentWidth = Convert.ToInt32(gridRunner.ActualWidth); // Get width of yellow grid

                int newLeft = _randomLeft.Next(1, currentWidth-40); // Get a random left margin based on the width of the yellow grid
                int newTop = _randomTop.Next(1, currentHeight-49); // Get a random top margin based on the height of the yellow grid

                imgAvatar.Margin = new Thickness(newLeft, newTop, 0, 0); // Change margins of image avatar to random margins
            }
            catch
            {
                btnStartStop.Content = "Start!"; // Change text on buton to Start!
                btnStartStop.Background = Brushes.Green; // Change button background color to green
                _moveTimer.Stop(); // Stop timer and stop moving

                imgAvatar.Margin = new Thickness(0, 0, 0, 0); // Set imgAvatar back to left top corner

                _timeTimer.Stop(); // Stop timer
                counterSeconds = 0; // Set seconds back to 0
                tbTimeCounter.Text = counterSeconds.ToString(); // Reset counter to 0

                MessageBox.Show("Grid too small", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartMoving()
        {
            _moveTimer.Interval = TimeSpan.FromMilliseconds(currentSpeed); // Timespan is set to current speed
            _moveTimer.Tick -= MoveOnTick; // Move avatar
            _moveTimer.Tick += MoveOnTick; // Move avatar
            _moveTimer.Start(); // Start Timer
        }

        // Timer for duration of game
        private void TimeTimer(object? sender, EventArgs e)
        {
            counterSeconds++; // Add 1 to seconds
            if (partyModeActivated == false)
            {
                tbTimeCounter.Text = counterSeconds.ToString(); // Change text to new seconds count
            }
        }

        private void StartTimeTimer()
        {
            _timeTimer.Interval = TimeSpan.FromSeconds(1); // Timespan is 1 second
            _timeTimer.Tick -= TimeTimer; // Add 1 to time (seconds)
            _timeTimer.Tick += TimeTimer; // Add 1 to time (seconds)
            _timeTimer.Start(); // Start Timer
        }
        
        // Party mode
        int countdown = 30; // Set countdow to 30 seconds
        bool partyModeActivated = false; // Decalre party mode boolean

        private void PartyTick(object sender, EventArgs e)
        {
            if (countdown > 0)
            {
                tbTimeCounter.Text = countdown.ToString(); // Change text to countdown number
                countdown--; // Remove 1 from countdown
            }
            else
            {
                tbTimeCounter.Text = countdown.ToString(); // Change text to countdown number
                BtnStartStop_Click(this, new RoutedEventArgs()); // Stop the game
                MessageBox.Show("You failed the party mode");
                partyModeActivated = false; // Deactivate party mode
                countdown = 30; // Reset the countdown to 30
            }
        }

        private void PartyCountdown()
        {
            _countdownTimer.Interval = TimeSpan.FromSeconds(1); // 
            _countdownTimer.Tick -= PartyTick; // Move avatar
            _countdownTimer.Tick += PartyTick; // Move avatar
            _countdownTimer.Start(); // Start Timer
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            string textOnButton = btnStartStop.Content.ToString(); // Get text on the start and stop button

            if (partyModeActivated == false)
            {
                if (textOnButton == "Start!") // Start
                {
                    btnStartStop.Content = "Stop!"; // Change text on button to Stop!
                    btnStartStop.Background = Brushes.Red; // Change button background color to red
                    StartMoving(); // Start moving
                    StartTimeTimer(); // Start Timer
                }
                else if (textOnButton == "Stop!") // Stop
                {
                    btnStartStop.Content = "Start!"; // Change text on buton to Start!
                    btnStartStop.Background = Brushes.Green; // Change button background color to green
                    
                    _moveTimer.Stop(); // Stop timer and stop moving
                    imgAvatar.Margin = new Thickness(0, 0, 0, 0); // Set imgAvatar back to left top corner

                    _timeTimer.Stop(); // Stop timer
                    counterSeconds = 0; // Set seconds back to 0
                    tbTimeCounter.Text = counterSeconds.ToString(); // Reset counter to 0
                }
            }
            else
            {
                if (textOnButton == "Start!") // Start
                {
                    btnStartStop.Content = "Stop!"; // Change text on button to Stop!
                    btnStartStop.Background = Brushes.Red; // Change button background color to red
                    StartMoving(); // Start moving
                    PartyCountdown(); // Start party countdown timer
                    StartTimeTimer(); // Start timer
                    btnSlower.IsEnabled = false; // Disable slower button
                }
                else if (textOnButton == "Stop!") // Stop
                {
                    btnStartStop.Content = "Start!"; // Change text on buton to Start!
                    btnStartStop.Background = Brushes.Green; // Change button background color to green
                   
                    _moveTimer.Stop(); // Stop timer and stop moving
                    imgAvatar.Margin = new Thickness(0, 0, 0, 0); // Set imgAvatar back to left top corner

                    _timeTimer.Stop(); // Stop timer
                    counterSeconds = 0; // Set seconds back to 0

                    _countdownTimer.Stop(); // Stop countdown timer

                    tbTimeCounter.Text = "0"; // Change text to countdown number
                    partyModeActivated = false; // Deactivate party mode
                    countdown = 30; // Reset the countdown to 30
                    btnSlower.IsEnabled = true; // Enable slower button
                    MessageBox.Show("You failed the party mode");
                }
            }
        }

        private void BtnFaster_Click(object sender, RoutedEventArgs e)
        {
            switch (currentSpeed) 
            {
                case 750: // speedSuperSlow
                    currentSpeed = 600;
                    break;
                case 600: // speedSlow
                    currentSpeed = 500;
                    break;
                case 500: // speedNormal
                    currentSpeed = 400;
                    break;
                case 400: // speedFast
                    currentSpeed = 300;
                    break;
                case 300: // speedSuperFast
                    MessageBox.Show("It's not possible to go faster!! This is fast enough!", "Error: Too Fast", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default: 
                    break;
            }
        }

        private void BtnSlower_Click(object sender, RoutedEventArgs e)
        {
            switch (currentSpeed)
            {
                case 300: // speedSuperFast
                    currentSpeed = 400;
                    break;
                case 400: // speedFast
                    currentSpeed = 500;
                    break;
                case 500: // speedNormal
                    currentSpeed = 600;
                    break;
                case 600: // speedSlow
                    currentSpeed = 750;
                    break;
                case 750: // speedSuperSlow
                    MessageBox.Show("It's not possible to go any slower!", "Error: As Slow As Possible", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }

        // DispatcherTimer timer = new DispatcherTimer();

        private void ImgRoadrunner_MouseEnter(object sender, MouseEventArgs e)
        {
            string textOnButton = btnStartStop.Content.ToString(); // Get text on the start and stop button
            if (partyModeActivated ==  false)
            {
                if (textOnButton == "Stop!")
                {
                    btnStartStop.Content = "Start!"; // Change text on buton to Start!
                    btnStartStop.Background = Brushes.Green; // Change button background color to green

                    _moveTimer.Stop(); // Stop timer

                    imgAvatar.Margin = new Thickness(0, 0, 0, 0); // Set imgAvatar back to left top corner

                    var succesWindow = new succes(counterSeconds); // Create new window
                    succesWindow.Show(); // Open new window

                    _timeTimer.Stop(); // Stop timer
                    counterSeconds = 0; // Set seconds back to 0
                    tbTimeCounter.Text = counterSeconds.ToString(); // Reset counter to 0
                }
            }
            else
            {
                if (textOnButton == "Stop!")
                {
                    btnStartStop.Content = "Start!"; // Change text on buton to Start!
                    btnStartStop.Background = Brushes.Green; // Change button background color to green

                    _moveTimer.Stop(); // Stop timer

                    imgAvatar.Margin = new Thickness(0, 0, 0, 0); // Set imgAvatar back to left top corner

                    var succesWindow = new succes(counterSeconds); // Create new window
                    succesWindow.Show(); // Open new window

                    partyModeActivated = false; // Deactivate party mode
                    _countdownTimer.Stop();

                    _timeTimer.Stop(); // Stop timer
                    counterSeconds = 0; // Set seconds back to 0
                    tbTimeCounter.Text = counterSeconds.ToString(); // Reset counter to 0
                    btnSlower.IsEnabled = true; // Enable the slow button
                }
            }
        }

        private void BtnParty_Click(object sender, RoutedEventArgs e)
        {
            // Set window to full screen
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Margin = new Thickness(0, 0, 0, 0);
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = 0;

            partyModeActivated = true; // Activate party mode

            BtnStartStop_Click(this, new RoutedEventArgs()); // Start the game
        }

        private void CmbChooseImg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedImg = cmbChooseImg.SelectedValue.ToString(); // Get selected img name

            switch (selectedImg)
            {
                case "Roadrunner":
                    // Change image to Roadrunner
                    imgAvatar.Source = new BitmapImage(new Uri("/assets/roadrunner.jpg", UriKind.RelativeOrAbsolute));
                    break;
                case "The Rock":
                    // Change image to The Rock
                    imgAvatar.Source = new BitmapImage(new Uri("/assets/Rock.jpg", UriKind.RelativeOrAbsolute));
                    break;
                case "Speedy":
                    // Change image to Speedy
                    imgAvatar.Source = new BitmapImage(new Uri("/assets/Speedy.jpg", UriKind.RelativeOrAbsolute));
                    break;
                case "NSTrein":
                    // Change image to NS Trein
                    imgAvatar.Source = new BitmapImage(new Uri("/assets/nstrein.jpg", UriKind.RelativeOrAbsolute));
                    break;
                default:
                    // Default change image to Roadrunner   
                    imgAvatar.Source = new BitmapImage(new Uri("/assets/roadrunner.jpg", UriKind.RelativeOrAbsolute));
                    break;
            }
        }
    }
}
