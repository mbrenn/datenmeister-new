using System;
using System.Xml.XPath;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.Validators;
using DatenMeister.Runtime;

namespace DatenMeister.WPF.Navigation.Validators
{
    public class NewXmiExtentValidator : IElementValidator
    {
        public ValidatorResult ValidateElement(IObject element)
        {
            ValidatorResult first = null;
            Uri uri;
            
            if (!Uri.TryCreate(element.getOrDefault<string>("uri"), UriKind.Absolute, out uri))
            {
                first = new ValidatorResult(ValidatorState.Failed, "Uri is not a uri")
                {
                    PropertyName = "uri"
                };
            }

            var result =
                new ValidatorResult(ValidatorState.Failed, "Not ok");
            if (first == null)
            {
                first = result;
            }
            else
            {
                first.Next = result;
            }

            return first;
        }
    }
}