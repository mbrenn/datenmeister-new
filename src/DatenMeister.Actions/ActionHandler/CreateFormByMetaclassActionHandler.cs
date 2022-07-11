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
        public const string Detail = "Detail";
        public const string List = "List";
        public const string DetailList = "Detail_List";
        public const string DetailAssociation = "Detail_Association";
        public const string ListAssociation = "List_Association";
        public const string DetailListAssociation = "Detail_List_Association";
        
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
                case CreateFormByMetaclassCreationMode.Detail:
                    CreateDetailForm(false);
                    break;
                case CreateFormByMetaclassCreationMode.List:
                    CreateListForm(false);
                    break;
                case CreateFormByMetaclassCreationMode.DetailList:
                    CreateDetailForm(false);
                    CreateListForm(false);
                    break;
                case CreateFormByMetaclassCreationMode.DetailAssociation:
                    CreateDetailForm(true);
                    break;
                case CreateFormByMetaclassCreationMode.ListAssociation:
                    CreateListForm(true);
                    break;
                case  CreateFormByMetaclassCreationMode.DetailListAssociation:
                    CreateDetailForm(true);
                    CreateListForm(true);
                    break;
                default:
                    throw new InvalidOperationException("Unknown creationMode");
            }
            
            void CreateDetailForm(bool includeFormAssociation)
            {
                form = formCreator.CreateRowFormByMetaClass(metaClass);
                formMethods.GetUserFormExtent().elements().add(form);

                if (includeFormAssociation)
                {
                    formMethods.AddFormAssociationForMetaclass(form, metaClass,
                        _DatenMeister._Forms.___FormType.Object);
                }
            }

            void CreateListForm(bool includeFormAssociation)
            {
                form = formCreator.CreateCollectionFormForItemsMetaClass(metaClass);
                formMethods.GetUserFormExtent().elements().add(form);

                if (includeFormAssociation)
                {
                    formMethods.AddFormAssociationForMetaclass(form, metaClass,
                        _DatenMeister._Forms.___FormType.Row);
                }
            }
        }
    }
}