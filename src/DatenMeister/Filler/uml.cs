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
                public object @edge = "edge";

                public object @group = "group";

                public object @isReadOnly = "isReadOnly";

                public object @isSingleExecution = "isSingleExecution";

                public object @node = "node";

                public object @partition = "partition";

                public object @structuredNode = "structuredNode";

                public object @variable = "variable";

            }

            public _Activity @Activity = new _Activity();
            public IElement @__Activity = new MofElement();

            public class _ActivityEdge
            {
                public object @activity = "activity";

                public object @guard = "guard";

                public object @inGroup = "inGroup";

                public object @inPartition = "inPartition";

                public object @inStructuredNode = "inStructuredNode";

                public object @interrupts = "interrupts";

                public object @redefinedEdge = "redefinedEdge";

                public object @source = "source";

                public object @target = "target";

                public object @weight = "weight";

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
                public object @containedEdge = "containedEdge";

                public object @containedNode = "containedNode";

                public object @inActivity = "inActivity";

                public object @subgroup = "subgroup";

                public object @superGroup = "superGroup";

            }

            public _ActivityGroup @ActivityGroup = new _ActivityGroup();
            public IElement @__ActivityGroup = new MofElement();

            public class _ActivityNode
            {
                public object @activity = "activity";

                public object @inGroup = "inGroup";

                public object @inInterruptibleRegion = "inInterruptibleRegion";

                public object @inPartition = "inPartition";

                public object @inStructuredNode = "inStructuredNode";

                public object @incoming = "incoming";

                public object @outgoing = "outgoing";

                public object @redefinedNode = "redefinedNode";

            }

            public _ActivityNode @ActivityNode = new _ActivityNode();
            public IElement @__ActivityNode = new MofElement();

            public class _ActivityParameterNode
            {
                public object @parameter = "parameter";

            }

            public _ActivityParameterNode @ActivityParameterNode = new _ActivityParameterNode();
            public IElement @__ActivityParameterNode = new MofElement();

            public class _ActivityPartition
            {
                public object @edge = "edge";

                public object @isDimension = "isDimension";

                public object @isExternal = "isExternal";

                public object @node = "node";

                public object @represents = "represents";

                public object @subpartition = "subpartition";

                public object @superPartition = "superPartition";

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
                public object @decisionInput = "decisionInput";

                public object @decisionInputFlow = "decisionInputFlow";

            }

            public _DecisionNode @DecisionNode = new _DecisionNode();
            public IElement @__DecisionNode = new MofElement();

            public class _ExceptionHandler
            {
                public object @exceptionInput = "exceptionInput";

                public object @exceptionType = "exceptionType";

                public object @handlerBody = "handlerBody";

                public object @protectedNode = "protectedNode";

            }

            public _ExceptionHandler @ExceptionHandler = new _ExceptionHandler();
            public IElement @__ExceptionHandler = new MofElement();

            public class _ExecutableNode
            {
                public object @handler = "handler";

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
                public object @interruptingEdge = "interruptingEdge";

                public object @node = "node";

            }

            public _InterruptibleActivityRegion @InterruptibleActivityRegion = new _InterruptibleActivityRegion();
            public IElement @__InterruptibleActivityRegion = new MofElement();

            public class _JoinNode
            {
                public object @isCombineDuplicate = "isCombineDuplicate";

                public object @joinSpec = "joinSpec";

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
                public object @isMulticast = "isMulticast";

                public object @isMultireceive = "isMultireceive";

                public object @selection = "selection";

                public object @transformation = "transformation";

            }

            public _ObjectFlow @ObjectFlow = new _ObjectFlow();
            public IElement @__ObjectFlow = new MofElement();

            public class _ObjectNode
            {
                public object @inState = "inState";

                public object @isControlType = "isControlType";

                public object @ordering = "ordering";

                public object @selection = "selection";

                public object @upperBound = "upperBound";

            }

            public _ObjectNode @ObjectNode = new _ObjectNode();
            public IElement @__ObjectNode = new MofElement();

            public class _Variable
            {
                public object @activityScope = "activityScope";

                public object @scope = "scope";

            }

            public _Variable @Variable = new _Variable();
            public IElement @__Variable = new MofElement();

        }

        public _Activities Activities = new _Activities();

        public class _Values
        {
            public class _Duration
            {
                public object @expr = "expr";

                public object @observation = "observation";

            }

            public _Duration @Duration = new _Duration();
            public IElement @__Duration = new MofElement();

            public class _DurationConstraint
            {
                public object @firstEvent = "firstEvent";

                public object @specification = "specification";

            }

            public _DurationConstraint @DurationConstraint = new _DurationConstraint();
            public IElement @__DurationConstraint = new MofElement();

            public class _DurationInterval
            {
                public object @max = "max";

                public object @min = "min";

            }

            public _DurationInterval @DurationInterval = new _DurationInterval();
            public IElement @__DurationInterval = new MofElement();

            public class _DurationObservation
            {
                public object @event = "event";

                public object @firstEvent = "firstEvent";

            }

            public _DurationObservation @DurationObservation = new _DurationObservation();
            public IElement @__DurationObservation = new MofElement();

            public class _Expression
            {
                public object @operand = "operand";

                public object @symbol = "symbol";

            }

            public _Expression @Expression = new _Expression();
            public IElement @__Expression = new MofElement();

            public class _Interval
            {
                public object @max = "max";

                public object @min = "min";

            }

            public _Interval @Interval = new _Interval();
            public IElement @__Interval = new MofElement();

            public class _IntervalConstraint
            {
                public object @specification = "specification";

            }

            public _IntervalConstraint @IntervalConstraint = new _IntervalConstraint();
            public IElement @__IntervalConstraint = new MofElement();

            public class _LiteralBoolean
            {
                public object @value = "value";

            }

            public _LiteralBoolean @LiteralBoolean = new _LiteralBoolean();
            public IElement @__LiteralBoolean = new MofElement();

            public class _LiteralInteger
            {
                public object @value = "value";

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
                public object @value = "value";

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
                public object @value = "value";

            }

            public _LiteralString @LiteralString = new _LiteralString();
            public IElement @__LiteralString = new MofElement();

            public class _LiteralUnlimitedNatural
            {
                public object @value = "value";

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
                public object @behavior = "behavior";

                public object @body = "body";

                public object @language = "language";

                public object @result = "result";

            }

            public _OpaqueExpression @OpaqueExpression = new _OpaqueExpression();
            public IElement @__OpaqueExpression = new MofElement();

            public class _StringExpression
            {
                public object @owningExpression = "owningExpression";

                public object @subExpression = "subExpression";

            }

            public _StringExpression @StringExpression = new _StringExpression();
            public IElement @__StringExpression = new MofElement();

            public class _TimeConstraint
            {
                public object @firstEvent = "firstEvent";

                public object @specification = "specification";

            }

            public _TimeConstraint @TimeConstraint = new _TimeConstraint();
            public IElement @__TimeConstraint = new MofElement();

            public class _TimeExpression
            {
                public object @expr = "expr";

                public object @observation = "observation";

            }

            public _TimeExpression @TimeExpression = new _TimeExpression();
            public IElement @__TimeExpression = new MofElement();

            public class _TimeInterval
            {
                public object @max = "max";

                public object @min = "min";

            }

            public _TimeInterval @TimeInterval = new _TimeInterval();
            public IElement @__TimeInterval = new MofElement();

            public class _TimeObservation
            {
                public object @event = "event";

                public object @firstEvent = "firstEvent";

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
                public object @condition = "condition";

                public object @extendedCase = "extendedCase";

                public object @extension = "extension";

                public object @extensionLocation = "extensionLocation";

            }

            public _Extend @Extend = new _Extend();
            public IElement @__Extend = new MofElement();

            public class _ExtensionPoint
            {
                public object @useCase = "useCase";

            }

            public _ExtensionPoint @ExtensionPoint = new _ExtensionPoint();
            public IElement @__ExtensionPoint = new MofElement();

            public class _Include
            {
                public object @addition = "addition";

                public object @includingCase = "includingCase";

            }

            public _Include @Include = new _Include();
            public IElement @__Include = new MofElement();

            public class _UseCase
            {
                public object @extend = "extend";

                public object @extensionPoint = "extensionPoint";

                public object @include = "include";

                public object @subject = "subject";

            }

            public _UseCase @UseCase = new _UseCase();
            public IElement @__UseCase = new MofElement();

        }

        public _UseCases UseCases = new _UseCases();

        public class _StructuredClassifiers
        {
            public class _Association
            {
                public object @endType = "endType";

                public object @isDerived = "isDerived";

                public object @memberEnd = "memberEnd";

                public object @navigableOwnedEnd = "navigableOwnedEnd";

                public object @ownedEnd = "ownedEnd";

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
                public object @extension = "extension";

                public object @isAbstract = "isAbstract";

                public object @isActive = "isActive";

                public object @nestedClassifier = "nestedClassifier";

                public object @ownedAttribute = "ownedAttribute";

                public object @ownedOperation = "ownedOperation";

                public object @ownedReception = "ownedReception";

                public object @superClass = "superClass";

            }

            public _Class @Class = new _Class();
            public IElement @__Class = new MofElement();

            public class _Collaboration
            {
                public object @collaborationRole = "collaborationRole";

            }

            public _Collaboration @Collaboration = new _Collaboration();
            public IElement @__Collaboration = new MofElement();

            public class _CollaborationUse
            {
                public object @roleBinding = "roleBinding";

                public object @type = "type";

            }

            public _CollaborationUse @CollaborationUse = new _CollaborationUse();
            public IElement @__CollaborationUse = new MofElement();

            public class _Component
            {
                public object @isIndirectlyInstantiated = "isIndirectlyInstantiated";

                public object @packagedElement = "packagedElement";

                public object @provided = "provided";

                public object @realization = "realization";

                public object @required = "required";

            }

            public _Component @Component = new _Component();
            public IElement @__Component = new MofElement();

            public class _ComponentRealization
            {
                public object @abstraction = "abstraction";

                public object @realizingClassifier = "realizingClassifier";

            }

            public _ComponentRealization @ComponentRealization = new _ComponentRealization();
            public IElement @__ComponentRealization = new MofElement();

            public class _ConnectableElement
            {
                public object @end = "end";

                public object @templateParameter = "templateParameter";

            }

            public _ConnectableElement @ConnectableElement = new _ConnectableElement();
            public IElement @__ConnectableElement = new MofElement();

            public class _ConnectableElementTemplateParameter
            {
                public object @parameteredElement = "parameteredElement";

            }

            public _ConnectableElementTemplateParameter @ConnectableElementTemplateParameter = new _ConnectableElementTemplateParameter();
            public IElement @__ConnectableElementTemplateParameter = new MofElement();

            public class _Connector
            {
                public object @contract = "contract";

                public object @end = "end";

                public object @kind = "kind";

                public object @redefinedConnector = "redefinedConnector";

                public object @type = "type";

            }

            public _Connector @Connector = new _Connector();
            public IElement @__Connector = new MofElement();

            public class _ConnectorEnd
            {
                public object @definingEnd = "definingEnd";

                public object @partWithPort = "partWithPort";

                public object @role = "role";

            }

            public _ConnectorEnd @ConnectorEnd = new _ConnectorEnd();
            public IElement @__ConnectorEnd = new MofElement();

            public class _EncapsulatedClassifier
            {
                public object @ownedPort = "ownedPort";

            }

            public _EncapsulatedClassifier @EncapsulatedClassifier = new _EncapsulatedClassifier();
            public IElement @__EncapsulatedClassifier = new MofElement();

            public class _Port
            {
                public object @isBehavior = "isBehavior";

                public object @isConjugated = "isConjugated";

                public object @isService = "isService";

                public object @protocol = "protocol";

                public object @provided = "provided";

                public object @redefinedPort = "redefinedPort";

                public object @required = "required";

            }

            public _Port @Port = new _Port();
            public IElement @__Port = new MofElement();

            public class _StructuredClassifier
            {
                public object @ownedAttribute = "ownedAttribute";

                public object @ownedConnector = "ownedConnector";

                public object @part = "part";

                public object @role = "role";

            }

            public _StructuredClassifier @StructuredClassifier = new _StructuredClassifier();
            public IElement @__StructuredClassifier = new MofElement();

        }

        public _StructuredClassifiers StructuredClassifiers = new _StructuredClassifiers();

        public class _StateMachines
        {
            public class _ConnectionPointReference
            {
                public object @entry = "entry";

                public object @exit = "exit";

                public object @state = "state";

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
                public object @generalMachine = "generalMachine";

                public object @specificMachine = "specificMachine";

            }

            public _ProtocolConformance @ProtocolConformance = new _ProtocolConformance();
            public IElement @__ProtocolConformance = new MofElement();

            public class _ProtocolStateMachine
            {
                public object @conformance = "conformance";

            }

            public _ProtocolStateMachine @ProtocolStateMachine = new _ProtocolStateMachine();
            public IElement @__ProtocolStateMachine = new MofElement();

            public class _ProtocolTransition
            {
                public object @postCondition = "postCondition";

                public object @preCondition = "preCondition";

                public object @referred = "referred";

            }

            public _ProtocolTransition @ProtocolTransition = new _ProtocolTransition();
            public IElement @__ProtocolTransition = new MofElement();

            public class _Pseudostate
            {
                public object @kind = "kind";

                public object @state = "state";

                public object @stateMachine = "stateMachine";

            }

            public _Pseudostate @Pseudostate = new _Pseudostate();
            public IElement @__Pseudostate = new MofElement();

            public class _Region
            {
                public object @extendedRegion = "extendedRegion";

                public object @redefinitionContext = "redefinitionContext";

                public object @state = "state";

                public object @stateMachine = "stateMachine";

                public object @subvertex = "subvertex";

                public object @transition = "transition";

            }

            public _Region @Region = new _Region();
            public IElement @__Region = new MofElement();

            public class _State
            {
                public object @connection = "connection";

                public object @connectionPoint = "connectionPoint";

                public object @deferrableTrigger = "deferrableTrigger";

                public object @doActivity = "doActivity";

                public object @entry = "entry";

                public object @exit = "exit";

                public object @isComposite = "isComposite";

                public object @isOrthogonal = "isOrthogonal";

                public object @isSimple = "isSimple";

                public object @isSubmachineState = "isSubmachineState";

                public object @redefinedState = "redefinedState";

                public object @redefinitionContext = "redefinitionContext";

                public object @region = "region";

                public object @stateInvariant = "stateInvariant";

                public object @submachine = "submachine";

            }

            public _State @State = new _State();
            public IElement @__State = new MofElement();

            public class _StateMachine
            {
                public object @connectionPoint = "connectionPoint";

                public object @extendedStateMachine = "extendedStateMachine";

                public object @region = "region";

                public object @submachineState = "submachineState";

            }

            public _StateMachine @StateMachine = new _StateMachine();
            public IElement @__StateMachine = new MofElement();

            public class _Transition
            {
                public object @container = "container";

                public object @effect = "effect";

                public object @guard = "guard";

                public object @kind = "kind";

                public object @redefinedTransition = "redefinedTransition";

                public object @redefinitionContext = "redefinitionContext";

                public object @source = "source";

                public object @target = "target";

                public object @trigger = "trigger";

            }

            public _Transition @Transition = new _Transition();
            public IElement @__Transition = new MofElement();

            public class _Vertex
            {
                public object @container = "container";

                public object @incoming = "incoming";

                public object @outgoing = "outgoing";

            }

            public _Vertex @Vertex = new _Vertex();
            public IElement @__Vertex = new MofElement();

        }

        public _StateMachines StateMachines = new _StateMachines();

        public class _SimpleClassifiers
        {
            public class _BehavioredClassifier
            {
                public object @classifierBehavior = "classifierBehavior";

                public object @interfaceRealization = "interfaceRealization";

                public object @ownedBehavior = "ownedBehavior";

            }

            public _BehavioredClassifier @BehavioredClassifier = new _BehavioredClassifier();
            public IElement @__BehavioredClassifier = new MofElement();

            public class _DataType
            {
                public object @ownedAttribute = "ownedAttribute";

                public object @ownedOperation = "ownedOperation";

            }

            public _DataType @DataType = new _DataType();
            public IElement @__DataType = new MofElement();

            public class _Enumeration
            {
                public object @ownedLiteral = "ownedLiteral";

            }

            public _Enumeration @Enumeration = new _Enumeration();
            public IElement @__Enumeration = new MofElement();

            public class _EnumerationLiteral
            {
                public object @classifier = "classifier";

                public object @enumeration = "enumeration";

            }

            public _EnumerationLiteral @EnumerationLiteral = new _EnumerationLiteral();
            public IElement @__EnumerationLiteral = new MofElement();

            public class _Interface
            {
                public object @nestedClassifier = "nestedClassifier";

                public object @ownedAttribute = "ownedAttribute";

                public object @ownedOperation = "ownedOperation";

                public object @ownedReception = "ownedReception";

                public object @protocol = "protocol";

                public object @redefinedInterface = "redefinedInterface";

            }

            public _Interface @Interface = new _Interface();
            public IElement @__Interface = new MofElement();

            public class _InterfaceRealization
            {
                public object @contract = "contract";

                public object @implementingClassifier = "implementingClassifier";

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
                public object @signal = "signal";

            }

            public _Reception @Reception = new _Reception();
            public IElement @__Reception = new MofElement();

            public class _Signal
            {
                public object @ownedAttribute = "ownedAttribute";

            }

            public _Signal @Signal = new _Signal();
            public IElement @__Signal = new MofElement();

        }

        public _SimpleClassifiers SimpleClassifiers = new _SimpleClassifiers();

        public class _Packages
        {
            public class _Extension
            {
                public object @isRequired = "isRequired";

                public object @metaclass = "metaclass";

                public object @ownedEnd = "ownedEnd";

            }

            public _Extension @Extension = new _Extension();
            public IElement @__Extension = new MofElement();

            public class _ExtensionEnd
            {
                public object @lower = "lower";

                public object @type = "type";

            }

            public _ExtensionEnd @ExtensionEnd = new _ExtensionEnd();
            public IElement @__ExtensionEnd = new MofElement();

            public class _Image
            {
                public object @content = "content";

                public object @format = "format";

                public object @location = "location";

            }

            public _Image @Image = new _Image();
            public IElement @__Image = new MofElement();

            public class _Model
            {
                public object @viewpoint = "viewpoint";

            }

            public _Model @Model = new _Model();
            public IElement @__Model = new MofElement();

            public class _Package
            {
                public object @URI = "URI";

                public object @nestedPackage = "nestedPackage";

                public object @nestingPackage = "nestingPackage";

                public object @ownedStereotype = "ownedStereotype";

                public object @ownedType = "ownedType";

                public object @packageMerge = "packageMerge";

                public object @packagedElement = "packagedElement";

                public object @profileApplication = "profileApplication";

            }

            public _Package @Package = new _Package();
            public IElement @__Package = new MofElement();

            public class _PackageMerge
            {
                public object @mergedPackage = "mergedPackage";

                public object @receivingPackage = "receivingPackage";

            }

            public _PackageMerge @PackageMerge = new _PackageMerge();
            public IElement @__PackageMerge = new MofElement();

            public class _Profile
            {
                public object @metaclassReference = "metaclassReference";

                public object @metamodelReference = "metamodelReference";

            }

            public _Profile @Profile = new _Profile();
            public IElement @__Profile = new MofElement();

            public class _ProfileApplication
            {
                public object @appliedProfile = "appliedProfile";

                public object @applyingPackage = "applyingPackage";

                public object @isStrict = "isStrict";

            }

            public _ProfileApplication @ProfileApplication = new _ProfileApplication();
            public IElement @__ProfileApplication = new MofElement();

            public class _Stereotype
            {
                public object @icon = "icon";

                public object @profile = "profile";

            }

            public _Stereotype @Stereotype = new _Stereotype();
            public IElement @__Stereotype = new MofElement();

        }

        public _Packages Packages = new _Packages();

        public class _Interactions
        {
            public class _ActionExecutionSpecification
            {
                public object @action = "action";

            }

            public _ActionExecutionSpecification @ActionExecutionSpecification = new _ActionExecutionSpecification();
            public IElement @__ActionExecutionSpecification = new MofElement();

            public class _BehaviorExecutionSpecification
            {
                public object @behavior = "behavior";

            }

            public _BehaviorExecutionSpecification @BehaviorExecutionSpecification = new _BehaviorExecutionSpecification();
            public IElement @__BehaviorExecutionSpecification = new MofElement();

            public class _CombinedFragment
            {
                public object @cfragmentGate = "cfragmentGate";

                public object @interactionOperator = "interactionOperator";

                public object @operand = "operand";

            }

            public _CombinedFragment @CombinedFragment = new _CombinedFragment();
            public IElement @__CombinedFragment = new MofElement();

            public class _ConsiderIgnoreFragment
            {
                public object @message = "message";

            }

            public _ConsiderIgnoreFragment @ConsiderIgnoreFragment = new _ConsiderIgnoreFragment();
            public IElement @__ConsiderIgnoreFragment = new MofElement();

            public class _Continuation
            {
                public object @setting = "setting";

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
                public object @execution = "execution";

            }

            public _ExecutionOccurrenceSpecification @ExecutionOccurrenceSpecification = new _ExecutionOccurrenceSpecification();
            public IElement @__ExecutionOccurrenceSpecification = new MofElement();

            public class _ExecutionSpecification
            {
                public object @finish = "finish";

                public object @start = "start";

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
                public object @after = "after";

                public object @before = "before";

            }

            public _GeneralOrdering @GeneralOrdering = new _GeneralOrdering();
            public IElement @__GeneralOrdering = new MofElement();

            public class _Interaction
            {
                public object @action = "action";

                public object @formalGate = "formalGate";

                public object @fragment = "fragment";

                public object @lifeline = "lifeline";

                public object @message = "message";

            }

            public _Interaction @Interaction = new _Interaction();
            public IElement @__Interaction = new MofElement();

            public class _InteractionConstraint
            {
                public object @maxint = "maxint";

                public object @minint = "minint";

            }

            public _InteractionConstraint @InteractionConstraint = new _InteractionConstraint();
            public IElement @__InteractionConstraint = new MofElement();

            public class _InteractionFragment
            {
                public object @covered = "covered";

                public object @enclosingInteraction = "enclosingInteraction";

                public object @enclosingOperand = "enclosingOperand";

                public object @generalOrdering = "generalOrdering";

            }

            public _InteractionFragment @InteractionFragment = new _InteractionFragment();
            public IElement @__InteractionFragment = new MofElement();

            public class _InteractionOperand
            {
                public object @fragment = "fragment";

                public object @guard = "guard";

            }

            public _InteractionOperand @InteractionOperand = new _InteractionOperand();
            public IElement @__InteractionOperand = new MofElement();

            public class _InteractionUse
            {
                public object @actualGate = "actualGate";

                public object @argument = "argument";

                public object @refersTo = "refersTo";

                public object @returnValue = "returnValue";

                public object @returnValueRecipient = "returnValueRecipient";

            }

            public _InteractionUse @InteractionUse = new _InteractionUse();
            public IElement @__InteractionUse = new MofElement();

            public class _Lifeline
            {
                public object @coveredBy = "coveredBy";

                public object @decomposedAs = "decomposedAs";

                public object @interaction = "interaction";

                public object @represents = "represents";

                public object @selector = "selector";

            }

            public _Lifeline @Lifeline = new _Lifeline();
            public IElement @__Lifeline = new MofElement();

            public class _Message
            {
                public object @argument = "argument";

                public object @connector = "connector";

                public object @interaction = "interaction";

                public object @messageKind = "messageKind";

                public object @messageSort = "messageSort";

                public object @receiveEvent = "receiveEvent";

                public object @sendEvent = "sendEvent";

                public object @signature = "signature";

            }

            public _Message @Message = new _Message();
            public IElement @__Message = new MofElement();

            public class _MessageEnd
            {
                public object @message = "message";

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
                public object @covered = "covered";

                public object @toAfter = "toAfter";

                public object @toBefore = "toBefore";

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
                public object @covered = "covered";

                public object @invariant = "invariant";

            }

            public _StateInvariant @StateInvariant = new _StateInvariant();
            public IElement @__StateInvariant = new MofElement();

        }

        public _Interactions Interactions = new _Interactions();

        public class _InformationFlows
        {
            public class _InformationFlow
            {
                public object @conveyed = "conveyed";

                public object @informationSource = "informationSource";

                public object @informationTarget = "informationTarget";

                public object @realization = "realization";

                public object @realizingActivityEdge = "realizingActivityEdge";

                public object @realizingConnector = "realizingConnector";

                public object @realizingMessage = "realizingMessage";

            }

            public _InformationFlow @InformationFlow = new _InformationFlow();
            public IElement @__InformationFlow = new MofElement();

            public class _InformationItem
            {
                public object @represented = "represented";

            }

            public _InformationItem @InformationItem = new _InformationItem();
            public IElement @__InformationItem = new MofElement();

        }

        public _InformationFlows InformationFlows = new _InformationFlows();

        public class _Deployments
        {
            public class _Artifact
            {
                public object @fileName = "fileName";

                public object @manifestation = "manifestation";

                public object @nestedArtifact = "nestedArtifact";

                public object @ownedAttribute = "ownedAttribute";

                public object @ownedOperation = "ownedOperation";

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
                public object @configuration = "configuration";

                public object @deployedArtifact = "deployedArtifact";

                public object @location = "location";

            }

            public _Deployment @Deployment = new _Deployment();
            public IElement @__Deployment = new MofElement();

            public class _DeploymentSpecification
            {
                public object @deployment = "deployment";

                public object @deploymentLocation = "deploymentLocation";

                public object @executionLocation = "executionLocation";

            }

            public _DeploymentSpecification @DeploymentSpecification = new _DeploymentSpecification();
            public IElement @__DeploymentSpecification = new MofElement();

            public class _DeploymentTarget
            {
                public object @deployedElement = "deployedElement";

                public object @deployment = "deployment";

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
                public object @utilizedElement = "utilizedElement";

            }

            public _Manifestation @Manifestation = new _Manifestation();
            public IElement @__Manifestation = new MofElement();

            public class _Node
            {
                public object @nestedNode = "nestedNode";

            }

            public _Node @Node = new _Node();
            public IElement @__Node = new MofElement();

        }

        public _Deployments Deployments = new _Deployments();

        public class _CommonStructure
        {
            public class _Abstraction
            {
                public object @mapping = "mapping";

            }

            public _Abstraction @Abstraction = new _Abstraction();
            public IElement @__Abstraction = new MofElement();

            public class _Comment
            {
                public object @annotatedElement = "annotatedElement";

                public object @body = "body";

            }

            public _Comment @Comment = new _Comment();
            public IElement @__Comment = new MofElement();

            public class _Constraint
            {
                public object @constrainedElement = "constrainedElement";

                public object @context = "context";

                public object @specification = "specification";

            }

            public _Constraint @Constraint = new _Constraint();
            public IElement @__Constraint = new MofElement();

            public class _Dependency
            {
                public object @client = "client";

                public object @supplier = "supplier";

            }

            public _Dependency @Dependency = new _Dependency();
            public IElement @__Dependency = new MofElement();

            public class _DirectedRelationship
            {
                public object @source = "source";

                public object @target = "target";

            }

            public _DirectedRelationship @DirectedRelationship = new _DirectedRelationship();
            public IElement @__DirectedRelationship = new MofElement();

            public class _Element
            {
                public object @ownedComment = "ownedComment";

                public object @ownedElement = "ownedElement";

                public object @owner = "owner";

            }

            public _Element @Element = new _Element();
            public IElement @__Element = new MofElement();

            public class _ElementImport
            {
                public object @alias = "alias";

                public object @importedElement = "importedElement";

                public object @importingNamespace = "importingNamespace";

                public object @visibility = "visibility";

            }

            public _ElementImport @ElementImport = new _ElementImport();
            public IElement @__ElementImport = new MofElement();

            public class _MultiplicityElement
            {
                public object @isOrdered = "isOrdered";

                public object @isUnique = "isUnique";

                public object @lower = "lower";

                public object @lowerValue = "lowerValue";

                public object @upper = "upper";

                public object @upperValue = "upperValue";

            }

            public _MultiplicityElement @MultiplicityElement = new _MultiplicityElement();
            public IElement @__MultiplicityElement = new MofElement();

            public class _NamedElement
            {
                public object @clientDependency = "clientDependency";

                public object @name = "name";

                public object @nameExpression = "nameExpression";

                public object @namespace = "namespace";

                public object @qualifiedName = "qualifiedName";

                public object @visibility = "visibility";

            }

            public _NamedElement @NamedElement = new _NamedElement();
            public IElement @__NamedElement = new MofElement();

            public class _Namespace
            {
                public object @elementImport = "elementImport";

                public object @importedMember = "importedMember";

                public object @member = "member";

                public object @ownedMember = "ownedMember";

                public object @ownedRule = "ownedRule";

                public object @packageImport = "packageImport";

            }

            public _Namespace @Namespace = new _Namespace();
            public IElement @__Namespace = new MofElement();

            public class _PackageableElement
            {
                public object @visibility = "visibility";

            }

            public _PackageableElement @PackageableElement = new _PackageableElement();
            public IElement @__PackageableElement = new MofElement();

            public class _PackageImport
            {
                public object @importedPackage = "importedPackage";

                public object @importingNamespace = "importingNamespace";

                public object @visibility = "visibility";

            }

            public _PackageImport @PackageImport = new _PackageImport();
            public IElement @__PackageImport = new MofElement();

            public class _ParameterableElement
            {
                public object @owningTemplateParameter = "owningTemplateParameter";

                public object @templateParameter = "templateParameter";

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
                public object @relatedElement = "relatedElement";

            }

            public _Relationship @Relationship = new _Relationship();
            public IElement @__Relationship = new MofElement();

            public class _TemplateableElement
            {
                public object @ownedTemplateSignature = "ownedTemplateSignature";

                public object @templateBinding = "templateBinding";

            }

            public _TemplateableElement @TemplateableElement = new _TemplateableElement();
            public IElement @__TemplateableElement = new MofElement();

            public class _TemplateBinding
            {
                public object @boundElement = "boundElement";

                public object @parameterSubstitution = "parameterSubstitution";

                public object @signature = "signature";

            }

            public _TemplateBinding @TemplateBinding = new _TemplateBinding();
            public IElement @__TemplateBinding = new MofElement();

            public class _TemplateParameter
            {
                public object @default = "default";

                public object @ownedDefault = "ownedDefault";

                public object @ownedParameteredElement = "ownedParameteredElement";

                public object @parameteredElement = "parameteredElement";

                public object @signature = "signature";

            }

            public _TemplateParameter @TemplateParameter = new _TemplateParameter();
            public IElement @__TemplateParameter = new MofElement();

            public class _TemplateParameterSubstitution
            {
                public object @actual = "actual";

                public object @formal = "formal";

                public object @ownedActual = "ownedActual";

                public object @templateBinding = "templateBinding";

            }

            public _TemplateParameterSubstitution @TemplateParameterSubstitution = new _TemplateParameterSubstitution();
            public IElement @__TemplateParameterSubstitution = new MofElement();

            public class _TemplateSignature
            {
                public object @ownedParameter = "ownedParameter";

                public object @parameter = "parameter";

                public object @template = "template";

            }

            public _TemplateSignature @TemplateSignature = new _TemplateSignature();
            public IElement @__TemplateSignature = new MofElement();

            public class _Type
            {
                public object @package = "package";

            }

            public _Type @Type = new _Type();
            public IElement @__Type = new MofElement();

            public class _TypedElement
            {
                public object @type = "type";

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
                public object @context = "context";

                public object @isReentrant = "isReentrant";

                public object @ownedParameter = "ownedParameter";

                public object @ownedParameterSet = "ownedParameterSet";

                public object @postcondition = "postcondition";

                public object @precondition = "precondition";

                public object @specification = "specification";

                public object @redefinedBehavior = "redefinedBehavior";

            }

            public _Behavior @Behavior = new _Behavior();
            public IElement @__Behavior = new MofElement();

            public class _CallEvent
            {
                public object @operation = "operation";

            }

            public _CallEvent @CallEvent = new _CallEvent();
            public IElement @__CallEvent = new MofElement();

            public class _ChangeEvent
            {
                public object @changeExpression = "changeExpression";

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
                public object @body = "body";

                public object @language = "language";

            }

            public _OpaqueBehavior @OpaqueBehavior = new _OpaqueBehavior();
            public IElement @__OpaqueBehavior = new MofElement();

            public class _SignalEvent
            {
                public object @signal = "signal";

            }

            public _SignalEvent @SignalEvent = new _SignalEvent();
            public IElement @__SignalEvent = new MofElement();

            public class _TimeEvent
            {
                public object @isRelative = "isRelative";

                public object @when = "when";

            }

            public _TimeEvent @TimeEvent = new _TimeEvent();
            public IElement @__TimeEvent = new MofElement();

            public class _Trigger
            {
                public object @event = "event";

                public object @port = "port";

            }

            public _Trigger @Trigger = new _Trigger();
            public IElement @__Trigger = new MofElement();

        }

        public _CommonBehavior CommonBehavior = new _CommonBehavior();

        public class _Classification
        {
            public class _Substitution
            {
                public object @contract = "contract";

                public object @substitutingClassifier = "substitutingClassifier";

            }

            public _Substitution @Substitution = new _Substitution();
            public IElement @__Substitution = new MofElement();

            public class _BehavioralFeature
            {
                public object @concurrency = "concurrency";

                public object @isAbstract = "isAbstract";

                public object @method = "method";

                public object @ownedParameter = "ownedParameter";

                public object @ownedParameterSet = "ownedParameterSet";

                public object @raisedException = "raisedException";

            }

            public _BehavioralFeature @BehavioralFeature = new _BehavioralFeature();
            public IElement @__BehavioralFeature = new MofElement();

            public class _Classifier
            {
                public object @attribute = "attribute";

                public object @collaborationUse = "collaborationUse";

                public object @feature = "feature";

                public object @general = "general";

                public object @generalization = "generalization";

                public object @inheritedMember = "inheritedMember";

                public object @isAbstract = "isAbstract";

                public object @isFinalSpecialization = "isFinalSpecialization";

                public object @ownedTemplateSignature = "ownedTemplateSignature";

                public object @ownedUseCase = "ownedUseCase";

                public object @powertypeExtent = "powertypeExtent";

                public object @redefinedClassifier = "redefinedClassifier";

                public object @representation = "representation";

                public object @substitution = "substitution";

                public object @templateParameter = "templateParameter";

                public object @useCase = "useCase";

            }

            public _Classifier @Classifier = new _Classifier();
            public IElement @__Classifier = new MofElement();

            public class _ClassifierTemplateParameter
            {
                public object @allowSubstitutable = "allowSubstitutable";

                public object @constrainingClassifier = "constrainingClassifier";

                public object @parameteredElement = "parameteredElement";

            }

            public _ClassifierTemplateParameter @ClassifierTemplateParameter = new _ClassifierTemplateParameter();
            public IElement @__ClassifierTemplateParameter = new MofElement();

            public class _Feature
            {
                public object @featuringClassifier = "featuringClassifier";

                public object @isStatic = "isStatic";

            }

            public _Feature @Feature = new _Feature();
            public IElement @__Feature = new MofElement();

            public class _Generalization
            {
                public object @general = "general";

                public object @generalizationSet = "generalizationSet";

                public object @isSubstitutable = "isSubstitutable";

                public object @specific = "specific";

            }

            public _Generalization @Generalization = new _Generalization();
            public IElement @__Generalization = new MofElement();

            public class _GeneralizationSet
            {
                public object @generalization = "generalization";

                public object @isCovering = "isCovering";

                public object @isDisjoint = "isDisjoint";

                public object @powertype = "powertype";

            }

            public _GeneralizationSet @GeneralizationSet = new _GeneralizationSet();
            public IElement @__GeneralizationSet = new MofElement();

            public class _InstanceSpecification
            {
                public object @classifier = "classifier";

                public object @slot = "slot";

                public object @specification = "specification";

            }

            public _InstanceSpecification @InstanceSpecification = new _InstanceSpecification();
            public IElement @__InstanceSpecification = new MofElement();

            public class _InstanceValue
            {
                public object @instance = "instance";

            }

            public _InstanceValue @InstanceValue = new _InstanceValue();
            public IElement @__InstanceValue = new MofElement();

            public class _Operation
            {
                public object @bodyCondition = "bodyCondition";

                public object @class = "class";

                public object @datatype = "datatype";

                public object @interface = "interface";

                public object @isOrdered = "isOrdered";

                public object @isQuery = "isQuery";

                public object @isUnique = "isUnique";

                public object @lower = "lower";

                public object @ownedParameter = "ownedParameter";

                public object @postcondition = "postcondition";

                public object @precondition = "precondition";

                public object @raisedException = "raisedException";

                public object @redefinedOperation = "redefinedOperation";

                public object @templateParameter = "templateParameter";

                public object @type = "type";

                public object @upper = "upper";

            }

            public _Operation @Operation = new _Operation();
            public IElement @__Operation = new MofElement();

            public class _OperationTemplateParameter
            {
                public object @parameteredElement = "parameteredElement";

            }

            public _OperationTemplateParameter @OperationTemplateParameter = new _OperationTemplateParameter();
            public IElement @__OperationTemplateParameter = new MofElement();

            public class _Parameter
            {
                public object @default = "default";

                public object @defaultValue = "defaultValue";

                public object @direction = "direction";

                public object @effect = "effect";

                public object @isException = "isException";

                public object @isStream = "isStream";

                public object @operation = "operation";

                public object @parameterSet = "parameterSet";

            }

            public _Parameter @Parameter = new _Parameter();
            public IElement @__Parameter = new MofElement();

            public class _ParameterSet
            {
                public object @condition = "condition";

                public object @parameter = "parameter";

            }

            public _ParameterSet @ParameterSet = new _ParameterSet();
            public IElement @__ParameterSet = new MofElement();

            public class _Property
            {
                public object @aggregation = "aggregation";

                public object @association = "association";

                public object @associationEnd = "associationEnd";

                public object @class = "class";

                public object @datatype = "datatype";

                public object @defaultValue = "defaultValue";

                public object @interface = "interface";

                public object @isComposite = "isComposite";

                public object @isDerived = "isDerived";

                public object @isDerivedUnion = "isDerivedUnion";

                public object @isID = "isID";

                public object @opposite = "opposite";

                public object @owningAssociation = "owningAssociation";

                public object @qualifier = "qualifier";

                public object @redefinedProperty = "redefinedProperty";

                public object @subsettedProperty = "subsettedProperty";

            }

            public _Property @Property = new _Property();
            public IElement @__Property = new MofElement();

            public class _RedefinableElement
            {
                public object @isLeaf = "isLeaf";

                public object @redefinedElement = "redefinedElement";

                public object @redefinitionContext = "redefinitionContext";

            }

            public _RedefinableElement @RedefinableElement = new _RedefinableElement();
            public IElement @__RedefinableElement = new MofElement();

            public class _RedefinableTemplateSignature
            {
                public object @classifier = "classifier";

                public object @extendedSignature = "extendedSignature";

                public object @inheritedParameter = "inheritedParameter";

            }

            public _RedefinableTemplateSignature @RedefinableTemplateSignature = new _RedefinableTemplateSignature();
            public IElement @__RedefinableTemplateSignature = new MofElement();

            public class _Slot
            {
                public object @definingFeature = "definingFeature";

                public object @owningInstance = "owningInstance";

                public object @value = "value";

            }

            public _Slot @Slot = new _Slot();
            public IElement @__Slot = new MofElement();

            public class _StructuralFeature
            {
                public object @isReadOnly = "isReadOnly";

            }

            public _StructuralFeature @StructuralFeature = new _StructuralFeature();
            public IElement @__StructuralFeature = new MofElement();

        }

        public _Classification Classification = new _Classification();

        public class _Actions
        {
            public class _ValueSpecificationAction
            {
                public object @result = "result";

                public object @value = "value";

            }

            public _ValueSpecificationAction @ValueSpecificationAction = new _ValueSpecificationAction();
            public IElement @__ValueSpecificationAction = new MofElement();

            public class _VariableAction
            {
                public object @variable = "variable";

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
                public object @result = "result";

                public object @value = "value";

            }

            public _WriteStructuralFeatureAction @WriteStructuralFeatureAction = new _WriteStructuralFeatureAction();
            public IElement @__WriteStructuralFeatureAction = new MofElement();

            public class _WriteVariableAction
            {
                public object @value = "value";

            }

            public _WriteVariableAction @WriteVariableAction = new _WriteVariableAction();
            public IElement @__WriteVariableAction = new MofElement();

            public class _AcceptCallAction
            {
                public object @returnInformation = "returnInformation";

            }

            public _AcceptCallAction @AcceptCallAction = new _AcceptCallAction();
            public IElement @__AcceptCallAction = new MofElement();

            public class _AcceptEventAction
            {
                public object @isUnmarshall = "isUnmarshall";

                public object @result = "result";

                public object @trigger = "trigger";

            }

            public _AcceptEventAction @AcceptEventAction = new _AcceptEventAction();
            public IElement @__AcceptEventAction = new MofElement();

            public class _Action
            {
                public object @context = "context";

                public object @input = "input";

                public object @isLocallyReentrant = "isLocallyReentrant";

                public object @localPostcondition = "localPostcondition";

                public object @localPrecondition = "localPrecondition";

                public object @output = "output";

            }

            public _Action @Action = new _Action();
            public IElement @__Action = new MofElement();

            public class _ActionInputPin
            {
                public object @fromAction = "fromAction";

            }

            public _ActionInputPin @ActionInputPin = new _ActionInputPin();
            public IElement @__ActionInputPin = new MofElement();

            public class _AddStructuralFeatureValueAction
            {
                public object @insertAt = "insertAt";

                public object @isReplaceAll = "isReplaceAll";

            }

            public _AddStructuralFeatureValueAction @AddStructuralFeatureValueAction = new _AddStructuralFeatureValueAction();
            public IElement @__AddStructuralFeatureValueAction = new MofElement();

            public class _AddVariableValueAction
            {
                public object @insertAt = "insertAt";

                public object @isReplaceAll = "isReplaceAll";

            }

            public _AddVariableValueAction @AddVariableValueAction = new _AddVariableValueAction();
            public IElement @__AddVariableValueAction = new MofElement();

            public class _BroadcastSignalAction
            {
                public object @signal = "signal";

            }

            public _BroadcastSignalAction @BroadcastSignalAction = new _BroadcastSignalAction();
            public IElement @__BroadcastSignalAction = new MofElement();

            public class _CallAction
            {
                public object @isSynchronous = "isSynchronous";

                public object @result = "result";

            }

            public _CallAction @CallAction = new _CallAction();
            public IElement @__CallAction = new MofElement();

            public class _CallBehaviorAction
            {
                public object @behavior = "behavior";

            }

            public _CallBehaviorAction @CallBehaviorAction = new _CallBehaviorAction();
            public IElement @__CallBehaviorAction = new MofElement();

            public class _CallOperationAction
            {
                public object @operation = "operation";

                public object @target = "target";

            }

            public _CallOperationAction @CallOperationAction = new _CallOperationAction();
            public IElement @__CallOperationAction = new MofElement();

            public class _Clause
            {
                public object @body = "body";

                public object @bodyOutput = "bodyOutput";

                public object @decider = "decider";

                public object @predecessorClause = "predecessorClause";

                public object @successorClause = "successorClause";

                public object @test = "test";

            }

            public _Clause @Clause = new _Clause();
            public IElement @__Clause = new MofElement();

            public class _ClearAssociationAction
            {
                public object @association = "association";

                public object @object = "object";

            }

            public _ClearAssociationAction @ClearAssociationAction = new _ClearAssociationAction();
            public IElement @__ClearAssociationAction = new MofElement();

            public class _ClearStructuralFeatureAction
            {
                public object @result = "result";

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
                public object @clause = "clause";

                public object @isAssured = "isAssured";

                public object @isDeterminate = "isDeterminate";

                public object @result = "result";

            }

            public _ConditionalNode @ConditionalNode = new _ConditionalNode();
            public IElement @__ConditionalNode = new MofElement();

            public class _CreateLinkAction
            {
                public object @endData = "endData";

            }

            public _CreateLinkAction @CreateLinkAction = new _CreateLinkAction();
            public IElement @__CreateLinkAction = new MofElement();

            public class _CreateLinkObjectAction
            {
                public object @result = "result";

            }

            public _CreateLinkObjectAction @CreateLinkObjectAction = new _CreateLinkObjectAction();
            public IElement @__CreateLinkObjectAction = new MofElement();

            public class _CreateObjectAction
            {
                public object @classifier = "classifier";

                public object @result = "result";

            }

            public _CreateObjectAction @CreateObjectAction = new _CreateObjectAction();
            public IElement @__CreateObjectAction = new MofElement();

            public class _DestroyLinkAction
            {
                public object @endData = "endData";

            }

            public _DestroyLinkAction @DestroyLinkAction = new _DestroyLinkAction();
            public IElement @__DestroyLinkAction = new MofElement();

            public class _DestroyObjectAction
            {
                public object @isDestroyLinks = "isDestroyLinks";

                public object @isDestroyOwnedObjects = "isDestroyOwnedObjects";

                public object @target = "target";

            }

            public _DestroyObjectAction @DestroyObjectAction = new _DestroyObjectAction();
            public IElement @__DestroyObjectAction = new MofElement();

            public class _ExpansionNode
            {
                public object @regionAsInput = "regionAsInput";

                public object @regionAsOutput = "regionAsOutput";

            }

            public _ExpansionNode @ExpansionNode = new _ExpansionNode();
            public IElement @__ExpansionNode = new MofElement();

            public class _ExpansionRegion
            {
                public object @inputElement = "inputElement";

                public object @mode = "mode";

                public object @outputElement = "outputElement";

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
                public object @argument = "argument";

                public object @onPort = "onPort";

            }

            public _InvocationAction @InvocationAction = new _InvocationAction();
            public IElement @__InvocationAction = new MofElement();

            public class _LinkAction
            {
                public object @endData = "endData";

                public object @inputValue = "inputValue";

            }

            public _LinkAction @LinkAction = new _LinkAction();
            public IElement @__LinkAction = new MofElement();

            public class _LinkEndCreationData
            {
                public object @insertAt = "insertAt";

                public object @isReplaceAll = "isReplaceAll";

            }

            public _LinkEndCreationData @LinkEndCreationData = new _LinkEndCreationData();
            public IElement @__LinkEndCreationData = new MofElement();

            public class _LinkEndData
            {
                public object @end = "end";

                public object @qualifier = "qualifier";

                public object @value = "value";

            }

            public _LinkEndData @LinkEndData = new _LinkEndData();
            public IElement @__LinkEndData = new MofElement();

            public class _LinkEndDestructionData
            {
                public object @destroyAt = "destroyAt";

                public object @isDestroyDuplicates = "isDestroyDuplicates";

            }

            public _LinkEndDestructionData @LinkEndDestructionData = new _LinkEndDestructionData();
            public IElement @__LinkEndDestructionData = new MofElement();

            public class _LoopNode
            {
                public object @bodyOutput = "bodyOutput";

                public object @bodyPart = "bodyPart";

                public object @decider = "decider";

                public object @isTestedFirst = "isTestedFirst";

                public object @loopVariable = "loopVariable";

                public object @loopVariableInput = "loopVariableInput";

                public object @result = "result";

                public object @setupPart = "setupPart";

                public object @test = "test";

            }

            public _LoopNode @LoopNode = new _LoopNode();
            public IElement @__LoopNode = new MofElement();

            public class _OpaqueAction
            {
                public object @body = "body";

                public object @inputValue = "inputValue";

                public object @language = "language";

                public object @outputValue = "outputValue";

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
                public object @isControl = "isControl";

            }

            public _Pin @Pin = new _Pin();
            public IElement @__Pin = new MofElement();

            public class _QualifierValue
            {
                public object @qualifier = "qualifier";

                public object @value = "value";

            }

            public _QualifierValue @QualifierValue = new _QualifierValue();
            public IElement @__QualifierValue = new MofElement();

            public class _RaiseExceptionAction
            {
                public object @exception = "exception";

            }

            public _RaiseExceptionAction @RaiseExceptionAction = new _RaiseExceptionAction();
            public IElement @__RaiseExceptionAction = new MofElement();

            public class _ReadExtentAction
            {
                public object @classifier = "classifier";

                public object @result = "result";

            }

            public _ReadExtentAction @ReadExtentAction = new _ReadExtentAction();
            public IElement @__ReadExtentAction = new MofElement();

            public class _ReadIsClassifiedObjectAction
            {
                public object @classifier = "classifier";

                public object @isDirect = "isDirect";

                public object @object = "object";

                public object @result = "result";

            }

            public _ReadIsClassifiedObjectAction @ReadIsClassifiedObjectAction = new _ReadIsClassifiedObjectAction();
            public IElement @__ReadIsClassifiedObjectAction = new MofElement();

            public class _ReadLinkAction
            {
                public object @result = "result";

            }

            public _ReadLinkAction @ReadLinkAction = new _ReadLinkAction();
            public IElement @__ReadLinkAction = new MofElement();

            public class _ReadLinkObjectEndAction
            {
                public object @end = "end";

                public object @object = "object";

                public object @result = "result";

            }

            public _ReadLinkObjectEndAction @ReadLinkObjectEndAction = new _ReadLinkObjectEndAction();
            public IElement @__ReadLinkObjectEndAction = new MofElement();

            public class _ReadLinkObjectEndQualifierAction
            {
                public object @object = "object";

                public object @qualifier = "qualifier";

                public object @result = "result";

            }

            public _ReadLinkObjectEndQualifierAction @ReadLinkObjectEndQualifierAction = new _ReadLinkObjectEndQualifierAction();
            public IElement @__ReadLinkObjectEndQualifierAction = new MofElement();

            public class _ReadSelfAction
            {
                public object @result = "result";

            }

            public _ReadSelfAction @ReadSelfAction = new _ReadSelfAction();
            public IElement @__ReadSelfAction = new MofElement();

            public class _ReadStructuralFeatureAction
            {
                public object @result = "result";

            }

            public _ReadStructuralFeatureAction @ReadStructuralFeatureAction = new _ReadStructuralFeatureAction();
            public IElement @__ReadStructuralFeatureAction = new MofElement();

            public class _ReadVariableAction
            {
                public object @result = "result";

            }

            public _ReadVariableAction @ReadVariableAction = new _ReadVariableAction();
            public IElement @__ReadVariableAction = new MofElement();

            public class _ReclassifyObjectAction
            {
                public object @isReplaceAll = "isReplaceAll";

                public object @newClassifier = "newClassifier";

                public object @object = "object";

                public object @oldClassifier = "oldClassifier";

            }

            public _ReclassifyObjectAction @ReclassifyObjectAction = new _ReclassifyObjectAction();
            public IElement @__ReclassifyObjectAction = new MofElement();

            public class _ReduceAction
            {
                public object @collection = "collection";

                public object @isOrdered = "isOrdered";

                public object @reducer = "reducer";

                public object @result = "result";

            }

            public _ReduceAction @ReduceAction = new _ReduceAction();
            public IElement @__ReduceAction = new MofElement();

            public class _RemoveStructuralFeatureValueAction
            {
                public object @isRemoveDuplicates = "isRemoveDuplicates";

                public object @removeAt = "removeAt";

            }

            public _RemoveStructuralFeatureValueAction @RemoveStructuralFeatureValueAction = new _RemoveStructuralFeatureValueAction();
            public IElement @__RemoveStructuralFeatureValueAction = new MofElement();

            public class _RemoveVariableValueAction
            {
                public object @isRemoveDuplicates = "isRemoveDuplicates";

                public object @removeAt = "removeAt";

            }

            public _RemoveVariableValueAction @RemoveVariableValueAction = new _RemoveVariableValueAction();
            public IElement @__RemoveVariableValueAction = new MofElement();

            public class _ReplyAction
            {
                public object @replyToCall = "replyToCall";

                public object @replyValue = "replyValue";

                public object @returnInformation = "returnInformation";

            }

            public _ReplyAction @ReplyAction = new _ReplyAction();
            public IElement @__ReplyAction = new MofElement();

            public class _SendObjectAction
            {
                public object @request = "request";

                public object @target = "target";

            }

            public _SendObjectAction @SendObjectAction = new _SendObjectAction();
            public IElement @__SendObjectAction = new MofElement();

            public class _SendSignalAction
            {
                public object @signal = "signal";

                public object @target = "target";

            }

            public _SendSignalAction @SendSignalAction = new _SendSignalAction();
            public IElement @__SendSignalAction = new MofElement();

            public class _SequenceNode
            {
                public object @executableNode = "executableNode";

            }

            public _SequenceNode @SequenceNode = new _SequenceNode();
            public IElement @__SequenceNode = new MofElement();

            public class _StartClassifierBehaviorAction
            {
                public object @object = "object";

            }

            public _StartClassifierBehaviorAction @StartClassifierBehaviorAction = new _StartClassifierBehaviorAction();
            public IElement @__StartClassifierBehaviorAction = new MofElement();

            public class _StartObjectBehaviorAction
            {
                public object @object = "object";

            }

            public _StartObjectBehaviorAction @StartObjectBehaviorAction = new _StartObjectBehaviorAction();
            public IElement @__StartObjectBehaviorAction = new MofElement();

            public class _StructuralFeatureAction
            {
                public object @object = "object";

                public object @structuralFeature = "structuralFeature";

            }

            public _StructuralFeatureAction @StructuralFeatureAction = new _StructuralFeatureAction();
            public IElement @__StructuralFeatureAction = new MofElement();

            public class _StructuredActivityNode
            {
                public object @activity = "activity";

                public object @edge = "edge";

                public object @mustIsolate = "mustIsolate";

                public object @node = "node";

                public object @structuredNodeInput = "structuredNodeInput";

                public object @structuredNodeOutput = "structuredNodeOutput";

                public object @variable = "variable";

            }

            public _StructuredActivityNode @StructuredActivityNode = new _StructuredActivityNode();
            public IElement @__StructuredActivityNode = new MofElement();

            public class _TestIdentityAction
            {
                public object @first = "first";

                public object @result = "result";

                public object @second = "second";

            }

            public _TestIdentityAction @TestIdentityAction = new _TestIdentityAction();
            public IElement @__TestIdentityAction = new MofElement();

            public class _UnmarshallAction
            {
                public object @object = "object";

                public object @result = "result";

                public object @unmarshallType = "unmarshallType";

            }

            public _UnmarshallAction @UnmarshallAction = new _UnmarshallAction();
            public IElement @__UnmarshallAction = new MofElement();

            public class _ValuePin
            {
                public object @value = "value";

            }

            public _ValuePin @ValuePin = new _ValuePin();
            public IElement @__ValuePin = new MofElement();

        }

        public _Actions Actions = new _Actions();

        public static _UML TheOne = new _UML();

    }

}
