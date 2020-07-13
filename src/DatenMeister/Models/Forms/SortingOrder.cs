using System.Dynamic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Forms
{
    public class SortingOrder
    {
        public FieldData? field { get; set; }

        public bool isDescending { get; set; }
    }
}