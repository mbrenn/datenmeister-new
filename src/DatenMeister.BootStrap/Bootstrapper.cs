using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.XmiFiles;
using DatenMeister.Types.Plugin;

namespace DatenMeister.BootStrap
{
    /// <summary>
    ///     Performs the bootstrap on the given object
    /// </summary>
    public class Bootstrapper
    {
        private static readonly string TypeProperty = (Namespaces.Xmi + "type").ToString();
        private static readonly string IdProperty = (Namespaces.Xmi + "id").ToString();
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        ///     Stores a value indicating whether the run was already performed.
        ///     If yes, the method is locked
        /// </summary>
        private bool _wasRun;

        /// <summary>
        /// The static constructor
        /// </summary>
        static Bootstrapper()
        {
        }

        /// <summary>
        /// Initializes the bootstrapper after having the the necessary extents
        /// </summary>
        /// <param name="workspaceLogic">Datalayerlogic  being used</param>
        public Bootstrapper(
            IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
        }

        private Dictionary<string, IElement> MofClasses { get; } = new();
        private Dictionary<string, IElement> UmlClasses { get; } = new();
        private Dictionary<string, IElement> UmlAssociations { get; } = new();

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent? MofInfrastructure { get; private set; }

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent? UmlInfrastructure { get; private set; }

        /// <summary>
        ///     Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent? PrimitiveTypesInfrastructure { get; private set; }

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
            UmlInfrastructure = umlInfrastructure ?? throw new ArgumentNullException(nameof(umlInfrastructure));
            PrimitiveTypesInfrastructure = primitiveInfrastructure ??
                                           throw new ArgumentNullException(nameof(primitiveInfrastructure));
        }

        private void StrapMof(IUriExtent primitiveInfrastructure, IUriExtent umlInfrastructure,
            IUriExtent mofInfrastructure)
        {
            StrapMofSlim(primitiveInfrastructure, umlInfrastructure, mofInfrastructure);

            StrapMof();
        }

