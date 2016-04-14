using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;
using DatenMeister.Filler;

namespace DatenMeister.XMI.UmlBootstrap
{
    /// <summary>
    ///     Performs the bootstrap on the given object
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        ///     Stores a vlaue indicating whether the run was already performed.
        ///     If yes, the method is locked
        /// </summary>
        private bool _wasRun;

        public Dictionary<string, IElement> MofClasses { get; } = new Dictionary<string, IElement>();

        public Dictionary<string, IElement> UmlClasses { get; } = new Dictionary<string, IElement>();
        public Dictionary<string, IElement> UmlAssociations { get; } = new Dictionary<string, IElement>();

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent MofInfrastructure { get; }

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent UmlInfrastructure { get; }

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent PrimitiveInfrastructure { get; }

        /// <summary>
        /// Initializes the bootstrapper after having the the necessary extents
        /// </summary>
        /// <param name="primitiveInfrastructure">Extent reflecting the primitive structure XMI definition</param>
        /// <param name="umlInfrastructure">Extent reflecting the Uml infrastructure XMI definition</param>
        /// <param name="mofInfrastructure">Extent reflecting the MOF infrastructure XMI definition</param>
        public Bootstrapper(
            IUriExtent primitiveInfrastructure, 
            IUriExtent umlInfrastructure,
            IUriExtent mofInfrastructure)
        {
            if (umlInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(umlInfrastructure));
            }
            if (mofInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(mofInfrastructure));
            }
            if (primitiveInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(primitiveInfrastructure));
            }

            UmlInfrastructure = umlInfrastructure;
            MofInfrastructure = mofInfrastructure;
            PrimitiveInfrastructure = primitiveInfrastructure;
        }

        /// <summary>
        ///     Performs the bootstrap
        /// </summary>
        public void Strap()
        {
            if (_wasRun)
            {
                throw new InvalidOperationException("Bootstrapper was already run. Create a new object for your run");
            }

            _wasRun = true;
            
            var umlDescendents = AllDescendentsQuery.getDescendents(UmlInfrastructure).ToList();
            var mofDescendents = AllDescendentsQuery.getDescendents(MofInfrastructure).ToList();
            var primitiveDescendents = AllDescendentsQuery.getDescendents(PrimitiveInfrastructure).ToList();
            var allElements =
                mofDescendents
                    .Union(umlDescendents)
                    .Union(primitiveDescendents)
                    .ToList();

            // First, find the all classes of the uml namespace...
            var typeProperty = (Namespaces.Xmi + "type").ToString();
            var idProperty = (Namespaces.Xmi + "id").ToString();

            var idToElementCache = new Dictionary<string, IElement>();

            // Go through all elements and set the id
            foreach (var element in allElements.OfType<IElement>())
            {
                if (element.isSet(typeProperty) && element.get(typeProperty).ToString() == "uml:PackageImport")
                {
                    continue;
                }

                if (element.isSet(idProperty))
                {
                    var id = element.get(idProperty).ToString();
                    if (id.StartsWith("_"))
                    {
                        // Due to a problem in the uml.xmi, duplicate IDs might be in the packageImport
                        // We also skip this... uml.xmi and primitivetypes.xmi have this id. Not used
                        // All duplicate items start with an underscore
                        continue;
                    }

                    if (idToElementCache.ContainsKey(id))
                    {
                        throw new InvalidOperationException($"ID '{id}' is duplicate");
                    }

                    idToElementCache[id] = element;
                }

                element.unset(idProperty);
            }

            // Go through all found classes and store them into the dictionaries
            foreach (var classInstance in umlDescendents.OfType<IElement>().Where(x => x.isSet("name")))
            {
                var name = classInstance.get("name").ToString();
                var typeValue = classInstance.isSet(typeProperty) ? classInstance.get(typeProperty).ToString() : null;
                if ( typeValue == "uml:Class")
                { 
                    UmlClasses[name] = classInstance;
                }
                if (typeValue == "uml:Association")
                {
                    UmlAssociations[name] = classInstance;
                }
            }

            // Second step: Find all classes in the Mof namespace
            var allClasses = mofDescendents
                .Where(x => x.isSet(typeProperty) && x.get(typeProperty).ToString() == "uml:Class");
            foreach (var classInstance in allClasses.Cast<IElement>())
            {
                var name = classInstance.get("name").ToString();
                MofClasses[name] = classInstance;
            }

            // After having the classes from MOF and UML, go through all classes and set
            // the metaclass of these element depending on the attribute value of Xmi:Type
            foreach (var elementInstance in allElements.Where(x => x.isSet(typeProperty)))
            {
                var name = elementInstance.get(typeProperty).ToString();
                if (name.StartsWith("uml:"))
                {
                    name = name.Substring(4);
                    ((IElementSetMetaClass) elementInstance).setMetaClass(UmlClasses[name]);
                }
                else if (name.StartsWith("mofext:"))
                {
                    name = name.Substring(7);
                    ((IElementSetMetaClass) elementInstance).setMetaClass(MofClasses[name]);
                }
                else
                {
                    throw new InvalidOperationException($"Found unknown class: {name}");
                }

                // We strip out the property and id information. 
                // It is not really required 
                elementInstance.unset(typeProperty);
            }

            // Now we handle the generalization information. 
            // For all classes and associations, whose type is class or associations, get the generalization property and convert it to a list of classes
            foreach (var elementInstance in umlDescendents
                .Where(x => (x as IElement)?.metaclass?.Equals(UmlClasses["Generalization"]) == true))
            {
                if (elementInstance.isSet("general"))
                {
                    var general = elementInstance.get("general").ToString();
                    if (UmlClasses.ContainsKey(general))
                    {
                        elementInstance.set("general", UmlClasses[general]);
                    }
                    else if (UmlAssociations.ContainsKey(general))
                    {
                        elementInstance.set("general", UmlAssociations[general]);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Found unknown generalization: {general}");
                    }
                }
            }
        }

        /// <summary>
        ///     Performs a full bootstrap by reading in the uml class
        /// </summary>
        /// <param name="paths">Paths storing the uml information</param>
        /// <returns>The instance of the bootstrapper</returns>
        public static Bootstrapper PerformFullBootstrap(FilePaths paths)
        {
            var factory = new MofFactory();
            var umlExtent = new MofUriExtent("datenmeister:///uml");
            var mofExtent = new MofUriExtent("datenmeister:///mof");
            var primitiveExtent = new MofUriExtent("datenmeister:///prototypes");
            var loader = new SimpleLoader(factory);
            loader.Load(primitiveExtent, paths.PathPrimitive);
            loader.Load(umlExtent, paths.PathUml);
            loader.Load(mofExtent, paths.PathMof);

            var bootStrapper = new Bootstrapper(primitiveExtent, umlExtent, mofExtent);
            bootStrapper.Strap();
            return bootStrapper;
        }

        /// <summary>
        /// Performs a full bootstrap by reading the uml classes 
        /// </summary>
        /// <param name="filePaths">Paths storing the uml</param>
        /// <param name="workspace">The workspace to which the extents will be aded</param>
        /// <param name="layerLogic">The datalayerlogic being used to add the </param>
        /// <param name="dataLayer">The datalayer to which the new extents will be added</param>
        /// <returns></returns>
        public static Bootstrapper PerformFullBootstrap(FilePaths filePaths, Workspace<IExtent> workspace,
            IDataLayerLogic layerLogic, IDataLayer dataLayer)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));
            if (layerLogic == null) throw new ArgumentNullException(nameof(layerLogic));
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            var strapper = PerformFullBootstrap(filePaths);
            workspace.AddExtent(strapper.MofInfrastructure);
            workspace.AddExtent(strapper.UmlInfrastructure);
            workspace.AddExtent(strapper.PrimitiveInfrastructure);
            layerLogic.AssignToDataLayer(strapper.MofInfrastructure, dataLayer);
            layerLogic.AssignToDataLayer(strapper.UmlInfrastructure, dataLayer);
            layerLogic.AssignToDataLayer(strapper.PrimitiveInfrastructure, dataLayer);
            layerLogic.Create<FillTheMOF, _MOF>(dataLayer);
            layerLogic.Create<FillTheUML, _UML>(dataLayer);
            layerLogic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(dataLayer);
            return strapper;
        }

        /// <summary>
        /// Defines the file paths for doing the boot strap. 
        /// This avoids the clutterin of arguments
        /// </summary>
        public class FilePaths
        {
            public string PathPrimitive { get; set; }
            public string PathUml { get; set; }
            public string PathMof { get; set; }
        }
    }
}