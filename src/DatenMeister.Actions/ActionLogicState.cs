using System.Collections.Generic;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Actions.ActionHandler.Reports;
using DatenMeister.Actions.Transformations;

namespace DatenMeister.Actions
{
    /// <summary>
    /// Stores the data of the action logic within the instance
    /// </summary>
    public class ActionLogicState
    {
        /// <summary>
        /// Gets the action handlers
        /// </summary>
        public List<IActionHandler> ActionHandlers { get; }
            = new();

        /// <summary>
        /// Adds one action handler
        /// </summary>
        /// <param name="actionHandler">ActionHandler to be added</param>
        public void AddActionHandler(IActionHandler actionHandler)
        {
            lock (ActionHandlers)
            {
                ActionHandlers.Add(actionHandler);
            }
        }

        public static ActionLogicState GetDefaultLogicState()
        {
            var logicState = new ActionLogicState();
            logicState.AddActionHandler(new LoggingWriterActionHandler());
            logicState.AddActionHandler(new CommandExecutionActionHandler());
            logicState.AddActionHandler(new CreateWorkspaceActionHandler());
            logicState.AddActionHandler(new DropExtentActionHandler());
            logicState.AddActionHandler(new StoreExtentActionHandler());
            logicState.AddActionHandler(new DropWorkspaceActionHandler());
            logicState.AddActionHandler(new LoadExtentActionHandler());
            logicState.AddActionHandler(new CopyElementsActionHandler());
            logicState.AddActionHandler(new ExportToXmiActionHandler());
            logicState.AddActionHandler(new ClearCollectionActionHandler());
            logicState.AddActionHandler(new ActionSetActionHandler());
            logicState.AddActionHandler(new ItemTransformationActionHandler());
            logicState.AddActionHandler(new SimpleReportActionHandler());
            logicState.AddActionHandler(new AdocReportActionHandler());
            logicState.AddActionHandler(new HtmlReportActionHandler());
            logicState.AddActionHandler(new OpenDocumentActionHandler());
            logicState.AddActionHandler(new EchoActionHandler());
            logicState.AddActionHandler(new CreateFormByMetaclassActionHandler());
            logicState.AddActionHandler(new MoveOrCopyActionHandler());
            logicState.AddActionHandler(new MoveUpDownActionHandler());
            logicState.AddActionHandler(new ImportXmiActionHandler());
            
            return logicState;
        }
    }
}