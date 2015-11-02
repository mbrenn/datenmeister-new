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

        public IObject theClassObject
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the extent for the uml infrastructure
        /// </summary>
        public IUriExtent UmlInfrastructure
        {
            get;
            private set;
        }

        public Bootstrapper(IUriExtent umlInfrastructure)
        {
            UmlInfrastructure = umlInfrastructure;
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

            // First, find the instance of the class itself...
            // Look by Id
            var descendents = AllDescendentsQuery.getDescendents(UmlInfrastructure);
            var idProperty = (Namespaces.Xmi + "id").ToString();
            theClassObject = descendents
                .Where (x=>x.isSet(idProperty))
                .Where (x=>x.get(idProperty).ToString() == "Class")
                .Single();
        }

        /// <summary>
        /// Performs a full bootstrap by reading in the uml class
        /// </summary>
        /// <param name="path">Path to be used</param>
        /// <returns>The instance of the bootstrapper</returns>
        public static Bootstrapper PerformFullBootstrap(string path)
        {
            var factory = new MofFactory();
            var extent = new MofUriExtent("datenmeister:///uml");
            var loader = new SimpleLoader(extent, factory);
            loader.Load(path);

            var bootStrapper = new Bootstrapper(extent);
            bootStrapper.Strap();
            return bootStrapper;
        }
    }
}
