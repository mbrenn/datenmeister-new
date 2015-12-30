// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.0.1.0
namespace DatenMeister
{
    public class _UML
    {
        public class _Activities
        {
            public class _Activity
            {
                public object @edge = new object();

                public object @group = new object();

                public object @isReadOnly = new object();

                public object @isSingleExecution = new object();

                public object @node = new object();

                public object @partition = new object();

                public object @structuredNode = new object();

                public object @variable = new object();

            }

            public _Activity @Activity = new _Activity();
            public object @__Activity = new object();

            public class _ActivityEdge
            {
                public object @activity = new object();

                public object @guard = new object();

                public object @inGroup = new object();

                public object @inPartition = new object();

                public object @inStructuredNode = new object();

                public object @interrupts = new object();

                public object @redefinedEdge = new object();

                public object @source = new object();

                public object @target = new object();

                public object @weight = new object();

            }

            public _ActivityEdge @ActivityEdge = new _ActivityEdge();
            public object @__ActivityEdge = new object();

            public class _ActivityFinalNode
            {
            }

            public _ActivityFinalNode @ActivityFinalNode = new _ActivityFinalNode();
            public object @__ActivityFinalNode = new object();

            public class _ActivityGroup
            {
                public object @containedEdge = new object();

                public object @containedNode = new object();

                public object @inActivity = new object();

                public object @subgroup = new object();

                public object @superGroup = new object();

            }

            public _ActivityGroup @ActivityGroup = new _ActivityGroup();
            public object @__ActivityGroup = new object();

            public class _ActivityNode
            {
                public object @activity = new object();

                public object @inGroup = new object();

                public object @inInterruptibleRegion = new object();

                public object @inPartition = new object();

                public object @inStructuredNode = new object();

                public object @incoming = new object();

                public object @outgoing = new object();

                public object @redefinedNode = new object();

            }

            public _ActivityNode @ActivityNode = new _ActivityNode();
            public object @__ActivityNode = new object();

            public class _ActivityParameterNode
            {
                public object @parameter = new object();

            }

            public _ActivityParameterNode @ActivityParameterNode = new _ActivityParameterNode();
            public object @__ActivityParameterNode = new object();

            public class _ActivityPartition
            {
                public object @edge = new object();

                public object @isDimension = new object();

                public object @isExternal = new object();

                public object @node = new object();

                public object @represents = new object();

                public object @subpartition = new object();

                public object @superPartition = new object();

            }

            public _ActivityPartition @ActivityPartition = new _ActivityPartition();
            public object @__ActivityPartition = new object();

            public class _CentralBufferNode
            {
            }

            public _CentralBufferNode @CentralBufferNode = new _CentralBufferNode();
            public object @__CentralBufferNode = new object();

            public class _ControlFlow
            {
            }

            public _ControlFlow @ControlFlow = new _ControlFlow();
            public object @__ControlFlow = new object();

            public class _ControlNode
            {
            }

            public _ControlNode @ControlNode = new _ControlNode();
            public object @__ControlNode = new object();

            public class _DataStoreNode
            {
            }

            public _DataStoreNode @DataStoreNode = new _DataStoreNode();
            public object @__DataStoreNode = new object();

            public class _DecisionNode
            {
                public object @decisionInput = new object();

                public object @decisionInputFlow = new object();

            }

            public _DecisionNode @DecisionNode = new _DecisionNode();
            public object @__DecisionNode = new object();

            public class _ExceptionHandler
            {
                public object @exceptionInput = new object();

                public object @exceptionType = new object();

                public object @handlerBody = new object();

                public object @protectedNode = new object();

            }

            public _ExceptionHandler @ExceptionHandler = new _ExceptionHandler();
            public object @__ExceptionHandler = new object();

            public class _ExecutableNode
            {
                public object @handler = new object();

            }

            public _ExecutableNode @ExecutableNode = new _ExecutableNode();
            public object @__ExecutableNode = new object();

            public class _FinalNode
            {
            }

            public _FinalNode @FinalNode = new _FinalNode();
            public object @__FinalNode = new object();

            public class _FlowFinalNode
            {
            }

            public _FlowFinalNode @FlowFinalNode = new _FlowFinalNode();
            public object @__FlowFinalNode = new object();

            public class _ForkNode
            {
            }

            public _ForkNode @ForkNode = new _ForkNode();
            public object @__ForkNode = new object();

            public class _InitialNode
            {
            }

            public _InitialNode @InitialNode = new _InitialNode();
            public object @__InitialNode = new object();

            public class _InterruptibleActivityRegion
            {
                public object @interruptingEdge = new object();

                public object @node = new object();

            }

            public _InterruptibleActivityRegion @InterruptibleActivityRegion = new _InterruptibleActivityRegion();
            public object @__InterruptibleActivityRegion = new object();

            public class _JoinNode
            {
                public object @isCombineDuplicate = new object();

                public object @joinSpec = new object();

            }

            public _JoinNode @JoinNode = new _JoinNode();
            public object @__JoinNode = new object();

            public class _MergeNode
            {
            }

            public _MergeNode @MergeNode = new _MergeNode();
            public object @__MergeNode = new object();

            public class _ObjectFlow
            {
                public object @isMulticast = new object();

                public object @isMultireceive = new object();

                public object @selection = new object();

                public object @transformation = new object();

            }

            public _ObjectFlow @ObjectFlow = new _ObjectFlow();
            public object @__ObjectFlow = new object();

            public class _ObjectNode
            {
                public object @inState = new object();

                public object @isControlType = new object();

                public object @ordering = new object();

                public object @selection = new object();

                public object @upperBound = new object();

            }

            public _ObjectNode @ObjectNode = new _ObjectNode();
            public object @__ObjectNode = new object();

            public class _Variable
            {
                public object @activityScope = new object();

                public object @scope = new object();

            }

            public _Variable @Variable = new _Variable();
            public object @__Variable = new object();

        }

        public _Activities Activities = new _Activities();

        public class _Values
        {
            public class _Duration
            {
                public object @expr = new object();

                public object @observation = new object();

            }

            public _Duration @Duration = new _Duration();
            public object @__Duration = new object();

            public class _DurationConstraint
            {
                public object @firstEvent = new object();

                public object @specification = new object();

            }

            public _DurationConstraint @DurationConstraint = new _DurationConstraint();
            public object @__DurationConstraint = new object();

            public class _DurationInterval
            {
                public object @max = new object();

                public object @min = new object();

            }

            public _DurationInterval @DurationInterval = new _DurationInterval();
            public object @__DurationInterval = new object();

            public class _DurationObservation
            {
                public object @event = new object();

                public object @firstEvent = new object();

            }

            public _DurationObservation @DurationObservation = new _DurationObservation();
            public object @__DurationObservation = new object();

            public class _Expression
            {
                public object @operand = new object();

                public object @symbol = new object();

            }

            public _Expression @Expression = new _Expression();
            public object @__Expression = new object();

            public class _Interval
            {
                public object @max = new object();

                public object @min = new object();

            }

            public _Interval @Interval = new _Interval();
            public object @__Interval = new object();

            public class _IntervalConstraint
            {
                public object @specification = new object();

            }

            public _IntervalConstraint @IntervalConstraint = new _IntervalConstraint();
            public object @__IntervalConstraint = new object();

            public class _LiteralBoolean
            {
                public object @value = new object();

            }

            public _LiteralBoolean @LiteralBoolean = new _LiteralBoolean();
            public object @__LiteralBoolean = new object();

