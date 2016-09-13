using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.ManualMapping;
using NPOI.SS.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class SheetItem : MMElement<ISheet>
    {
        public SheetItem()
        {
            
        }

        public SheetItem(TypeMapping typeMapping, ISheet value, IElement container = null) : base(typeMapping, value, container)
        {
        }

        public string GetName()
        {
            return Value.SheetName;
        }
    }
}