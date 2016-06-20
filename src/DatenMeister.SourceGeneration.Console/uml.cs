using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.1.0.0
namespace DatenMeister
{
    public class _UML
    {
        public class _Activities
        {
            public class _Activity
            {
                public string @edge = "edge";
                public IElement _edge = null;

                public string @group = "group";
                public IElement _group = null;

                public string @isReadOnly = "isReadOnly";
                public IElement _isReadOnly = null;

                public string @isSingleExecution = "isSingleExecution";
                public IElement _isSingleExecution = null;

                public string @node = "node";
                public IElement _node = null;

                public string @partition = "partition";
                public IElement _partition = null;

                public string @structuredNode = "structuredNode";
                public IElement _structuredNode = null;

                public string @variable = "variable";
                public IElement _variable = null;

            }

            public _Activity @Activity = new _Activity();
            public IElement @__Activity = new MofElement();

            public class _ActivityEdge
            {
                public string @activity = "activity";
                public IElement _activity = null;

                public string @guard = "guard";
                public IElement _guard = null;

                public string @inGroup = "inGroup";
                public IElement _inGroup = null;

                public string @inPartition = "inPartition";
                public IElement _inPartition = null;

                public string @inStructuredNode = "inStructuredNode";
                public IElement _inStructuredNode = null;

                public string @interrupts = "interrupts";
                public IElement _interrupts = null;

                public string @redefinedEdge = "redefinedEdge";
                public IElement _redefinedEdge = null;

                public string @source = "source";
                public IElement _source = null;

                public string @target = "target";
                public IElement _target = null;

                public string @weight = "weight";
                public IElement _weight = null;

            }

            public _ActivityEdge @ActivityEdge = new _ActivityEdge();
            public IElement @__ActivityEdge = new MofElement();

            public class _ActivityFinalNode
            {
            }

            public _ActivityFinalNode @ActivityFinalNode = new _ActivityFinalNode();
            public IElement @__ActivityFinalNode = new MofElement();

            public class _ActivityGroup
            {
                public string @containedEdge = "containedEdge";
                public IElement _containedEdge = null;

                public string @containedNode = "containedNode";
                public IElement _containedNode = null;

                public string @inActivity = "inActivity";
                public IElement _inActivity = null;

                public string @subgroup = "subgroup";
                public IElement _subgroup = null;

                public string @superGroup = "superGroup";
                public IElement _superGroup = null;

            }

            public _ActivityGroup @ActivityGroup = new _ActivityGroup();
            public IElement @__ActivityGroup = new MofElement();

            public class _ActivityNode
            {
                public string @activity = "activity";
                public IElement _activity = null;

                public string @inGroup = "inGroup";
                public IElement _inGroup = null;

                public string @inInterruptibleRegion = "inInterruptibleRegion";
                public IElement _inInterruptibleRegion = null;

                public string @inPartition = "inPartition";
                public IElement _inPartition = null;

                public string @inStructuredNode = "inStructuredNode";
                public IElement _inStructuredNode = null;

                public string @incoming = "incoming";
                public IElement _incoming = null;

                public string @outgoing = "outgoing";
                public IElement _outgoing = null;

                public string @redefinedNode = "redefinedNode";
                public IElement _redefinedNode = null;

            }

            public _ActivityNode @ActivityNode = new _ActivityNode();
            public IElement @__ActivityNode = new MofElement();

            public class _ActivityParameterNode
            {
                public string @parameter = "parameter";
                public IElement _parameter = null;

            }

            public _ActivityParameterNode @ActivityParameterNode = new _ActivityParameterNode();
            public IElement @__ActivityParameterNode = new MofElement();

            public class _ActivityPartition
            {
                public string @edge = "edge";
                public IElement _edge = null;

                public string @isDimension = "isDimension";
                public IElement _isDimension = null;

                public string @isExternal = "isExternal";
                public IElement _isExternal = null;

                public string @node = "node";
                public IElement _node = null;

                public string @represents = "represents";
                public IElement _represents = null;

                public string @subpartition = "subpartition";
                public IElement _subpartition = null;

                public string @superPartition = "superPartition";
                public IElement _superPartition = null;

            }

            public _ActivityPartition @ActivityPartition = new _ActivityPartition();
            public IElement @__ActivityPartition = new MofElement();

            public class _CentralBufferNode
            {
            }

            public _CentralBufferNode @CentralBufferNode = new _CentralBufferNode();
            public IElement @__CentralBufferNode = new MofElement();

            public class _ControlFlow
            {
            }

            public _ControlFlow @ControlFlow = new _ControlFlow();
            public IElement @__ControlFlow = new MofElement();

            public class _ControlNode
            {
            }

            public _ControlNode @ControlNode = new _ControlNode();
            public IElement @__ControlNode = new MofElement();

            public class _DataStoreNode
            {
            }

            public _DataStoreNode @DataStoreNode = new _DataStoreNode();
            public IElement @__DataStoreNode = new MofElement();

            public class _DecisionNode
            {
                public string @decisionInput = "decisionInput";
                public IElement _decisionInput = null;

                public string @decisionInputFlow = "decisionInputFlow";
                public IElement _decisionInputFlow = null;

            }

            public _DecisionNode @DecisionNode = new _DecisionNode();
            public IElement @__DecisionNode = new MofElement();

            public class _ExceptionHandler
            {
                public string @exceptionInput = "exceptionInput";
                public IElement _exceptionInput = null;

                public string @exceptionType = "exceptionType";
                public IElement _exceptionType = null;

                public string @handlerBody = "handlerBody";
                public IElement _handlerBody = null;

                public string @protectedNode = "protectedNode";
                public IElement _protectedNode = null;

            }

            public _ExceptionHandler @ExceptionHandler = new _ExceptionHandler();
            public IElement @__ExceptionHandler = new MofElement();

            public class _ExecutableNode
            {
                public string @handler = "handler";
                public IElement _handler = null;

            }

            public _ExecutableNode @ExecutableNode = new _ExecutableNode();
            public IElement @__ExecutableNode = new MofElement();

            public class _FinalNode
            {
            }

            public _FinalNode @FinalNode = new _FinalNode();
            public IElement @__FinalNode = new MofElement();

            public class _FlowFinalNode
            {
            }

            public _FlowFinalNode @FlowFinalNode = new _FlowFinalNode();
            public IElement @__FlowFinalNode = new MofElement();

            public class _ForkNode
            {
            }

            public _ForkNode @ForkNode = new _ForkNode();
            public IElement @__ForkNode = new MofElement();

            public class _InitialNode
            {
            }

            public _InitialNode @InitialNode = new _InitialNode();
            public IElement @__InitialNode = new MofElement();

            public class _InterruptibleActivityRegion
            {
                public string @interruptingEdge = "interruptingEdge";
                public IElement _interruptingEdge = null;

                public string @node = "node";
                public IElement _node = null;

            }

            public _InterruptibleActivityRegion @InterruptibleActivityRegion = new _InterruptibleActivityRegion();
            public IElement @__InterruptibleActivityRegion = new MofElement();

            public class _JoinNode
            {
                public string @isCombineDuplicate = "isCombineDuplicate";
                public IElement _isCombineDuplicate = null;

                public string @joinSpec = "joinSpec";
                public IElement _joinSpec = null;

            }

            public _JoinNode @JoinNode = new _JoinNode();
            public IElement @__JoinNode = new MofElement();

            public class _MergeNode
            {
            }

            public _MergeNode @MergeNode = new _MergeNode();
            public IElement @__MergeNode = new MofElement();

            public class _ObjectFlow
            {
                public string @isMulticast = "isMulticast";
                public IElement _isMulticast = null;

                public string @isMultireceive = "isMultireceive";
                public IElement _isMultireceive = null;

                public string @selection = "selection";
                public IElement _selection = null;

                public string @transformation = "transformation";
                public IElement _transformation = null;

            }

            public _ObjectFlow @ObjectFlow = new _ObjectFlow();
            public IElement @__ObjectFlow = new MofElement();

            public class _ObjectNode
            {
                public string @inState = "inState";
                public IElement _inState = null;

                public string @isControlType = "isControlType";
                public IElement _isControlType = null;

                public string @ordering = "ordering";
                public IElement _ordering = null;

                public string @selection = "selection";
                public IElement _selection = null;

                public string @upperBound = "upperBound";
                public IElement _upperBound = null;

            }

            public _ObjectNode @ObjectNode = new _ObjectNode();
            public IElement @__ObjectNode = new MofElement();

            public class _Variable
            {
                public string @activityScope = "activityScope";
                public IElement _activityScope = null;

                public string @scope = "scope";
                public IElement _scope = null;

            }

            public _Variable @Variable = new _Variable();
            public IElement @__Variable = new MofElement();

        }

        public _Activities Activities = new _Activities();

        public class _Values
        {
            public class _Duration
            {
                public string @expr = "expr";
                public IElement _expr = null;

                public string @observation = "observation";
                public IElement _observation = null;

            }

            public _Duration @Duration = new _Duration();
            public IElement @__Duration = new MofElement();

            public class _DurationConstraint
            {
                public string @firstEvent = "firstEvent";
                public IElement _firstEvent = null;

                public string @specification = "specification";
                public IElement _specification = null;

            }

            public _DurationConstraint @DurationConstraint = new _DurationConstraint();
            public IElement @__DurationConstraint = new MofElement();

            public class _DurationInterval
            {
                public string @max = "max";
                public IElement _max = null;

                public string @min = "min";
                public IElement _min = null;

            }

            public _DurationInterval @DurationInterval = new _DurationInterval();
            public IElement @__DurationInterval = new MofElement();

            public class _DurationObservation
            {
                public string @event = "event";
                public IElement _event = null;

                public string @firstEvent = "firstEvent";
                public IElement _firstEvent = null;

            }

            public _DurationObservation @DurationObservation = new _DurationObservation();
            public IElement @__DurationObservation = new MofElement();

            public class _Expression
            {
                public string @operand = "operand";
                public IElement _operand = null;

                public string @symbol = "symbol";
                public IElement _symbol = null;

            }

            public _Expression @Expression = new _Expression();
            public IElement @__Expression = new MofElement();

            public class _Interval
            {
                public string @max = "max";
                public IElement _max = null;

                public string @min = "min";
                public IElement _min = null;

            }

            public _Interval @Interval = new _Interval();
            public IElement @__Interval = new MofElement();

            public class _IntervalConstraint
            {
                public string @specification = "specification";
                public IElement _specification = null;

            }

            public _IntervalConstraint @IntervalConstraint = new _IntervalConstraint();
            public IElement @__IntervalConstraint = new MofElement();

            public class _LiteralBoolean
            {
                public string @value = "value";
                public IElement _value = null;

            }

            public _LiteralBoolean @LiteralBoolean = new _LiteralBoolean();
            public IElement @__LiteralBoolean = new MofElement();