            public class _LiteralInteger
            {
                public object @value = new object();

            }

            public _LiteralInteger @LiteralInteger = new _LiteralInteger();
            public object @__LiteralInteger = new object();

            public class _LiteralNull
            {
            }

            public _LiteralNull @LiteralNull = new _LiteralNull();
            public object @__LiteralNull = new object();

            public class _LiteralReal
            {
                public object @value = new object();

            }

            public _LiteralReal @LiteralReal = new _LiteralReal();
            public object @__LiteralReal = new object();

            public class _LiteralSpecification
            {
            }

            public _LiteralSpecification @LiteralSpecification = new _LiteralSpecification();
            public object @__LiteralSpecification = new object();

            public class _LiteralString
            {
                public object @value = new object();

            }

            public _LiteralString @LiteralString = new _LiteralString();
            public object @__LiteralString = new object();

            public class _LiteralUnlimitedNatural
            {
                public object @value = new object();

            }

            public _LiteralUnlimitedNatural @LiteralUnlimitedNatural = new _LiteralUnlimitedNatural();
            public object @__LiteralUnlimitedNatural = new object();

            public class _Observation
            {
            }

            public _Observation @Observation = new _Observation();
            public object @__Observation = new object();

            public class _OpaqueExpression
            {
                public object @behavior = new object();

                public object @body = new object();

                public object @language = new object();

                public object @result = new object();

            }

            public _OpaqueExpression @OpaqueExpression = new _OpaqueExpression();
            public object @__OpaqueExpression = new object();

            public class _StringExpression
            {
                public object @owningExpression = new object();

                public object @subExpression = new object();

            }

            public _StringExpression @StringExpression = new _StringExpression();
            public object @__StringExpression = new object();

            public class _TimeConstraint
            {
                public object @firstEvent = new object();

                public object @specification = new object();

            }

            public _TimeConstraint @TimeConstraint = new _TimeConstraint();
            public object @__TimeConstraint = new object();

            public class _TimeExpression
            {
                public object @expr = new object();

                public object @observation = new object();

            }

            public _TimeExpression @TimeExpression = new _TimeExpression();
            public object @__TimeExpression = new object();

            public class _TimeInterval
            {
                public object @max = new object();

                public object @min = new object();

            }

            public _TimeInterval @TimeInterval = new _TimeInterval();
            public object @__TimeInterval = new object();

            public class _TimeObservation
            {
                public object @event = new object();

                public object @firstEvent = new object();

            }

            public _TimeObservation @TimeObservation = new _TimeObservation();
            public object @__TimeObservation = new object();

            public class _ValueSpecification
            {
            }

            public _ValueSpecification @ValueSpecification = new _ValueSpecification();
            public object @__ValueSpecification = new object();

        }

        public _Values Values = new _Values();

        public class _UseCases
        {
            public class _Actor
            {
            }

            public _Actor @Actor = new _Actor();
            public object @__Actor = new object();

            public class _Extend
            {
                public object @condition = new object();

                public object @extendedCase = new object();

                public object @extension = new object();

                public object @extensionLocation = new object();

            }

            public _Extend @Extend = new _Extend();
            public object @__Extend = new object();

            public class _ExtensionPoint
            {
                public object @useCase = new object();

            }

            public _ExtensionPoint @ExtensionPoint = new _ExtensionPoint();
            public object @__ExtensionPoint = new object();

            public class _Include
            {
                public object @addition = new object();

                public object @includingCase = new object();

            }

            public _Include @Include = new _Include();
            public object @__Include = new object();

            public class _UseCase
            {
                public object @extend = new object();

                public object @extensionPoint = new object();

                public object @include = new object();

                public object @subject = new object();

            }

            public _UseCase @UseCase = new _UseCase();
            public object @__UseCase = new object();

        }

        public _UseCases UseCases = new _UseCases();

        public class _StructuredClassifiers
        {
            public class _Association
            {
                public object @endType = new object();

                public object @isDerived = new object();

                public object @memberEnd = new object();

                public object @navigableOwnedEnd = new object();

                public object @ownedEnd = new object();

            }

            public _Association @Association = new _Association();
            public object @__Association = new object();

            public class _AssociationClass
            {
            }

            public _AssociationClass @AssociationClass = new _AssociationClass();
            public object @__AssociationClass = new object();

            public class _Class
            {
                public object @extension = new object();

                public object @isAbstract = new object();

                public object @isActive = new object();

                public object @nestedClassifier = new object();

                public object @ownedAttribute = new object();

                public object @ownedOperation = new object();

                public object @ownedReception = new object();

                public object @superClass = new object();

            }

            public _Class @Class = new _Class();
            public object @__Class = new object();

            public class _Collaboration
            {
                public object @collaborationRole = new object();

            }

            public _Collaboration @Collaboration = new _Collaboration();
            public object @__Collaboration = new object();

            public class _CollaborationUse
            {
                public object @roleBinding = new object();

                public object @type = new object();

            }

            public _CollaborationUse @CollaborationUse = new _CollaborationUse();
            public object @__CollaborationUse = new object();

            public class _Component
            {
                public object @isIndirectlyInstantiated = new object();

                public object @packagedElement = new object();

                public object @provided = new object();

                public object @realization = new object();

                public object @required = new object();

            }

            public _Component @Component = new _Component();
            public object @__Component = new object();

            public class _ComponentRealization
            {
                public object @abstraction = new object();

                public object @realizingClassifier = new object();

            }

            public _ComponentRealization @ComponentRealization = new _ComponentRealization();
            public object @__ComponentRealization = new object();

            public class _ConnectableElement
            {
                public object @end = new object();

                public object @templateParameter = new object();

            }

            public _ConnectableElement @ConnectableElement = new _ConnectableElement();
            public object @__ConnectableElement = new object();

            public class _ConnectableElementTemplateParameter
            {
                public object @parameteredElement = new object();

            }

            public _ConnectableElementTemplateParameter @ConnectableElementTemplateParameter = new _ConnectableElementTemplateParameter();
            public object @__ConnectableElementTemplateParameter = new object();

            public class _Connector
            {
                public object @contract = new object();

                public object @end = new object();

                public object @kind = new object();

                public object @redefinedConnector = new object();

                public object @type = new object();

            }

            public _Connector @Connector = new _Connector();
            public object @__Connector = new object();

            public class _ConnectorEnd
            {
                public object @definingEnd = new object();

                public object @partWithPort = new object();

                public object @role = new object();

            }

            public _ConnectorEnd @ConnectorEnd = new _ConnectorEnd();
            public object @__ConnectorEnd = new object();

            public class _EncapsulatedClassifier
            {
                public object @ownedPort = new object();

            }

            public _EncapsulatedClassifier @EncapsulatedClassifier = new _EncapsulatedClassifier();
            public object @__EncapsulatedClassifier = new object();

            public class _Port
            {
                public object @isBehavior = new object();

                public object @isConjugated = new object();

                public object @isService = new object();

                public object @protocol = new object();

                public object @provided = new object();

                public object @redefinedPort = new object();

                public object @required = new object();

            }

            public _Port @Port = new _Port();
            public object @__Port = new object();

            public class _StructuredClassifier
            {
                public object @ownedAttribute = new object();

                public object @ownedConnector = new object();

                public object @part = new object();

                public object @role = new object();

            }

            public _StructuredClassifier @StructuredClassifier = new _StructuredClassifier();
            public object @__StructuredClassifier = new object();

        }

