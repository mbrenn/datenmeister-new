using System.Diagnostics;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Html;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Reports.Simple;

/// <summary>
/// This class creates a simple report just for a collection. 
/// </summary>
public class SimpleReportForCollection(FormCreationContext formContext, ItemFormatter itemFormatter, IHtmlReport report)
{
    /// <summary>
    /// Gets or sets the report table mode
    /// </summary>
    public _Reports._Elements.___ReportTableForTypeMode TableForTypeMode
    {
        get;
        set;
    } = _Reports._Elements.___ReportTableForTypeMode.PerType;

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

    public void WriteReportForCollection(
        IReflectiveCollection elements)
    {
        var foundForm = Form;
        
        if (TableForTypeMode == _Reports._Elements.___ReportTableForTypeMode.PerType)
        {
            // Splits them up by metaclasses 
            var metaClasses =
                elements.GroupBy(
                    x => x is IElement element ? element.metaclass : null,
                    new MofElementEqualityComparer()).ToList();

            foreach (var metaClass in metaClasses)
            {
                // Gets the name of the metaclass
                var metaClassName = metaClass.Key == null
                    ? "Unclassified"
                    : "Classifier: " + NamedElementMethods.GetName(metaClass.Key);

                if (ShowMetaClassHeadlines)
                {
                    report.Add(new HtmlHeadline(metaClassName, 2));
                }

                var collection = new TemporaryReflectiveCollection(metaClass);

                if (metaClass.Key == null)
                {
                    foundForm = FormCreation.CreateTableForm(
                        new TableFormFactoryParameter()
                        {
                            Collection = collection
                        },
                        formContext).Forms.FirstOrDefault();
                }
                else
                {
                    foundForm = FormCreation.CreateTableForm(
                        new TableFormFactoryParameter()
                        {
                            MetaClass = metaClass.Key
                        }, formContext).Forms.FirstOrDefault();
                }
                
                if(foundForm == null)
                    throw new InvalidOperationException("foundForm is null");

                AddFullNameColumnIfNecessary(foundForm);

                ReportItemCollection(collection, foundForm);
            }
        }
        else
        {
            if (foundForm == null)
            {
                foundForm = FormCreation.CreateTableForm(
                    new TableFormFactoryParameter()
                    {
                        Collection = elements
                    }, formContext).Forms.FirstOrDefault() ?? throw new InvalidOperationException("foundForm is null");
                
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
            var fullNamefield = MofFactory.CreateElement(foundForm, _Forms.TheOne.__FullNameFieldData);
            fullNamefield.set(_Forms._MetaClassElementFieldData.name, "Path");
            fullNamefield.set(_Forms._MetaClassElementFieldData.title, "Path");
            foundForm.get<IReflectiveSequence>(_Forms._TableForm.field).add(0, fullNamefield);
        }
    }

    private void ReportItemCollection(IReflectiveCollection metaClass, IObject form)
    {
        // Gets the reflective sequence for the name
        var collection = new TemporaryReflectiveSequence(metaClass);

        Debug.Assert(itemFormatter != null, nameof(itemFormatter) + " != null");
        itemFormatter.FormatCollectionOfItems(collection, form);
    }
}