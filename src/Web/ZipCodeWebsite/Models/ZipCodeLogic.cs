using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.ExtentManager.ExtentStorage;
using DatenMeister.Modules.ZipCodeExample;
using ZipCodeLibrary;
using ZipCodeModel = ZipCodeLibrary.ZipCodeModel;

namespace ZipCodeWebsite.Models
{
    public class ZipCodeLogic
    {
        /// <summary>
        /// Gets the zipcodes upon a search string
        /// </summary>
        /// <param name="search">Search string to be queried</param>
        /// <returns>The model being returned</returns>
        public static ZipCodeModel GetZipCodes(string? search)
        {
            if (ZipCodeExtent == null)
            {
                throw new InvalidOperationException("ZipCodeExtent not booted up");
            }
            
            IReflectiveCollection elements = ZipCodeExtent.elements();
            var data = new List<ZipCodeData>();

            if (!string.IsNullOrEmpty(search))
            {
                elements = elements.WhenOneOfThePropertyContains(
                    new[] {"zip", "name"},
                    search,
                    StringComparison.CurrentCultureIgnoreCase);
            }

            foreach (var element in elements.OfType<IElement>().Take(101))
            {
                data.Add(
                    new ZipCodeData
                    {
                        id = element.getOrDefault<int>("id"),
                        name = element.getOrDefault<string>("name"),
                        zip = element.getOrDefault<string>("zip"),
                        positionLong = element.getOrDefault<double>("positionLong"),
                        positionLat = element.getOrDefault<double>("positionLat")
                    });
            }

            return new ZipCodeModel
            {
                items = data.Take(100).ToList(),
                truncated = data.Count >= 100,
                noItemFound = data.Count == 0
            };
        }
        
        /// <summary>
        /// Gets or sets the zipcode extent
        /// </summary>
        public static IUriExtent? ZipCodeExtent { get; set; }
        
        /// <summary>
        /// Prepares the zipcode
        /// </summary>
        /// <param name="dm"></param>
        public static void PrepareZipCode(IDatenMeisterScope dm)
        {
            var manager = new ZipCodeExampleManager(
                dm.WorkspaceLogic,
                new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage), 
                dm.ScopeStorage);

            var foundExtent = 
                dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceData, "dm:///zipcodes/")
                    as IUriExtent;
            
            if (foundExtent == null)
            {
                foundExtent = manager.AddZipCodeExample(
                    WorkspaceNames.WorkspaceData, 
                    "dm:///zipcodes/",
                    null,
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program))!.Location!)!,
                        "Loaded/zipcodes.csv"));
            }

            ZipCodeExtent = foundExtent;
        }
        
    }
}