            public class _LiteralInteger
            {
                public string @value = "value";
                public IElement _value = null;

            }

            public _LiteralInteger @LiteralInteger = new _LiteralInteger();
            public IElement @__LiteralInteger = new MofElement();

            public class _LiteralNull
            {
            }

            public _LiteralNull @LiteralNull = new _LiteralNull();
            public IElement @__LiteralNull = new MofElement();

            public class _LiteralReal
            {
                public string @value = "value";
                public IElement _value = null;

            }

            public _LiteralReal @LiteralReal = new _LiteralReal();
            public IElement @__LiteralReal = new MofElement();

            public class _LiteralSpecification
            {
            }

            public _LiteralSpecification @LiteralSpecification = new _LiteralSpecification();
            public IElement @__LiteralSpecification = new MofElement();

            public class _LiteralString
            {
                public string @value = "value";
                public IElement _value = null;

            }

            public _LiteralString @LiteralString = new _LiteralString();
            public IElement @__LiteralString = new MofElement();

            public class _LiteralUnlimitedNatural
            {
                public string @value = "value";
                public IElement _value = null;

            }

            public _LiteralUnlimitedNatural @LiteralUnlimitedNatural = new _LiteralUnlimitedNatural();
            public IElement @__LiteralUnlimitedNatural = new MofElement();

            public class _Observation
            {
            }

            public _Observation @Observation = new _Observation();
            public IElement @__Observation = new MofElement();

            public class _OpaqueExpression
            {
                public string @behavior = "behavior";
                public IElement _behavior = null;

                public string @body = "body";
                public IElement _body = null;

                public string @language = "language";
                public IElement _language = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _OpaqueExpression @OpaqueExpression = new _OpaqueExpression();
            public IElement @__OpaqueExpression = new MofElement();

            public class _StringExpression
            {
                public string @owningExpression = "owningExpression";
                public IElement _owningExpression = null;

                public string @subExpression = "subExpression";
                public IElement _subExpression = null;

            }

            public _StringExpression @StringExpression = new _StringExpression();
            public IElement @__StringExpression = new MofElement();

            public class _TimeConstraint
            {
                public string @firstEvent = "firstEvent";
                public IElement _firstEvent = null;

                public string @specification = "specification";
                public IElement _specification = null;

            }

            public _TimeConstraint @TimeConstraint = new _TimeConstraint();
            public IElement @__TimeConstraint = new MofElement();

            public class _TimeExpression
            {
                public string @expr = "expr";
                public IElement _expr = null;

                public string @observation = "observation";
                public IElement _observation = null;

            }

            public _TimeExpression @TimeExpression = new _TimeExpression();
            public IElement @__TimeExpression = new MofElement();

            public class _TimeInterval
            {
                public string @max = "max";
                public IElement _max = null;

                public string @min = "min";
                public IElement _min = null;

            }

            public _TimeInterval @TimeInterval = new _TimeInterval();
            public IElement @__TimeInterval = new MofElement();

            public class _TimeObservation
            {
                public string @event = "event";
                public IElement _event = null;

                public string @firstEvent = "firstEvent";
                public IElement _firstEvent = null;

            }

            public _TimeObservation @TimeObservation = new _TimeObservation();
            public IElement @__TimeObservation = new MofElement();

            public class _ValueSpecification
            {
            }

            public _ValueSpecification @ValueSpecification = new _ValueSpecification();
            public IElement @__ValueSpecification = new MofElement();

        }

        public _Values Values = new _Values();

        public class _UseCases
        {
            public class _Actor
            {
            }

            public _Actor @Actor = new _Actor();
            public IElement @__Actor = new MofElement();

            public class _Extend
            {
                public string @condition = "condition";
                public IElement _condition = null;

                public string @extendedCase = "extendedCase";
                public IElement _extendedCase = null;

                public string @extension = "extension";
                public IElement _extension = null;

                public string @extensionLocation = "extensionLocation";
                public IElement _extensionLocation = null;

            }

            public _Extend @Extend = new _Extend();
            public IElement @__Extend = new MofElement();

            public class _ExtensionPoint
            {
                public string @useCase = "useCase";
                public IElement _useCase = null;

            }

            public _ExtensionPoint @ExtensionPoint = new _ExtensionPoint();
            public IElement @__ExtensionPoint = new MofElement();

            public class _Include
            {
                public string @addition = "addition";
                public IElement _addition = null;

                public string @includingCase = "includingCase";
                public IElement _includingCase = null;

            }

            public _Include @Include = new _Include();
            public IElement @__Include = new MofElement();

            public class _UseCase
            {
                public string @extend = "extend";
                public IElement _extend = null;

                public string @extensionPoint = "extensionPoint";
                public IElement _extensionPoint = null;

                public string @include = "include";
                public IElement _include = null;

                public string @subject = "subject";
                public IElement _subject = null;

            }

            public _UseCase @UseCase = new _UseCase();
            public IElement @__UseCase = new MofElement();

        }

        public _UseCases UseCases = new _UseCases();

        public class _StructuredClassifiers
        {
            public class _Association
            {
                public string @endType = "endType";
                public IElement _endType = null;

                public string @isDerived = "isDerived";
                public IElement _isDerived = null;

                public string @memberEnd = "memberEnd";
                public IElement _memberEnd = null;

                public string @navigableOwnedEnd = "navigableOwnedEnd";
                public IElement _navigableOwnedEnd = null;

                public string @ownedEnd = "ownedEnd";
                public IElement _ownedEnd = null;

            }

            public _Association @Association = new _Association();
            public IElement @__Association = new MofElement();

            public class _AssociationClass
            {
            }

            public _AssociationClass @AssociationClass = new _AssociationClass();
            public IElement @__AssociationClass = new MofElement();

            public class _Class
            {
                public string @extension = "extension";
                public IElement _extension = null;

                public string @isAbstract = "isAbstract";
                public IElement _isAbstract = null;

                public string @isActive = "isActive";
                public IElement _isActive = null;

                public string @nestedClassifier = "nestedClassifier";
                public IElement _nestedClassifier = null;

                public string @ownedAttribute = "ownedAttribute";
                public IElement _ownedAttribute = null;

                public string @ownedOperation = "ownedOperation";
                public IElement _ownedOperation = null;

                public string @ownedReception = "ownedReception";
                public IElement _ownedReception = null;

                public string @superClass = "superClass";
                public IElement _superClass = null;

            }

            public _Class @Class = new _Class();
            public IElement @__Class = new MofElement();

            public class _Collaboration
            {
                public string @collaborationRole = "collaborationRole";
                public IElement _collaborationRole = null;

            }

            public _Collaboration @Collaboration = new _Collaboration();
            public IElement @__Collaboration = new MofElement();

            public class _CollaborationUse
            {
                public string @roleBinding = "roleBinding";
                public IElement _roleBinding = null;

                public string @type = "type";
                public IElement _type = null;

            }

            public _CollaborationUse @CollaborationUse = new _CollaborationUse();
            public IElement @__CollaborationUse = new MofElement();

            public class _Component
            {
                public string @isIndirectlyInstantiated = "isIndirectlyInstantiated";
                public IElement _isIndirectlyInstantiated = null;

                public string @packagedElement = "packagedElement";
                public IElement _packagedElement = null;

                public string @provided = "provided";
                public IElement _provided = null;

                public string @realization = "realization";
                public IElement _realization = null;

                public string @required = "required";
                public IElement _required = null;

            }

            public _Component @Component = new _Component();
            public IElement @__Component = new MofElement();

            public class _ComponentRealization
            {
                public string @abstraction = "abstraction";
                public IElement _abstraction = null;

                public string @realizingClassifier = "realizingClassifier";
                public IElement _realizingClassifier = null;

            }

            public _ComponentRealization @ComponentRealization = new _ComponentRealization();
            public IElement @__ComponentRealization = new MofElement();

            public class _ConnectableElement
            {
                public string @end = "end";
                public IElement _end = null;

                public string @templateParameter = "templateParameter";
                public IElement _templateParameter = null;

            }

            public _ConnectableElement @ConnectableElement = new _ConnectableElement();
            public IElement @__ConnectableElement = new MofElement();

            public class _ConnectableElementTemplateParameter
            {
                public string @parameteredElement = "parameteredElement";
                public IElement _parameteredElement = null;

            }

            public _ConnectableElementTemplateParameter @ConnectableElementTemplateParameter = new _ConnectableElementTemplateParameter();
            public IElement @__ConnectableElementTemplateParameter = new MofElement();

            public class _Connector
            {
                public string @contract = "contract";
                public IElement _contract = null;

                public string @end = "end";
                public IElement _end = null;

                public string @kind = "kind";
                public IElement _kind = null;

                public string @redefinedConnector = "redefinedConnector";
                public IElement _redefinedConnector = null;

                public string @type = "type";
                public IElement _type = null;

            }

            public _Connector @Connector = new _Connector();
            public IElement @__Connector = new MofElement();

            public class _ConnectorEnd
            {
                public string @definingEnd = "definingEnd";
                public IElement _definingEnd = null;

                public string @partWithPort = "partWithPort";
                public IElement _partWithPort = null;

                public string @role = "role";
                public IElement _role = null;

            }

            public _ConnectorEnd @ConnectorEnd = new _ConnectorEnd();
            public IElement @__ConnectorEnd = new MofElement();

            public class _EncapsulatedClassifier
            {
                public string @ownedPort = "ownedPort";
                public IElement _ownedPort = null;

            }

            public _EncapsulatedClassifier @EncapsulatedClassifier = new _EncapsulatedClassifier();
            public IElement @__EncapsulatedClassifier = new MofElement();

            public class _Port
            {
                public string @isBehavior = "isBehavior";
                public IElement _isBehavior = null;

                public string @isConjugated = "isConjugated";
                public IElement _isConjugated = null;

                public string @isService = "isService";
                public IElement _isService = null;

                public string @protocol = "protocol";
                public IElement _protocol = null;

                public string @provided = "provided";
                public IElement _provided = null;

                public string @redefinedPort = "redefinedPort";
                public IElement _redefinedPort = null;

                public string @required = "required";
                public IElement _required = null;

            }

            public _Port @Port = new _Port();
            public IElement @__Port = new MofElement();

            public class _StructuredClassifier
            {
                public string @ownedAttribute = "ownedAttribute";
                public IElement _ownedAttribute = null;

                public string @ownedConnector = "ownedConnector";
                public IElement _ownedConnector = null;

                public string @part = "part";
                public IElement _part = null;

                public string @role = "role";
                public IElement _role = null;

            }

            public _StructuredClassifier @StructuredClassifier = new _StructuredClassifier();
            public IElement @__StructuredClassifier = new MofElement();

        }

        public _StructuredClassifiers StructuredClassifiers = new _StructuredClassifiers();

