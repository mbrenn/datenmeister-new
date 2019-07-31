using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Autofac;
using BurnSystems.Logging;
using BurnSystems.WPF;
using DatenMeister.Integration;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using StundenMeister.Logic;

namespace StundenMeister
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

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TheLog.AddProvider(new TextBlockLogProvider(TxtLogging), LogLevel.Trace);
            
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
            
            Timer timer = new Timer();
            timer.AutoReset = true;
            
            var timer2 = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
            timer2.Interval = TimeSpan.FromSeconds(1.0);
            timer2.Tick += (x, y) => UpdateContentByTick();
            timer2.Start();
            
            UpdateContentByTick();
        }

        private void UpdateContentByTick()
        {
            var logic = new TimeRecordingLogic(StundenMeisterLogic.Get());
            logic.UpdateCurrentRecording();
            
            var timeSpanDay = logic.CalculateWorkingHoursInDay();
            var timeSpanWeek = logic.CalculateWorkingHoursInWeek();
            var timeSpanMonth = logic.CalculateWorkingHoursInMonth();
            ActiveTimeDay.Text =
                $"{timeSpanDay.Hours:00}:{timeSpanDay.Minutes:00}:{timeSpanDay.Seconds:00}";
            ActiveTimeWeek.Text = $"{timeSpanWeek.Hours:00}:{timeSpanWeek.Minutes:00}:{timeSpanWeek.Seconds:00}";
            ActiveTimeMonth.Text = $"{timeSpanMonth.Hours:00}:{timeSpanMonth.Minutes:00}:{timeSpanMonth.Seconds:00}";

            Title = logic.IsTimeRecordingActive() ? "StundenMeister (active)" : "StundenMeister";
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            var logic = new TimeRecordingLogic(
                StundenMeisterLogic.Get());
            logic.StartNewRecording();
            UpdateContentByTick();
        }

        private void End_OnClick(object sender, RoutedEventArgs e)
        {
            var logic = new TimeRecordingLogic(
                StundenMeisterLogic.Get());
            logic.EndRecording();
            UpdateContentByTick();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenStorageFolder_Click(object sender, RoutedEventArgs e)
        {
            var settings = GiveMe.Scope.Resolve<IntegrationSettings>();
            Process.Start(settings.DatabasePath);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button senderButton))
            {
                throw new InvalidOperationException("sender is not a button");
            }

            if (senderButton.ContextMenu == null)
            {
                // Nothing to see here
                return;
            }

            senderButton.ContextMenu.IsOpen = true;
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            GiveMe.Scope.UnuseDatenMeister();
        }

        private void ManageCostCenters_Click(object sender, RoutedEventArgs e)
        {
            var metaclass = StundenMeisterLogic.Get().Data.ClassCostCenter;
            NavigatorForItems.NavigateToItems(
                StundenMeisterLogic.Get().Data
                    .Extent
                    .elements()
                    .WhenMetaClassIs(metaclass),
                metaclass);
        }

        private void ManageTimeRecordings_Click(object sender, RoutedEventArgs e)
        {
            var metaclass = StundenMeisterLogic.Get().Data.ClassTimeRecording;
            NavigatorForItems.NavigateToItems(
                StundenMeisterLogic.Get().Data
                    .Extent
                    .elements()
                    .WhenMetaClassIs(metaclass),
                metaclass);
        }
    }
}
