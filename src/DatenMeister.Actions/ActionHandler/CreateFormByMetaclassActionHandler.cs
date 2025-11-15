using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Actions.ActionHandler;

public static class CreateFormByMetaclassCreationMode
{
    public const string Object = "Object";
    public const string Collection = "Collection";
    public const string ObjectCollection = "Object_Collection";
    public const string ObjectAssociation = "Object_Association";
    public const string CollectionAssociation = "Collection_Association";
    public const string ObjectCollectionAssociation = "Object_Collection_Association";
}

public class CreateFormByMetaclassActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Forms.__CreateFormByMetaClass) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var formMethods = new FormMethods(actionLogic.WorkspaceLogic);
            var metaClass = action.getOrDefault<IElement>(_Actions._Forms._CreateFormByMetaClass.metaClass);
            var creationMode =
                action.getOrDefault<string>(_Actions._Forms._CreateFormByMetaClass.creationMode);
            var targetContainer =
                action.getOrDefault<IObject>(_Actions._Forms._CreateFormByMetaClass.targetContainer);
            var targetReflection = targetContainer == null
                ? formMethods.GetUserFormExtent().elements()
                : DefaultClassifierHints.GetDefaultReflectiveCollection(targetContainer);
            var factory = new FormCreationContextFactory(
                actionLogic.WorkspaceLogic,
                actionLogic.ScopeStorage);
            var context = factory.Create(string.Empty);
            context.Global.Factory = new MofFactory(targetReflection);

            switch (creationMode)
            {
                case CreateFormByMetaclassCreationMode.Object:
                    CreateObjectForm(false);
                    break;
                case CreateFormByMetaclassCreationMode.Collection:
                    CreateCollectionForm(false);
                    break;
                case CreateFormByMetaclassCreationMode.ObjectCollection:
                    CreateObjectForm(false);
                    CreateCollectionForm(false);
                    break;
                case CreateFormByMetaclassCreationMode.ObjectAssociation:
                    CreateObjectForm(true);
                    break;
                case CreateFormByMetaclassCreationMode.CollectionAssociation:
                    CreateCollectionForm(true);
                    break;
                case CreateFormByMetaclassCreationMode.ObjectCollectionAssociation:
                    CreateObjectForm(true);
                    CreateCollectionForm(true);
                    break;
                default:
                    throw new InvalidOperationException("Unknown creationMode");
            }

            void CreateObjectForm(bool includeFormAssociation)
            {
                var form = FormCreation.CreateObjectForm(
                           new ObjectFormFactoryParameter
                           {
                               MetaClass = metaClass
                           }, context).Form
                       ?? throw new InvalidOperationException("Form was not created");
                targetReflection.add(form);

                if (includeFormAssociation)
                {
                    var association = FormMethods.AddFormAssociationForMetaclass(
                        form,
                        metaClass,
                        _Forms.___FormType.Object);
                    targetReflection.add(association);
                }
            }

            void CreateCollectionForm(bool includeFormAssociation)
            {
                var form = FormCreation.CreateCollectionForm(
                           new CollectionFormFactoryParameter
                           {
                               MetaClass = metaClass
                           },
                           context).Form
                       ?? throw new InvalidOperationException("Form is null");
                targetReflection.add(form);

                if (includeFormAssociation)
                {
                    var association = FormMethods.AddFormAssociationForMetaclass(
                        form,
                        metaClass,
                        _Forms.___FormType.Collection);
                    targetReflection.add(association);
                }
            }
        });

        return null;
    }
}