        public class _StateMachines
        {
            public class _ConnectionPointReference
            {
                public string @entry = "entry";
                public IElement _entry = null;

                public string @exit = "exit";
                public IElement _exit = null;

                public string @state = "state";
                public IElement _state = null;

            }

            public _ConnectionPointReference @ConnectionPointReference = new _ConnectionPointReference();
            public IElement @__ConnectionPointReference = new MofElement();

            public class _FinalState
            {
            }

            public _FinalState @FinalState = new _FinalState();
            public IElement @__FinalState = new MofElement();

            public class _ProtocolConformance
            {
                public string @generalMachine = "generalMachine";
                public IElement _generalMachine = null;

                public string @specificMachine = "specificMachine";
                public IElement _specificMachine = null;

            }

            public _ProtocolConformance @ProtocolConformance = new _ProtocolConformance();
            public IElement @__ProtocolConformance = new MofElement();

            public class _ProtocolStateMachine
            {
                public string @conformance = "conformance";
                public IElement _conformance = null;

            }

            public _ProtocolStateMachine @ProtocolStateMachine = new _ProtocolStateMachine();
            public IElement @__ProtocolStateMachine = new MofElement();

            public class _ProtocolTransition
            {
                public string @postCondition = "postCondition";
                public IElement _postCondition = null;

                public string @preCondition = "preCondition";
                public IElement _preCondition = null;

                public string @referred = "referred";
                public IElement _referred = null;

            }

            public _ProtocolTransition @ProtocolTransition = new _ProtocolTransition();
            public IElement @__ProtocolTransition = new MofElement();

            public class _Pseudostate
            {
                public string @kind = "kind";
                public IElement _kind = null;

                public string @state = "state";
                public IElement _state = null;

                public string @stateMachine = "stateMachine";
                public IElement _stateMachine = null;

            }

            public _Pseudostate @Pseudostate = new _Pseudostate();
            public IElement @__Pseudostate = new MofElement();

            public class _Region
            {
                public string @extendedRegion = "extendedRegion";
                public IElement _extendedRegion = null;

                public string @redefinitionContext = "redefinitionContext";
                public IElement _redefinitionContext = null;

                public string @state = "state";
                public IElement _state = null;

                public string @stateMachine = "stateMachine";
                public IElement _stateMachine = null;

                public string @subvertex = "subvertex";
                public IElement _subvertex = null;

                public string @transition = "transition";
                public IElement _transition = null;

            }

            public _Region @Region = new _Region();
            public IElement @__Region = new MofElement();

            public class _State
            {
                public string @connection = "connection";
                public IElement _connection = null;

                public string @connectionPoint = "connectionPoint";
                public IElement _connectionPoint = null;

                public string @deferrableTrigger = "deferrableTrigger";
                public IElement _deferrableTrigger = null;

                public string @doActivity = "doActivity";
                public IElement _doActivity = null;

                public string @entry = "entry";
                public IElement _entry = null;

                public string @exit = "exit";
                public IElement _exit = null;

                public string @isComposite = "isComposite";
                public IElement _isComposite = null;

                public string @isOrthogonal = "isOrthogonal";
                public IElement _isOrthogonal = null;

                public string @isSimple = "isSimple";
                public IElement _isSimple = null;

                public string @isSubmachineState = "isSubmachineState";
                public IElement _isSubmachineState = null;

                public string @redefinedState = "redefinedState";
                public IElement _redefinedState = null;

                public string @redefinitionContext = "redefinitionContext";
                public IElement _redefinitionContext = null;

                public string @region = "region";
                public IElement _region = null;

                public string @stateInvariant = "stateInvariant";
                public IElement _stateInvariant = null;

                public string @submachine = "submachine";
                public IElement _submachine = null;

            }

            public _State @State = new _State();
            public IElement @__State = new MofElement();

            public class _StateMachine
            {
                public string @connectionPoint = "connectionPoint";
                public IElement _connectionPoint = null;

                public string @extendedStateMachine = "extendedStateMachine";
                public IElement _extendedStateMachine = null;

                public string @region = "region";
                public IElement _region = null;

                public string @submachineState = "submachineState";
                public IElement _submachineState = null;

            }

            public _StateMachine @StateMachine = new _StateMachine();
            public IElement @__StateMachine = new MofElement();

            public class _Transition
            {
                public string @container = "container";
                public IElement _container = null;

                public string @effect = "effect";
                public IElement _effect = null;

                public string @guard = "guard";
                public IElement _guard = null;

                public string @kind = "kind";
                public IElement _kind = null;

                public string @redefinedTransition = "redefinedTransition";
                public IElement _redefinedTransition = null;

                public string @redefinitionContext = "redefinitionContext";
                public IElement _redefinitionContext = null;

                public string @source = "source";
                public IElement _source = null;

                public string @target = "target";
                public IElement _target = null;

                public string @trigger = "trigger";
                public IElement _trigger = null;

            }

            public _Transition @Transition = new _Transition();
            public IElement @__Transition = new MofElement();

            public class _Vertex
            {
                public string @container = "container";
                public IElement _container = null;

                public string @incoming = "incoming";
                public IElement _incoming = null;

                public string @outgoing = "outgoing";
                public IElement _outgoing = null;

            }

            public _Vertex @Vertex = new _Vertex();
            public IElement @__Vertex = new MofElement();

        }

        public _StateMachines StateMachines = new _StateMachines();

        public class _SimpleClassifiers
        {
            public class _BehavioredClassifier
            {
                public string @classifierBehavior = "classifierBehavior";
                public IElement _classifierBehavior = null;

                public string @interfaceRealization = "interfaceRealization";
                public IElement _interfaceRealization = null;

                public string @ownedBehavior = "ownedBehavior";
                public IElement _ownedBehavior = null;

            }

            public _BehavioredClassifier @BehavioredClassifier = new _BehavioredClassifier();
            public IElement @__BehavioredClassifier = new MofElement();

            public class _DataType
            {
                public string @ownedAttribute = "ownedAttribute";
                public IElement _ownedAttribute = null;

                public string @ownedOperation = "ownedOperation";
                public IElement _ownedOperation = null;

            }

            public _DataType @DataType = new _DataType();
            public IElement @__DataType = new MofElement();

            public class _Enumeration
            {
                public string @ownedLiteral = "ownedLiteral";
                public IElement _ownedLiteral = null;

            }

            public _Enumeration @Enumeration = new _Enumeration();
            public IElement @__Enumeration = new MofElement();

            public class _EnumerationLiteral
            {
                public string @classifier = "classifier";
                public IElement _classifier = null;

                public string @enumeration = "enumeration";
                public IElement _enumeration = null;

            }

            public _EnumerationLiteral @EnumerationLiteral = new _EnumerationLiteral();
            public IElement @__EnumerationLiteral = new MofElement();

            public class _Interface
            {
                public string @nestedClassifier = "nestedClassifier";
                public IElement _nestedClassifier = null;

                public string @ownedAttribute = "ownedAttribute";
                public IElement _ownedAttribute = null;

                public string @ownedOperation = "ownedOperation";
                public IElement _ownedOperation = null;

                public string @ownedReception = "ownedReception";
                public IElement _ownedReception = null;

                public string @protocol = "protocol";
                public IElement _protocol = null;

                public string @redefinedInterface = "redefinedInterface";
                public IElement _redefinedInterface = null;

            }

            public _Interface @Interface = new _Interface();
            public IElement @__Interface = new MofElement();

            public class _InterfaceRealization
            {
                public string @contract = "contract";
                public IElement _contract = null;

                public string @implementingClassifier = "implementingClassifier";
                public IElement _implementingClassifier = null;

            }

            public _InterfaceRealization @InterfaceRealization = new _InterfaceRealization();
            public IElement @__InterfaceRealization = new MofElement();

            public class _PrimitiveType
            {
            }

            public _PrimitiveType @PrimitiveType = new _PrimitiveType();
            public IElement @__PrimitiveType = new MofElement();

            public class _Reception
            {
                public string @signal = "signal";
                public IElement _signal = null;

            }

            public _Reception @Reception = new _Reception();
            public IElement @__Reception = new MofElement();

            public class _Signal
            {
                public string @ownedAttribute = "ownedAttribute";
                public IElement _ownedAttribute = null;

            }

            public _Signal @Signal = new _Signal();
            public IElement @__Signal = new MofElement();

        }

        public _SimpleClassifiers SimpleClassifiers = new _SimpleClassifiers();

        public class _Packages
        {
            public class _Extension
            {
                public string @isRequired = "isRequired";
                public IElement _isRequired = null;

                public string @metaclass = "metaclass";
                public IElement _metaclass = null;

                public string @ownedEnd = "ownedEnd";
                public IElement _ownedEnd = null;

            }

            public _Extension @Extension = new _Extension();
            public IElement @__Extension = new MofElement();

            public class _ExtensionEnd
            {
                public string @lower = "lower";
                public IElement _lower = null;

                public string @type = "type";
                public IElement _type = null;

            }

            public _ExtensionEnd @ExtensionEnd = new _ExtensionEnd();
            public IElement @__ExtensionEnd = new MofElement();

            public class _Image
            {
                public string @content = "content";
                public IElement _content = null;

                public string @format = "format";
                public IElement _format = null;

                public string @location = "location";
                public IElement _location = null;

            }

            public _Image @Image = new _Image();
            public IElement @__Image = new MofElement();

            public class _Model
            {
                public string @viewpoint = "viewpoint";
                public IElement _viewpoint = null;

            }

            public _Model @Model = new _Model();
            public IElement @__Model = new MofElement();

            public class _Package
            {
                public string @URI = "URI";
                public IElement _URI = null;

                public string @nestedPackage = "nestedPackage";
                public IElement _nestedPackage = null;

                public string @nestingPackage = "nestingPackage";
                public IElement _nestingPackage = null;

                public string @ownedStereotype = "ownedStereotype";
                public IElement _ownedStereotype = null;

                public string @ownedType = "ownedType";
                public IElement _ownedType = null;

                public string @packageMerge = "packageMerge";
                public IElement _packageMerge = null;

                public string @packagedElement = "packagedElement";
                public IElement _packagedElement = null;

                public string @profileApplication = "profileApplication";
                public IElement _profileApplication = null;

            }

            public _Package @Package = new _Package();
            public IElement @__Package = new MofElement();

            public class _PackageMerge
            {
                public string @mergedPackage = "mergedPackage";
                public IElement _mergedPackage = null;

                public string @receivingPackage = "receivingPackage";
                public IElement _receivingPackage = null;

            }

            public _PackageMerge @PackageMerge = new _PackageMerge();
            public IElement @__PackageMerge = new MofElement();

            public class _Profile
            {
                public string @metaclassReference = "metaclassReference";
                public IElement _metaclassReference = null;

                public string @metamodelReference = "metamodelReference";
                public IElement _metamodelReference = null;

            }

