using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;using DatenMeister.Filler;
using DatenMeister.Uml.Helper;
using DatenMeister.XMI;

namespace DatenMeister.Uml
{
    /// <summary>
    ///     Performs the bootstrap on the given object
    /// </summary>
    public class Bootstrapper
    {
        private readonly IDataLayerLogic _dataLayerLogic;

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
        /// <param name="primitiveInfrastructure">Extent reflecting the primitive structure XMI definition</param>
        /// <param name="umlInfrastructure">Extent reflecting the Uml infrastructure XMI definition</param>
        /// <param name="mofInfrastructure">Extent reflecting the MOF infrastructure XMI definition</param>
        /// <param name="dataLayerLogic">Datalayerlogic  being used</param>
        public Bootstrapper(
            IUriExtent primitiveInfrastructure,
            IUriExtent umlInfrastructure,
            IUriExtent mofInfrastructure,
            IDataLayerLogic dataLayerLogic)
        {
            if (dataLayerLogic == null) throw new ArgumentNullException(nameof(dataLayerLogic));
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

            _dataLayerLogic = dataLayerLogic;
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

            var idToElementCache = new Dictionary<string, IElement>();

            // Go through all elements and set the id
            foreach (var element in allElements.OfType<IElement>())
            {
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

                element.unset(IdProperty);
            }

            // Go through all found classes and store them into the dictionaries
            foreach (var classInstance in umlDescendents.OfType<IElement>().Where(x => x.isSet("name")))
            {
                var name = classInstance.get("name").ToString();
                var typeValue = classInstance.isSet(TypeProperty) ? classInstance.get(TypeProperty).ToString() : null;
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

            // Now we replace the property information from string form to real properties
            List<Action> actions = new List<Action>();
            var classifierMethod = new ClassifierMethods(_dataLayerLogic, true);
            foreach (var element in allElements.OfType<IObjectAllProperties>())
            {
                var asElement = element as IElement;
                if (asElement == null)
                {
                    throw new InvalidOperationException($"Given Element is not an element: ");
                }

                var metaClass = asElement.getMetaClass();
                if (metaClass == null)
                {
                    // The item does not have a metaclass, most probable a reference
                    continue;
                }

                var propertiesOfMetaClass = classifierMethod.GetPropertiesOfClassifier(metaClass).ToList();
                var mapping = new Dictionary<string, IElement>();
                foreach (var property in propertiesOfMetaClass)
                {
                    mapping[property.get("name").ToString()] = property;
                }
                
                foreach (var property in element.getPropertiesBeingSet())
                {
                    var textProperty = property.ToString();
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
        ///     Performs a full bootstrap by reading in the uml class
        /// </summary>
        /// <param name="paths">Paths storing the uml information</param>
        /// <param name="dataLayerLogic">The datalayer logic to be used</param>
        /// <param name="dataLayer">The datalayer to be filled before the bootstrap itself</param>
        /// <returns>The instance of the bootstrapper</returns>
        public static Bootstrapper PerformFullBootstrap(
            FilePaths paths,
            IDataLayerLogic dataLayerLogic,
            IDataLayer dataLayer)
        {
            var factory = new MofFactory();
            var umlExtent = new MofUriExtent("datenmeister:///uml");
            var mofExtent = new MofUriExtent("datenmeister:///mof");
            var primitiveExtent = new MofUriExtent("datenmeister:///prototypes");
            var loader = new SimpleLoader(factory);
            loader.Load(primitiveExtent, paths.PathPrimitive);
            loader.Load(umlExtent, paths.PathUml);
            loader.Load(mofExtent, paths.PathMof);

            // Assigns the extents to the datalayer
            if (dataLayer != null && dataLayerLogic != null)
            {
                dataLayerLogic.AssignToDataLayer(mofExtent, dataLayer);
                dataLayerLogic.AssignToDataLayer(umlExtent, dataLayer);
                dataLayerLogic.AssignToDataLayer(primitiveExtent, dataLayer);
                dataLayerLogic.Create<FillTheMOF, _MOF>(dataLayer);
                dataLayerLogic.Create<FillTheUML, _UML>(dataLayer);
                dataLayerLogic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(dataLayer);
            }
            else
            {
                // To support the creation of _MOF and _UML, we need to have datalayers and their logic
                throw new InvalidOperationException("datalayers or dataLayerLogic is null");
            }

            // Now do the bootstrap
            var bootStrapper = new Bootstrapper(primitiveExtent, umlExtent, mofExtent, dataLayerLogic);
            bootStrapper.Strap();
            return bootStrapper;
        }

        /// <summary>
        /// Performs a full bootstrap by reading the uml classes 
        /// </summary>
        /// <param name="filePaths">Paths storing the uml</param>
        /// <param name="workspace">The workspace to which the extents will be aded</param>
        /// <param name="dataLayerLogic">The datalayerlogic being used to add the </param>
        /// <param name="dataLayer">The datalayer to which the new extents will be added</param>
        /// <returns></returns>
        public static Bootstrapper PerformFullBootstrap(FilePaths filePaths, Workspace<IExtent> workspace,
            IDataLayerLogic dataLayerLogic, IDataLayer dataLayer)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));
            if (dataLayerLogic == null) throw new ArgumentNullException(nameof(dataLayerLogic));
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            var strapper = PerformFullBootstrap(filePaths, dataLayerLogic, dataLayer);

            workspace.AddExtent(strapper.MofInfrastructure);
            workspace.AddExtent(strapper.UmlInfrastructure);
            workspace.AddExtent(strapper.PrimitiveInfrastructure);
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