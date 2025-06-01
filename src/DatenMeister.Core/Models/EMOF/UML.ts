// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.0.0.0
export namespace _UML
{
        export namespace _Activities
        {
                export class _Activity
                {
                    static edge = "edge";
                    static group = "group";
                    static isReadOnly = "isReadOnly";
                    static isSingleExecution = "isSingleExecution";
                    static node = "node";
                    static partition = "partition";
                    static structuredNode = "structuredNode";
                    static variable = "variable";
                    static context = "context";
                    static isReentrant = "isReentrant";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static specification = "specification";
                    static redefinedBehavior = "redefinedBehavior";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __Activity_Uri = "dm:///_internal/model/mof#Activity";
                export class _ActivityEdge
                {
                    static activity = "activity";
                    static guard = "guard";
                    static inGroup = "inGroup";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static interrupts = "interrupts";
                    static redefinedEdge = "redefinedEdge";
                    static source = "source";
                    static target = "target";
                    static weight = "weight";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ActivityEdge_Uri = "dm:///_internal/model/mof#ActivityEdge";
                export class _ActivityFinalNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ActivityFinalNode_Uri = "dm:///_internal/model/mof#ActivityFinalNode";
                export class _ActivityGroup
                {
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ActivityGroup_Uri = "dm:///_internal/model/mof#ActivityGroup";
                export class _ActivityNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ActivityNode_Uri = "dm:///_internal/model/mof#ActivityNode";
                export class _ActivityParameterNode
                {
                    static parameter = "parameter";
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __ActivityParameterNode_Uri = "dm:///_internal/model/mof#ActivityParameterNode";
                export class _ActivityPartition
                {
                    static edge = "edge";
                    static isDimension = "isDimension";
                    static isExternal = "isExternal";
                    static node = "node";
                    static represents = "represents";
                    static subpartition = "subpartition";
                    static superPartition = "superPartition";
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ActivityPartition_Uri = "dm:///_internal/model/mof#ActivityPartition";
                export class _CentralBufferNode
                {
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __CentralBufferNode_Uri = "dm:///_internal/model/mof#CentralBufferNode";
                export class _ControlFlow
                {
                    static activity = "activity";
                    static guard = "guard";
                    static inGroup = "inGroup";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static interrupts = "interrupts";
                    static redefinedEdge = "redefinedEdge";
                    static source = "source";
                    static target = "target";
                    static weight = "weight";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ControlFlow_Uri = "dm:///_internal/model/mof#ControlFlow";
                export class _ControlNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ControlNode_Uri = "dm:///_internal/model/mof#ControlNode";
                export class _DataStoreNode
                {
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __DataStoreNode_Uri = "dm:///_internal/model/mof#DataStoreNode";
                export class _DecisionNode
                {
                    static decisionInput = "decisionInput";
                    static decisionInputFlow = "decisionInputFlow";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __DecisionNode_Uri = "dm:///_internal/model/mof#DecisionNode";
                export class _ExceptionHandler
                {
                    static exceptionInput = "exceptionInput";
                    static exceptionType = "exceptionType";
                    static handlerBody = "handlerBody";
                    static protectedNode = "protectedNode";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ExceptionHandler_Uri = "dm:///_internal/model/mof#ExceptionHandler";
                export class _ExecutableNode
                {
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ExecutableNode_Uri = "dm:///_internal/model/mof#ExecutableNode";
                export class _FinalNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __FinalNode_Uri = "dm:///_internal/model/mof#FinalNode";
                export class _FlowFinalNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __FlowFinalNode_Uri = "dm:///_internal/model/mof#FlowFinalNode";
                export class _ForkNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ForkNode_Uri = "dm:///_internal/model/mof#ForkNode";
                export class _InitialNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __InitialNode_Uri = "dm:///_internal/model/mof#InitialNode";
                export class _InterruptibleActivityRegion
                {
                    static interruptingEdge = "interruptingEdge";
                    static node = "node";
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __InterruptibleActivityRegion_Uri = "dm:///_internal/model/mof#InterruptibleActivityRegion";
                export class _JoinNode
                {
                    static isCombineDuplicate = "isCombineDuplicate";
                    static joinSpec = "joinSpec";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __JoinNode_Uri = "dm:///_internal/model/mof#JoinNode";
                export class _MergeNode
                {
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __MergeNode_Uri = "dm:///_internal/model/mof#MergeNode";
                export class _ObjectFlow
                {
                    static isMulticast = "isMulticast";
                    static isMultireceive = "isMultireceive";
                    static selection = "selection";
                    static transformation = "transformation";
                    static activity = "activity";
                    static guard = "guard";
                    static inGroup = "inGroup";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static interrupts = "interrupts";
                    static redefinedEdge = "redefinedEdge";
                    static source = "source";
                    static target = "target";
                    static weight = "weight";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ObjectFlow_Uri = "dm:///_internal/model/mof#ObjectFlow";
                export class _ObjectNode
                {
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __ObjectNode_Uri = "dm:///_internal/model/mof#ObjectNode";
                export class _Variable
                {
                    static activityScope = "activityScope";
                    static scope = "scope";
                    static end = "end";
                    static templateParameter = "templateParameter";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                }

                export const __Variable_Uri = "dm:///_internal/model/mof#Variable";
                export module _ObjectNodeOrderingKind
                {
                    export const unordered = "unordered";
                    export const ordered = "ordered";
                    export const LIFO = "LIFO";
                    export const FIFO = "FIFO";
                }

                export enum ___ObjectNodeOrderingKind
                {
                    unordered,
                    ordered,
                    LIFO,
                    FIFO
                }

        }

        export namespace _Values
        {
                export class _Duration
                {
                    static expr = "expr";
                    static observation = "observation";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __Duration_Uri = "dm:///_internal/model/mof#Duration";
                export class _DurationConstraint
                {
                    static firstEvent = "firstEvent";
                    static specification = "specification";
                    static constrainedElement = "constrainedElement";
                    static context = "context";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __DurationConstraint_Uri = "dm:///_internal/model/mof#DurationConstraint";
                export class _DurationInterval
                {
                    static max = "max";
                    static min = "min";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __DurationInterval_Uri = "dm:///_internal/model/mof#DurationInterval";
                export class _DurationObservation
                {
                    static event = "event";
                    static firstEvent = "firstEvent";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __DurationObservation_Uri = "dm:///_internal/model/mof#DurationObservation";
                export class _Expression
                {
                    static operand = "operand";
                    static symbol = "symbol";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __Expression_Uri = "dm:///_internal/model/mof#Expression";
                export class _Interval
                {
                    static max = "max";
                    static min = "min";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __Interval_Uri = "dm:///_internal/model/mof#Interval";
                export class _IntervalConstraint
                {
                    static specification = "specification";
                    static constrainedElement = "constrainedElement";
                    static context = "context";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __IntervalConstraint_Uri = "dm:///_internal/model/mof#IntervalConstraint";
                export class _LiteralBoolean
                {
                    static value = "value";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __LiteralBoolean_Uri = "dm:///_internal/model/mof#LiteralBoolean";
                export class _LiteralInteger
                {
                    static value = "value";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __LiteralInteger_Uri = "dm:///_internal/model/mof#LiteralInteger";
                export class _LiteralNull
                {
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __LiteralNull_Uri = "dm:///_internal/model/mof#LiteralNull";
                export class _LiteralReal
                {
                    static value = "value";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __LiteralReal_Uri = "dm:///_internal/model/mof#LiteralReal";
                export class _LiteralSpecification
                {
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __LiteralSpecification_Uri = "dm:///_internal/model/mof#LiteralSpecification";
                export class _LiteralString
                {
                    static value = "value";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __LiteralString_Uri = "dm:///_internal/model/mof#LiteralString";
                export class _LiteralUnlimitedNatural
                {
                    static value = "value";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __LiteralUnlimitedNatural_Uri = "dm:///_internal/model/mof#LiteralUnlimitedNatural";
                export class _Observation
                {
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Observation_Uri = "dm:///_internal/model/mof#Observation";
                export class _OpaqueExpression
                {
                    static behavior = "behavior";
                    static body = "body";
                    static language = "language";
                    static result = "result";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __OpaqueExpression_Uri = "dm:///_internal/model/mof#OpaqueExpression";
                export class _StringExpression
                {
                    static owningExpression = "owningExpression";
                    static subExpression = "subExpression";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static templateBinding = "templateBinding";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static operand = "operand";
                    static symbol = "symbol";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __StringExpression_Uri = "dm:///_internal/model/mof#StringExpression";
                export class _TimeConstraint
                {
                    static firstEvent = "firstEvent";
                    static specification = "specification";
                    static constrainedElement = "constrainedElement";
                    static context = "context";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __TimeConstraint_Uri = "dm:///_internal/model/mof#TimeConstraint";
                export class _TimeExpression
                {
                    static expr = "expr";
                    static observation = "observation";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __TimeExpression_Uri = "dm:///_internal/model/mof#TimeExpression";
                export class _TimeInterval
                {
                    static max = "max";
                    static min = "min";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __TimeInterval_Uri = "dm:///_internal/model/mof#TimeInterval";
                export class _TimeObservation
                {
                    static event = "event";
                    static firstEvent = "firstEvent";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __TimeObservation_Uri = "dm:///_internal/model/mof#TimeObservation";
                export class _ValueSpecification
                {
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __ValueSpecification_Uri = "dm:///_internal/model/mof#ValueSpecification";
        }