            public _Profile @Profile = new _Profile();
            public IElement @__Profile = new MofElement();

            public class _ProfileApplication
            {
                public string @appliedProfile = "appliedProfile";
                public IElement _appliedProfile = null;

                public string @applyingPackage = "applyingPackage";
                public IElement _applyingPackage = null;

                public string @isStrict = "isStrict";
                public IElement _isStrict = null;

            }

            public _ProfileApplication @ProfileApplication = new _ProfileApplication();
            public IElement @__ProfileApplication = new MofElement();

            public class _Stereotype
            {
                public string @icon = "icon";
                public IElement _icon = null;

                public string @profile = "profile";
                public IElement _profile = null;

            }

            public _Stereotype @Stereotype = new _Stereotype();
            public IElement @__Stereotype = new MofElement();

        }

        public _Packages Packages = new _Packages();

        public class _Interactions
        {
            public class _ActionExecutionSpecification
            {
                public string @action = "action";
                public IElement _action = null;

            }

            public _ActionExecutionSpecification @ActionExecutionSpecification = new _ActionExecutionSpecification();
            public IElement @__ActionExecutionSpecification = new MofElement();

            public class _BehaviorExecutionSpecification
            {
                public string @behavior = "behavior";
                public IElement _behavior = null;

            }

            public _BehaviorExecutionSpecification @BehaviorExecutionSpecification = new _BehaviorExecutionSpecification();
            public IElement @__BehaviorExecutionSpecification = new MofElement();

            public class _CombinedFragment
            {
                public string @cfragmentGate = "cfragmentGate";
                public IElement _cfragmentGate = null;

                public string @interactionOperator = "interactionOperator";
                public IElement _interactionOperator = null;

                public string @operand = "operand";
                public IElement _operand = null;

            }

            public _CombinedFragment @CombinedFragment = new _CombinedFragment();
            public IElement @__CombinedFragment = new MofElement();

            public class _ConsiderIgnoreFragment
            {
                public string @message = "message";
                public IElement _message = null;

            }

            public _ConsiderIgnoreFragment @ConsiderIgnoreFragment = new _ConsiderIgnoreFragment();
            public IElement @__ConsiderIgnoreFragment = new MofElement();

            public class _Continuation
            {
                public string @setting = "setting";
                public IElement _setting = null;

            }

            public _Continuation @Continuation = new _Continuation();
            public IElement @__Continuation = new MofElement();

            public class _DestructionOccurrenceSpecification
            {
            }

            public _DestructionOccurrenceSpecification @DestructionOccurrenceSpecification = new _DestructionOccurrenceSpecification();
            public IElement @__DestructionOccurrenceSpecification = new MofElement();

            public class _ExecutionOccurrenceSpecification
            {
                public string @execution = "execution";
                public IElement _execution = null;

            }

            public _ExecutionOccurrenceSpecification @ExecutionOccurrenceSpecification = new _ExecutionOccurrenceSpecification();
            public IElement @__ExecutionOccurrenceSpecification = new MofElement();

            public class _ExecutionSpecification
            {
                public string @finish = "finish";
                public IElement _finish = null;

                public string @start = "start";
                public IElement _start = null;

            }

            public _ExecutionSpecification @ExecutionSpecification = new _ExecutionSpecification();
            public IElement @__ExecutionSpecification = new MofElement();

            public class _Gate
            {
            }

            public _Gate @Gate = new _Gate();
            public IElement @__Gate = new MofElement();

            public class _GeneralOrdering
            {
                public string @after = "after";
                public IElement _after = null;

                public string @before = "before";
                public IElement _before = null;

            }

            public _GeneralOrdering @GeneralOrdering = new _GeneralOrdering();
            public IElement @__GeneralOrdering = new MofElement();

            public class _Interaction
            {
                public string @action = "action";
                public IElement _action = null;

                public string @formalGate = "formalGate";
                public IElement _formalGate = null;

                public string @fragment = "fragment";
                public IElement _fragment = null;

                public string @lifeline = "lifeline";
                public IElement _lifeline = null;

                public string @message = "message";
                public IElement _message = null;

            }

            public _Interaction @Interaction = new _Interaction();
            public IElement @__Interaction = new MofElement();

            public class _InteractionConstraint
            {
                public string @maxint = "maxint";
                public IElement _maxint = null;

                public string @minint = "minint";
                public IElement _minint = null;

            }

            public _InteractionConstraint @InteractionConstraint = new _InteractionConstraint();
            public IElement @__InteractionConstraint = new MofElement();

            public class _InteractionFragment
            {
                public string @covered = "covered";
                public IElement _covered = null;

                public string @enclosingInteraction = "enclosingInteraction";
                public IElement _enclosingInteraction = null;

                public string @enclosingOperand = "enclosingOperand";
                public IElement _enclosingOperand = null;

                public string @generalOrdering = "generalOrdering";
                public IElement _generalOrdering = null;

            }

            public _InteractionFragment @InteractionFragment = new _InteractionFragment();
            public IElement @__InteractionFragment = new MofElement();

            public class _InteractionOperand
            {
                public string @fragment = "fragment";
                public IElement _fragment = null;

                public string @guard = "guard";
                public IElement _guard = null;

            }

            public _InteractionOperand @InteractionOperand = new _InteractionOperand();
            public IElement @__InteractionOperand = new MofElement();

            public class _InteractionUse
            {
                public string @actualGate = "actualGate";
                public IElement _actualGate = null;

                public string @argument = "argument";
                public IElement _argument = null;

                public string @refersTo = "refersTo";
                public IElement _refersTo = null;

                public string @returnValue = "returnValue";
                public IElement _returnValue = null;

                public string @returnValueRecipient = "returnValueRecipient";
                public IElement _returnValueRecipient = null;

            }

            public _InteractionUse @InteractionUse = new _InteractionUse();
            public IElement @__InteractionUse = new MofElement();

            public class _Lifeline
            {
                public string @coveredBy = "coveredBy";
                public IElement _coveredBy = null;

                public string @decomposedAs = "decomposedAs";
                public IElement _decomposedAs = null;

                public string @interaction = "interaction";
                public IElement _interaction = null;

                public string @represents = "represents";
                public IElement _represents = null;

                public string @selector = "selector";
                public IElement _selector = null;

            }

            public _Lifeline @Lifeline = new _Lifeline();
            public IElement @__Lifeline = new MofElement();

            public class _Message
            {
                public string @argument = "argument";
                public IElement _argument = null;

                public string @connector = "connector";
                public IElement _connector = null;

                public string @interaction = "interaction";
                public IElement _interaction = null;

                public string @messageKind = "messageKind";
                public IElement _messageKind = null;

                public string @messageSort = "messageSort";
                public IElement _messageSort = null;

                public string @receiveEvent = "receiveEvent";
                public IElement _receiveEvent = null;

                public string @sendEvent = "sendEvent";
                public IElement _sendEvent = null;

                public string @signature = "signature";
                public IElement _signature = null;

            }

            public _Message @Message = new _Message();
            public IElement @__Message = new MofElement();

            public class _MessageEnd
            {
                public string @message = "message";
                public IElement _message = null;

            }

            public _MessageEnd @MessageEnd = new _MessageEnd();
            public IElement @__MessageEnd = new MofElement();

            public class _MessageOccurrenceSpecification
            {
            }

            public _MessageOccurrenceSpecification @MessageOccurrenceSpecification = new _MessageOccurrenceSpecification();
            public IElement @__MessageOccurrenceSpecification = new MofElement();

            public class _OccurrenceSpecification
            {
                public string @covered = "covered";
                public IElement _covered = null;

                public string @toAfter = "toAfter";
                public IElement _toAfter = null;

                public string @toBefore = "toBefore";
                public IElement _toBefore = null;

            }

            public _OccurrenceSpecification @OccurrenceSpecification = new _OccurrenceSpecification();
            public IElement @__OccurrenceSpecification = new MofElement();

            public class _PartDecomposition
            {
            }

            public _PartDecomposition @PartDecomposition = new _PartDecomposition();
            public IElement @__PartDecomposition = new MofElement();

            public class _StateInvariant
            {
                public string @covered = "covered";
                public IElement _covered = null;

                public string @invariant = "invariant";
                public IElement _invariant = null;

            }

            public _StateInvariant @StateInvariant = new _StateInvariant();
            public IElement @__StateInvariant = new MofElement();

        }

        public _Interactions Interactions = new _Interactions();

        public class _InformationFlows
        {
            public class _InformationFlow
            {
                public string @conveyed = "conveyed";
                public IElement _conveyed = null;

                public string @informationSource = "informationSource";
                public IElement _informationSource = null;

                public string @informationTarget = "informationTarget";
                public IElement _informationTarget = null;

                public string @realization = "realization";
                public IElement _realization = null;

                public string @realizingActivityEdge = "realizingActivityEdge";
                public IElement _realizingActivityEdge = null;

                public string @realizingConnector = "realizingConnector";
                public IElement _realizingConnector = null;

                public string @realizingMessage = "realizingMessage";
                public IElement _realizingMessage = null;

            }

            public _InformationFlow @InformationFlow = new _InformationFlow();
            public IElement @__InformationFlow = new MofElement();

            public class _InformationItem
            {
                public string @represented = "represented";
                public IElement _represented = null;

            }

            public _InformationItem @InformationItem = new _InformationItem();
            public IElement @__InformationItem = new MofElement();

        }

        public _InformationFlows InformationFlows = new _InformationFlows();

        public class _Deployments
        {
            public class _Artifact
            {
                public string @fileName = "fileName";
                public IElement _fileName = null;

                public string @manifestation = "manifestation";
                public IElement _manifestation = null;

                public string @nestedArtifact = "nestedArtifact";
                public IElement _nestedArtifact = null;

                public string @ownedAttribute = "ownedAttribute";
                public IElement _ownedAttribute = null;

                public string @ownedOperation = "ownedOperation";
                public IElement _ownedOperation = null;

            }

            public _Artifact @Artifact = new _Artifact();
            public IElement @__Artifact = new MofElement();

            public class _CommunicationPath
            {
            }

            public _CommunicationPath @CommunicationPath = new _CommunicationPath();
            public IElement @__CommunicationPath = new MofElement();

            public class _DeployedArtifact
            {
            }

            public _DeployedArtifact @DeployedArtifact = new _DeployedArtifact();
            public IElement @__DeployedArtifact = new MofElement();

            public class _Deployment
            {
                public string @configuration = "configuration";
                public IElement _configuration = null;

                public string @deployedArtifact = "deployedArtifact";
                public IElement _deployedArtifact = null;

                public string @location = "location";
                public IElement _location = null;

            }

            public _Deployment @Deployment = new _Deployment();
            public IElement @__Deployment = new MofElement();

            public class _DeploymentSpecification
            {
                public string @deployment = "deployment";
                public IElement _deployment = null;

