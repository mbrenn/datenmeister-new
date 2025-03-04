﻿using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Core.Runtime.Proxies
{
    /// <summary>
    /// Defines a reflective sequence that is just a place holder for a list of objects which can be group
    /// used within a virtual reflective sequence.
    /// The elements that are included into the property are not transformed.
    /// </summary>
    public class PureReflectiveSequence : IReflectiveSequence
    {
        /// <summary>
        /// Initializes a new instance of the PureReflectiveSequence
        /// </summary>
        public PureReflectiveSequence()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the elements
        /// </summary>
        /// <param name="elements">Elements to be added</param>
        public PureReflectiveSequence(IEnumerable<object> elements)
        {
            foreach (var element in elements)
            {
                add(element);
            }            
        }
        /// <summary>
        /// Stores the elements
        /// </summary>
        private readonly List<object> _elements = new();

        public IEnumerator<object> GetEnumerator()
        {
            lock (_elements)
            {
                return _elements.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_elements)
            {
                return _elements.GetEnumerator();
            }
        }

        public bool add(object value)
        {
            lock (_elements)
            {
                _elements.Add(value);
            }

            return true;
        }

        public bool addAll(IReflectiveSequence values)
        {
            var result = false;
            lock (_elements)
            {
                foreach (var value in values)
                {
                    if (value == null) continue;

                    result |= add(value);
                }
            }

            return result;
        }

        public void clear()
        {
            lock (_elements)
            {
                _elements.Clear();
            }
        }

        public bool remove(object? value)
        {
            lock (_elements)
            {
                if (value == null) return false;

                return _elements.Remove(value);
            }
        }

        public int size()
        {
            lock (_elements)
            {
                return _elements.Count;
            }
        }

        public void add(int index, object value)
        {
            lock (_elements)
            {
                _elements.Insert(index, value);
            }
        }

        public object get(int index)
        {
            lock (_elements)
            {
                return _elements[index];
            }
        }

        public void remove(int index)
        {
            lock (_elements)
            {
                _elements.RemoveAt(index);
            }
        }

        public object set(int index, object value)
        {
            lock (_elements)
            {
                var result = _elements[index];
                _elements[index] = value;
                return result;
            }
        }
    }
}