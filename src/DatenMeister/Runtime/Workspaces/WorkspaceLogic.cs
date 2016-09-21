using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.Helper;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

namespace DatenMeister.Core
{
    /// <summary>
    /// The logic defines the relationships between the layers and the metalayers. 
    /// </summary>
    public class WorkspaceLogic : IWorkspaceLogic
    {
        private readonly WorkspaceData _fileData;

        public WorkspaceLogic(WorkspaceData fileData)
        {
            _fileData = fileData;
            if (_fileData.Default == null)
            {
                throw new InvalidOperationException("DataLayer.Default was not set");
            }
        }

        public void SetDefaultDatalayer(Workspace layer)
        {
            lock (_fileData)
            {
                _fileData.Default = layer;
            }
        }

        public void SetRelationShip(Workspace dataLayer, Workspace metaDataLayer)
        {
            lock (_fileData)
            {
                dataLayer.MetaWorkspace = metaDataLayer;
            }
        }

        public void AssignToDataLayer(IExtent extent, Workspace dataLayer)
        {
            lock (_fileData)
            {
                dataLayer.extent.Add(extent);
            }
        }

        public Workspace GetDataLayerOfExtent(IExtent extent)
        {
            lock (_fileData)
            {
                return _fileData.Workspaces.Single(x => x.extent.Contains(extent));
            }
        }

        public IEnumerable<IExtent> GetExtentsOfDatalayer(Workspace layer)
        {
            lock (_fileData)
            {
                return layer.extent;
            }
        }

        public Workspace GetDataLayerOfObject(IObject value)
        {
            // If the object is contained by another object, query the contained objects 
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetDataLayerOfObject(parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IObjectKnowsExtent;
            if (objectKnowsExtent != null)
            {
                var found = objectKnowsExtent.Extents.FirstOrDefault();
                return found == null
                    ? _fileData.Default
                    : GetDataLayerOfExtent(found);
            }

            // Otherwise check it by the dataextent
            lock (_fileData)
            {
                return _fileData.Workspaces.FirstOrDefault(x => x.extent.Cast<IUriExtent>().WithElement(value) != null);
            }
        }

        public Workspace GetMetaLayerFor(Workspace data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            lock (_fileData)
            {
                return data.MetaWorkspace;
            }
        }

        public IEnumerable<IUriExtent> GetExtentsForDatalayer(Workspace dataLayer)
        {
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            lock (_fileData)
            {
                return dataLayer.extent
                    .Select(x => x as IUriExtent)
                    .Where(x => x != null)
                    .ToList();
            }
        }

        public TFilledType Create<TFiller, TFilledType>(Workspace layer)
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new()
        {
            if (layer == null) throw new ArgumentNullException(nameof(layer));

            lock (_fileData)
            {
                var layerAsObject = layer as Workspace;
                VerifyThatNotNull(layerAsObject);

                var filledType = Get<TFilledType>(layerAsObject);
                if (filledType != null)
                {
                    return filledType;
                }

                // Not found, we need to fill it on our own... Congratulation
                var filler = new TFiller();
                filledType = new TFilledType();

                // Go through all extents of this datalayer
                foreach (var extent in GetExtentsOfDatalayer(layer))
                {
                    filler.Fill(extent.elements(), filledType);
                }

                // Adds it to the database
                layerAsObject.FilledTypeCache.Add(filledType);
                return filledType;
            }
        }

        public TFilledType Get<TFilledType>(Workspace layer)
            where TFilledType : class, new()
        {
            if (layer == null) throw new ArgumentNullException(nameof(layer));

            lock (_fileData)
            {
                var layerAsObject = layer as Workspace;
                VerifyThatNotNull(layerAsObject);
                    
                // Looks into the cache for the filledtypes
                foreach (var value in layerAsObject.FilledTypeCache)
                {
                    if (value is TFilledType)
                    {
                        return value as TFilledType;
                    }
                }

                return null;
            }
        }

        public void Set<TFilledType>(Workspace layer, TFilledType value) where TFilledType : class, new()
        {
            lock (_fileData)
            {
                var layerAsObject = layer as Workspace;
                VerifyThatNotNull(layerAsObject);

                layerAsObject.FilledTypeCache.Add(value);
            }
        }

        /// <summary>
        /// Gets the datalayer by name.
        /// The datalayer will only be returned, if there is a relationship
        /// </summary>
        /// <param name="id">Name of the datalayer</param>
        /// <returns>Found datalayer or null</returns>
        public Workspace GetById(string id)
        {
            lock (_fileData)
            {
                return _fileData.Workspaces.FirstOrDefault(x => x.id == id);
            }
        }

        private static void VerifyThatNotNull(Workspace layerAsObject)
        {
            if (layerAsObject == null)
            {
                throw new ArgumentException($"{nameof(layerAsObject)} is not of type DataLayer", nameof(layerAsObject));
            }
        }

        public void ClearCache(Workspace layer)
        {
            lock (_fileData)
            {
                var layerAsObject = layer as Workspace;
                layerAsObject.FilledTypeCache.Clear();
            }
        }

        public static Workspaces InitDefault(out WorkspaceData workspace)
        {
            var workspaces = new Workspaces
            {
                Data = new Workspace("Data"),
                Mof = new Workspace("Mof"),
                Uml = new Workspace("Uml"),
                Types = new Workspace("Types")
            };

            workspace = new WorkspaceData {Default = workspaces.Data};
            var logic = new WorkspaceLogic(workspace);
            workspaces.SetRelationsForDefaultDataLayers(logic);
            
            workspace.Workspaces.Add(workspaces.Mof);
            workspace.Workspaces.Add(workspaces.Uml);
            workspace.Workspaces.Add(workspaces.Types);
            workspace.Workspaces.Add(workspaces.Data);
            return workspaces;
        }
    }
}