                public string @deploymentLocation = "deploymentLocation";
                public IElement _deploymentLocation = null;

                public string @executionLocation = "executionLocation";
                public IElement _executionLocation = null;

            }

            public _DeploymentSpecification @DeploymentSpecification = new _DeploymentSpecification();
            public IElement @__DeploymentSpecification = new MofElement();

            public class _DeploymentTarget
            {
                public string @deployedElement = "deployedElement";
                public IElement _deployedElement = null;

                public string @deployment = "deployment";
                public IElement _deployment = null;

            }

            public _DeploymentTarget @DeploymentTarget = new _DeploymentTarget();
            public IElement @__DeploymentTarget = new MofElement();

            public class _Device
            {
            }

            public _Device @Device = new _Device();
            public IElement @__Device = new MofElement();

            public class _ExecutionEnvironment
            {
            }

            public _ExecutionEnvironment @ExecutionEnvironment = new _ExecutionEnvironment();
            public IElement @__ExecutionEnvironment = new MofElement();

            public class _Manifestation
            {
                public string @utilizedElement = "utilizedElement";
                public IElement _utilizedElement = null;

            }

            public _Manifestation @Manifestation = new _Manifestation();
            public IElement @__Manifestation = new MofElement();

            public class _Node
            {
                public string @nestedNode = "nestedNode";
                public IElement _nestedNode = null;

            }

            public _Node @Node = new _Node();
            public IElement @__Node = new MofElement();

        }

        public _Deployments Deployments = new _Deployments();

        public class _CommonStructure
        {
            public class _Abstraction
            {
                public string @mapping = "mapping";
                public IElement _mapping = null;

            }

            public _Abstraction @Abstraction = new _Abstraction();
            public IElement @__Abstraction = new MofElement();

            public class _Comment
            {
                public string @annotatedElement = "annotatedElement";
                public IElement _annotatedElement = null;

                public string @body = "body";
                public IElement _body = null;

            }

            public _Comment @Comment = new _Comment();
            public IElement @__Comment = new MofElement();

            public class _Constraint
            {
                public string @constrainedElement = "constrainedElement";
                public IElement _constrainedElement = null;

                public string @context = "context";
                public IElement _context = null;

                public string @specification = "specification";
                public IElement _specification = null;

            }

            public _Constraint @Constraint = new _Constraint();
            public IElement @__Constraint = new MofElement();

            public class _Dependency
            {
                public string @client = "client";
                public IElement _client = null;

                public string @supplier = "supplier";
                public IElement _supplier = null;

            }

            public _Dependency @Dependency = new _Dependency();
            public IElement @__Dependency = new MofElement();

            public class _DirectedRelationship
            {
                public string @source = "source";
                public IElement _source = null;

                public string @target = "target";
                public IElement _target = null;

            }

            public _DirectedRelationship @DirectedRelationship = new _DirectedRelationship();
            public IElement @__DirectedRelationship = new MofElement();

            public class _Element
            {
                public string @ownedComment = "ownedComment";
                public IElement _ownedComment = null;

                public string @ownedElement = "ownedElement";
                public IElement _ownedElement = null;

                public string @owner = "owner";
                public IElement _owner = null;

            }

            public _Element @Element = new _Element();
            public IElement @__Element = new MofElement();

            public class _ElementImport
            {
                public string @alias = "alias";
                public IElement _alias = null;

                public string @importedElement = "importedElement";
                public IElement _importedElement = null;

                public string @importingNamespace = "importingNamespace";
                public IElement _importingNamespace = null;

                public string @visibility = "visibility";
                public IElement _visibility = null;

            }

            public _ElementImport @ElementImport = new _ElementImport();
            public IElement @__ElementImport = new MofElement();

            public class _MultiplicityElement
            {
                public string @isOrdered = "isOrdered";
                public IElement _isOrdered = null;

                public string @isUnique = "isUnique";
                public IElement _isUnique = null;

                public string @lower = "lower";
                public IElement _lower = null;

                public string @lowerValue = "lowerValue";
                public IElement _lowerValue = null;

                public string @upper = "upper";
                public IElement _upper = null;

                public string @upperValue = "upperValue";
                public IElement _upperValue = null;

            }

            public _MultiplicityElement @MultiplicityElement = new _MultiplicityElement();
            public IElement @__MultiplicityElement = new MofElement();

            public class _NamedElement
            {
                public string @clientDependency = "clientDependency";
                public IElement _clientDependency = null;

                public string @name = "name";
                public IElement _name = null;

                public string @nameExpression = "nameExpression";
                public IElement _nameExpression = null;

                public string @namespace = "namespace";
                public IElement _namespace = null;

                public string @qualifiedName = "qualifiedName";
                public IElement _qualifiedName = null;

                public string @visibility = "visibility";
                public IElement _visibility = null;

            }

            public _NamedElement @NamedElement = new _NamedElement();
            public IElement @__NamedElement = new MofElement();

            public class _Namespace
            {
                public string @elementImport = "elementImport";
                public IElement _elementImport = null;

                public string @importedMember = "importedMember";
                public IElement _importedMember = null;

                public string @member = "member";
                public IElement _member = null;

                public string @ownedMember = "ownedMember";
                public IElement _ownedMember = null;

                public string @ownedRule = "ownedRule";
                public IElement _ownedRule = null;

                public string @packageImport = "packageImport";
                public IElement _packageImport = null;

            }

            public _Namespace @Namespace = new _Namespace();
            public IElement @__Namespace = new MofElement();

            public class _PackageableElement
            {
                public string @visibility = "visibility";
                public IElement _visibility = null;

            }

            public _PackageableElement @PackageableElement = new _PackageableElement();
            public IElement @__PackageableElement = new MofElement();

            public class _PackageImport
            {
                public string @importedPackage = "importedPackage";
                public IElement _importedPackage = null;

                public string @importingNamespace = "importingNamespace";
                public IElement _importingNamespace = null;

                public string @visibility = "visibility";
                public IElement _visibility = null;

            }

            public _PackageImport @PackageImport = new _PackageImport();
            public IElement @__PackageImport = new MofElement();

            public class _ParameterableElement
            {
                public string @owningTemplateParameter = "owningTemplateParameter";
                public IElement _owningTemplateParameter = null;

                public string @templateParameter = "templateParameter";
                public IElement _templateParameter = null;

            }

            public _ParameterableElement @ParameterableElement = new _ParameterableElement();
            public IElement @__ParameterableElement = new MofElement();

            public class _Realization
            {
            }

            public _Realization @Realization = new _Realization();
            public IElement @__Realization = new MofElement();

            public class _Relationship
            {
                public string @relatedElement = "relatedElement";
                public IElement _relatedElement = null;

            }

            public _Relationship @Relationship = new _Relationship();
            public IElement @__Relationship = new MofElement();

            public class _TemplateableElement
            {
                public string @ownedTemplateSignature = "ownedTemplateSignature";
                public IElement _ownedTemplateSignature = null;

                public string @templateBinding = "templateBinding";
                public IElement _templateBinding = null;

            }

            public _TemplateableElement @TemplateableElement = new _TemplateableElement();
            public IElement @__TemplateableElement = new MofElement();

            public class _TemplateBinding
            {
                public string @boundElement = "boundElement";
                public IElement _boundElement = null;

                public string @parameterSubstitution = "parameterSubstitution";
                public IElement _parameterSubstitution = null;

                public string @signature = "signature";
                public IElement _signature = null;

            }

            public _TemplateBinding @TemplateBinding = new _TemplateBinding();
            public IElement @__TemplateBinding = new MofElement();

            public class _TemplateParameter
            {
                public string @default = "default";
                public IElement _default = null;

                public string @ownedDefault = "ownedDefault";
                public IElement _ownedDefault = null;

                public string @ownedParameteredElement = "ownedParameteredElement";
                public IElement _ownedParameteredElement = null;

                public string @parameteredElement = "parameteredElement";
                public IElement _parameteredElement = null;

                public string @signature = "signature";
                public IElement _signature = null;

            }

            public _TemplateParameter @TemplateParameter = new _TemplateParameter();
            public IElement @__TemplateParameter = new MofElement();

            public class _TemplateParameterSubstitution
            {
                public string @actual = "actual";
                public IElement _actual = null;

                public string @formal = "formal";
                public IElement _formal = null;

                public string @ownedActual = "ownedActual";
                public IElement _ownedActual = null;

                public string @templateBinding = "templateBinding";
                public IElement _templateBinding = null;

            }

            public _TemplateParameterSubstitution @TemplateParameterSubstitution = new _TemplateParameterSubstitution();
            public IElement @__TemplateParameterSubstitution = new MofElement();

            public class _TemplateSignature
            {
                public string @ownedParameter = "ownedParameter";
                public IElement _ownedParameter = null;

                public string @parameter = "parameter";
                public IElement _parameter = null;

                public string @template = "template";
                public IElement _template = null;

            }

            public _TemplateSignature @TemplateSignature = new _TemplateSignature();
            public IElement @__TemplateSignature = new MofElement();

            public class _Type
            {
                public string @package = "package";
                public IElement _package = null;

            }

            public _Type @Type = new _Type();
            public IElement @__Type = new MofElement();

            public class _TypedElement
            {
                public string @type = "type";
                public IElement _type = null;

            }

            public _TypedElement @TypedElement = new _TypedElement();
            public IElement @__TypedElement = new MofElement();

            public class _Usage
            {
            }

            public _Usage @Usage = new _Usage();
            public IElement @__Usage = new MofElement();

        }

        public _CommonStructure CommonStructure = new _CommonStructure();

        public class _CommonBehavior
        {
            public class _AnyReceiveEvent
            {
            }

            public _AnyReceiveEvent @AnyReceiveEvent = new _AnyReceiveEvent();
            public IElement @__AnyReceiveEvent = new MofElement();

            public class _Behavior
            {
                public string @context = "context";
                public IElement _context = null;

                public string @isReentrant = "isReentrant";
                public IElement _isReentrant = null;

                public string @ownedParameter = "ownedParameter";
                public IElement _ownedParameter = null;

                public string @ownedParameterSet = "ownedParameterSet";
                public IElement _ownedParameterSet = null;

                public string @postcondition = "postcondition";
                public IElement _postcondition = null;

                public string @precondition = "precondition";
                public IElement _precondition = null;

                public string @specification = "specification";
                public IElement _specification = null;

                public string @redefinedBehavior = "redefinedBehavior";
                public IElement _redefinedBehavior = null;

            }

            public _Behavior @Behavior = new _Behavior();
            public IElement @__Behavior = new MofElement();

            public class _CallEvent
            {
                public string @operation = "operation";
                public IElement _operation = null;

            }

            public _CallEvent @CallEvent = new _CallEvent();
            public IElement @__CallEvent = new MofElement();