        export namespace _UseCases
        {
                export class _Actor
                {
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Actor_Uri = "dm:///_internal/model/mof#Actor";
                export class _Extend
                {
                    static condition = "condition";
                    static extendedCase = "extendedCase";
                    static extension = "extension";
                    static extensionLocation = "extensionLocation";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                }

                export const __Extend_Uri = "dm:///_internal/model/mof#Extend";
                export class _ExtensionPoint
                {
                    static useCase = "useCase";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ExtensionPoint_Uri = "dm:///_internal/model/mof#ExtensionPoint";
                export class _Include
                {
                    static addition = "addition";
                    static includingCase = "includingCase";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                }

                export const __Include_Uri = "dm:///_internal/model/mof#Include";
                export class _UseCase
                {
                    static extend = "extend";
                    static extensionPoint = "extensionPoint";
                    static include = "include";
                    static subject = "subject";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __UseCase_Uri = "dm:///_internal/model/mof#UseCase";
        }

        export namespace _StructuredClassifiers
        {
                export class _Association
                {
                    static endType = "endType";
                    static isDerived = "isDerived";
                    static memberEnd = "memberEnd";
                    static navigableOwnedEnd = "navigableOwnedEnd";
                    static ownedEnd = "ownedEnd";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Association_Uri = "dm:///_internal/model/mof#Association";
                export class _AssociationClass
                {
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                    static endType = "endType";
                    static isDerived = "isDerived";
                    static memberEnd = "memberEnd";
                    static navigableOwnedEnd = "navigableOwnedEnd";
                    static ownedEnd = "ownedEnd";
                    static relatedElement = "relatedElement";
                }

                export const __AssociationClass_Uri = "dm:///_internal/model/mof#AssociationClass";
                export class _Class
                {
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __Class_Uri = "dm:///_internal/model/mof#Class";
                export class _Collaboration
                {
                    static collaborationRole = "collaborationRole";
                    static ownedAttribute = "ownedAttribute";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                }

                export const __Collaboration_Uri = "dm:///_internal/model/mof#Collaboration";
                export class _CollaborationUse
                {
                    static roleBinding = "roleBinding";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CollaborationUse_Uri = "dm:///_internal/model/mof#CollaborationUse";
                export class _Component
                {
                    static isIndirectlyInstantiated = "isIndirectlyInstantiated";
                    static packagedElement = "packagedElement";
                    static provided = "provided";
                    static realization = "realization";
                    static required = "required";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __Component_Uri = "dm:///_internal/model/mof#Component";
                export class _ComponentRealization
                {
                    static abstraction = "abstraction";
                    static realizingClassifier = "realizingClassifier";
                    static mapping = "mapping";
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __ComponentRealization_Uri = "dm:///_internal/model/mof#ComponentRealization";
                export class _ConnectableElement
                {
                    static end = "end";
                    static templateParameter = "templateParameter";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                }

                export const __ConnectableElement_Uri = "dm:///_internal/model/mof#ConnectableElement";
                export class _ConnectableElementTemplateParameter
                {
                    static parameteredElement = "parameteredElement";
                    static _default_ = "default";
                    static ownedDefault = "ownedDefault";
                    static ownedParameteredElement = "ownedParameteredElement";
                    static signature = "signature";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ConnectableElementTemplateParameter_Uri = "dm:///_internal/model/mof#ConnectableElementTemplateParameter";
                export class _Connector
                {
                    static contract = "contract";
                    static end = "end";
                    static kind = "kind";
                    static redefinedConnector = "redefinedConnector";
                    static type = "type";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Connector_Uri = "dm:///_internal/model/mof#Connector";
                export class _ConnectorEnd
                {
                    static definingEnd = "definingEnd";
                    static partWithPort = "partWithPort";
                    static role = "role";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ConnectorEnd_Uri = "dm:///_internal/model/mof#ConnectorEnd";
                export class _EncapsulatedClassifier
                {
                    static ownedPort = "ownedPort";
                    static ownedAttribute = "ownedAttribute";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __EncapsulatedClassifier_Uri = "dm:///_internal/model/mof#EncapsulatedClassifier";
                export class _Port
                {
                    static isBehavior = "isBehavior";
                    static isConjugated = "isConjugated";
                    static isService = "isService";
                    static protocol = "protocol";
                    static provided = "provided";
                    static redefinedPort = "redefinedPort";
                    static required = "required";
                    static aggregation = "aggregation";
                    static association = "association";
                    static associationEnd = "associationEnd";
                    static _class_ = "class";
                    static datatype = "datatype";
                    static defaultValue = "defaultValue";
                    static _interface_ = "interface";
                    static isComposite = "isComposite";
                    static isDerived = "isDerived";
                    static isDerivedUnion = "isDerivedUnion";
                    static isID = "isID";
                    static opposite = "opposite";
                    static owningAssociation = "owningAssociation";
                    static qualifier = "qualifier";
                    static redefinedProperty = "redefinedProperty";
                    static subsettedProperty = "subsettedProperty";
                    static end = "end";
                    static templateParameter = "templateParameter";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                    static isReadOnly = "isReadOnly";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Port_Uri = "dm:///_internal/model/mof#Port";
                export class _StructuredClassifier
                {
                    static ownedAttribute = "ownedAttribute";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __StructuredClassifier_Uri = "dm:///_internal/model/mof#StructuredClassifier";
                export module _ConnectorKind
                {
                    export const assembly = "assembly";
                    export const delegation = "delegation";
                }

                export enum ___ConnectorKind
                {
                    assembly,
                    delegation
                }

        }

        export namespace _StateMachines
        {
                export class _ConnectionPointReference
                {
                    static entry = "entry";
                    static exit = "exit";
                    static state = "state";
                    static container = "container";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ConnectionPointReference_Uri = "dm:///_internal/model/mof#ConnectionPointReference";
                export class _FinalState
                {
                    static connection = "connection";
                    static connectionPoint = "connectionPoint";
                    static deferrableTrigger = "deferrableTrigger";
                    static doActivity = "doActivity";
                    static entry = "entry";
                    static exit = "exit";
                    static isComposite = "isComposite";
                    static isOrthogonal = "isOrthogonal";
                    static isSimple = "isSimple";
                    static isSubmachineState = "isSubmachineState";
                    static redefinedState = "redefinedState";
                    static redefinitionContext = "redefinitionContext";
                    static region = "region";
                    static stateInvariant = "stateInvariant";
                    static submachine = "submachine";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static container = "container";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                }

                export const __FinalState_Uri = "dm:///_internal/model/mof#FinalState";
                export class _ProtocolConformance
                {
                    static generalMachine = "generalMachine";
                    static specificMachine = "specificMachine";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ProtocolConformance_Uri = "dm:///_internal/model/mof#ProtocolConformance";
                export class _ProtocolStateMachine
                {
                    static conformance = "conformance";
                    static connectionPoint = "connectionPoint";
                    static extendedStateMachine = "extendedStateMachine";
                    static region = "region";
                    static submachineState = "submachineState";
                    static context = "context";
                    static isReentrant = "isReentrant";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static specification = "specification";
                    static redefinedBehavior = "redefinedBehavior";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __ProtocolStateMachine_Uri = "dm:///_internal/model/mof#ProtocolStateMachine";
                export class _ProtocolTransition
                {
                    static postCondition = "postCondition";
                    static preCondition = "preCondition";
                    static referred = "referred";
                    static container = "container";
                    static effect = "effect";
                    static guard = "guard";
                    static kind = "kind";
                    static redefinedTransition = "redefinedTransition";
                    static redefinitionContext = "redefinitionContext";
                    static source = "source";
                    static target = "target";
                    static trigger = "trigger";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                }

