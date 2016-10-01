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
        public static void Convert(HierarchyByParentSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            Debug.Assert(settings.Sequence != null);
            Debug.Assert(settings.TargetFactory != null);
            Debug.Assert(settings.TargetSequence != null);

            var copier = new ObjectCopier(settings.TargetFactory);

            // Stores the lists
            var lists = new Dictionary<object, List<object>>();

            // First: List items by id
            var values = new Dictionary<object, IObject>();
            foreach (var element  in settings.Sequence.Select(x => x as IObject).Where(x => x != null))
            {
                if (element.isSet(settings.OldIdColumn))
                {
                    values[element.get(settings.OldIdColumn)] = copier.Copy(element);
                }
            }

            // Now: Do the adding into temporary lists and copy 
            foreach (var element in settings.Sequence.Select(x => x as IObject).Where(x => x != null))
            {
                var id = element.getOrDefault(settings.OldIdColumn);

                if (id != null && element.isSet(settings.OldParentColumn))
                {
                    var parentId = element.get(settings.OldParentColumn);
                    if (parentId != null)
                    {
                        IObject found;
                        if (values.TryGetValue(parentId, out found))
                        {
                            List<object> foundList;
                            if (!lists.TryGetValue(parentId, out foundList))
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

            // Copies all the elements
            if (settings.TargetSequence != null)
            {
                // Copy only the one, which don't have a parent
                foreach (var element in settings.Sequence. Select (x=> x as IElement).Where (x=> x != null))
                {
                    var id = element.getOrDefault(settings.OldIdColumn);
                    if (id != null && element.isSet(settings.OldParentColumn))
                    {
                        var parentId = element.get(settings.OldParentColumn);
                        if (parentId == null || !values.ContainsKey(parentId))
                        {
                            targetExtent.add(values[id]);
                        }
                    }
                }
            }

            // Adds the elements
            foreach (var element in targetExtent.Select(x => x as IObject).Where(x => x != null))
            {
                if (element.isSet(settings.OldIdColumn))
                {
                    var key = element.get(settings.OldIdColumn);
                    List<object> list;
                    if (lists.TryGetValue(key, out list))
                    {
                        values[key].set(settings.NewChildColumn, list);
                    }
                }
            }
        }
    }
}