        public _StructuredClassifiers StructuredClassifiers = new _StructuredClassifiers();

        public class _StateMachines
        {
            public class _ConnectionPointReference
            {
                public object @entry = new object();

                public object @exit = new object();

                public object @state = new object();

            }

            public _ConnectionPointReference @ConnectionPointReference = new _ConnectionPointReference();
            public object @__ConnectionPointReference = new object();

            public class _FinalState
            {
            }

            public _FinalState @FinalState = new _FinalState();
            public object @__FinalState = new object();

            public class _ProtocolConformance
            {
                public object @generalMachine = new object();

                public object @specificMachine = new object();

            }

            public _ProtocolConformance @ProtocolConformance = new _ProtocolConformance();
            public object @__ProtocolConformance = new object();

            public class _ProtocolStateMachine
            {
                public object @conformance = new object();

            }

            public _ProtocolStateMachine @ProtocolStateMachine = new _ProtocolStateMachine();
            public object @__ProtocolStateMachine = new object();

            public class _ProtocolTransition
            {
                public object @postCondition = new object();

                public object @preCondition = new object();

                public object @referred = new object();

            }

            public _ProtocolTransition @ProtocolTransition = new _ProtocolTransition();
            public object @__ProtocolTransition = new object();

            public class _Pseudostate
            {
                public object @kind = new object();

                public object @state = new object();

                public object @stateMachine = new object();

            }

            public _Pseudostate @Pseudostate = new _Pseudostate();
            public object @__Pseudostate = new object();

            public class _Region
            {
                public object @extendedRegion = new object();

                public object @redefinitionContext = new object();

                public object @state = new object();

                public object @stateMachine = new object();

                public object @subvertex = new object();

                public object @transition = new object();

            }

            public _Region @Region = new _Region();
            public object @__Region = new object();

            public class _State
            {
                public object @connection = new object();

                public object @connectionPoint = new object();

                public object @deferrableTrigger = new object();

                public object @doActivity = new object();

                public object @entry = new object();

                public object @exit = new object();

                public object @isComposite = new object();

                public object @isOrthogonal = new object();

                public object @isSimple = new object();

                public object @isSubmachineState = new object();

                public object @redefinedState = new object();

                public object @redefinitionContext = new object();

                public object @region = new object();

                public object @stateInvariant = new object();

                public object @submachine = new object();

            }

            public _State @State = new _State();
            public object @__State = new object();

            public class _StateMachine
            {
                public object @connectionPoint = new object();

                public object @extendedStateMachine = new object();

                public object @region = new object();

                public object @submachineState = new object();

            }

            public _StateMachine @StateMachine = new _StateMachine();
            public object @__StateMachine = new object();

            public class _Transition
            {
                public object @container = new object();

                public object @effect = new object();

                public object @guard = new object();

                public object @kind = new object();

                public object @redefinedTransition = new object();

                public object @redefinitionContext = new object();

                public object @source = new object();

                public object @target = new object();

                public object @trigger = new object();

            }

            public _Transition @Transition = new _Transition();
            public object @__Transition = new object();

            public class _Vertex
            {
                public object @container = new object();

                public object @incoming = new object();

                public object @outgoing = new object();

            }

            public _Vertex @Vertex = new _Vertex();
            public object @__Vertex = new object();

        }

        public _StateMachines StateMachines = new _StateMachines();

        public class _SimpleClassifiers
        {
            public class _BehavioredClassifier
            {
                public object @classifierBehavior = new object();

                public object @interfaceRealization = new object();

                public object @ownedBehavior = new object();

            }

            public _BehavioredClassifier @BehavioredClassifier = new _BehavioredClassifier();
            public object @__BehavioredClassifier = new object();

            public class _DataType
            {
                public object @ownedAttribute = new object();

                public object @ownedOperation = new object();

            }

            public _DataType @DataType = new _DataType();
            public object @__DataType = new object();

            public class _Enumeration
            {
                public object @ownedLiteral = new object();

            }

            public _Enumeration @Enumeration = new _Enumeration();
            public object @__Enumeration = new object();

            public class _EnumerationLiteral
            {
                public object @classifier = new object();

                public object @enumeration = new object();

            }

            public _EnumerationLiteral @EnumerationLiteral = new _EnumerationLiteral();
            public object @__EnumerationLiteral = new object();

            public class _Interface
            {
                public object @nestedClassifier = new object();

                public object @ownedAttribute = new object();

                public object @ownedOperation = new object();

                public object @ownedReception = new object();

                public object @protocol = new object();

                public object @redefinedInterface = new object();

            }

            public _Interface @Interface = new _Interface();
            public object @__Interface = new object();

            public class _InterfaceRealization
            {
                public object @contract = new object();

                public object @implementingClassifier = new object();

            }

            public _InterfaceRealization @InterfaceRealization = new _InterfaceRealization();
            public object @__InterfaceRealization = new object();

            public class _PrimitiveType
            {
            }

            public _PrimitiveType @PrimitiveType = new _PrimitiveType();
            public object @__PrimitiveType = new object();

            public class _Reception
            {
                public object @signal = new object();

            }

            public _Reception @Reception = new _Reception();
            public object @__Reception = new object();

            public class _Signal
            {
                public object @ownedAttribute = new object();

            }

            public _Signal @Signal = new _Signal();
            public object @__Signal = new object();

        }

        public _SimpleClassifiers SimpleClassifiers = new _SimpleClassifiers();

        public class _Packages
        {
            public class _Extension
            {
                public object @isRequired = new object();

                public object @metaclass = new object();

                public object @ownedEnd = new object();

            }

            public _Extension @Extension = new _Extension();
            public object @__Extension = new object();

            public class _ExtensionEnd
            {
                public object @lower = new object();

                public object @type = new object();

            }

            public _ExtensionEnd @ExtensionEnd = new _ExtensionEnd();
            public object @__ExtensionEnd = new object();

            public class _Image
            {
                public object @content = new object();

                public object @format = new object();

                public object @location = new object();

            }

            public _Image @Image = new _Image();
            public object @__Image = new object();

            public class _Model
            {
                public object @viewpoint = new object();

            }

            public _Model @Model = new _Model();
            public object @__Model = new object();

            public class _Package
            {
                public object @URI = new object();

                public object @nestedPackage = new object();

                public object @nestingPackage = new object();

                public object @ownedStereotype = new object();

                public object @ownedType = new object();

                public object @packageMerge = new object();

                public object @packagedElement = new object();

                public object @profileApplication = new object();

            }

            public _Package @Package = new _Package();
            public object @__Package = new object();

            public class _PackageMerge
            {
                public object @mergedPackage = new object();

                public object @receivingPackage = new object();

            }

            public _PackageMerge @PackageMerge = new _PackageMerge();
            public object @__PackageMerge = new object();

            public class _Profile
            {
                public object @metaclassReference = new object();

                public object @metamodelReference = new object();

            }

            public _Profile @Profile = new _Profile();
            public object @__Profile = new object();

            public class _ProfileApplication
            {
                public object @appliedProfile = new object();

                public object @applyingPackage = new object();

                public object @isStrict = new object();

            }

            public _ProfileApplication @ProfileApplication = new _ProfileApplication();
            public object @__ProfileApplication = new object();

            public class _Stereotype
            {
                public object @icon = new object();

                public object @profile = new object();

            }

            public _Stereotype @Stereotype = new _Stereotype();
            public object @__Stereotype = new object();

        }

