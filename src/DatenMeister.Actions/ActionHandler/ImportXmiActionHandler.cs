using System;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler
{
    /// <summary>
    /// Performs an import of items into an extent or into a properties extent
    /// </summary>
    public class ImportXmiActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__ImportXmiAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspace = action.getOrDefault<string>(_DatenMeister._Actions._ImportXmiAction.workspace);
            var itemUri = action.getOrDefault<string>(_DatenMeister._Actions._ImportXmiAction.itemUri);
            var xmi = action.getOrDefault<string>(_DatenMeister._Actions._ImportXmiAction.xmi);
            
            var targetObject = actionLogic.WorkspaceLogic.FindItem(workspace, itemUri);

            if (targetObject is IExtent targetExtent)
            {
                var provider = new XmiProvider(XDocument.Parse(xmi));
                var extent = new MofUriExtent(provider, "dm:///import", actionLogic.ScopeStorage);

                // Now do the copying. it makes us all happy
                var extentCopier = new ExtentCopier(new MofFactory(targetExtent));
                extentCopier.Copy(extent.elements(), targetExtent.elements());
            }
            else
            {
                throw new InvalidOperationException("Only importing into extents is currently supported");
            }
        }
    }
}