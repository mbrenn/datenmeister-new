using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Core.Models.EMOF;

public class MOF
{
    public class Identifiers
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Identifiers-URIExtent",
            TypeKind = TypeKind.WrappedClass)]
        public class URIExtent_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public URIExtent_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public URIExtent_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Identifiers-URIExtent");

            public static URIExtent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Identifiers-Extent",
            TypeKind = TypeKind.WrappedClass)]
        public class Extent_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Extent_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Extent_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Identifiers-Extent");

            public static Extent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

    }

    public class EMOF
    {
    }

    public class CMOFExtension
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFExtension-Tag",
            TypeKind = TypeKind.WrappedClass)]
        public class Tag_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Tag_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Tag_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFExtension-Tag");

            public static Tag_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper? @tagOwner
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("tagOwner");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("tagOwner", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("tagOwner", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Comment_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Comment_Wrapper? @ownedComment
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("ownedComment");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Comment_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("ownedComment", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("ownedComment", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper? @ownedElement
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("ownedElement");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("ownedElement", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("ownedElement", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper? @owner
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("owner");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("owner", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("owner", value);
                    }
                }
            }

        }

    }

    public class Extension
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Extension-Tag",
            TypeKind = TypeKind.WrappedClass)]
        public class Tag_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Tag_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Tag_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Extension-Tag");

            public static Tag_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public string? @value
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("value");
                set => 
                    _wrappedElement.set("value", value);
            }

            // Not found
            public object? @element
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("element");
                set => 
                    _wrappedElement.set("element", value);
            }

        }

    }

    public class Common
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence",
            TypeKind = TypeKind.WrappedClass)]
        public class ReflectiveSequence_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReflectiveSequence_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReflectiveSequence_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence");

            public static ReflectiveSequence_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection",
            TypeKind = TypeKind.WrappedClass)]
        public class ReflectiveCollection_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReflectiveCollection_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReflectiveCollection_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection");

            public static ReflectiveCollection_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

    }

    public class CMOF
    {
    }

    public class CMOFReflection
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Factory",
            TypeKind = TypeKind.WrappedClass)]
        public class Factory_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Factory_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Factory_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Factory");

            public static Factory_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Element",
            TypeKind = TypeKind.WrappedClass)]
        public class Element_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Element_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Element_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Element");

            public static Element_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Argument",
            TypeKind = TypeKind.WrappedClass)]
        public class Argument_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Argument_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Argument_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument");

            public static Argument_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            // Not found
            public object? @value
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("value");
                set => 
                    _wrappedElement.set("value", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Extent",
            TypeKind = TypeKind.WrappedClass)]
        public class Extent_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Extent_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Extent_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Extent");

            public static Extent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Link",
            TypeKind = TypeKind.WrappedClass)]
        public class Link_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Link_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Link_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Link");

            public static Link_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // Not found
            public object? @firstElement
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("firstElement");
                set => 
                    _wrappedElement.set("firstElement", value);
            }

            // Not found
            public object? @secondElement
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("secondElement");
                set => 
                    _wrappedElement.set("secondElement", value);
            }

            // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Association_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Association_Wrapper? @association
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("association");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Association_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("association", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("association", value);
                    }
                }
            }

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Exception",
            TypeKind = TypeKind.WrappedClass)]
        public class Exception_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Exception_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Exception_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception");

            public static Exception_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // Not found
            public object? @objectInError
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("objectInError");
                set => 
                    _wrappedElement.set("objectInError", value);
            }

            // Not found
            public object? @elementInError
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("elementInError");
                set => 
                    _wrappedElement.set("elementInError", value);
            }

            public string? @description
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("description");
                set => 
                    _wrappedElement.set("description", value);
            }

        }

    }

    public class Reflection
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Factory",
            TypeKind = TypeKind.WrappedClass)]
        public class Factory_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Factory_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Factory_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Factory");

            public static Factory_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper? @package
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("package");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("package", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("package", value);
                    }
                }
            }

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Type",
            TypeKind = TypeKind.WrappedClass)]
        public class Type_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Type_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Type_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Type");

            public static Type_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Object",
            TypeKind = TypeKind.WrappedClass)]
        public class Object_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Object_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Object_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Object");

            public static Object_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Element",
            TypeKind = TypeKind.WrappedClass)]
        public class Element_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Element_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Element_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Element");

            public static Element_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaclass
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("metaclass");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("metaclass", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("metaclass", value);
                    }
                }
            }

        }

    }

}

