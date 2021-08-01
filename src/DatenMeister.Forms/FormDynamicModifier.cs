using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Extensions;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Forms
{
    public static class FormDynamicModifier
    {
        /// <summary>
        /// Modifies the form dependent on the selected item and the form definition
        /// </summary>
        /// <param name="form">The selected form</param>
        /// <param name="selectedObject">The selected object to be used</param>
        public static void ModifyFormDependingOnObject(IObject? form, IObject selectedObject)
        {
            if (form == null) return;
            
            var tabs = form.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._ExtentForm.tab);
            if (tabs == null)
            {
                // No tabs, nothing to do
                return;
            }

            IReflectiveCollection? collection = null;
            
            // Now go through the tabs of the form
            var n = 0;
            foreach (var tab in tabs.OfType<IElement>().ToList())
            {
                // Confirms the type of the listforms
                if (tab.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__ListForm) == true)
                {
                    var isDynamicList = tab.getOrDefault<bool>(_DatenMeister._Forms._ListForm.duplicatePerType);
                    
                    // Only when it is a dynamic list
                    if (isDynamicList)
                    {
                        collection ??= ListFormCollectionCreator.GetCollection(tab, selectedObject);

                        var foundOne = false;
                        // Now duplicate the tab
                        var groups = 
                            ByMetaClassGrouper
                                .Group(collection)
                                // Descending order since the tabs are inserted at a certain position
                                .OrderByDescending(x=>NamedElementMethods.GetName(x.MetaClass))
                                .ToList();
                        foreach (var group in groups)
                        {
                            foundOne = true;
                            var title = tab.getOrDefault<string>(_DatenMeister._Forms._ListForm.title);
                            
                            var copiesTab = ObjectCopier.Copy(new MofFactory(tab), tab);
                            if (group.MetaClass == null)
                            {
                                copiesTab.set(_DatenMeister._Forms._ListForm.noItemsWithMetaClass, true);
                                copiesTab.set(_DatenMeister._Forms._ListForm.name, $"{title} - Unspecified");
                            }
                            else
                            {
                                copiesTab.set(_DatenMeister._Forms._ListForm.metaClass, group.MetaClass);
                                copiesTab.set(
                                    _DatenMeister._Forms._ListForm.title,
                                    $"{title} - {NamedElementMethods.GetName(group.MetaClass)}");
                            }

                            // That is the reason for the descending order
                            tabs.add(n + 1, copiesTab);
                        }

                        if (foundOne)
                        {
                            tabs.remove(n);
                        }
                    }
                }

                n++;
            }
                    
        }
    }
}