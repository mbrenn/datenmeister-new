#nullable enable

using BurnSystems.Collections;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Validators;

namespace DatenMeister.Extent.Manager.ExtentStorage.Validators
{
    /// <summary>
    /// Defines the extent validator being used to create new extents
    /// </summary>
    public class NewXmiExtentValidator : IElementValidator
    {
        /// <summary>
        /// Initializes a new instance of the NewXmiExtentValidator class
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic being used</param>
        /// <param name="workspace">The workspace in which the new extent will be created</param>
        public NewXmiExtentValidator(IWorkspaceLogic workspaceLogic, string workspace)
        {
            Workspace = workspace;
            WorkspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets the name of the workspace
        /// </summary>
        private string Workspace { get; }

        /// <summary>
        /// Gets the workspace logic
        /// </summary>
        private IWorkspaceLogic WorkspaceLogic { get; }

        /// <summary>
        /// Validates whether the given form contains the correct information to create a new xmi extent
        /// </summary>
        /// <param name="element">Element to be checked</param>
        /// <returns>The validator result as chained list</returns>
        public ValidatorResult? ValidateElement(IObject element)
        {
            if (WorkspaceLogic == null)
            {
                throw new InvalidOperationException("WorkspaceLogic not set");
            }
            
            var chain = new ChainHelper<ValidatorResult>();

            // Check Uri
            var uri = element.getOrDefault<string>("uri");
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                chain.Add(new ValidatorResult(ValidatorState.Error, "Uri is not a well-formed uri.", "uri"));
            }

            // Check path
            var filepath = element.getOrDefault<string>("filepath");
            if (!Path.IsPathRooted(filepath))
            {
                chain.Add(
                    new ValidatorResult(
                        ValidatorState.Recommendation,
                        "The file path is not an absolute path.",
                        "filepath"));
            }

            var directory = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directory))
            {
                chain.Add(new ValidatorResult(ValidatorState.Error, "The directory does not exist.", "filepath"));
            }

            var workspace = WorkspaceLogic.GetWorkspace(Workspace);
            if (workspace == null)
            {
                chain.Add(new ValidatorResult(ValidatorState.Error, "The workspace does not exist anymore."));
                
            }
            else if (workspace.extent.OfType<IUriExtent>().Any(x=>x.contextURI() == uri))
            {
                chain.Add(
                    new ValidatorResult(
                        ValidatorState.Error,
                        "An extent with given uri is already existing.",
                        "uri"));
            }

            return chain;
        }
    }
}