                export const __ProtocolTransition_Uri = "dm:///_internal/model/mof#ProtocolTransition";
                export class _Pseudostate
                {
                    static kind = "kind";
                    static state = "state";
                    static stateMachine = "stateMachine";
                    static container = "container";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Pseudostate_Uri = "dm:///_internal/model/mof#Pseudostate";
                export class _Region
                {
                    static extendedRegion = "extendedRegion";
                    static redefinitionContext = "redefinitionContext";
                    static state = "state";
                    static stateMachine = "stateMachine";
                    static subvertex = "subvertex";
                    static transition = "transition";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                }

                export const __Region_Uri = "dm:///_internal/model/mof#Region";
                export class _State
                {
                    static connection = "connection";
                    static connectionPoint = "connectionPoint";
                    static deferrableTrigger = "deferrableTrigger";
                    static doActivity = "doActivity";
                    static entry = "entry";
                    static exit = "exit";
                    static isComposite = "isComposite";
                    static isOrthogonal = "isOrthogonal";
                    static isSimple = "isSimple";
                    static isSubmachineState = "isSubmachineState";
                    static redefinedState = "redefinedState";
                    static redefinitionContext = "redefinitionContext";
                    static region = "region";
                    static stateInvariant = "stateInvariant";
                    static submachine = "submachine";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static container = "container";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                }

                export const __State_Uri = "dm:///_internal/model/mof#State";
                export class _StateMachine
                {
                    static connectionPoint = "connectionPoint";
                    static extendedStateMachine = "extendedStateMachine";
                    static region = "region";
                    static submachineState = "submachineState";
                    static context = "context";
                    static isReentrant = "isReentrant";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static specification = "specification";
                    static redefinedBehavior = "redefinedBehavior";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __StateMachine_Uri = "dm:///_internal/model/mof#StateMachine";
                export class _Transition
                {
                    static container = "container";
                    static effect = "effect";
                    static guard = "guard";
                    static kind = "kind";
                    static redefinedTransition = "redefinedTransition";
                    static redefinitionContext = "redefinitionContext";
                    static source = "source";
                    static target = "target";
                    static trigger = "trigger";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                }

                export const __Transition_Uri = "dm:///_internal/model/mof#Transition";
                export class _Vertex
                {
                    static container = "container";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Vertex_Uri = "dm:///_internal/model/mof#Vertex";
                export module _PseudostateKind
                {
                    export const initial = "initial";
                    export const deepHistory = "deepHistory";
                    export const shallowHistory = "shallowHistory";
                    export const join = "join";
                    export const fork = "fork";
                    export const junction = "junction";
                    export const choice = "choice";
                    export const entryPoint = "entryPoint";
                    export const exitPoint = "exitPoint";
                    export const terminate = "terminate";
                }

                export enum ___PseudostateKind
                {
                    initial,
                    deepHistory,
                    shallowHistory,
                    join,
                    fork,
                    junction,
                    choice,
                    entryPoint,
                    exitPoint,
                    terminate
                }

                export module _TransitionKind
                {
                    export const internal = "internal";
                    export const local = "local";
                    export const external = "external";
                }

                export enum ___TransitionKind
                {
                    internal,
                    local,
                    external
                }

        }

        export namespace _SimpleClassifiers
        {
                export class _BehavioredClassifier
                {
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __BehavioredClassifier_Uri = "dm:///_internal/model/mof#BehavioredClassifier";
                export class _DataType
                {
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __DataType_Uri = "dm:///_internal/model/mof#DataType";
                export class _Enumeration
                {
                    static ownedLiteral = "ownedLiteral";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Enumeration_Uri = "dm:///_internal/model/mof#Enumeration";
                export class _EnumerationLiteral
                {
                    static classifier = "classifier";
                    static enumeration = "enumeration";
                    static slot = "slot";
                    static specification = "specification";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __EnumerationLiteral_Uri = "dm:///_internal/model/mof#EnumerationLiteral";
                export class _Interface
                {
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static protocol = "protocol";
                    static redefinedInterface = "redefinedInterface";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Interface_Uri = "dm:///_internal/model/mof#Interface";
                export class _InterfaceRealization
                {
                    static contract = "contract";
                    static implementingClassifier = "implementingClassifier";
                    static mapping = "mapping";
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __InterfaceRealization_Uri = "dm:///_internal/model/mof#InterfaceRealization";
                export class _PrimitiveType
                {
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __PrimitiveType_Uri = "dm:///_internal/model/mof#PrimitiveType";
                export class _Reception
                {
                    static signal = "signal";
                    static concurrency = "concurrency";
                    static isAbstract = "isAbstract";
                    static method = "method";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static raisedException = "raisedException";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                }

                export const __Reception_Uri = "dm:///_internal/model/mof#Reception";
                export class _Signal
                {
                    static ownedAttribute = "ownedAttribute";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Signal_Uri = "dm:///_internal/model/mof#Signal";
        }

        export namespace _Packages
        {
                export class _Extension
                {
                    static isRequired = "isRequired";
                    static metaclass = "metaclass";
                    static ownedEnd = "ownedEnd";
                    static endType = "endType";
                    static isDerived = "isDerived";
                    static memberEnd = "memberEnd";
                    static navigableOwnedEnd = "navigableOwnedEnd";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Extension_Uri = "dm:///_internal/model/mof#Extension";
                export class _ExtensionEnd
                {
                    static lower = "lower";
                    static type = "type";
                    static aggregation = "aggregation";
                    static association = "association";
                    static associationEnd = "associationEnd";
                    static _class_ = "class";
                    static datatype = "datatype";
                    static defaultValue = "defaultValue";
                    static _interface_ = "interface";
                    static isComposite = "isComposite";
                    static isDerived = "isDerived";
                    static isDerivedUnion = "isDerivedUnion";
                    static isID = "isID";
                    static opposite = "opposite";
                    static owningAssociation = "owningAssociation";
                    static qualifier = "qualifier";
                    static redefinedProperty = "redefinedProperty";
                    static subsettedProperty = "subsettedProperty";
                    static end = "end";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                    static isReadOnly = "isReadOnly";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __ExtensionEnd_Uri = "dm:///_internal/model/mof#ExtensionEnd";
                export class _Image
                {
                    static content = "content";
                    static format = "format";
                    static location = "location";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Image_Uri = "dm:///_internal/model/mof#Image";
                export class _Model
                {
                    static viewpoint = "viewpoint";
                    static URI = "URI";
                    static nestedPackage = "nestedPackage";
                    static nestingPackage = "nestingPackage";
                    static ownedStereotype = "ownedStereotype";
                    static ownedType = "ownedType";
                    static packageMerge = "packageMerge";
                    static packagedElement = "packagedElement";
                    static profileApplication = "profileApplication";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static templateBinding = "templateBinding";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                }

                export const __Model_Uri = "dm:///_internal/model/mof#Model";
                export class _Package
                {
                    static URI = "URI";
                    static nestedPackage = "nestedPackage";
                    static nestingPackage = "nestingPackage";
                    static ownedStereotype = "ownedStereotype";
                    static ownedType = "ownedType";
                    static packageMerge = "packageMerge";
                    static packagedElement = "packagedElement";
                    static profileApplication = "profileApplication";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static templateBinding = "templateBinding";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                }

                export const __Package_Uri = "dm:///_internal/model/mof#Package";
                export class _PackageMerge
                {
                    static mergedPackage = "mergedPackage";
                    static receivingPackage = "receivingPackage";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __PackageMerge_Uri = "dm:///_internal/model/mof#PackageMerge";
                export class _Profile
                {
                    static metaclassReference = "metaclassReference";
                    static metamodelReference = "metamodelReference";
                    static URI = "URI";
                    static nestedPackage = "nestedPackage";
                    static nestingPackage = "nestingPackage";
                    static ownedStereotype = "ownedStereotype";
                    static ownedType = "ownedType";
                    static packageMerge = "packageMerge";
                    static packagedElement = "packagedElement";
                    static profileApplication = "profileApplication";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static templateBinding = "templateBinding";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                }

                export const __Profile_Uri = "dm:///_internal/model/mof#Profile";
                export class _ProfileApplication
                {
                    static appliedProfile = "appliedProfile";
                    static applyingPackage = "applyingPackage";
                    static isStrict = "isStrict";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ProfileApplication_Uri = "dm:///_internal/model/mof#ProfileApplication";
                export class _Stereotype
                {
                    static icon = "icon";
                    static profile = "profile";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __Stereotype_Uri = "dm:///_internal/model/mof#Stereotype";
        }

