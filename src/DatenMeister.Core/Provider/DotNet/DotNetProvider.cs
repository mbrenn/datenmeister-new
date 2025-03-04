﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation.DotNet;

namespace DatenMeister.Core.Provider.DotNet
{
    /// <summary>
    /// Implements the provider for the DotNet objects
    /// </summary>
    public class DotNetProvider : IProvider
    {
        private readonly List<DotNetProviderObject> _elements = new();

        /// <summary>
        /// Stores the capabilities of the provider
        /// </summary>
        /// <returns></returns>
        private readonly ProviderCapability _providerCapability = new()
        {
            IsTemporaryStorage = true
        };

        private readonly object _syncObject = new();

        /// <summary>
        /// Initializes a new instance of the DotNetExtent class
        /// </summary>
        /// <param name="typeLookup">Looked up type</param>
        public DotNetProvider(IDotNetTypeLookup typeLookup)
        {
            TypeLookup = typeLookup;
        }

        /// <summary>
        ///     Gets the type lookup
        /// </summary>
        internal IDotNetTypeLookup TypeLookup { get; }

        /// <inheritdoc />
        public IProviderObject CreateElement(string? metaClassUri)
        {
            lock (_syncObject)
            {
                if (metaClassUri == null || string.IsNullOrEmpty(metaClassUri))
                {
                    throw new InvalidOperationException(".Net-Provider requires a meta class");
                }

                var type = TypeLookup.ToType(metaClassUri);
                if (type == null)
                {
                    throw new InvalidOperationException("No metaclass with uri '" + metaClassUri + "' is known");
                }

                var result = Activator.CreateInstance(type) ??
                             throw new InvalidOperationException("Activation did not have success");
                return new DotNetProviderObject(this, result, metaClassUri);
            }
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject? valueAsObject, int index = -1)
        {
            lock (_syncObject)
            {
                if (!(valueAsObject is DotNetProviderObject providerObject))
                {
                    throw new ArgumentNullException(nameof(providerObject));
                }

                if (index == -1)
                {
                    _elements.Add(providerObject);
                }
                else
                {
                    _elements.Insert(index, providerObject);
                }
            }
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            lock (_syncObject)
            {
                return _elements.RemoveAll(x => x.Id == id) > 0;
            }
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            lock (_syncObject)
            {
                _elements.Clear();
            }
        }

        /// <inheritdoc />
        public IProviderObject? Get(string? id)
        {
            lock (_syncObject)
            {
                return _elements.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            foreach (var element in _elements)
            {
                yield return element;
            }
        }

        /// <summary>
        /// Gets the capabilities of the provider
        /// </summary>
        /// <returns>Capabilities of the provider</returns>
        public ProviderCapability GetCapabilities() => _providerCapability;
    }
}