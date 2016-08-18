using DatenMeister.Models.Forms;

namespace DatenMeister.UWP.Forms
{
    public interface IField
    {
        void CreateField(DetailFormHelper helper, FieldData fieldData);
    }
}