using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.XMI.UmlBootstrap
{
    /// <summary>
    /// Performs the bootstrap on the given object 
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Stores a vlaue indicating whether the run was already performed.
        /// If yes, the method is locked
        /// </summary>
        private bool wasRun = false;

        public Dictionary<string, IElement> mofClasses
        {
            get;
            private set;
        }

        public Dictionary<string, IElement> umlClasses
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent MofInfrastructure
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent UmlInfrastructure
        {
            get;
            private set;
        }

        public Bootstrapper(IUriExtent umlInfrastructure, IUriExtent mofInfrastructure)
        {
            if (umlInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(umlInfrastructure));
            }
            if (mofInfrastructure == null)
            {
                throw new ArgumentNullException(nameof(mofInfrastructure));
            }

            UmlInfrastructure = umlInfrastructure;
            MofInfrastructure = mofInfrastructure;
            mofClasses = new Dictionary<string, IElement>();
            umlClasses = new Dictionary<string, IElement>();
        }

        /// <summary>
        /// Performs the bootstrap
        /// </summary>
        public void Strap()
        {
            if (wasRun)
            {
                throw new InvalidOperationException("Bootstrapper was already run. Create a new object for your run");
            }

            wasRun = true;
            
            // First, find the all classes of the uml namespace...            
            var descendents = AllDescendentsQuery.getDescendents(UmlInfrastructure);
            var typeProperty = (Namespaces.Xmi + "type").ToString();
            var allClasses = descendents
                .Where(x => x.isSet(typeProperty) && x.get(typeProperty).ToString() == "uml:Class");
            foreach (var classInstance in allClasses.Cast<IElement>())
            {
                var name = classInstance.get("name").ToString();
                umlClasses[name] = classInstance;
            }

            // Second step: Go throught the MOF
            descendents = AllDescendentsQuery.getDescendents(MofInfrastructure);
            allClasses = descendents
                .Where(x => x.isSet(typeProperty) && x.get(typeProperty).ToString() == "uml:Class");
            foreach (var classInstance in allClasses.Cast<IElement>())
            {
                var name = classInstance.get("name").ToString();
                mofClasses[name] = classInstance;
            }

            allClasses = descendents
                .Where(x => x.isSet(typeProperty));
            foreach (var classInstance in allClasses)
            {
                var name = classInstance.get(typeProperty).ToString();
                if (name.StartsWith("uml:"))
                {
                    name = name.Substring(4);
                    (classInstance as IElementExt).setMetaClass(umlClasses[name]);
                }
                else if (name.StartsWith("mofext:"))
                {
                    name = name.Substring(7);
                    (classInstance as IElementExt).setMetaClass(mofClasses[name]);
                }
                else
                {
                    throw new InvalidOperationException($"Found unknown class: {name}");
                }
            }
        }

        /// <summary>
        /// Performs a full bootstrap by reading in the uml class
        /// </summary>
        /// <param name="path">Path to be used</param>
        /// <returns>The instance of the bootstrapper</returns>
        public static Bootstrapper PerformFullBootstrap(string pathUml, string pathMof)
        {
            var factory = new MofFactory();
            var umlExtent = new MofUriExtent("datenmeister:///uml");
            var mofExtent = new MofUriExtent("datenmeister:///mof");
            var loader = new SimpleLoader(factory);
            loader.Load(umlExtent, pathUml);
            loader.Load(mofExtent, pathMof);

            var bootStrapper = new Bootstrapper(umlExtent, mofExtent);
            bootStrapper.Strap();
            return bootStrapper;
        }
    }
}
