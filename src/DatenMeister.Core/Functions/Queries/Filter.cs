using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Functions.Queries;

/// <summary>
///     Defines a static helper class, which eases
///     the access to the filters.
/// </summary>
public static class Filter
{
    /// <summary>
    /// Gets the elements out of the reflective collection in which a certain proprety starts
    /// with a the given value.
    /// </summary>
    /// <param name="collection">Collection to be queried</param>
    /// <param name="property">Property to be set</param>
    /// <param name="value">The property needs to start with the given value</param>
    /// <returns></returns>
    public static IReflectiveCollection WhenPropertyStartsWith(
        this IReflectiveCollection collection,
        string property,
        string value)
    {
        return new RowFilterOnPropertyByPredicateCollection<string>(
            collection,
            property,
            x => x?.StartsWith(value) == true);
    }      

    public static IReflectiveCollection WhenElementIsObject(
        this IReflectiveCollection collection)
        =>
            new RowFilterOnElementType<IObject>(collection);

    public static IReflectiveCollection WhenMetaClassIs(
        this IReflectiveCollection collection,
        IElement? metaClass)
        =>
            new RowFilterOnMetaClass(collection, metaClass);

    public static IReflectiveCollection WhenMetaClassIsOrSpecialized(
        this IReflectiveCollection collection,
        IElement? metaClass)
        =>
            new RowFilterOnMetaClassOrSpecialized(collection, metaClass);

    public static IReflectiveCollection WhenMetaClassIsOneOf(
        this IReflectiveCollection collection,
        params IElement[] metaClasses)
        =>
            new RowFilterOnMetaClass(collection, metaClasses);

    public static IReflectiveCollection WhenMetaClassIsNotSet(
        this IReflectiveCollection collection)
        =>
            new RowFilterOnMetaClassIsNotSet(collection);

    public static IReflectiveCollection WhenPropertyIsSet(
        this IReflectiveCollection collection,
        string propertyName)
        =>
            new RowFilterOnPropertyIsSet(collection, propertyName);

    public static IReflectiveCollection WhenPropertyHasValue<T>(
        this IReflectiveCollection collection,
        string property,
        T value)
    {
        return new RowFilterOnPropertyByPredicateCollection<T>(
            collection,
            property,
            x => x?.Equals(value) == true);
    }

    public static IReflectiveCollection WhenPropertyIsOneOf<T>(
        this IReflectiveCollection collection,
        string property,
        IEnumerable<T> values)
    {
        var valuesAsList = values.ToList();
        return new RowFilterOnPropertyByPredicateCollection<T>(
            collection,
            property,
            x => valuesAsList.Any(y => x?.Equals(y) == true));
    }

    public static IReflectiveCollection WhenPropertyContains(
        this IReflectiveCollection collection,
        string property,
        string value,
        StringComparison comparer = StringComparison.CurrentCulture)
        =>
            new RowFilterOnMultipleProperties(
                collection,
                [property],
                value,
                comparer);

    public static IReflectiveCollection WhenOneOfThePropertyContains(
        this IReflectiveCollection collection,
        IEnumerable<string> properties,
        string value,
        StringComparison comparer = StringComparison.CurrentCulture)
        =>
            new RowFilterOnMultipleProperties(
                collection,
                properties,
                value,
                comparer);

    /// <summary>
    /// Gets all descendents of a reflective collection by opening all properties recursively
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <param name="filter">Filter to be added</param>
    /// <returns>A reflective collection, containing all items</returns>
    public static IReflectiveSequence WhenFiltered(
        this IReflectiveCollection collection,
        Func<object?, bool> filter)
        =>
            new TemporaryReflectiveSequence(collection.Where(filter));

    /// <summary>
    /// Gets all descendents of a reflective collection by opening all properties recursively
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <returns>A reflective collection, containing all items</returns>
    public static IReflectiveSequence GetAllDescendants(
        this IReflectiveCollection collection)
        =>
            new TemporaryReflectiveSequence(AllDescendentsQuery.GetDescendents(collection).Cast<object>());