        public _Packages Packages = new _Packages();

        public class _Interactions
        {
            public class _ActionExecutionSpecification
            {
                public object @action = new object();

            }

            public _ActionExecutionSpecification @ActionExecutionSpecification = new _ActionExecutionSpecification();
            public object @__ActionExecutionSpecification = new object();

            public class _BehaviorExecutionSpecification
            {
                public object @behavior = new object();

            }

            public _BehaviorExecutionSpecification @BehaviorExecutionSpecification = new _BehaviorExecutionSpecification();
            public object @__BehaviorExecutionSpecification = new object();

            public class _CombinedFragment
            {
                public object @cfragmentGate = new object();

                public object @interactionOperator = new object();

                public object @operand = new object();

            }

            public _CombinedFragment @CombinedFragment = new _CombinedFragment();
            public object @__CombinedFragment = new object();

            public class _ConsiderIgnoreFragment
            {
                public object @message = new object();

            }

            public _ConsiderIgnoreFragment @ConsiderIgnoreFragment = new _ConsiderIgnoreFragment();
            public object @__ConsiderIgnoreFragment = new object();

            public class _Continuation
            {
                public object @setting = new object();

            }

            public _Continuation @Continuation = new _Continuation();
            public object @__Continuation = new object();

            public class _DestructionOccurrenceSpecification
            {
            }

            public _DestructionOccurrenceSpecification @DestructionOccurrenceSpecification = new _DestructionOccurrenceSpecification();
            public object @__DestructionOccurrenceSpecification = new object();

            public class _ExecutionOccurrenceSpecification
            {
                public object @execution = new object();

            }

            public _ExecutionOccurrenceSpecification @ExecutionOccurrenceSpecification = new _ExecutionOccurrenceSpecification();
            public object @__ExecutionOccurrenceSpecification = new object();

            public class _ExecutionSpecification
            {
                public object @finish = new object();

                public object @start = new object();

            }

            public _ExecutionSpecification @ExecutionSpecification = new _ExecutionSpecification();
            public object @__ExecutionSpecification = new object();

            public class _Gate
            {
            }

            public _Gate @Gate = new _Gate();
            public object @__Gate = new object();

            public class _GeneralOrdering
            {
                public object @after = new object();

                public object @before = new object();

            }

            public _GeneralOrdering @GeneralOrdering = new _GeneralOrdering();
            public object @__GeneralOrdering = new object();

            public class _Interaction
            {
                public object @action = new object();

                public object @formalGate = new object();

                public object @fragment = new object();

                public object @lifeline = new object();

                public object @message = new object();

            }

            public _Interaction @Interaction = new _Interaction();
            public object @__Interaction = new object();

            public class _InteractionConstraint
            {
                public object @maxint = new object();

                public object @minint = new object();

            }

            public _InteractionConstraint @InteractionConstraint = new _InteractionConstraint();
            public object @__InteractionConstraint = new object();

            public class _InteractionFragment
            {
                public object @covered = new object();

                public object @enclosingInteraction = new object();

                public object @enclosingOperand = new object();

                public object @generalOrdering = new object();

            }

            public _InteractionFragment @InteractionFragment = new _InteractionFragment();
            public object @__InteractionFragment = new object();

            public class _InteractionOperand
            {
                public object @fragment = new object();

                public object @guard = new object();

            }

            public _InteractionOperand @InteractionOperand = new _InteractionOperand();
            public object @__InteractionOperand = new object();

            public class _InteractionUse
            {
                public object @actualGate = new object();

                public object @argument = new object();

                public object @refersTo = new object();

                public object @returnValue = new object();

                public object @returnValueRecipient = new object();

            }

            public _InteractionUse @InteractionUse = new _InteractionUse();
            public object @__InteractionUse = new object();

            public class _Lifeline
            {
                public object @coveredBy = new object();

                public object @decomposedAs = new object();

                public object @interaction = new object();

                public object @represents = new object();

                public object @selector = new object();

            }

            public _Lifeline @Lifeline = new _Lifeline();
            public object @__Lifeline = new object();

            public class _Message
            {
                public object @argument = new object();

                public object @connector = new object();

                public object @interaction = new object();

                public object @messageKind = new object();

                public object @messageSort = new object();

                public object @receiveEvent = new object();

                public object @sendEvent = new object();

                public object @signature = new object();

            }

            public _Message @Message = new _Message();
            public object @__Message = new object();

            public class _MessageEnd
            {
                public object @message = new object();

            }

            public _MessageEnd @MessageEnd = new _MessageEnd();
            public object @__MessageEnd = new object();

            public class _MessageOccurrenceSpecification
            {
            }

            public _MessageOccurrenceSpecification @MessageOccurrenceSpecification = new _MessageOccurrenceSpecification();
            public object @__MessageOccurrenceSpecification = new object();

            public class _OccurrenceSpecification
            {
                public object @covered = new object();

                public object @toAfter = new object();

                public object @toBefore = new object();

            }

            public _OccurrenceSpecification @OccurrenceSpecification = new _OccurrenceSpecification();
            public object @__OccurrenceSpecification = new object();

            public class _PartDecomposition
            {
            }

            public _PartDecomposition @PartDecomposition = new _PartDecomposition();
            public object @__PartDecomposition = new object();

            public class _StateInvariant
            {
                public object @covered = new object();

                public object @invariant = new object();

            }

            public _StateInvariant @StateInvariant = new _StateInvariant();
            public object @__StateInvariant = new object();

        }

        public _Interactions Interactions = new _Interactions();

        public class _InformationFlows
        {
            public class _InformationFlow
            {
                public object @conveyed = new object();

                public object @informationSource = new object();

                public object @informationTarget = new object();

                public object @realization = new object();

                public object @realizingActivityEdge = new object();

                public object @realizingConnector = new object();

                public object @realizingMessage = new object();

            }

            public _InformationFlow @InformationFlow = new _InformationFlow();
            public object @__InformationFlow = new object();

            public class _InformationItem
            {
                public object @represented = new object();

            }

            public _InformationItem @InformationItem = new _InformationItem();
            public object @__InformationItem = new object();

        }

        public _InformationFlows InformationFlows = new _InformationFlows();

        public class _Deployments
        {
            public class _Artifact
            {
                public object @fileName = new object();

                public object @manifestation = new object();

                public object @nestedArtifact = new object();

                public object @ownedAttribute = new object();

                public object @ownedOperation = new object();

            }

            public _Artifact @Artifact = new _Artifact();
            public object @__Artifact = new object();

            public class _CommunicationPath
            {
            }

            public _CommunicationPath @CommunicationPath = new _CommunicationPath();
            public object @__CommunicationPath = new object();

            public class _DeployedArtifact
            {
            }

            public _DeployedArtifact @DeployedArtifact = new _DeployedArtifact();
            public object @__DeployedArtifact = new object();

            public class _Deployment
            {
                public object @configuration = new object();

                public object @deployedArtifact = new object();

                public object @location = new object();

            }

            public _Deployment @Deployment = new _Deployment();
            public object @__Deployment = new object();

            public class _DeploymentSpecification
            {
                public object @deployment = new object();

                public object @deploymentLocation = new object();

                public object @executionLocation = new object();

            }

            public _DeploymentSpecification @DeploymentSpecification = new _DeploymentSpecification();
            public object @__DeploymentSpecification = new object();

            public class _DeploymentTarget
            {
                public object @deployedElement = new object();

