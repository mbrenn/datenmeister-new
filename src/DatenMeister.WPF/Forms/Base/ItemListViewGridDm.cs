using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base.GridControl;

namespace DatenMeister.WPF.Forms.Base
{
    public class ItemListViewGridDm : ItemListViewGrid
    {
        /// <summary>
        /// Stores the current form
        /// </summary>
        private IElement? _currentForm;

        private IReflectiveCollection? _currentElements;
        
        /// <summary>
        /// Sets the form and creates the depending columns out of the form definition
        /// </summary>
        /// <param name="formDefinition">Definition of the form </param>
        public void SetForm(IElement formDefinition)
        {
            _currentForm = formDefinition;
            ColumnDefinitions.Clear();
            
            var fields = _currentForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field);
            foreach (var field in fields.Cast<IElement>())
            {
                var fieldMetaClass = field.getMetaClass();
                
                var title = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.title) ?? string.Empty;

                if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
                {
                    title = "Metaclass";
                }

                ColumnDefinitions.Add(new GridColumnDefinition {Title = title, Width = 100});
            }
            
            RefreshGrid();
        }

        public void SetContent(IReflectiveCollection collection)
        {
            _currentElements = collection;
        }
        
    }
}