        export namespace _Interactions
        {
                export class _ActionExecutionSpecification
                {
                    static action = "action";
                    static finish = "finish";
                    static start = "start";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ActionExecutionSpecification_Uri = "dm:///_internal/model/mof#ActionExecutionSpecification";
                export class _BehaviorExecutionSpecification
                {
                    static behavior = "behavior";
                    static finish = "finish";
                    static start = "start";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __BehaviorExecutionSpecification_Uri = "dm:///_internal/model/mof#BehaviorExecutionSpecification";
                export class _CombinedFragment
                {
                    static cfragmentGate = "cfragmentGate";
                    static interactionOperator = "interactionOperator";
                    static operand = "operand";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CombinedFragment_Uri = "dm:///_internal/model/mof#CombinedFragment";
                export class _ConsiderIgnoreFragment
                {
                    static message = "message";
                    static cfragmentGate = "cfragmentGate";
                    static interactionOperator = "interactionOperator";
                    static operand = "operand";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ConsiderIgnoreFragment_Uri = "dm:///_internal/model/mof#ConsiderIgnoreFragment";
                export class _Continuation
                {
                    static setting = "setting";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Continuation_Uri = "dm:///_internal/model/mof#Continuation";
                export class _DestructionOccurrenceSpecification
                {
                    static message = "message";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static covered = "covered";
                    static toAfter = "toAfter";
                    static toBefore = "toBefore";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                }

                export const __DestructionOccurrenceSpecification_Uri = "dm:///_internal/model/mof#DestructionOccurrenceSpecification";
                export class _ExecutionOccurrenceSpecification
                {
                    static execution = "execution";
                    static covered = "covered";
                    static toAfter = "toAfter";
                    static toBefore = "toBefore";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ExecutionOccurrenceSpecification_Uri = "dm:///_internal/model/mof#ExecutionOccurrenceSpecification";
                export class _ExecutionSpecification
                {
                    static finish = "finish";
                    static start = "start";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ExecutionSpecification_Uri = "dm:///_internal/model/mof#ExecutionSpecification";
                export class _Gate
                {
                    static message = "message";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Gate_Uri = "dm:///_internal/model/mof#Gate";
                export class _GeneralOrdering
                {
                    static after = "after";
                    static before = "before";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __GeneralOrdering_Uri = "dm:///_internal/model/mof#GeneralOrdering";
                export class _Interaction
                {
                    static action = "action";
                    static formalGate = "formalGate";
                    static fragment = "fragment";
                    static lifeline = "lifeline";
                    static message = "message";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static context = "context";
                    static isReentrant = "isReentrant";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static specification = "specification";
                    static redefinedBehavior = "redefinedBehavior";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __Interaction_Uri = "dm:///_internal/model/mof#Interaction";
                export class _InteractionConstraint
                {
                    static maxint = "maxint";
                    static minint = "minint";
                    static constrainedElement = "constrainedElement";
                    static context = "context";
                    static specification = "specification";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __InteractionConstraint_Uri = "dm:///_internal/model/mof#InteractionConstraint";
                export class _InteractionFragment
                {
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __InteractionFragment_Uri = "dm:///_internal/model/mof#InteractionFragment";
                export class _InteractionOperand
                {
                    static fragment = "fragment";
                    static guard = "guard";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                }

                export const __InteractionOperand_Uri = "dm:///_internal/model/mof#InteractionOperand";
                export class _InteractionUse
                {
                    static actualGate = "actualGate";
                    static argument = "argument";
                    static refersTo = "refersTo";
                    static returnValue = "returnValue";
                    static returnValueRecipient = "returnValueRecipient";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __InteractionUse_Uri = "dm:///_internal/model/mof#InteractionUse";
                export class _Lifeline
                {
                    static coveredBy = "coveredBy";
                    static decomposedAs = "decomposedAs";
                    static interaction = "interaction";
                    static represents = "represents";
                    static selector = "selector";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Lifeline_Uri = "dm:///_internal/model/mof#Lifeline";
                export class _Message
                {
                    static argument = "argument";
                    static connector = "connector";
                    static interaction = "interaction";
                    static messageKind = "messageKind";
                    static messageSort = "messageSort";
                    static receiveEvent = "receiveEvent";
                    static sendEvent = "sendEvent";
                    static signature = "signature";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Message_Uri = "dm:///_internal/model/mof#Message";
                export class _MessageEnd
                {
                    static message = "message";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __MessageEnd_Uri = "dm:///_internal/model/mof#MessageEnd";
                export class _MessageOccurrenceSpecification
                {
                    static message = "message";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static covered = "covered";
                    static toAfter = "toAfter";
                    static toBefore = "toBefore";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                }

                export const __MessageOccurrenceSpecification_Uri = "dm:///_internal/model/mof#MessageOccurrenceSpecification";
                export class _OccurrenceSpecification
                {
                    static covered = "covered";
                    static toAfter = "toAfter";
                    static toBefore = "toBefore";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __OccurrenceSpecification_Uri = "dm:///_internal/model/mof#OccurrenceSpecification";
                export class _PartDecomposition
                {
                    static actualGate = "actualGate";
                    static argument = "argument";
                    static refersTo = "refersTo";
                    static returnValue = "returnValue";
                    static returnValueRecipient = "returnValueRecipient";
                    static covered = "covered";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __PartDecomposition_Uri = "dm:///_internal/model/mof#PartDecomposition";
                export class _StateInvariant
                {
                    static covered = "covered";
                    static invariant = "invariant";
                    static enclosingInteraction = "enclosingInteraction";
                    static enclosingOperand = "enclosingOperand";
                    static generalOrdering = "generalOrdering";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __StateInvariant_Uri = "dm:///_internal/model/mof#StateInvariant";
                export module _InteractionOperatorKind
                {
                    export const seq = "seq";
                    export const alt = "alt";
                    export const opt = "opt";
                    export const _break_ = "break";
                    export const par = "par";
                    export const strict = "strict";
                    export const loop = "loop";
                    export const critical = "critical";
                    export const neg = "neg";
                    export const assert = "assert";
                    export const ignore = "ignore";
                    export const consider = "consider";
                }

                export enum ___InteractionOperatorKind
                {
                    seq,
                    alt,
                    opt,
                    _break_,
                    par,
                    strict,
                    loop,
                    critical,
                    neg,
                    assert,
                    ignore,
                    consider
                }

                export module _MessageKind
                {
                    export const complete = "complete";
                    export const lost = "lost";
                    export const found = "found";
                    export const unknown = "unknown";
                }

                export enum ___MessageKind
                {
                    complete,
                    lost,
                    found,
                    unknown
                }

                export module _MessageSort
                {
                    export const synchCall = "synchCall";
                    export const asynchCall = "asynchCall";
                    export const asynchSignal = "asynchSignal";
                    export const createMessage = "createMessage";
                    export const deleteMessage = "deleteMessage";
                    export const reply = "reply";
                }

                export enum ___MessageSort
                {
                    synchCall,
                    asynchCall,
                    asynchSignal,
                    createMessage,
                    deleteMessage,
                    reply
                }

        }

        export namespace _InformationFlows
        {
                export class _InformationFlow
                {
                    static conveyed = "conveyed";
                    static informationSource = "informationSource";
                    static informationTarget = "informationTarget";
                    static realization = "realization";
                    static realizingActivityEdge = "realizingActivityEdge";
                    static realizingConnector = "realizingConnector";
                    static realizingMessage = "realizingMessage";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __InformationFlow_Uri = "dm:///_internal/model/mof#InformationFlow";
                export class _InformationItem
                {
                    static represented = "represented";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __InformationItem_Uri = "dm:///_internal/model/mof#InformationItem";
        }

