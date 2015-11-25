using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.0.0.0
namespace DatenMeister.Filler
{
    public class FillTheUML
    {
        private static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public static void DoFill(IEnumerable<object> collection, DatenMeister._UML tree)
        {
            string name;
            IObject value;
            foreach (var item in collection)
            {
                value = item as IObject;
                name = GetNameOfElement(value);
                if (name == "UML") // Looking for package
                {
                    collection = value.get("packagedElement") as IEnumerable<object>;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IObject;
                        name = GetNameOfElement(value);
                        if (name == "Activities") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Activity") // Looking for class
                                {
                                    tree.Activities.@ActivityInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "edge") // Looking for property
                                        {
                                            tree.Activities.Activity.@edge = value;
                                        }
                                        if (name == "group") // Looking for property
                                        {
                                            tree.Activities.Activity.@group = value;
                                        }
                                        if (name == "isReadOnly") // Looking for property
                                        {
                                            tree.Activities.Activity.@isReadOnly = value;
                                        }
                                        if (name == "isSingleExecution") // Looking for property
                                        {
                                            tree.Activities.Activity.@isSingleExecution = value;
                                        }
                                        if (name == "node") // Looking for property
                                        {
                                            tree.Activities.Activity.@node = value;
                                        }
                                        if (name == "partition") // Looking for property
                                        {
                                            tree.Activities.Activity.@partition = value;
                                        }
                                        if (name == "structuredNode") // Looking for property
                                        {
                                            tree.Activities.Activity.@structuredNode = value;
                                        }
                                        if (name == "variable") // Looking for property
                                        {
                                            tree.Activities.Activity.@variable = value;
                                        }
                                    }
                                }
                                if (name == "ActivityEdge") // Looking for class
                                {
                                    tree.Activities.@ActivityEdgeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "activity") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@activity = value;
                                        }
                                        if (name == "guard") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@guard = value;
                                        }
                                        if (name == "inGroup") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@inGroup = value;
                                        }
                                        if (name == "inPartition") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@inPartition = value;
                                        }
                                        if (name == "inStructuredNode") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@inStructuredNode = value;
                                        }
                                        if (name == "interrupts") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@interrupts = value;
                                        }
                                        if (name == "redefinedEdge") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@redefinedEdge = value;
                                        }
                                        if (name == "source") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@source = value;
                                        }
                                        if (name == "target") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@target = value;
                                        }
                                        if (name == "weight") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge.@weight = value;
                                        }
                                    }
                                }
                                if (name == "ActivityFinalNode") // Looking for class
                                {
                                    tree.Activities.@ActivityFinalNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ActivityGroup") // Looking for class
                                {
                                    tree.Activities.@ActivityGroupInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "containedEdge") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup.@containedEdge = value;
                                        }
                                        if (name == "containedNode") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup.@containedNode = value;
                                        }
                                        if (name == "inActivity") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup.@inActivity = value;
                                        }
                                        if (name == "subgroup") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup.@subgroup = value;
                                        }
                                        if (name == "superGroup") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup.@superGroup = value;
                                        }
                                    }
                                }
                                if (name == "ActivityNode") // Looking for class
                                {
                                    tree.Activities.@ActivityNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "activity") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@activity = value;
                                        }
                                        if (name == "inGroup") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@inGroup = value;
                                        }
                                        if (name == "inInterruptibleRegion") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@inInterruptibleRegion = value;
                                        }
                                        if (name == "inPartition") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@inPartition = value;
                                        }
                                        if (name == "inStructuredNode") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@inStructuredNode = value;
                                        }
                                        if (name == "incoming") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@incoming = value;
                                        }
                                        if (name == "outgoing") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@outgoing = value;
                                        }
                                        if (name == "redefinedNode") // Looking for property
                                        {
                                            tree.Activities.ActivityNode.@redefinedNode = value;
                                        }
                                    }
                                }
                                if (name == "ActivityParameterNode") // Looking for class
                                {
                                    tree.Activities.@ActivityParameterNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "parameter") // Looking for property
                                        {
                                            tree.Activities.ActivityParameterNode.@parameter = value;
                                        }
                                    }
                                }
                                if (name == "ActivityPartition") // Looking for class
                                {
                                    tree.Activities.@ActivityPartitionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "edge") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition.@edge = value;
                                        }
                                        if (name == "isDimension") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition.@isDimension = value;
                                        }
                                        if (name == "isExternal") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition.@isExternal = value;
                                        }
                                        if (name == "node") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition.@node = value;
                                        }
                                        if (name == "represents") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition.@represents = value;
                                        }
                                        if (name == "subpartition") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition.@subpartition = value;
                                        }
                                        if (name == "superPartition") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition.@superPartition = value;
                                        }
                                    }
                                }
                                if (name == "CentralBufferNode") // Looking for class
                                {
                                    tree.Activities.@CentralBufferNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ControlFlow") // Looking for class
                                {
                                    tree.Activities.@ControlFlowInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ControlNode") // Looking for class
                                {
                                    tree.Activities.@ControlNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "DataStoreNode") // Looking for class
                                {
                                    tree.Activities.@DataStoreNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "DecisionNode") // Looking for class
                                {
                                    tree.Activities.@DecisionNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "decisionInput") // Looking for property
                                        {
                                            tree.Activities.DecisionNode.@decisionInput = value;
                                        }
                                        if (name == "decisionInputFlow") // Looking for property
                                        {
                                            tree.Activities.DecisionNode.@decisionInputFlow = value;
                                        }
                                    }
                                }
                                if (name == "ExceptionHandler") // Looking for class
                                {
                                    tree.Activities.@ExceptionHandlerInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "exceptionInput") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler.@exceptionInput = value;
                                        }
                                        if (name == "exceptionType") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler.@exceptionType = value;
                                        }
                                        if (name == "handlerBody") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler.@handlerBody = value;
                                        }
                                        if (name == "protectedNode") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler.@protectedNode = value;
                                        }
                                    }
                                }
                                if (name == "ExecutableNode") // Looking for class
                                {
                                    tree.Activities.@ExecutableNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "handler") // Looking for property
                                        {
                                            tree.Activities.ExecutableNode.@handler = value;
                                        }
                                    }
                                }
                                if (name == "FinalNode") // Looking for class
                                {
                                    tree.Activities.@FinalNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "FlowFinalNode") // Looking for class
                                {
                                    tree.Activities.@FlowFinalNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ForkNode") // Looking for class
                                {
                                    tree.Activities.@ForkNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "InitialNode") // Looking for class
                                {
                                    tree.Activities.@InitialNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "InterruptibleActivityRegion") // Looking for class
                                {
                                    tree.Activities.@InterruptibleActivityRegionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "interruptingEdge") // Looking for property
                                        {
                                            tree.Activities.InterruptibleActivityRegion.@interruptingEdge = value;
                                        }
                                        if (name == "node") // Looking for property
                                        {
                                            tree.Activities.InterruptibleActivityRegion.@node = value;
                                        }
                                    }
                                }
                                if (name == "JoinNode") // Looking for class
                                {
                                    tree.Activities.@JoinNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isCombineDuplicate") // Looking for property
                                        {
                                            tree.Activities.JoinNode.@isCombineDuplicate = value;
                                        }
                                        if (name == "joinSpec") // Looking for property
                                        {
                                            tree.Activities.JoinNode.@joinSpec = value;
                                        }
                                    }
                                }
                                if (name == "MergeNode") // Looking for class
                                {
                                    tree.Activities.@MergeNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ObjectFlow") // Looking for class
                                {
                                    tree.Activities.@ObjectFlowInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isMulticast") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow.@isMulticast = value;
                                        }
                                        if (name == "isMultireceive") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow.@isMultireceive = value;
                                        }
                                        if (name == "selection") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow.@selection = value;
                                        }
                                        if (name == "transformation") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow.@transformation = value;
                                        }
                                    }
                                }
                                if (name == "ObjectNode") // Looking for class
                                {
                                    tree.Activities.@ObjectNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "inState") // Looking for property
                                        {
                                            tree.Activities.ObjectNode.@inState = value;
                                        }
                                        if (name == "isControlType") // Looking for property
                                        {
                                            tree.Activities.ObjectNode.@isControlType = value;
                                        }
                                        if (name == "ordering") // Looking for property
                                        {
                                            tree.Activities.ObjectNode.@ordering = value;
                                        }
                                        if (name == "selection") // Looking for property
                                        {
                                            tree.Activities.ObjectNode.@selection = value;
                                        }
                                        if (name == "upperBound") // Looking for property
                                        {
                                            tree.Activities.ObjectNode.@upperBound = value;
                                        }
                                    }
                                }
                                if (name == "Variable") // Looking for class
                                {
                                    tree.Activities.@VariableInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "activityScope") // Looking for property
                                        {
                                            tree.Activities.Variable.@activityScope = value;
                                        }
                                        if (name == "scope") // Looking for property
                                        {
                                            tree.Activities.Variable.@scope = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Values") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Duration") // Looking for class
                                {
                                    tree.Values.@DurationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "expr") // Looking for property
                                        {
                                            tree.Values.Duration.@expr = value;
                                        }
                                        if (name == "observation") // Looking for property
                                        {
                                            tree.Values.Duration.@observation = value;
                                        }
                                    }
                                }
                                if (name == "DurationConstraint") // Looking for class
                                {
                                    tree.Values.@DurationConstraintInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.DurationConstraint.@firstEvent = value;
                                        }
                                        if (name == "specification") // Looking for property
                                        {
                                            tree.Values.DurationConstraint.@specification = value;
                                        }
                                    }
                                }
                                if (name == "DurationInterval") // Looking for class
                                {
                                    tree.Values.@DurationIntervalInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "max") // Looking for property
                                        {
                                            tree.Values.DurationInterval.@max = value;
                                        }
                                        if (name == "min") // Looking for property
                                        {
                                            tree.Values.DurationInterval.@min = value;
                                        }
                                    }
                                }
                                if (name == "DurationObservation") // Looking for class
                                {
                                    tree.Values.@DurationObservationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "event") // Looking for property
                                        {
                                            tree.Values.DurationObservation.@event = value;
                                        }
                                        if (name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.DurationObservation.@firstEvent = value;
                                        }
                                    }
                                }
                                if (name == "Expression") // Looking for class
                                {
                                    tree.Values.@ExpressionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "operand") // Looking for property
                                        {
                                            tree.Values.Expression.@operand = value;
                                        }
                                        if (name == "symbol") // Looking for property
                                        {
                                            tree.Values.Expression.@symbol = value;
                                        }
                                    }
                                }
                                if (name == "Interval") // Looking for class
                                {
                                    tree.Values.@IntervalInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "max") // Looking for property
                                        {
                                            tree.Values.Interval.@max = value;
                                        }
                                        if (name == "min") // Looking for property
                                        {
                                            tree.Values.Interval.@min = value;
                                        }
                                    }
                                }
                                if (name == "IntervalConstraint") // Looking for class
                                {
                                    tree.Values.@IntervalConstraintInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "specification") // Looking for property
                                        {
                                            tree.Values.IntervalConstraint.@specification = value;
                                        }
                                    }
                                }
                                if (name == "LiteralBoolean") // Looking for class
                                {
                                    tree.Values.@LiteralBooleanInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralBoolean.@value = value;
                                        }
                                    }
                                }
                                if (name == "LiteralInteger") // Looking for class
                                {
                                    tree.Values.@LiteralIntegerInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralInteger.@value = value;
                                        }
                                    }
                                }
                                if (name == "LiteralNull") // Looking for class
                                {
                                    tree.Values.@LiteralNullInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "LiteralReal") // Looking for class
                                {
                                    tree.Values.@LiteralRealInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralReal.@value = value;
                                        }
                                    }
                                }
                                if (name == "LiteralSpecification") // Looking for class
                                {
                                    tree.Values.@LiteralSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "LiteralString") // Looking for class
                                {
                                    tree.Values.@LiteralStringInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralString.@value = value;
                                        }
                                    }
                                }
                                if (name == "LiteralUnlimitedNatural") // Looking for class
                                {
                                    tree.Values.@LiteralUnlimitedNaturalInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralUnlimitedNatural.@value = value;
                                        }
                                    }
                                }
                                if (name == "Observation") // Looking for class
                                {
                                    tree.Values.@ObservationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "OpaqueExpression") // Looking for class
                                {
                                    tree.Values.@OpaqueExpressionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "behavior") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression.@behavior = value;
                                        }
                                        if (name == "body") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression.@body = value;
                                        }
                                        if (name == "language") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression.@language = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression.@result = value;
                                        }
                                    }
                                }
                                if (name == "StringExpression") // Looking for class
                                {
                                    tree.Values.@StringExpressionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "owningExpression") // Looking for property
                                        {
                                            tree.Values.StringExpression.@owningExpression = value;
                                        }
                                        if (name == "subExpression") // Looking for property
                                        {
                                            tree.Values.StringExpression.@subExpression = value;
                                        }
                                    }
                                }
                                if (name == "TimeConstraint") // Looking for class
                                {
                                    tree.Values.@TimeConstraintInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.TimeConstraint.@firstEvent = value;
                                        }
                                        if (name == "specification") // Looking for property
                                        {
                                            tree.Values.TimeConstraint.@specification = value;
                                        }
                                    }
                                }
                                if (name == "TimeExpression") // Looking for class
                                {
                                    tree.Values.@TimeExpressionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "expr") // Looking for property
                                        {
                                            tree.Values.TimeExpression.@expr = value;
                                        }
                                        if (name == "observation") // Looking for property
                                        {
                                            tree.Values.TimeExpression.@observation = value;
                                        }
                                    }
                                }
                                if (name == "TimeInterval") // Looking for class
                                {
                                    tree.Values.@TimeIntervalInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "max") // Looking for property
                                        {
                                            tree.Values.TimeInterval.@max = value;
                                        }
                                        if (name == "min") // Looking for property
                                        {
                                            tree.Values.TimeInterval.@min = value;
                                        }
                                    }
                                }
                                if (name == "TimeObservation") // Looking for class
                                {
                                    tree.Values.@TimeObservationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "event") // Looking for property
                                        {
                                            tree.Values.TimeObservation.@event = value;
                                        }
                                        if (name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.TimeObservation.@firstEvent = value;
                                        }
                                    }
                                }
                                if (name == "ValueSpecification") // Looking for class
                                {
                                    tree.Values.@ValueSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "UseCases") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Actor") // Looking for class
                                {
                                    tree.UseCases.@ActorInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Extend") // Looking for class
                                {
                                    tree.UseCases.@ExtendInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "condition") // Looking for property
                                        {
                                            tree.UseCases.Extend.@condition = value;
                                        }
                                        if (name == "extendedCase") // Looking for property
                                        {
                                            tree.UseCases.Extend.@extendedCase = value;
                                        }
                                        if (name == "extension") // Looking for property
                                        {
                                            tree.UseCases.Extend.@extension = value;
                                        }
                                        if (name == "extensionLocation") // Looking for property
                                        {
                                            tree.UseCases.Extend.@extensionLocation = value;
                                        }
                                    }
                                }
                                if (name == "ExtensionPoint") // Looking for class
                                {
                                    tree.UseCases.@ExtensionPointInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "useCase") // Looking for property
                                        {
                                            tree.UseCases.ExtensionPoint.@useCase = value;
                                        }
                                    }
                                }
                                if (name == "Include") // Looking for class
                                {
                                    tree.UseCases.@IncludeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "addition") // Looking for property
                                        {
                                            tree.UseCases.Include.@addition = value;
                                        }
                                        if (name == "includingCase") // Looking for property
                                        {
                                            tree.UseCases.Include.@includingCase = value;
                                        }
                                    }
                                }
                                if (name == "UseCase") // Looking for class
                                {
                                    tree.UseCases.@UseCaseInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "extend") // Looking for property
                                        {
                                            tree.UseCases.UseCase.@extend = value;
                                        }
                                        if (name == "extensionPoint") // Looking for property
                                        {
                                            tree.UseCases.UseCase.@extensionPoint = value;
                                        }
                                        if (name == "include") // Looking for property
                                        {
                                            tree.UseCases.UseCase.@include = value;
                                        }
                                        if (name == "subject") // Looking for property
                                        {
                                            tree.UseCases.UseCase.@subject = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "StructuredClassifiers") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Association") // Looking for class
                                {
                                    tree.StructuredClassifiers.@AssociationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "endType") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association.@endType = value;
                                        }
                                        if (name == "isDerived") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association.@isDerived = value;
                                        }
                                        if (name == "memberEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association.@memberEnd = value;
                                        }
                                        if (name == "navigableOwnedEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association.@navigableOwnedEnd = value;
                                        }
                                        if (name == "ownedEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association.@ownedEnd = value;
                                        }
                                    }
                                }
                                if (name == "AssociationClass") // Looking for class
                                {
                                    tree.StructuredClassifiers.@AssociationClassInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Class") // Looking for class
                                {
                                    tree.StructuredClassifiers.@ClassInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "extension") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@extension = value;
                                        }
                                        if (name == "isAbstract") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@isAbstract = value;
                                        }
                                        if (name == "isActive") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@isActive = value;
                                        }
                                        if (name == "nestedClassifier") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@nestedClassifier = value;
                                        }
                                        if (name == "ownedAttribute") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@ownedAttribute = value;
                                        }
                                        if (name == "ownedOperation") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@ownedOperation = value;
                                        }
                                        if (name == "ownedReception") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@ownedReception = value;
                                        }
                                        if (name == "superClass") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class.@superClass = value;
                                        }
                                    }
                                }
                                if (name == "Collaboration") // Looking for class
                                {
                                    tree.StructuredClassifiers.@CollaborationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "collaborationRole") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Collaboration.@collaborationRole = value;
                                        }
                                    }
                                }
                                if (name == "CollaborationUse") // Looking for class
                                {
                                    tree.StructuredClassifiers.@CollaborationUseInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "roleBinding") // Looking for property
                                        {
                                            tree.StructuredClassifiers.CollaborationUse.@roleBinding = value;
                                        }
                                        if (name == "type") // Looking for property
                                        {
                                            tree.StructuredClassifiers.CollaborationUse.@type = value;
                                        }
                                    }
                                }
                                if (name == "Component") // Looking for class
                                {
                                    tree.StructuredClassifiers.@ComponentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isIndirectlyInstantiated") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component.@isIndirectlyInstantiated = value;
                                        }
                                        if (name == "packagedElement") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component.@packagedElement = value;
                                        }
                                        if (name == "provided") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component.@provided = value;
                                        }
                                        if (name == "realization") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component.@realization = value;
                                        }
                                        if (name == "required") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component.@required = value;
                                        }
                                    }
                                }
                                if (name == "ComponentRealization") // Looking for class
                                {
                                    tree.StructuredClassifiers.@ComponentRealizationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "abstraction") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ComponentRealization.@abstraction = value;
                                        }
                                        if (name == "realizingClassifier") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ComponentRealization.@realizingClassifier = value;
                                        }
                                    }
                                }
                                if (name == "ConnectableElement") // Looking for class
                                {
                                    tree.StructuredClassifiers.@ConnectableElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "end") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectableElement.@end = value;
                                        }
                                        if (name == "templateParameter") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectableElement.@templateParameter = value;
                                        }
                                    }
                                }
                                if (name == "ConnectableElementTemplateParameter") // Looking for class
                                {
                                    tree.StructuredClassifiers.@ConnectableElementTemplateParameterInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "parameteredElement") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectableElementTemplateParameter.@parameteredElement = value;
                                        }
                                    }
                                }
                                if (name == "Connector") // Looking for class
                                {
                                    tree.StructuredClassifiers.@ConnectorInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "contract") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector.@contract = value;
                                        }
                                        if (name == "end") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector.@end = value;
                                        }
                                        if (name == "kind") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector.@kind = value;
                                        }
                                        if (name == "redefinedConnector") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector.@redefinedConnector = value;
                                        }
                                        if (name == "type") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector.@type = value;
                                        }
                                    }
                                }
                                if (name == "ConnectorEnd") // Looking for class
                                {
                                    tree.StructuredClassifiers.@ConnectorEndInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "definingEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectorEnd.@definingEnd = value;
                                        }
                                        if (name == "partWithPort") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectorEnd.@partWithPort = value;
                                        }
                                        if (name == "role") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectorEnd.@role = value;
                                        }
                                    }
                                }
                                if (name == "EncapsulatedClassifier") // Looking for class
                                {
                                    tree.StructuredClassifiers.@EncapsulatedClassifierInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedPort") // Looking for property
                                        {
                                            tree.StructuredClassifiers.EncapsulatedClassifier.@ownedPort = value;
                                        }
                                    }
                                }
                                if (name == "Port") // Looking for class
                                {
                                    tree.StructuredClassifiers.@PortInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isBehavior") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port.@isBehavior = value;
                                        }
                                        if (name == "isConjugated") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port.@isConjugated = value;
                                        }
                                        if (name == "isService") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port.@isService = value;
                                        }
                                        if (name == "protocol") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port.@protocol = value;
                                        }
                                        if (name == "provided") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port.@provided = value;
                                        }
                                        if (name == "redefinedPort") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port.@redefinedPort = value;
                                        }
                                        if (name == "required") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port.@required = value;
                                        }
                                    }
                                }
                                if (name == "StructuredClassifier") // Looking for class
                                {
                                    tree.StructuredClassifiers.@StructuredClassifierInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedAttribute") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier.@ownedAttribute = value;
                                        }
                                        if (name == "ownedConnector") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier.@ownedConnector = value;
                                        }
                                        if (name == "part") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier.@part = value;
                                        }
                                        if (name == "role") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier.@role = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "StateMachines") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "ConnectionPointReference") // Looking for class
                                {
                                    tree.StateMachines.@ConnectionPointReferenceInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "entry") // Looking for property
                                        {
                                            tree.StateMachines.ConnectionPointReference.@entry = value;
                                        }
                                        if (name == "exit") // Looking for property
                                        {
                                            tree.StateMachines.ConnectionPointReference.@exit = value;
                                        }
                                        if (name == "state") // Looking for property
                                        {
                                            tree.StateMachines.ConnectionPointReference.@state = value;
                                        }
                                    }
                                }
                                if (name == "FinalState") // Looking for class
                                {
                                    tree.StateMachines.@FinalStateInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ProtocolConformance") // Looking for class
                                {
                                    tree.StateMachines.@ProtocolConformanceInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "generalMachine") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolConformance.@generalMachine = value;
                                        }
                                        if (name == "specificMachine") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolConformance.@specificMachine = value;
                                        }
                                    }
                                }
                                if (name == "ProtocolStateMachine") // Looking for class
                                {
                                    tree.StateMachines.@ProtocolStateMachineInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "conformance") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolStateMachine.@conformance = value;
                                        }
                                    }
                                }
                                if (name == "ProtocolTransition") // Looking for class
                                {
                                    tree.StateMachines.@ProtocolTransitionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "postCondition") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolTransition.@postCondition = value;
                                        }
                                        if (name == "preCondition") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolTransition.@preCondition = value;
                                        }
                                        if (name == "referred") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolTransition.@referred = value;
                                        }
                                    }
                                }
                                if (name == "Pseudostate") // Looking for class
                                {
                                    tree.StateMachines.@PseudostateInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "kind") // Looking for property
                                        {
                                            tree.StateMachines.Pseudostate.@kind = value;
                                        }
                                        if (name == "state") // Looking for property
                                        {
                                            tree.StateMachines.Pseudostate.@state = value;
                                        }
                                        if (name == "stateMachine") // Looking for property
                                        {
                                            tree.StateMachines.Pseudostate.@stateMachine = value;
                                        }
                                    }
                                }
                                if (name == "Region") // Looking for class
                                {
                                    tree.StateMachines.@RegionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "extendedRegion") // Looking for property
                                        {
                                            tree.StateMachines.Region.@extendedRegion = value;
                                        }
                                        if (name == "redefinitionContext") // Looking for property
                                        {
                                            tree.StateMachines.Region.@redefinitionContext = value;
                                        }
                                        if (name == "state") // Looking for property
                                        {
                                            tree.StateMachines.Region.@state = value;
                                        }
                                        if (name == "stateMachine") // Looking for property
                                        {
                                            tree.StateMachines.Region.@stateMachine = value;
                                        }
                                        if (name == "subvertex") // Looking for property
                                        {
                                            tree.StateMachines.Region.@subvertex = value;
                                        }
                                        if (name == "transition") // Looking for property
                                        {
                                            tree.StateMachines.Region.@transition = value;
                                        }
                                    }
                                }
                                if (name == "State") // Looking for class
                                {
                                    tree.StateMachines.@StateInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "connection") // Looking for property
                                        {
                                            tree.StateMachines.State.@connection = value;
                                        }
                                        if (name == "connectionPoint") // Looking for property
                                        {
                                            tree.StateMachines.State.@connectionPoint = value;
                                        }
                                        if (name == "deferrableTrigger") // Looking for property
                                        {
                                            tree.StateMachines.State.@deferrableTrigger = value;
                                        }
                                        if (name == "doActivity") // Looking for property
                                        {
                                            tree.StateMachines.State.@doActivity = value;
                                        }
                                        if (name == "entry") // Looking for property
                                        {
                                            tree.StateMachines.State.@entry = value;
                                        }
                                        if (name == "exit") // Looking for property
                                        {
                                            tree.StateMachines.State.@exit = value;
                                        }
                                        if (name == "isComposite") // Looking for property
                                        {
                                            tree.StateMachines.State.@isComposite = value;
                                        }
                                        if (name == "isOrthogonal") // Looking for property
                                        {
                                            tree.StateMachines.State.@isOrthogonal = value;
                                        }
                                        if (name == "isSimple") // Looking for property
                                        {
                                            tree.StateMachines.State.@isSimple = value;
                                        }
                                        if (name == "isSubmachineState") // Looking for property
                                        {
                                            tree.StateMachines.State.@isSubmachineState = value;
                                        }
                                        if (name == "redefinedState") // Looking for property
                                        {
                                            tree.StateMachines.State.@redefinedState = value;
                                        }
                                        if (name == "redefinitionContext") // Looking for property
                                        {
                                            tree.StateMachines.State.@redefinitionContext = value;
                                        }
                                        if (name == "region") // Looking for property
                                        {
                                            tree.StateMachines.State.@region = value;
                                        }
                                        if (name == "stateInvariant") // Looking for property
                                        {
                                            tree.StateMachines.State.@stateInvariant = value;
                                        }
                                        if (name == "submachine") // Looking for property
                                        {
                                            tree.StateMachines.State.@submachine = value;
                                        }
                                    }
                                }
                                if (name == "StateMachine") // Looking for class
                                {
                                    tree.StateMachines.@StateMachineInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "connectionPoint") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine.@connectionPoint = value;
                                        }
                                        if (name == "extendedStateMachine") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine.@extendedStateMachine = value;
                                        }
                                        if (name == "region") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine.@region = value;
                                        }
                                        if (name == "submachineState") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine.@submachineState = value;
                                        }
                                    }
                                }
                                if (name == "Transition") // Looking for class
                                {
                                    tree.StateMachines.@TransitionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "container") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@container = value;
                                        }
                                        if (name == "effect") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@effect = value;
                                        }
                                        if (name == "guard") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@guard = value;
                                        }
                                        if (name == "kind") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@kind = value;
                                        }
                                        if (name == "redefinedTransition") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@redefinedTransition = value;
                                        }
                                        if (name == "redefinitionContext") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@redefinitionContext = value;
                                        }
                                        if (name == "source") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@source = value;
                                        }
                                        if (name == "target") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@target = value;
                                        }
                                        if (name == "trigger") // Looking for property
                                        {
                                            tree.StateMachines.Transition.@trigger = value;
                                        }
                                    }
                                }
                                if (name == "Vertex") // Looking for class
                                {
                                    tree.StateMachines.@VertexInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "container") // Looking for property
                                        {
                                            tree.StateMachines.Vertex.@container = value;
                                        }
                                        if (name == "incoming") // Looking for property
                                        {
                                            tree.StateMachines.Vertex.@incoming = value;
                                        }
                                        if (name == "outgoing") // Looking for property
                                        {
                                            tree.StateMachines.Vertex.@outgoing = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "SimpleClassifiers") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "BehavioredClassifier") // Looking for class
                                {
                                    tree.SimpleClassifiers.@BehavioredClassifierInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "classifierBehavior") // Looking for property
                                        {
                                            tree.SimpleClassifiers.BehavioredClassifier.@classifierBehavior = value;
                                        }
                                        if (name == "interfaceRealization") // Looking for property
                                        {
                                            tree.SimpleClassifiers.BehavioredClassifier.@interfaceRealization = value;
                                        }
                                        if (name == "ownedBehavior") // Looking for property
                                        {
                                            tree.SimpleClassifiers.BehavioredClassifier.@ownedBehavior = value;
                                        }
                                    }
                                }
                                if (name == "DataType") // Looking for class
                                {
                                    tree.SimpleClassifiers.@DataTypeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedAttribute") // Looking for property
                                        {
                                            tree.SimpleClassifiers.DataType.@ownedAttribute = value;
                                        }
                                        if (name == "ownedOperation") // Looking for property
                                        {
                                            tree.SimpleClassifiers.DataType.@ownedOperation = value;
                                        }
                                    }
                                }
                                if (name == "Enumeration") // Looking for class
                                {
                                    tree.SimpleClassifiers.@EnumerationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedLiteral") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Enumeration.@ownedLiteral = value;
                                        }
                                    }
                                }
                                if (name == "EnumerationLiteral") // Looking for class
                                {
                                    tree.SimpleClassifiers.@EnumerationLiteralInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "classifier") // Looking for property
                                        {
                                            tree.SimpleClassifiers.EnumerationLiteral.@classifier = value;
                                        }
                                        if (name == "enumeration") // Looking for property
                                        {
                                            tree.SimpleClassifiers.EnumerationLiteral.@enumeration = value;
                                        }
                                    }
                                }
                                if (name == "Interface") // Looking for class
                                {
                                    tree.SimpleClassifiers.@InterfaceInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "nestedClassifier") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface.@nestedClassifier = value;
                                        }
                                        if (name == "ownedAttribute") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface.@ownedAttribute = value;
                                        }
                                        if (name == "ownedOperation") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface.@ownedOperation = value;
                                        }
                                        if (name == "ownedReception") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface.@ownedReception = value;
                                        }
                                        if (name == "protocol") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface.@protocol = value;
                                        }
                                        if (name == "redefinedInterface") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface.@redefinedInterface = value;
                                        }
                                    }
                                }
                                if (name == "InterfaceRealization") // Looking for class
                                {
                                    tree.SimpleClassifiers.@InterfaceRealizationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "contract") // Looking for property
                                        {
                                            tree.SimpleClassifiers.InterfaceRealization.@contract = value;
                                        }
                                        if (name == "implementingClassifier") // Looking for property
                                        {
                                            tree.SimpleClassifiers.InterfaceRealization.@implementingClassifier = value;
                                        }
                                    }
                                }
                                if (name == "PrimitiveType") // Looking for class
                                {
                                    tree.SimpleClassifiers.@PrimitiveTypeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Reception") // Looking for class
                                {
                                    tree.SimpleClassifiers.@ReceptionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "signal") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Reception.@signal = value;
                                        }
                                    }
                                }
                                if (name == "Signal") // Looking for class
                                {
                                    tree.SimpleClassifiers.@SignalInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedAttribute") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Signal.@ownedAttribute = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Packages") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Extension") // Looking for class
                                {
                                    tree.Packages.@ExtensionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isRequired") // Looking for property
                                        {
                                            tree.Packages.Extension.@isRequired = value;
                                        }
                                        if (name == "metaclass") // Looking for property
                                        {
                                            tree.Packages.Extension.@metaclass = value;
                                        }
                                        if (name == "ownedEnd") // Looking for property
                                        {
                                            tree.Packages.Extension.@ownedEnd = value;
                                        }
                                    }
                                }
                                if (name == "ExtensionEnd") // Looking for class
                                {
                                    tree.Packages.@ExtensionEndInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "lower") // Looking for property
                                        {
                                            tree.Packages.ExtensionEnd.@lower = value;
                                        }
                                        if (name == "type") // Looking for property
                                        {
                                            tree.Packages.ExtensionEnd.@type = value;
                                        }
                                    }
                                }
                                if (name == "Image") // Looking for class
                                {
                                    tree.Packages.@ImageInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "content") // Looking for property
                                        {
                                            tree.Packages.Image.@content = value;
                                        }
                                        if (name == "format") // Looking for property
                                        {
                                            tree.Packages.Image.@format = value;
                                        }
                                        if (name == "location") // Looking for property
                                        {
                                            tree.Packages.Image.@location = value;
                                        }
                                    }
                                }
                                if (name == "Model") // Looking for class
                                {
                                    tree.Packages.@ModelInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "viewpoint") // Looking for property
                                        {
                                            tree.Packages.Model.@viewpoint = value;
                                        }
                                    }
                                }
                                if (name == "Package") // Looking for class
                                {
                                    tree.Packages.@PackageInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "URI") // Looking for property
                                        {
                                            tree.Packages.Package.@URI = value;
                                        }
                                        if (name == "nestedPackage") // Looking for property
                                        {
                                            tree.Packages.Package.@nestedPackage = value;
                                        }
                                        if (name == "nestingPackage") // Looking for property
                                        {
                                            tree.Packages.Package.@nestingPackage = value;
                                        }
                                        if (name == "ownedStereotype") // Looking for property
                                        {
                                            tree.Packages.Package.@ownedStereotype = value;
                                        }
                                        if (name == "ownedType") // Looking for property
                                        {
                                            tree.Packages.Package.@ownedType = value;
                                        }
                                        if (name == "packageMerge") // Looking for property
                                        {
                                            tree.Packages.Package.@packageMerge = value;
                                        }
                                        if (name == "packagedElement") // Looking for property
                                        {
                                            tree.Packages.Package.@packagedElement = value;
                                        }
                                        if (name == "profileApplication") // Looking for property
                                        {
                                            tree.Packages.Package.@profileApplication = value;
                                        }
                                    }
                                }
                                if (name == "PackageMerge") // Looking for class
                                {
                                    tree.Packages.@PackageMergeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "mergedPackage") // Looking for property
                                        {
                                            tree.Packages.PackageMerge.@mergedPackage = value;
                                        }
                                        if (name == "receivingPackage") // Looking for property
                                        {
                                            tree.Packages.PackageMerge.@receivingPackage = value;
                                        }
                                    }
                                }
                                if (name == "Profile") // Looking for class
                                {
                                    tree.Packages.@ProfileInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "metaclassReference") // Looking for property
                                        {
                                            tree.Packages.Profile.@metaclassReference = value;
                                        }
                                        if (name == "metamodelReference") // Looking for property
                                        {
                                            tree.Packages.Profile.@metamodelReference = value;
                                        }
                                    }
                                }
                                if (name == "ProfileApplication") // Looking for class
                                {
                                    tree.Packages.@ProfileApplicationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "appliedProfile") // Looking for property
                                        {
                                            tree.Packages.ProfileApplication.@appliedProfile = value;
                                        }
                                        if (name == "applyingPackage") // Looking for property
                                        {
                                            tree.Packages.ProfileApplication.@applyingPackage = value;
                                        }
                                        if (name == "isStrict") // Looking for property
                                        {
                                            tree.Packages.ProfileApplication.@isStrict = value;
                                        }
                                    }
                                }
                                if (name == "Stereotype") // Looking for class
                                {
                                    tree.Packages.@StereotypeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "icon") // Looking for property
                                        {
                                            tree.Packages.Stereotype.@icon = value;
                                        }
                                        if (name == "profile") // Looking for property
                                        {
                                            tree.Packages.Stereotype.@profile = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Interactions") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "ActionExecutionSpecification") // Looking for class
                                {
                                    tree.Interactions.@ActionExecutionSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "action") // Looking for property
                                        {
                                            tree.Interactions.ActionExecutionSpecification.@action = value;
                                        }
                                    }
                                }
                                if (name == "BehaviorExecutionSpecification") // Looking for class
                                {
                                    tree.Interactions.@BehaviorExecutionSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "behavior") // Looking for property
                                        {
                                            tree.Interactions.BehaviorExecutionSpecification.@behavior = value;
                                        }
                                    }
                                }
                                if (name == "CombinedFragment") // Looking for class
                                {
                                    tree.Interactions.@CombinedFragmentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "cfragmentGate") // Looking for property
                                        {
                                            tree.Interactions.CombinedFragment.@cfragmentGate = value;
                                        }
                                        if (name == "interactionOperator") // Looking for property
                                        {
                                            tree.Interactions.CombinedFragment.@interactionOperator = value;
                                        }
                                        if (name == "operand") // Looking for property
                                        {
                                            tree.Interactions.CombinedFragment.@operand = value;
                                        }
                                    }
                                }
                                if (name == "ConsiderIgnoreFragment") // Looking for class
                                {
                                    tree.Interactions.@ConsiderIgnoreFragmentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "message") // Looking for property
                                        {
                                            tree.Interactions.ConsiderIgnoreFragment.@message = value;
                                        }
                                    }
                                }
                                if (name == "Continuation") // Looking for class
                                {
                                    tree.Interactions.@ContinuationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "setting") // Looking for property
                                        {
                                            tree.Interactions.Continuation.@setting = value;
                                        }
                                    }
                                }
                                if (name == "DestructionOccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.@DestructionOccurrenceSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ExecutionOccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.@ExecutionOccurrenceSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "execution") // Looking for property
                                        {
                                            tree.Interactions.ExecutionOccurrenceSpecification.@execution = value;
                                        }
                                    }
                                }
                                if (name == "ExecutionSpecification") // Looking for class
                                {
                                    tree.Interactions.@ExecutionSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "finish") // Looking for property
                                        {
                                            tree.Interactions.ExecutionSpecification.@finish = value;
                                        }
                                        if (name == "start") // Looking for property
                                        {
                                            tree.Interactions.ExecutionSpecification.@start = value;
                                        }
                                    }
                                }
                                if (name == "Gate") // Looking for class
                                {
                                    tree.Interactions.@GateInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "GeneralOrdering") // Looking for class
                                {
                                    tree.Interactions.@GeneralOrderingInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "after") // Looking for property
                                        {
                                            tree.Interactions.GeneralOrdering.@after = value;
                                        }
                                        if (name == "before") // Looking for property
                                        {
                                            tree.Interactions.GeneralOrdering.@before = value;
                                        }
                                    }
                                }
                                if (name == "Interaction") // Looking for class
                                {
                                    tree.Interactions.@InteractionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "action") // Looking for property
                                        {
                                            tree.Interactions.Interaction.@action = value;
                                        }
                                        if (name == "formalGate") // Looking for property
                                        {
                                            tree.Interactions.Interaction.@formalGate = value;
                                        }
                                        if (name == "fragment") // Looking for property
                                        {
                                            tree.Interactions.Interaction.@fragment = value;
                                        }
                                        if (name == "lifeline") // Looking for property
                                        {
                                            tree.Interactions.Interaction.@lifeline = value;
                                        }
                                        if (name == "message") // Looking for property
                                        {
                                            tree.Interactions.Interaction.@message = value;
                                        }
                                    }
                                }
                                if (name == "InteractionConstraint") // Looking for class
                                {
                                    tree.Interactions.@InteractionConstraintInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "maxint") // Looking for property
                                        {
                                            tree.Interactions.InteractionConstraint.@maxint = value;
                                        }
                                        if (name == "minint") // Looking for property
                                        {
                                            tree.Interactions.InteractionConstraint.@minint = value;
                                        }
                                    }
                                }
                                if (name == "InteractionFragment") // Looking for class
                                {
                                    tree.Interactions.@InteractionFragmentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "covered") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment.@covered = value;
                                        }
                                        if (name == "enclosingInteraction") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment.@enclosingInteraction = value;
                                        }
                                        if (name == "enclosingOperand") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment.@enclosingOperand = value;
                                        }
                                        if (name == "generalOrdering") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment.@generalOrdering = value;
                                        }
                                    }
                                }
                                if (name == "InteractionOperand") // Looking for class
                                {
                                    tree.Interactions.@InteractionOperandInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "fragment") // Looking for property
                                        {
                                            tree.Interactions.InteractionOperand.@fragment = value;
                                        }
                                        if (name == "guard") // Looking for property
                                        {
                                            tree.Interactions.InteractionOperand.@guard = value;
                                        }
                                    }
                                }
                                if (name == "InteractionUse") // Looking for class
                                {
                                    tree.Interactions.@InteractionUseInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "actualGate") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse.@actualGate = value;
                                        }
                                        if (name == "argument") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse.@argument = value;
                                        }
                                        if (name == "refersTo") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse.@refersTo = value;
                                        }
                                        if (name == "returnValue") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse.@returnValue = value;
                                        }
                                        if (name == "returnValueRecipient") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse.@returnValueRecipient = value;
                                        }
                                    }
                                }
                                if (name == "Lifeline") // Looking for class
                                {
                                    tree.Interactions.@LifelineInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "coveredBy") // Looking for property
                                        {
                                            tree.Interactions.Lifeline.@coveredBy = value;
                                        }
                                        if (name == "decomposedAs") // Looking for property
                                        {
                                            tree.Interactions.Lifeline.@decomposedAs = value;
                                        }
                                        if (name == "interaction") // Looking for property
                                        {
                                            tree.Interactions.Lifeline.@interaction = value;
                                        }
                                        if (name == "represents") // Looking for property
                                        {
                                            tree.Interactions.Lifeline.@represents = value;
                                        }
                                        if (name == "selector") // Looking for property
                                        {
                                            tree.Interactions.Lifeline.@selector = value;
                                        }
                                    }
                                }
                                if (name == "Message") // Looking for class
                                {
                                    tree.Interactions.@MessageInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "argument") // Looking for property
                                        {
                                            tree.Interactions.Message.@argument = value;
                                        }
                                        if (name == "connector") // Looking for property
                                        {
                                            tree.Interactions.Message.@connector = value;
                                        }
                                        if (name == "interaction") // Looking for property
                                        {
                                            tree.Interactions.Message.@interaction = value;
                                        }
                                        if (name == "messageKind") // Looking for property
                                        {
                                            tree.Interactions.Message.@messageKind = value;
                                        }
                                        if (name == "messageSort") // Looking for property
                                        {
                                            tree.Interactions.Message.@messageSort = value;
                                        }
                                        if (name == "receiveEvent") // Looking for property
                                        {
                                            tree.Interactions.Message.@receiveEvent = value;
                                        }
                                        if (name == "sendEvent") // Looking for property
                                        {
                                            tree.Interactions.Message.@sendEvent = value;
                                        }
                                        if (name == "signature") // Looking for property
                                        {
                                            tree.Interactions.Message.@signature = value;
                                        }
                                    }
                                }
                                if (name == "MessageEnd") // Looking for class
                                {
                                    tree.Interactions.@MessageEndInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "message") // Looking for property
                                        {
                                            tree.Interactions.MessageEnd.@message = value;
                                        }
                                    }
                                }
                                if (name == "MessageOccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.@MessageOccurrenceSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "OccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.@OccurrenceSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "covered") // Looking for property
                                        {
                                            tree.Interactions.OccurrenceSpecification.@covered = value;
                                        }
                                        if (name == "toAfter") // Looking for property
                                        {
                                            tree.Interactions.OccurrenceSpecification.@toAfter = value;
                                        }
                                        if (name == "toBefore") // Looking for property
                                        {
                                            tree.Interactions.OccurrenceSpecification.@toBefore = value;
                                        }
                                    }
                                }
                                if (name == "PartDecomposition") // Looking for class
                                {
                                    tree.Interactions.@PartDecompositionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "StateInvariant") // Looking for class
                                {
                                    tree.Interactions.@StateInvariantInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "covered") // Looking for property
                                        {
                                            tree.Interactions.StateInvariant.@covered = value;
                                        }
                                        if (name == "invariant") // Looking for property
                                        {
                                            tree.Interactions.StateInvariant.@invariant = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "InformationFlows") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "InformationFlow") // Looking for class
                                {
                                    tree.InformationFlows.@InformationFlowInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "conveyed") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow.@conveyed = value;
                                        }
                                        if (name == "informationSource") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow.@informationSource = value;
                                        }
                                        if (name == "informationTarget") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow.@informationTarget = value;
                                        }
                                        if (name == "realization") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow.@realization = value;
                                        }
                                        if (name == "realizingActivityEdge") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow.@realizingActivityEdge = value;
                                        }
                                        if (name == "realizingConnector") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow.@realizingConnector = value;
                                        }
                                        if (name == "realizingMessage") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow.@realizingMessage = value;
                                        }
                                    }
                                }
                                if (name == "InformationItem") // Looking for class
                                {
                                    tree.InformationFlows.@InformationItemInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "represented") // Looking for property
                                        {
                                            tree.InformationFlows.InformationItem.@represented = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Deployments") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Artifact") // Looking for class
                                {
                                    tree.Deployments.@ArtifactInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "fileName") // Looking for property
                                        {
                                            tree.Deployments.Artifact.@fileName = value;
                                        }
                                        if (name == "manifestation") // Looking for property
                                        {
                                            tree.Deployments.Artifact.@manifestation = value;
                                        }
                                        if (name == "nestedArtifact") // Looking for property
                                        {
                                            tree.Deployments.Artifact.@nestedArtifact = value;
                                        }
                                        if (name == "ownedAttribute") // Looking for property
                                        {
                                            tree.Deployments.Artifact.@ownedAttribute = value;
                                        }
                                        if (name == "ownedOperation") // Looking for property
                                        {
                                            tree.Deployments.Artifact.@ownedOperation = value;
                                        }
                                    }
                                }
                                if (name == "CommunicationPath") // Looking for class
                                {
                                    tree.Deployments.@CommunicationPathInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "DeployedArtifact") // Looking for class
                                {
                                    tree.Deployments.@DeployedArtifactInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Deployment") // Looking for class
                                {
                                    tree.Deployments.@DeploymentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "configuration") // Looking for property
                                        {
                                            tree.Deployments.Deployment.@configuration = value;
                                        }
                                        if (name == "deployedArtifact") // Looking for property
                                        {
                                            tree.Deployments.Deployment.@deployedArtifact = value;
                                        }
                                        if (name == "location") // Looking for property
                                        {
                                            tree.Deployments.Deployment.@location = value;
                                        }
                                    }
                                }
                                if (name == "DeploymentSpecification") // Looking for class
                                {
                                    tree.Deployments.@DeploymentSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "deployment") // Looking for property
                                        {
                                            tree.Deployments.DeploymentSpecification.@deployment = value;
                                        }
                                        if (name == "deploymentLocation") // Looking for property
                                        {
                                            tree.Deployments.DeploymentSpecification.@deploymentLocation = value;
                                        }
                                        if (name == "executionLocation") // Looking for property
                                        {
                                            tree.Deployments.DeploymentSpecification.@executionLocation = value;
                                        }
                                    }
                                }
                                if (name == "DeploymentTarget") // Looking for class
                                {
                                    tree.Deployments.@DeploymentTargetInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "deployedElement") // Looking for property
                                        {
                                            tree.Deployments.DeploymentTarget.@deployedElement = value;
                                        }
                                        if (name == "deployment") // Looking for property
                                        {
                                            tree.Deployments.DeploymentTarget.@deployment = value;
                                        }
                                    }
                                }
                                if (name == "Device") // Looking for class
                                {
                                    tree.Deployments.@DeviceInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ExecutionEnvironment") // Looking for class
                                {
                                    tree.Deployments.@ExecutionEnvironmentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Manifestation") // Looking for class
                                {
                                    tree.Deployments.@ManifestationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "utilizedElement") // Looking for property
                                        {
                                            tree.Deployments.Manifestation.@utilizedElement = value;
                                        }
                                    }
                                }
                                if (name == "Node") // Looking for class
                                {
                                    tree.Deployments.@NodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "nestedNode") // Looking for property
                                        {
                                            tree.Deployments.Node.@nestedNode = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "CommonStructure") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Abstraction") // Looking for class
                                {
                                    tree.CommonStructure.@AbstractionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "mapping") // Looking for property
                                        {
                                            tree.CommonStructure.Abstraction.@mapping = value;
                                        }
                                    }
                                }
                                if (name == "Comment") // Looking for class
                                {
                                    tree.CommonStructure.@CommentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "annotatedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Comment.@annotatedElement = value;
                                        }
                                        if (name == "body") // Looking for property
                                        {
                                            tree.CommonStructure.Comment.@body = value;
                                        }
                                    }
                                }
                                if (name == "Constraint") // Looking for class
                                {
                                    tree.CommonStructure.@ConstraintInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "constrainedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Constraint.@constrainedElement = value;
                                        }
                                        if (name == "context") // Looking for property
                                        {
                                            tree.CommonStructure.Constraint.@context = value;
                                        }
                                        if (name == "specification") // Looking for property
                                        {
                                            tree.CommonStructure.Constraint.@specification = value;
                                        }
                                    }
                                }
                                if (name == "Dependency") // Looking for class
                                {
                                    tree.CommonStructure.@DependencyInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "client") // Looking for property
                                        {
                                            tree.CommonStructure.Dependency.@client = value;
                                        }
                                        if (name == "supplier") // Looking for property
                                        {
                                            tree.CommonStructure.Dependency.@supplier = value;
                                        }
                                    }
                                }
                                if (name == "DirectedRelationship") // Looking for class
                                {
                                    tree.CommonStructure.@DirectedRelationshipInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "source") // Looking for property
                                        {
                                            tree.CommonStructure.DirectedRelationship.@source = value;
                                        }
                                        if (name == "target") // Looking for property
                                        {
                                            tree.CommonStructure.DirectedRelationship.@target = value;
                                        }
                                    }
                                }
                                if (name == "Element") // Looking for class
                                {
                                    tree.CommonStructure.@ElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedComment") // Looking for property
                                        {
                                            tree.CommonStructure.Element.@ownedComment = value;
                                        }
                                        if (name == "ownedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Element.@ownedElement = value;
                                        }
                                        if (name == "owner") // Looking for property
                                        {
                                            tree.CommonStructure.Element.@owner = value;
                                        }
                                    }
                                }
                                if (name == "ElementImport") // Looking for class
                                {
                                    tree.CommonStructure.@ElementImportInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "alias") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport.@alias = value;
                                        }
                                        if (name == "importedElement") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport.@importedElement = value;
                                        }
                                        if (name == "importingNamespace") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport.@importingNamespace = value;
                                        }
                                        if (name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport.@visibility = value;
                                        }
                                    }
                                }
                                if (name == "MultiplicityElement") // Looking for class
                                {
                                    tree.CommonStructure.@MultiplicityElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isOrdered") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement.@isOrdered = value;
                                        }
                                        if (name == "isUnique") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement.@isUnique = value;
                                        }
                                        if (name == "lower") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement.@lower = value;
                                        }
                                        if (name == "lowerValue") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement.@lowerValue = value;
                                        }
                                        if (name == "upper") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement.@upper = value;
                                        }
                                        if (name == "upperValue") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement.@upperValue = value;
                                        }
                                    }
                                }
                                if (name == "NamedElement") // Looking for class
                                {
                                    tree.CommonStructure.@NamedElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "clientDependency") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement.@clientDependency = value;
                                        }
                                        if (name == "name") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement.@name = value;
                                        }
                                        if (name == "nameExpression") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement.@nameExpression = value;
                                        }
                                        if (name == "namespace") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement.@namespace = value;
                                        }
                                        if (name == "qualifiedName") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement.@qualifiedName = value;
                                        }
                                        if (name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement.@visibility = value;
                                        }
                                    }
                                }
                                if (name == "Namespace") // Looking for class
                                {
                                    tree.CommonStructure.@NamespaceInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "elementImport") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace.@elementImport = value;
                                        }
                                        if (name == "importedMember") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace.@importedMember = value;
                                        }
                                        if (name == "member") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace.@member = value;
                                        }
                                        if (name == "ownedMember") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace.@ownedMember = value;
                                        }
                                        if (name == "ownedRule") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace.@ownedRule = value;
                                        }
                                        if (name == "packageImport") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace.@packageImport = value;
                                        }
                                    }
                                }
                                if (name == "PackageableElement") // Looking for class
                                {
                                    tree.CommonStructure.@PackageableElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.PackageableElement.@visibility = value;
                                        }
                                    }
                                }
                                if (name == "PackageImport") // Looking for class
                                {
                                    tree.CommonStructure.@PackageImportInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "importedPackage") // Looking for property
                                        {
                                            tree.CommonStructure.PackageImport.@importedPackage = value;
                                        }
                                        if (name == "importingNamespace") // Looking for property
                                        {
                                            tree.CommonStructure.PackageImport.@importingNamespace = value;
                                        }
                                        if (name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.PackageImport.@visibility = value;
                                        }
                                    }
                                }
                                if (name == "ParameterableElement") // Looking for class
                                {
                                    tree.CommonStructure.@ParameterableElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "owningTemplateParameter") // Looking for property
                                        {
                                            tree.CommonStructure.ParameterableElement.@owningTemplateParameter = value;
                                        }
                                        if (name == "templateParameter") // Looking for property
                                        {
                                            tree.CommonStructure.ParameterableElement.@templateParameter = value;
                                        }
                                    }
                                }
                                if (name == "Realization") // Looking for class
                                {
                                    tree.CommonStructure.@RealizationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Relationship") // Looking for class
                                {
                                    tree.CommonStructure.@RelationshipInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "relatedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Relationship.@relatedElement = value;
                                        }
                                    }
                                }
                                if (name == "TemplateableElement") // Looking for class
                                {
                                    tree.CommonStructure.@TemplateableElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedTemplateSignature") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateableElement.@ownedTemplateSignature = value;
                                        }
                                        if (name == "templateBinding") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateableElement.@templateBinding = value;
                                        }
                                    }
                                }
                                if (name == "TemplateBinding") // Looking for class
                                {
                                    tree.CommonStructure.@TemplateBindingInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "boundElement") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateBinding.@boundElement = value;
                                        }
                                        if (name == "parameterSubstitution") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateBinding.@parameterSubstitution = value;
                                        }
                                        if (name == "signature") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateBinding.@signature = value;
                                        }
                                    }
                                }
                                if (name == "TemplateParameter") // Looking for class
                                {
                                    tree.CommonStructure.@TemplateParameterInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "default") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter.@default = value;
                                        }
                                        if (name == "ownedDefault") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter.@ownedDefault = value;
                                        }
                                        if (name == "ownedParameteredElement") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter.@ownedParameteredElement = value;
                                        }
                                        if (name == "parameteredElement") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter.@parameteredElement = value;
                                        }
                                        if (name == "signature") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter.@signature = value;
                                        }
                                    }
                                }
                                if (name == "TemplateParameterSubstitution") // Looking for class
                                {
                                    tree.CommonStructure.@TemplateParameterSubstitutionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "actual") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution.@actual = value;
                                        }
                                        if (name == "formal") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution.@formal = value;
                                        }
                                        if (name == "ownedActual") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution.@ownedActual = value;
                                        }
                                        if (name == "templateBinding") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution.@templateBinding = value;
                                        }
                                    }
                                }
                                if (name == "TemplateSignature") // Looking for class
                                {
                                    tree.CommonStructure.@TemplateSignatureInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "ownedParameter") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateSignature.@ownedParameter = value;
                                        }
                                        if (name == "parameter") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateSignature.@parameter = value;
                                        }
                                        if (name == "template") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateSignature.@template = value;
                                        }
                                    }
                                }
                                if (name == "Type") // Looking for class
                                {
                                    tree.CommonStructure.@TypeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "package") // Looking for property
                                        {
                                            tree.CommonStructure.Type.@package = value;
                                        }
                                    }
                                }
                                if (name == "TypedElement") // Looking for class
                                {
                                    tree.CommonStructure.@TypedElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "type") // Looking for property
                                        {
                                            tree.CommonStructure.TypedElement.@type = value;
                                        }
                                    }
                                }
                                if (name == "Usage") // Looking for class
                                {
                                    tree.CommonStructure.@UsageInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "CommonBehavior") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "AnyReceiveEvent") // Looking for class
                                {
                                    tree.CommonBehavior.@AnyReceiveEventInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Behavior") // Looking for class
                                {
                                    tree.CommonBehavior.@BehaviorInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "context") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@context = value;
                                        }
                                        if (name == "isReentrant") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@isReentrant = value;
                                        }
                                        if (name == "ownedParameter") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@ownedParameter = value;
                                        }
                                        if (name == "ownedParameterSet") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@ownedParameterSet = value;
                                        }
                                        if (name == "postcondition") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@postcondition = value;
                                        }
                                        if (name == "precondition") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@precondition = value;
                                        }
                                        if (name == "specification") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@specification = value;
                                        }
                                        if (name == "redefinedBehavior") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior.@redefinedBehavior = value;
                                        }
                                    }
                                }
                                if (name == "CallEvent") // Looking for class
                                {
                                    tree.CommonBehavior.@CallEventInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "operation") // Looking for property
                                        {
                                            tree.CommonBehavior.CallEvent.@operation = value;
                                        }
                                    }
                                }
                                if (name == "ChangeEvent") // Looking for class
                                {
                                    tree.CommonBehavior.@ChangeEventInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "changeExpression") // Looking for property
                                        {
                                            tree.CommonBehavior.ChangeEvent.@changeExpression = value;
                                        }
                                    }
                                }
                                if (name == "Event") // Looking for class
                                {
                                    tree.CommonBehavior.@EventInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "FunctionBehavior") // Looking for class
                                {
                                    tree.CommonBehavior.@FunctionBehaviorInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "MessageEvent") // Looking for class
                                {
                                    tree.CommonBehavior.@MessageEventInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "OpaqueBehavior") // Looking for class
                                {
                                    tree.CommonBehavior.@OpaqueBehaviorInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "body") // Looking for property
                                        {
                                            tree.CommonBehavior.OpaqueBehavior.@body = value;
                                        }
                                        if (name == "language") // Looking for property
                                        {
                                            tree.CommonBehavior.OpaqueBehavior.@language = value;
                                        }
                                    }
                                }
                                if (name == "SignalEvent") // Looking for class
                                {
                                    tree.CommonBehavior.@SignalEventInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "signal") // Looking for property
                                        {
                                            tree.CommonBehavior.SignalEvent.@signal = value;
                                        }
                                    }
                                }
                                if (name == "TimeEvent") // Looking for class
                                {
                                    tree.CommonBehavior.@TimeEventInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isRelative") // Looking for property
                                        {
                                            tree.CommonBehavior.TimeEvent.@isRelative = value;
                                        }
                                        if (name == "when") // Looking for property
                                        {
                                            tree.CommonBehavior.TimeEvent.@when = value;
                                        }
                                    }
                                }
                                if (name == "Trigger") // Looking for class
                                {
                                    tree.CommonBehavior.@TriggerInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "event") // Looking for property
                                        {
                                            tree.CommonBehavior.Trigger.@event = value;
                                        }
                                        if (name == "port") // Looking for property
                                        {
                                            tree.CommonBehavior.Trigger.@port = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Classification") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Substitution") // Looking for class
                                {
                                    tree.Classification.@SubstitutionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "contract") // Looking for property
                                        {
                                            tree.Classification.Substitution.@contract = value;
                                        }
                                        if (name == "substitutingClassifier") // Looking for property
                                        {
                                            tree.Classification.Substitution.@substitutingClassifier = value;
                                        }
                                    }
                                }
                                if (name == "BehavioralFeature") // Looking for class
                                {
                                    tree.Classification.@BehavioralFeatureInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "concurrency") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature.@concurrency = value;
                                        }
                                        if (name == "isAbstract") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature.@isAbstract = value;
                                        }
                                        if (name == "method") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature.@method = value;
                                        }
                                        if (name == "ownedParameter") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature.@ownedParameter = value;
                                        }
                                        if (name == "ownedParameterSet") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature.@ownedParameterSet = value;
                                        }
                                        if (name == "raisedException") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature.@raisedException = value;
                                        }
                                    }
                                }
                                if (name == "Classifier") // Looking for class
                                {
                                    tree.Classification.@ClassifierInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "attribute") // Looking for property
                                        {
                                            tree.Classification.Classifier.@attribute = value;
                                        }
                                        if (name == "collaborationUse") // Looking for property
                                        {
                                            tree.Classification.Classifier.@collaborationUse = value;
                                        }
                                        if (name == "feature") // Looking for property
                                        {
                                            tree.Classification.Classifier.@feature = value;
                                        }
                                        if (name == "general") // Looking for property
                                        {
                                            tree.Classification.Classifier.@general = value;
                                        }
                                        if (name == "generalization") // Looking for property
                                        {
                                            tree.Classification.Classifier.@generalization = value;
                                        }
                                        if (name == "inheritedMember") // Looking for property
                                        {
                                            tree.Classification.Classifier.@inheritedMember = value;
                                        }
                                        if (name == "isAbstract") // Looking for property
                                        {
                                            tree.Classification.Classifier.@isAbstract = value;
                                        }
                                        if (name == "isFinalSpecialization") // Looking for property
                                        {
                                            tree.Classification.Classifier.@isFinalSpecialization = value;
                                        }
                                        if (name == "ownedTemplateSignature") // Looking for property
                                        {
                                            tree.Classification.Classifier.@ownedTemplateSignature = value;
                                        }
                                        if (name == "ownedUseCase") // Looking for property
                                        {
                                            tree.Classification.Classifier.@ownedUseCase = value;
                                        }
                                        if (name == "powertypeExtent") // Looking for property
                                        {
                                            tree.Classification.Classifier.@powertypeExtent = value;
                                        }
                                        if (name == "redefinedClassifier") // Looking for property
                                        {
                                            tree.Classification.Classifier.@redefinedClassifier = value;
                                        }
                                        if (name == "representation") // Looking for property
                                        {
                                            tree.Classification.Classifier.@representation = value;
                                        }
                                        if (name == "substitution") // Looking for property
                                        {
                                            tree.Classification.Classifier.@substitution = value;
                                        }
                                        if (name == "templateParameter") // Looking for property
                                        {
                                            tree.Classification.Classifier.@templateParameter = value;
                                        }
                                        if (name == "useCase") // Looking for property
                                        {
                                            tree.Classification.Classifier.@useCase = value;
                                        }
                                    }
                                }
                                if (name == "ClassifierTemplateParameter") // Looking for class
                                {
                                    tree.Classification.@ClassifierTemplateParameterInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "allowSubstitutable") // Looking for property
                                        {
                                            tree.Classification.ClassifierTemplateParameter.@allowSubstitutable = value;
                                        }
                                        if (name == "constrainingClassifier") // Looking for property
                                        {
                                            tree.Classification.ClassifierTemplateParameter.@constrainingClassifier = value;
                                        }
                                        if (name == "parameteredElement") // Looking for property
                                        {
                                            tree.Classification.ClassifierTemplateParameter.@parameteredElement = value;
                                        }
                                    }
                                }
                                if (name == "Feature") // Looking for class
                                {
                                    tree.Classification.@FeatureInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "featuringClassifier") // Looking for property
                                        {
                                            tree.Classification.Feature.@featuringClassifier = value;
                                        }
                                        if (name == "isStatic") // Looking for property
                                        {
                                            tree.Classification.Feature.@isStatic = value;
                                        }
                                    }
                                }
                                if (name == "Generalization") // Looking for class
                                {
                                    tree.Classification.@GeneralizationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "general") // Looking for property
                                        {
                                            tree.Classification.Generalization.@general = value;
                                        }
                                        if (name == "generalizationSet") // Looking for property
                                        {
                                            tree.Classification.Generalization.@generalizationSet = value;
                                        }
                                        if (name == "isSubstitutable") // Looking for property
                                        {
                                            tree.Classification.Generalization.@isSubstitutable = value;
                                        }
                                        if (name == "specific") // Looking for property
                                        {
                                            tree.Classification.Generalization.@specific = value;
                                        }
                                    }
                                }
                                if (name == "GeneralizationSet") // Looking for class
                                {
                                    tree.Classification.@GeneralizationSetInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "generalization") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet.@generalization = value;
                                        }
                                        if (name == "isCovering") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet.@isCovering = value;
                                        }
                                        if (name == "isDisjoint") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet.@isDisjoint = value;
                                        }
                                        if (name == "powertype") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet.@powertype = value;
                                        }
                                    }
                                }
                                if (name == "InstanceSpecification") // Looking for class
                                {
                                    tree.Classification.@InstanceSpecificationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "classifier") // Looking for property
                                        {
                                            tree.Classification.InstanceSpecification.@classifier = value;
                                        }
                                        if (name == "slot") // Looking for property
                                        {
                                            tree.Classification.InstanceSpecification.@slot = value;
                                        }
                                        if (name == "specification") // Looking for property
                                        {
                                            tree.Classification.InstanceSpecification.@specification = value;
                                        }
                                    }
                                }
                                if (name == "InstanceValue") // Looking for class
                                {
                                    tree.Classification.@InstanceValueInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "instance") // Looking for property
                                        {
                                            tree.Classification.InstanceValue.@instance = value;
                                        }
                                    }
                                }
                                if (name == "Operation") // Looking for class
                                {
                                    tree.Classification.@OperationInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "bodyCondition") // Looking for property
                                        {
                                            tree.Classification.Operation.@bodyCondition = value;
                                        }
                                        if (name == "class") // Looking for property
                                        {
                                            tree.Classification.Operation.@class = value;
                                        }
                                        if (name == "datatype") // Looking for property
                                        {
                                            tree.Classification.Operation.@datatype = value;
                                        }
                                        if (name == "interface") // Looking for property
                                        {
                                            tree.Classification.Operation.@interface = value;
                                        }
                                        if (name == "isOrdered") // Looking for property
                                        {
                                            tree.Classification.Operation.@isOrdered = value;
                                        }
                                        if (name == "isQuery") // Looking for property
                                        {
                                            tree.Classification.Operation.@isQuery = value;
                                        }
                                        if (name == "isUnique") // Looking for property
                                        {
                                            tree.Classification.Operation.@isUnique = value;
                                        }
                                        if (name == "lower") // Looking for property
                                        {
                                            tree.Classification.Operation.@lower = value;
                                        }
                                        if (name == "ownedParameter") // Looking for property
                                        {
                                            tree.Classification.Operation.@ownedParameter = value;
                                        }
                                        if (name == "postcondition") // Looking for property
                                        {
                                            tree.Classification.Operation.@postcondition = value;
                                        }
                                        if (name == "precondition") // Looking for property
                                        {
                                            tree.Classification.Operation.@precondition = value;
                                        }
                                        if (name == "raisedException") // Looking for property
                                        {
                                            tree.Classification.Operation.@raisedException = value;
                                        }
                                        if (name == "redefinedOperation") // Looking for property
                                        {
                                            tree.Classification.Operation.@redefinedOperation = value;
                                        }
                                        if (name == "templateParameter") // Looking for property
                                        {
                                            tree.Classification.Operation.@templateParameter = value;
                                        }
                                        if (name == "type") // Looking for property
                                        {
                                            tree.Classification.Operation.@type = value;
                                        }
                                        if (name == "upper") // Looking for property
                                        {
                                            tree.Classification.Operation.@upper = value;
                                        }
                                    }
                                }
                                if (name == "OperationTemplateParameter") // Looking for class
                                {
                                    tree.Classification.@OperationTemplateParameterInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "parameteredElement") // Looking for property
                                        {
                                            tree.Classification.OperationTemplateParameter.@parameteredElement = value;
                                        }
                                    }
                                }
                                if (name == "Parameter") // Looking for class
                                {
                                    tree.Classification.@ParameterInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "default") // Looking for property
                                        {
                                            tree.Classification.Parameter.@default = value;
                                        }
                                        if (name == "defaultValue") // Looking for property
                                        {
                                            tree.Classification.Parameter.@defaultValue = value;
                                        }
                                        if (name == "direction") // Looking for property
                                        {
                                            tree.Classification.Parameter.@direction = value;
                                        }
                                        if (name == "effect") // Looking for property
                                        {
                                            tree.Classification.Parameter.@effect = value;
                                        }
                                        if (name == "isException") // Looking for property
                                        {
                                            tree.Classification.Parameter.@isException = value;
                                        }
                                        if (name == "isStream") // Looking for property
                                        {
                                            tree.Classification.Parameter.@isStream = value;
                                        }
                                        if (name == "operation") // Looking for property
                                        {
                                            tree.Classification.Parameter.@operation = value;
                                        }
                                        if (name == "parameterSet") // Looking for property
                                        {
                                            tree.Classification.Parameter.@parameterSet = value;
                                        }
                                    }
                                }
                                if (name == "ParameterSet") // Looking for class
                                {
                                    tree.Classification.@ParameterSetInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "condition") // Looking for property
                                        {
                                            tree.Classification.ParameterSet.@condition = value;
                                        }
                                        if (name == "parameter") // Looking for property
                                        {
                                            tree.Classification.ParameterSet.@parameter = value;
                                        }
                                    }
                                }
                                if (name == "Property") // Looking for class
                                {
                                    tree.Classification.@PropertyInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "aggregation") // Looking for property
                                        {
                                            tree.Classification.Property.@aggregation = value;
                                        }
                                        if (name == "association") // Looking for property
                                        {
                                            tree.Classification.Property.@association = value;
                                        }
                                        if (name == "associationEnd") // Looking for property
                                        {
                                            tree.Classification.Property.@associationEnd = value;
                                        }
                                        if (name == "class") // Looking for property
                                        {
                                            tree.Classification.Property.@class = value;
                                        }
                                        if (name == "datatype") // Looking for property
                                        {
                                            tree.Classification.Property.@datatype = value;
                                        }
                                        if (name == "defaultValue") // Looking for property
                                        {
                                            tree.Classification.Property.@defaultValue = value;
                                        }
                                        if (name == "interface") // Looking for property
                                        {
                                            tree.Classification.Property.@interface = value;
                                        }
                                        if (name == "isComposite") // Looking for property
                                        {
                                            tree.Classification.Property.@isComposite = value;
                                        }
                                        if (name == "isDerived") // Looking for property
                                        {
                                            tree.Classification.Property.@isDerived = value;
                                        }
                                        if (name == "isDerivedUnion") // Looking for property
                                        {
                                            tree.Classification.Property.@isDerivedUnion = value;
                                        }
                                        if (name == "isID") // Looking for property
                                        {
                                            tree.Classification.Property.@isID = value;
                                        }
                                        if (name == "opposite") // Looking for property
                                        {
                                            tree.Classification.Property.@opposite = value;
                                        }
                                        if (name == "owningAssociation") // Looking for property
                                        {
                                            tree.Classification.Property.@owningAssociation = value;
                                        }
                                        if (name == "qualifier") // Looking for property
                                        {
                                            tree.Classification.Property.@qualifier = value;
                                        }
                                        if (name == "redefinedProperty") // Looking for property
                                        {
                                            tree.Classification.Property.@redefinedProperty = value;
                                        }
                                        if (name == "subsettedProperty") // Looking for property
                                        {
                                            tree.Classification.Property.@subsettedProperty = value;
                                        }
                                    }
                                }
                                if (name == "RedefinableElement") // Looking for class
                                {
                                    tree.Classification.@RedefinableElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isLeaf") // Looking for property
                                        {
                                            tree.Classification.RedefinableElement.@isLeaf = value;
                                        }
                                        if (name == "redefinedElement") // Looking for property
                                        {
                                            tree.Classification.RedefinableElement.@redefinedElement = value;
                                        }
                                        if (name == "redefinitionContext") // Looking for property
                                        {
                                            tree.Classification.RedefinableElement.@redefinitionContext = value;
                                        }
                                    }
                                }
                                if (name == "RedefinableTemplateSignature") // Looking for class
                                {
                                    tree.Classification.@RedefinableTemplateSignatureInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "classifier") // Looking for property
                                        {
                                            tree.Classification.RedefinableTemplateSignature.@classifier = value;
                                        }
                                        if (name == "extendedSignature") // Looking for property
                                        {
                                            tree.Classification.RedefinableTemplateSignature.@extendedSignature = value;
                                        }
                                        if (name == "inheritedParameter") // Looking for property
                                        {
                                            tree.Classification.RedefinableTemplateSignature.@inheritedParameter = value;
                                        }
                                    }
                                }
                                if (name == "Slot") // Looking for class
                                {
                                    tree.Classification.@SlotInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "definingFeature") // Looking for property
                                        {
                                            tree.Classification.Slot.@definingFeature = value;
                                        }
                                        if (name == "owningInstance") // Looking for property
                                        {
                                            tree.Classification.Slot.@owningInstance = value;
                                        }
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Classification.Slot.@value = value;
                                        }
                                    }
                                }
                                if (name == "StructuralFeature") // Looking for class
                                {
                                    tree.Classification.@StructuralFeatureInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isReadOnly") // Looking for property
                                        {
                                            tree.Classification.StructuralFeature.@isReadOnly = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Actions") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "ValueSpecificationAction") // Looking for class
                                {
                                    tree.Actions.@ValueSpecificationActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ValueSpecificationAction.@result = value;
                                        }
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Actions.ValueSpecificationAction.@value = value;
                                        }
                                    }
                                }
                                if (name == "VariableAction") // Looking for class
                                {
                                    tree.Actions.@VariableActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "variable") // Looking for property
                                        {
                                            tree.Actions.VariableAction.@variable = value;
                                        }
                                    }
                                }
                                if (name == "WriteLinkAction") // Looking for class
                                {
                                    tree.Actions.@WriteLinkActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "WriteStructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.@WriteStructuralFeatureActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.WriteStructuralFeatureAction.@result = value;
                                        }
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Actions.WriteStructuralFeatureAction.@value = value;
                                        }
                                    }
                                }
                                if (name == "WriteVariableAction") // Looking for class
                                {
                                    tree.Actions.@WriteVariableActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Actions.WriteVariableAction.@value = value;
                                        }
                                    }
                                }
                                if (name == "AcceptCallAction") // Looking for class
                                {
                                    tree.Actions.@AcceptCallActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "returnInformation") // Looking for property
                                        {
                                            tree.Actions.AcceptCallAction.@returnInformation = value;
                                        }
                                    }
                                }
                                if (name == "AcceptEventAction") // Looking for class
                                {
                                    tree.Actions.@AcceptEventActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isUnmarshall") // Looking for property
                                        {
                                            tree.Actions.AcceptEventAction.@isUnmarshall = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.AcceptEventAction.@result = value;
                                        }
                                        if (name == "trigger") // Looking for property
                                        {
                                            tree.Actions.AcceptEventAction.@trigger = value;
                                        }
                                    }
                                }
                                if (name == "Action") // Looking for class
                                {
                                    tree.Actions.@ActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "context") // Looking for property
                                        {
                                            tree.Actions.Action.@context = value;
                                        }
                                        if (name == "input") // Looking for property
                                        {
                                            tree.Actions.Action.@input = value;
                                        }
                                        if (name == "isLocallyReentrant") // Looking for property
                                        {
                                            tree.Actions.Action.@isLocallyReentrant = value;
                                        }
                                        if (name == "localPostcondition") // Looking for property
                                        {
                                            tree.Actions.Action.@localPostcondition = value;
                                        }
                                        if (name == "localPrecondition") // Looking for property
                                        {
                                            tree.Actions.Action.@localPrecondition = value;
                                        }
                                        if (name == "output") // Looking for property
                                        {
                                            tree.Actions.Action.@output = value;
                                        }
                                    }
                                }
                                if (name == "ActionInputPin") // Looking for class
                                {
                                    tree.Actions.@ActionInputPinInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "fromAction") // Looking for property
                                        {
                                            tree.Actions.ActionInputPin.@fromAction = value;
                                        }
                                    }
                                }
                                if (name == "AddStructuralFeatureValueAction") // Looking for class
                                {
                                    tree.Actions.@AddStructuralFeatureValueActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "insertAt") // Looking for property
                                        {
                                            tree.Actions.AddStructuralFeatureValueAction.@insertAt = value;
                                        }
                                        if (name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.AddStructuralFeatureValueAction.@isReplaceAll = value;
                                        }
                                    }
                                }
                                if (name == "AddVariableValueAction") // Looking for class
                                {
                                    tree.Actions.@AddVariableValueActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "insertAt") // Looking for property
                                        {
                                            tree.Actions.AddVariableValueAction.@insertAt = value;
                                        }
                                        if (name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.AddVariableValueAction.@isReplaceAll = value;
                                        }
                                    }
                                }
                                if (name == "BroadcastSignalAction") // Looking for class
                                {
                                    tree.Actions.@BroadcastSignalActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "signal") // Looking for property
                                        {
                                            tree.Actions.BroadcastSignalAction.@signal = value;
                                        }
                                    }
                                }
                                if (name == "CallAction") // Looking for class
                                {
                                    tree.Actions.@CallActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isSynchronous") // Looking for property
                                        {
                                            tree.Actions.CallAction.@isSynchronous = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.CallAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "CallBehaviorAction") // Looking for class
                                {
                                    tree.Actions.@CallBehaviorActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "behavior") // Looking for property
                                        {
                                            tree.Actions.CallBehaviorAction.@behavior = value;
                                        }
                                    }
                                }
                                if (name == "CallOperationAction") // Looking for class
                                {
                                    tree.Actions.@CallOperationActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "operation") // Looking for property
                                        {
                                            tree.Actions.CallOperationAction.@operation = value;
                                        }
                                        if (name == "target") // Looking for property
                                        {
                                            tree.Actions.CallOperationAction.@target = value;
                                        }
                                    }
                                }
                                if (name == "Clause") // Looking for class
                                {
                                    tree.Actions.@ClauseInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "body") // Looking for property
                                        {
                                            tree.Actions.Clause.@body = value;
                                        }
                                        if (name == "bodyOutput") // Looking for property
                                        {
                                            tree.Actions.Clause.@bodyOutput = value;
                                        }
                                        if (name == "decider") // Looking for property
                                        {
                                            tree.Actions.Clause.@decider = value;
                                        }
                                        if (name == "predecessorClause") // Looking for property
                                        {
                                            tree.Actions.Clause.@predecessorClause = value;
                                        }
                                        if (name == "successorClause") // Looking for property
                                        {
                                            tree.Actions.Clause.@successorClause = value;
                                        }
                                        if (name == "test") // Looking for property
                                        {
                                            tree.Actions.Clause.@test = value;
                                        }
                                    }
                                }
                                if (name == "ClearAssociationAction") // Looking for class
                                {
                                    tree.Actions.@ClearAssociationActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "association") // Looking for property
                                        {
                                            tree.Actions.ClearAssociationAction.@association = value;
                                        }
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.ClearAssociationAction.@object = value;
                                        }
                                    }
                                }
                                if (name == "ClearStructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.@ClearStructuralFeatureActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ClearStructuralFeatureAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ClearVariableAction") // Looking for class
                                {
                                    tree.Actions.@ClearVariableActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ConditionalNode") // Looking for class
                                {
                                    tree.Actions.@ConditionalNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "clause") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode.@clause = value;
                                        }
                                        if (name == "isAssured") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode.@isAssured = value;
                                        }
                                        if (name == "isDeterminate") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode.@isDeterminate = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode.@result = value;
                                        }
                                    }
                                }
                                if (name == "CreateLinkAction") // Looking for class
                                {
                                    tree.Actions.@CreateLinkActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "endData") // Looking for property
                                        {
                                            tree.Actions.CreateLinkAction.@endData = value;
                                        }
                                    }
                                }
                                if (name == "CreateLinkObjectAction") // Looking for class
                                {
                                    tree.Actions.@CreateLinkObjectActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.CreateLinkObjectAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "CreateObjectAction") // Looking for class
                                {
                                    tree.Actions.@CreateObjectActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "classifier") // Looking for property
                                        {
                                            tree.Actions.CreateObjectAction.@classifier = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.CreateObjectAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "DestroyLinkAction") // Looking for class
                                {
                                    tree.Actions.@DestroyLinkActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "endData") // Looking for property
                                        {
                                            tree.Actions.DestroyLinkAction.@endData = value;
                                        }
                                    }
                                }
                                if (name == "DestroyObjectAction") // Looking for class
                                {
                                    tree.Actions.@DestroyObjectActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isDestroyLinks") // Looking for property
                                        {
                                            tree.Actions.DestroyObjectAction.@isDestroyLinks = value;
                                        }
                                        if (name == "isDestroyOwnedObjects") // Looking for property
                                        {
                                            tree.Actions.DestroyObjectAction.@isDestroyOwnedObjects = value;
                                        }
                                        if (name == "target") // Looking for property
                                        {
                                            tree.Actions.DestroyObjectAction.@target = value;
                                        }
                                    }
                                }
                                if (name == "ExpansionNode") // Looking for class
                                {
                                    tree.Actions.@ExpansionNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "regionAsInput") // Looking for property
                                        {
                                            tree.Actions.ExpansionNode.@regionAsInput = value;
                                        }
                                        if (name == "regionAsOutput") // Looking for property
                                        {
                                            tree.Actions.ExpansionNode.@regionAsOutput = value;
                                        }
                                    }
                                }
                                if (name == "ExpansionRegion") // Looking for class
                                {
                                    tree.Actions.@ExpansionRegionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "inputElement") // Looking for property
                                        {
                                            tree.Actions.ExpansionRegion.@inputElement = value;
                                        }
                                        if (name == "mode") // Looking for property
                                        {
                                            tree.Actions.ExpansionRegion.@mode = value;
                                        }
                                        if (name == "outputElement") // Looking for property
                                        {
                                            tree.Actions.ExpansionRegion.@outputElement = value;
                                        }
                                    }
                                }
                                if (name == "InputPin") // Looking for class
                                {
                                    tree.Actions.@InputPinInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "InvocationAction") // Looking for class
                                {
                                    tree.Actions.@InvocationActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "argument") // Looking for property
                                        {
                                            tree.Actions.InvocationAction.@argument = value;
                                        }
                                        if (name == "onPort") // Looking for property
                                        {
                                            tree.Actions.InvocationAction.@onPort = value;
                                        }
                                    }
                                }
                                if (name == "LinkAction") // Looking for class
                                {
                                    tree.Actions.@LinkActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "endData") // Looking for property
                                        {
                                            tree.Actions.LinkAction.@endData = value;
                                        }
                                        if (name == "inputValue") // Looking for property
                                        {
                                            tree.Actions.LinkAction.@inputValue = value;
                                        }
                                    }
                                }
                                if (name == "LinkEndCreationData") // Looking for class
                                {
                                    tree.Actions.@LinkEndCreationDataInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "insertAt") // Looking for property
                                        {
                                            tree.Actions.LinkEndCreationData.@insertAt = value;
                                        }
                                        if (name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.LinkEndCreationData.@isReplaceAll = value;
                                        }
                                    }
                                }
                                if (name == "LinkEndData") // Looking for class
                                {
                                    tree.Actions.@LinkEndDataInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "end") // Looking for property
                                        {
                                            tree.Actions.LinkEndData.@end = value;
                                        }
                                        if (name == "qualifier") // Looking for property
                                        {
                                            tree.Actions.LinkEndData.@qualifier = value;
                                        }
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Actions.LinkEndData.@value = value;
                                        }
                                    }
                                }
                                if (name == "LinkEndDestructionData") // Looking for class
                                {
                                    tree.Actions.@LinkEndDestructionDataInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "destroyAt") // Looking for property
                                        {
                                            tree.Actions.LinkEndDestructionData.@destroyAt = value;
                                        }
                                        if (name == "isDestroyDuplicates") // Looking for property
                                        {
                                            tree.Actions.LinkEndDestructionData.@isDestroyDuplicates = value;
                                        }
                                    }
                                }
                                if (name == "LoopNode") // Looking for class
                                {
                                    tree.Actions.@LoopNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "bodyOutput") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@bodyOutput = value;
                                        }
                                        if (name == "bodyPart") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@bodyPart = value;
                                        }
                                        if (name == "decider") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@decider = value;
                                        }
                                        if (name == "isTestedFirst") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@isTestedFirst = value;
                                        }
                                        if (name == "loopVariable") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@loopVariable = value;
                                        }
                                        if (name == "loopVariableInput") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@loopVariableInput = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@result = value;
                                        }
                                        if (name == "setupPart") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@setupPart = value;
                                        }
                                        if (name == "test") // Looking for property
                                        {
                                            tree.Actions.LoopNode.@test = value;
                                        }
                                    }
                                }
                                if (name == "OpaqueAction") // Looking for class
                                {
                                    tree.Actions.@OpaqueActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "body") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction.@body = value;
                                        }
                                        if (name == "inputValue") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction.@inputValue = value;
                                        }
                                        if (name == "language") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction.@language = value;
                                        }
                                        if (name == "outputValue") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction.@outputValue = value;
                                        }
                                    }
                                }
                                if (name == "OutputPin") // Looking for class
                                {
                                    tree.Actions.@OutputPinInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Pin") // Looking for class
                                {
                                    tree.Actions.@PinInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isControl") // Looking for property
                                        {
                                            tree.Actions.Pin.@isControl = value;
                                        }
                                    }
                                }
                                if (name == "QualifierValue") // Looking for class
                                {
                                    tree.Actions.@QualifierValueInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "qualifier") // Looking for property
                                        {
                                            tree.Actions.QualifierValue.@qualifier = value;
                                        }
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Actions.QualifierValue.@value = value;
                                        }
                                    }
                                }
                                if (name == "RaiseExceptionAction") // Looking for class
                                {
                                    tree.Actions.@RaiseExceptionActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "exception") // Looking for property
                                        {
                                            tree.Actions.RaiseExceptionAction.@exception = value;
                                        }
                                    }
                                }
                                if (name == "ReadExtentAction") // Looking for class
                                {
                                    tree.Actions.@ReadExtentActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "classifier") // Looking for property
                                        {
                                            tree.Actions.ReadExtentAction.@classifier = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadExtentAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReadIsClassifiedObjectAction") // Looking for class
                                {
                                    tree.Actions.@ReadIsClassifiedObjectActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "classifier") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction.@classifier = value;
                                        }
                                        if (name == "isDirect") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction.@isDirect = value;
                                        }
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction.@object = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReadLinkAction") // Looking for class
                                {
                                    tree.Actions.@ReadLinkActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadLinkAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReadLinkObjectEndAction") // Looking for class
                                {
                                    tree.Actions.@ReadLinkObjectEndActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "end") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndAction.@end = value;
                                        }
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndAction.@object = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReadLinkObjectEndQualifierAction") // Looking for class
                                {
                                    tree.Actions.@ReadLinkObjectEndQualifierActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndQualifierAction.@object = value;
                                        }
                                        if (name == "qualifier") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndQualifierAction.@qualifier = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndQualifierAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReadSelfAction") // Looking for class
                                {
                                    tree.Actions.@ReadSelfActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadSelfAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReadStructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.@ReadStructuralFeatureActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadStructuralFeatureAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReadVariableAction") // Looking for class
                                {
                                    tree.Actions.@ReadVariableActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadVariableAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "ReclassifyObjectAction") // Looking for class
                                {
                                    tree.Actions.@ReclassifyObjectActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction.@isReplaceAll = value;
                                        }
                                        if (name == "newClassifier") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction.@newClassifier = value;
                                        }
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction.@object = value;
                                        }
                                        if (name == "oldClassifier") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction.@oldClassifier = value;
                                        }
                                    }
                                }
                                if (name == "ReduceAction") // Looking for class
                                {
                                    tree.Actions.@ReduceActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "collection") // Looking for property
                                        {
                                            tree.Actions.ReduceAction.@collection = value;
                                        }
                                        if (name == "isOrdered") // Looking for property
                                        {
                                            tree.Actions.ReduceAction.@isOrdered = value;
                                        }
                                        if (name == "reducer") // Looking for property
                                        {
                                            tree.Actions.ReduceAction.@reducer = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.ReduceAction.@result = value;
                                        }
                                    }
                                }
                                if (name == "RemoveStructuralFeatureValueAction") // Looking for class
                                {
                                    tree.Actions.@RemoveStructuralFeatureValueActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isRemoveDuplicates") // Looking for property
                                        {
                                            tree.Actions.RemoveStructuralFeatureValueAction.@isRemoveDuplicates = value;
                                        }
                                        if (name == "removeAt") // Looking for property
                                        {
                                            tree.Actions.RemoveStructuralFeatureValueAction.@removeAt = value;
                                        }
                                    }
                                }
                                if (name == "RemoveVariableValueAction") // Looking for class
                                {
                                    tree.Actions.@RemoveVariableValueActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "isRemoveDuplicates") // Looking for property
                                        {
                                            tree.Actions.RemoveVariableValueAction.@isRemoveDuplicates = value;
                                        }
                                        if (name == "removeAt") // Looking for property
                                        {
                                            tree.Actions.RemoveVariableValueAction.@removeAt = value;
                                        }
                                    }
                                }
                                if (name == "ReplyAction") // Looking for class
                                {
                                    tree.Actions.@ReplyActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "replyToCall") // Looking for property
                                        {
                                            tree.Actions.ReplyAction.@replyToCall = value;
                                        }
                                        if (name == "replyValue") // Looking for property
                                        {
                                            tree.Actions.ReplyAction.@replyValue = value;
                                        }
                                        if (name == "returnInformation") // Looking for property
                                        {
                                            tree.Actions.ReplyAction.@returnInformation = value;
                                        }
                                    }
                                }
                                if (name == "SendObjectAction") // Looking for class
                                {
                                    tree.Actions.@SendObjectActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "request") // Looking for property
                                        {
                                            tree.Actions.SendObjectAction.@request = value;
                                        }
                                        if (name == "target") // Looking for property
                                        {
                                            tree.Actions.SendObjectAction.@target = value;
                                        }
                                    }
                                }
                                if (name == "SendSignalAction") // Looking for class
                                {
                                    tree.Actions.@SendSignalActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "signal") // Looking for property
                                        {
                                            tree.Actions.SendSignalAction.@signal = value;
                                        }
                                        if (name == "target") // Looking for property
                                        {
                                            tree.Actions.SendSignalAction.@target = value;
                                        }
                                    }
                                }
                                if (name == "SequenceNode") // Looking for class
                                {
                                    tree.Actions.@SequenceNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "executableNode") // Looking for property
                                        {
                                            tree.Actions.SequenceNode.@executableNode = value;
                                        }
                                    }
                                }
                                if (name == "StartClassifierBehaviorAction") // Looking for class
                                {
                                    tree.Actions.@StartClassifierBehaviorActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.StartClassifierBehaviorAction.@object = value;
                                        }
                                    }
                                }
                                if (name == "StartObjectBehaviorAction") // Looking for class
                                {
                                    tree.Actions.@StartObjectBehaviorActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.StartObjectBehaviorAction.@object = value;
                                        }
                                    }
                                }
                                if (name == "StructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.@StructuralFeatureActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.StructuralFeatureAction.@object = value;
                                        }
                                        if (name == "structuralFeature") // Looking for property
                                        {
                                            tree.Actions.StructuralFeatureAction.@structuralFeature = value;
                                        }
                                    }
                                }
                                if (name == "StructuredActivityNode") // Looking for class
                                {
                                    tree.Actions.@StructuredActivityNodeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "activity") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode.@activity = value;
                                        }
                                        if (name == "edge") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode.@edge = value;
                                        }
                                        if (name == "mustIsolate") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode.@mustIsolate = value;
                                        }
                                        if (name == "node") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode.@node = value;
                                        }
                                        if (name == "structuredNodeInput") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode.@structuredNodeInput = value;
                                        }
                                        if (name == "structuredNodeOutput") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode.@structuredNodeOutput = value;
                                        }
                                        if (name == "variable") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode.@variable = value;
                                        }
                                    }
                                }
                                if (name == "TestIdentityAction") // Looking for class
                                {
                                    tree.Actions.@TestIdentityActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "first") // Looking for property
                                        {
                                            tree.Actions.TestIdentityAction.@first = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.TestIdentityAction.@result = value;
                                        }
                                        if (name == "second") // Looking for property
                                        {
                                            tree.Actions.TestIdentityAction.@second = value;
                                        }
                                    }
                                }
                                if (name == "UnmarshallAction") // Looking for class
                                {
                                    tree.Actions.@UnmarshallActionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "object") // Looking for property
                                        {
                                            tree.Actions.UnmarshallAction.@object = value;
                                        }
                                        if (name == "result") // Looking for property
                                        {
                                            tree.Actions.UnmarshallAction.@result = value;
                                        }
                                        if (name == "unmarshallType") // Looking for property
                                        {
                                            tree.Actions.UnmarshallAction.@unmarshallType = value;
                                        }
                                    }
                                }
                                if (name == "ValuePin") // Looking for class
                                {
                                    tree.Actions.@ValuePinInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Actions.ValuePin.@value = value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