            public class _ChangeEvent
            {
                public string @changeExpression = "changeExpression";
                public IElement _changeExpression = null;

            }

            public _ChangeEvent @ChangeEvent = new _ChangeEvent();
            public IElement @__ChangeEvent = new MofElement();

            public class _Event
            {
            }

            public _Event @Event = new _Event();
            public IElement @__Event = new MofElement();

            public class _FunctionBehavior
            {
            }

            public _FunctionBehavior @FunctionBehavior = new _FunctionBehavior();
            public IElement @__FunctionBehavior = new MofElement();

            public class _MessageEvent
            {
            }

            public _MessageEvent @MessageEvent = new _MessageEvent();
            public IElement @__MessageEvent = new MofElement();

            public class _OpaqueBehavior
            {
                public string @body = "body";
                public IElement _body = null;

                public string @language = "language";
                public IElement _language = null;

            }

            public _OpaqueBehavior @OpaqueBehavior = new _OpaqueBehavior();
            public IElement @__OpaqueBehavior = new MofElement();

            public class _SignalEvent
            {
                public string @signal = "signal";
                public IElement _signal = null;

            }

            public _SignalEvent @SignalEvent = new _SignalEvent();
            public IElement @__SignalEvent = new MofElement();

            public class _TimeEvent
            {
                public string @isRelative = "isRelative";
                public IElement _isRelative = null;

                public string @when = "when";
                public IElement _when = null;

            }

            public _TimeEvent @TimeEvent = new _TimeEvent();
            public IElement @__TimeEvent = new MofElement();

            public class _Trigger
            {
                public string @event = "event";
                public IElement _event = null;

                public string @port = "port";
                public IElement _port = null;

            }

            public _Trigger @Trigger = new _Trigger();
            public IElement @__Trigger = new MofElement();

        }

        public _CommonBehavior CommonBehavior = new _CommonBehavior();

        public class _Classification
        {
            public class _Substitution
            {
                public string @contract = "contract";
                public IElement _contract = null;

                public string @substitutingClassifier = "substitutingClassifier";
                public IElement _substitutingClassifier = null;

            }

            public _Substitution @Substitution = new _Substitution();
            public IElement @__Substitution = new MofElement();

            public class _BehavioralFeature
            {
                public string @concurrency = "concurrency";
                public IElement _concurrency = null;

                public string @isAbstract = "isAbstract";
                public IElement _isAbstract = null;

                public string @method = "method";
                public IElement _method = null;

                public string @ownedParameter = "ownedParameter";
                public IElement _ownedParameter = null;

                public string @ownedParameterSet = "ownedParameterSet";
                public IElement _ownedParameterSet = null;

                public string @raisedException = "raisedException";
                public IElement _raisedException = null;

            }

            public _BehavioralFeature @BehavioralFeature = new _BehavioralFeature();
            public IElement @__BehavioralFeature = new MofElement();

            public class _Classifier
            {
                public string @attribute = "attribute";
                public IElement _attribute = null;

                public string @collaborationUse = "collaborationUse";
                public IElement _collaborationUse = null;

                public string @feature = "feature";
                public IElement _feature = null;

                public string @general = "general";
                public IElement _general = null;

                public string @generalization = "generalization";
                public IElement _generalization = null;

                public string @inheritedMember = "inheritedMember";
                public IElement _inheritedMember = null;

                public string @isAbstract = "isAbstract";
                public IElement _isAbstract = null;

                public string @isFinalSpecialization = "isFinalSpecialization";
                public IElement _isFinalSpecialization = null;

                public string @ownedTemplateSignature = "ownedTemplateSignature";
                public IElement _ownedTemplateSignature = null;

                public string @ownedUseCase = "ownedUseCase";
                public IElement _ownedUseCase = null;

                public string @powertypeExtent = "powertypeExtent";
                public IElement _powertypeExtent = null;

                public string @redefinedClassifier = "redefinedClassifier";
                public IElement _redefinedClassifier = null;

                public string @representation = "representation";
                public IElement _representation = null;

                public string @substitution = "substitution";
                public IElement _substitution = null;

                public string @templateParameter = "templateParameter";
                public IElement _templateParameter = null;

                public string @useCase = "useCase";
                public IElement _useCase = null;

            }

            public _Classifier @Classifier = new _Classifier();
            public IElement @__Classifier = new MofElement();

            public class _ClassifierTemplateParameter
            {
                public string @allowSubstitutable = "allowSubstitutable";
                public IElement _allowSubstitutable = null;

                public string @constrainingClassifier = "constrainingClassifier";
                public IElement _constrainingClassifier = null;

                public string @parameteredElement = "parameteredElement";
                public IElement _parameteredElement = null;

            }

            public _ClassifierTemplateParameter @ClassifierTemplateParameter = new _ClassifierTemplateParameter();
            public IElement @__ClassifierTemplateParameter = new MofElement();

            public class _Feature
            {
                public string @featuringClassifier = "featuringClassifier";
                public IElement _featuringClassifier = null;

                public string @isStatic = "isStatic";
                public IElement _isStatic = null;

            }

            public _Feature @Feature = new _Feature();
            public IElement @__Feature = new MofElement();

            public class _Generalization
            {
                public string @general = "general";
                public IElement _general = null;

                public string @generalizationSet = "generalizationSet";
                public IElement _generalizationSet = null;

                public string @isSubstitutable = "isSubstitutable";
                public IElement _isSubstitutable = null;

                public string @specific = "specific";
                public IElement _specific = null;

            }

            public _Generalization @Generalization = new _Generalization();
            public IElement @__Generalization = new MofElement();

            public class _GeneralizationSet
            {
                public string @generalization = "generalization";
                public IElement _generalization = null;

                public string @isCovering = "isCovering";
                public IElement _isCovering = null;

                public string @isDisjoint = "isDisjoint";
                public IElement _isDisjoint = null;

                public string @powertype = "powertype";
                public IElement _powertype = null;

            }

            public _GeneralizationSet @GeneralizationSet = new _GeneralizationSet();
            public IElement @__GeneralizationSet = new MofElement();

            public class _InstanceSpecification
            {
                public string @classifier = "classifier";
                public IElement _classifier = null;

                public string @slot = "slot";
                public IElement _slot = null;

                public string @specification = "specification";
                public IElement _specification = null;

            }

            public _InstanceSpecification @InstanceSpecification = new _InstanceSpecification();
            public IElement @__InstanceSpecification = new MofElement();

            public class _InstanceValue
            {
                public string @instance = "instance";
                public IElement _instance = null;

            }

            public _InstanceValue @InstanceValue = new _InstanceValue();
            public IElement @__InstanceValue = new MofElement();

            public class _Operation
            {
                public string @bodyCondition = "bodyCondition";
                public IElement _bodyCondition = null;

                public string @class = "class";
                public IElement _class = null;

                public string @datatype = "datatype";
                public IElement _datatype = null;

                public string @interface = "interface";
                public IElement _interface = null;

                public string @isOrdered = "isOrdered";
                public IElement _isOrdered = null;

                public string @isQuery = "isQuery";
                public IElement _isQuery = null;

                public string @isUnique = "isUnique";
                public IElement _isUnique = null;

                public string @lower = "lower";
                public IElement _lower = null;

                public string @ownedParameter = "ownedParameter";
                public IElement _ownedParameter = null;

                public string @postcondition = "postcondition";
                public IElement _postcondition = null;

                public string @precondition = "precondition";
                public IElement _precondition = null;

                public string @raisedException = "raisedException";
                public IElement _raisedException = null;

                public string @redefinedOperation = "redefinedOperation";
                public IElement _redefinedOperation = null;

                public string @templateParameter = "templateParameter";
                public IElement _templateParameter = null;

                public string @type = "type";
                public IElement _type = null;

                public string @upper = "upper";
                public IElement _upper = null;

            }

            public _Operation @Operation = new _Operation();
            public IElement @__Operation = new MofElement();

            public class _OperationTemplateParameter
            {
                public string @parameteredElement = "parameteredElement";
                public IElement _parameteredElement = null;

            }

            public _OperationTemplateParameter @OperationTemplateParameter = new _OperationTemplateParameter();
            public IElement @__OperationTemplateParameter = new MofElement();

            public class _Parameter
            {
                public string @default = "default";
                public IElement _default = null;

                public string @defaultValue = "defaultValue";
                public IElement _defaultValue = null;

                public string @direction = "direction";
                public IElement _direction = null;

                public string @effect = "effect";
                public IElement _effect = null;

                public string @isException = "isException";
                public IElement _isException = null;

                public string @isStream = "isStream";
                public IElement _isStream = null;

                public string @operation = "operation";
                public IElement _operation = null;

                public string @parameterSet = "parameterSet";
                public IElement _parameterSet = null;

            }

            public _Parameter @Parameter = new _Parameter();
            public IElement @__Parameter = new MofElement();

            public class _ParameterSet
            {
                public string @condition = "condition";
                public IElement _condition = null;

                public string @parameter = "parameter";
                public IElement _parameter = null;

            }

            public _ParameterSet @ParameterSet = new _ParameterSet();
            public IElement @__ParameterSet = new MofElement();

            public class _Property
            {
                public string @aggregation = "aggregation";
                public IElement _aggregation = null;

                public string @association = "association";
                public IElement _association = null;

                public string @associationEnd = "associationEnd";
                public IElement _associationEnd = null;

                public string @class = "class";
                public IElement _class = null;

                public string @datatype = "datatype";
                public IElement _datatype = null;

                public string @defaultValue = "defaultValue";
                public IElement _defaultValue = null;

                public string @interface = "interface";
                public IElement _interface = null;

                public string @isComposite = "isComposite";
                public IElement _isComposite = null;

                public string @isDerived = "isDerived";
                public IElement _isDerived = null;

                public string @isDerivedUnion = "isDerivedUnion";
                public IElement _isDerivedUnion = null;

                public string @isID = "isID";
                public IElement _isID = null;

                public string @opposite = "opposite";
                public IElement _opposite = null;

                public string @owningAssociation = "owningAssociation";
                public IElement _owningAssociation = null;

                public string @qualifier = "qualifier";
                public IElement _qualifier = null;

                public string @redefinedProperty = "redefinedProperty";
                public IElement _redefinedProperty = null;

                public string @subsettedProperty = "subsettedProperty";
                public IElement _subsettedProperty = null;

            }

            public _Property @Property = new _Property();
            public IElement @__Property = new MofElement();

            public class _RedefinableElement
            {
                public string @isLeaf = "isLeaf";
                public IElement _isLeaf = null;

                public string @redefinedElement = "redefinedElement";
                public IElement _redefinedElement = null;

                public string @redefinitionContext = "redefinitionContext";
                public IElement _redefinitionContext = null;

            }

            public _RedefinableElement @RedefinableElement = new _RedefinableElement();
            public IElement @__RedefinableElement = new MofElement();

