using DatenMeister.Web.Models.Forms;

namespace DatenMeister.UWP.Forms
{
    public interface IField
    {
        void CreateField(DetailFormHelper helper, FieldData fieldData);
    }
}