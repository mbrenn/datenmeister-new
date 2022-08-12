using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Json;
using DatenMeister.Types;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;
        private readonly LocalTypeSupport _localTypeSupport;

        public TypesController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;

            _localTypeSupport = new LocalTypeSupport(_workspaceLogic, _scopeStorage);
        }

        [HttpGet("api/types/all")]
        public ActionResult<List<ItemWithNameAndId>> GetTypes()
        {
            var result = new List<ItemWithNameAndId>();

            var allTypes = _localTypeSupport.GetAllTypes();

            result.AddRange(
                allTypes.OfType<IObject>().Select(o => ItemWithNameAndId.Create(o)!));

            return result;
        }

        /// <summary>
        /// Gets the property type of a certain property within a metaclass.
        /// This is a helper-method for the SubElements View, so the user gets already the right
        /// property type pre-selected when he wants to create a new subelement
        /// </summary>
        /// <param name="metaClass">Metaclass of the element to which a new element shall
        /// be created within one of its properties</param>
        /// <param name="propertyName">Name of the property to which the subelement
        /// shall be added to. </param>
        /// <returns>The item with name and id containing the type of the property</returns>
        [HttpGet("api/types/propertytype/{metaClass}/{propertyName}")]
        public ActionResult<ItemWithNameAndId?> GetPropertyType(string metaClass, string propertyName)
        {
            metaClass = HttpUtility.UrlDecode(metaClass);
            propertyName = HttpUtility.UrlDecode(propertyName);
            
            var foundMetaClass = _workspaceLogic.GetTypesWorkspace().FindElementByUri(metaClass);
            if (foundMetaClass == null)
            {
                return NotFound($"MetaClass '{metaClass}' is not found");
            }

            var property = ClassifierMethods.GetPropertyOfClassifier(foundMetaClass, propertyName);
            if (property == null)
            {
                return NotFound($"Property {propertyName} is not found");
            }
            
            var propertyType = 
                PropertyMethods.GetPropertyType(property);
            if (propertyType == null)
            {
                return new ActionResult<ItemWithNameAndId?>((ItemWithNameAndId?) null);
            }

            return ItemWithNameAndId.Create(propertyType);
        }
    }
}