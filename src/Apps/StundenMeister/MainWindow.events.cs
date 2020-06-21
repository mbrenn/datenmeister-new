using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Navigation;
using StundenMeister.Logic;

namespace StundenMeister
{
    public partial class MainWindow
    {
        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            var logic = new TimeRecordingLogic(
                StundenMeisterPlugin.Get());
            logic.StartNewRecording(GetSelectedCostCenter());
            UpdateContentByTick(true);
        }

        private void End_OnClick(object sender, RoutedEventArgs e)
        {
            var logic = new TimeRecordingLogic(
                StundenMeisterPlugin.Get());
            logic.EndRecording();
            UpdateContentByTick(false);
        }

        private void CboCostCenters_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCostCenter = GetSelectedCostCenter();
            var logic = StundenMeisterPlugin.Get();
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
            var metaclass = StundenMeisterPlugin.Get().Data.ClassCostCenter;
            NavigatorForItems.NavigateToItems(
                StundenMeisterPlugin.Get().Data
                    .Extent
                    .elements()
                    .WhenMetaClassIs(metaclass),
                metaclass);
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            var dataWorkspace = GiveMe.Scope.WorkspaceLogic.GetDataWorkspace();
            var hourReport = dataWorkspace.ResolveById("hourReport");
            
            MessageBox.Show(hourReport?.ToString() ?? "Null");
        }

        private void ManageTimeRecordings_Click(object sender, RoutedEventArgs e)
        {
            var metaclass = StundenMeisterPlugin.Get().Data.ClassTimeRecording;
            NavigatorForItems.NavigateToItems(
                StundenMeisterPlugin.Get().Data
                    .Extent
                    .elements()
                    .WhenMetaClassIs(metaclass),
                metaclass);
        }

        private void StoreNow_Click(object sender, RoutedEventArgs e)
        {
            StundenMeisterPlugin.Get().StoreExtent();
        }
    }
}