                public object @deployment = new object();

            }

            public _DeploymentTarget @DeploymentTarget = new _DeploymentTarget();
            public object @__DeploymentTarget = new object();

            public class _Device
            {
            }

            public _Device @Device = new _Device();
            public object @__Device = new object();

            public class _ExecutionEnvironment
            {
            }

            public _ExecutionEnvironment @ExecutionEnvironment = new _ExecutionEnvironment();
            public object @__ExecutionEnvironment = new object();

            public class _Manifestation
            {
                public object @utilizedElement = new object();

            }

            public _Manifestation @Manifestation = new _Manifestation();
            public object @__Manifestation = new object();

            public class _Node
            {
                public object @nestedNode = new object();

            }

            public _Node @Node = new _Node();
            public object @__Node = new object();

        }

        public _Deployments Deployments = new _Deployments();

        public class _CommonStructure
        {
            public class _Abstraction
            {
                public object @mapping = new object();

            }

            public _Abstraction @Abstraction = new _Abstraction();
            public object @__Abstraction = new object();

            public class _Comment
            {
                public object @annotatedElement = new object();

                public object @body = new object();

            }

            public _Comment @Comment = new _Comment();
            public object @__Comment = new object();

            public class _Constraint
            {
                public object @constrainedElement = new object();

                public object @context = new object();

                public object @specification = new object();

            }

            public _Constraint @Constraint = new _Constraint();
            public object @__Constraint = new object();

            public class _Dependency
            {
                public object @client = new object();

                public object @supplier = new object();

            }

            public _Dependency @Dependency = new _Dependency();
            public object @__Dependency = new object();

            public class _DirectedRelationship
            {
                public object @source = new object();

                public object @target = new object();

            }

            public _DirectedRelationship @DirectedRelationship = new _DirectedRelationship();
            public object @__DirectedRelationship = new object();

            public class _Element
            {
                public object @ownedComment = new object();

                public object @ownedElement = new object();

                public object @owner = new object();

            }

            public _Element @Element = new _Element();
            public object @__Element = new object();

            public class _ElementImport
            {
                public object @alias = new object();

                public object @importedElement = new object();

                public object @importingNamespace = new object();

                public object @visibility = new object();

            }

            public _ElementImport @ElementImport = new _ElementImport();
            public object @__ElementImport = new object();

            public class _MultiplicityElement
            {
                public object @isOrdered = new object();

                public object @isUnique = new object();

                public object @lower = new object();

                public object @lowerValue = new object();

                public object @upper = new object();

                public object @upperValue = new object();

            }

            public _MultiplicityElement @MultiplicityElement = new _MultiplicityElement();
            public object @__MultiplicityElement = new object();

            public class _NamedElement
            {
                public object @clientDependency = new object();

                public object @name = new object();

                public object @nameExpression = new object();

                public object @namespace = new object();

                public object @qualifiedName = new object();

                public object @visibility = new object();

            }

            public _NamedElement @NamedElement = new _NamedElement();
            public object @__NamedElement = new object();

            public class _Namespace
            {
                public object @elementImport = new object();

                public object @importedMember = new object();

                public object @member = new object();

                public object @ownedMember = new object();

                public object @ownedRule = new object();

                public object @packageImport = new object();

            }

            public _Namespace @Namespace = new _Namespace();
            public object @__Namespace = new object();

            public class _PackageableElement
            {
                public object @visibility = new object();

            }

            public _PackageableElement @PackageableElement = new _PackageableElement();
            public object @__PackageableElement = new object();

            public class _PackageImport
            {
                public object @importedPackage = new object();

                public object @importingNamespace = new object();

                public object @visibility = new object();

            }

            public _PackageImport @PackageImport = new _PackageImport();
            public object @__PackageImport = new object();

            public class _ParameterableElement
            {
                public object @owningTemplateParameter = new object();

                public object @templateParameter = new object();

            }

            public _ParameterableElement @ParameterableElement = new _ParameterableElement();
            public object @__ParameterableElement = new object();

            public class _Realization
            {
            }

            public _Realization @Realization = new _Realization();
            public object @__Realization = new object();

            public class _Relationship
            {
                public object @relatedElement = new object();

            }

            public _Relationship @Relationship = new _Relationship();
            public object @__Relationship = new object();

            public class _TemplateableElement
            {
                public object @ownedTemplateSignature = new object();

                public object @templateBinding = new object();

            }

            public _TemplateableElement @TemplateableElement = new _TemplateableElement();
            public object @__TemplateableElement = new object();

            public class _TemplateBinding
            {
                public object @boundElement = new object();

                public object @parameterSubstitution = new object();

                public object @signature = new object();

            }

            public _TemplateBinding @TemplateBinding = new _TemplateBinding();
            public object @__TemplateBinding = new object();

            public class _TemplateParameter
            {
                public object @default = new object();

                public object @ownedDefault = new object();

                public object @ownedParameteredElement = new object();

                public object @parameteredElement = new object();

                public object @signature = new object();

            }

            public _TemplateParameter @TemplateParameter = new _TemplateParameter();
            public object @__TemplateParameter = new object();

            public class _TemplateParameterSubstitution
            {
                public object @actual = new object();

                public object @formal = new object();

                public object @ownedActual = new object();

                public object @templateBinding = new object();

            }

            public _TemplateParameterSubstitution @TemplateParameterSubstitution = new _TemplateParameterSubstitution();
            public object @__TemplateParameterSubstitution = new object();

            public class _TemplateSignature
            {
                public object @ownedParameter = new object();

                public object @parameter = new object();

                public object @template = new object();

            }

            public _TemplateSignature @TemplateSignature = new _TemplateSignature();
            public object @__TemplateSignature = new object();

            public class _Type
            {
                public object @package = new object();

            }

            public _Type @Type = new _Type();
            public object @__Type = new object();

            public class _TypedElement
            {
                public object @type = new object();

            }

            public _TypedElement @TypedElement = new _TypedElement();
            public object @__TypedElement = new object();

            public class _Usage
            {
            }

            public _Usage @Usage = new _Usage();
            public object @__Usage = new object();

        }

        public _CommonStructure CommonStructure = new _CommonStructure();

        public class _CommonBehavior
        {
            public class _AnyReceiveEvent
            {
            }

            public _AnyReceiveEvent @AnyReceiveEvent = new _AnyReceiveEvent();
            public object @__AnyReceiveEvent = new object();

            public class _Behavior
            {
                public object @context = new object();

                public object @isReentrant = new object();

                public object @ownedParameter = new object();

                public object @ownedParameterSet = new object();

                public object @postcondition = new object();

                public object @precondition = new object();

                public object @specification = new object();

                public object @redefinedBehavior = new object();

            }

            public _Behavior @Behavior = new _Behavior();
            public object @__Behavior = new object();

            public class _CallEvent
            {
                public object @operation = new object();

            }

            public _CallEvent @CallEvent = new _CallEvent();
            public object @__CallEvent = new object();

            public class _ChangeEvent
            {
                public object @changeExpression = new object();

            }

            public _ChangeEvent @ChangeEvent = new _ChangeEvent();
            public object @__ChangeEvent = new object();

            public class _Event
            {
            }

            public _Event @Event = new _Event();
            public object @__Event = new object();

            public class _FunctionBehavior
            {
            }

            public _FunctionBehavior @FunctionBehavior = new _FunctionBehavior();
            public object @__FunctionBehavior = new object();

