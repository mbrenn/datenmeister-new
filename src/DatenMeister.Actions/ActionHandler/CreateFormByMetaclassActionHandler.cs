using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
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

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var metaClass = action.getOrDefault<IElement>(_DatenMeister._Actions._CreateFormByMetaClass.metaClass);
            var creationMode = action.getOrDefault<string>(_DatenMeister._Actions._CreateFormByMetaClass.creationMode);
            
            
            var formCreator = new FormCreator(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
            var formMethods = new FormMethods(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);

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
                case  CreateFormByMetaclassCreationMode.ObjectCollectionAssociation:
                    CreateObjectForm(true);
                    CreateCollectionForm(true);
                    break;
                default:
                    throw new InvalidOperationException("Unknown creationMode");
            }
            
            void CreateObjectForm(bool includeFormAssociation)
            {
                form = formCreator.CreateObjectFormForMetaClass(metaClass, new FormFactoryConfiguration());
                formMethods.GetUserFormExtent().elements().add(form);

                if (includeFormAssociation)
                {
                    formMethods.AddFormAssociationForMetaclass(form, metaClass,
                        _DatenMeister._Forms.___FormType.Object);
                }
            }

            void CreateCollectionForm(bool includeFormAssociation)
            {
                form = formCreator.CreateCollectionFormForMetaClass(metaClass);
                formMethods.GetUserFormExtent().elements().add(form);

                if (includeFormAssociation)
                {
                    formMethods.AddFormAssociationForMetaclass(form, metaClass,
                        _DatenMeister._Forms.___FormType.Collection);
                }
            }
        }
    }
}