        export namespace _Deployments
        {
                export class _Artifact
                {
                    static fileName = "fileName";
                    static manifestation = "manifestation";
                    static nestedArtifact = "nestedArtifact";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Artifact_Uri = "dm:///_internal/model/mof#Artifact";
                export class _CommunicationPath
                {
                    static endType = "endType";
                    static isDerived = "isDerived";
                    static memberEnd = "memberEnd";
                    static navigableOwnedEnd = "navigableOwnedEnd";
                    static ownedEnd = "ownedEnd";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __CommunicationPath_Uri = "dm:///_internal/model/mof#CommunicationPath";
                export class _DeployedArtifact
                {
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __DeployedArtifact_Uri = "dm:///_internal/model/mof#DeployedArtifact";
                export class _Deployment
                {
                    static configuration = "configuration";
                    static deployedArtifact = "deployedArtifact";
                    static location = "location";
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Deployment_Uri = "dm:///_internal/model/mof#Deployment";
                export class _DeploymentSpecification
                {
                    static deployment = "deployment";
                    static deploymentLocation = "deploymentLocation";
                    static executionLocation = "executionLocation";
                    static fileName = "fileName";
                    static manifestation = "manifestation";
                    static nestedArtifact = "nestedArtifact";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __DeploymentSpecification_Uri = "dm:///_internal/model/mof#DeploymentSpecification";
                export class _DeploymentTarget
                {
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __DeploymentTarget_Uri = "dm:///_internal/model/mof#DeploymentTarget";
                export class _Device
                {
                    static nestedNode = "nestedNode";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                }

                export const __Device_Uri = "dm:///_internal/model/mof#Device";
                export class _ExecutionEnvironment
                {
                    static nestedNode = "nestedNode";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                }

                export const __ExecutionEnvironment_Uri = "dm:///_internal/model/mof#ExecutionEnvironment";
                export class _Manifestation
                {
                    static utilizedElement = "utilizedElement";
                    static mapping = "mapping";
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Manifestation_Uri = "dm:///_internal/model/mof#Manifestation";
                export class _Node
                {
                    static nestedNode = "nestedNode";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                }

                export const __Node_Uri = "dm:///_internal/model/mof#Node";
        }

        export namespace _CommonStructure
        {
                export class _Abstraction
                {
                    static mapping = "mapping";
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Abstraction_Uri = "dm:///_internal/model/mof#Abstraction";
                export class _Comment
                {
                    static annotatedElement = "annotatedElement";
                    static body = "body";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Comment_Uri = "dm:///_internal/model/mof#Comment";
                export class _Constraint
                {
                    static constrainedElement = "constrainedElement";
                    static context = "context";
                    static specification = "specification";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Constraint_Uri = "dm:///_internal/model/mof#Constraint";
                export class _Dependency
                {
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Dependency_Uri = "dm:///_internal/model/mof#Dependency";
                export class _DirectedRelationship
                {
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __DirectedRelationship_Uri = "dm:///_internal/model/mof#DirectedRelationship";
                export class _Element
                {
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Element_Uri = "dm:///_internal/model/mof#Element";
                export class _ElementImport
                {
                    static alias = "alias";
                    static importedElement = "importedElement";
                    static importingNamespace = "importingNamespace";
                    static visibility = "visibility";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ElementImport_Uri = "dm:///_internal/model/mof#ElementImport";
                export class _MultiplicityElement
                {
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __MultiplicityElement_Uri = "dm:///_internal/model/mof#MultiplicityElement";
                export class _NamedElement
                {
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __NamedElement_Uri = "dm:///_internal/model/mof#NamedElement";
                export class _Namespace
                {
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Namespace_Uri = "dm:///_internal/model/mof#Namespace";
                export class _PackageableElement
                {
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __PackageableElement_Uri = "dm:///_internal/model/mof#PackageableElement";
                export class _PackageImport
                {
                    static importedPackage = "importedPackage";
                    static importingNamespace = "importingNamespace";
                    static visibility = "visibility";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __PackageImport_Uri = "dm:///_internal/model/mof#PackageImport";
                export class _ParameterableElement
                {
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ParameterableElement_Uri = "dm:///_internal/model/mof#ParameterableElement";
                export class _Realization
                {
                    static mapping = "mapping";
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Realization_Uri = "dm:///_internal/model/mof#Realization";
                export class _Relationship
                {
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Relationship_Uri = "dm:///_internal/model/mof#Relationship";
                export class _TemplateableElement
                {
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static templateBinding = "templateBinding";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __TemplateableElement_Uri = "dm:///_internal/model/mof#TemplateableElement";
                export class _TemplateBinding
                {
                    static boundElement = "boundElement";
                    static parameterSubstitution = "parameterSubstitution";
                    static signature = "signature";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __TemplateBinding_Uri = "dm:///_internal/model/mof#TemplateBinding";
                export class _TemplateParameter
                {
                    static _default_ = "default";
                    static ownedDefault = "ownedDefault";
                    static ownedParameteredElement = "ownedParameteredElement";
                    static parameteredElement = "parameteredElement";
                    static signature = "signature";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __TemplateParameter_Uri = "dm:///_internal/model/mof#TemplateParameter";
                export class _TemplateParameterSubstitution
                {
                    static actual = "actual";
                    static formal = "formal";
                    static ownedActual = "ownedActual";
                    static templateBinding = "templateBinding";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __TemplateParameterSubstitution_Uri = "dm:///_internal/model/mof#TemplateParameterSubstitution";
                export class _TemplateSignature
                {
                    static ownedParameter = "ownedParameter";
                    static parameter = "parameter";
                    static template = "template";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __TemplateSignature_Uri = "dm:///_internal/model/mof#TemplateSignature";
                export class _Type
                {
                    static _package_ = "package";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Type_Uri = "dm:///_internal/model/mof#Type";
                export class _TypedElement
                {
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __TypedElement_Uri = "dm:///_internal/model/mof#TypedElement";
                export class _Usage
                {
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Usage_Uri = "dm:///_internal/model/mof#Usage";
                export module _VisibilityKind
                {
                    export const _public_ = "public";
                    export const _private_ = "private";
                    export const _protected_ = "protected";
                    export const _package_ = "package";
                }

                export enum ___VisibilityKind
                {
                    _public_,
                    _private_,
                    _protected_,
                    _package_
                }

        }

        export namespace _CommonBehavior
        {
                export class _AnyReceiveEvent
                {
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __AnyReceiveEvent_Uri = "dm:///_internal/model/mof#AnyReceiveEvent";
                export class _Behavior
                {
                    static context = "context";
                    static isReentrant = "isReentrant";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static specification = "specification";
                    static redefinedBehavior = "redefinedBehavior";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __Behavior_Uri = "dm:///_internal/model/mof#Behavior";
                export class _CallEvent
                {
                    static operation = "operation";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __CallEvent_Uri = "dm:///_internal/model/mof#CallEvent";
                export class _ChangeEvent
                {
                    static changeExpression = "changeExpression";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __ChangeEvent_Uri = "dm:///_internal/model/mof#ChangeEvent";
                export class _Event
                {
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Event_Uri = "dm:///_internal/model/mof#Event";
                export class _FunctionBehavior
                {
                    static body = "body";
                    static language = "language";
                    static context = "context";
                    static isReentrant = "isReentrant";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static specification = "specification";
                    static redefinedBehavior = "redefinedBehavior";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __FunctionBehavior_Uri = "dm:///_internal/model/mof#FunctionBehavior";
                export class _MessageEvent
                {
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __MessageEvent_Uri = "dm:///_internal/model/mof#MessageEvent";
                export class _OpaqueBehavior
                {
                    static body = "body";
                    static language = "language";
                    static context = "context";
                    static isReentrant = "isReentrant";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static specification = "specification";
                    static redefinedBehavior = "redefinedBehavior";
                    static extension = "extension";
                    static isAbstract = "isAbstract";
                    static isActive = "isActive";
                    static nestedClassifier = "nestedClassifier";
                    static ownedAttribute = "ownedAttribute";
                    static ownedOperation = "ownedOperation";
                    static ownedReception = "ownedReception";
                    static superClass = "superClass";
                    static classifierBehavior = "classifierBehavior";
                    static interfaceRealization = "interfaceRealization";
                    static ownedBehavior = "ownedBehavior";
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static ownedPort = "ownedPort";
                    static ownedConnector = "ownedConnector";
                    static part = "part";
                    static role = "role";
                }

                export const __OpaqueBehavior_Uri = "dm:///_internal/model/mof#OpaqueBehavior";
                export class _SignalEvent
                {
                    static signal = "signal";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __SignalEvent_Uri = "dm:///_internal/model/mof#SignalEvent";
                export class _TimeEvent
                {
                    static isRelative = "isRelative";
                    static when = "when";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __TimeEvent_Uri = "dm:///_internal/model/mof#TimeEvent";
                export class _Trigger
                {
                    static event = "event";
                    static port = "port";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Trigger_Uri = "dm:///_internal/model/mof#Trigger";
        }