            public class _MessageEvent
            {
            }

            public _MessageEvent @MessageEvent = new _MessageEvent();
            public object @__MessageEvent = new object();

            public class _OpaqueBehavior
            {
                public object @body = new object();

                public object @language = new object();

            }

            public _OpaqueBehavior @OpaqueBehavior = new _OpaqueBehavior();
            public object @__OpaqueBehavior = new object();

            public class _SignalEvent
            {
                public object @signal = new object();

            }

            public _SignalEvent @SignalEvent = new _SignalEvent();
            public object @__SignalEvent = new object();

            public class _TimeEvent
            {
                public object @isRelative = new object();

                public object @when = new object();

            }

            public _TimeEvent @TimeEvent = new _TimeEvent();
            public object @__TimeEvent = new object();

            public class _Trigger
            {
                public object @event = new object();

                public object @port = new object();

            }

            public _Trigger @Trigger = new _Trigger();
            public object @__Trigger = new object();

        }

        public _CommonBehavior CommonBehavior = new _CommonBehavior();

        public class _Classification
        {
            public class _Substitution
            {
                public object @contract = new object();

                public object @substitutingClassifier = new object();

            }

            public _Substitution @Substitution = new _Substitution();
            public object @__Substitution = new object();

            public class _BehavioralFeature
            {
                public object @concurrency = new object();

                public object @isAbstract = new object();

                public object @method = new object();

                public object @ownedParameter = new object();

                public object @ownedParameterSet = new object();

                public object @raisedException = new object();

            }

            public _BehavioralFeature @BehavioralFeature = new _BehavioralFeature();
            public object @__BehavioralFeature = new object();

            public class _Classifier
            {
                public object @attribute = new object();

                public object @collaborationUse = new object();

                public object @feature = new object();

                public object @general = new object();

                public object @generalization = new object();

                public object @inheritedMember = new object();

                public object @isAbstract = new object();

                public object @isFinalSpecialization = new object();

                public object @ownedTemplateSignature = new object();

                public object @ownedUseCase = new object();

                public object @powertypeExtent = new object();

                public object @redefinedClassifier = new object();

                public object @representation = new object();

                public object @substitution = new object();

                public object @templateParameter = new object();

                public object @useCase = new object();

            }

            public _Classifier @Classifier = new _Classifier();
            public object @__Classifier = new object();

            public class _ClassifierTemplateParameter
            {
                public object @allowSubstitutable = new object();

                public object @constrainingClassifier = new object();

                public object @parameteredElement = new object();

            }

            public _ClassifierTemplateParameter @ClassifierTemplateParameter = new _ClassifierTemplateParameter();
            public object @__ClassifierTemplateParameter = new object();

            public class _Feature
            {
                public object @featuringClassifier = new object();

                public object @isStatic = new object();

            }

            public _Feature @Feature = new _Feature();
            public object @__Feature = new object();

            public class _Generalization
            {
                public object @general = new object();

                public object @generalizationSet = new object();

                public object @isSubstitutable = new object();

                public object @specific = new object();

            }

            public _Generalization @Generalization = new _Generalization();
            public object @__Generalization = new object();

            public class _GeneralizationSet
            {
                public object @generalization = new object();

                public object @isCovering = new object();

                public object @isDisjoint = new object();

                public object @powertype = new object();

            }

            public _GeneralizationSet @GeneralizationSet = new _GeneralizationSet();
            public object @__GeneralizationSet = new object();

            public class _InstanceSpecification
            {
                public object @classifier = new object();

                public object @slot = new object();

                public object @specification = new object();

            }

            public _InstanceSpecification @InstanceSpecification = new _InstanceSpecification();
            public object @__InstanceSpecification = new object();

            public class _InstanceValue
            {
                public object @instance = new object();

            }

            public _InstanceValue @InstanceValue = new _InstanceValue();
            public object @__InstanceValue = new object();

            public class _Operation
            {
                public object @bodyCondition = new object();

                public object @class = new object();

                public object @datatype = new object();

                public object @interface = new object();

                public object @isOrdered = new object();

                public object @isQuery = new object();

                public object @isUnique = new object();

                public object @lower = new object();

                public object @ownedParameter = new object();

                public object @postcondition = new object();

                public object @precondition = new object();

                public object @raisedException = new object();

                public object @redefinedOperation = new object();

                public object @templateParameter = new object();

                public object @type = new object();

                public object @upper = new object();

            }

            public _Operation @Operation = new _Operation();
            public object @__Operation = new object();

            public class _OperationTemplateParameter
            {
                public object @parameteredElement = new object();

            }

            public _OperationTemplateParameter @OperationTemplateParameter = new _OperationTemplateParameter();
            public object @__OperationTemplateParameter = new object();

            public class _Parameter
            {
                public object @default = new object();

                public object @defaultValue = new object();

                public object @direction = new object();

                public object @effect = new object();

                public object @isException = new object();

                public object @isStream = new object();

                public object @operation = new object();

                public object @parameterSet = new object();

            }

            public _Parameter @Parameter = new _Parameter();
            public object @__Parameter = new object();

            public class _ParameterSet
            {
                public object @condition = new object();

                public object @parameter = new object();

            }

            public _ParameterSet @ParameterSet = new _ParameterSet();
            public object @__ParameterSet = new object();

            public class _Property
            {
                public object @aggregation = new object();

                public object @association = new object();

                public object @associationEnd = new object();

                public object @class = new object();

                public object @datatype = new object();

                public object @defaultValue = new object();

                public object @interface = new object();

                public object @isComposite = new object();

                public object @isDerived = new object();

                public object @isDerivedUnion = new object();

                public object @isID = new object();

                public object @opposite = new object();

                public object @owningAssociation = new object();

                public object @qualifier = new object();

                public object @redefinedProperty = new object();

                public object @subsettedProperty = new object();

            }

            public _Property @Property = new _Property();
            public object @__Property = new object();

            public class _RedefinableElement
            {
                public object @isLeaf = new object();

                public object @redefinedElement = new object();

                public object @redefinitionContext = new object();

            }

            public _RedefinableElement @RedefinableElement = new _RedefinableElement();
            public object @__RedefinableElement = new object();

            public class _RedefinableTemplateSignature
            {
                public object @classifier = new object();

                public object @extendedSignature = new object();

                public object @inheritedParameter = new object();

            }

            public _RedefinableTemplateSignature @RedefinableTemplateSignature = new _RedefinableTemplateSignature();
            public object @__RedefinableTemplateSignature = new object();

            public class _Slot
            {
                public object @definingFeature = new object();

                public object @owningInstance = new object();

                public object @value = new object();

            }

            public _Slot @Slot = new _Slot();
            public object @__Slot = new object();

            public class _StructuralFeature
            {
                public object @isReadOnly = new object();

            }

            public _StructuralFeature @StructuralFeature = new _StructuralFeature();
            public object @__StructuralFeature = new object();

        }

        public _Classification Classification = new _Classification();

        public class _Actions
        {
            public class _ValueSpecificationAction
            {
                public object @result = new object();

                public object @value = new object();

            }

            public _ValueSpecificationAction @ValueSpecificationAction = new _ValueSpecificationAction();
            public object @__ValueSpecificationAction = new object();

            public class _VariableAction
            {
                public object @variable = new object();

            }

            public _VariableAction @VariableAction = new _VariableAction();
            public object @__VariableAction = new object();

            public class _WriteLinkAction
            {
            }

