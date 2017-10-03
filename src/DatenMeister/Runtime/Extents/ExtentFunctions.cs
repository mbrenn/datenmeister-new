using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.Extents
{
    public class ExtentFunctions
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Initializes a new instance of the ExtentFunctions class.
        /// </summary>
        /// <param name="workspaceLogic"></param>
        public ExtentFunctions(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets an enumeration of creatable types for a given extent. 
        /// It navigates to the meta extent and looks for all classes
        /// </summary>
        /// <param name="extent">The extent into which a new instance shall be created</param>
        /// <returns>Enumeration of types</returns>
        public CreateableTypeResult GetCreatableTypes(IUriExtent extent)
        {
            var dataLayer = _workspaceLogic.GetWorkspaceOfExtent(extent);
            var typeLayer = dataLayer.MetaWorkspaces.FirstOrDefault();
            var umlLayer= typeLayer.MetaWorkspaces.FirstOrDefault();

            var uml = umlLayer.Get<_UML>();
            var classType = uml?.StructuredClassifiers.__Class;

            if (classType == null)
            {
                // We did not find the corresponding class type
                return new CreateableTypeResult
                {

                    MetaLayer = typeLayer,
                    CreatableTypes = new IElement[] {}
                };
            }

            return new CreateableTypeResult
            {
                MetaLayer = typeLayer,
                CreatableTypes = _workspaceLogic.GetExtentsForWorkspace(typeLayer)
                    .SelectMany(x => x.elements().GetAllDescendants().WhenMetaClassIs(classType))
                    .Cast<IElement>()
                    .ToList()
            };
        }

        public class CreateableTypeResult
        {
            public Workspace MetaLayer { get; set; }
            public IList<IElement> CreatableTypes { get; set; }
        }
    }
}