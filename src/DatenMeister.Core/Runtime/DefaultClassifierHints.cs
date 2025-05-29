using System.Collections;
using System.Globalization;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.Runtime;

/// <summary>
/// Stores the default classifier hints which allow a harmonized identification
/// of common classifier, like packages, 
/// </summary>
public class DefaultClassifierHints
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(DefaultClassifierHints));

    /// <summary>
    /// Gets the default package classifier for a given extent
    /// </summary>
    /// <param name="uriExtent">Extent to be used</param>
    /// <returns>The found extent</returns>
    public IElement? GetDefaultPackageClassifier(IHasExtent uriExtent)
    {
        var extent = uriExtent.Extent ?? throw new InvalidOperationException("UriExtent does not have an extent");
        return GetDefaultPackageClassifier(extent);
    }

    /// <summary>
    /// Gets the default package classifier for a given extent
    /// </summary>
    /// <param name="extent">Extent to be used</param>
    /// <returns>The found extent</returns>
    public static IElement? GetDefaultPackageClassifier(IExtent extent)
    {
        // First look into the standard uml meta classes
        var findByUrl = GetDefaultPackageClassifiers(extent).FirstOrDefault();
        if (findByUrl == null)
        {
            Logger.Warn("No default package was found in the given extent");
        }

        return findByUrl;
    }

    /// <summary>
    /// Gets the default classifiers defining packaging elements
    /// </summary>
    /// <param name="extent">Extent whose packages are evaluated</param>
    /// <returns>The defined packages</returns>
    public static IEnumerable<IElement> GetDefaultPackageClassifiers(IExtent extent)
    {
        var workspace = extent.GetWorkspace();
        if (workspace != null && workspace.MetaWorkspaces.Any(x => x.id == WorkspaceNames.WorkspaceUml || x.id == WorkspaceNames.WorkspaceMof))
        {
            yield return _UML.TheOne.Packages.__Package;
        }

        yield return _DatenMeister.TheOne.CommonTypes.Default.__Package;
    }

    /// <summary>
    /// Gets the default name of the property which contains the elements of a property
    /// This name is dependent upon the element to which the object will be added and
    /// the extent in which the element will be added.  
    /// </summary>
    /// <param name="packagingElement">Element in which the element will be added</param>
    /// <returns>The name of the property to which the element will be added</returns>
    public static string GetDefaultPackagePropertyName(IObject packagingElement)
    {
        return _UML._Packages._Package.packagedElement;
    }

    /// <summary>
    /// Adds the given child to the container by considering whether the container
    /// is an extent or is a MOF object. If an extent, it will be simply added as a child
    /// otherwise, it will be added to the property
    /// </summary>
    /// <param name="container">Container to which the element will be added</param>
    /// <param name="child">Child element which will be added</param>
    public static void AddToExtentOrElement(IObject container, IObject child)
    {
        switch (container)
        {
            case null:
                throw new InvalidOperationException("container is null");
            case IExtent extent:
                extent.elements().add(child);
                break;
            default:
            {
                var propertyName = GetDefaultPackagePropertyName(container);
                container.AddCollectionItem(propertyName, child);
                break;
            }
        }
    }

    /// <summary>
    /// Gets the default reflective collection for the given element.
    /// For extents, it is an enumeration of all elements, for the objects, it is the default
    /// property being used for compositions
    /// </summary>
    /// <param name="element">Element to be used. </param>
    /// <returns>the reflectiv collection</returns>
    public static IReflectiveCollection GetDefaultReflectiveCollection(IObject element)
    {
        return element switch
        {
            null => throw new InvalidOperationException("container is null"),
            IExtent extent => extent.elements(),
            _ => element.get<IReflectiveCollection>(GetDefaultPackagePropertyName(element))
        };
    }

    public static void RemoveFromExtentOrElement(IObject container, IObject child)
    {
        if (container is IExtent extent)
        {
            extent.elements().remove(child);
        }
        else
        {
            var propertyName = GetDefaultPackagePropertyName(container);
            if (!container.RemoveCollectionItem(propertyName, child))
            {
                throw new InvalidOperationException(
                    $"The element {NamedElementMethods.GetName(child)} could not be removed as " +
                    $"expected from container {NamedElementMethods.GetName(container)}");
            }
        }
    }

    /// <summary>
    /// Gets the information whether the given element is a package to which
    /// an additional object can be added as an enumeration. If this method returns true,
    /// an element can be added via AddToExtentOrElement
    /// </summary>
    /// <param name="value">Element which shall be checked</param>
    /// <returns>true, if the given element is a package</returns>
    public static bool IsPackageLike(IObject value)
    {
        // At the moment, every element is a package
        return true;
    }

    public static IEnumerable<string> GetPackagingPropertyNames(IObject item)
    {
        var metaClass = (item as IElement)?.getMetaClass();
        if (metaClass?.Equals(_DatenMeister.TheOne.Management.__Workspace) == true)
        {
            yield return _DatenMeister._Management._Workspace.extents;
        }

        // Hard coded at the moment, will be replaced by composite property identification
        if (metaClass?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
        {
            yield return _UML._StructuredClassifiers._Class.ownedAttribute;
            yield return _UML._StructuredClassifiers._Class.ownedOperation;
            yield return _UML._StructuredClassifiers._Class.ownedReception;
        }
        else
        {
            var hadPackagedElement = false;
            if (metaClass != null)
            {
                var compositing = ClassifierMethods.GetCompositingProperties(metaClass).ToList();
                if (compositing.Count > 0)
                {
                    foreach (var composite in compositing)
                    {
                        var name = NamedElementMethods.GetName(composite);
                        if (name == _UML._Packages._Package.packagedElement)
                        {
                            hadPackagedElement = true;
                        }

                        yield return name;
                    }
                }
            }
                
            // If we know nothing, then add the packaged element
            if (!hadPackagedElement)
            {
                yield return _UML._Packages._Package.packagedElement;
            }
        }
    }

    public static IEnumerable<IElement> GetPackagedElements(IObject item)
    {
        // Gets the items as elements
        if (item is IExtent asExtent)
        {
            foreach (var element in asExtent.elements())
            {
                if (element is IElement asElement)
                {
                    yield return asElement;
                }
            }

            yield break;
        }
            
        // Gets the items as properties
        var propertyName = GetPackagingPropertyNames(item).ToList();
        foreach (var property in propertyName)
        {
            if (item.isSet(property))
            {
                var value = item.get(property);
                if (DotNetHelper.IsOfEnumeration(value))
                {
                    var valueAsEnumerable  = (value as IEnumerable)!;
                    foreach (var valueItem in valueAsEnumerable)
                    {
                        if (valueItem is IElement asElement)
                        {
                            yield return asElement;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the information whether the propertyname is a generic property
    /// </summary>
    /// <param name="element">Element to be handled</param>
    /// <param name="propertyName">Name of the property</param>
    /// <returns></returns>
    public static bool IsPackagingProperty(IObject element, string propertyName)
    {
        return GetPackagingPropertyNames(element).Contains(propertyName);
    }
        
    /// <summary>
    /// Checks whether the property is so generic, that it shall be kept in the lists.
    /// This method is especially used in the FormCreator.ListForm which tries to minimize
    /// the number of columns within the subelementsfield.
    ///
    /// All properties which return 'true' here, are kept even though they sould have
    /// been removed since the property is not available in other fields
    /// </summary>
    /// <param name="element">Element containing the property</param>
    /// <param name="propertyName">Name of the property</param>
    /// <returns>true, if the field shall be kept</returns>
    public static bool IsGenericProperty(IObject element, string propertyName)
        =>
            propertyName == _UML._CommonStructure._NamedElement.name
            || propertyName.ToLower(CultureInfo.InvariantCulture) == "id";
}