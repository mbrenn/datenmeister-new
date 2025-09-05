using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Functions.Manipulation;

public class FieldManipulation
{
    public static void KeepOnlyFields(IEnumerable<IObject> elements, IEnumerable<string> fields)
    {
        var fieldsAsHashSet = fields.ToHashSet();
        foreach (var element in elements)
        {
            var propertiesBeingSet = (element as IObjectAllProperties)?.getPropertiesBeingSet();
            if(propertiesBeingSet == null ) continue;

            foreach (var field in propertiesBeingSet)
            {
                if (fieldsAsHashSet.Contains(field)) continue;

                element.unset(field);
            }
        }
    }
}