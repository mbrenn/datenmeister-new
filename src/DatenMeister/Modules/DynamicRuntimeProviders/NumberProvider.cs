using System;
using System.Collections.Generic;
using System.Globalization;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Provider.DynamicRuntime;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using static DatenMeister.Models._DatenMeister._DynamicRuntimeProvider._Examples;

namespace DatenMeister.Modules.DynamicRuntimeProviders
{
    // ReSharper disable once UnusedMember.Global
    public class NumberProvider : IDynamicRuntimeProvider
    {
        public IEnumerable<IProviderObject> Get(DynamicRuntimeProviderWrapper wrapper, IElement configuration)
        {
            var begin =
                configuration?.isSet(_NumberProviderSettings.start) == true
                    ? configuration.getOrDefault<int>(_NumberProviderSettings.start)
                    : 0;
            var end =
                configuration?.isSet(_NumberProviderSettings.start) == true
                    ? configuration.getOrDefault<int>(_NumberProviderSettings.end)
                    : 100;

            for (var n = begin; n <= end; n++)
            {
                var item = new InMemoryObject(wrapper);
                item.SetProperty("binary", Convert.ToString(n, 2));
                item.SetProperty("octal", Convert.ToString(n, 8));
                item.SetProperty("decimal", Convert.ToString(n, 10));
                item.SetProperty("hexadecimal", Convert.ToString(n, 16));
                item.Id = n.ToString(CultureInfo.InvariantCulture);
                yield return item;
            }
        }
    }
}