            public _WriteLinkAction @WriteLinkAction = new _WriteLinkAction();
            public object @__WriteLinkAction = new object();

            public class _WriteStructuralFeatureAction
            {
                public object @result = new object();

                public object @value = new object();

            }

            public _WriteStructuralFeatureAction @WriteStructuralFeatureAction = new _WriteStructuralFeatureAction();
            public object @__WriteStructuralFeatureAction = new object();

            public class _WriteVariableAction
            {
                public object @value = new object();

            }

            public _WriteVariableAction @WriteVariableAction = new _WriteVariableAction();
            public object @__WriteVariableAction = new object();

            public class _AcceptCallAction
            {
                public object @returnInformation = new object();

            }

            public _AcceptCallAction @AcceptCallAction = new _AcceptCallAction();
            public object @__AcceptCallAction = new object();

            public class _AcceptEventAction
            {
                public object @isUnmarshall = new object();

                public object @result = new object();

                public object @trigger = new object();

            }

            public _AcceptEventAction @AcceptEventAction = new _AcceptEventAction();
            public object @__AcceptEventAction = new object();

            public class _Action
            {
                public object @context = new object();

                public object @input = new object();

                public object @isLocallyReentrant = new object();

                public object @localPostcondition = new object();

                public object @localPrecondition = new object();

                public object @output = new object();

            }

            public _Action @Action = new _Action();
            public object @__Action = new object();

            public class _ActionInputPin
            {
                public object @fromAction = new object();

            }

            public _ActionInputPin @ActionInputPin = new _ActionInputPin();
            public object @__ActionInputPin = new object();

            public class _AddStructuralFeatureValueAction
            {
                public object @insertAt = new object();

                public object @isReplaceAll = new object();

            }

            public _AddStructuralFeatureValueAction @AddStructuralFeatureValueAction = new _AddStructuralFeatureValueAction();
            public object @__AddStructuralFeatureValueAction = new object();

            public class _AddVariableValueAction
            {
                public object @insertAt = new object();

                public object @isReplaceAll = new object();

            }

            public _AddVariableValueAction @AddVariableValueAction = new _AddVariableValueAction();
            public object @__AddVariableValueAction = new object();

            public class _BroadcastSignalAction
            {
                public object @signal = new object();

            }

            public _BroadcastSignalAction @BroadcastSignalAction = new _BroadcastSignalAction();
            public object @__BroadcastSignalAction = new object();

            public class _CallAction
            {
                public object @isSynchronous = new object();

                public object @result = new object();

            }

            public _CallAction @CallAction = new _CallAction();
            public object @__CallAction = new object();

            public class _CallBehaviorAction
            {
                public object @behavior = new object();

            }

            public _CallBehaviorAction @CallBehaviorAction = new _CallBehaviorAction();
            public object @__CallBehaviorAction = new object();

            public class _CallOperationAction
            {
                public object @operation = new object();

                public object @target = new object();

            }

            public _CallOperationAction @CallOperationAction = new _CallOperationAction();
            public object @__CallOperationAction = new object();

            public class _Clause
            {
                public object @body = new object();

                public object @bodyOutput = new object();

                public object @decider = new object();

                public object @predecessorClause = new object();

                public object @successorClause = new object();

                public object @test = new object();

            }

            public _Clause @Clause = new _Clause();
            public object @__Clause = new object();

            public class _ClearAssociationAction
            {
                public object @association = new object();

                public object @object = new object();

            }

            public _ClearAssociationAction @ClearAssociationAction = new _ClearAssociationAction();
            public object @__ClearAssociationAction = new object();

            public class _ClearStructuralFeatureAction
            {
                public object @result = new object();

            }

            public _ClearStructuralFeatureAction @ClearStructuralFeatureAction = new _ClearStructuralFeatureAction();
            public object @__ClearStructuralFeatureAction = new object();

            public class _ClearVariableAction
            {
            }

            public _ClearVariableAction @ClearVariableAction = new _ClearVariableAction();
            public object @__ClearVariableAction = new object();

            public class _ConditionalNode
            {
                public object @clause = new object();

                public object @isAssured = new object();

                public object @isDeterminate = new object();

                public object @result = new object();

            }

            public _ConditionalNode @ConditionalNode = new _ConditionalNode();
            public object @__ConditionalNode = new object();

            public class _CreateLinkAction
            {
                public object @endData = new object();

            }

            public _CreateLinkAction @CreateLinkAction = new _CreateLinkAction();
            public object @__CreateLinkAction = new object();

            public class _CreateLinkObjectAction
            {
                public object @result = new object();

            }

            public _CreateLinkObjectAction @CreateLinkObjectAction = new _CreateLinkObjectAction();
            public object @__CreateLinkObjectAction = new object();

            public class _CreateObjectAction
            {
                public object @classifier = new object();

                public object @result = new object();

            }

            public _CreateObjectAction @CreateObjectAction = new _CreateObjectAction();
            public object @__CreateObjectAction = new object();

            public class _DestroyLinkAction
            {
                public object @endData = new object();

            }

            public _DestroyLinkAction @DestroyLinkAction = new _DestroyLinkAction();
            public object @__DestroyLinkAction = new object();

            public class _DestroyObjectAction
            {
                public object @isDestroyLinks = new object();

                public object @isDestroyOwnedObjects = new object();

                public object @target = new object();

            }

            public _DestroyObjectAction @DestroyObjectAction = new _DestroyObjectAction();
            public object @__DestroyObjectAction = new object();

            public class _ExpansionNode
            {
                public object @regionAsInput = new object();

                public object @regionAsOutput = new object();

            }

            public _ExpansionNode @ExpansionNode = new _ExpansionNode();
            public object @__ExpansionNode = new object();

            public class _ExpansionRegion
            {
                public object @inputElement = new object();

                public object @mode = new object();

                public object @outputElement = new object();

            }

            public _ExpansionRegion @ExpansionRegion = new _ExpansionRegion();
            public object @__ExpansionRegion = new object();

            public class _InputPin
            {
            }

            public _InputPin @InputPin = new _InputPin();
            public object @__InputPin = new object();

            public class _InvocationAction
            {
                public object @argument = new object();

                public object @onPort = new object();

            }

            public _InvocationAction @InvocationAction = new _InvocationAction();
            public object @__InvocationAction = new object();

            public class _LinkAction
            {
                public object @endData = new object();

                public object @inputValue = new object();

            }

            public _LinkAction @LinkAction = new _LinkAction();
            public object @__LinkAction = new object();

            public class _LinkEndCreationData
            {
                public object @insertAt = new object();

                public object @isReplaceAll = new object();

            }

            public _LinkEndCreationData @LinkEndCreationData = new _LinkEndCreationData();
            public object @__LinkEndCreationData = new object();

            public class _LinkEndData
            {
                public object @end = new object();

                public object @qualifier = new object();

                public object @value = new object();

            }

            public _LinkEndData @LinkEndData = new _LinkEndData();
            public object @__LinkEndData = new object();

            public class _LinkEndDestructionData
            {
                public object @destroyAt = new object();

                public object @isDestroyDuplicates = new object();

            }

            public _LinkEndDestructionData @LinkEndDestructionData = new _LinkEndDestructionData();
            public object @__LinkEndDestructionData = new object();

            public class _LoopNode
            {
                public object @bodyOutput = new object();

                public object @bodyPart = new object();

                public object @decider = new object();

                public object @isTestedFirst = new object();

                public object @loopVariable = new object();

