using DatenMeister.Web.Models.Fields;

namespace DatenMeister.UWP.Forms
{
    public interface IField
    {
        void CreateField(DetailFormHelper helper, FieldData field);
    }
}