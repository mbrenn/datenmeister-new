using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.2.0.0
namespace DatenMeister.Zip.Model
{
    public class Root
    {
        public class ZipFileExtractAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @sourcePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("sourcePath");
                set => 
                    innerDmElement.set("sourcePath", value);
            }

            public string @targetPath
            {
                get =>
                    innerDmElement.getOrDefault<string>("targetPath");
                set => 
                    innerDmElement.set("targetPath", value);
            }

            public bool @overwriteIfExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("overwriteIfExisting");
                set => 
                    innerDmElement.set("overwriteIfExisting", value);
            }

            public bool @overwriteOnlyIfNewer
            {
                get =>
                    innerDmElement.getOrDefault<bool>("overwriteOnlyIfNewer");
                set => 
                    innerDmElement.set("overwriteOnlyIfNewer", value);
            }

        }

        public class ZipFileExtractActionResult_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @success
            {
                get =>
                    innerDmElement.getOrDefault<bool>("success");
                set => 
                    innerDmElement.set("success", value);
            }

            public bool @alreadyExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("alreadyExisting");
                set => 
                    innerDmElement.set("alreadyExisting", value);
            }

            public bool @isAlreadyUpToDate
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAlreadyUpToDate");
                set => 
                    innerDmElement.set("isAlreadyUpToDate", value);
            }

        }

    }

}
