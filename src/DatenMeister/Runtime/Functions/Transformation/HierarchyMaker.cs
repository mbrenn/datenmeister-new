using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Runtime.Functions.Transformation
{
    public static class HierarchyMaker
    {
        /// <summary>
        /// Converts a linear reflective collection to a hierarchical reflective collection
        /// Each object in the reflective collection has a property that references to its parent
        /// </summary>
        /// <param name="settings">Settings being used</param>
        public static void Convert(HierarchyByParentSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (settings.Sequence == null) throw new InvalidOperationException("settings.Sequence == null");
            if (settings.TargetFactory == null) throw new InvalidOperationException("target.TargetFactory == null");
            if (settings.TargetSequence == null) throw new InvalidOperationException("target.TargetSequence == null");

            // Stores the lists
            var lists = new Dictionary<object, List<object>>();

            var values = CopyElements(settings);

            // Now: Do the adding of the children into a temporary lists and copy
            foreach (var element in settings.Sequence.Select(x => x as IObject).Where(x => x != null))
            {
                var id = element.getOrDefault<object>(settings.IdColumn);

                if (id != null && element.isSet(settings.OldParentColumn))
                {
                    var parentId = element.get(settings.OldParentColumn);
                    if (parentId != null)
                    {
                        if (values.TryGetValue(parentId, out _))
                        {
                            if (!lists.TryGetValue(parentId, out var foundList))
                            {
                                foundList = new List<object>();
                                lists[parentId] = foundList;
                            }

                            foundList.Add(values[id]);
                        }
                    }
                }
            }

            // Ok, finally add them
            var targetExtent = settings.TargetSequence;

            // Copies all the elements which do not have parent into the generic one
            foreach (var element in settings.Sequence.Select(x => x as IElement).Where(x => x != null))
            {
                var id = element.getOrDefault<object>(settings.IdColumn);
                if (id != null && element.isSet(settings.OldParentColumn))
                {
                    var parentId = element.get(settings.OldParentColumn);
                    if (parentId == null || !values.ContainsKey(parentId))
                    {
                        targetExtent.add(values[id]);
                    }
                }
            }

            // Adds the children to the target elements
            foreach (var element in targetExtent.Select(x => x as IObject).Where(x => x != null))
            {
                if (element.isSet(settings.IdColumn))
                {
                    var key = element.get(settings.IdColumn);
                    List<object> list;
                    if (lists.TryGetValue(key, out list))
                    {
                        values[key].set(settings.NewChildColumn, list);
                    }
                }
            }
        }

        public static void Convert(HierarchyByChildrenSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            Debug.Assert(settings.Sequence != null);
            Debug.Assert(settings.TargetFactory != null);
            Debug.Assert(settings.TargetSequence != null);

            // Copies the elements
            var copiedElements = CopyElements(settings);
            var isChild = new HashSet<IObject>();

            // Ok, add the children
            foreach (var pair in copiedElements)
            {
                var element = pair.Value;
                var childrenId = element.getOrDefault<string>(settings.OldChildrenColumn)
                    ?.Split(new[] {settings.ChildIdSeparator}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());

                if (childrenId == null)
                {
                    continue;
                }

                var list = new List<IObject>();
                foreach (var childId in childrenId)
                {
                    if (copiedElements.ContainsKey(childId))
                    {
                        var found = copiedElements[childId];
                        list.Add(found);
                        isChild.Add(found);
                    }
                }

                element.set(settings.NewChildColumn, list);
            }

            // Ok, add the elements, which are not child to final sequence
            foreach (var pair in copiedElements)
            {
                var element = pair.Value;
                if (!isChild.Contains(element))
                {
                    settings.TargetSequence.add(element);
                }
            }
        }

        /// <summary>
        /// Copies the elements into a dictionary where
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static Dictionary<object, IObject> CopyElements(HierarchyMakerBase settings)
        {
            // First: List items by id
            var copier = new ObjectCopier(settings.TargetFactory);
            var values = new Dictionary<object, IObject>();
            foreach (var element in settings.Sequence.OnlyObjects())
            {
                if (element.isSet(settings.IdColumn))
                {
                    values[element.get(settings.IdColumn)] = copier.Copy(element);
                }
            }

            return values;
        }
    }
}