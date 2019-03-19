using System;
using DatenMeister.Modules.DataViews.Model;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewPlugin
    {
        public static Type[] GetTypes()
        {
            return new[]
            {
                typeof(DataView),
                typeof(ViewNode)
            };
        }
    }
}