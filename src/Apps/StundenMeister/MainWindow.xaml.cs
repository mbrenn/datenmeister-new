using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Runtime;
using StundenMeister.Logic;
using StundenMeister.Model;

namespace StundenMeister
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public IElement CurrentTimeRecoding; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadingText.Visibility = Visibility.Visible;
            LoadedAsset.Visibility = Visibility.Collapsed;

            var settings = new IntegrationSettings
            {
                DatabasePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "StundenMeister")
            };
            
            GiveMe.Scope = await GiveMe.DatenMeisterAsync(settings);

            LoadingText.Visibility = Visibility.Collapsed;
            LoadedAsset.Visibility = Visibility.Visible;
            
            System.Timers.Timer timer = new Timer();
            timer.AutoReset = true;
            
            var timer2 = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
            timer2.Interval = TimeSpan.FromSeconds(1.0);
            timer2.Tick += (x, y) => UpdateContentByTick();
            timer2.Start();


        }

        private void UpdateContentByTick()
        {
            if (CurrentTimeRecoding != null)
            {
                // There is an active recording. We have to update the information to show the user the latest
                // and greatest information. 
                var startDate = CurrentTimeRecoding.getOrDefault<DateTime>(nameof(TimeRecording.startDate));
                DateTime endDate;
                var isActive = CurrentTimeRecoding.getOrDefault<bool>(nameof(TimeRecording.isActive));
                
                if (isActive)
                {
                    // While current time recording is active, advance content
                    endDate = DateTime.UtcNow;
                    CurrentTimeRecoding.set(nameof(TimeRecording.endDate), endDate);
                }
                else
                {
                    // If current time recording is not active, just use the stored time
                    endDate = CurrentTimeRecoding.getOrDefault<DateTime>(nameof(TimeRecording.endDate));
                }

                var timeSpan = endDate - startDate;
                ActiveTime.Text = $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            }
            else
            {
                ActiveTime.Text = "Not Started";
            }
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentTimeRecoding != null)
            {
                CurrentTimeRecoding.set(nameof(TimeRecording.isActive), false);
                CurrentTimeRecoding.set(nameof(TimeRecording.endDate), DateTime.UtcNow);
            }

            var logic = StundenMeisterLogic.Get();

            CurrentTimeRecoding = logic.CreateAndAddNewTimeRecoding();
            CurrentTimeRecoding.set(nameof(TimeRecording.startDate), DateTime.UtcNow);
            CurrentTimeRecoding.set(nameof(TimeRecording.endDate), DateTime.UtcNow);
            CurrentTimeRecoding.set(nameof(TimeRecording.isActive), true);
        }

        private void End_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentTimeRecoding != null)
            {
                CurrentTimeRecoding.set(nameof(TimeRecording.isActive), false);
                CurrentTimeRecoding.set(nameof(TimeRecording.endDate), DateTime.UtcNow);
            }
        }
    }
}
