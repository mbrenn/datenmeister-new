﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder.Helper
{
    /// <summary>
    /// Creates a view out of the given extent, elements (collection) or element). 
    /// 
    /// </summary>
    public class FormCreator
    {
        /// <summary>
        /// Stores the reference to the view logic which is required to get the views
        /// for the tabs of the extent form
        /// </summary>
        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Stores the factory to create the fields and forms
        /// </summary>
        private readonly IFactory _factory;

        /// <summary>
        /// Stores the information for form and fields including all metaclasses
        /// </summary>
        private readonly _FormAndFields _formAndFields;

        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(FormCreator));

        /// <summary>
        /// Stores the creation mode
        /// </summary>
        [Flags]
        public enum CreationMode
        {
            ByMetaClass = 1,
            ByProperties = 2,
            OnlyPropertiesIfNoMetaClass = 4,
            AddMetaClass = 8,
            All = ByMetaClass | ByProperties | AddMetaClass
        }

        public FormCreator(ViewLogic viewLogic)
        {
            _viewLogic = viewLogic;
            var userExtent = _viewLogic.GetUserViewExtent();
            _factory = new MofFactory(userExtent);
            _formAndFields = userExtent.GetWorkspace().GetFromMetaWorkspace<_FormAndFields>();
        }

        public IElement CreateDetailForm (IObject element)
        {
            var createdForm = _factory.create(_formAndFields.__DetailForm);
            createdForm.set(_FormAndFields._DetailForm.name, "Item");

            AddToForm(createdForm, element, FormCreator.CreationMode.All);

            return createdForm;
        }

        public IElement CreateExtentForm(IUriExtent extent, CreationMode creationMode)
        {
            return CreateExtentForm(extent.elements(), creationMode);
        }

        public IElement CreateExtentForm(IReflectiveCollection elements, CreationMode creationMode)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            var elementsAsObjects = elements.OfType<IObject>().ToList();
            var elementsWithoutMetaClass = elementsAsObjects.Where(x =>
            {
                if (x is IElement element)
                {
                    return element.getMetaClass() == null;
                }

                return true;
            }).ToList();

            var elementsWithMetaClass = elementsAsObjects
                .OfType<IElement>()
                .GroupBy(x => x.getMetaClass());


            var result = _factory.create(_formAndFields.__ExtentForm);
            result.set(_FormAndFields._ExtentForm.name, "Items");

            var tabs = new List<IElement>();

            if (elementsWithoutMetaClass.Any())
            {
                var form = _factory.create(_formAndFields.__ListForm);
                form.set(_FormAndFields._ListForm.name, "Unclassified");

                foreach (var item in elementsWithoutMetaClass)
                {
                    AddToForm(form, item, creationMode);
                }

                tabs.Add(form);
            }

            foreach (var group in elementsWithMetaClass)
            {
                var form = _factory.create(_formAndFields.__ListForm);
                form.set(_FormAndFields._ListForm.name, NamedElementMethods.GetName(group.Key));
                foreach (var item in group)
                {
                    AddToForm(form, item, creationMode);
                }

                tabs.Add(form);
            }

            result.set(_FormAndFields._ExtentForm.tab, tabs);

            return result;
        }

        /// <summary>
        /// Creates the form out of the given element. 
        /// </summary>
        /// <param name="form">Form which will be extended by the given object</param>
        /// <param name="item">Item being used</param>
        /// <param name="creationMode">Creation mode for the form. Whether by metaclass or ByProperties</param>
        public void AddToForm(IElement form, object item, CreationMode creationMode)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // First phase: Get the properties by using the metaclass
            var asElement = item as IElement;
            var metaClass = asElement?.metaclass;
            var wasInMetaClass = false;

            if (creationMode.HasFlag(CreationMode.ByMetaClass)
                && metaClass != null)
            {
                var classifierMethods = ClassifierMethods.
                    GetPropertiesOfClassifier(metaClass).Where(x=> x.isSet("name")).ToList();
                foreach (var property in classifierMethods.OrderBy(x=>x.get("name").ToString()))
                {
                    wasInMetaClass = true;
                    var propertyName = property.get("name").ToString();

                    var isAlreadyIn = form
                        .get<IReflectiveCollection>(_FormAndFields._Form.field)
                        .OfType<IObject>()
                        .Any(x => x.getOrDefault<string>(_FormAndFields._FieldData.name) == propertyName);

                    if (isAlreadyIn)
                    {
                        continue;
                    }

                    var column = GetFieldForProperty(property);

                    form.get<IReflectiveCollection>(_FormAndFields._Form.field).add(column);
                }
            }

            // Second phase: Get properties by the object iself
            // This item does not have a metaclass and also no properties, so we try to find them by using the item
            var itemAsAllProperties = item as IObjectAllProperties;
            var itemAsObject = item as IObject;

            var isByProperties =
                creationMode.HasFlag(CreationMode.ByProperties);
            var isOnlyPropertiesIfNoMetaClass =
                creationMode.HasFlag(CreationMode.OnlyPropertiesIfNoMetaClass);

            if ((isByProperties
                 || (isOnlyPropertiesIfNoMetaClass && !wasInMetaClass))
                && itemAsAllProperties != null)
            {
                var properties = itemAsAllProperties.getPropertiesBeingSet();

                foreach (var property in properties)
                {
                    var column = form
                        .get<IReflectiveCollection>(_FormAndFields._Form.field)
                        .OfType<IObject>()
                        .FirstOrDefault(x => x.getOrDefault<string>(_FormAndFields._FieldData.name) == property);
                    if (column == null)
                    {
                        // Check by content, which type of field shall be created
                        var propertyValue = itemAsObject?.GetOrDefault(property);
                        var propertyType = propertyValue?.GetType();

                        if (DotNetHelper.IsPrimitiveType(propertyType))
                        {
                            column = _factory.create(_formAndFields.__TextFieldData);
                        }
                        else
                        {
                            if (DotNetHelper.IsEnumeration(propertyType))
                            {
                                column = _factory.create(_formAndFields.__SubElementFieldData);
                            }
                            else
                            {
                                column = _factory.create(_formAndFields.__ReferenceFieldData);
                                column.set(_FormAndFields._ReferenceFieldData.isSelectionInline, false);
                            }
                        }

                        column.set(_FormAndFields._TextFieldData.name, property);
                        column.set(_FormAndFields._TextFieldData.title, property);

                        form.get<IReflectiveCollection>(_FormAndFields._Form.field).add(column);
                    }

                    var value = ((IObject) item).get(property);
                    column.set(_FormAndFields._FieldData.isEnumeration,
                        column.getOrDefault<bool>(_FormAndFields._FieldData.isEnumeration) | value is IEnumerable && !(value is string));
                }

            }

            // Third phase: Add metaclass
            var isMetaClass = creationMode.HasFlag(CreationMode.AddMetaClass);
            if (isMetaClass && !form
                    .get<IReflectiveCollection>(_FormAndFields._Form.field)
                    .OfType<IElement>()
                    .Any(x => x.getMetaClass().@equals(_formAndFields.__MetaClassElementFieldData)))
            {
                
                form.get<IReflectiveCollection>(_FormAndFields._Form.field).add(new MetaClassElementFieldData());
            }
        }

        /// <summary>
        /// Gets the field data, depending upon the given property
        /// </summary>
        /// <param name="property">Property which is requesting a field</param>
        /// <returns>The field data</returns>
        public IElement GetFieldForProperty(IElement property)
        {
            var propertyType = PropertyHelper.GetPropertyType(property);

            if (propertyType != null)
            {
                // Check, if correct property
                Logger.Trace(propertyType.ToString());
            }

            var propertyName = property.get<string>("name");

            if (propertyType == null)
            {
                var columnNoPropertyType = _factory.create(_formAndFields.__TextFieldData);
                columnNoPropertyType.set(_FormAndFields._TextFieldData.name, propertyName);
                columnNoPropertyType.set(_FormAndFields._TextFieldData.title, propertyName);

                return columnNoPropertyType;
            }

            // Check, if field property is an enumeration
            var uriResolver = propertyType.GetUriResolver();
            var stringType = uriResolver.Resolve(WorkspaceNames.StandardPrimitiveTypeNamespace + "#String",
                ResolveType.Default);
            var integerType = uriResolver.Resolve(WorkspaceNames.StandardPrimitiveTypeNamespace + "#Integer",
                ResolveType.Default);
            var booleanType = uriResolver.Resolve(WorkspaceNames.StandardPrimitiveTypeNamespace + "#Boolean",
                ResolveType.Default);
            var realType = uriResolver.Resolve(WorkspaceNames.StandardPrimitiveTypeNamespace + "#Real",
                ResolveType.Default);

            // Checks, if the property is an enumeration. 
            if (propertyType.metaclass != null)
            {
                var umlTypeExtent = propertyType.metaclass.GetUriExtentOf();
                var uml = umlTypeExtent.GetWorkspace().Get<_UML>();

                if (propertyType.metaclass.@equals(uml.SimpleClassifiers.__Enumeration))
                {
                    var comboBox = _factory.create(_formAndFields.__DropDownFieldData);
                    comboBox.set(_FormAndFields._DropDownFieldData.name, propertyName);
                    comboBox.set(_FormAndFields._DropDownFieldData.title, propertyName);

                    var values = EnumerationHelper.GetEnumValues(propertyType);
                    comboBox.set(_FormAndFields._DropDownFieldData.values, values.Select(x =>
                    {
                        var data = _factory.create(_formAndFields.__ValuePair);
                        data.set(_FormAndFields._ValuePair.name, x);
                        data.set(_FormAndFields._ValuePair.value, x);
                        return data;
                    }).ToList());
                    return comboBox;
                }

                if (propertyType.@equals(booleanType))
                {
                    var checkbox = _factory.create(_formAndFields.__CheckboxFieldData);
                    checkbox.set(_FormAndFields._CheckboxFieldData.name, propertyName);
                    checkbox.set(_FormAndFields._CheckboxFieldData.title, propertyName);
                    return checkbox;
                }

                if (!propertyType.@equals(stringType) && !propertyType.@equals(integerType) && !propertyType.@equals(realType))
                {
                    if (property.getOrDefault<int>(_UML._CommonStructure._MultiplicityElement.upper) > 1)
                    {

                        var elements = _factory.create(_formAndFields.__SubElementFieldData);
                        elements.set(_FormAndFields._SubElementFieldData.name, propertyName);
                        elements.set(_FormAndFields._SubElementFieldData.title, propertyName);
                        elements.set(_FormAndFields._SubElementFieldData.defaultTypesForNewElements,
                            new List<IElement> {propertyType});

                        return elements;
                    }
                    else
                    {

                        var reference = _factory.create(_formAndFields.__ReferenceFieldData);
                        reference.set(_FormAndFields._ReferenceFieldData.name, propertyName);
                        reference.set(_FormAndFields._ReferenceFieldData.title, propertyName);

                        return reference;
                    }
                }
            }

            // Checks, if the property is a field data
            var column = _factory.create(_formAndFields.__TextFieldData);
            column.set(_FormAndFields._TextFieldData.name, propertyName);
            column.set(_FormAndFields._TextFieldData.title, propertyName);

            // If propertyType is an integer, the field can be smaller
            if (propertyType.@equals(integerType))
            {
                column.set(_FormAndFields._TextFieldData.width, 10);
            }

            return column;
        }

        /// <summary>
        /// Creates the extent form for a specific object which is shown in the item explorer view
        /// </summary>
        /// <param name="element">Element which shall be shown</param>
        /// <param name="extent">Extent containing the element</param>
        /// <param name="creationMode">The creation mode for autogeneration of the fields</param>
        /// <returns>Created Extent form as MofObject</returns>
        public IElement CreateExtentFormForObject(IObject element, IExtent extent, CreationMode creationMode)
        {
            throw new NotImplementedException();
        }
    }
}