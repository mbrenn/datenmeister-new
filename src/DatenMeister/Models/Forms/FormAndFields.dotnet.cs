using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
// Created by $DatenMeister.SourcecodeGenerator.DotNetIntegrationGenerator
// ReSharper disable RedundantNameQualifier

namespace DatenMeister.Models.Forms
{
    public static class IntegrateFormAndFields
    {
        /// <summary>
        /// Assigns the types of form and fields by converting the 
        /// .Net objects to DatenMeister elements and adds them into 
        /// the filler, the collection and also into the lookup. 
        /// </summary>
        /// <param name="uml">The uml metamodel to be used</param>
        /// <param name="factory">Factory being used for creation</param>
        /// <param name="collection">Collection that shall be filled</param>
        /// <param name="filledStructure">The form and fields structure</param>
        /// <param name="extent">And finally extent to which the types shall be registered</param>
        public static void Assign(_UML uml, IFactory factory, IReflectiveCollection collection, _FormAndFields filledStructure, MofExtent extent)
        {
            var generator = new DotNetTypeGenerator(factory, uml, extent);
            {
                var type = typeof(DatenMeister.Models.Forms.FieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__FieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.SortingOrder);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__SortingOrder = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.AnyDataFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__AnyDataFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.CheckboxFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__CheckboxFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.DateTimeFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__DateTimeFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.FormAssociation);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__FormAssociation = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.DropDownFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__DropDownFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.ValuePair);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__ValuePair = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.MetaClassElementFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__MetaClassElementFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.ReferenceFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__ReferenceFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.SubElementFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__SubElementFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.TextFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__TextFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.SeparatorLineFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__SeparatorLineFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.FileSelectionFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__FileSelectionFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.DefaultTypeForNewElement);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__DefaultTypeForNewElement = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.FullNameFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__FullNameFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.CheckboxListTaggingFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__CheckboxListTaggingFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.NumberFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__NumberFieldData = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.FormType);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__FormType = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.Form);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__Form = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.DetailForm);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__DetailForm = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.ListForm);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__ListForm = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.ExtentForm);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__ExtentForm = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.ViewModes.ViewMode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__ViewMode = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
        }
    }
}
