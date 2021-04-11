using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.Reports.Html;
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
            var settings = GiveMe.Scope.ScopeStorage.Get<IntegrationSettings>();
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
            GiveMe.Scope.UnuseDatenMeister();
        }

        private async void ManageCostCenters_Click(object sender, RoutedEventArgs e)
        {
            var metaclass = StundenMeisterPlugin.Get().Data.ClassCostCenter;
            await NavigatorForItems.NavigateToItemsWithAutomaticForm(
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
            var stundenMeisterLogic = StundenMeisterPlugin.Get();

            using var stream = ReportHelper.CreateRandomFile(out var filePath);

            var htmlReportCreator =
                new HtmlReportCreator(stream);
            var htmlReportLogic =
                new ReportLogic(GiveMe.Scope.WorkspaceLogic, GiveMe.Scope.ScopeStorage, htmlReportCreator);
            htmlReportLogic.AddSource(
                "timeRecordings", 
                stundenMeisterLogic.Data.Extent.elements()
                    .WhenMetaClassIs(stundenMeisterLogic.Data.ClassTimeRecording));
            if (hourReport == null)
            {
                MessageBox.Show("Hour Report cannot be found. Internal failure");
                return;
            }

            htmlReportLogic.GenerateReportByDefinition(hourReport);
            
            DotNetHelper.CreateProcess(filePath);
        }

        private async void ManageTimeRecordings_Click(object sender, RoutedEventArgs e)
        {
            var metaclass = StundenMeisterPlugin.Get().Data.ClassTimeRecording;
            var dataWorkspace = GiveMe.Scope.WorkspaceLogic.GetDataWorkspace();
            var formTimeRecordings = dataWorkspace.ResolveById("formListTimeRecordings")
                ?? throw new InvalidOperationException("formListTimeRecordings not found");
            
            await NavigatorForItems.NavigateToItems(
                StundenMeisterPlugin.Get().Data
                    .Extent
                    .elements()
                    .WhenMetaClassIs(metaclass),
                formTimeRecordings);
        }

        private void StoreNow_Click(object sender, RoutedEventArgs e)
        {
            StundenMeisterPlugin.Get().StoreExtent();
        }
    }
}