        export namespace _Classification
        {
                export class _Substitution
                {
                    static contract = "contract";
                    static substitutingClassifier = "substitutingClassifier";
                    static mapping = "mapping";
                    static client = "client";
                    static supplier = "supplier";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __Substitution_Uri = "dm:///_internal/model/mof#Substitution";
                export class _BehavioralFeature
                {
                    static concurrency = "concurrency";
                    static isAbstract = "isAbstract";
                    static method = "method";
                    static ownedParameter = "ownedParameter";
                    static ownedParameterSet = "ownedParameterSet";
                    static raisedException = "raisedException";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                }

                export const __BehavioralFeature_Uri = "dm:///_internal/model/mof#BehavioralFeature";
                export class _Classifier
                {
                    static attribute = "attribute";
                    static collaborationUse = "collaborationUse";
                    static feature = "feature";
                    static general = "general";
                    static generalization = "generalization";
                    static inheritedMember = "inheritedMember";
                    static isAbstract = "isAbstract";
                    static isFinalSpecialization = "isFinalSpecialization";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static ownedUseCase = "ownedUseCase";
                    static powertypeExtent = "powertypeExtent";
                    static redefinedClassifier = "redefinedClassifier";
                    static representation = "representation";
                    static substitution = "substitution";
                    static templateParameter = "templateParameter";
                    static useCase = "useCase";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static _package_ = "package";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateBinding = "templateBinding";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Classifier_Uri = "dm:///_internal/model/mof#Classifier";
                export class _ClassifierTemplateParameter
                {
                    static allowSubstitutable = "allowSubstitutable";
                    static constrainingClassifier = "constrainingClassifier";
                    static parameteredElement = "parameteredElement";
                    static _default_ = "default";
                    static ownedDefault = "ownedDefault";
                    static ownedParameteredElement = "ownedParameteredElement";
                    static signature = "signature";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ClassifierTemplateParameter_Uri = "dm:///_internal/model/mof#ClassifierTemplateParameter";
                export class _Feature
                {
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Feature_Uri = "dm:///_internal/model/mof#Feature";
                export class _Generalization
                {
                    static general = "general";
                    static generalizationSet = "generalizationSet";
                    static isSubstitutable = "isSubstitutable";
                    static specific = "specific";
                    static source = "source";
                    static target = "target";
                    static relatedElement = "relatedElement";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Generalization_Uri = "dm:///_internal/model/mof#Generalization";
                export class _GeneralizationSet
                {
                    static generalization = "generalization";
                    static isCovering = "isCovering";
                    static isDisjoint = "isDisjoint";
                    static powertype = "powertype";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                }

                export const __GeneralizationSet_Uri = "dm:///_internal/model/mof#GeneralizationSet";
                export class _InstanceSpecification
                {
                    static classifier = "classifier";
                    static slot = "slot";
                    static specification = "specification";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __InstanceSpecification_Uri = "dm:///_internal/model/mof#InstanceSpecification";
                export class _InstanceValue
                {
                    static instance = "instance";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static templateParameter = "templateParameter";
                }

                export const __InstanceValue_Uri = "dm:///_internal/model/mof#InstanceValue";
                export class _Operation
                {
                    static bodyCondition = "bodyCondition";
                    static _class_ = "class";
                    static datatype = "datatype";
                    static _interface_ = "interface";
                    static isOrdered = "isOrdered";
                    static isQuery = "isQuery";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static ownedParameter = "ownedParameter";
                    static postcondition = "postcondition";
                    static precondition = "precondition";
                    static raisedException = "raisedException";
                    static redefinedOperation = "redefinedOperation";
                    static templateParameter = "templateParameter";
                    static type = "type";
                    static upper = "upper";
                    static ownedTemplateSignature = "ownedTemplateSignature";
                    static templateBinding = "templateBinding";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static concurrency = "concurrency";
                    static isAbstract = "isAbstract";
                    static method = "method";
                    static ownedParameterSet = "ownedParameterSet";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                }

                export const __Operation_Uri = "dm:///_internal/model/mof#Operation";
                export class _OperationTemplateParameter
                {
                    static parameteredElement = "parameteredElement";
                    static _default_ = "default";
                    static ownedDefault = "ownedDefault";
                    static ownedParameteredElement = "ownedParameteredElement";
                    static signature = "signature";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __OperationTemplateParameter_Uri = "dm:///_internal/model/mof#OperationTemplateParameter";
                export class _Parameter
                {
                    static _default_ = "default";
                    static defaultValue = "defaultValue";
                    static direction = "direction";
                    static effect = "effect";
                    static isException = "isException";
                    static isStream = "isStream";
                    static operation = "operation";
                    static parameterSet = "parameterSet";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static end = "end";
                    static templateParameter = "templateParameter";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static owningTemplateParameter = "owningTemplateParameter";
                }

                export const __Parameter_Uri = "dm:///_internal/model/mof#Parameter";
                export class _ParameterSet
                {
                    static condition = "condition";
                    static parameter = "parameter";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ParameterSet_Uri = "dm:///_internal/model/mof#ParameterSet";
                export class _Property
                {
                    static aggregation = "aggregation";
                    static association = "association";
                    static associationEnd = "associationEnd";
                    static _class_ = "class";
                    static datatype = "datatype";
                    static defaultValue = "defaultValue";
                    static _interface_ = "interface";
                    static isComposite = "isComposite";
                    static isDerived = "isDerived";
                    static isDerivedUnion = "isDerivedUnion";
                    static isID = "isID";
                    static opposite = "opposite";
                    static owningAssociation = "owningAssociation";
                    static qualifier = "qualifier";
                    static redefinedProperty = "redefinedProperty";
                    static subsettedProperty = "subsettedProperty";
                    static end = "end";
                    static templateParameter = "templateParameter";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static owningTemplateParameter = "owningTemplateParameter";
                    static deployedElement = "deployedElement";
                    static deployment = "deployment";
                    static isReadOnly = "isReadOnly";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __Property_Uri = "dm:///_internal/model/mof#Property";
                export class _RedefinableElement
                {
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __RedefinableElement_Uri = "dm:///_internal/model/mof#RedefinableElement";
                export class _RedefinableTemplateSignature
                {
                    static classifier = "classifier";
                    static extendedSignature = "extendedSignature";
                    static inheritedParameter = "inheritedParameter";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static ownedParameter = "ownedParameter";
                    static parameter = "parameter";
                    static template = "template";
                }

                export const __RedefinableTemplateSignature_Uri = "dm:///_internal/model/mof#RedefinableTemplateSignature";
                export class _Slot
                {
                    static definingFeature = "definingFeature";
                    static owningInstance = "owningInstance";
                    static value = "value";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Slot_Uri = "dm:///_internal/model/mof#Slot";
                export class _StructuralFeature
                {
                    static isReadOnly = "isReadOnly";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static featuringClassifier = "featuringClassifier";
                    static isStatic = "isStatic";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __StructuralFeature_Uri = "dm:///_internal/model/mof#StructuralFeature";
                export module _AggregationKind
                {
                    export const none = "none";
                    export const shared = "shared";
                    export const composite = "composite";
                }

                export enum ___AggregationKind
                {
                    none,
                    shared,
                    composite
                }

                export module _CallConcurrencyKind
                {
                    export const sequential = "sequential";
                    export const guarded = "guarded";
                    export const concurrent = "concurrent";
                }

                export enum ___CallConcurrencyKind
                {
                    sequential,
                    guarded,
                    concurrent
                }

                export module _ParameterDirectionKind
                {
                    export const _in_ = "in";
                    export const inout = "inout";
                    export const out = "out";
                    export const _return_ = "return";
                }

                export enum ___ParameterDirectionKind
                {
                    _in_,
                    inout,
                    out,
                    _return_
                }

                export module _ParameterEffectKind
                {
                    export const create = "create";
                    export const read = "read";
                    export const update = "update";
                    export const _delete_ = "delete";
                }

                export enum ___ParameterEffectKind
                {
                    create,
                    read,
                    update,
                    _delete_
                }

        }

        export namespace _Actions
        {
                export class _ValueSpecificationAction
                {
                    static result = "result";
                    static value = "value";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ValueSpecificationAction_Uri = "dm:///_internal/model/mof#ValueSpecificationAction";
                export class _VariableAction
                {
                    static variable = "variable";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __VariableAction_Uri = "dm:///_internal/model/mof#VariableAction";
                export class _WriteLinkAction
                {
                    static endData = "endData";
                    static inputValue = "inputValue";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __WriteLinkAction_Uri = "dm:///_internal/model/mof#WriteLinkAction";
                export class _WriteStructuralFeatureAction
                {
                    static result = "result";
                    static value = "value";
                    static object = "object";
                    static structuralFeature = "structuralFeature";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __WriteStructuralFeatureAction_Uri = "dm:///_internal/model/mof#WriteStructuralFeatureAction";
                export class _WriteVariableAction
                {
                    static value = "value";
                    static variable = "variable";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __WriteVariableAction_Uri = "dm:///_internal/model/mof#WriteVariableAction";
                export module _ExpansionKind
                {
                    export const parallel = "parallel";
                    export const iterative = "iterative";
                    export const stream = "stream";
                }

