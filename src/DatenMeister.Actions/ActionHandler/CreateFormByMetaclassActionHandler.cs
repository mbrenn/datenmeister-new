using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;

namespace DatenMeister.Actions.ActionHandler
{
    public class CreateFormByMetaclassCreationMode
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
                _DatenMeister.TheOne.Actions.__CreateFormByMetaClass) == true;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(() =>
            {
                var formCreator = new FormCreator(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
                var formMethods = new FormMethods(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
                var metaClass = action.getOrDefault<IElement>(_DatenMeister._Actions._CreateFormByMetaClass.metaClass);
                var creationMode =
                    action.getOrDefault<string>(_DatenMeister._Actions._CreateFormByMetaClass.creationMode);
                var targetContainer =
                    action.getOrDefault<IObject>(_DatenMeister._Actions._CreateFormByMetaClass.targetContainer);
                var targetReflection = targetContainer == null
                    ? formMethods.GetUserFormExtent().elements()
                    : DefaultClassifierHints.GetDefaultReflectiveCollection(targetContainer);

                IElement form;
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
                    form = formCreator.CreateObjectFormForMetaClass(metaClass, new FormFactoryConfiguration());
                    targetReflection.add(form);

                    if (includeFormAssociation)
                    {
                        var association = formMethods.AddFormAssociationForMetaclass(
                            form,
                            metaClass,
                            _DatenMeister._Forms.___FormType.Object);
                        targetReflection.add(association);
                    }
                }

                void CreateCollectionForm(bool includeFormAssociation)
                {
                    form = formCreator.CreateCollectionFormForMetaClass(metaClass);
                    targetReflection.add(form);

                    if (includeFormAssociation)
                    {
                        var association = formMethods.AddFormAssociationForMetaclass(
                            form,
                            metaClass,
                            _DatenMeister._Forms.___FormType.Collection);
                        targetReflection.add(association);
                    }
                }
            });

            return null;
        }
    }
}