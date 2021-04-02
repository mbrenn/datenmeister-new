using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Runtime;
using DatenMeister.WPF.Controls;

namespace DatenMeister.WPF.Forms.Fields
{
    public partial class ReferenceField
    {
        /// <summary>
        /// Creates an inline field in which the user can directly click
        /// workspace, extent and item within a selection form
        /// </summary>
        /// <param name="fieldData">Field Data describing the field</param>
        /// <param name="fieldFlags">The flags</param>
        /// <returns>The created element allowing the user to navigate</returns>
        private UIElement CreateInlineField(IObject fieldData, FieldParameter fieldFlags)
        {
            // Defines the locate element control in which the user can select
            // workspace, extent and element
            _control = new LocateElementControl
            {
                MinHeight = 400,
                MaxHeight = 400,
                MinWidth = 600
            };

            var filterMetaClasses =
                fieldData.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ReferenceFieldData.metaClassFilter);
            if (filterMetaClasses != null)
            {
                _control.FilterMetaClasses = filterMetaClasses.OfType<IElement>();
            }

            var showAllChildren =
                fieldData.getOrDefault<bool>(_DatenMeister._Forms._ReferenceFieldData.showAllChildren);
            if (showAllChildren) _control.ShowAllChildren = true;

            var element = fieldData.getOrDefault<IElement>(_DatenMeister._Forms._ReferenceFieldData.defaultValue);
            if (element != null)
            {
                _control.Select(element);
            }
            else
            {
                if (!string.IsNullOrEmpty(_workspace)
                    && !string.IsNullOrEmpty(_extent)
                    && _workspace != null && _extent != null)
                {
                    var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                    var (foundWorkspace, foundExtent) =
                        workspaceLogic.RetrieveWorkspaceAndExtent(
                            _workspace,
                            _extent);

                    _control.Select(foundWorkspace, foundExtent);
                }
                else if (!string.IsNullOrEmpty(_workspace) && _workspace != null)
                {
                    var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                    var foundWorkspace = workspaceLogic.GetWorkspace(_workspace);
                    if (foundWorkspace != null)
                    {
                        _control.Select((IWorkspace) foundWorkspace);
                    }
                }
            }

            fieldFlags.CanBeFocused = true;
            return _control;
        }
    }
}