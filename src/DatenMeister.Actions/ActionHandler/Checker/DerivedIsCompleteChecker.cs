using DatenMeister.Actions.Model;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Actions.ActionHandler.Checker;

public class DerivedIsCompleteChecker : IChecker
{
    public bool IsResponsible(IElement checkerConfig)
    {   
        return checkerConfig.getMetaClass()?.equals(
            _Actions.TheOne.Checker.__DerivedIsCompleteChecker) == true;
    }

    public Task Check(CheckerParameter parameters)
    {
        throw new NotImplementedException();
    }
}