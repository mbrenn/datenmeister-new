define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports._MOF = void 0;
    // Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.0.0.0
    var _MOF;
    (function (_MOF) {
        let _Identifiers;
        (function (_Identifiers) {
            _Identifiers.__URIExtent_Uri = "dm:///_internal/model/mof#_MOF-Identifiers-URIExtent";
            _Identifiers.__Extent_Uri = "dm:///_internal/model/mof#_MOF-Identifiers-Extent";
        })(_Identifiers = _MOF._Identifiers || (_MOF._Identifiers = {}));
        let _CMOFExtension;
        (function (_CMOFExtension) {
            let _Tag;
            (function (_Tag) {
                _Tag.tagOwner = "tagOwner";
            })(_Tag = _CMOFExtension._Tag || (_CMOFExtension._Tag = {}));
            _CMOFExtension.__Tag_Uri = "dm:///_internal/model/mof#_MOF-CMOFExtension-Tag";
        })(_CMOFExtension = _MOF._CMOFExtension || (_MOF._CMOFExtension = {}));
        let _Extension;
        (function (_Extension) {
            let _Tag;
            (function (_Tag) {
                _Tag.name = "name";
                _Tag.value = "value";
                _Tag.element = "element";
                _Tag.metaclass = "metaclass";
            })(_Tag = _Extension._Tag || (_Extension._Tag = {}));
            _Extension.__Tag_Uri = "dm:///_internal/model/mof#_MOF-Extension-Tag";
        })(_Extension = _MOF._Extension || (_MOF._Extension = {}));
        let _Common;
        (function (_Common) {
            _Common.__ReflectiveSequence_Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence";
            _Common.__ReflectiveCollection_Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection";
        })(_Common = _MOF._Common || (_MOF._Common = {}));
        let _CMOFReflection;
        (function (_CMOFReflection) {
            _CMOFReflection.__Factory_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Factory";
            _CMOFReflection.__Element_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Element";
            let _Argument;
            (function (_Argument) {
                _Argument.name = "name";
                _Argument.value = "value";
            })(_Argument = _CMOFReflection._Argument || (_CMOFReflection._Argument = {}));
            _CMOFReflection.__Argument_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Argument";
            _CMOFReflection.__Extent_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Extent";
            let _Link;
            (function (_Link) {
                _Link.firstElement = "firstElement";
                _Link.secondElement = "secondElement";
                _Link.association = "association";
            })(_Link = _CMOFReflection._Link || (_CMOFReflection._Link = {}));
            _CMOFReflection.__Link_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Link";
            let _Exception;
            (function (_Exception) {
                _Exception.objectInError = "objectInError";
                _Exception.elementInError = "elementInError";
                _Exception.description = "description";
            })(_Exception = _CMOFReflection._Exception || (_CMOFReflection._Exception = {}));
            _CMOFReflection.__Exception_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Exception";
        })(_CMOFReflection = _MOF._CMOFReflection || (_MOF._CMOFReflection = {}));
        let _Reflection;
        (function (_Reflection) {
            let _Factory;
            (function (_Factory) {
                _Factory._package_ = "package";
                _Factory.metaclass = "metaclass";
            })(_Factory = _Reflection._Factory || (_Reflection._Factory = {}));
            _Reflection.__Factory_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Factory";
            _Reflection.__Type_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Type";
            _Reflection.__Object_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Object";
            let _Element;
            (function (_Element) {
                _Element.metaclass = "metaclass";
            })(_Element = _Reflection._Element || (_Reflection._Element = {}));
            _Reflection.__Element_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Element";
        })(_Reflection = _MOF._Reflection || (_MOF._Reflection = {}));
    })(_MOF = exports._MOF || (exports._MOF = {}));
});
//# sourceMappingURL=mof.js.map