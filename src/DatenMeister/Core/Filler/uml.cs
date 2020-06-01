#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Core
{
    public class _UML
    {
        public class _Activities
        {
            public class _Activity
            {
                public static string @edge = "edge";
                public IElement? _edge = null;

                public static string @group = "group";
                public IElement? _group = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? _isReadOnly = null;

                public static string @isSingleExecution = "isSingleExecution";
                public IElement? _isSingleExecution = null;

                public static string @node = "node";
                public IElement? _node = null;

                public static string @partition = "partition";
                public IElement? _partition = null;

                public static string @structuredNode = "structuredNode";
                public IElement? _structuredNode = null;

                public static string @variable = "variable";
                public IElement? _variable = null;

            }

            public _Activity @Activity = new _Activity();
            public IElement @__Activity = new MofObjectShadow("dm:///_internal/xmi/uml#Activity");

            public class _ActivityEdge
            {
                public static string @activity = "activity";
                public IElement? _activity = null;

                public static string @guard = "guard";
                public IElement? _guard = null;

                public static string @inGroup = "inGroup";
                public IElement? _inGroup = null;

                public static string @inPartition = "inPartition";
                public IElement? _inPartition = null;

                public static string @inStructuredNode = "inStructuredNode";
                public IElement? _inStructuredNode = null;

                public static string @interrupts = "interrupts";
                public IElement? _interrupts = null;

                public static string @redefinedEdge = "redefinedEdge";
                public IElement? _redefinedEdge = null;

                public static string @source = "source";
                public IElement? _source = null;

                public static string @target = "target";
                public IElement? _target = null;

                public static string @weight = "weight";
                public IElement? _weight = null;

            }

            public _ActivityEdge @ActivityEdge = new _ActivityEdge();
            public IElement @__ActivityEdge = new MofObjectShadow("dm:///_internal/xmi/uml#ActivityEdge");

            public class _ActivityFinalNode
            {
            }

            public _ActivityFinalNode @ActivityFinalNode = new _ActivityFinalNode();
            public IElement @__ActivityFinalNode = new MofObjectShadow("dm:///_internal/xmi/uml#ActivityFinalNode");

            public class _ActivityGroup
            {
                public static string @containedEdge = "containedEdge";
                public IElement? _containedEdge = null;

                public static string @containedNode = "containedNode";
                public IElement? _containedNode = null;

                public static string @inActivity = "inActivity";
                public IElement? _inActivity = null;

                public static string @subgroup = "subgroup";
                public IElement? _subgroup = null;

                public static string @superGroup = "superGroup";
                public IElement? _superGroup = null;

            }

            public _ActivityGroup @ActivityGroup = new _ActivityGroup();
            public IElement @__ActivityGroup = new MofObjectShadow("dm:///_internal/xmi/uml#ActivityGroup");

            public class _ActivityNode
            {
                public static string @activity = "activity";
                public IElement? _activity = null;

                public static string @inGroup = "inGroup";
                public IElement? _inGroup = null;

                public static string @inInterruptibleRegion = "inInterruptibleRegion";
                public IElement? _inInterruptibleRegion = null;

                public static string @inPartition = "inPartition";
                public IElement? _inPartition = null;

                public static string @inStructuredNode = "inStructuredNode";
                public IElement? _inStructuredNode = null;

                public static string @incoming = "incoming";
                public IElement? _incoming = null;

                public static string @outgoing = "outgoing";
                public IElement? _outgoing = null;

                public static string @redefinedNode = "redefinedNode";
                public IElement? _redefinedNode = null;

            }

            public _ActivityNode @ActivityNode = new _ActivityNode();
            public IElement @__ActivityNode = new MofObjectShadow("dm:///_internal/xmi/uml#ActivityNode");

            public class _ActivityParameterNode
            {
                public static string @parameter = "parameter";
                public IElement? _parameter = null;

            }

            public _ActivityParameterNode @ActivityParameterNode = new _ActivityParameterNode();
            public IElement @__ActivityParameterNode = new MofObjectShadow("dm:///_internal/xmi/uml#ActivityParameterNode");

            public class _ActivityPartition
            {
                public static string @edge = "edge";
                public IElement? _edge = null;

                public static string @isDimension = "isDimension";
                public IElement? _isDimension = null;

                public static string @isExternal = "isExternal";
                public IElement? _isExternal = null;

                public static string @node = "node";
                public IElement? _node = null;

                public static string @represents = "represents";
                public IElement? _represents = null;

                public static string @subpartition = "subpartition";
                public IElement? _subpartition = null;

                public static string @superPartition = "superPartition";
                public IElement? _superPartition = null;

            }

            public _ActivityPartition @ActivityPartition = new _ActivityPartition();
            public IElement @__ActivityPartition = new MofObjectShadow("dm:///_internal/xmi/uml#ActivityPartition");

            public class _CentralBufferNode
            {
            }

            public _CentralBufferNode @CentralBufferNode = new _CentralBufferNode();
            public IElement @__CentralBufferNode = new MofObjectShadow("dm:///_internal/xmi/uml#CentralBufferNode");

            public class _ControlFlow
            {
            }

            public _ControlFlow @ControlFlow = new _ControlFlow();
            public IElement @__ControlFlow = new MofObjectShadow("dm:///_internal/xmi/uml#ControlFlow");

            public class _ControlNode
            {
            }

            public _ControlNode @ControlNode = new _ControlNode();
            public IElement @__ControlNode = new MofObjectShadow("dm:///_internal/xmi/uml#ControlNode");

            public class _DataStoreNode
            {
            }

            public _DataStoreNode @DataStoreNode = new _DataStoreNode();
            public IElement @__DataStoreNode = new MofObjectShadow("dm:///_internal/xmi/uml#DataStoreNode");

            public class _DecisionNode
            {
                public static string @decisionInput = "decisionInput";
                public IElement? _decisionInput = null;

                public static string @decisionInputFlow = "decisionInputFlow";
                public IElement? _decisionInputFlow = null;

            }

            public _DecisionNode @DecisionNode = new _DecisionNode();
            public IElement @__DecisionNode = new MofObjectShadow("dm:///_internal/xmi/uml#DecisionNode");

            public class _ExceptionHandler
            {
                public static string @exceptionInput = "exceptionInput";
                public IElement? _exceptionInput = null;

                public static string @exceptionType = "exceptionType";
                public IElement? _exceptionType = null;

                public static string @handlerBody = "handlerBody";
                public IElement? _handlerBody = null;

                public static string @protectedNode = "protectedNode";
                public IElement? _protectedNode = null;

            }

            public _ExceptionHandler @ExceptionHandler = new _ExceptionHandler();
            public IElement @__ExceptionHandler = new MofObjectShadow("dm:///_internal/xmi/uml#ExceptionHandler");

            public class _ExecutableNode
            {
                public static string @handler = "handler";
                public IElement? _handler = null;

            }

            public _ExecutableNode @ExecutableNode = new _ExecutableNode();
            public IElement @__ExecutableNode = new MofObjectShadow("dm:///_internal/xmi/uml#ExecutableNode");

            public class _FinalNode
            {
            }

            public _FinalNode @FinalNode = new _FinalNode();
            public IElement @__FinalNode = new MofObjectShadow("dm:///_internal/xmi/uml#FinalNode");

            public class _FlowFinalNode
            {
            }

            public _FlowFinalNode @FlowFinalNode = new _FlowFinalNode();
            public IElement @__FlowFinalNode = new MofObjectShadow("dm:///_internal/xmi/uml#FlowFinalNode");

            public class _ForkNode
            {
            }

            public _ForkNode @ForkNode = new _ForkNode();
            public IElement @__ForkNode = new MofObjectShadow("dm:///_internal/xmi/uml#ForkNode");

            public class _InitialNode
            {
            }

            public _InitialNode @InitialNode = new _InitialNode();
            public IElement @__InitialNode = new MofObjectShadow("dm:///_internal/xmi/uml#InitialNode");

            public class _InterruptibleActivityRegion
            {
                public static string @interruptingEdge = "interruptingEdge";
                public IElement? _interruptingEdge = null;

                public static string @node = "node";
                public IElement? _node = null;

            }

            public _InterruptibleActivityRegion @InterruptibleActivityRegion = new _InterruptibleActivityRegion();
            public IElement @__InterruptibleActivityRegion = new MofObjectShadow("dm:///_internal/xmi/uml#InterruptibleActivityRegion");

            public class _JoinNode
            {
                public static string @isCombineDuplicate = "isCombineDuplicate";
                public IElement? _isCombineDuplicate = null;

                public static string @joinSpec = "joinSpec";
                public IElement? _joinSpec = null;

            }

            public _JoinNode @JoinNode = new _JoinNode();
            public IElement @__JoinNode = new MofObjectShadow("dm:///_internal/xmi/uml#JoinNode");

            public class _MergeNode
            {
            }

            public _MergeNode @MergeNode = new _MergeNode();
            public IElement @__MergeNode = new MofObjectShadow("dm:///_internal/xmi/uml#MergeNode");

            public class _ObjectFlow
            {
                public static string @isMulticast = "isMulticast";
                public IElement? _isMulticast = null;

                public static string @isMultireceive = "isMultireceive";
                public IElement? _isMultireceive = null;

                public static string @selection = "selection";
                public IElement? _selection = null;

                public static string @transformation = "transformation";
                public IElement? _transformation = null;

            }

            public _ObjectFlow @ObjectFlow = new _ObjectFlow();
            public IElement @__ObjectFlow = new MofObjectShadow("dm:///_internal/xmi/uml#ObjectFlow");

            public class _ObjectNode
            {
                public static string @inState = "inState";
                public IElement? _inState = null;

                public static string @isControlType = "isControlType";
                public IElement? _isControlType = null;

                public static string @ordering = "ordering";
                public IElement? _ordering = null;

                public static string @selection = "selection";
                public IElement? _selection = null;

                public static string @upperBound = "upperBound";
                public IElement? _upperBound = null;

            }

            public _ObjectNode @ObjectNode = new _ObjectNode();
            public IElement @__ObjectNode = new MofObjectShadow("dm:///_internal/xmi/uml#ObjectNode");

            public class _Variable
            {
                public static string @activityScope = "activityScope";
                public IElement? _activityScope = null;

                public static string @scope = "scope";
                public IElement? _scope = null;

            }

            public _Variable @Variable = new _Variable();
            public IElement @__Variable = new MofObjectShadow("dm:///_internal/xmi/uml#Variable");

            public class _ObjectNodeOrderingKind
            {
                public static string @unordered = "unordered";
                public IElement @__unordered = new MofObjectShadow("dm:///_internal/xmi/uml#ObjectNodeOrderingKind-unordered");
                public static string @ordered = "ordered";
                public IElement @__ordered = new MofObjectShadow("dm:///_internal/xmi/uml#ObjectNodeOrderingKind-ordered");
                public static string @LIFO = "LIFO";
                public IElement @__LIFO = new MofObjectShadow("dm:///_internal/xmi/uml#ObjectNodeOrderingKind-LIFO");
                public static string @FIFO = "FIFO";
                public IElement @__FIFO = new MofObjectShadow("dm:///_internal/xmi/uml#ObjectNodeOrderingKind-FIFO");

            }

            public _ObjectNodeOrderingKind @ObjectNodeOrderingKind = new _ObjectNodeOrderingKind();
            public IElement @__ObjectNodeOrderingKind = new MofObjectShadow("dm:///_internal/xmi/uml#ObjectNodeOrderingKind");

        }

        public _Activities Activities = new _Activities();

        public class _Values
        {
            public class _Duration
            {
                public static string @expr = "expr";
                public IElement? _expr = null;

                public static string @observation = "observation";
                public IElement? _observation = null;

            }

            public _Duration @Duration = new _Duration();
            public IElement @__Duration = new MofObjectShadow("dm:///_internal/xmi/uml#Duration");

            public class _DurationConstraint
            {
                public static string @firstEvent = "firstEvent";
                public IElement? _firstEvent = null;

                public static string @specification = "specification";
                public IElement? _specification = null;

            }

            public _DurationConstraint @DurationConstraint = new _DurationConstraint();
            public IElement @__DurationConstraint = new MofObjectShadow("dm:///_internal/xmi/uml#DurationConstraint");

            public class _DurationInterval
            {
                public static string @max = "max";
                public IElement? _max = null;

                public static string @min = "min";
                public IElement? _min = null;

            }

            public _DurationInterval @DurationInterval = new _DurationInterval();
            public IElement @__DurationInterval = new MofObjectShadow("dm:///_internal/xmi/uml#DurationInterval");

            public class _DurationObservation
            {
                public static string @event = "event";
                public IElement? _event = null;

                public static string @firstEvent = "firstEvent";
                public IElement? _firstEvent = null;

            }

            public _DurationObservation @DurationObservation = new _DurationObservation();
            public IElement @__DurationObservation = new MofObjectShadow("dm:///_internal/xmi/uml#DurationObservation");

            public class _Expression
            {
                public static string @operand = "operand";
                public IElement? _operand = null;

                public static string @symbol = "symbol";
                public IElement? _symbol = null;

            }

            public _Expression @Expression = new _Expression();
            public IElement @__Expression = new MofObjectShadow("dm:///_internal/xmi/uml#Expression");

            public class _Interval
            {
                public static string @max = "max";
                public IElement? _max = null;

                public static string @min = "min";
                public IElement? _min = null;

            }

            public _Interval @Interval = new _Interval();
            public IElement @__Interval = new MofObjectShadow("dm:///_internal/xmi/uml#Interval");

            public class _IntervalConstraint
            {
                public static string @specification = "specification";
                public IElement? _specification = null;

            }

            public _IntervalConstraint @IntervalConstraint = new _IntervalConstraint();
            public IElement @__IntervalConstraint = new MofObjectShadow("dm:///_internal/xmi/uml#IntervalConstraint");

            public class _LiteralBoolean
            {
                public static string @value = "value";
                public IElement? _value = null;

            }

            public _LiteralBoolean @LiteralBoolean = new _LiteralBoolean();
            public IElement @__LiteralBoolean = new MofObjectShadow("dm:///_internal/xmi/uml#LiteralBoolean");

            public class _LiteralInteger
            {
                public static string @value = "value";
                public IElement? _value = null;

            }

            public _LiteralInteger @LiteralInteger = new _LiteralInteger();
            public IElement @__LiteralInteger = new MofObjectShadow("dm:///_internal/xmi/uml#LiteralInteger");

            public class _LiteralNull
            {
            }

            public _LiteralNull @LiteralNull = new _LiteralNull();
            public IElement @__LiteralNull = new MofObjectShadow("dm:///_internal/xmi/uml#LiteralNull");

            public class _LiteralReal
            {
                public static string @value = "value";
                public IElement? _value = null;

            }

            public _LiteralReal @LiteralReal = new _LiteralReal();
            public IElement @__LiteralReal = new MofObjectShadow("dm:///_internal/xmi/uml#LiteralReal");

            public class _LiteralSpecification
            {
            }

            public _LiteralSpecification @LiteralSpecification = new _LiteralSpecification();
            public IElement @__LiteralSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#LiteralSpecification");

            public class _LiteralString
            {
                public static string @value = "value";
                public IElement? _value = null;

            }

            public _LiteralString @LiteralString = new _LiteralString();
            public IElement @__LiteralString = new MofObjectShadow("dm:///_internal/xmi/uml#LiteralString");

            public class _LiteralUnlimitedNatural
            {
                public static string @value = "value";
                public IElement? _value = null;

            }

            public _LiteralUnlimitedNatural @LiteralUnlimitedNatural = new _LiteralUnlimitedNatural();
            public IElement @__LiteralUnlimitedNatural = new MofObjectShadow("dm:///_internal/xmi/uml#LiteralUnlimitedNatural");

            public class _Observation
            {
            }

            public _Observation @Observation = new _Observation();
            public IElement @__Observation = new MofObjectShadow("dm:///_internal/xmi/uml#Observation");

            public class _OpaqueExpression
            {
                public static string @behavior = "behavior";
                public IElement? _behavior = null;

                public static string @body = "body";
                public IElement? _body = null;

                public static string @language = "language";
                public IElement? _language = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _OpaqueExpression @OpaqueExpression = new _OpaqueExpression();
            public IElement @__OpaqueExpression = new MofObjectShadow("dm:///_internal/xmi/uml#OpaqueExpression");

            public class _StringExpression
            {
                public static string @owningExpression = "owningExpression";
                public IElement? _owningExpression = null;

                public static string @subExpression = "subExpression";
                public IElement? _subExpression = null;

            }

            public _StringExpression @StringExpression = new _StringExpression();
            public IElement @__StringExpression = new MofObjectShadow("dm:///_internal/xmi/uml#StringExpression");

            public class _TimeConstraint
            {
                public static string @firstEvent = "firstEvent";
                public IElement? _firstEvent = null;

                public static string @specification = "specification";
                public IElement? _specification = null;

            }

            public _TimeConstraint @TimeConstraint = new _TimeConstraint();
            public IElement @__TimeConstraint = new MofObjectShadow("dm:///_internal/xmi/uml#TimeConstraint");

            public class _TimeExpression
            {
                public static string @expr = "expr";
                public IElement? _expr = null;

                public static string @observation = "observation";
                public IElement? _observation = null;

            }

            public _TimeExpression @TimeExpression = new _TimeExpression();
            public IElement @__TimeExpression = new MofObjectShadow("dm:///_internal/xmi/uml#TimeExpression");

            public class _TimeInterval
            {
                public static string @max = "max";
                public IElement? _max = null;

                public static string @min = "min";
                public IElement? _min = null;

            }

            public _TimeInterval @TimeInterval = new _TimeInterval();
            public IElement @__TimeInterval = new MofObjectShadow("dm:///_internal/xmi/uml#TimeInterval");

            public class _TimeObservation
            {
                public static string @event = "event";
                public IElement? _event = null;

                public static string @firstEvent = "firstEvent";
                public IElement? _firstEvent = null;

            }

            public _TimeObservation @TimeObservation = new _TimeObservation();
            public IElement @__TimeObservation = new MofObjectShadow("dm:///_internal/xmi/uml#TimeObservation");

            public class _ValueSpecification
            {
            }

            public _ValueSpecification @ValueSpecification = new _ValueSpecification();
            public IElement @__ValueSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#ValueSpecification");

        }

        public _Values Values = new _Values();

        public class _UseCases
        {
            public class _Actor
            {
            }

            public _Actor @Actor = new _Actor();
            public IElement @__Actor = new MofObjectShadow("dm:///_internal/xmi/uml#Actor");

            public class _Extend
            {
                public static string @condition = "condition";
                public IElement? _condition = null;

                public static string @extendedCase = "extendedCase";
                public IElement? _extendedCase = null;

                public static string @extension = "extension";
                public IElement? _extension = null;

                public static string @extensionLocation = "extensionLocation";
                public IElement? _extensionLocation = null;

            }

            public _Extend @Extend = new _Extend();
            public IElement @__Extend = new MofObjectShadow("dm:///_internal/xmi/uml#Extend");

            public class _ExtensionPoint
            {
                public static string @useCase = "useCase";
                public IElement? _useCase = null;

            }

            public _ExtensionPoint @ExtensionPoint = new _ExtensionPoint();
            public IElement @__ExtensionPoint = new MofObjectShadow("dm:///_internal/xmi/uml#ExtensionPoint");

            public class _Include
            {
                public static string @addition = "addition";
                public IElement? _addition = null;

                public static string @includingCase = "includingCase";
                public IElement? _includingCase = null;

            }

            public _Include @Include = new _Include();
            public IElement @__Include = new MofObjectShadow("dm:///_internal/xmi/uml#Include");

            public class _UseCase
            {
                public static string @extend = "extend";
                public IElement? _extend = null;

                public static string @extensionPoint = "extensionPoint";
                public IElement? _extensionPoint = null;

                public static string @include = "include";
                public IElement? _include = null;

                public static string @subject = "subject";
                public IElement? _subject = null;

            }

            public _UseCase @UseCase = new _UseCase();
            public IElement @__UseCase = new MofObjectShadow("dm:///_internal/xmi/uml#UseCase");

        }

        public _UseCases UseCases = new _UseCases();

        public class _StructuredClassifiers
        {
            public class _Association
            {
                public static string @endType = "endType";
                public IElement? _endType = null;

                public static string @isDerived = "isDerived";
                public IElement? _isDerived = null;

                public static string @memberEnd = "memberEnd";
                public IElement? _memberEnd = null;

                public static string @navigableOwnedEnd = "navigableOwnedEnd";
                public IElement? _navigableOwnedEnd = null;

                public static string @ownedEnd = "ownedEnd";
                public IElement? _ownedEnd = null;

            }

            public _Association @Association = new _Association();
            public IElement @__Association = new MofObjectShadow("dm:///_internal/xmi/uml#Association");

            public class _AssociationClass
            {
            }

            public _AssociationClass @AssociationClass = new _AssociationClass();
            public IElement @__AssociationClass = new MofObjectShadow("dm:///_internal/xmi/uml#AssociationClass");

            public class _Class
            {
                public static string @extension = "extension";
                public IElement? _extension = null;

                public static string @isAbstract = "isAbstract";
                public IElement? _isAbstract = null;

                public static string @isActive = "isActive";
                public IElement? _isActive = null;

                public static string @nestedClassifier = "nestedClassifier";
                public IElement? _nestedClassifier = null;

                public static string @ownedAttribute = "ownedAttribute";
                public IElement? _ownedAttribute = null;

                public static string @ownedOperation = "ownedOperation";
                public IElement? _ownedOperation = null;

                public static string @ownedReception = "ownedReception";
                public IElement? _ownedReception = null;

                public static string @superClass = "superClass";
                public IElement? _superClass = null;

            }

            public _Class @Class = new _Class();
            public IElement @__Class = new MofObjectShadow("dm:///_internal/xmi/uml#Class");

            public class _Collaboration
            {
                public static string @collaborationRole = "collaborationRole";
                public IElement? _collaborationRole = null;

            }

            public _Collaboration @Collaboration = new _Collaboration();
            public IElement @__Collaboration = new MofObjectShadow("dm:///_internal/xmi/uml#Collaboration");

            public class _CollaborationUse
            {
                public static string @roleBinding = "roleBinding";
                public IElement? _roleBinding = null;

                public static string @type = "type";
                public IElement? _type = null;

            }

            public _CollaborationUse @CollaborationUse = new _CollaborationUse();
            public IElement @__CollaborationUse = new MofObjectShadow("dm:///_internal/xmi/uml#CollaborationUse");

            public class _Component
            {
                public static string @isIndirectlyInstantiated = "isIndirectlyInstantiated";
                public IElement? _isIndirectlyInstantiated = null;

                public static string @packagedElement = "packagedElement";
                public IElement? _packagedElement = null;

                public static string @provided = "provided";
                public IElement? _provided = null;

                public static string @realization = "realization";
                public IElement? _realization = null;

                public static string @required = "required";
                public IElement? _required = null;

            }

            public _Component @Component = new _Component();
            public IElement @__Component = new MofObjectShadow("dm:///_internal/xmi/uml#Component");

            public class _ComponentRealization
            {
                public static string @abstraction = "abstraction";
                public IElement? _abstraction = null;

                public static string @realizingClassifier = "realizingClassifier";
                public IElement? _realizingClassifier = null;

            }

            public _ComponentRealization @ComponentRealization = new _ComponentRealization();
            public IElement @__ComponentRealization = new MofObjectShadow("dm:///_internal/xmi/uml#ComponentRealization");

            public class _ConnectableElement
            {
                public static string @end = "end";
                public IElement? _end = null;

                public static string @templateParameter = "templateParameter";
                public IElement? _templateParameter = null;

            }

            public _ConnectableElement @ConnectableElement = new _ConnectableElement();
            public IElement @__ConnectableElement = new MofObjectShadow("dm:///_internal/xmi/uml#ConnectableElement");

            public class _ConnectableElementTemplateParameter
            {
                public static string @parameteredElement = "parameteredElement";
                public IElement? _parameteredElement = null;

            }

            public _ConnectableElementTemplateParameter @ConnectableElementTemplateParameter = new _ConnectableElementTemplateParameter();
            public IElement @__ConnectableElementTemplateParameter = new MofObjectShadow("dm:///_internal/xmi/uml#ConnectableElementTemplateParameter");

            public class _Connector
            {
                public static string @contract = "contract";
                public IElement? _contract = null;

                public static string @end = "end";
                public IElement? _end = null;

                public static string @kind = "kind";
                public IElement? _kind = null;

                public static string @redefinedConnector = "redefinedConnector";
                public IElement? _redefinedConnector = null;

                public static string @type = "type";
                public IElement? _type = null;

            }

            public _Connector @Connector = new _Connector();
            public IElement @__Connector = new MofObjectShadow("dm:///_internal/xmi/uml#Connector");

            public class _ConnectorEnd
            {
                public static string @definingEnd = "definingEnd";
                public IElement? _definingEnd = null;

                public static string @partWithPort = "partWithPort";
                public IElement? _partWithPort = null;

                public static string @role = "role";
                public IElement? _role = null;

            }

            public _ConnectorEnd @ConnectorEnd = new _ConnectorEnd();
            public IElement @__ConnectorEnd = new MofObjectShadow("dm:///_internal/xmi/uml#ConnectorEnd");

            public class _EncapsulatedClassifier
            {
                public static string @ownedPort = "ownedPort";
                public IElement? _ownedPort = null;

            }

            public _EncapsulatedClassifier @EncapsulatedClassifier = new _EncapsulatedClassifier();
            public IElement @__EncapsulatedClassifier = new MofObjectShadow("dm:///_internal/xmi/uml#EncapsulatedClassifier");

            public class _Port
            {
                public static string @isBehavior = "isBehavior";
                public IElement? _isBehavior = null;

                public static string @isConjugated = "isConjugated";
                public IElement? _isConjugated = null;

                public static string @isService = "isService";
                public IElement? _isService = null;

                public static string @protocol = "protocol";
                public IElement? _protocol = null;

                public static string @provided = "provided";
                public IElement? _provided = null;

                public static string @redefinedPort = "redefinedPort";
                public IElement? _redefinedPort = null;

                public static string @required = "required";
                public IElement? _required = null;

            }

            public _Port @Port = new _Port();
            public IElement @__Port = new MofObjectShadow("dm:///_internal/xmi/uml#Port");

            public class _StructuredClassifier
            {
                public static string @ownedAttribute = "ownedAttribute";
                public IElement? _ownedAttribute = null;

                public static string @ownedConnector = "ownedConnector";
                public IElement? _ownedConnector = null;

                public static string @part = "part";
                public IElement? _part = null;

                public static string @role = "role";
                public IElement? _role = null;

            }

            public _StructuredClassifier @StructuredClassifier = new _StructuredClassifier();
            public IElement @__StructuredClassifier = new MofObjectShadow("dm:///_internal/xmi/uml#StructuredClassifier");

            public class _ConnectorKind
            {
                public static string @assembly = "assembly";
                public IElement @__assembly = new MofObjectShadow("dm:///_internal/xmi/uml#ConnectorKind-assembly");
                public static string @delegation = "delegation";
                public IElement @__delegation = new MofObjectShadow("dm:///_internal/xmi/uml#ConnectorKind-delegation");

            }

            public _ConnectorKind @ConnectorKind = new _ConnectorKind();
            public IElement @__ConnectorKind = new MofObjectShadow("dm:///_internal/xmi/uml#ConnectorKind");

        }

        public _StructuredClassifiers StructuredClassifiers = new _StructuredClassifiers();

        public class _StateMachines
        {
            public class _ConnectionPointReference
            {
                public static string @entry = "entry";
                public IElement? _entry = null;

                public static string @exit = "exit";
                public IElement? _exit = null;

                public static string @state = "state";
                public IElement? _state = null;

            }

            public _ConnectionPointReference @ConnectionPointReference = new _ConnectionPointReference();
            public IElement @__ConnectionPointReference = new MofObjectShadow("dm:///_internal/xmi/uml#ConnectionPointReference");

            public class _FinalState
            {
            }

            public _FinalState @FinalState = new _FinalState();
            public IElement @__FinalState = new MofObjectShadow("dm:///_internal/xmi/uml#FinalState");

            public class _ProtocolConformance
            {
                public static string @generalMachine = "generalMachine";
                public IElement? _generalMachine = null;

                public static string @specificMachine = "specificMachine";
                public IElement? _specificMachine = null;

            }

            public _ProtocolConformance @ProtocolConformance = new _ProtocolConformance();
            public IElement @__ProtocolConformance = new MofObjectShadow("dm:///_internal/xmi/uml#ProtocolConformance");

            public class _ProtocolStateMachine
            {
                public static string @conformance = "conformance";
                public IElement? _conformance = null;

            }

            public _ProtocolStateMachine @ProtocolStateMachine = new _ProtocolStateMachine();
            public IElement @__ProtocolStateMachine = new MofObjectShadow("dm:///_internal/xmi/uml#ProtocolStateMachine");

            public class _ProtocolTransition
            {
                public static string @postCondition = "postCondition";
                public IElement? _postCondition = null;

                public static string @preCondition = "preCondition";
                public IElement? _preCondition = null;

                public static string @referred = "referred";
                public IElement? _referred = null;

            }

            public _ProtocolTransition @ProtocolTransition = new _ProtocolTransition();
            public IElement @__ProtocolTransition = new MofObjectShadow("dm:///_internal/xmi/uml#ProtocolTransition");

            public class _Pseudostate
            {
                public static string @kind = "kind";
                public IElement? _kind = null;

                public static string @state = "state";
                public IElement? _state = null;

                public static string @stateMachine = "stateMachine";
                public IElement? _stateMachine = null;

            }

            public _Pseudostate @Pseudostate = new _Pseudostate();
            public IElement @__Pseudostate = new MofObjectShadow("dm:///_internal/xmi/uml#Pseudostate");

            public class _Region
            {
                public static string @extendedRegion = "extendedRegion";
                public IElement? _extendedRegion = null;

                public static string @redefinitionContext = "redefinitionContext";
                public IElement? _redefinitionContext = null;

                public static string @state = "state";
                public IElement? _state = null;

                public static string @stateMachine = "stateMachine";
                public IElement? _stateMachine = null;

                public static string @subvertex = "subvertex";
                public IElement? _subvertex = null;

                public static string @transition = "transition";
                public IElement? _transition = null;

            }

            public _Region @Region = new _Region();
            public IElement @__Region = new MofObjectShadow("dm:///_internal/xmi/uml#Region");

            public class _State
            {
                public static string @connection = "connection";
                public IElement? _connection = null;

                public static string @connectionPoint = "connectionPoint";
                public IElement? _connectionPoint = null;

                public static string @deferrableTrigger = "deferrableTrigger";
                public IElement? _deferrableTrigger = null;

                public static string @doActivity = "doActivity";
                public IElement? _doActivity = null;

                public static string @entry = "entry";
                public IElement? _entry = null;

                public static string @exit = "exit";
                public IElement? _exit = null;

                public static string @isComposite = "isComposite";
                public IElement? _isComposite = null;

                public static string @isOrthogonal = "isOrthogonal";
                public IElement? _isOrthogonal = null;

                public static string @isSimple = "isSimple";
                public IElement? _isSimple = null;

                public static string @isSubmachineState = "isSubmachineState";
                public IElement? _isSubmachineState = null;

                public static string @redefinedState = "redefinedState";
                public IElement? _redefinedState = null;

                public static string @redefinitionContext = "redefinitionContext";
                public IElement? _redefinitionContext = null;

                public static string @region = "region";
                public IElement? _region = null;

                public static string @stateInvariant = "stateInvariant";
                public IElement? _stateInvariant = null;

                public static string @submachine = "submachine";
                public IElement? _submachine = null;

            }

            public _State @State = new _State();
            public IElement @__State = new MofObjectShadow("dm:///_internal/xmi/uml#State");

            public class _StateMachine
            {
                public static string @connectionPoint = "connectionPoint";
                public IElement? _connectionPoint = null;

                public static string @extendedStateMachine = "extendedStateMachine";
                public IElement? _extendedStateMachine = null;

                public static string @region = "region";
                public IElement? _region = null;

                public static string @submachineState = "submachineState";
                public IElement? _submachineState = null;

            }

            public _StateMachine @StateMachine = new _StateMachine();
            public IElement @__StateMachine = new MofObjectShadow("dm:///_internal/xmi/uml#StateMachine");

            public class _Transition
            {
                public static string @container = "container";
                public IElement? _container = null;

                public static string @effect = "effect";
                public IElement? _effect = null;

                public static string @guard = "guard";
                public IElement? _guard = null;

                public static string @kind = "kind";
                public IElement? _kind = null;

                public static string @redefinedTransition = "redefinedTransition";
                public IElement? _redefinedTransition = null;

                public static string @redefinitionContext = "redefinitionContext";
                public IElement? _redefinitionContext = null;

                public static string @source = "source";
                public IElement? _source = null;

                public static string @target = "target";
                public IElement? _target = null;

                public static string @trigger = "trigger";
                public IElement? _trigger = null;

            }

            public _Transition @Transition = new _Transition();
            public IElement @__Transition = new MofObjectShadow("dm:///_internal/xmi/uml#Transition");

            public class _Vertex
            {
                public static string @container = "container";
                public IElement? _container = null;

                public static string @incoming = "incoming";
                public IElement? _incoming = null;

                public static string @outgoing = "outgoing";
                public IElement? _outgoing = null;

            }

            public _Vertex @Vertex = new _Vertex();
            public IElement @__Vertex = new MofObjectShadow("dm:///_internal/xmi/uml#Vertex");

            public class _PseudostateKind
            {
                public static string @initial = "initial";
                public IElement @__initial = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-initial");
                public static string @deepHistory = "deepHistory";
                public IElement @__deepHistory = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-deepHistory");
                public static string @shallowHistory = "shallowHistory";
                public IElement @__shallowHistory = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-shallowHistory");
                public static string @join = "join";
                public IElement @__join = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-join");
                public static string @fork = "fork";
                public IElement @__fork = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-fork");
                public static string @junction = "junction";
                public IElement @__junction = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-junction");
                public static string @choice = "choice";
                public IElement @__choice = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-choice");
                public static string @entryPoint = "entryPoint";
                public IElement @__entryPoint = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-entryPoint");
                public static string @exitPoint = "exitPoint";
                public IElement @__exitPoint = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-exitPoint");
                public static string @terminate = "terminate";
                public IElement @__terminate = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind-terminate");

            }

            public _PseudostateKind @PseudostateKind = new _PseudostateKind();
            public IElement @__PseudostateKind = new MofObjectShadow("dm:///_internal/xmi/uml#PseudostateKind");

            public class _TransitionKind
            {
                public static string @internal = "internal";
                public IElement @__internal = new MofObjectShadow("dm:///_internal/xmi/uml#TransitionKind-internal");
                public static string @local = "local";
                public IElement @__local = new MofObjectShadow("dm:///_internal/xmi/uml#TransitionKind-local");
                public static string @external = "external";
                public IElement @__external = new MofObjectShadow("dm:///_internal/xmi/uml#TransitionKind-external");

            }

            public _TransitionKind @TransitionKind = new _TransitionKind();
            public IElement @__TransitionKind = new MofObjectShadow("dm:///_internal/xmi/uml#TransitionKind");

        }

        public _StateMachines StateMachines = new _StateMachines();

        public class _SimpleClassifiers
        {
            public class _BehavioredClassifier
            {
                public static string @classifierBehavior = "classifierBehavior";
                public IElement? _classifierBehavior = null;

                public static string @interfaceRealization = "interfaceRealization";
                public IElement? _interfaceRealization = null;

                public static string @ownedBehavior = "ownedBehavior";
                public IElement? _ownedBehavior = null;

            }

            public _BehavioredClassifier @BehavioredClassifier = new _BehavioredClassifier();
            public IElement @__BehavioredClassifier = new MofObjectShadow("dm:///_internal/xmi/uml#BehavioredClassifier");

            public class _DataType
            {
                public static string @ownedAttribute = "ownedAttribute";
                public IElement? _ownedAttribute = null;

                public static string @ownedOperation = "ownedOperation";
                public IElement? _ownedOperation = null;

            }

            public _DataType @DataType = new _DataType();
            public IElement @__DataType = new MofObjectShadow("dm:///_internal/xmi/uml#DataType");

            public class _Enumeration
            {
                public static string @ownedLiteral = "ownedLiteral";
                public IElement? _ownedLiteral = null;

            }

            public _Enumeration @Enumeration = new _Enumeration();
            public IElement @__Enumeration = new MofObjectShadow("dm:///_internal/xmi/uml#Enumeration");

            public class _EnumerationLiteral
            {
                public static string @classifier = "classifier";
                public IElement? _classifier = null;

                public static string @enumeration = "enumeration";
                public IElement? _enumeration = null;

            }

            public _EnumerationLiteral @EnumerationLiteral = new _EnumerationLiteral();
            public IElement @__EnumerationLiteral = new MofObjectShadow("dm:///_internal/xmi/uml#EnumerationLiteral");

            public class _Interface
            {
                public static string @nestedClassifier = "nestedClassifier";
                public IElement? _nestedClassifier = null;

                public static string @ownedAttribute = "ownedAttribute";
                public IElement? _ownedAttribute = null;

                public static string @ownedOperation = "ownedOperation";
                public IElement? _ownedOperation = null;

                public static string @ownedReception = "ownedReception";
                public IElement? _ownedReception = null;

                public static string @protocol = "protocol";
                public IElement? _protocol = null;

                public static string @redefinedInterface = "redefinedInterface";
                public IElement? _redefinedInterface = null;

            }

            public _Interface @Interface = new _Interface();
            public IElement @__Interface = new MofObjectShadow("dm:///_internal/xmi/uml#Interface");

            public class _InterfaceRealization
            {
                public static string @contract = "contract";
                public IElement? _contract = null;

                public static string @implementingClassifier = "implementingClassifier";
                public IElement? _implementingClassifier = null;

            }

            public _InterfaceRealization @InterfaceRealization = new _InterfaceRealization();
            public IElement @__InterfaceRealization = new MofObjectShadow("dm:///_internal/xmi/uml#InterfaceRealization");

            public class _PrimitiveType
            {
            }

            public _PrimitiveType @PrimitiveType = new _PrimitiveType();
            public IElement @__PrimitiveType = new MofObjectShadow("dm:///_internal/xmi/uml#PrimitiveType");

            public class _Reception
            {
                public static string @signal = "signal";
                public IElement? _signal = null;

            }

            public _Reception @Reception = new _Reception();
            public IElement @__Reception = new MofObjectShadow("dm:///_internal/xmi/uml#Reception");

            public class _Signal
            {
                public static string @ownedAttribute = "ownedAttribute";
                public IElement? _ownedAttribute = null;

            }

            public _Signal @Signal = new _Signal();
            public IElement @__Signal = new MofObjectShadow("dm:///_internal/xmi/uml#Signal");

        }

        public _SimpleClassifiers SimpleClassifiers = new _SimpleClassifiers();

        public class _Packages
        {
            public class _Extension
            {
                public static string @isRequired = "isRequired";
                public IElement? _isRequired = null;

                public static string @metaclass = "metaclass";
                public IElement? _metaclass = null;

                public static string @ownedEnd = "ownedEnd";
                public IElement? _ownedEnd = null;

            }

            public _Extension @Extension = new _Extension();
            public IElement @__Extension = new MofObjectShadow("dm:///_internal/xmi/uml#Extension");

            public class _ExtensionEnd
            {
                public static string @lower = "lower";
                public IElement? _lower = null;

                public static string @type = "type";
                public IElement? _type = null;

            }

            public _ExtensionEnd @ExtensionEnd = new _ExtensionEnd();
            public IElement @__ExtensionEnd = new MofObjectShadow("dm:///_internal/xmi/uml#ExtensionEnd");

            public class _Image
            {
                public static string @content = "content";
                public IElement? _content = null;

                public static string @format = "format";
                public IElement? _format = null;

                public static string @location = "location";
                public IElement? _location = null;

            }

            public _Image @Image = new _Image();
            public IElement @__Image = new MofObjectShadow("dm:///_internal/xmi/uml#Image");

            public class _Model
            {
                public static string @viewpoint = "viewpoint";
                public IElement? _viewpoint = null;

            }

            public _Model @Model = new _Model();
            public IElement @__Model = new MofObjectShadow("dm:///_internal/xmi/uml#Model");

            public class _Package
            {
                public static string @URI = "URI";
                public IElement? _URI = null;

                public static string @nestedPackage = "nestedPackage";
                public IElement? _nestedPackage = null;

                public static string @nestingPackage = "nestingPackage";
                public IElement? _nestingPackage = null;

                public static string @ownedStereotype = "ownedStereotype";
                public IElement? _ownedStereotype = null;

                public static string @ownedType = "ownedType";
                public IElement? _ownedType = null;

                public static string @packageMerge = "packageMerge";
                public IElement? _packageMerge = null;

                public static string @packagedElement = "packagedElement";
                public IElement? _packagedElement = null;

                public static string @profileApplication = "profileApplication";
                public IElement? _profileApplication = null;

            }

            public _Package @Package = new _Package();
            public IElement @__Package = new MofObjectShadow("dm:///_internal/xmi/uml#Package");

            public class _PackageMerge
            {
                public static string @mergedPackage = "mergedPackage";
                public IElement? _mergedPackage = null;

                public static string @receivingPackage = "receivingPackage";
                public IElement? _receivingPackage = null;

            }

            public _PackageMerge @PackageMerge = new _PackageMerge();
            public IElement @__PackageMerge = new MofObjectShadow("dm:///_internal/xmi/uml#PackageMerge");

            public class _Profile
            {
                public static string @metaclassReference = "metaclassReference";
                public IElement? _metaclassReference = null;

                public static string @metamodelReference = "metamodelReference";
                public IElement? _metamodelReference = null;

            }

            public _Profile @Profile = new _Profile();
            public IElement @__Profile = new MofObjectShadow("dm:///_internal/xmi/uml#Profile");

            public class _ProfileApplication
            {
                public static string @appliedProfile = "appliedProfile";
                public IElement? _appliedProfile = null;

                public static string @applyingPackage = "applyingPackage";
                public IElement? _applyingPackage = null;

                public static string @isStrict = "isStrict";
                public IElement? _isStrict = null;

            }

            public _ProfileApplication @ProfileApplication = new _ProfileApplication();
            public IElement @__ProfileApplication = new MofObjectShadow("dm:///_internal/xmi/uml#ProfileApplication");

            public class _Stereotype
            {
                public static string @icon = "icon";
                public IElement? _icon = null;

                public static string @profile = "profile";
                public IElement? _profile = null;

            }

            public _Stereotype @Stereotype = new _Stereotype();
            public IElement @__Stereotype = new MofObjectShadow("dm:///_internal/xmi/uml#Stereotype");

        }

        public _Packages Packages = new _Packages();

        public class _Interactions
        {
            public class _ActionExecutionSpecification
            {
                public static string @action = "action";
                public IElement? _action = null;

            }

            public _ActionExecutionSpecification @ActionExecutionSpecification = new _ActionExecutionSpecification();
            public IElement @__ActionExecutionSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#ActionExecutionSpecification");

            public class _BehaviorExecutionSpecification
            {
                public static string @behavior = "behavior";
                public IElement? _behavior = null;

            }

            public _BehaviorExecutionSpecification @BehaviorExecutionSpecification = new _BehaviorExecutionSpecification();
            public IElement @__BehaviorExecutionSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#BehaviorExecutionSpecification");

            public class _CombinedFragment
            {
                public static string @cfragmentGate = "cfragmentGate";
                public IElement? _cfragmentGate = null;

                public static string @interactionOperator = "interactionOperator";
                public IElement? _interactionOperator = null;

                public static string @operand = "operand";
                public IElement? _operand = null;

            }

            public _CombinedFragment @CombinedFragment = new _CombinedFragment();
            public IElement @__CombinedFragment = new MofObjectShadow("dm:///_internal/xmi/uml#CombinedFragment");

            public class _ConsiderIgnoreFragment
            {
                public static string @message = "message";
                public IElement? _message = null;

            }

            public _ConsiderIgnoreFragment @ConsiderIgnoreFragment = new _ConsiderIgnoreFragment();
            public IElement @__ConsiderIgnoreFragment = new MofObjectShadow("dm:///_internal/xmi/uml#ConsiderIgnoreFragment");

            public class _Continuation
            {
                public static string @setting = "setting";
                public IElement? _setting = null;

            }

            public _Continuation @Continuation = new _Continuation();
            public IElement @__Continuation = new MofObjectShadow("dm:///_internal/xmi/uml#Continuation");

            public class _DestructionOccurrenceSpecification
            {
            }

            public _DestructionOccurrenceSpecification @DestructionOccurrenceSpecification = new _DestructionOccurrenceSpecification();
            public IElement @__DestructionOccurrenceSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#DestructionOccurrenceSpecification");

            public class _ExecutionOccurrenceSpecification
            {
                public static string @execution = "execution";
                public IElement? _execution = null;

            }

            public _ExecutionOccurrenceSpecification @ExecutionOccurrenceSpecification = new _ExecutionOccurrenceSpecification();
            public IElement @__ExecutionOccurrenceSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#ExecutionOccurrenceSpecification");

            public class _ExecutionSpecification
            {
                public static string @finish = "finish";
                public IElement? _finish = null;

                public static string @start = "start";
                public IElement? _start = null;

            }

            public _ExecutionSpecification @ExecutionSpecification = new _ExecutionSpecification();
            public IElement @__ExecutionSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#ExecutionSpecification");

            public class _Gate
            {
            }

            public _Gate @Gate = new _Gate();
            public IElement @__Gate = new MofObjectShadow("dm:///_internal/xmi/uml#Gate");

            public class _GeneralOrdering
            {
                public static string @after = "after";
                public IElement? _after = null;

                public static string @before = "before";
                public IElement? _before = null;

            }

            public _GeneralOrdering @GeneralOrdering = new _GeneralOrdering();
            public IElement @__GeneralOrdering = new MofObjectShadow("dm:///_internal/xmi/uml#GeneralOrdering");

            public class _Interaction
            {
                public static string @action = "action";
                public IElement? _action = null;

                public static string @formalGate = "formalGate";
                public IElement? _formalGate = null;

                public static string @fragment = "fragment";
                public IElement? _fragment = null;

                public static string @lifeline = "lifeline";
                public IElement? _lifeline = null;

                public static string @message = "message";
                public IElement? _message = null;

            }

            public _Interaction @Interaction = new _Interaction();
            public IElement @__Interaction = new MofObjectShadow("dm:///_internal/xmi/uml#Interaction");

            public class _InteractionConstraint
            {
                public static string @maxint = "maxint";
                public IElement? _maxint = null;

                public static string @minint = "minint";
                public IElement? _minint = null;

            }

            public _InteractionConstraint @InteractionConstraint = new _InteractionConstraint();
            public IElement @__InteractionConstraint = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionConstraint");

            public class _InteractionFragment
            {
                public static string @covered = "covered";
                public IElement? _covered = null;

                public static string @enclosingInteraction = "enclosingInteraction";
                public IElement? _enclosingInteraction = null;

                public static string @enclosingOperand = "enclosingOperand";
                public IElement? _enclosingOperand = null;

                public static string @generalOrdering = "generalOrdering";
                public IElement? _generalOrdering = null;

            }

            public _InteractionFragment @InteractionFragment = new _InteractionFragment();
            public IElement @__InteractionFragment = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionFragment");

            public class _InteractionOperand
            {
                public static string @fragment = "fragment";
                public IElement? _fragment = null;

                public static string @guard = "guard";
                public IElement? _guard = null;

            }

            public _InteractionOperand @InteractionOperand = new _InteractionOperand();
            public IElement @__InteractionOperand = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperand");

            public class _InteractionUse
            {
                public static string @actualGate = "actualGate";
                public IElement? _actualGate = null;

                public static string @argument = "argument";
                public IElement? _argument = null;

                public static string @refersTo = "refersTo";
                public IElement? _refersTo = null;

                public static string @returnValue = "returnValue";
                public IElement? _returnValue = null;

                public static string @returnValueRecipient = "returnValueRecipient";
                public IElement? _returnValueRecipient = null;

            }

            public _InteractionUse @InteractionUse = new _InteractionUse();
            public IElement @__InteractionUse = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionUse");

            public class _Lifeline
            {
                public static string @coveredBy = "coveredBy";
                public IElement? _coveredBy = null;

                public static string @decomposedAs = "decomposedAs";
                public IElement? _decomposedAs = null;

                public static string @interaction = "interaction";
                public IElement? _interaction = null;

                public static string @represents = "represents";
                public IElement? _represents = null;

                public static string @selector = "selector";
                public IElement? _selector = null;

            }

            public _Lifeline @Lifeline = new _Lifeline();
            public IElement @__Lifeline = new MofObjectShadow("dm:///_internal/xmi/uml#Lifeline");

            public class _Message
            {
                public static string @argument = "argument";
                public IElement? _argument = null;

                public static string @connector = "connector";
                public IElement? _connector = null;

                public static string @interaction = "interaction";
                public IElement? _interaction = null;

                public static string @messageKind = "messageKind";
                public IElement? _messageKind = null;

                public static string @messageSort = "messageSort";
                public IElement? _messageSort = null;

                public static string @receiveEvent = "receiveEvent";
                public IElement? _receiveEvent = null;

                public static string @sendEvent = "sendEvent";
                public IElement? _sendEvent = null;

                public static string @signature = "signature";
                public IElement? _signature = null;

            }

            public _Message @Message = new _Message();
            public IElement @__Message = new MofObjectShadow("dm:///_internal/xmi/uml#Message");

            public class _MessageEnd
            {
                public static string @message = "message";
                public IElement? _message = null;

            }

            public _MessageEnd @MessageEnd = new _MessageEnd();
            public IElement @__MessageEnd = new MofObjectShadow("dm:///_internal/xmi/uml#MessageEnd");

            public class _MessageOccurrenceSpecification
            {
            }

            public _MessageOccurrenceSpecification @MessageOccurrenceSpecification = new _MessageOccurrenceSpecification();
            public IElement @__MessageOccurrenceSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#MessageOccurrenceSpecification");

            public class _OccurrenceSpecification
            {
                public static string @covered = "covered";
                public IElement? _covered = null;

                public static string @toAfter = "toAfter";
                public IElement? _toAfter = null;

                public static string @toBefore = "toBefore";
                public IElement? _toBefore = null;

            }

            public _OccurrenceSpecification @OccurrenceSpecification = new _OccurrenceSpecification();
            public IElement @__OccurrenceSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#OccurrenceSpecification");

            public class _PartDecomposition
            {
            }

            public _PartDecomposition @PartDecomposition = new _PartDecomposition();
            public IElement @__PartDecomposition = new MofObjectShadow("dm:///_internal/xmi/uml#PartDecomposition");

            public class _StateInvariant
            {
                public static string @covered = "covered";
                public IElement? _covered = null;

                public static string @invariant = "invariant";
                public IElement? _invariant = null;

            }

            public _StateInvariant @StateInvariant = new _StateInvariant();
            public IElement @__StateInvariant = new MofObjectShadow("dm:///_internal/xmi/uml#StateInvariant");

            public class _InteractionOperatorKind
            {
                public static string @seq = "seq";
                public IElement @__seq = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-seq");
                public static string @alt = "alt";
                public IElement @__alt = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-alt");
                public static string @opt = "opt";
                public IElement @__opt = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-opt");
                public static string @break = "break";
                public IElement @__break = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-break");
                public static string @par = "par";
                public IElement @__par = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-par");
                public static string @strict = "strict";
                public IElement @__strict = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-strict");
                public static string @loop = "loop";
                public IElement @__loop = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-loop");
                public static string @critical = "critical";
                public IElement @__critical = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-critical");
                public static string @neg = "neg";
                public IElement @__neg = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-neg");
                public static string @assert = "assert";
                public IElement @__assert = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-assert");
                public static string @ignore = "ignore";
                public IElement @__ignore = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-ignore");
                public static string @consider = "consider";
                public IElement @__consider = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind-consider");

            }

            public _InteractionOperatorKind @InteractionOperatorKind = new _InteractionOperatorKind();
            public IElement @__InteractionOperatorKind = new MofObjectShadow("dm:///_internal/xmi/uml#InteractionOperatorKind");

            public class _MessageKind
            {
                public static string @complete = "complete";
                public IElement @__complete = new MofObjectShadow("dm:///_internal/xmi/uml#MessageKind-complete");
                public static string @lost = "lost";
                public IElement @__lost = new MofObjectShadow("dm:///_internal/xmi/uml#MessageKind-lost");
                public static string @found = "found";
                public IElement @__found = new MofObjectShadow("dm:///_internal/xmi/uml#MessageKind-found");
                public static string @unknown = "unknown";
                public IElement @__unknown = new MofObjectShadow("dm:///_internal/xmi/uml#MessageKind-unknown");

            }

            public _MessageKind @MessageKind = new _MessageKind();
            public IElement @__MessageKind = new MofObjectShadow("dm:///_internal/xmi/uml#MessageKind");

            public class _MessageSort
            {
                public static string @synchCall = "synchCall";
                public IElement @__synchCall = new MofObjectShadow("dm:///_internal/xmi/uml#MessageSort-synchCall");
                public static string @asynchCall = "asynchCall";
                public IElement @__asynchCall = new MofObjectShadow("dm:///_internal/xmi/uml#MessageSort-asynchCall");
                public static string @asynchSignal = "asynchSignal";
                public IElement @__asynchSignal = new MofObjectShadow("dm:///_internal/xmi/uml#MessageSort-asynchSignal");
                public static string @createMessage = "createMessage";
                public IElement @__createMessage = new MofObjectShadow("dm:///_internal/xmi/uml#MessageSort-createMessage");
                public static string @deleteMessage = "deleteMessage";
                public IElement @__deleteMessage = new MofObjectShadow("dm:///_internal/xmi/uml#MessageSort-deleteMessage");
                public static string @reply = "reply";
                public IElement @__reply = new MofObjectShadow("dm:///_internal/xmi/uml#MessageSort-reply");

            }

            public _MessageSort @MessageSort = new _MessageSort();
            public IElement @__MessageSort = new MofObjectShadow("dm:///_internal/xmi/uml#MessageSort");

        }

        public _Interactions Interactions = new _Interactions();

        public class _InformationFlows
        {
            public class _InformationFlow
            {
                public static string @conveyed = "conveyed";
                public IElement? _conveyed = null;

                public static string @informationSource = "informationSource";
                public IElement? _informationSource = null;

                public static string @informationTarget = "informationTarget";
                public IElement? _informationTarget = null;

                public static string @realization = "realization";
                public IElement? _realization = null;

                public static string @realizingActivityEdge = "realizingActivityEdge";
                public IElement? _realizingActivityEdge = null;

                public static string @realizingConnector = "realizingConnector";
                public IElement? _realizingConnector = null;

                public static string @realizingMessage = "realizingMessage";
                public IElement? _realizingMessage = null;

            }

            public _InformationFlow @InformationFlow = new _InformationFlow();
            public IElement @__InformationFlow = new MofObjectShadow("dm:///_internal/xmi/uml#InformationFlow");

            public class _InformationItem
            {
                public static string @represented = "represented";
                public IElement? _represented = null;

            }

            public _InformationItem @InformationItem = new _InformationItem();
            public IElement @__InformationItem = new MofObjectShadow("dm:///_internal/xmi/uml#InformationItem");

        }

        public _InformationFlows InformationFlows = new _InformationFlows();

        public class _Deployments
        {
            public class _Artifact
            {
                public static string @fileName = "fileName";
                public IElement? _fileName = null;

                public static string @manifestation = "manifestation";
                public IElement? _manifestation = null;

                public static string @nestedArtifact = "nestedArtifact";
                public IElement? _nestedArtifact = null;

                public static string @ownedAttribute = "ownedAttribute";
                public IElement? _ownedAttribute = null;

                public static string @ownedOperation = "ownedOperation";
                public IElement? _ownedOperation = null;

            }

            public _Artifact @Artifact = new _Artifact();
            public IElement @__Artifact = new MofObjectShadow("dm:///_internal/xmi/uml#Artifact");

            public class _CommunicationPath
            {
            }

            public _CommunicationPath @CommunicationPath = new _CommunicationPath();
            public IElement @__CommunicationPath = new MofObjectShadow("dm:///_internal/xmi/uml#CommunicationPath");

            public class _DeployedArtifact
            {
            }

            public _DeployedArtifact @DeployedArtifact = new _DeployedArtifact();
            public IElement @__DeployedArtifact = new MofObjectShadow("dm:///_internal/xmi/uml#DeployedArtifact");

            public class _Deployment
            {
                public static string @configuration = "configuration";
                public IElement? _configuration = null;

                public static string @deployedArtifact = "deployedArtifact";
                public IElement? _deployedArtifact = null;

                public static string @location = "location";
                public IElement? _location = null;

            }

            public _Deployment @Deployment = new _Deployment();
            public IElement @__Deployment = new MofObjectShadow("dm:///_internal/xmi/uml#Deployment");

            public class _DeploymentSpecification
            {
                public static string @deployment = "deployment";
                public IElement? _deployment = null;

                public static string @deploymentLocation = "deploymentLocation";
                public IElement? _deploymentLocation = null;

                public static string @executionLocation = "executionLocation";
                public IElement? _executionLocation = null;

            }

            public _DeploymentSpecification @DeploymentSpecification = new _DeploymentSpecification();
            public IElement @__DeploymentSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#DeploymentSpecification");

            public class _DeploymentTarget
            {
                public static string @deployedElement = "deployedElement";
                public IElement? _deployedElement = null;

                public static string @deployment = "deployment";
                public IElement? _deployment = null;

            }

            public _DeploymentTarget @DeploymentTarget = new _DeploymentTarget();
            public IElement @__DeploymentTarget = new MofObjectShadow("dm:///_internal/xmi/uml#DeploymentTarget");

            public class _Device
            {
            }

            public _Device @Device = new _Device();
            public IElement @__Device = new MofObjectShadow("dm:///_internal/xmi/uml#Device");

            public class _ExecutionEnvironment
            {
            }

            public _ExecutionEnvironment @ExecutionEnvironment = new _ExecutionEnvironment();
            public IElement @__ExecutionEnvironment = new MofObjectShadow("dm:///_internal/xmi/uml#ExecutionEnvironment");

            public class _Manifestation
            {
                public static string @utilizedElement = "utilizedElement";
                public IElement? _utilizedElement = null;

            }

            public _Manifestation @Manifestation = new _Manifestation();
            public IElement @__Manifestation = new MofObjectShadow("dm:///_internal/xmi/uml#Manifestation");

            public class _Node
            {
                public static string @nestedNode = "nestedNode";
                public IElement? _nestedNode = null;

            }

            public _Node @Node = new _Node();
            public IElement @__Node = new MofObjectShadow("dm:///_internal/xmi/uml#Node");

        }

        public _Deployments Deployments = new _Deployments();

        public class _CommonStructure
        {
            public class _Abstraction
            {
                public static string @mapping = "mapping";
                public IElement? _mapping = null;

            }

            public _Abstraction @Abstraction = new _Abstraction();
            public IElement @__Abstraction = new MofObjectShadow("dm:///_internal/xmi/uml#Abstraction");

            public class _Comment
            {
                public static string @annotatedElement = "annotatedElement";
                public IElement? _annotatedElement = null;

                public static string @body = "body";
                public IElement? _body = null;

            }

            public _Comment @Comment = new _Comment();
            public IElement @__Comment = new MofObjectShadow("dm:///_internal/xmi/uml#Comment");

            public class _Constraint
            {
                public static string @constrainedElement = "constrainedElement";
                public IElement? _constrainedElement = null;

                public static string @context = "context";
                public IElement? _context = null;

                public static string @specification = "specification";
                public IElement? _specification = null;

            }

            public _Constraint @Constraint = new _Constraint();
            public IElement @__Constraint = new MofObjectShadow("dm:///_internal/xmi/uml#Constraint");

            public class _Dependency
            {
                public static string @client = "client";
                public IElement? _client = null;

                public static string @supplier = "supplier";
                public IElement? _supplier = null;

            }

            public _Dependency @Dependency = new _Dependency();
            public IElement @__Dependency = new MofObjectShadow("dm:///_internal/xmi/uml#Dependency");

            public class _DirectedRelationship
            {
                public static string @source = "source";
                public IElement? _source = null;

                public static string @target = "target";
                public IElement? _target = null;

            }

            public _DirectedRelationship @DirectedRelationship = new _DirectedRelationship();
            public IElement @__DirectedRelationship = new MofObjectShadow("dm:///_internal/xmi/uml#DirectedRelationship");

            public class _Element
            {
                public static string @ownedComment = "ownedComment";
                public IElement? _ownedComment = null;

                public static string @ownedElement = "ownedElement";
                public IElement? _ownedElement = null;

                public static string @owner = "owner";
                public IElement? _owner = null;

            }

            public _Element @Element = new _Element();
            public IElement @__Element = new MofObjectShadow("dm:///_internal/xmi/uml#Element");

            public class _ElementImport
            {
                public static string @alias = "alias";
                public IElement? _alias = null;

                public static string @importedElement = "importedElement";
                public IElement? _importedElement = null;

                public static string @importingNamespace = "importingNamespace";
                public IElement? _importingNamespace = null;

                public static string @visibility = "visibility";
                public IElement? _visibility = null;

            }

            public _ElementImport @ElementImport = new _ElementImport();
            public IElement @__ElementImport = new MofObjectShadow("dm:///_internal/xmi/uml#ElementImport");

            public class _MultiplicityElement
            {
                public static string @isOrdered = "isOrdered";
                public IElement? _isOrdered = null;

                public static string @isUnique = "isUnique";
                public IElement? _isUnique = null;

                public static string @lower = "lower";
                public IElement? _lower = null;

                public static string @lowerValue = "lowerValue";
                public IElement? _lowerValue = null;

                public static string @upper = "upper";
                public IElement? _upper = null;

                public static string @upperValue = "upperValue";
                public IElement? _upperValue = null;

            }

            public _MultiplicityElement @MultiplicityElement = new _MultiplicityElement();
            public IElement @__MultiplicityElement = new MofObjectShadow("dm:///_internal/xmi/uml#MultiplicityElement");

            public class _NamedElement
            {
                public static string @clientDependency = "clientDependency";
                public IElement? _clientDependency = null;

                public static string @name = "name";
                public IElement? _name = null;

                public static string @nameExpression = "nameExpression";
                public IElement? _nameExpression = null;

                public static string @namespace = "namespace";
                public IElement? _namespace = null;

                public static string @qualifiedName = "qualifiedName";
                public IElement? _qualifiedName = null;

                public static string @visibility = "visibility";
                public IElement? _visibility = null;

            }

            public _NamedElement @NamedElement = new _NamedElement();
            public IElement @__NamedElement = new MofObjectShadow("dm:///_internal/xmi/uml#NamedElement");

            public class _Namespace
            {
                public static string @elementImport = "elementImport";
                public IElement? _elementImport = null;

                public static string @importedMember = "importedMember";
                public IElement? _importedMember = null;

                public static string @member = "member";
                public IElement? _member = null;

                public static string @ownedMember = "ownedMember";
                public IElement? _ownedMember = null;

                public static string @ownedRule = "ownedRule";
                public IElement? _ownedRule = null;

                public static string @packageImport = "packageImport";
                public IElement? _packageImport = null;

            }

            public _Namespace @Namespace = new _Namespace();
            public IElement @__Namespace = new MofObjectShadow("dm:///_internal/xmi/uml#Namespace");

            public class _PackageableElement
            {
                public static string @visibility = "visibility";
                public IElement? _visibility = null;

            }

            public _PackageableElement @PackageableElement = new _PackageableElement();
            public IElement @__PackageableElement = new MofObjectShadow("dm:///_internal/xmi/uml#PackageableElement");

            public class _PackageImport
            {
                public static string @importedPackage = "importedPackage";
                public IElement? _importedPackage = null;

                public static string @importingNamespace = "importingNamespace";
                public IElement? _importingNamespace = null;

                public static string @visibility = "visibility";
                public IElement? _visibility = null;

            }

            public _PackageImport @PackageImport = new _PackageImport();
            public IElement @__PackageImport = new MofObjectShadow("dm:///_internal/xmi/uml#PackageImport");

            public class _ParameterableElement
            {
                public static string @owningTemplateParameter = "owningTemplateParameter";
                public IElement? _owningTemplateParameter = null;

                public static string @templateParameter = "templateParameter";
                public IElement? _templateParameter = null;

            }

            public _ParameterableElement @ParameterableElement = new _ParameterableElement();
            public IElement @__ParameterableElement = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterableElement");

            public class _Realization
            {
            }

            public _Realization @Realization = new _Realization();
            public IElement @__Realization = new MofObjectShadow("dm:///_internal/xmi/uml#Realization");

            public class _Relationship
            {
                public static string @relatedElement = "relatedElement";
                public IElement? _relatedElement = null;

            }

            public _Relationship @Relationship = new _Relationship();
            public IElement @__Relationship = new MofObjectShadow("dm:///_internal/xmi/uml#Relationship");

            public class _TemplateableElement
            {
                public static string @ownedTemplateSignature = "ownedTemplateSignature";
                public IElement? _ownedTemplateSignature = null;

                public static string @templateBinding = "templateBinding";
                public IElement? _templateBinding = null;

            }

            public _TemplateableElement @TemplateableElement = new _TemplateableElement();
            public IElement @__TemplateableElement = new MofObjectShadow("dm:///_internal/xmi/uml#TemplateableElement");

            public class _TemplateBinding
            {
                public static string @boundElement = "boundElement";
                public IElement? _boundElement = null;

                public static string @parameterSubstitution = "parameterSubstitution";
                public IElement? _parameterSubstitution = null;

                public static string @signature = "signature";
                public IElement? _signature = null;

            }

            public _TemplateBinding @TemplateBinding = new _TemplateBinding();
            public IElement @__TemplateBinding = new MofObjectShadow("dm:///_internal/xmi/uml#TemplateBinding");

            public class _TemplateParameter
            {
                public static string @default = "default";
                public IElement? _default = null;

                public static string @ownedDefault = "ownedDefault";
                public IElement? _ownedDefault = null;

                public static string @ownedParameteredElement = "ownedParameteredElement";
                public IElement? _ownedParameteredElement = null;

                public static string @parameteredElement = "parameteredElement";
                public IElement? _parameteredElement = null;

                public static string @signature = "signature";
                public IElement? _signature = null;

            }

            public _TemplateParameter @TemplateParameter = new _TemplateParameter();
            public IElement @__TemplateParameter = new MofObjectShadow("dm:///_internal/xmi/uml#TemplateParameter");

            public class _TemplateParameterSubstitution
            {
                public static string @actual = "actual";
                public IElement? _actual = null;

                public static string @formal = "formal";
                public IElement? _formal = null;

                public static string @ownedActual = "ownedActual";
                public IElement? _ownedActual = null;

                public static string @templateBinding = "templateBinding";
                public IElement? _templateBinding = null;

            }

            public _TemplateParameterSubstitution @TemplateParameterSubstitution = new _TemplateParameterSubstitution();
            public IElement @__TemplateParameterSubstitution = new MofObjectShadow("dm:///_internal/xmi/uml#TemplateParameterSubstitution");

            public class _TemplateSignature
            {
                public static string @ownedParameter = "ownedParameter";
                public IElement? _ownedParameter = null;

                public static string @parameter = "parameter";
                public IElement? _parameter = null;

                public static string @template = "template";
                public IElement? _template = null;

            }

            public _TemplateSignature @TemplateSignature = new _TemplateSignature();
            public IElement @__TemplateSignature = new MofObjectShadow("dm:///_internal/xmi/uml#TemplateSignature");

            public class _Type
            {
                public static string @package = "package";
                public IElement? _package = null;

            }

            public _Type @Type = new _Type();
            public IElement @__Type = new MofObjectShadow("dm:///_internal/xmi/uml#Type");

            public class _TypedElement
            {
                public static string @type = "type";
                public IElement? _type = null;

            }

            public _TypedElement @TypedElement = new _TypedElement();
            public IElement @__TypedElement = new MofObjectShadow("dm:///_internal/xmi/uml#TypedElement");

            public class _Usage
            {
            }

            public _Usage @Usage = new _Usage();
            public IElement @__Usage = new MofObjectShadow("dm:///_internal/xmi/uml#Usage");

            public class _VisibilityKind
            {
                public static string @public = "public";
                public IElement @__public = new MofObjectShadow("dm:///_internal/xmi/uml#VisibilityKind-public");
                public static string @private = "private";
                public IElement @__private = new MofObjectShadow("dm:///_internal/xmi/uml#VisibilityKind-private");
                public static string @protected = "protected";
                public IElement @__protected = new MofObjectShadow("dm:///_internal/xmi/uml#VisibilityKind-protected");
                public static string @package = "package";
                public IElement @__package = new MofObjectShadow("dm:///_internal/xmi/uml#VisibilityKind-package");

            }

            public _VisibilityKind @VisibilityKind = new _VisibilityKind();
            public IElement @__VisibilityKind = new MofObjectShadow("dm:///_internal/xmi/uml#VisibilityKind");

        }

        public _CommonStructure CommonStructure = new _CommonStructure();

        public class _CommonBehavior
        {
            public class _AnyReceiveEvent
            {
            }

            public _AnyReceiveEvent @AnyReceiveEvent = new _AnyReceiveEvent();
            public IElement @__AnyReceiveEvent = new MofObjectShadow("dm:///_internal/xmi/uml#AnyReceiveEvent");

            public class _Behavior
            {
                public static string @context = "context";
                public IElement? _context = null;

                public static string @isReentrant = "isReentrant";
                public IElement? _isReentrant = null;

                public static string @ownedParameter = "ownedParameter";
                public IElement? _ownedParameter = null;

                public static string @ownedParameterSet = "ownedParameterSet";
                public IElement? _ownedParameterSet = null;

                public static string @postcondition = "postcondition";
                public IElement? _postcondition = null;

                public static string @precondition = "precondition";
                public IElement? _precondition = null;

                public static string @specification = "specification";
                public IElement? _specification = null;

                public static string @redefinedBehavior = "redefinedBehavior";
                public IElement? _redefinedBehavior = null;

            }

            public _Behavior @Behavior = new _Behavior();
            public IElement @__Behavior = new MofObjectShadow("dm:///_internal/xmi/uml#Behavior");

            public class _CallEvent
            {
                public static string @operation = "operation";
                public IElement? _operation = null;

            }

            public _CallEvent @CallEvent = new _CallEvent();
            public IElement @__CallEvent = new MofObjectShadow("dm:///_internal/xmi/uml#CallEvent");

            public class _ChangeEvent
            {
                public static string @changeExpression = "changeExpression";
                public IElement? _changeExpression = null;

            }

            public _ChangeEvent @ChangeEvent = new _ChangeEvent();
            public IElement @__ChangeEvent = new MofObjectShadow("dm:///_internal/xmi/uml#ChangeEvent");

            public class _Event
            {
            }

            public _Event @Event = new _Event();
            public IElement @__Event = new MofObjectShadow("dm:///_internal/xmi/uml#Event");

            public class _FunctionBehavior
            {
            }

            public _FunctionBehavior @FunctionBehavior = new _FunctionBehavior();
            public IElement @__FunctionBehavior = new MofObjectShadow("dm:///_internal/xmi/uml#FunctionBehavior");

            public class _MessageEvent
            {
            }

            public _MessageEvent @MessageEvent = new _MessageEvent();
            public IElement @__MessageEvent = new MofObjectShadow("dm:///_internal/xmi/uml#MessageEvent");

            public class _OpaqueBehavior
            {
                public static string @body = "body";
                public IElement? _body = null;

                public static string @language = "language";
                public IElement? _language = null;

            }

            public _OpaqueBehavior @OpaqueBehavior = new _OpaqueBehavior();
            public IElement @__OpaqueBehavior = new MofObjectShadow("dm:///_internal/xmi/uml#OpaqueBehavior");

            public class _SignalEvent
            {
                public static string @signal = "signal";
                public IElement? _signal = null;

            }

            public _SignalEvent @SignalEvent = new _SignalEvent();
            public IElement @__SignalEvent = new MofObjectShadow("dm:///_internal/xmi/uml#SignalEvent");

            public class _TimeEvent
            {
                public static string @isRelative = "isRelative";
                public IElement? _isRelative = null;

                public static string @when = "when";
                public IElement? _when = null;

            }

            public _TimeEvent @TimeEvent = new _TimeEvent();
            public IElement @__TimeEvent = new MofObjectShadow("dm:///_internal/xmi/uml#TimeEvent");

            public class _Trigger
            {
                public static string @event = "event";
                public IElement? _event = null;

                public static string @port = "port";
                public IElement? _port = null;

            }

            public _Trigger @Trigger = new _Trigger();
            public IElement @__Trigger = new MofObjectShadow("dm:///_internal/xmi/uml#Trigger");

        }

        public _CommonBehavior CommonBehavior = new _CommonBehavior();

        public class _Classification
        {
            public class _Substitution
            {
                public static string @contract = "contract";
                public IElement? _contract = null;

                public static string @substitutingClassifier = "substitutingClassifier";
                public IElement? _substitutingClassifier = null;

            }

            public _Substitution @Substitution = new _Substitution();
            public IElement @__Substitution = new MofObjectShadow("dm:///_internal/xmi/uml#Substitution");

            public class _BehavioralFeature
            {
                public static string @concurrency = "concurrency";
                public IElement? _concurrency = null;

                public static string @isAbstract = "isAbstract";
                public IElement? _isAbstract = null;

                public static string @method = "method";
                public IElement? _method = null;

                public static string @ownedParameter = "ownedParameter";
                public IElement? _ownedParameter = null;

                public static string @ownedParameterSet = "ownedParameterSet";
                public IElement? _ownedParameterSet = null;

                public static string @raisedException = "raisedException";
                public IElement? _raisedException = null;

            }

            public _BehavioralFeature @BehavioralFeature = new _BehavioralFeature();
            public IElement @__BehavioralFeature = new MofObjectShadow("dm:///_internal/xmi/uml#BehavioralFeature");

            public class _Classifier
            {
                public static string @attribute = "attribute";
                public IElement? _attribute = null;

                public static string @collaborationUse = "collaborationUse";
                public IElement? _collaborationUse = null;

                public static string @feature = "feature";
                public IElement? _feature = null;

                public static string @general = "general";
                public IElement? _general = null;

                public static string @generalization = "generalization";
                public IElement? _generalization = null;

                public static string @inheritedMember = "inheritedMember";
                public IElement? _inheritedMember = null;

                public static string @isAbstract = "isAbstract";
                public IElement? _isAbstract = null;

                public static string @isFinalSpecialization = "isFinalSpecialization";
                public IElement? _isFinalSpecialization = null;

                public static string @ownedTemplateSignature = "ownedTemplateSignature";
                public IElement? _ownedTemplateSignature = null;

                public static string @ownedUseCase = "ownedUseCase";
                public IElement? _ownedUseCase = null;

                public static string @powertypeExtent = "powertypeExtent";
                public IElement? _powertypeExtent = null;

                public static string @redefinedClassifier = "redefinedClassifier";
                public IElement? _redefinedClassifier = null;

                public static string @representation = "representation";
                public IElement? _representation = null;

                public static string @substitution = "substitution";
                public IElement? _substitution = null;

                public static string @templateParameter = "templateParameter";
                public IElement? _templateParameter = null;

                public static string @useCase = "useCase";
                public IElement? _useCase = null;

            }

            public _Classifier @Classifier = new _Classifier();
            public IElement @__Classifier = new MofObjectShadow("dm:///_internal/xmi/uml#Classifier");

            public class _ClassifierTemplateParameter
            {
                public static string @allowSubstitutable = "allowSubstitutable";
                public IElement? _allowSubstitutable = null;

                public static string @constrainingClassifier = "constrainingClassifier";
                public IElement? _constrainingClassifier = null;

                public static string @parameteredElement = "parameteredElement";
                public IElement? _parameteredElement = null;

            }

            public _ClassifierTemplateParameter @ClassifierTemplateParameter = new _ClassifierTemplateParameter();
            public IElement @__ClassifierTemplateParameter = new MofObjectShadow("dm:///_internal/xmi/uml#ClassifierTemplateParameter");

            public class _Feature
            {
                public static string @featuringClassifier = "featuringClassifier";
                public IElement? _featuringClassifier = null;

                public static string @isStatic = "isStatic";
                public IElement? _isStatic = null;

            }

            public _Feature @Feature = new _Feature();
            public IElement @__Feature = new MofObjectShadow("dm:///_internal/xmi/uml#Feature");

            public class _Generalization
            {
                public static string @general = "general";
                public IElement? _general = null;

                public static string @generalizationSet = "generalizationSet";
                public IElement? _generalizationSet = null;

                public static string @isSubstitutable = "isSubstitutable";
                public IElement? _isSubstitutable = null;

                public static string @specific = "specific";
                public IElement? _specific = null;

            }

            public _Generalization @Generalization = new _Generalization();
            public IElement @__Generalization = new MofObjectShadow("dm:///_internal/xmi/uml#Generalization");

            public class _GeneralizationSet
            {
                public static string @generalization = "generalization";
                public IElement? _generalization = null;

                public static string @isCovering = "isCovering";
                public IElement? _isCovering = null;

                public static string @isDisjoint = "isDisjoint";
                public IElement? _isDisjoint = null;

                public static string @powertype = "powertype";
                public IElement? _powertype = null;

            }

            public _GeneralizationSet @GeneralizationSet = new _GeneralizationSet();
            public IElement @__GeneralizationSet = new MofObjectShadow("dm:///_internal/xmi/uml#GeneralizationSet");

            public class _InstanceSpecification
            {
                public static string @classifier = "classifier";
                public IElement? _classifier = null;

                public static string @slot = "slot";
                public IElement? _slot = null;

                public static string @specification = "specification";
                public IElement? _specification = null;

            }

            public _InstanceSpecification @InstanceSpecification = new _InstanceSpecification();
            public IElement @__InstanceSpecification = new MofObjectShadow("dm:///_internal/xmi/uml#InstanceSpecification");

            public class _InstanceValue
            {
                public static string @instance = "instance";
                public IElement? _instance = null;

            }

            public _InstanceValue @InstanceValue = new _InstanceValue();
            public IElement @__InstanceValue = new MofObjectShadow("dm:///_internal/xmi/uml#InstanceValue");

            public class _Operation
            {
                public static string @bodyCondition = "bodyCondition";
                public IElement? _bodyCondition = null;

                public static string @class = "class";
                public IElement? _class = null;

                public static string @datatype = "datatype";
                public IElement? _datatype = null;

                public static string @interface = "interface";
                public IElement? _interface = null;

                public static string @isOrdered = "isOrdered";
                public IElement? _isOrdered = null;

                public static string @isQuery = "isQuery";
                public IElement? _isQuery = null;

                public static string @isUnique = "isUnique";
                public IElement? _isUnique = null;

                public static string @lower = "lower";
                public IElement? _lower = null;

                public static string @ownedParameter = "ownedParameter";
                public IElement? _ownedParameter = null;

                public static string @postcondition = "postcondition";
                public IElement? _postcondition = null;

                public static string @precondition = "precondition";
                public IElement? _precondition = null;

                public static string @raisedException = "raisedException";
                public IElement? _raisedException = null;

                public static string @redefinedOperation = "redefinedOperation";
                public IElement? _redefinedOperation = null;

                public static string @templateParameter = "templateParameter";
                public IElement? _templateParameter = null;

                public static string @type = "type";
                public IElement? _type = null;

                public static string @upper = "upper";
                public IElement? _upper = null;

            }

            public _Operation @Operation = new _Operation();
            public IElement @__Operation = new MofObjectShadow("dm:///_internal/xmi/uml#Operation");

            public class _OperationTemplateParameter
            {
                public static string @parameteredElement = "parameteredElement";
                public IElement? _parameteredElement = null;

            }

            public _OperationTemplateParameter @OperationTemplateParameter = new _OperationTemplateParameter();
            public IElement @__OperationTemplateParameter = new MofObjectShadow("dm:///_internal/xmi/uml#OperationTemplateParameter");

            public class _Parameter
            {
                public static string @default = "default";
                public IElement? _default = null;

                public static string @defaultValue = "defaultValue";
                public IElement? _defaultValue = null;

                public static string @direction = "direction";
                public IElement? _direction = null;

                public static string @effect = "effect";
                public IElement? _effect = null;

                public static string @isException = "isException";
                public IElement? _isException = null;

                public static string @isStream = "isStream";
                public IElement? _isStream = null;

                public static string @operation = "operation";
                public IElement? _operation = null;

                public static string @parameterSet = "parameterSet";
                public IElement? _parameterSet = null;

            }

            public _Parameter @Parameter = new _Parameter();
            public IElement @__Parameter = new MofObjectShadow("dm:///_internal/xmi/uml#Parameter");

            public class _ParameterSet
            {
                public static string @condition = "condition";
                public IElement? _condition = null;

                public static string @parameter = "parameter";
                public IElement? _parameter = null;

            }

            public _ParameterSet @ParameterSet = new _ParameterSet();
            public IElement @__ParameterSet = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterSet");

            public class _Property
            {
                public static string @aggregation = "aggregation";
                public IElement? _aggregation = null;

                public static string @association = "association";
                public IElement? _association = null;

                public static string @associationEnd = "associationEnd";
                public IElement? _associationEnd = null;

                public static string @class = "class";
                public IElement? _class = null;

                public static string @datatype = "datatype";
                public IElement? _datatype = null;

                public static string @defaultValue = "defaultValue";
                public IElement? _defaultValue = null;

                public static string @interface = "interface";
                public IElement? _interface = null;

                public static string @isComposite = "isComposite";
                public IElement? _isComposite = null;

                public static string @isDerived = "isDerived";
                public IElement? _isDerived = null;

                public static string @isDerivedUnion = "isDerivedUnion";
                public IElement? _isDerivedUnion = null;

                public static string @isID = "isID";
                public IElement? _isID = null;

                public static string @opposite = "opposite";
                public IElement? _opposite = null;

                public static string @owningAssociation = "owningAssociation";
                public IElement? _owningAssociation = null;

                public static string @qualifier = "qualifier";
                public IElement? _qualifier = null;

                public static string @redefinedProperty = "redefinedProperty";
                public IElement? _redefinedProperty = null;

                public static string @subsettedProperty = "subsettedProperty";
                public IElement? _subsettedProperty = null;

            }

            public _Property @Property = new _Property();
            public IElement @__Property = new MofObjectShadow("dm:///_internal/xmi/uml#Property");

            public class _RedefinableElement
            {
                public static string @isLeaf = "isLeaf";
                public IElement? _isLeaf = null;

                public static string @redefinedElement = "redefinedElement";
                public IElement? _redefinedElement = null;

                public static string @redefinitionContext = "redefinitionContext";
                public IElement? _redefinitionContext = null;

            }

            public _RedefinableElement @RedefinableElement = new _RedefinableElement();
            public IElement @__RedefinableElement = new MofObjectShadow("dm:///_internal/xmi/uml#RedefinableElement");

            public class _RedefinableTemplateSignature
            {
                public static string @classifier = "classifier";
                public IElement? _classifier = null;

                public static string @extendedSignature = "extendedSignature";
                public IElement? _extendedSignature = null;

                public static string @inheritedParameter = "inheritedParameter";
                public IElement? _inheritedParameter = null;

            }

            public _RedefinableTemplateSignature @RedefinableTemplateSignature = new _RedefinableTemplateSignature();
            public IElement @__RedefinableTemplateSignature = new MofObjectShadow("dm:///_internal/xmi/uml#RedefinableTemplateSignature");

            public class _Slot
            {
                public static string @definingFeature = "definingFeature";
                public IElement? _definingFeature = null;

                public static string @owningInstance = "owningInstance";
                public IElement? _owningInstance = null;

                public static string @value = "value";
                public IElement? _value = null;

            }

            public _Slot @Slot = new _Slot();
            public IElement @__Slot = new MofObjectShadow("dm:///_internal/xmi/uml#Slot");

            public class _StructuralFeature
            {
                public static string @isReadOnly = "isReadOnly";
                public IElement? _isReadOnly = null;

            }

            public _StructuralFeature @StructuralFeature = new _StructuralFeature();
            public IElement @__StructuralFeature = new MofObjectShadow("dm:///_internal/xmi/uml#StructuralFeature");

            public class _AggregationKind
            {
                public static string @none = "none";
                public IElement @__none = new MofObjectShadow("dm:///_internal/xmi/uml#AggregationKind-none");
                public static string @shared = "shared";
                public IElement @__shared = new MofObjectShadow("dm:///_internal/xmi/uml#AggregationKind-shared");
                public static string @composite = "composite";
                public IElement @__composite = new MofObjectShadow("dm:///_internal/xmi/uml#AggregationKind-composite");

            }

            public _AggregationKind @AggregationKind = new _AggregationKind();
            public IElement @__AggregationKind = new MofObjectShadow("dm:///_internal/xmi/uml#AggregationKind");

            public class _CallConcurrencyKind
            {
                public static string @sequential = "sequential";
                public IElement @__sequential = new MofObjectShadow("dm:///_internal/xmi/uml#CallConcurrencyKind-sequential");
                public static string @guarded = "guarded";
                public IElement @__guarded = new MofObjectShadow("dm:///_internal/xmi/uml#CallConcurrencyKind-guarded");
                public static string @concurrent = "concurrent";
                public IElement @__concurrent = new MofObjectShadow("dm:///_internal/xmi/uml#CallConcurrencyKind-concurrent");

            }

            public _CallConcurrencyKind @CallConcurrencyKind = new _CallConcurrencyKind();
            public IElement @__CallConcurrencyKind = new MofObjectShadow("dm:///_internal/xmi/uml#CallConcurrencyKind");

            public class _ParameterDirectionKind
            {
                public static string @in = "in";
                public IElement @__in = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterDirectionKind-in");
                public static string @inout = "inout";
                public IElement @__inout = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterDirectionKind-inout");
                public static string @out = "out";
                public IElement @__out = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterDirectionKind-out");
                public static string @return = "return";
                public IElement @__return = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterDirectionKind-return");

            }

            public _ParameterDirectionKind @ParameterDirectionKind = new _ParameterDirectionKind();
            public IElement @__ParameterDirectionKind = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterDirectionKind");

            public class _ParameterEffectKind
            {
                public static string @create = "create";
                public IElement @__create = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterEffectKind-create");
                public static string @read = "read";
                public IElement @__read = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterEffectKind-read");
                public static string @update = "update";
                public IElement @__update = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterEffectKind-update");
                public static string @delete = "delete";
                public IElement @__delete = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterEffectKind-delete");

            }

            public _ParameterEffectKind @ParameterEffectKind = new _ParameterEffectKind();
            public IElement @__ParameterEffectKind = new MofObjectShadow("dm:///_internal/xmi/uml#ParameterEffectKind");

        }

        public _Classification Classification = new _Classification();

        public class _Actions
        {
            public class _ValueSpecificationAction
            {
                public static string @result = "result";
                public IElement? _result = null;

                public static string @value = "value";
                public IElement? _value = null;

            }

            public _ValueSpecificationAction @ValueSpecificationAction = new _ValueSpecificationAction();
            public IElement @__ValueSpecificationAction = new MofObjectShadow("dm:///_internal/xmi/uml#ValueSpecificationAction");

            public class _VariableAction
            {
                public static string @variable = "variable";
                public IElement? _variable = null;

            }

            public _VariableAction @VariableAction = new _VariableAction();
            public IElement @__VariableAction = new MofObjectShadow("dm:///_internal/xmi/uml#VariableAction");

            public class _WriteLinkAction
            {
            }

            public _WriteLinkAction @WriteLinkAction = new _WriteLinkAction();
            public IElement @__WriteLinkAction = new MofObjectShadow("dm:///_internal/xmi/uml#WriteLinkAction");

            public class _WriteStructuralFeatureAction
            {
                public static string @result = "result";
                public IElement? _result = null;

                public static string @value = "value";
                public IElement? _value = null;

            }

            public _WriteStructuralFeatureAction @WriteStructuralFeatureAction = new _WriteStructuralFeatureAction();
            public IElement @__WriteStructuralFeatureAction = new MofObjectShadow("dm:///_internal/xmi/uml#WriteStructuralFeatureAction");

            public class _WriteVariableAction
            {
                public static string @value = "value";
                public IElement? _value = null;

            }

            public _WriteVariableAction @WriteVariableAction = new _WriteVariableAction();
            public IElement @__WriteVariableAction = new MofObjectShadow("dm:///_internal/xmi/uml#WriteVariableAction");

            public class _ExpansionKind
            {
                public static string @parallel = "parallel";
                public IElement @__parallel = new MofObjectShadow("dm:///_internal/xmi/uml#ExpansionKind-parallel");
                public static string @iterative = "iterative";
                public IElement @__iterative = new MofObjectShadow("dm:///_internal/xmi/uml#ExpansionKind-iterative");
                public static string @stream = "stream";
                public IElement @__stream = new MofObjectShadow("dm:///_internal/xmi/uml#ExpansionKind-stream");

            }

            public _ExpansionKind @ExpansionKind = new _ExpansionKind();
            public IElement @__ExpansionKind = new MofObjectShadow("dm:///_internal/xmi/uml#ExpansionKind");

            public class _AcceptCallAction
            {
                public static string @returnInformation = "returnInformation";
                public IElement? _returnInformation = null;

            }

            public _AcceptCallAction @AcceptCallAction = new _AcceptCallAction();
            public IElement @__AcceptCallAction = new MofObjectShadow("dm:///_internal/xmi/uml#AcceptCallAction");

            public class _AcceptEventAction
            {
                public static string @isUnmarshall = "isUnmarshall";
                public IElement? _isUnmarshall = null;

                public static string @result = "result";
                public IElement? _result = null;

                public static string @trigger = "trigger";
                public IElement? _trigger = null;

            }

            public _AcceptEventAction @AcceptEventAction = new _AcceptEventAction();
            public IElement @__AcceptEventAction = new MofObjectShadow("dm:///_internal/xmi/uml#AcceptEventAction");

            public class _Action
            {
                public static string @context = "context";
                public IElement? _context = null;

                public static string @input = "input";
                public IElement? _input = null;

                public static string @isLocallyReentrant = "isLocallyReentrant";
                public IElement? _isLocallyReentrant = null;

                public static string @localPostcondition = "localPostcondition";
                public IElement? _localPostcondition = null;

                public static string @localPrecondition = "localPrecondition";
                public IElement? _localPrecondition = null;

                public static string @output = "output";
                public IElement? _output = null;

            }

            public _Action @Action = new _Action();
            public IElement @__Action = new MofObjectShadow("dm:///_internal/xmi/uml#Action");

            public class _ActionInputPin
            {
                public static string @fromAction = "fromAction";
                public IElement? _fromAction = null;

            }

            public _ActionInputPin @ActionInputPin = new _ActionInputPin();
            public IElement @__ActionInputPin = new MofObjectShadow("dm:///_internal/xmi/uml#ActionInputPin");

            public class _AddStructuralFeatureValueAction
            {
                public static string @insertAt = "insertAt";
                public IElement? _insertAt = null;

                public static string @isReplaceAll = "isReplaceAll";
                public IElement? _isReplaceAll = null;

            }

            public _AddStructuralFeatureValueAction @AddStructuralFeatureValueAction = new _AddStructuralFeatureValueAction();
            public IElement @__AddStructuralFeatureValueAction = new MofObjectShadow("dm:///_internal/xmi/uml#AddStructuralFeatureValueAction");

            public class _AddVariableValueAction
            {
                public static string @insertAt = "insertAt";
                public IElement? _insertAt = null;

                public static string @isReplaceAll = "isReplaceAll";
                public IElement? _isReplaceAll = null;

            }

            public _AddVariableValueAction @AddVariableValueAction = new _AddVariableValueAction();
            public IElement @__AddVariableValueAction = new MofObjectShadow("dm:///_internal/xmi/uml#AddVariableValueAction");

            public class _BroadcastSignalAction
            {
                public static string @signal = "signal";
                public IElement? _signal = null;

            }

            public _BroadcastSignalAction @BroadcastSignalAction = new _BroadcastSignalAction();
            public IElement @__BroadcastSignalAction = new MofObjectShadow("dm:///_internal/xmi/uml#BroadcastSignalAction");

            public class _CallAction
            {
                public static string @isSynchronous = "isSynchronous";
                public IElement? _isSynchronous = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _CallAction @CallAction = new _CallAction();
            public IElement @__CallAction = new MofObjectShadow("dm:///_internal/xmi/uml#CallAction");

            public class _CallBehaviorAction
            {
                public static string @behavior = "behavior";
                public IElement? _behavior = null;

            }

            public _CallBehaviorAction @CallBehaviorAction = new _CallBehaviorAction();
            public IElement @__CallBehaviorAction = new MofObjectShadow("dm:///_internal/xmi/uml#CallBehaviorAction");

            public class _CallOperationAction
            {
                public static string @operation = "operation";
                public IElement? _operation = null;

                public static string @target = "target";
                public IElement? _target = null;

            }

            public _CallOperationAction @CallOperationAction = new _CallOperationAction();
            public IElement @__CallOperationAction = new MofObjectShadow("dm:///_internal/xmi/uml#CallOperationAction");

            public class _Clause
            {
                public static string @body = "body";
                public IElement? _body = null;

                public static string @bodyOutput = "bodyOutput";
                public IElement? _bodyOutput = null;

                public static string @decider = "decider";
                public IElement? _decider = null;

                public static string @predecessorClause = "predecessorClause";
                public IElement? _predecessorClause = null;

                public static string @successorClause = "successorClause";
                public IElement? _successorClause = null;

                public static string @test = "test";
                public IElement? _test = null;

            }

            public _Clause @Clause = new _Clause();
            public IElement @__Clause = new MofObjectShadow("dm:///_internal/xmi/uml#Clause");

            public class _ClearAssociationAction
            {
                public static string @association = "association";
                public IElement? _association = null;

                public static string @object = "object";
                public IElement? _object = null;

            }

            public _ClearAssociationAction @ClearAssociationAction = new _ClearAssociationAction();
            public IElement @__ClearAssociationAction = new MofObjectShadow("dm:///_internal/xmi/uml#ClearAssociationAction");

            public class _ClearStructuralFeatureAction
            {
                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ClearStructuralFeatureAction @ClearStructuralFeatureAction = new _ClearStructuralFeatureAction();
            public IElement @__ClearStructuralFeatureAction = new MofObjectShadow("dm:///_internal/xmi/uml#ClearStructuralFeatureAction");

            public class _ClearVariableAction
            {
            }

            public _ClearVariableAction @ClearVariableAction = new _ClearVariableAction();
            public IElement @__ClearVariableAction = new MofObjectShadow("dm:///_internal/xmi/uml#ClearVariableAction");

            public class _ConditionalNode
            {
                public static string @clause = "clause";
                public IElement? _clause = null;

                public static string @isAssured = "isAssured";
                public IElement? _isAssured = null;

                public static string @isDeterminate = "isDeterminate";
                public IElement? _isDeterminate = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ConditionalNode @ConditionalNode = new _ConditionalNode();
            public IElement @__ConditionalNode = new MofObjectShadow("dm:///_internal/xmi/uml#ConditionalNode");

            public class _CreateLinkAction
            {
                public static string @endData = "endData";
                public IElement? _endData = null;

            }

            public _CreateLinkAction @CreateLinkAction = new _CreateLinkAction();
            public IElement @__CreateLinkAction = new MofObjectShadow("dm:///_internal/xmi/uml#CreateLinkAction");

            public class _CreateLinkObjectAction
            {
                public static string @result = "result";
                public IElement? _result = null;

            }

            public _CreateLinkObjectAction @CreateLinkObjectAction = new _CreateLinkObjectAction();
            public IElement @__CreateLinkObjectAction = new MofObjectShadow("dm:///_internal/xmi/uml#CreateLinkObjectAction");

            public class _CreateObjectAction
            {
                public static string @classifier = "classifier";
                public IElement? _classifier = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _CreateObjectAction @CreateObjectAction = new _CreateObjectAction();
            public IElement @__CreateObjectAction = new MofObjectShadow("dm:///_internal/xmi/uml#CreateObjectAction");

            public class _DestroyLinkAction
            {
                public static string @endData = "endData";
                public IElement? _endData = null;

            }

            public _DestroyLinkAction @DestroyLinkAction = new _DestroyLinkAction();
            public IElement @__DestroyLinkAction = new MofObjectShadow("dm:///_internal/xmi/uml#DestroyLinkAction");

            public class _DestroyObjectAction
            {
                public static string @isDestroyLinks = "isDestroyLinks";
                public IElement? _isDestroyLinks = null;

                public static string @isDestroyOwnedObjects = "isDestroyOwnedObjects";
                public IElement? _isDestroyOwnedObjects = null;

                public static string @target = "target";
                public IElement? _target = null;

            }

            public _DestroyObjectAction @DestroyObjectAction = new _DestroyObjectAction();
            public IElement @__DestroyObjectAction = new MofObjectShadow("dm:///_internal/xmi/uml#DestroyObjectAction");

            public class _ExpansionNode
            {
                public static string @regionAsInput = "regionAsInput";
                public IElement? _regionAsInput = null;

                public static string @regionAsOutput = "regionAsOutput";
                public IElement? _regionAsOutput = null;

            }

            public _ExpansionNode @ExpansionNode = new _ExpansionNode();
            public IElement @__ExpansionNode = new MofObjectShadow("dm:///_internal/xmi/uml#ExpansionNode");

            public class _ExpansionRegion
            {
                public static string @inputElement = "inputElement";
                public IElement? _inputElement = null;

                public static string @mode = "mode";
                public IElement? _mode = null;

                public static string @outputElement = "outputElement";
                public IElement? _outputElement = null;

            }

            public _ExpansionRegion @ExpansionRegion = new _ExpansionRegion();
            public IElement @__ExpansionRegion = new MofObjectShadow("dm:///_internal/xmi/uml#ExpansionRegion");

            public class _InputPin
            {
            }

            public _InputPin @InputPin = new _InputPin();
            public IElement @__InputPin = new MofObjectShadow("dm:///_internal/xmi/uml#InputPin");

            public class _InvocationAction
            {
                public static string @argument = "argument";
                public IElement? _argument = null;

                public static string @onPort = "onPort";
                public IElement? _onPort = null;

            }

            public _InvocationAction @InvocationAction = new _InvocationAction();
            public IElement @__InvocationAction = new MofObjectShadow("dm:///_internal/xmi/uml#InvocationAction");

            public class _LinkAction
            {
                public static string @endData = "endData";
                public IElement? _endData = null;

                public static string @inputValue = "inputValue";
                public IElement? _inputValue = null;

            }

            public _LinkAction @LinkAction = new _LinkAction();
            public IElement @__LinkAction = new MofObjectShadow("dm:///_internal/xmi/uml#LinkAction");

            public class _LinkEndCreationData
            {
                public static string @insertAt = "insertAt";
                public IElement? _insertAt = null;

                public static string @isReplaceAll = "isReplaceAll";
                public IElement? _isReplaceAll = null;

            }

            public _LinkEndCreationData @LinkEndCreationData = new _LinkEndCreationData();
            public IElement @__LinkEndCreationData = new MofObjectShadow("dm:///_internal/xmi/uml#LinkEndCreationData");

            public class _LinkEndData
            {
                public static string @end = "end";
                public IElement? _end = null;

                public static string @qualifier = "qualifier";
                public IElement? _qualifier = null;

                public static string @value = "value";
                public IElement? _value = null;

            }

            public _LinkEndData @LinkEndData = new _LinkEndData();
            public IElement @__LinkEndData = new MofObjectShadow("dm:///_internal/xmi/uml#LinkEndData");

            public class _LinkEndDestructionData
            {
                public static string @destroyAt = "destroyAt";
                public IElement? _destroyAt = null;

                public static string @isDestroyDuplicates = "isDestroyDuplicates";
                public IElement? _isDestroyDuplicates = null;

            }

            public _LinkEndDestructionData @LinkEndDestructionData = new _LinkEndDestructionData();
            public IElement @__LinkEndDestructionData = new MofObjectShadow("dm:///_internal/xmi/uml#LinkEndDestructionData");

            public class _LoopNode
            {
                public static string @bodyOutput = "bodyOutput";
                public IElement? _bodyOutput = null;

                public static string @bodyPart = "bodyPart";
                public IElement? _bodyPart = null;

                public static string @decider = "decider";
                public IElement? _decider = null;

                public static string @isTestedFirst = "isTestedFirst";
                public IElement? _isTestedFirst = null;

                public static string @loopVariable = "loopVariable";
                public IElement? _loopVariable = null;

                public static string @loopVariableInput = "loopVariableInput";
                public IElement? _loopVariableInput = null;

                public static string @result = "result";
                public IElement? _result = null;

                public static string @setupPart = "setupPart";
                public IElement? _setupPart = null;

                public static string @test = "test";
                public IElement? _test = null;

            }

            public _LoopNode @LoopNode = new _LoopNode();
            public IElement @__LoopNode = new MofObjectShadow("dm:///_internal/xmi/uml#LoopNode");

            public class _OpaqueAction
            {
                public static string @body = "body";
                public IElement? _body = null;

                public static string @inputValue = "inputValue";
                public IElement? _inputValue = null;

                public static string @language = "language";
                public IElement? _language = null;

                public static string @outputValue = "outputValue";
                public IElement? _outputValue = null;

            }

            public _OpaqueAction @OpaqueAction = new _OpaqueAction();
            public IElement @__OpaqueAction = new MofObjectShadow("dm:///_internal/xmi/uml#OpaqueAction");

            public class _OutputPin
            {
            }

            public _OutputPin @OutputPin = new _OutputPin();
            public IElement @__OutputPin = new MofObjectShadow("dm:///_internal/xmi/uml#OutputPin");

            public class _Pin
            {
                public static string @isControl = "isControl";
                public IElement? _isControl = null;

            }

            public _Pin @Pin = new _Pin();
            public IElement @__Pin = new MofObjectShadow("dm:///_internal/xmi/uml#Pin");

            public class _QualifierValue
            {
                public static string @qualifier = "qualifier";
                public IElement? _qualifier = null;

                public static string @value = "value";
                public IElement? _value = null;

            }

            public _QualifierValue @QualifierValue = new _QualifierValue();
            public IElement @__QualifierValue = new MofObjectShadow("dm:///_internal/xmi/uml#QualifierValue");

            public class _RaiseExceptionAction
            {
                public static string @exception = "exception";
                public IElement? _exception = null;

            }

            public _RaiseExceptionAction @RaiseExceptionAction = new _RaiseExceptionAction();
            public IElement @__RaiseExceptionAction = new MofObjectShadow("dm:///_internal/xmi/uml#RaiseExceptionAction");

            public class _ReadExtentAction
            {
                public static string @classifier = "classifier";
                public IElement? _classifier = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadExtentAction @ReadExtentAction = new _ReadExtentAction();
            public IElement @__ReadExtentAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadExtentAction");

            public class _ReadIsClassifiedObjectAction
            {
                public static string @classifier = "classifier";
                public IElement? _classifier = null;

                public static string @isDirect = "isDirect";
                public IElement? _isDirect = null;

                public static string @object = "object";
                public IElement? _object = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadIsClassifiedObjectAction @ReadIsClassifiedObjectAction = new _ReadIsClassifiedObjectAction();
            public IElement @__ReadIsClassifiedObjectAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadIsClassifiedObjectAction");

            public class _ReadLinkAction
            {
                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadLinkAction @ReadLinkAction = new _ReadLinkAction();
            public IElement @__ReadLinkAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadLinkAction");

            public class _ReadLinkObjectEndAction
            {
                public static string @end = "end";
                public IElement? _end = null;

                public static string @object = "object";
                public IElement? _object = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadLinkObjectEndAction @ReadLinkObjectEndAction = new _ReadLinkObjectEndAction();
            public IElement @__ReadLinkObjectEndAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadLinkObjectEndAction");

            public class _ReadLinkObjectEndQualifierAction
            {
                public static string @object = "object";
                public IElement? _object = null;

                public static string @qualifier = "qualifier";
                public IElement? _qualifier = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadLinkObjectEndQualifierAction @ReadLinkObjectEndQualifierAction = new _ReadLinkObjectEndQualifierAction();
            public IElement @__ReadLinkObjectEndQualifierAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadLinkObjectEndQualifierAction");

            public class _ReadSelfAction
            {
                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadSelfAction @ReadSelfAction = new _ReadSelfAction();
            public IElement @__ReadSelfAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadSelfAction");

            public class _ReadStructuralFeatureAction
            {
                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadStructuralFeatureAction @ReadStructuralFeatureAction = new _ReadStructuralFeatureAction();
            public IElement @__ReadStructuralFeatureAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadStructuralFeatureAction");

            public class _ReadVariableAction
            {
                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReadVariableAction @ReadVariableAction = new _ReadVariableAction();
            public IElement @__ReadVariableAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReadVariableAction");

            public class _ReclassifyObjectAction
            {
                public static string @isReplaceAll = "isReplaceAll";
                public IElement? _isReplaceAll = null;

                public static string @newClassifier = "newClassifier";
                public IElement? _newClassifier = null;

                public static string @object = "object";
                public IElement? _object = null;

                public static string @oldClassifier = "oldClassifier";
                public IElement? _oldClassifier = null;

            }

            public _ReclassifyObjectAction @ReclassifyObjectAction = new _ReclassifyObjectAction();
            public IElement @__ReclassifyObjectAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReclassifyObjectAction");

            public class _ReduceAction
            {
                public static string @collection = "collection";
                public IElement? _collection = null;

                public static string @isOrdered = "isOrdered";
                public IElement? _isOrdered = null;

                public static string @reducer = "reducer";
                public IElement? _reducer = null;

                public static string @result = "result";
                public IElement? _result = null;

            }

            public _ReduceAction @ReduceAction = new _ReduceAction();
            public IElement @__ReduceAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReduceAction");

            public class _RemoveStructuralFeatureValueAction
            {
                public static string @isRemoveDuplicates = "isRemoveDuplicates";
                public IElement? _isRemoveDuplicates = null;

                public static string @removeAt = "removeAt";
                public IElement? _removeAt = null;

            }

            public _RemoveStructuralFeatureValueAction @RemoveStructuralFeatureValueAction = new _RemoveStructuralFeatureValueAction();
            public IElement @__RemoveStructuralFeatureValueAction = new MofObjectShadow("dm:///_internal/xmi/uml#RemoveStructuralFeatureValueAction");

            public class _RemoveVariableValueAction
            {
                public static string @isRemoveDuplicates = "isRemoveDuplicates";
                public IElement? _isRemoveDuplicates = null;

                public static string @removeAt = "removeAt";
                public IElement? _removeAt = null;

            }

            public _RemoveVariableValueAction @RemoveVariableValueAction = new _RemoveVariableValueAction();
            public IElement @__RemoveVariableValueAction = new MofObjectShadow("dm:///_internal/xmi/uml#RemoveVariableValueAction");

            public class _ReplyAction
            {
                public static string @replyToCall = "replyToCall";
                public IElement? _replyToCall = null;

                public static string @replyValue = "replyValue";
                public IElement? _replyValue = null;

                public static string @returnInformation = "returnInformation";
                public IElement? _returnInformation = null;

            }

            public _ReplyAction @ReplyAction = new _ReplyAction();
            public IElement @__ReplyAction = new MofObjectShadow("dm:///_internal/xmi/uml#ReplyAction");

            public class _SendObjectAction
            {
                public static string @request = "request";
                public IElement? _request = null;

                public static string @target = "target";
                public IElement? _target = null;

            }

            public _SendObjectAction @SendObjectAction = new _SendObjectAction();
            public IElement @__SendObjectAction = new MofObjectShadow("dm:///_internal/xmi/uml#SendObjectAction");

            public class _SendSignalAction
            {
                public static string @signal = "signal";
                public IElement? _signal = null;

                public static string @target = "target";
                public IElement? _target = null;

            }

            public _SendSignalAction @SendSignalAction = new _SendSignalAction();
            public IElement @__SendSignalAction = new MofObjectShadow("dm:///_internal/xmi/uml#SendSignalAction");

            public class _SequenceNode
            {
                public static string @executableNode = "executableNode";
                public IElement? _executableNode = null;

            }

            public _SequenceNode @SequenceNode = new _SequenceNode();
            public IElement @__SequenceNode = new MofObjectShadow("dm:///_internal/xmi/uml#SequenceNode");

            public class _StartClassifierBehaviorAction
            {
                public static string @object = "object";
                public IElement? _object = null;

            }

            public _StartClassifierBehaviorAction @StartClassifierBehaviorAction = new _StartClassifierBehaviorAction();
            public IElement @__StartClassifierBehaviorAction = new MofObjectShadow("dm:///_internal/xmi/uml#StartClassifierBehaviorAction");

            public class _StartObjectBehaviorAction
            {
                public static string @object = "object";
                public IElement? _object = null;

            }

            public _StartObjectBehaviorAction @StartObjectBehaviorAction = new _StartObjectBehaviorAction();
            public IElement @__StartObjectBehaviorAction = new MofObjectShadow("dm:///_internal/xmi/uml#StartObjectBehaviorAction");

            public class _StructuralFeatureAction
            {
                public static string @object = "object";
                public IElement? _object = null;

                public static string @structuralFeature = "structuralFeature";
                public IElement? _structuralFeature = null;

            }

            public _StructuralFeatureAction @StructuralFeatureAction = new _StructuralFeatureAction();
            public IElement @__StructuralFeatureAction = new MofObjectShadow("dm:///_internal/xmi/uml#StructuralFeatureAction");

            public class _StructuredActivityNode
            {
                public static string @activity = "activity";
                public IElement? _activity = null;

                public static string @edge = "edge";
                public IElement? _edge = null;

                public static string @mustIsolate = "mustIsolate";
                public IElement? _mustIsolate = null;

                public static string @node = "node";
                public IElement? _node = null;

                public static string @structuredNodeInput = "structuredNodeInput";
                public IElement? _structuredNodeInput = null;

                public static string @structuredNodeOutput = "structuredNodeOutput";
                public IElement? _structuredNodeOutput = null;

                public static string @variable = "variable";
                public IElement? _variable = null;

            }

            public _StructuredActivityNode @StructuredActivityNode = new _StructuredActivityNode();
            public IElement @__StructuredActivityNode = new MofObjectShadow("dm:///_internal/xmi/uml#StructuredActivityNode");

            public class _TestIdentityAction
            {
                public static string @first = "first";
                public IElement? _first = null;

                public static string @result = "result";
                public IElement? _result = null;

                public static string @second = "second";
                public IElement? _second = null;

            }

            public _TestIdentityAction @TestIdentityAction = new _TestIdentityAction();
            public IElement @__TestIdentityAction = new MofObjectShadow("dm:///_internal/xmi/uml#TestIdentityAction");

            public class _UnmarshallAction
            {
                public static string @object = "object";
                public IElement? _object = null;

                public static string @result = "result";
                public IElement? _result = null;

                public static string @unmarshallType = "unmarshallType";
                public IElement? _unmarshallType = null;

            }

            public _UnmarshallAction @UnmarshallAction = new _UnmarshallAction();
            public IElement @__UnmarshallAction = new MofObjectShadow("dm:///_internal/xmi/uml#UnmarshallAction");

            public class _ValuePin
            {
                public static string @value = "value";
                public IElement? _value = null;

            }

            public _ValuePin @ValuePin = new _ValuePin();
            public IElement @__ValuePin = new MofObjectShadow("dm:///_internal/xmi/uml#ValuePin");

        }

        public _Actions Actions = new _Actions();

        public static _UML TheOne = new _UML();

    }

}
