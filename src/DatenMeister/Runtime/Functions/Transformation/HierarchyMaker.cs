using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Runtime.Functions.Transformation
{
    public static class HierarchyMaker
    {
        public static void Convert(HierarchyMakerSettings settings)
        {
            var copier = new ObjectCopier(settings.TargetFactory);
            // Stores the lists
            var lists = new Dictionary<object, List<object>>();

            // First: List items by id
            var values = new Dictionary<object, IObject>();
            foreach (var element  in settings.Sequence.Select(x => x as IObject).Where(x => x != null))
            {
                if (element.isSet(settings.OldIdColumn))
                {
                    values[element.get(settings.OldIdColumn)] = element;
                }
            }

            // Now: Do the adding
            foreach (var element in settings.Sequence.Select(x => x as IObject).Where(x => x != null))
            {
                if (element.isSet(settings.OldParentColumn))
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

                            foundList.Add(copier.Copy(element));
                        }
                    }
                }
            }

            // Ok, finally add them
            var targetExtent = settings.TargetSequence;

            if (settings.TargetSequence != null)
            {
                // Copy only the one, which don't have a parent
                foreach (var element in settings.Sequence. Select (x=> x as IElement).Where (x=> x != null))
                {
                    if (element.isSet(settings.OldIdColumn))
                    {
                        var parentId = element.get(settings.OldParentColumn);
                        if (parentId == null || !values.ContainsKey(parentId))
                        {
                            targetExtent.add(copier.Copy(element));
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
                targetExtent = settings.Sequence;
            }

            foreach (var element in targetExtent.Select(x => x as IObject).Where(x => x != null))
            {
                if (element.isSet(settings.OldIdColumn))
                {
                    var key = element.get(settings.OldIdColumn);
                    List<object> list;
                    if (lists.TryGetValue(key, out list))
                    {
                        element.set(settings.NewChildColumn, list);
                    }
                }
            }
        }
    }
}