            public class _RedefinableTemplateSignature
            {
                public string @classifier = "classifier";
                public IElement _classifier = null;

                public string @extendedSignature = "extendedSignature";
                public IElement _extendedSignature = null;

                public string @inheritedParameter = "inheritedParameter";
                public IElement _inheritedParameter = null;

            }

            public _RedefinableTemplateSignature @RedefinableTemplateSignature = new _RedefinableTemplateSignature();
            public IElement @__RedefinableTemplateSignature = new MofElement();

            public class _Slot
            {
                public string @definingFeature = "definingFeature";
                public IElement _definingFeature = null;

                public string @owningInstance = "owningInstance";
                public IElement _owningInstance = null;

                public string @value = "value";
                public IElement _value = null;

            }

            public _Slot @Slot = new _Slot();
            public IElement @__Slot = new MofElement();

            public class _StructuralFeature
            {
                public string @isReadOnly = "isReadOnly";
                public IElement _isReadOnly = null;

            }

            public _StructuralFeature @StructuralFeature = new _StructuralFeature();
            public IElement @__StructuralFeature = new MofElement();

        }

        public _Classification Classification = new _Classification();

        public class _Actions
        {
            public class _ValueSpecificationAction
            {
                public string @result = "result";
                public IElement _result = null;

                public string @value = "value";
                public IElement _value = null;

            }

            public _ValueSpecificationAction @ValueSpecificationAction = new _ValueSpecificationAction();
            public IElement @__ValueSpecificationAction = new MofElement();

            public class _VariableAction
            {
                public string @variable = "variable";
                public IElement _variable = null;

            }

            public _VariableAction @VariableAction = new _VariableAction();
            public IElement @__VariableAction = new MofElement();

            public class _WriteLinkAction
            {
            }

            public _WriteLinkAction @WriteLinkAction = new _WriteLinkAction();
            public IElement @__WriteLinkAction = new MofElement();

            public class _WriteStructuralFeatureAction
            {
                public string @result = "result";
                public IElement _result = null;

                public string @value = "value";
                public IElement _value = null;

            }

            public _WriteStructuralFeatureAction @WriteStructuralFeatureAction = new _WriteStructuralFeatureAction();
            public IElement @__WriteStructuralFeatureAction = new MofElement();

            public class _WriteVariableAction
            {
                public string @value = "value";
                public IElement _value = null;

            }

            public _WriteVariableAction @WriteVariableAction = new _WriteVariableAction();
            public IElement @__WriteVariableAction = new MofElement();

            public class _AcceptCallAction
            {
                public string @returnInformation = "returnInformation";
                public IElement _returnInformation = null;

            }

            public _AcceptCallAction @AcceptCallAction = new _AcceptCallAction();
            public IElement @__AcceptCallAction = new MofElement();

            public class _AcceptEventAction
            {
                public string @isUnmarshall = "isUnmarshall";
                public IElement _isUnmarshall = null;

                public string @result = "result";
                public IElement _result = null;

                public string @trigger = "trigger";
                public IElement _trigger = null;

            }

            public _AcceptEventAction @AcceptEventAction = new _AcceptEventAction();
            public IElement @__AcceptEventAction = new MofElement();

            public class _Action
            {
                public string @context = "context";
                public IElement _context = null;

                public string @input = "input";
                public IElement _input = null;

                public string @isLocallyReentrant = "isLocallyReentrant";
                public IElement _isLocallyReentrant = null;

                public string @localPostcondition = "localPostcondition";
                public IElement _localPostcondition = null;

                public string @localPrecondition = "localPrecondition";
                public IElement _localPrecondition = null;

                public string @output = "output";
                public IElement _output = null;

            }

            public _Action @Action = new _Action();
            public IElement @__Action = new MofElement();

            public class _ActionInputPin
            {
                public string @fromAction = "fromAction";
                public IElement _fromAction = null;

            }

            public _ActionInputPin @ActionInputPin = new _ActionInputPin();
            public IElement @__ActionInputPin = new MofElement();

            public class _AddStructuralFeatureValueAction
            {
                public string @insertAt = "insertAt";
                public IElement _insertAt = null;

                public string @isReplaceAll = "isReplaceAll";
                public IElement _isReplaceAll = null;

            }

            public _AddStructuralFeatureValueAction @AddStructuralFeatureValueAction = new _AddStructuralFeatureValueAction();
            public IElement @__AddStructuralFeatureValueAction = new MofElement();

            public class _AddVariableValueAction
            {
                public string @insertAt = "insertAt";
                public IElement _insertAt = null;

                public string @isReplaceAll = "isReplaceAll";
                public IElement _isReplaceAll = null;

            }

            public _AddVariableValueAction @AddVariableValueAction = new _AddVariableValueAction();
            public IElement @__AddVariableValueAction = new MofElement();

            public class _BroadcastSignalAction
            {
                public string @signal = "signal";
                public IElement _signal = null;

            }

            public _BroadcastSignalAction @BroadcastSignalAction = new _BroadcastSignalAction();
            public IElement @__BroadcastSignalAction = new MofElement();

            public class _CallAction
            {
                public string @isSynchronous = "isSynchronous";
                public IElement _isSynchronous = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _CallAction @CallAction = new _CallAction();
            public IElement @__CallAction = new MofElement();

            public class _CallBehaviorAction
            {
                public string @behavior = "behavior";
                public IElement _behavior = null;

            }

            public _CallBehaviorAction @CallBehaviorAction = new _CallBehaviorAction();
            public IElement @__CallBehaviorAction = new MofElement();

            public class _CallOperationAction
            {
                public string @operation = "operation";
                public IElement _operation = null;

                public string @target = "target";
                public IElement _target = null;

            }

            public _CallOperationAction @CallOperationAction = new _CallOperationAction();
            public IElement @__CallOperationAction = new MofElement();

            public class _Clause
            {
                public string @body = "body";
                public IElement _body = null;

                public string @bodyOutput = "bodyOutput";
                public IElement _bodyOutput = null;

                public string @decider = "decider";
                public IElement _decider = null;

                public string @predecessorClause = "predecessorClause";
                public IElement _predecessorClause = null;

                public string @successorClause = "successorClause";
                public IElement _successorClause = null;

                public string @test = "test";
                public IElement _test = null;

            }

            public _Clause @Clause = new _Clause();
            public IElement @__Clause = new MofElement();

            public class _ClearAssociationAction
            {
                public string @association = "association";
                public IElement _association = null;

                public string @object = "object";
                public IElement _object = null;

            }

            public _ClearAssociationAction @ClearAssociationAction = new _ClearAssociationAction();
            public IElement @__ClearAssociationAction = new MofElement();

            public class _ClearStructuralFeatureAction
            {
                public string @result = "result";
                public IElement _result = null;

            }

            public _ClearStructuralFeatureAction @ClearStructuralFeatureAction = new _ClearStructuralFeatureAction();
            public IElement @__ClearStructuralFeatureAction = new MofElement();

            public class _ClearVariableAction
            {
            }

            public _ClearVariableAction @ClearVariableAction = new _ClearVariableAction();
            public IElement @__ClearVariableAction = new MofElement();

            public class _ConditionalNode
            {
                public string @clause = "clause";
                public IElement _clause = null;

                public string @isAssured = "isAssured";
                public IElement _isAssured = null;

                public string @isDeterminate = "isDeterminate";
                public IElement _isDeterminate = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _ConditionalNode @ConditionalNode = new _ConditionalNode();
            public IElement @__ConditionalNode = new MofElement();

            public class _CreateLinkAction
            {
                public string @endData = "endData";
                public IElement _endData = null;

            }

            public _CreateLinkAction @CreateLinkAction = new _CreateLinkAction();
            public IElement @__CreateLinkAction = new MofElement();

            public class _CreateLinkObjectAction
            {
                public string @result = "result";
                public IElement _result = null;

            }

            public _CreateLinkObjectAction @CreateLinkObjectAction = new _CreateLinkObjectAction();
            public IElement @__CreateLinkObjectAction = new MofElement();

            public class _CreateObjectAction
            {
                public string @classifier = "classifier";
                public IElement _classifier = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _CreateObjectAction @CreateObjectAction = new _CreateObjectAction();
            public IElement @__CreateObjectAction = new MofElement();

            public class _DestroyLinkAction
            {
                public string @endData = "endData";
                public IElement _endData = null;

            }

            public _DestroyLinkAction @DestroyLinkAction = new _DestroyLinkAction();
            public IElement @__DestroyLinkAction = new MofElement();

            public class _DestroyObjectAction
            {
                public string @isDestroyLinks = "isDestroyLinks";
                public IElement _isDestroyLinks = null;

                public string @isDestroyOwnedObjects = "isDestroyOwnedObjects";
                public IElement _isDestroyOwnedObjects = null;

                public string @target = "target";
                public IElement _target = null;

            }

            public _DestroyObjectAction @DestroyObjectAction = new _DestroyObjectAction();
            public IElement @__DestroyObjectAction = new MofElement();

            public class _ExpansionNode
            {
                public string @regionAsInput = "regionAsInput";
                public IElement _regionAsInput = null;

                public string @regionAsOutput = "regionAsOutput";
                public IElement _regionAsOutput = null;

            }

            public _ExpansionNode @ExpansionNode = new _ExpansionNode();
            public IElement @__ExpansionNode = new MofElement();

            public class _ExpansionRegion
            {
                public string @inputElement = "inputElement";
                public IElement _inputElement = null;

                public string @mode = "mode";
                public IElement _mode = null;

                public string @outputElement = "outputElement";
                public IElement _outputElement = null;

            }

            public _ExpansionRegion @ExpansionRegion = new _ExpansionRegion();
            public IElement @__ExpansionRegion = new MofElement();

            public class _InputPin
            {
            }

            public _InputPin @InputPin = new _InputPin();
            public IElement @__InputPin = new MofElement();

            public class _InvocationAction
            {
                public string @argument = "argument";
                public IElement _argument = null;

                public string @onPort = "onPort";
                public IElement _onPort = null;

            }

            public _InvocationAction @InvocationAction = new _InvocationAction();
            public IElement @__InvocationAction = new MofElement();

            public class _LinkAction
            {
                public string @endData = "endData";
                public IElement _endData = null;

                public string @inputValue = "inputValue";
                public IElement _inputValue = null;

            }

            public _LinkAction @LinkAction = new _LinkAction();
            public IElement @__LinkAction = new MofElement();

            public class _LinkEndCreationData
            {
                public string @insertAt = "insertAt";
                public IElement _insertAt = null;

                public string @isReplaceAll = "isReplaceAll";
                public IElement _isReplaceAll = null;

            }

            public _LinkEndCreationData @LinkEndCreationData = new _LinkEndCreationData();
            public IElement @__LinkEndCreationData = new MofElement();

