using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Autofac;
using BurnSystems.Logging;
using BurnSystems.WPF;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.Formatter;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Navigation;
using StundenMeister.Logic;
using StundenMeister.Model;

namespace StundenMeister
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Stores the cached values
        /// </summary>
        private readonly List<TimeRecordingSetUi> _uiRecordingSetCache = new List<TimeRecordingSetUi>();

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

            var timer2 = new DispatcherTimer(DispatcherPriority.Background, Dispatcher)
            {
                Interval = TimeSpan.FromSeconds(0.99)
            };

            timer2.Tick += (x, y) => UpdateContentByTick(false);
            timer2.Start();

            UpdateContentByTick(true);
            UpdateCostCenters();

            new CostCenterLogic(StundenMeisterLogic.Get()).NotifyForCostCenterChange(
                (x, y) => { UpdateCostCenters(); });
        }

        private int _ticksOccured = 0;

        /// <summary>
        /// Just a flag indicating whether the form is currently
        /// in user interaction to avoid multiple queries. 
        /// </summary>
        private bool _inUserInteraction;

        /// <summary>
        /// Updates the ui of the complete window. 
        /// </summary>
        /// <param name="refreshComplete">true, if the window shall be complete regenerated</param>
        private void UpdateContentByTick(bool refreshComplete)
        {
            _ticksOccured++;

            var logic = new TimeRecordingLogic(StundenMeisterLogic.Get());
            logic.UpdateCurrentRecording();

            var timeRecordingSets = logic.GetTimeRecordingSets();
            if (refreshComplete)
            {
                _uiRecordingSetCache.Clear();
                while (gridRecording.Children.Count > 4)
                {
                    gridRecording.Children.RemoveAt(4);
                }

                foreach (var set in timeRecordingSets)
                {
                    AddTimeRecordingLine(set);
                }
            }
            else
            {
                foreach (var set in timeRecordingSets)
                {
                    // Find point
                    var found = _uiRecordingSetCache.FirstOrDefault(
                        x =>
                        {
                            if (x.TimeRecordingSet.CostCenter == null && set.CostCenter == null)
                            {
                                return true;
                            }

                            return x.TimeRecordingSet.CostCenter != null &&
                                   x.TimeRecordingSet.CostCenter.Equals(set.CostCenter);
                        });

                    if (found != null)
                    {
                        found.TextBlockDay.Text = FormatTimeSpan(set.Day);
                        found.TextBlockWeek.Text = FormatTimeSpan(set.Week);
                        found.TextBlockMonth.Text = FormatTimeSpan(set.Month);
                    }
                }
            }

            var costCenter = StundenMeisterLogic.Get().Data.CurrentTimeRecording
                ?.getOrDefault<IElement>(nameof(TimeRecording.costCenter));
            var text = costCenter != null ? new StringFormatter().Format(costCenter, "{{id}} - {{name}}") : "Running";

            Title = logic.IsTimeRecordingActive() ? $"StundenMeister ({text})" : "StundenMeister";

            if (_ticksOccured > 60 * 5)
            {
                _ticksOccured = 0;
                StundenMeisterLogic.Get().StoreExtent();
            }

            // Checks, if hibernation is currently active, if yes, ask the user
            if (StundenMeisterLogic.Get().Data.HibernationDetected && !_inUserInteraction)
            {
                _inUserInteraction = true;
                var result = MessageBox.Show("Hibernation was detected\r\n" +
                                             "Press Yes to continue time-recording.\r\n" +
                                             "Press No to stop time-recording at point of start of hibernation.",
                    "Hibernation detected",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes);

                _inUserInteraction = false;

                if (result == MessageBoxResult.Yes)
                {
                    logic.ConfirmHibernation(true);
                }
                else if (result == MessageBoxResult.No)
                {
                    logic.ConfirmHibernation(false);
                }
            }
        }

        /// <summary>
        /// Formats the timespan and returns the timespan as a string being usable
        /// for times
        /// </summary>
        /// <param name="timeSpan">Time span to be converted</param>
        /// <returns>The converted Timespan</returns>
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{Math.Floor(timeSpan.TotalHours):00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            var logic = new TimeRecordingLogic(
                StundenMeisterLogic.Get());
            logic.StartNewRecording(GetSelectedCostCenter());
            UpdateContentByTick(true);
        }

        private void End_OnClick(object sender, RoutedEventArgs e)
        {
            var logic = new TimeRecordingLogic(
                StundenMeisterLogic.Get());
            logic.EndRecording();
            UpdateContentByTick(false);
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
            GiveMe.Scope?.UnuseDatenMeister();
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

        private void AddTimeRecordingLine(TimeRecordingSet set)
        {
            var row = new RowDefinition();

            var titleBlock = new TextBlock {Text = set.Title, Margin = new Thickness(3)};
            Grid.SetRow(titleBlock, gridRecording.RowDefinitions.Count);
            Grid.SetColumn(titleBlock, 0);

            var dayBlock = new TextBlock {Text = FormatTimeSpan(set.Day), Margin = new Thickness(3)};
            Grid.SetRow(dayBlock, gridRecording.RowDefinitions.Count);
            Grid.SetColumn(dayBlock, 1);

            var weekBlock = new TextBlock {Text = FormatTimeSpan(set.Week), Margin = new Thickness(3)};
            Grid.SetRow(weekBlock, gridRecording.RowDefinitions.Count);
            Grid.SetColumn(weekBlock, 2);

            var monthBlock = new TextBlock {Text = FormatTimeSpan(set.Month), Margin = new Thickness(3)};
            Grid.SetRow(monthBlock, gridRecording.RowDefinitions.Count);
            Grid.SetColumn(monthBlock, 3);

            gridRecording.RowDefinitions.Add(row);
            gridRecording.Children.Add(titleBlock);
            gridRecording.Children.Add(dayBlock);
            gridRecording.Children.Add(weekBlock);
            gridRecording.Children.Add(monthBlock);

            var item = new TimeRecordingSetUi
            {
                TimeRecordingSet = set,
                TextBlockDay = dayBlock,
                TextBlockWeek = weekBlock,
                TextBlockMonth = monthBlock
            };

            _uiRecordingSetCache.Add(item);
        }

        private void StoreNow_Click(object sender, RoutedEventArgs e)
        {
            StundenMeisterLogic.Get().StoreExtent();
        }

        private void UpdateCostCenters()
        {
            var selectedCostCenter = (cboCostCenters.SelectedItem as CostCenterDropDownItem)
                ?.CostCenter;

            var costCenterLogic = new CostCenterLogic(
                StundenMeisterLogic.Get());
            var currentTimeRecording = StundenMeisterLogic.Get().Data.CurrentTimeRecording;
            var currentCostCenter = currentTimeRecording?.getOrDefault<IElement>(nameof(TimeRecording.costCenter));

            var costCenters = costCenterLogic.GetCostCenters();

            CostCenterDropDownItem selectItem = null;
            var list = new List<CostCenterDropDownItem>();
            var formatter = new StringFormatter();
            foreach (var costCenter in costCenters)
            {
                var item = new CostCenterDropDownItem(
                    costCenter,
                    formatter.Format(costCenter, "{{id}} - {{name}}"));
                list.Add(item);

                // Checks, if the user already has selected a cost center or
                // if there is an active cost center due to active time recording
                if (costCenter.@equals(selectedCostCenter)
                    || selectedCostCenter == null && currentCostCenter?.@equals(costCenter) == true)
                {
                    selectItem = item;
                }
            }

            cboCostCenters.ItemsSource = list;
            cboCostCenters.SelectedItem = selectItem;

            if (selectItem == null && list.Count > 0)
            {
                cboCostCenters.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Gets the selected cost center by the drop down
        /// </summary>
        /// <returns>The selected cost center</returns>
        public IElement GetSelectedCostCenter()
        {
            var selectedItem = cboCostCenters.SelectedItem as CostCenterDropDownItem;
            return selectedItem?.CostCenter;
        }

        private void CboCostCenters_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var logic = StundenMeisterLogic.Get();
            var costCenter =
                logic.Data.CurrentTimeRecording
                    ?.getOrDefault<IElement>(nameof(TimeRecording.costCenter));

            var selectedCostCenter = (cboCostCenters.SelectedItem as CostCenterDropDownItem)?.CostCenter;

            if (costCenter != null
                && costCenter == selectedCostCenter)
            {
                return;
            }

            var recordingLogic = new TimeRecordingLogic(logic);
            recordingLogic.StartNewRecording(selectedCostCenter);
            UpdateContentByTick(true);
        }

        private class CostCenterDropDownItem
        {
            public CostCenterDropDownItem(IElement costCenter, string title)
            {
                CostCenter = costCenter;
                Title = title;
            }

            public IElement CostCenter { get; }
            public string Title { get; }

            public override string ToString()
            {
                return Title;
            }
        }

        private class TimeRecordingSetUi
        {
            public TimeRecordingSet TimeRecordingSet { get; set; }

            public TextBlock TextBlockDay { get; set; }

            public TextBlock TextBlockWeek { get; set; }

            public TextBlock TextBlockMonth { get; set; }
        }
    }
}