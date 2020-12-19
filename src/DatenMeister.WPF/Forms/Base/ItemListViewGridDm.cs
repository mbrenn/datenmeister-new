using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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

        private List<IElement>? _currentElements;
        
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

                ColumnDefinitions.Add(new GridColumnDefinition {Title = title, Width = 100 + new Random().Next(100)});
            }
            
            RefreshGrid();
        }

        public void SetContent(IReflectiveCollection collection)
        {
            _currentElements = collection.OfType<IElement>().ToList();
            
            RefreshGrid();
        }

        public override int RowCount => _currentElements?.Count ?? 0;

        public override RowInstantiation? GetRowOfContent(int row)
        {
            if (_currentElements == null)
                return null;

            var rowInstantiation = new RowInstantiation {Height = RowHeight};

            var currentElement = _currentElements.ElementAtOrDefault(row);
            if (currentElement == null)
                return null;
            
            var fields = _currentForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field);
            foreach (var field in fields.Cast<IElement>())
            {
                var cell = new CellInstantiation();
                var name = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) ?? string.Empty;
                if (string.IsNullOrEmpty(name))
                {
                    cell.CellElement = new TextBlock {Text = "Not set"};
                }
                else
                {
                    var text = currentElement.getOrDefault<string>(name);
                    
                    cell.CellElement = new TextBlock {Text = text};
                }

                rowInstantiation.Cells.Add(cell);
            }

            return rowInstantiation;
        }
    }
}