    /// <summary>
    /// Gets all descendents of a reflective collection by opening all properties recursively
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <returns>A reflective collection, containing all items</returns>
    public static IReflectiveSequence GetCompositeDescendents(
        this IReflectiveCollection collection)
        =>
            new TemporaryReflectiveSequence(AllDescendentsQuery.GetDescendents(collection, null, DescendentMode.OnlyComposites).Cast<object>());

    /// <summary>
    /// Gets all descendents of a reflective collection by opening all properties recursively.
    /// The elements of the collection themselves will also be returned
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <param name="byFollowingProperties">The attached properties are followed up</param>
    /// <returns>A reflective collection, containing all items</returns>
    public static IReflectiveSequence GetAllDescendantsIncludingThemselves(
        this IReflectiveCollection collection,
        IEnumerable<string>? byFollowingProperties = null)
    {
        return new TemporaryReflectiveSequence(
            collection.AsEnumerable().Union(
                AllDescendentsQuery.GetDescendents(collection, byFollowingProperties, DescendentMode.IncludingItself).Cast<object>()));
    }

    /// <summary>
    /// Gets all descendents of a reflective collection by opening all properties recursively.
    /// The elements of the collection themselves will also be returned
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <returns>A reflective collection, containing all items</returns>
    public static IReflectiveSequence GetAllCompositesIncludingThemselves(
        this IReflectiveCollection collection)
    {
        return new TemporaryReflectiveSequence(
            AllDescendentsQuery.GetDescendents(
                    collection, 
                    null, 
                    DescendentMode.IncludingItself | DescendentMode.OnlyComposites)
                .Cast<object>().ToList());
    }

    /// <summary>
    /// Gets all descendents of a reflective collection by opening all properties recursively
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <param name="byFollowingProperties">Properties that shall be followed. This prevents the following of properties</param>
    /// <returns>A reflective collection, containing all items</returns>
    public static IReflectiveSequence GetAllDescendants(
        this IReflectiveCollection collection,
        IEnumerable<string> byFollowingProperties)
        =>
            new TemporaryReflectiveSequence(AllDescendentsQuery.GetDescendents(collection, byFollowingProperties).Cast<object>());

    /// <summary>
    /// Gets all descendents of a reflective collection by opening all properties recursively
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <param name="byFollowingProperty">Property that shall be followed. This prevents the following of properties</param>
    /// <returns>A reflective collection, containing all items</returns>
    public static IReflectiveSequence GetAllDescendants(
        this IReflectiveCollection collection,
        string byFollowingProperty)
        =>
            new TemporaryReflectiveSequence(AllDescendentsQuery
                .GetDescendents(collection, [byFollowingProperty]).Cast<object>().ToList());

    /// <summary>
    /// Orders the reflective section by the given property
    /// </summary>
    /// <param name="collection">Collection to be ordered</param>
    /// <param name="property">Property being used as key</param>
    /// <returns>Ordered reflective collection</returns>
    public static IReflectiveCollection OrderElementsBy(
        this IReflectiveCollection collection,
        string property)
        =>
            new RowOrderByProperties(collection, [property]);

    /// <summary>
    /// Orders the reflective section by the given property
    /// </summary>
    /// <param name="collection">Collection to be ordered</param>
    /// <param name="properties">Properties being used as key</param>
    /// <returns>Ordered reflective collection</returns>
    public static IReflectiveCollection OrderElementsBy(
        this IReflectiveCollection collection,
        IEnumerable<string> properties)
        =>
            new RowOrderByProperties(collection, properties);

    public static IReflectiveCollection FilterDistinct(
        this IReflectiveCollection collection,
        string property)
        =>
            new RowFilterDistinct(collection, property);

    /// <summary>
    /// Unionizes two reflective collections into one collection
    /// </summary>
    /// <param name="first">First collection</param>
    /// <param name="second">Second collection</param>
    /// <returns>The resulting union</returns>
    public static IReflectiveCollection Union(
        this IReflectiveCollection first,
        IReflectiveCollection second)
        =>
            new UnionQuery(first, second);

    public static IReflectiveCollection TakeFirst(
        this IReflectiveCollection collection,
        int number) => new TakeFirstQuery(collection, number);
}