                export enum ___ExpansionKind
                {
                    parallel,
                    iterative,
                    stream
                }

                export class _AcceptCallAction
                {
                    static returnInformation = "returnInformation";
                    static isUnmarshall = "isUnmarshall";
                    static result = "result";
                    static trigger = "trigger";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __AcceptCallAction_Uri = "dm:///_internal/model/mof#AcceptCallAction";
                export class _AcceptEventAction
                {
                    static isUnmarshall = "isUnmarshall";
                    static result = "result";
                    static trigger = "trigger";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __AcceptEventAction_Uri = "dm:///_internal/model/mof#AcceptEventAction";
                export class _Action
                {
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Action_Uri = "dm:///_internal/model/mof#Action";
                export class _ActionInputPin
                {
                    static fromAction = "fromAction";
                    static isControl = "isControl";
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                }

                export const __ActionInputPin_Uri = "dm:///_internal/model/mof#ActionInputPin";
                export class _AddStructuralFeatureValueAction
                {
                    static insertAt = "insertAt";
                    static isReplaceAll = "isReplaceAll";
                    static result = "result";
                    static value = "value";
                    static object = "object";
                    static structuralFeature = "structuralFeature";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __AddStructuralFeatureValueAction_Uri = "dm:///_internal/model/mof#AddStructuralFeatureValueAction";
                export class _AddVariableValueAction
                {
                    static insertAt = "insertAt";
                    static isReplaceAll = "isReplaceAll";
                    static value = "value";
                    static variable = "variable";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __AddVariableValueAction_Uri = "dm:///_internal/model/mof#AddVariableValueAction";
                export class _BroadcastSignalAction
                {
                    static signal = "signal";
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __BroadcastSignalAction_Uri = "dm:///_internal/model/mof#BroadcastSignalAction";
                export class _CallAction
                {
                    static isSynchronous = "isSynchronous";
                    static result = "result";
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CallAction_Uri = "dm:///_internal/model/mof#CallAction";
                export class _CallBehaviorAction
                {
                    static behavior = "behavior";
                    static isSynchronous = "isSynchronous";
                    static result = "result";
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CallBehaviorAction_Uri = "dm:///_internal/model/mof#CallBehaviorAction";
                export class _CallOperationAction
                {
                    static operation = "operation";
                    static target = "target";
                    static isSynchronous = "isSynchronous";
                    static result = "result";
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CallOperationAction_Uri = "dm:///_internal/model/mof#CallOperationAction";
                export class _Clause
                {
                    static body = "body";
                    static bodyOutput = "bodyOutput";
                    static decider = "decider";
                    static predecessorClause = "predecessorClause";
                    static successorClause = "successorClause";
                    static test = "test";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __Clause_Uri = "dm:///_internal/model/mof#Clause";
                export class _ClearAssociationAction
                {
                    static association = "association";
                    static object = "object";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ClearAssociationAction_Uri = "dm:///_internal/model/mof#ClearAssociationAction";
                export class _ClearStructuralFeatureAction
                {
                    static result = "result";
                    static object = "object";
                    static structuralFeature = "structuralFeature";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ClearStructuralFeatureAction_Uri = "dm:///_internal/model/mof#ClearStructuralFeatureAction";
                export class _ClearVariableAction
                {
                    static variable = "variable";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ClearVariableAction_Uri = "dm:///_internal/model/mof#ClearVariableAction";
                export class _ConditionalNode
                {
                    static clause = "clause";
                    static isAssured = "isAssured";
                    static isDeterminate = "isDeterminate";
                    static result = "result";
                    static activity = "activity";
                    static edge = "edge";
                    static mustIsolate = "mustIsolate";
                    static node = "node";
                    static structuredNodeInput = "structuredNodeInput";
                    static structuredNodeOutput = "structuredNodeOutput";
                    static variable = "variable";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __ConditionalNode_Uri = "dm:///_internal/model/mof#ConditionalNode";
                export class _CreateLinkAction
                {
                    static endData = "endData";
                    static inputValue = "inputValue";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CreateLinkAction_Uri = "dm:///_internal/model/mof#CreateLinkAction";
                export class _CreateLinkObjectAction
                {
                    static result = "result";
                    static endData = "endData";
                    static inputValue = "inputValue";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CreateLinkObjectAction_Uri = "dm:///_internal/model/mof#CreateLinkObjectAction";
                export class _CreateObjectAction
                {
                    static classifier = "classifier";
                    static result = "result";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __CreateObjectAction_Uri = "dm:///_internal/model/mof#CreateObjectAction";
                export class _DestroyLinkAction
                {
                    static endData = "endData";
                    static inputValue = "inputValue";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __DestroyLinkAction_Uri = "dm:///_internal/model/mof#DestroyLinkAction";
                export class _DestroyObjectAction
                {
                    static isDestroyLinks = "isDestroyLinks";
                    static isDestroyOwnedObjects = "isDestroyOwnedObjects";
                    static target = "target";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __DestroyObjectAction_Uri = "dm:///_internal/model/mof#DestroyObjectAction";
                export class _ExpansionNode
                {
                    static regionAsInput = "regionAsInput";
                    static regionAsOutput = "regionAsOutput";
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __ExpansionNode_Uri = "dm:///_internal/model/mof#ExpansionNode";
                export class _ExpansionRegion
                {
                    static inputElement = "inputElement";
                    static mode = "mode";
                    static outputElement = "outputElement";
                    static activity = "activity";
                    static edge = "edge";
                    static mustIsolate = "mustIsolate";
                    static node = "node";
                    static structuredNodeInput = "structuredNodeInput";
                    static structuredNodeOutput = "structuredNodeOutput";
                    static variable = "variable";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __ExpansionRegion_Uri = "dm:///_internal/model/mof#ExpansionRegion";
                export class _InputPin
                {
                    static isControl = "isControl";
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                }

                export const __InputPin_Uri = "dm:///_internal/model/mof#InputPin";
                export class _InvocationAction
                {
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __InvocationAction_Uri = "dm:///_internal/model/mof#InvocationAction";
                export class _LinkAction
                {
                    static endData = "endData";
                    static inputValue = "inputValue";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __LinkAction_Uri = "dm:///_internal/model/mof#LinkAction";
                export class _LinkEndCreationData
                {
                    static insertAt = "insertAt";
                    static isReplaceAll = "isReplaceAll";
                    static end = "end";
                    static qualifier = "qualifier";
                    static value = "value";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __LinkEndCreationData_Uri = "dm:///_internal/model/mof#LinkEndCreationData";
                export class _LinkEndData
                {
                    static end = "end";
                    static qualifier = "qualifier";
                    static value = "value";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __LinkEndData_Uri = "dm:///_internal/model/mof#LinkEndData";
                export class _LinkEndDestructionData
                {
                    static destroyAt = "destroyAt";
                    static isDestroyDuplicates = "isDestroyDuplicates";
                    static end = "end";
                    static qualifier = "qualifier";
                    static value = "value";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __LinkEndDestructionData_Uri = "dm:///_internal/model/mof#LinkEndDestructionData";
                export class _LoopNode
                {
                    static bodyOutput = "bodyOutput";
                    static bodyPart = "bodyPart";
                    static decider = "decider";
                    static isTestedFirst = "isTestedFirst";
                    static loopVariable = "loopVariable";
                    static loopVariableInput = "loopVariableInput";
                    static result = "result";
                    static setupPart = "setupPart";
                    static test = "test";
                    static activity = "activity";
                    static edge = "edge";
                    static mustIsolate = "mustIsolate";
                    static node = "node";
                    static structuredNodeInput = "structuredNodeInput";
                    static structuredNodeOutput = "structuredNodeOutput";
                    static variable = "variable";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __LoopNode_Uri = "dm:///_internal/model/mof#LoopNode";
                export class _OpaqueAction
                {
                    static body = "body";
                    static inputValue = "inputValue";
                    static language = "language";
                    static outputValue = "outputValue";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __OpaqueAction_Uri = "dm:///_internal/model/mof#OpaqueAction";
                export class _OutputPin
                {
                    static isControl = "isControl";
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                }

