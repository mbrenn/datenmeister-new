using System.Diagnostics;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Html;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Reports.Simple
{
    /// <summary>
    /// This class creates a simple report just for a collection. 
    /// </summary>
    public class SimpleReportForCollection
    {
        private readonly IHtmlReport _report;
        private readonly FormCreator _formCreator;
        private readonly ItemFormatter _itemFormatter;

        /// <summary>
        /// Gets or sets the report table mode
        /// </summary>
        public _DatenMeister._Reports._Elements.___ReportTableForTypeMode TableForTypeMode
        {
            get;
            set;
        } = _DatenMeister._Reports._Elements.___ReportTableForTypeMode.PerType;

        /// <summary>
        /// Gets or sets the form to be set
        /// If set to null, the default form will be applied
        /// </summary>
        public IElement? Form
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether metaclass headlines shall be shown
        /// </summary>
        public bool ShowMetaClassHeadlines { get; set; } = true;
        
        public bool AddFullNameColumn { get; set; }

        public SimpleReportForCollection(FormCreator formCreator, ItemFormatter itemFormatter, IHtmlReport report)
        {
            _formCreator = formCreator;
            _itemFormatter = itemFormatter;
            _report = report;
        }

        public SimpleReportForCollection(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage, IHtmlReport report)
        {
            _report = report;
            _formCreator = FormCreator.Create(workspaceLogic, scopeStorage);
            _itemFormatter = new ItemFormatter(_report);
        }
        
        public void WriteReportForCollection(
            IReflectiveCollection elements,
            FormFactoryConfiguration creationMode)
        {
            var foundForm = Form;
            if (TableForTypeMode == _DatenMeister._Reports._Elements.___ReportTableForTypeMode.PerType)
            {
                // Splits them up by metaclasses 
                var metaClasses =
                    elements.GroupBy(
                        x => x is IElement element ? element.metaclass : null,
                        new MofObjectEqualityComparer()).ToList();

                foreach (var metaClass in metaClasses)
                {
                    // Gets the name of the metaclass
                    var metaClassName = metaClass.Key == null
                        ? "Unclassified"
                        : "Classifier: " + NamedElementMethods.GetName(metaClass.Key);

                    if (ShowMetaClassHeadlines)
                    {
                        _report.Add(new HtmlHeadline(metaClassName, 2));
                    }

                    var collection = new TemporaryReflectiveCollection(metaClass);

                    if (metaClass.Key == null)
                    {
                        foundForm = _formCreator.CreateListFormForCollection(
                            collection,
                            creationMode);
                    }
                    else
                    {
                        foundForm = _formCreator.CreateListFormForMetaClass(metaClass.Key, creationMode);
                    }

                    AddFullNameColumnIfNecessary(foundForm);

                    ReportItemCollection(collection, foundForm);
                }
            }
            else
            {
                if (foundForm == null)
                {
                    foundForm = _formCreator.CreateListFormForCollection(
                        elements,
                        creationMode);

                    AddFullNameColumnIfNecessary(foundForm);
                }

                ReportItemCollection(elements, foundForm);
            }
        }

        private void AddFullNameColumnIfNecessary(IObject foundForm)
        {
            if (AddFullNameColumn)
            {
                // Create the metaclass as a field
                var fullNamefield = MofFactory.Create(foundForm, _DatenMeister.TheOne.Forms.__FullNameFieldData);
                fullNamefield.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Path");
                fullNamefield.set(_DatenMeister._Forms._MetaClassElementFieldData.title, "Path");
                foundForm.get<IReflectiveSequence>(_DatenMeister._Forms._ListForm.field).add(0, fullNamefield);
            }
        }
        
        

        private void ReportItemCollection(IReflectiveCollection metaClass, IObject form)
        {
            // Gets the reflective sequence for the name
            var collection = new TemporaryReflectiveSequence(metaClass);

            Debug.Assert(_itemFormatter != null, nameof(_itemFormatter) + " != null");
            _itemFormatter!.FormatCollectionOfItems(collection, form);
        }
    }
}