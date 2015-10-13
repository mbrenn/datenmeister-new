using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.IO;
using DatenMeister.App.ZipCode;

namespace DatenMeister.Web.Server
{
	public partial class Startup
	{
        public void StartDatenmeister(IAppBuilder app)
        {
            Core.TheOne.Init();

            var workspaceData = Core.TheOne.Workspaces.Where(x => x.id == "Data").First();

            var file = Path.Combine(
                AppDomain.CurrentDomain.GetData("DataDirectory") as string,
                "plz.csv");
                        using (var stream = new FileStream(file, FileMode.Open))
            {
                DataProvider.TheOne.LoadZipCodes(stream);
                workspaceData.AddExtent(DataProvider.TheOne.ZipCodes);
            }
        }
	}
}