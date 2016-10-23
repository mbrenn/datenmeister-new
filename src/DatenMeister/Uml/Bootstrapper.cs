using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Uml
{
    /// <summary>
    ///     Performs the bootstrap on the given object
    /// </summary>
    public class Bootstrapper
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        ///     Stores a vlaue indicating whether the run was already performed.
        ///     If yes, the method is locked
        /// </summary>
        private bool _wasRun;

        private static readonly string TypeProperty;
        private static readonly string IdProperty;

        public Dictionary<string, IElement> MofClasses { get; } = new Dictionary<string, IElement>();

        public Dictionary<string, IElement> UmlClasses { get; } = new Dictionary<string, IElement>();
        public Dictionary<string, IElement> UmlAssociations { get; } = new Dictionary<string, IElement>();

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent MofInfrastructure { get; private set; }

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent UmlInfrastructure { get; private set; }

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent PrimitiveInfrastructure { get; private set; }

        /// <summary>
        /// The static constructor
        /// </summary>
        static Bootstrapper()
        {
            TypeProperty = (Namespaces.Xmi + "type").ToString();
            IdProperty = (Namespaces.Xmi + "id").ToString();
        }

        /// <summary>
        /// Initializes the bootstrapper after having the the necessary extents
        /// </summary>
        /// <param name="workspaceLogic">Datalayerlogic  being used</param>
        public Bootstrapper(
            IWorkspaceLogic workspaceLogic)
        {
            if (workspaceLogic == null) throw new ArgumentNullException(nameof(workspaceLogic));

            _workspaceLogic = workspaceLogic;
        }

        private void StrapUml(
            IUriExtent primitiveInfrastructure,
            IUriExtent umlInfrastructure,
            Workspace mofDataLayer)
        {
            StrapUmlSlim(primitiveInfrastructure, umlInfrastructure);

            StrapUml(mofDataLayer);
        }

        private void StrapUmlSlim(IUriExtent primitiveInfrastructure, IUriExtent umlInfrastructure)
        {
            if (umlInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(umlInfrastructure));
            }
            if (primitiveInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(primitiveInfrastructure));
            }

            UmlInfrastructure = umlInfrastructure;
            PrimitiveInfrastructure = primitiveInfrastructure;
        }

        private void StrapMof(IUriExtent primitiveInfrastructure, IUriExtent umlInfrastructure, IUriExtent mofInfrastructure)
        {
            StrapMofSlim(primitiveInfrastructure, umlInfrastructure, mofInfrastructure);

            StrapMof();
        }

        private void StrapMofSlim(IUriExtent primitiveInfrastructure, IUriExtent umlInfrastructure, IUriExtent mofInfrastructure)
        {
            if (umlInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(umlInfrastructure));
            }
            if (primitiveInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(primitiveInfrastructure));
            }
            if (mofInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(mofInfrastructure));
            }

            UmlInfrastructure = umlInfrastructure;
            MofInfrastructure = mofInfrastructure;
            PrimitiveInfrastructure = primitiveInfrastructure;
        }

        /// <summary>
        ///     Performs the bootstrap
        /// </summary>
        private void StrapMof()
        {
            if (_wasRun)
            {
                throw new InvalidOperationException("Bootstrapper was already run. Create a new object for your run");
            }

            _wasRun = true;

            var umlDescendents = AllDescendentsQuery.GetDescendents(UmlInfrastructure).ToList();
            var primitiveDescendents = AllDescendentsQuery.GetDescendents(PrimitiveInfrastructure).ToList();
            var mofDescendents = AllDescendentsQuery.GetDescendents(MofInfrastructure).ToList();
            var allElements =
                umlDescendents
                    .Union(primitiveDescendents)
                    .Union(mofDescendents)
                    .ToList();

            // First, find the all classes of the uml namespace...

            var idToElementCache = new Dictionary<string, IElement>();

            // Go through all elements and set the id
            foreach (var element in allElements.OfType<IElement>())
            {
                // Skip the package imports since we are not able to handle these
                if (element.isSet(TypeProperty) && element.get(TypeProperty).ToString() == "uml:PackageImport")
                {
                    continue;
                }

                if (element.isSet(IdProperty))
                {
                    var id = element.get(IdProperty).ToString();
                    if (id.StartsWith("_"))
                    {
                        // Due to a problem in the uml.xmi, duplicate IDs might be in the packageImport
                        // We also skip these... uml.xmi and primitivetypes.xmi have this id. Not used
                        // All duplicate items start with an underscore
                        continue;
                    }

                    if (idToElementCache.ContainsKey(id))
                    {
                        throw new InvalidOperationException($"ID '{id}' is duplicate");
                    }

                    idToElementCache[id] = element;
                }

                element.unset(IdProperty);
            }

            // Go through all found classes and store them into the dictionaries
            foreach (var classInstance in umlDescendents.OfType<IElement>().Where(x => x.isSet("name")))
            {
                var name = classInstance.get("name").ToString();
                var typeValue = classInstance.isSet(TypeProperty) ? classInstance.get(TypeProperty).ToString() : null;
                if (typeValue == "uml:Class")
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
                .Where(x => x.isSet(TypeProperty) && x.get(TypeProperty).ToString() == "uml:Class");
            foreach (var classInstance in allClasses.Cast<IElement>())
            {
                var name = classInstance.get("name").ToString();
                MofClasses[name] = classInstance;
            }

            // After having the classes from MOF and UML, go through all classes and set
            // the metaclass of these element depending on the attribute value of Xmi:Type
            foreach (var elementInstance in allElements.Where(x => x.isSet(TypeProperty)))
            {
                var name = elementInstance.get(TypeProperty).ToString();
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
                elementInstance.unset(TypeProperty);
            }

            // Now we handle the generalization information. 
            // For all classes and associations, whose type is class or associations, get the generalization property and convert it to a list of classes
            EvaluateGeneralizations(umlDescendents, UmlClasses["Generalization"]);
            EvaluateGeneralizations(mofDescendents, UmlClasses["Generalization"]);

            // ConvertPropertiesToRealProperties(allElements);
        }

        /// <summary>
        ///     Performs the bootstrap
        /// </summary>
        private void StrapUml(Workspace metaLayer)
        {
            if (_wasRun)
            {
                throw new InvalidOperationException("Bootstrapper was already run. Create a new object for your run");
            }

            _wasRun = true;

            var umlDescendents = AllDescendentsQuery.GetDescendents(UmlInfrastructure).ToList();
            var primitiveDescendents = AllDescendentsQuery.GetDescendents(PrimitiveInfrastructure).ToList();
            var allElements =
                    umlDescendents
                    .Union(primitiveDescendents)
                    .ToList();

            // First, find the all classes of the uml namespace...
            var idToElementCache = new Dictionary<string, IElement>();

            // Go through all elements and set the id
            foreach (var element in allElements.OfType<IElement>())
            {
                // Skip the package imports since we are not able to handle these
                if (element.isSet(TypeProperty) && element.get(TypeProperty).ToString() == "uml:PackageImport")
                {
                    continue;
                }

                if (element.isSet(IdProperty))
                {
                    var id = element.get(IdProperty).ToString();
                    if (id.StartsWith("_"))
                    {
                        // Due to a problem in the uml.xmi, duplicate IDs might be in the packageImport
                        // We also skip these... uml.xmi and primitivetypes.xmi have this id. Not used
                        // All duplicate items start with an underscore
                        continue;
                    }

                    if (idToElementCache.ContainsKey(id))
                    {
                        throw new InvalidOperationException($"ID '{id}' is duplicate");
                    }

                    idToElementCache[id] = element;
                }

                element.unset(IdProperty);
            }

            // Go through all found classes and store them into the dictionaries
            foreach (var classInstance in umlDescendents.OfType<IElement>().Where(x => x.isSet("name")))
            {
                var name = classInstance.get("name").ToString();
                var typeValue = classInstance.isSet(TypeProperty) ? classInstance.get(TypeProperty).ToString() : null;
                if (typeValue == "uml:Class")
                {
                    UmlClasses[name] = classInstance;
                }
                if (typeValue == "uml:Association")
                {
                    UmlAssociations[name] = classInstance;
                }
            }

            // After having the classes from MOF and UML, go through all classes and set
            // the metaclass of these element depending on the attribute value of Xmi:Type
            // If the current layer is UML, the metaLayer needs to trace to the MOF metalayer
            var extentsOfMetaLayer = _workspaceLogic.GetExtentsForWorkspace(metaLayer).ToList();
            var umlElements =
                extentsOfMetaLayer.First(x => x.contextURI() == WorkspaceNames.UriUml).elements().GetAllDescendants();
            var mofElements =
                extentsOfMetaLayer.First(x => x.contextURI() == WorkspaceNames.UriMof).elements().GetAllDescendants();
            var mofMetaClasses = 
                mofElements
                    .Cast<IElement>()
                    .Where(x => x.isSet("name") && x.metaclass?.get("name").ToString() == "Class")
                    .ToList();

            // Hacky hack to get rid of one of the tags and the duplicate MOF Elements
            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.get("name").ToString() == "Tag").ElementAt(0));
            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.get("name").ToString() == "Factory").ElementAt(0));
            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.get("name").ToString() == "Extent").ElementAt(0));
            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.get("name").ToString() == "Element").ElementAt(0));

            var umlMetaClasses =
                umlElements
                    .Cast<IElement>()
                    .Where(x => x.isSet("name") && x.metaclass?.get("name").ToString() == "Class");

            var umlNameCache = umlMetaClasses
                    .ToDictionary(x => x.get("name").ToString(), x => x);
            var mofNameCache = mofMetaClasses
                    .ToDictionary(x => x.get("name").ToString(), x => x);

            foreach (var elementInstance in allElements.Where(x => x.isSet(TypeProperty)))
            {
                var name = elementInstance.get(TypeProperty).ToString();

                // Find it in the higher instance mof
                IElement metaClass;
                
                // Translates the uri to the correct one
                if (name.StartsWith("uml:"))
                {
                    name = name.Substring(4);
                    metaClass = umlNameCache[name];

                }
                else if (name.StartsWith("mofext:"))
                {
                    name = name.Substring(7);
                    metaClass = mofNameCache[name];
                }
                else
                {
                    throw new InvalidOperationException($"Unknown name: {name}");
                }

                if (metaClass == null)
                {
                    throw new InvalidOperationException($"Metaclass for {name} is not found");
                }

                ((IElementSetMetaClass)elementInstance).setMetaClass(metaClass);

                // We strip out the property and id information. 
                // It is not really required 
                elementInstance.unset(TypeProperty);
            }

            var metaClassGeneralization =
                extentsOfMetaLayer.First(x => x.contextURI() == WorkspaceNames.UriUml)
                    .element(WorkspaceNames.UriUml + "#Generalization");
            EvaluateGeneralizations(umlDescendents, metaClassGeneralization);

            // ConvertPropertiesToRealProperties(allElements);
        }

        /// <summary>
        /// Evaluates the 
        /// </summary>
        /// <param name="umlDescendents"></param>
        /// <param name="metaClassGeneralization">The metaclass being used to find the generalization class type</param>
        private void EvaluateGeneralizations(IEnumerable<IObject> umlDescendents, IElement metaClassGeneralization)
        {
            // Now we handle the generalization information. 
            // For all classes and associations, whose type is class or associations, get the generalization property and convert it to a list of classes
            foreach (var elementInstance in umlDescendents
                .Where(x => (x as IElement)?.metaclass?.Equals(metaClassGeneralization) == true))
            {
                if (elementInstance.isSet("general"))
                {
                    var general = elementInstance.getFirstOrDefault("general");
                    var generalAsString = general.ToString();
                    if (DotNetHelper.IsOfMofObject(general))
                    {
                        elementInstance.set("general", general as IElement);
                    }
                    else if (UmlClasses.ContainsKey(generalAsString))
                    {
                        elementInstance.set("general", UmlClasses[generalAsString]);
                    }
                    else if (UmlAssociations.ContainsKey(generalAsString))
                    {
                        elementInstance.set("general", UmlAssociations[generalAsString]);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Found unknown generalization: {general}");
                    }
                }
            }
        }

        /// <summary>
        /// Converts all properties of all objects to the real property function. 
        /// This method is not needed anymore, since we are using now strings as the property reference
        /// and not the real properties anymore
        /// </summary>
        /// <param name="allElements"></param>
        private void ConvertPropertiesToRealProperties(IEnumerable<IObject> allElements)
        {
            // Now we replace the property information from string form to real properties
            List<Action> actions = new List<Action>();

            foreach (var element in allElements.OfType<IObjectAllProperties>())
            {
                var asElement = element as IElement;
                if (asElement == null)
                {
                    throw new InvalidOperationException($"Given Element is not an element: {element.ToString()}");
                }

                var metaClass = asElement.getMetaClass();
                if (metaClass == null)
                {
                    // The item does not have a metaclass, most probable a reference
                    continue;
                }

                var propertiesOfMetaClass = ClassifierMethods.GetPropertyNamesOfClassifier(metaClass).ToList();
                var mapping = new Dictionary<string, string>();
                foreach (var property in propertiesOfMetaClass)
                {
                    mapping[property] = property;
                }

                foreach (var property in element.getPropertiesBeingSet())
                {
                    var textProperty = property;
                    if (textProperty.Contains(Namespaces.Xmi.ToString())
                        || textProperty == "href")
                    {
                        continue;
                    }

                    var value = asElement.get(property);

                    // Find the property in the properties of the metaclass

                    if (!mapping.ContainsKey(textProperty))
                    {
                        throw new InvalidOperationException($"Property {textProperty} not found in {element}");
                    }

                    var realProperty = mapping[textProperty];

                    actions.Add(() =>
                    {
                        asElement.unset(textProperty);
                        asElement.set(realProperty, value);
                    });
                }
            }

            // Now, execute the collected actions
            foreach (var action in actions)
            {
                action();
            }
        }

        /// <summary>
        /// Performs a full bootstrap by reading in the uml class
        /// </summary>
        /// <param name="workspaceLogic">The datalayer logic to be used</param>
        /// <param name="dataLayer">The datalayer to be filled before the bootstrap itself</param>
        /// <param name="mode">The mode being used to load the information</param>
        /// <param name="paths">Paths storing the uml information</param>
        /// <returns>The instance of the bootstrapper</returns>
        public static Bootstrapper PerformFullBootstrap(
            IWorkspaceLogic workspaceLogic,
            Workspace dataLayer,
            BootstrapMode mode,
            FilePaths paths = null)
        {
            var isSlim = mode == BootstrapMode.SlimMof || mode == BootstrapMode.SlimUml;
            if (workspaceLogic == null) throw new ArgumentNullException(nameof(workspaceLogic));
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            var factory = new InMemoryFactory();
            var umlExtent = new InMemoryUriExtent(WorkspaceNames.UriUml);
            var mofExtent = new InMemoryUriExtent(WorkspaceNames.UriMof);
            var primitiveExtent = new InMemoryUriExtent(WorkspaceNames.UriPrimitiveTypes);

            if (!isSlim)
            {
                var loader = new SimpleLoader(factory);
                if (paths == null || paths.LoadFromEmbeddedResources)
                {
                    loader.LoadFromEmbeddedResource(primitiveExtent, "DatenMeister.XmiFiles.PrimitiveTypes.xmi");
                    loader.LoadFromEmbeddedResource(umlExtent, "DatenMeister.XmiFiles.UML.xmi");

                    if (mode == BootstrapMode.Mof)
                    {
                        loader.LoadFromEmbeddedResource(mofExtent, "DatenMeister.XmiFiles.MOF.xmi");
                    }
                }
                else
                {
                    loader.LoadFromFile(primitiveExtent, paths.PathPrimitive);
                    loader.LoadFromFile(umlExtent, paths.PathUml);
                    if (mode == BootstrapMode.Mof)
                    {
                        loader.LoadFromFile(mofExtent, paths.PathMof);
                    }
                }
            }

            // Assigns the extents to the datalayer
            dataLayer.AddExtent(umlExtent);
            dataLayer.AddExtent(primitiveExtent);
            if (mode == BootstrapMode.Mof || mode == BootstrapMode.SlimMof)
            {
                dataLayer.AddExtent(mofExtent);
            }
            
            var bootStrapper = new Bootstrapper(workspaceLogic);
            if (isSlim)
            {
                // Now do the bootstrap
                if (mode == BootstrapMode.SlimMof)
                {
                    bootStrapper.StrapMofSlim(
                        primitiveInfrastructure: primitiveExtent,
                        umlInfrastructure: umlExtent,
                        mofInfrastructure: mofExtent);
                }
                else if (mode == BootstrapMode.SlimUml)
                {
                    bootStrapper.StrapUmlSlim(
                        primitiveInfrastructure: primitiveExtent,
                        umlInfrastructure: umlExtent);
                }

                dataLayer.Set(new _UML());
                dataLayer.Set(new _PrimitiveTypes());

                if (mode == BootstrapMode.SlimMof)
                {
                    dataLayer.Set(new _MOF());
                }
            }
            else
            {
                // Now do the bootstrap
                if (mode == BootstrapMode.Mof)
                {
                    bootStrapper.StrapMof(
                        primitiveInfrastructure: primitiveExtent,
                        umlInfrastructure: umlExtent,
                        mofInfrastructure: mofExtent);
                }
                else if (mode == BootstrapMode.Uml)
                {
                    bootStrapper.StrapUml(
                        primitiveInfrastructure: primitiveExtent,
                        umlInfrastructure: umlExtent,
                        mofDataLayer: dataLayer.MetaWorkspace);
                }

                dataLayer.Create<FillTheUML, _UML>();
                dataLayer.Create<FillThePrimitiveTypes, _PrimitiveTypes>();

                if (mode == BootstrapMode.Mof)
                {
                    dataLayer.Create<FillTheMOF, _MOF>();
                }
            }

            return bootStrapper;
        }

        /// <summary>
        /// Performs a full bootstrap by reading the uml classes 
        /// </summary>
        /// <param name="filePaths">Paths storing the uml</param>
        /// <param name="workspace">The workspace to which the extents will be aded</param>
        /// <param name="workspaceLogic">The datalayerlogic being used to add the </param>
        /// <param name="dataLayer">The datalayer to which the new extents will be added</param>
        /// <param name="mode">Bootstrap mode. Is this for UML or MOF?</param>
        /// <returns></returns>
        public static Bootstrapper PerformFullBootstrap(
            FilePaths filePaths,
            Workspace workspace,
            IWorkspaceLogic workspaceLogic,
            Workspace dataLayer,
            BootstrapMode mode)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));
            if (workspaceLogic == null) throw new ArgumentNullException(nameof(workspaceLogic));
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            return  PerformFullBootstrap(workspaceLogic, dataLayer, mode, filePaths);
        }

        /// <summary>
        /// Defines the file paths for doing the boot strap. 
        /// This avoids the clutterin of arguments
        /// </summary>
        public class FilePaths
        {
            /// <summary>
            /// Gets or sets a value indicating whether the resources shall be loaded from embedded resources
            /// </summary>
            public bool LoadFromEmbeddedResources { get; set; }

            public string PathPrimitive { get; set; }
            public string PathUml { get; set; }
            public string PathMof { get; set; }
        }
    }
}