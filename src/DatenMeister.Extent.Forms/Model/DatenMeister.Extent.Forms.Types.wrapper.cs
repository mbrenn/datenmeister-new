using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Extent.Forms.Model
{
    public class Root
    {
        public class MassImportDefinitionAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @item
            {
                get =>
                    innerDmElement.get("item");
                set => 
                    innerDmElement.set("item", value);
            }

            public string @text
            {
                get =>
                    innerDmElement.getOrDefault<string>("text");
                set => 
                    innerDmElement.set("text", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

    }

}