                public object @loopVariableInput = new object();

                public object @result = new object();

                public object @setupPart = new object();

                public object @test = new object();

            }

            public _LoopNode @LoopNode = new _LoopNode();
            public object @__LoopNode = new object();

            public class _OpaqueAction
            {
                public object @body = new object();

                public object @inputValue = new object();

                public object @language = new object();

                public object @outputValue = new object();

            }

            public _OpaqueAction @OpaqueAction = new _OpaqueAction();
            public object @__OpaqueAction = new object();

            public class _OutputPin
            {
            }

            public _OutputPin @OutputPin = new _OutputPin();
            public object @__OutputPin = new object();

            public class _Pin
            {
                public object @isControl = new object();

            }

            public _Pin @Pin = new _Pin();
            public object @__Pin = new object();

            public class _QualifierValue
            {
                public object @qualifier = new object();

                public object @value = new object();

            }

            public _QualifierValue @QualifierValue = new _QualifierValue();
            public object @__QualifierValue = new object();

            public class _RaiseExceptionAction
            {
                public object @exception = new object();

            }

            public _RaiseExceptionAction @RaiseExceptionAction = new _RaiseExceptionAction();
            public object @__RaiseExceptionAction = new object();

            public class _ReadExtentAction
            {
                public object @classifier = new object();

                public object @result = new object();

            }

            public _ReadExtentAction @ReadExtentAction = new _ReadExtentAction();
            public object @__ReadExtentAction = new object();

            public class _ReadIsClassifiedObjectAction
            {
                public object @classifier = new object();

                public object @isDirect = new object();

                public object @object = new object();

                public object @result = new object();

            }

            public _ReadIsClassifiedObjectAction @ReadIsClassifiedObjectAction = new _ReadIsClassifiedObjectAction();
            public object @__ReadIsClassifiedObjectAction = new object();

            public class _ReadLinkAction
            {
                public object @result = new object();

            }

            public _ReadLinkAction @ReadLinkAction = new _ReadLinkAction();
            public object @__ReadLinkAction = new object();

            public class _ReadLinkObjectEndAction
            {
                public object @end = new object();

                public object @object = new object();

                public object @result = new object();

            }

            public _ReadLinkObjectEndAction @ReadLinkObjectEndAction = new _ReadLinkObjectEndAction();
            public object @__ReadLinkObjectEndAction = new object();

            public class _ReadLinkObjectEndQualifierAction
            {
                public object @object = new object();

                public object @qualifier = new object();

                public object @result = new object();

            }

            public _ReadLinkObjectEndQualifierAction @ReadLinkObjectEndQualifierAction = new _ReadLinkObjectEndQualifierAction();
            public object @__ReadLinkObjectEndQualifierAction = new object();

            public class _ReadSelfAction
            {
                public object @result = new object();

            }

            public _ReadSelfAction @ReadSelfAction = new _ReadSelfAction();
            public object @__ReadSelfAction = new object();

            public class _ReadStructuralFeatureAction
            {
                public object @result = new object();

            }

            public _ReadStructuralFeatureAction @ReadStructuralFeatureAction = new _ReadStructuralFeatureAction();
            public object @__ReadStructuralFeatureAction = new object();

            public class _ReadVariableAction
            {
                public object @result = new object();

            }

            public _ReadVariableAction @ReadVariableAction = new _ReadVariableAction();
            public object @__ReadVariableAction = new object();

            public class _ReclassifyObjectAction
            {
                public object @isReplaceAll = new object();

                public object @newClassifier = new object();

                public object @object = new object();

                public object @oldClassifier = new object();

            }

            public _ReclassifyObjectAction @ReclassifyObjectAction = new _ReclassifyObjectAction();
            public object @__ReclassifyObjectAction = new object();

            public class _ReduceAction
            {
                public object @collection = new object();

                public object @isOrdered = new object();

                public object @reducer = new object();

                public object @result = new object();

            }

            public _ReduceAction @ReduceAction = new _ReduceAction();
            public object @__ReduceAction = new object();

            public class _RemoveStructuralFeatureValueAction
            {
                public object @isRemoveDuplicates = new object();

                public object @removeAt = new object();

            }

            public _RemoveStructuralFeatureValueAction @RemoveStructuralFeatureValueAction = new _RemoveStructuralFeatureValueAction();
            public object @__RemoveStructuralFeatureValueAction = new object();

            public class _RemoveVariableValueAction
            {
                public object @isRemoveDuplicates = new object();

                public object @removeAt = new object();

            }

            public _RemoveVariableValueAction @RemoveVariableValueAction = new _RemoveVariableValueAction();
            public object @__RemoveVariableValueAction = new object();

            public class _ReplyAction
            {
                public object @replyToCall = new object();

                public object @replyValue = new object();

                public object @returnInformation = new object();

            }

            public _ReplyAction @ReplyAction = new _ReplyAction();
            public object @__ReplyAction = new object();

            public class _SendObjectAction
            {
                public object @request = new object();

                public object @target = new object();

            }

            public _SendObjectAction @SendObjectAction = new _SendObjectAction();
            public object @__SendObjectAction = new object();

            public class _SendSignalAction
            {
                public object @signal = new object();

                public object @target = new object();

            }

            public _SendSignalAction @SendSignalAction = new _SendSignalAction();
            public object @__SendSignalAction = new object();

            public class _SequenceNode
            {
                public object @executableNode = new object();

            }

            public _SequenceNode @SequenceNode = new _SequenceNode();
            public object @__SequenceNode = new object();

            public class _StartClassifierBehaviorAction
            {
                public object @object = new object();

            }

            public _StartClassifierBehaviorAction @StartClassifierBehaviorAction = new _StartClassifierBehaviorAction();
            public object @__StartClassifierBehaviorAction = new object();

            public class _StartObjectBehaviorAction
            {
                public object @object = new object();

            }

            public _StartObjectBehaviorAction @StartObjectBehaviorAction = new _StartObjectBehaviorAction();
            public object @__StartObjectBehaviorAction = new object();

            public class _StructuralFeatureAction
            {
                public object @object = new object();

                public object @structuralFeature = new object();

            }

            public _StructuralFeatureAction @StructuralFeatureAction = new _StructuralFeatureAction();
            public object @__StructuralFeatureAction = new object();

            public class _StructuredActivityNode
            {
                public object @activity = new object();

                public object @edge = new object();

                public object @mustIsolate = new object();

                public object @node = new object();

                public object @structuredNodeInput = new object();

                public object @structuredNodeOutput = new object();

                public object @variable = new object();

            }

            public _StructuredActivityNode @StructuredActivityNode = new _StructuredActivityNode();
            public object @__StructuredActivityNode = new object();

            public class _TestIdentityAction
            {
                public object @first = new object();

                public object @result = new object();

                public object @second = new object();

            }

            public _TestIdentityAction @TestIdentityAction = new _TestIdentityAction();
            public object @__TestIdentityAction = new object();

            public class _UnmarshallAction
            {
                public object @object = new object();

                public object @result = new object();

                public object @unmarshallType = new object();

            }

            public _UnmarshallAction @UnmarshallAction = new _UnmarshallAction();
            public object @__UnmarshallAction = new object();

            public class _ValuePin
            {
                public object @value = new object();

            }

            public _ValuePin @ValuePin = new _ValuePin();
            public object @__ValuePin = new object();

        }

        public _Actions Actions = new _Actions();

        public static _UML TheOne = new _UML();

    }

}
