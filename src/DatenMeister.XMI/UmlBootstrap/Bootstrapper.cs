using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;

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

        public Dictionary<string, IElement> MofClasses { get; }

        public Dictionary<string, IElement> UmlClasses { get; }

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
            MofClasses = new Dictionary<string, IElement>();
            UmlClasses = new Dictionary<string, IElement>();
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

            // First, find the all classes of the uml namespace...            
            var descendents = AllDescendentsQuery.getDescendents(UmlInfrastructure);
            var typeProperty = (Namespaces.Xmi + "type").ToString();
            var idProperty = (Namespaces.Xmi + "id").ToString();

            var idToElementCache = new Dictionary<string, IElement>();
            var allClasses = descendents
                .Where(x => x.isSet(typeProperty) && x.get(typeProperty).ToString() == "uml:Class");

            foreach (var classInstance in allClasses.Cast<IElement>())
            {
                var name = classInstance.get("name").ToString();
                UmlClasses[name] = classInstance;

                if (classInstance.isSet(idProperty))
                {
                    var id = classInstance.get(idProperty).ToString();
                    if (idToElementCache.ContainsKey(id))
                    {
                        throw new InvalidOperationException($"ID '{id}' is duplicate");
                    }
                    idToElementCache[id] = classInstance;
                    Debug.WriteLine($"ID: {id}");
                }
            }

            // Second step: Find all classes in the Mof namespace
            descendents = AllDescendentsQuery.getDescendents(MofInfrastructure);
            allClasses = descendents
                .Where(x => x.isSet(typeProperty) && x.get(typeProperty).ToString() == "uml:Class");
            foreach (var classInstance in allClasses.Cast<IElement>())
            {
                var name = classInstance.get("name").ToString();
                MofClasses[name] = classInstance;
            }

            // Ok, finally, set the metaclasses on base of the found classes
            allClasses =
                AllDescendentsQuery.getDescendents(MofInfrastructure)
                    .Union(AllDescendentsQuery.getDescendents(UmlInfrastructure))
                    .Union(AllDescendentsQuery.getDescendents(PrimitiveInfrastructure))
                    .ToList();
                
            foreach (var classInstance in allClasses.Where(x => x.isSet(typeProperty)))
            {
                var name = classInstance.get(typeProperty).ToString();
                if (name.StartsWith("uml:"))
                {
                    name = name.Substring(4);
                    ((IElementSetMetaClass) classInstance).setMetaClass(UmlClasses[name]);
                }
                else if (name.StartsWith("mofext:"))
                {
                    name = name.Substring(7);
                    ((IElementSetMetaClass) classInstance).setMetaClass(MofClasses[name]);
                }
                else
                {
                    throw new InvalidOperationException($"Found unknown class: {name}");
                }

                // We strip out the property and id information. 
                // It is not really required 
                classInstance.unset(typeProperty);
            }

            // Unsets the id property, it is already stored in
            foreach (var classInstance in allClasses)
            {
                classInstance.unset(idProperty);
            }

            // Now we handle the generalization information. 

        }

        /// <summary>
        ///     Performs a full bootstrap by reading in the uml class
        /// </summary>
        /// <param name="pathUml">Path of XMI file containing the UML file to be used</param>
        /// <param name="pathMof">Path of XMI file containing the MOF specification being</param>
        /// <returns>The instance of the bootstrapper</returns>
        public static Bootstrapper PerformFullBootstrap(string pathPrimitive, string pathUml, string pathMof)
        {
            var factory = new MofFactory();
            var umlExtent = new MofUriExtent("datenmeister:///uml");
            var mofExtent = new MofUriExtent("datenmeister:///mof");
            var primitiveExtent = new MofUriExtent("datenmeister:///prototypes");
            var loader = new SimpleLoader(factory);
            loader.Load(primitiveExtent, pathPrimitive);
            loader.Load(umlExtent, pathUml);
            loader.Load(mofExtent, pathMof);

            var bootStrapper = new Bootstrapper(primitiveExtent, umlExtent, mofExtent);
            bootStrapper.Strap();
            return bootStrapper;
        }
    }
}