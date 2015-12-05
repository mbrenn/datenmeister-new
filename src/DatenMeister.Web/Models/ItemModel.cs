using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace DatenMeister.Web.Models
{
    public class ItemModel
    {
        public ExtentModel extent
        {
            get;
            set;
        }

        public string url
        {
            get;
            private set;
        }

        public ItemModel(ExtentModel extent, string url)
        {
            this.extent = extent;
            this.url = url;
        }
    }
}