                export const __OutputPin_Uri = "dm:///_internal/model/mof#OutputPin";
                export class _Pin
                {
                    static isControl = "isControl";
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                }

                export const __Pin_Uri = "dm:///_internal/model/mof#Pin";
                export class _QualifierValue
                {
                    static qualifier = "qualifier";
                    static value = "value";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __QualifierValue_Uri = "dm:///_internal/model/mof#QualifierValue";
                export class _RaiseExceptionAction
                {
                    static exception = "exception";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __RaiseExceptionAction_Uri = "dm:///_internal/model/mof#RaiseExceptionAction";
                export class _ReadExtentAction
                {
                    static classifier = "classifier";
                    static result = "result";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadExtentAction_Uri = "dm:///_internal/model/mof#ReadExtentAction";
                export class _ReadIsClassifiedObjectAction
                {
                    static classifier = "classifier";
                    static isDirect = "isDirect";
                    static object = "object";
                    static result = "result";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadIsClassifiedObjectAction_Uri = "dm:///_internal/model/mof#ReadIsClassifiedObjectAction";
                export class _ReadLinkAction
                {
                    static result = "result";
                    static endData = "endData";
                    static inputValue = "inputValue";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadLinkAction_Uri = "dm:///_internal/model/mof#ReadLinkAction";
                export class _ReadLinkObjectEndAction
                {
                    static end = "end";
                    static object = "object";
                    static result = "result";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadLinkObjectEndAction_Uri = "dm:///_internal/model/mof#ReadLinkObjectEndAction";
                export class _ReadLinkObjectEndQualifierAction
                {
                    static object = "object";
                    static qualifier = "qualifier";
                    static result = "result";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadLinkObjectEndQualifierAction_Uri = "dm:///_internal/model/mof#ReadLinkObjectEndQualifierAction";
                export class _ReadSelfAction
                {
                    static result = "result";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadSelfAction_Uri = "dm:///_internal/model/mof#ReadSelfAction";
                export class _ReadStructuralFeatureAction
                {
                    static result = "result";
                    static object = "object";
                    static structuralFeature = "structuralFeature";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadStructuralFeatureAction_Uri = "dm:///_internal/model/mof#ReadStructuralFeatureAction";
                export class _ReadVariableAction
                {
                    static result = "result";
                    static variable = "variable";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReadVariableAction_Uri = "dm:///_internal/model/mof#ReadVariableAction";
                export class _ReclassifyObjectAction
                {
                    static isReplaceAll = "isReplaceAll";
                    static newClassifier = "newClassifier";
                    static object = "object";
                    static oldClassifier = "oldClassifier";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReclassifyObjectAction_Uri = "dm:///_internal/model/mof#ReclassifyObjectAction";
                export class _ReduceAction
                {
                    static collection = "collection";
                    static isOrdered = "isOrdered";
                    static reducer = "reducer";
                    static result = "result";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReduceAction_Uri = "dm:///_internal/model/mof#ReduceAction";
                export class _RemoveStructuralFeatureValueAction
                {
                    static isRemoveDuplicates = "isRemoveDuplicates";
                    static removeAt = "removeAt";
                    static result = "result";
                    static value = "value";
                    static object = "object";
                    static structuralFeature = "structuralFeature";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __RemoveStructuralFeatureValueAction_Uri = "dm:///_internal/model/mof#RemoveStructuralFeatureValueAction";
                export class _RemoveVariableValueAction
                {
                    static isRemoveDuplicates = "isRemoveDuplicates";
                    static removeAt = "removeAt";
                    static value = "value";
                    static variable = "variable";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __RemoveVariableValueAction_Uri = "dm:///_internal/model/mof#RemoveVariableValueAction";
                export class _ReplyAction
                {
                    static replyToCall = "replyToCall";
                    static replyValue = "replyValue";
                    static returnInformation = "returnInformation";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __ReplyAction_Uri = "dm:///_internal/model/mof#ReplyAction";
                export class _SendObjectAction
                {
                    static request = "request";
                    static target = "target";
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __SendObjectAction_Uri = "dm:///_internal/model/mof#SendObjectAction";
                export class _SendSignalAction
                {
                    static signal = "signal";
                    static target = "target";
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __SendSignalAction_Uri = "dm:///_internal/model/mof#SendSignalAction";
                export class _SequenceNode
                {
                    static executableNode = "executableNode";
                    static activity = "activity";
                    static edge = "edge";
                    static mustIsolate = "mustIsolate";
                    static node = "node";
                    static structuredNodeInput = "structuredNodeInput";
                    static structuredNodeOutput = "structuredNodeOutput";
                    static variable = "variable";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __SequenceNode_Uri = "dm:///_internal/model/mof#SequenceNode";
                export class _StartClassifierBehaviorAction
                {
                    static object = "object";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __StartClassifierBehaviorAction_Uri = "dm:///_internal/model/mof#StartClassifierBehaviorAction";
                export class _StartObjectBehaviorAction
                {
                    static object = "object";
                    static isSynchronous = "isSynchronous";
                    static result = "result";
                    static argument = "argument";
                    static onPort = "onPort";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __StartObjectBehaviorAction_Uri = "dm:///_internal/model/mof#StartObjectBehaviorAction";
                export class _StructuralFeatureAction
                {
                    static object = "object";
                    static structuralFeature = "structuralFeature";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __StructuralFeatureAction_Uri = "dm:///_internal/model/mof#StructuralFeatureAction";
                export class _StructuredActivityNode
                {
                    static activity = "activity";
                    static edge = "edge";
                    static mustIsolate = "mustIsolate";
                    static node = "node";
                    static structuredNodeInput = "structuredNodeInput";
                    static structuredNodeOutput = "structuredNodeOutput";
                    static variable = "variable";
                    static elementImport = "elementImport";
                    static importedMember = "importedMember";
                    static member = "member";
                    static ownedMember = "ownedMember";
                    static ownedRule = "ownedRule";
                    static packageImport = "packageImport";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static containedEdge = "containedEdge";
                    static containedNode = "containedNode";
                    static inActivity = "inActivity";
                    static subgroup = "subgroup";
                    static superGroup = "superGroup";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                }

                export const __StructuredActivityNode_Uri = "dm:///_internal/model/mof#StructuredActivityNode";
                export class _TestIdentityAction
                {
                    static first = "first";
                    static result = "result";
                    static second = "second";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __TestIdentityAction_Uri = "dm:///_internal/model/mof#TestIdentityAction";
                export class _UnmarshallAction
                {
                    static object = "object";
                    static result = "result";
                    static unmarshallType = "unmarshallType";
                    static context = "context";
                    static input = "input";
                    static isLocallyReentrant = "isLocallyReentrant";
                    static localPostcondition = "localPostcondition";
                    static localPrecondition = "localPrecondition";
                    static output = "output";
                    static handler = "handler";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                }

                export const __UnmarshallAction_Uri = "dm:///_internal/model/mof#UnmarshallAction";
                export class _ValuePin
                {
                    static value = "value";
                    static isControl = "isControl";
                    static inState = "inState";
                    static isControlType = "isControlType";
                    static ordering = "ordering";
                    static selection = "selection";
                    static upperBound = "upperBound";
                    static type = "type";
                    static clientDependency = "clientDependency";
                    static _name_ = "name";
                    static nameExpression = "nameExpression";
                    static namespace = "namespace";
                    static qualifiedName = "qualifiedName";
                    static visibility = "visibility";
                    static ownedComment = "ownedComment";
                    static ownedElement = "ownedElement";
                    static owner = "owner";
                    static activity = "activity";
                    static inGroup = "inGroup";
                    static inInterruptibleRegion = "inInterruptibleRegion";
                    static inPartition = "inPartition";
                    static inStructuredNode = "inStructuredNode";
                    static incoming = "incoming";
                    static outgoing = "outgoing";
                    static redefinedNode = "redefinedNode";
                    static isLeaf = "isLeaf";
                    static redefinedElement = "redefinedElement";
                    static redefinitionContext = "redefinitionContext";
                    static isOrdered = "isOrdered";
                    static isUnique = "isUnique";
                    static lower = "lower";
                    static lowerValue = "lowerValue";
                    static upper = "upper";
                    static upperValue = "upperValue";
                }

                export const __ValuePin_Uri = "dm:///_internal/model/mof#ValuePin";
        }

}