            public class _LinkEndData
            {
                public string @end = "end";
                public IElement _end = null;

                public string @qualifier = "qualifier";
                public IElement _qualifier = null;

                public string @value = "value";
                public IElement _value = null;

            }

            public _LinkEndData @LinkEndData = new _LinkEndData();
            public IElement @__LinkEndData = new MofElement();

            public class _LinkEndDestructionData
            {
                public string @destroyAt = "destroyAt";
                public IElement _destroyAt = null;

                public string @isDestroyDuplicates = "isDestroyDuplicates";
                public IElement _isDestroyDuplicates = null;

            }

            public _LinkEndDestructionData @LinkEndDestructionData = new _LinkEndDestructionData();
            public IElement @__LinkEndDestructionData = new MofElement();

            public class _LoopNode
            {
                public string @bodyOutput = "bodyOutput";
                public IElement _bodyOutput = null;

                public string @bodyPart = "bodyPart";
                public IElement _bodyPart = null;

                public string @decider = "decider";
                public IElement _decider = null;

                public string @isTestedFirst = "isTestedFirst";
                public IElement _isTestedFirst = null;

                public string @loopVariable = "loopVariable";
                public IElement _loopVariable = null;

                public string @loopVariableInput = "loopVariableInput";
                public IElement _loopVariableInput = null;

                public string @result = "result";
                public IElement _result = null;

                public string @setupPart = "setupPart";
                public IElement _setupPart = null;

                public string @test = "test";
                public IElement _test = null;

            }

            public _LoopNode @LoopNode = new _LoopNode();
            public IElement @__LoopNode = new MofElement();

            public class _OpaqueAction
            {
                public string @body = "body";
                public IElement _body = null;

                public string @inputValue = "inputValue";
                public IElement _inputValue = null;

                public string @language = "language";
                public IElement _language = null;

                public string @outputValue = "outputValue";
                public IElement _outputValue = null;

            }

            public _OpaqueAction @OpaqueAction = new _OpaqueAction();
            public IElement @__OpaqueAction = new MofElement();

            public class _OutputPin
            {
            }

            public _OutputPin @OutputPin = new _OutputPin();
            public IElement @__OutputPin = new MofElement();

            public class _Pin
            {
                public string @isControl = "isControl";
                public IElement _isControl = null;

            }

            public _Pin @Pin = new _Pin();
            public IElement @__Pin = new MofElement();

            public class _QualifierValue
            {
                public string @qualifier = "qualifier";
                public IElement _qualifier = null;

                public string @value = "value";
                public IElement _value = null;

            }

            public _QualifierValue @QualifierValue = new _QualifierValue();
            public IElement @__QualifierValue = new MofElement();

            public class _RaiseExceptionAction
            {
                public string @exception = "exception";
                public IElement _exception = null;

            }

            public _RaiseExceptionAction @RaiseExceptionAction = new _RaiseExceptionAction();
            public IElement @__RaiseExceptionAction = new MofElement();

            public class _ReadExtentAction
            {
                public string @classifier = "classifier";
                public IElement _classifier = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadExtentAction @ReadExtentAction = new _ReadExtentAction();
            public IElement @__ReadExtentAction = new MofElement();

            public class _ReadIsClassifiedObjectAction
            {
                public string @classifier = "classifier";
                public IElement _classifier = null;

                public string @isDirect = "isDirect";
                public IElement _isDirect = null;

                public string @object = "object";
                public IElement _object = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadIsClassifiedObjectAction @ReadIsClassifiedObjectAction = new _ReadIsClassifiedObjectAction();
            public IElement @__ReadIsClassifiedObjectAction = new MofElement();

            public class _ReadLinkAction
            {
                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadLinkAction @ReadLinkAction = new _ReadLinkAction();
            public IElement @__ReadLinkAction = new MofElement();

            public class _ReadLinkObjectEndAction
            {
                public string @end = "end";
                public IElement _end = null;

                public string @object = "object";
                public IElement _object = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadLinkObjectEndAction @ReadLinkObjectEndAction = new _ReadLinkObjectEndAction();
            public IElement @__ReadLinkObjectEndAction = new MofElement();

            public class _ReadLinkObjectEndQualifierAction
            {
                public string @object = "object";
                public IElement _object = null;

                public string @qualifier = "qualifier";
                public IElement _qualifier = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadLinkObjectEndQualifierAction @ReadLinkObjectEndQualifierAction = new _ReadLinkObjectEndQualifierAction();
            public IElement @__ReadLinkObjectEndQualifierAction = new MofElement();

            public class _ReadSelfAction
            {
                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadSelfAction @ReadSelfAction = new _ReadSelfAction();
            public IElement @__ReadSelfAction = new MofElement();

            public class _ReadStructuralFeatureAction
            {
                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadStructuralFeatureAction @ReadStructuralFeatureAction = new _ReadStructuralFeatureAction();
            public IElement @__ReadStructuralFeatureAction = new MofElement();

            public class _ReadVariableAction
            {
                public string @result = "result";
                public IElement _result = null;

            }

            public _ReadVariableAction @ReadVariableAction = new _ReadVariableAction();
            public IElement @__ReadVariableAction = new MofElement();

            public class _ReclassifyObjectAction
            {
                public string @isReplaceAll = "isReplaceAll";
                public IElement _isReplaceAll = null;

                public string @newClassifier = "newClassifier";
                public IElement _newClassifier = null;

                public string @object = "object";
                public IElement _object = null;

                public string @oldClassifier = "oldClassifier";
                public IElement _oldClassifier = null;

            }

            public _ReclassifyObjectAction @ReclassifyObjectAction = new _ReclassifyObjectAction();
            public IElement @__ReclassifyObjectAction = new MofElement();

            public class _ReduceAction
            {
                public string @collection = "collection";
                public IElement _collection = null;

                public string @isOrdered = "isOrdered";
                public IElement _isOrdered = null;

                public string @reducer = "reducer";
                public IElement _reducer = null;

                public string @result = "result";
                public IElement _result = null;

            }

            public _ReduceAction @ReduceAction = new _ReduceAction();
            public IElement @__ReduceAction = new MofElement();

            public class _RemoveStructuralFeatureValueAction
            {
                public string @isRemoveDuplicates = "isRemoveDuplicates";
                public IElement _isRemoveDuplicates = null;

                public string @removeAt = "removeAt";
                public IElement _removeAt = null;

            }

            public _RemoveStructuralFeatureValueAction @RemoveStructuralFeatureValueAction = new _RemoveStructuralFeatureValueAction();
            public IElement @__RemoveStructuralFeatureValueAction = new MofElement();

            public class _RemoveVariableValueAction
            {
                public string @isRemoveDuplicates = "isRemoveDuplicates";
                public IElement _isRemoveDuplicates = null;

                public string @removeAt = "removeAt";
                public IElement _removeAt = null;

            }

            public _RemoveVariableValueAction @RemoveVariableValueAction = new _RemoveVariableValueAction();
            public IElement @__RemoveVariableValueAction = new MofElement();

            public class _ReplyAction
            {
                public string @replyToCall = "replyToCall";
                public IElement _replyToCall = null;

                public string @replyValue = "replyValue";
                public IElement _replyValue = null;

                public string @returnInformation = "returnInformation";
                public IElement _returnInformation = null;

            }

            public _ReplyAction @ReplyAction = new _ReplyAction();
            public IElement @__ReplyAction = new MofElement();

            public class _SendObjectAction
            {
                public string @request = "request";
                public IElement _request = null;

                public string @target = "target";
                public IElement _target = null;

            }

            public _SendObjectAction @SendObjectAction = new _SendObjectAction();
            public IElement @__SendObjectAction = new MofElement();

            public class _SendSignalAction
            {
                public string @signal = "signal";
                public IElement _signal = null;

                public string @target = "target";
                public IElement _target = null;

            }

            public _SendSignalAction @SendSignalAction = new _SendSignalAction();
            public IElement @__SendSignalAction = new MofElement();

            public class _SequenceNode
            {
                public string @executableNode = "executableNode";
                public IElement _executableNode = null;

            }

            public _SequenceNode @SequenceNode = new _SequenceNode();
            public IElement @__SequenceNode = new MofElement();

            public class _StartClassifierBehaviorAction
            {
                public string @object = "object";
                public IElement _object = null;

            }

            public _StartClassifierBehaviorAction @StartClassifierBehaviorAction = new _StartClassifierBehaviorAction();
            public IElement @__StartClassifierBehaviorAction = new MofElement();

            public class _StartObjectBehaviorAction
            {
                public string @object = "object";
                public IElement _object = null;

            }

            public _StartObjectBehaviorAction @StartObjectBehaviorAction = new _StartObjectBehaviorAction();
            public IElement @__StartObjectBehaviorAction = new MofElement();

            public class _StructuralFeatureAction
            {
                public string @object = "object";
                public IElement _object = null;

                public string @structuralFeature = "structuralFeature";
                public IElement _structuralFeature = null;

            }

            public _StructuralFeatureAction @StructuralFeatureAction = new _StructuralFeatureAction();
            public IElement @__StructuralFeatureAction = new MofElement();

            public class _StructuredActivityNode
            {
                public string @activity = "activity";
                public IElement _activity = null;

                public string @edge = "edge";
                public IElement _edge = null;

                public string @mustIsolate = "mustIsolate";
                public IElement _mustIsolate = null;

                public string @node = "node";
                public IElement _node = null;

                public string @structuredNodeInput = "structuredNodeInput";
                public IElement _structuredNodeInput = null;

                public string @structuredNodeOutput = "structuredNodeOutput";
                public IElement _structuredNodeOutput = null;

                public string @variable = "variable";
                public IElement _variable = null;

            }

            public _StructuredActivityNode @StructuredActivityNode = new _StructuredActivityNode();
            public IElement @__StructuredActivityNode = new MofElement();

            public class _TestIdentityAction
            {
                public string @first = "first";
                public IElement _first = null;

                public string @result = "result";
                public IElement _result = null;

                public string @second = "second";
                public IElement _second = null;

            }

            public _TestIdentityAction @TestIdentityAction = new _TestIdentityAction();
            public IElement @__TestIdentityAction = new MofElement();

            public class _UnmarshallAction
            {
                public string @object = "object";
                public IElement _object = null;

                public string @result = "result";
                public IElement _result = null;

                public string @unmarshallType = "unmarshallType";
                public IElement _unmarshallType = null;

            }

            public _UnmarshallAction @UnmarshallAction = new _UnmarshallAction();
            public IElement @__UnmarshallAction = new MofElement();

            public class _ValuePin
            {
                public string @value = "value";
                public IElement _value = null;

            }

            public _ValuePin @ValuePin = new _ValuePin();
            public IElement @__ValuePin = new MofElement();

        }

        public _Actions Actions = new _Actions();

        public static _UML TheOne = new _UML();

    }

}
