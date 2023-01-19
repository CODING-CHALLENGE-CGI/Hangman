using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace hangman
{
    public partial class MainMenuWindow : Window
    {

        string currentTime = string.Empty;
        Stopwatch stopWatch = new Stopwatch();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public bool HangCompleted { get; set; } = false;
        public bool FormCompleted { get; set; } = false;

        public MainMenuWindow()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TBoxHang.IsChecked = HangCompleted;
            TBoxForm.IsChecked = FormCompleted;
            if (!HangCompleted || !FormCompleted)
            {
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                lblTime.Content = currentTime;
            }
            else
            {
                if (stopWatch.IsRunning)
                {
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                        ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    lblTime.Content = currentTime;
                    MessageBox.Show("Félicitation", $"Bravo! Vous avez terminé en {currentTime}");
                }
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Confirmation de choix", "Ne pas ouvrir la fenêtre", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return;
            }
            if (HangCompleted)
            {
                MessageBox.Show("Attention", "Vous avez déjà completé cette page!");
                return;
            }
            MainWindow mainWindow = new MainWindow(this);
            dispatcherTimer.Start();
            stopWatch.Start();
            mainWindow.Show();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            if (HangCompleted || FormCompleted)
            {
                this.WindowState = WindowState.Normal;
                MessageBox.Show("Attention", "Vous ne pouvez pas partir alors que vous avez completé une des deux pages!");
                return;
            }
            MessageBoxResult messageBoxResult = MessageBox.Show("Message de fermeture", "Vous partez déjà ? Dommage ! Au revoir!");
            this.Close();
        }

        private void AboutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Confirmation de choix", "Ne pas ouvrir la fenêtre", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return;
            }
            if (FormCompleted)
            {
                MessageBox.Show("Attention", "Vous avez déjà completé cette page!");
                return;
            }
            AboutWindow abw = new AboutWindow(this);
            dispatcherTimer.Start();
            stopWatch.Start();
            abw.Show();
        }

        private void AboutButtonFullScreenClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
    }
}
