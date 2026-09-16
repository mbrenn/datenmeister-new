using DatenMeister.Core;
using DatenMeister.Forms;
using DatenMeister.Forms.CollectionForms;
using DatenMeister.Forms.Fields;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.ObjectForm;
using DatenMeister.Forms.RowForm;
using DatenMeister.Forms.TableForms;
using DatenMeister.Forms.ViewModeConfigurators;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Forms;

[TestFixture]
public class ViewModeConfiguratorTests
{
    [Test]
    public async Task TestDefaultViewModeConfigurator()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        var factory = new FormCreationContextFactory(scope.WorkspaceLogic, scope.ScopeStorage);

        var context = factory.Create(ViewModes.Default);

        Assert.That(context.ViewModeId, Is.EqualTo(ViewModes.Default));
        Assert.That(context.Global.CollectionFormFactories.Any(f => f is FormFinderFactory), Is.True);
        Assert.That(context.Global.CollectionFormFactories.Any(f => f is CollectionFormFromMetaClass), Is.True);
        Assert.That(context.Global.CollectionFormFactories.Any(f => f is CollectionFormFromData), Is.True);
        Assert.That(context.Global.FieldFormFactories.Any(f => f is FieldFromPropertyType), Is.True);
        Assert.That(context.Global.FieldFormFactories.Any(f => f is FieldFromPropertyValue), Is.True);
    }

    [Test]
    public async Task TestAutoGenerateViewModeConfigurator()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        var factory = new FormCreationContextFactory(scope.WorkspaceLogic, scope.ScopeStorage);

        var context = factory.Create(ViewModes.AutoGenerate);

        Assert.That(context.ViewModeId, Is.EqualTo(ViewModes.AutoGenerate));
        Assert.That(context.Global.CollectionFormFactories.Any(f => f is FormFinderFactory), Is.False);
        Assert.That(context.Global.ObjectFormFactories.Any(f => f is FormFinderFactory), Is.False);
        Assert.That(context.Global.TableFormFactories.Any(f => f is FormFinderFactory), Is.False);
        Assert.That(context.Global.RowFormFactories.Any(f => f is FormFinderFactory), Is.False);
        Assert.That(context.Global.CollectionFormFactories.Any(f => f is CollectionFormFromMetaClass), Is.True);
        Assert.That(context.Global.FieldFormFactories.Any(f => f is FieldFromPropertyType), Is.True);
    }

    [Test]
    public async Task TestByPropertiesViewModeConfigurator()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        var factory = new FormCreationContextFactory(scope.WorkspaceLogic, scope.ScopeStorage);

        var context = factory.Create(ViewModes.ByProperties);

        Assert.That(context.ViewModeId, Is.EqualTo(ViewModes.ByProperties));
        Assert.That(context.Global.CollectionFormFactories.Any(f => f is FormFinderFactory), Is.False);
        Assert.That(context.Global.CollectionFormFactories.Any(f => f is CollectionFormFromMetaClass), Is.False);
        Assert.That(context.Global.ObjectFormFactories.Any(f => f is ObjectFormFromMetaClass), Is.False);
        Assert.That(context.Global.TableFormFactories.Any(f => f is TableFormForMetaClass), Is.False);
        Assert.That(context.Global.RowFormFactories.Any(f => f is RowFormFromMetaClass), Is.False);
        Assert.That(context.Global.FieldFormFactories.Any(f => f is FieldFromPropertyType), Is.False);
        Assert.That(context.Global.FieldFormFactories.Any(f => f is FieldFromPropertyValue), Is.True);
    }

    [Test]
    public async Task TestPluginIsExecuted()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        var scopeStorage = new ScopeStorage();
        var formsState = new FormsState();
        var pluginCalled = false;
        formsState.FormModificationPlugins.Add(new FormModificationPlugin
        {
            Name = "TestPlugin",
            CreateContext = ctx => pluginCalled = true
        });
        scopeStorage.Add(formsState);

        var factory = new FormCreationContextFactory(scope.WorkspaceLogic, scopeStorage);
        factory.Create(ViewModes.Default);

        Assert.That(pluginCalled, Is.True);
    }
}
