using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Provider.CSV;
using System.Text;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Extent.Forms.MassImport;

public class MassImportLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    /// <summary>
    /// Performs the massimport
    /// </summary>
    /// <param name="extent">Extent in which the massimport shall be executed</param>
    /// <param name="importText">Text to be imported</param>
    public void PerformMassImport(IUriExtent extent, string importText)
    {
        // Checks if import Text is null or empty. 
        if (importText == null || importText == string.Empty)
        {
            return;
        }

        // Ok, we have at least some kind of text
        var factory = new MofFactory(extent);

        /// Step 1: Load the data
        // Does nothing... Test stub
        // Use the CSV Importer
        var data = new InMemoryProvider();
        var csvLoader = new CsvLoader(workspaceLogic);

        // Convert the string to a stream
        byte[] byteArray = Encoding.UTF8.GetBytes(importText);
        var stream = new MemoryStream(byteArray);

        // Configures the CSV Import
        var settings = InMemoryObject.CreateEmpty(
            _ExtentLoaderConfigs.TheOne.__CsvSettings);
        settings.set(_ExtentLoaderConfigs._CsvSettings.hasHeader, true);
        settings.set(_ExtentLoaderConfigs._CsvSettings.encoding, "UTF-8");
        settings.set(_ExtentLoaderConfigs._CsvSettings.separator, ",");

        // Now, do the import
        csvLoader.Load(data, stream, settings);

        var dataExtent = new MofUriExtent(data, scopeStorage);

        /// Step 2: 
        // Go through the properties and check that everything is ok
        foreach (var item in dataExtent.elements().OfType<IElement>().Where(x => x is IObjectAllProperties))
        {
            IElement? foundReference = null;

            // Checks, if the item has a property called 'id'
            var id = item.getOrDefault<string>("id");
            if (id != null)
            {
                foundReference = extent.elements().OfType<IElement>().Where(x => x.getOrDefault<string>("id") == id).FirstOrDefault();
            }

            if (foundReference == null)
            {
                // If item is not found or no id was given, we have to create a new item
                foundReference = factory.create(null);
                extent.elements().add(foundReference);
            }

            // Ok, now add the other attributes
            var objectAllProperties = item as IObjectAllProperties;
            foreach (var property in objectAllProperties!.getPropertiesBeingSet())
            {
                var value = item.get(property);
                if (value != null && value.ToString() != string.Empty)
                {
                    // Copy the data over
                    foundReference.set(property, item.get(property));
                }
            }
        }

        // Done... we can go to the next item
    }
}