        private void StrapMofSlim(IUriExtent primitiveInfrastructure, IUriExtent umlInfrastructure,
            IUriExtent mofInfrastructure)
        {
            UmlInfrastructure = umlInfrastructure ?? throw new ArgumentNullException(nameof(umlInfrastructure));
            MofInfrastructure = mofInfrastructure ?? throw new ArgumentNullException(nameof(mofInfrastructure));
            PrimitiveTypesInfrastructure = primitiveInfrastructure ??
                                           throw new ArgumentNullException(nameof(primitiveInfrastructure));
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

            var umlDescendents = AllDescendentsQuery
                .GetDescendents(UmlInfrastructure
                                ?? throw new InvalidOperationException("Uml Workspace not found"))
                .ToList();
            var primitiveDescendents = AllDescendentsQuery
                .GetDescendents(PrimitiveTypesInfrastructure
                                ?? throw new InvalidOperationException("PrimitiveTypes Workspace not found"))
                .ToList();
            var mofDescendents = AllDescendentsQuery
                .GetDescendents(MofInfrastructure
                                ?? throw new InvalidOperationException("Mof Workspace not found"))
                .ToList();

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
                if (element.getOrDefault<string>(TypeProperty) == "uml:PackageImport")
                {
                    continue;
                }

                if (element.isSet(IdProperty))
                {
                    var id = element.getOrDefault<string>(IdProperty);
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
                var name = classInstance.getOrDefault<string>("name");
                var typeValue = classInstance.isSet(TypeProperty)
                    ? classInstance.getOrDefault<string>(TypeProperty)
                    : null;
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
                .Where(x => x.isSet(TypeProperty) && x.getOrDefault<string>(TypeProperty) == "uml:Class");
            foreach (var classInstance in allClasses.Cast<IElement>())
            {
                var name = classInstance.getOrDefault<string>("name");
                MofClasses[name] = classInstance;
            }

            // After having the classes from MOF and UML, go through all classes and set
            // the metaclass of these element depending on the attribute value of Xmi:Type
            foreach (var elementInstance in allElements.Where(x => x.isSet(TypeProperty)))
            {
                var name = elementInstance.getOrDefault<string>(TypeProperty);
                if (name.StartsWith("uml:"))
                {
                    name = name.Substring(4);
                    ((IElementSetMetaClass) elementInstance).SetMetaClass(UmlClasses[name]);
                }
                else if (name.StartsWith("mofext:"))
                {
                    name = name.Substring(7);
                    ((IElementSetMetaClass) elementInstance).SetMetaClass(MofClasses[name]);
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

            var umlDescendents = AllDescendentsQuery
                .GetDescendents(UmlInfrastructure ?? throw new InvalidOperationException("UmlInfrastructure == null"))
                .ToList();
            var primitiveDescendents = AllDescendentsQuery
                .GetDescendents(PrimitiveTypesInfrastructure ??
                                throw new InvalidOperationException("PrimitiveTypesInfrastructure == null")).ToList();
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
                if (element.isSet(TypeProperty) && element.getOrDefault<string>(TypeProperty) == "uml:PackageImport")
                {
                    continue;
                }

                if (element.isSet(IdProperty))
                {
                    var id = element.getOrDefault<string>(IdProperty) ?? string.Empty;
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
                var name = classInstance.getOrDefault<string>("name");
                var typeValue = classInstance.isSet(TypeProperty)
                    ? classInstance.getOrDefault<string>(TypeProperty)
                    : null;
                if (typeValue == "uml:Class")
                    UmlClasses[name] = classInstance;

                if (typeValue == "uml:Association")
                    UmlAssociations[name] = classInstance;
            }

            // After having the classes from MOF and UML, go through all classes and set
            // the metaclass of these element depending on the attribute value of Xmi:Type
            // If the current layer is UML, the metaLayer needs to trace to the MOF metalayer
            var extentsOfMetaLayer = _workspaceLogic.GetExtentsForWorkspace(metaLayer).ToList();
            var umlElements = extentsOfMetaLayer.First(x => x.contextURI() == WorkspaceNames.UriExtentUml).elements()
                .GetAllDescendants();
            var mofElements = extentsOfMetaLayer.First(x => x.contextURI() == WorkspaceNames.UriExtentMof).elements()
                .GetAllDescendants();
            var mofMetaClasses =
                mofElements
                    .Cast<IElement>()
                    .Where(x => x.isSet("name") && x.metaclass?.getOrDefault<string>("name") == "Class")
                    .ToList();

            // Hacky hack to get rid of one of the tags and the duplicate MOF Elements
            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.getOrDefault<string>("name") == "Tag").ElementAt(0));
            while (mofMetaClasses.Count(x => x.getOrDefault<string>("name") == "Tag") > 1)
            {
                mofMetaClasses.Remove(mofMetaClasses.Where(x => x.getOrDefault<string>("name") == "Tag").ElementAt(1));
            }

            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.getOrDefault<string>("name") == "Factory").ElementAt(0));
            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.getOrDefault<string>("name") == "Extent").ElementAt(0));
            mofMetaClasses.Remove(mofMetaClasses.Where(x => x.getOrDefault<string>("name") == "Element").ElementAt(0));

            // Gets all the elements which are classes of the uml
            var umlMetaClasses =
                umlElements
                    .Cast<IElement>()
                    .Where(x => x.isSet("name") && x.metaclass?.getOrDefault<string>("name") == "Class")
                    .ToList();

            // Caches all the classes.

            var umlNameCache = umlMetaClasses
                .ToDictionary(x => x.getOrDefault<string>("name"), x => x);
            var mofNameCache = mofMetaClasses
                .ToDictionary(x => x.getOrDefault<string>("name"), x => x);

            foreach (var elementInstance in allElements.Where(x => x.isSet(TypeProperty)))
            {
                var name = elementInstance.getOrDefault<string>(TypeProperty);

                // Find it in the higher instance mof
                IElement? metaClass;

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

                ((IElementSetMetaClass) elementInstance).SetMetaClass(metaClass);

                // We strip out the property and id information.
                // It is not really required
                elementInstance.unset(TypeProperty);
            }

            var metaClassGeneralization =
                extentsOfMetaLayer.First(x => x.contextURI() == WorkspaceNames.UriExtentUml)
                    .element(WorkspaceNames.UriExtentUml + "#Generalization");
            if (metaClassGeneralization == null)
                throw new InvalidOperationException("Type for Generalization is not found");

            EvaluateGeneralizations(umlDescendents, metaClassGeneralization);

            // Go through all the elements and get the ownedAttributes and sets the correct property types
            foreach (var classes in umlMetaClasses)
            {
                var ownedAttributes =
                    classes.getOrDefault<IReflectiveCollection>(_UML._StructuredClassifiers._Class.ownedAttribute);
                if (ownedAttributes == null)
                {
                    continue;
                }

                foreach (var ownedAttribute in ownedAttributes.OfType<IElement>())
                {
                    var type = ownedAttribute.getOrDefault<string>(_UML._CommonStructure._TypedElement.type);
                    if (!string.IsNullOrEmpty(type) && UmlClasses.TryGetValue(type, out var foundUml))
                    {
                        ownedAttribute.set(_UML._CommonStructure._TypedElement.type, foundUml);
                    }
                }
            }

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
                    if (general == null)
                    {
                        throw new InvalidOperationException(elementInstance.ToString());
                    }

                    var generalAsString = general.ToString() ??
                                          throw new InvalidOperationException("ToString did not work");
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
        /// Performs a full bootstrap by reading in the uml class
        /// </summary>
        /// <param name="workspaceLogic">The datalayer logic to be used</param>
        /// <param name="workspace">The datalayer to be filled before the bootstrap itself</param>
        /// <param name="mode">The mode being used to load the information</param>
        /// <param name="paths">Paths storing the uml information</param>
        /// <returns>The instance of the bootstrapper</returns>
        public static Bootstrapper PerformFullBootstrap(
            IWorkspaceLogic workspaceLogic,
            Workspace workspace,
            BootstrapMode mode,
            FilePaths? paths = null)
        {
            if (workspaceLogic == null) throw new ArgumentNullException(nameof(workspaceLogic));
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));

            string xmlPrimitiveTypes;
            string xmlMof;
            string xmlUml;

            if (paths?.LoadFromEmbeddedResources != false)
            {
                xmlPrimitiveTypes = XmiResources.GetStringFromStream(
                    XmiResources.GetPrimitiveTypeStream);
                xmlUml = XmiResources.GetStringFromStream(
                    XmiResources.GetUmlStream);
                xmlMof = XmiResources.GetStringFromStream(
                    XmiResources.GetMofStream);
            }
            else
            {
                xmlPrimitiveTypes =
                    File.ReadAllText(paths.PathPrimitive ?? throw new InvalidOperationException("Path is null"));
                xmlUml = File.ReadAllText(paths.PathUml ?? throw new InvalidOperationException("Path is null"));
                xmlMof = File.ReadAllText(paths.PathMof ?? throw new InvalidOperationException("Path is null"));
            }

            var umlExtent = new MofUriExtent(
                new XmiProvider(XDocument.Parse(xmlUml)),
                WorkspaceNames.UriExtentUml,
                workspaceLogic.ScopeStorage);
            umlExtent.LocalSlimUmlEvaluation = true;

            umlExtent.AddAlternativeUri("http://www.omg.org/spec/UML/20131001");
            umlExtent.AddAlternativeUri("http://www.omg.org/spec/UML/20131001/UML.xmi");
            umlExtent.GetConfiguration().ExtentType = UmlPlugin.ExtentType;

            var mofExtent = new MofUriExtent(
                new XmiProvider(XDocument.Parse(xmlMof)),
                WorkspaceNames.UriExtentMof,
                workspaceLogic.ScopeStorage);
            mofExtent.AddAlternativeUri("http://www.omg.org/spec/MOF/20131001");
            mofExtent.LocalSlimUmlEvaluation = true;

            var primitiveExtent = new MofUriExtent(
                new XmiProvider(XDocument.Parse(xmlPrimitiveTypes)),
                WorkspaceNames.UriExtentPrimitiveTypes,
                workspaceLogic.ScopeStorage);
            primitiveExtent.AddAlternativeUri("http://www.omg.org/spec/PrimitiveTypes/20131001");
            primitiveExtent.AddAlternativeUri("http://www.omg.org/spec/UML/20131001/PrimitiveTypes.xmi");
            primitiveExtent.LocalSlimUmlEvaluation = true;

            // Assigns the extents to the datalayer
            workspaceLogic.AddExtent(workspace, umlExtent);
            workspaceLogic.AddExtent(workspace, primitiveExtent);
            if (mode == BootstrapMode.Mof || mode == BootstrapMode.SlimMof)
            {
                workspaceLogic.AddExtent(workspace, mofExtent);
            }

            var bootStrapper = new Bootstrapper(workspaceLogic)
            {
                UmlInfrastructure = umlExtent,
                MofInfrastructure = mofExtent,
                PrimitiveTypesInfrastructure = primitiveExtent
            };

            /*if (isSlim)
            {
                dataLayer.Set(new _UML());
                dataLayer.Set(new _PrimitiveTypes());
            }
            else
            {
                dataLayer.Create<FillTheUML, _UML>();
                dataLayer.Create<FillThePrimitiveTypes, _PrimitiveTypes>();

                if (mode == BootstrapMode.Mof)
                {
                    dataLayer.Create<FillTheMOF, _MOF>();
                }
            }*/

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

            return PerformFullBootstrap(workspaceLogic, dataLayer, mode, filePaths);
        }

        /// <summary>
        /// Defines the file paths for doing the boot strap.
        /// This avoids the cluttering of arguments
        /// </summary>
        public class FilePaths
        {
            /// <summary>
            /// Gets or sets a value indicating whether the resources shall be loaded from embedded resources
            /// </summary>
            public bool LoadFromEmbeddedResources { get; set; }

            public string? PathPrimitive { get; set; }

            public string? PathUml { get; set; }

            public string? PathMof { get; set; }
        }
    }
}