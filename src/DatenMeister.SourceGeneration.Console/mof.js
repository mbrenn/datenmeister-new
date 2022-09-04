"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports._MOF = void 0;
// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.0.0.0
var _MOF;
(function (_MOF) {
    var _Identifiers;
    (function (_Identifiers) {
        var _URIExtent = /** @class */ (function () {
            function _URIExtent() {
            }
            return _URIExtent;
        }());
        _Identifiers._URIExtent = _URIExtent;
        _Identifiers.__URIExtent_Uri = "dm:///_internal/model/mof#_MOF-Identifiers-URIExtent";
        var _Extent = /** @class */ (function () {
            function _Extent() {
            }
            return _Extent;
        }());
        _Identifiers._Extent = _Extent;
        _Identifiers.__Extent_Uri = "dm:///_internal/model/mof#_MOF-Identifiers-Extent";
    })(_Identifiers = _MOF._Identifiers || (_MOF._Identifiers = {}));
    var _CMOFExtension;
    (function (_CMOFExtension) {
        var _Tag = /** @class */ (function () {
            function _Tag() {
            }
            _Tag.tagOwner = "tagOwner";
            return _Tag;
        }());
        _CMOFExtension._Tag = _Tag;
        _CMOFExtension.__Tag_Uri = "dm:///_internal/model/mof#_MOF-CMOFExtension-Tag";
    })(_CMOFExtension = _MOF._CMOFExtension || (_MOF._CMOFExtension = {}));
    var _Extension;
    (function (_Extension) {
        var _Tag = /** @class */ (function () {
            function _Tag() {
            }
            _Tag._name_ = "name";
            _Tag.value = "value";
            _Tag.element = "element";
            _Tag.metaclass = "metaclass";
            return _Tag;
        }());
        _Extension._Tag = _Tag;
        _Extension.__Tag_Uri = "dm:///_internal/model/mof#_MOF-Extension-Tag";
    })(_Extension = _MOF._Extension || (_MOF._Extension = {}));
    var _Common;
    (function (_Common) {
        var _ReflectiveSequence = /** @class */ (function () {
            function _ReflectiveSequence() {
            }
            return _ReflectiveSequence;
        }());
        _Common._ReflectiveSequence = _ReflectiveSequence;
        _Common.__ReflectiveSequence_Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence";
        var _ReflectiveCollection = /** @class */ (function () {
            function _ReflectiveCollection() {
            }
            return _ReflectiveCollection;
        }());
        _Common._ReflectiveCollection = _ReflectiveCollection;
        _Common.__ReflectiveCollection_Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection";
    })(_Common = _MOF._Common || (_MOF._Common = {}));
    var _CMOFReflection;
    (function (_CMOFReflection) {
        var _Factory = /** @class */ (function () {
            function _Factory() {
            }
            return _Factory;
        }());
        _CMOFReflection._Factory = _Factory;
        _CMOFReflection.__Factory_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Factory";
        var _Element = /** @class */ (function () {
            function _Element() {
            }
            return _Element;
        }());
        _CMOFReflection._Element = _Element;
        _CMOFReflection.__Element_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Element";
        var _Argument = /** @class */ (function () {
            function _Argument() {
            }
            _Argument._name_ = "name";
            _Argument.value = "value";
            return _Argument;
        }());
        _CMOFReflection._Argument = _Argument;
        _CMOFReflection.__Argument_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Argument";
        var _Extent = /** @class */ (function () {
            function _Extent() {
            }
            return _Extent;
        }());
        _CMOFReflection._Extent = _Extent;
        _CMOFReflection.__Extent_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Extent";
        var _Link = /** @class */ (function () {
            function _Link() {
            }
            _Link.firstElement = "firstElement";
            _Link.secondElement = "secondElement";
            _Link.association = "association";
            return _Link;
        }());
        _CMOFReflection._Link = _Link;
        _CMOFReflection.__Link_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Link";
        var _Exception = /** @class */ (function () {
            function _Exception() {
            }
            _Exception.objectInError = "objectInError";
            _Exception.elementInError = "elementInError";
            _Exception.description = "description";
            return _Exception;
        }());
        _CMOFReflection._Exception = _Exception;
        _CMOFReflection.__Exception_Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Exception";
    })(_CMOFReflection = _MOF._CMOFReflection || (_MOF._CMOFReflection = {}));
    var _Reflection;
    (function (_Reflection) {
        var _Factory = /** @class */ (function () {
            function _Factory() {
            }
            _Factory._package_ = "package";
            _Factory.metaclass = "metaclass";
            return _Factory;
        }());
        _Reflection._Factory = _Factory;
        _Reflection.__Factory_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Factory";
        var _Type = /** @class */ (function () {
            function _Type() {
            }
            return _Type;
        }());
        _Reflection._Type = _Type;
        _Reflection.__Type_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Type";
        var _Object = /** @class */ (function () {
            function _Object() {
            }
            return _Object;
        }());
        _Reflection._Object = _Object;
        _Reflection.__Object_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Object";
        var _Element = /** @class */ (function () {
            function _Element() {
            }
            _Element.metaclass = "metaclass";
            return _Element;
        }());
        _Reflection._Element = _Element;
        _Reflection.__Element_Uri = "dm:///_internal/model/mof#_MOF-Reflection-Element";
    })(_Reflection = _MOF._Reflection || (_MOF._Reflection = {}));
})(_MOF = exports._MOF || (exports._MOF = {}));
