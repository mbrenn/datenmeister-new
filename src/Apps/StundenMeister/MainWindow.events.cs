using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Navigation;
using StundenMeister.Logic;

namespace StundenMeister
{
    public partial class MainWindow
    {
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

        private void CboCostCenters_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCostCenter = GetSelectedCostCenter();
            var logic = StundenMeisterLogic.Get();
            var recordingLogic = new TimeRecordingLogic(logic);
            
            recordingLogic.ChangeCostCenter(selectedCostCenter);
            UpdateContentByTick(true);
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenStorageFolder_Click(object sender, RoutedEventArgs e)
        {
            var settings = GiveMe.Scope.Resolve<IntegrationSettings>();
            DotNetHelper.CreateProcess(settings.DatabasePath);
        }

        private void HamburgerMenuItem_Click(object sender, RoutedEventArgs e)
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

        private void StoreNow_Click(object sender, RoutedEventArgs e)
        {
            StundenMeisterLogic.Get().StoreExtent();
        }
    }
}