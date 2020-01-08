using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Performs an allocation between the view and a specific metaclass which supports the retrieval of default views.
    /// </summary>
    public class FormAssociation
    {
        public FormType formType { get; set; }

        public IObject metaClass { get; set; }

        public string extentType { get; set; }

        /// <summary>
        /// Gets or sets the metaclass of the parent object containg the list of elements
        /// </summary>
        public IElement parentMetaClass { get; set; }

        public string parentProperty { get; set; }


        public Form form { get; set; }

        public FormAssociation()
        {
        }

        public FormAssociation(FormType formType)
        {
            this.formType = formType;
        }

        public FormAssociation(FormType formType, Form form) : this(formType)
        {
            this.form = form;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Form for: ");
            builder.Append(formType.ToString());

            if (extentType != null)
            {
                builder.Append($" - ExtentType: {extentType}");
            }

            if (metaClass != null)
            {
                builder.Append($" - MetaClass: {NamedElementMethods.GetName(metaClass)}");
            }

            return builder.ToString();
        }
    }
}