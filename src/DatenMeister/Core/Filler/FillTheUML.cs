#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Core.Filler
{
    public class FillTheUML : DatenMeister.Core.Filler.IFiller<DatenMeister.Core._UML>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject? element)
        {
            if (element == null) throw new System.ArgumentNullException(nameof(element));
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object?> collection, DatenMeister.Core._UML tree)
        {
            FillTheUML.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object?> collection, DatenMeister.Core._UML tree)
        {
            string? name;
            IElement? value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement ?? throw new System.InvalidOperationException("value == null");
                name = GetNameOfElement(value);
                if (name == "UML") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement ?? throw new System.InvalidOperationException("value == null");
                        name = GetNameOfElement(value);
                        if (name == "Activities") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Activity") // Looking for class
                                {
                                    tree.Activities.__Activity = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "edge") // Looking for property
                                        {
                                            tree.Activities.Activity._edge = value;
                                        }
                                        if(name == "group") // Looking for property
                                        {
                                            tree.Activities.Activity._group = value;
                                        }
                                        if(name == "isReadOnly") // Looking for property
                                        {
                                            tree.Activities.Activity._isReadOnly = value;
                                        }
                                        if(name == "isSingleExecution") // Looking for property
                                        {
                                            tree.Activities.Activity._isSingleExecution = value;
                                        }
                                        if(name == "node") // Looking for property
                                        {
                                            tree.Activities.Activity._node = value;
                                        }
                                        if(name == "partition") // Looking for property
                                        {
                                            tree.Activities.Activity._partition = value;
                                        }
                                        if(name == "structuredNode") // Looking for property
                                        {
                                            tree.Activities.Activity._structuredNode = value;
                                        }
                                        if(name == "variable") // Looking for property
                                        {
                                            tree.Activities.Activity._variable = value;
                                        }
                                    }
                                }
                                if(name == "ActivityEdge") // Looking for class
                                {
                                    tree.Activities.__ActivityEdge = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "activity") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._activity = value;
                                        }
                                        if(name == "guard") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._guard = value;
                                        }
                                        if(name == "inGroup") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._inGroup = value;
                                        }
                                        if(name == "inPartition") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._inPartition = value;
                                        }
                                        if(name == "inStructuredNode") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._inStructuredNode = value;
                                        }
                                        if(name == "interrupts") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._interrupts = value;
                                        }
                                        if(name == "redefinedEdge") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._redefinedEdge = value;
                                        }
                                        if(name == "source") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._source = value;
                                        }
                                        if(name == "target") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._target = value;
                                        }
                                        if(name == "weight") // Looking for property
                                        {
                                            tree.Activities.ActivityEdge._weight = value;
                                        }
                                    }
                                }
                                if(name == "ActivityFinalNode") // Looking for class
                                {
                                    tree.Activities.__ActivityFinalNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ActivityGroup") // Looking for class
                                {
                                    tree.Activities.__ActivityGroup = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "containedEdge") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup._containedEdge = value;
                                        }
                                        if(name == "containedNode") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup._containedNode = value;
                                        }
                                        if(name == "inActivity") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup._inActivity = value;
                                        }
                                        if(name == "subgroup") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup._subgroup = value;
                                        }
                                        if(name == "superGroup") // Looking for property
                                        {
                                            tree.Activities.ActivityGroup._superGroup = value;
                                        }
                                    }
                                }
                                if(name == "ActivityNode") // Looking for class
                                {
                                    tree.Activities.__ActivityNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "activity") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._activity = value;
                                        }
                                        if(name == "inGroup") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._inGroup = value;
                                        }
                                        if(name == "inInterruptibleRegion") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._inInterruptibleRegion = value;
                                        }
                                        if(name == "inPartition") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._inPartition = value;
                                        }
                                        if(name == "inStructuredNode") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._inStructuredNode = value;
                                        }
                                        if(name == "incoming") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._incoming = value;
                                        }
                                        if(name == "outgoing") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._outgoing = value;
                                        }
                                        if(name == "redefinedNode") // Looking for property
                                        {
                                            tree.Activities.ActivityNode._redefinedNode = value;
                                        }
                                    }
                                }
                                if(name == "ActivityParameterNode") // Looking for class
                                {
                                    tree.Activities.__ActivityParameterNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "parameter") // Looking for property
                                        {
                                            tree.Activities.ActivityParameterNode._parameter = value;
                                        }
                                    }
                                }
                                if(name == "ActivityPartition") // Looking for class
                                {
                                    tree.Activities.__ActivityPartition = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "edge") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition._edge = value;
                                        }
                                        if(name == "isDimension") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition._isDimension = value;
                                        }
                                        if(name == "isExternal") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition._isExternal = value;
                                        }
                                        if(name == "node") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition._node = value;
                                        }
                                        if(name == "represents") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition._represents = value;
                                        }
                                        if(name == "subpartition") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition._subpartition = value;
                                        }
                                        if(name == "superPartition") // Looking for property
                                        {
                                            tree.Activities.ActivityPartition._superPartition = value;
                                        }
                                    }
                                }
                                if(name == "CentralBufferNode") // Looking for class
                                {
                                    tree.Activities.__CentralBufferNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ControlFlow") // Looking for class
                                {
                                    tree.Activities.__ControlFlow = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ControlNode") // Looking for class
                                {
                                    tree.Activities.__ControlNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "DataStoreNode") // Looking for class
                                {
                                    tree.Activities.__DataStoreNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "DecisionNode") // Looking for class
                                {
                                    tree.Activities.__DecisionNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "decisionInput") // Looking for property
                                        {
                                            tree.Activities.DecisionNode._decisionInput = value;
                                        }
                                        if(name == "decisionInputFlow") // Looking for property
                                        {
                                            tree.Activities.DecisionNode._decisionInputFlow = value;
                                        }
                                    }
                                }
                                if(name == "ExceptionHandler") // Looking for class
                                {
                                    tree.Activities.__ExceptionHandler = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "exceptionInput") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler._exceptionInput = value;
                                        }
                                        if(name == "exceptionType") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler._exceptionType = value;
                                        }
                                        if(name == "handlerBody") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler._handlerBody = value;
                                        }
                                        if(name == "protectedNode") // Looking for property
                                        {
                                            tree.Activities.ExceptionHandler._protectedNode = value;
                                        }
                                    }
                                }
                                if(name == "ExecutableNode") // Looking for class
                                {
                                    tree.Activities.__ExecutableNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "handler") // Looking for property
                                        {
                                            tree.Activities.ExecutableNode._handler = value;
                                        }
                                    }
                                }
                                if(name == "FinalNode") // Looking for class
                                {
                                    tree.Activities.__FinalNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "FlowFinalNode") // Looking for class
                                {
                                    tree.Activities.__FlowFinalNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ForkNode") // Looking for class
                                {
                                    tree.Activities.__ForkNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "InitialNode") // Looking for class
                                {
                                    tree.Activities.__InitialNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "InterruptibleActivityRegion") // Looking for class
                                {
                                    tree.Activities.__InterruptibleActivityRegion = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "interruptingEdge") // Looking for property
                                        {
                                            tree.Activities.InterruptibleActivityRegion._interruptingEdge = value;
                                        }
                                        if(name == "node") // Looking for property
                                        {
                                            tree.Activities.InterruptibleActivityRegion._node = value;
                                        }
                                    }
                                }
                                if(name == "JoinNode") // Looking for class
                                {
                                    tree.Activities.__JoinNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isCombineDuplicate") // Looking for property
                                        {
                                            tree.Activities.JoinNode._isCombineDuplicate = value;
                                        }
                                        if(name == "joinSpec") // Looking for property
                                        {
                                            tree.Activities.JoinNode._joinSpec = value;
                                        }
                                    }
                                }
                                if(name == "MergeNode") // Looking for class
                                {
                                    tree.Activities.__MergeNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ObjectFlow") // Looking for class
                                {
                                    tree.Activities.__ObjectFlow = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isMulticast") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow._isMulticast = value;
                                        }
                                        if(name == "isMultireceive") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow._isMultireceive = value;
                                        }
                                        if(name == "selection") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow._selection = value;
                                        }
                                        if(name == "transformation") // Looking for property
                                        {
                                            tree.Activities.ObjectFlow._transformation = value;
                                        }
                                    }
                                }
                                if(name == "ObjectNode") // Looking for class
                                {
                                    tree.Activities.__ObjectNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "inState") // Looking for property
                                        {
                                            tree.Activities.ObjectNode._inState = value;
                                        }
                                        if(name == "isControlType") // Looking for property
                                        {
                                            tree.Activities.ObjectNode._isControlType = value;
                                        }
                                        if(name == "ordering") // Looking for property
                                        {
                                            tree.Activities.ObjectNode._ordering = value;
                                        }
                                        if(name == "selection") // Looking for property
                                        {
                                            tree.Activities.ObjectNode._selection = value;
                                        }
                                        if(name == "upperBound") // Looking for property
                                        {
                                            tree.Activities.ObjectNode._upperBound = value;
                                        }
                                    }
                                }
                                if(name == "Variable") // Looking for class
                                {
                                    tree.Activities.__Variable = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "activityScope") // Looking for property
                                        {
                                            tree.Activities.Variable._activityScope = value;
                                        }
                                        if(name == "scope") // Looking for property
                                        {
                                            tree.Activities.Variable._scope = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Values") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Duration") // Looking for class
                                {
                                    tree.Values.__Duration = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "expr") // Looking for property
                                        {
                                            tree.Values.Duration._expr = value;
                                        }
                                        if(name == "observation") // Looking for property
                                        {
                                            tree.Values.Duration._observation = value;
                                        }
                                    }
                                }
                                if(name == "DurationConstraint") // Looking for class
                                {
                                    tree.Values.__DurationConstraint = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.DurationConstraint._firstEvent = value;
                                        }
                                        if(name == "specification") // Looking for property
                                        {
                                            tree.Values.DurationConstraint._specification = value;
                                        }
                                    }
                                }
                                if(name == "DurationInterval") // Looking for class
                                {
                                    tree.Values.__DurationInterval = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "max") // Looking for property
                                        {
                                            tree.Values.DurationInterval._max = value;
                                        }
                                        if(name == "min") // Looking for property
                                        {
                                            tree.Values.DurationInterval._min = value;
                                        }
                                    }
                                }
                                if(name == "DurationObservation") // Looking for class
                                {
                                    tree.Values.__DurationObservation = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "event") // Looking for property
                                        {
                                            tree.Values.DurationObservation._event = value;
                                        }
                                        if(name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.DurationObservation._firstEvent = value;
                                        }
                                    }
                                }
                                if(name == "Expression") // Looking for class
                                {
                                    tree.Values.__Expression = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "operand") // Looking for property
                                        {
                                            tree.Values.Expression._operand = value;
                                        }
                                        if(name == "symbol") // Looking for property
                                        {
                                            tree.Values.Expression._symbol = value;
                                        }
                                    }
                                }
                                if(name == "Interval") // Looking for class
                                {
                                    tree.Values.__Interval = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "max") // Looking for property
                                        {
                                            tree.Values.Interval._max = value;
                                        }
                                        if(name == "min") // Looking for property
                                        {
                                            tree.Values.Interval._min = value;
                                        }
                                    }
                                }
                                if(name == "IntervalConstraint") // Looking for class
                                {
                                    tree.Values.__IntervalConstraint = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "specification") // Looking for property
                                        {
                                            tree.Values.IntervalConstraint._specification = value;
                                        }
                                    }
                                }
                                if(name == "LiteralBoolean") // Looking for class
                                {
                                    tree.Values.__LiteralBoolean = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralBoolean._value = value;
                                        }
                                    }
                                }
                                if(name == "LiteralInteger") // Looking for class
                                {
                                    tree.Values.__LiteralInteger = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralInteger._value = value;
                                        }
                                    }
                                }
                                if(name == "LiteralNull") // Looking for class
                                {
                                    tree.Values.__LiteralNull = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "LiteralReal") // Looking for class
                                {
                                    tree.Values.__LiteralReal = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralReal._value = value;
                                        }
                                    }
                                }
                                if(name == "LiteralSpecification") // Looking for class
                                {
                                    tree.Values.__LiteralSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "LiteralString") // Looking for class
                                {
                                    tree.Values.__LiteralString = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralString._value = value;
                                        }
                                    }
                                }
                                if(name == "LiteralUnlimitedNatural") // Looking for class
                                {
                                    tree.Values.__LiteralUnlimitedNatural = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Values.LiteralUnlimitedNatural._value = value;
                                        }
                                    }
                                }
                                if(name == "Observation") // Looking for class
                                {
                                    tree.Values.__Observation = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "OpaqueExpression") // Looking for class
                                {
                                    tree.Values.__OpaqueExpression = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "behavior") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression._behavior = value;
                                        }
                                        if(name == "body") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression._body = value;
                                        }
                                        if(name == "language") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression._language = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Values.OpaqueExpression._result = value;
                                        }
                                    }
                                }
                                if(name == "StringExpression") // Looking for class
                                {
                                    tree.Values.__StringExpression = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "owningExpression") // Looking for property
                                        {
                                            tree.Values.StringExpression._owningExpression = value;
                                        }
                                        if(name == "subExpression") // Looking for property
                                        {
                                            tree.Values.StringExpression._subExpression = value;
                                        }
                                    }
                                }
                                if(name == "TimeConstraint") // Looking for class
                                {
                                    tree.Values.__TimeConstraint = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.TimeConstraint._firstEvent = value;
                                        }
                                        if(name == "specification") // Looking for property
                                        {
                                            tree.Values.TimeConstraint._specification = value;
                                        }
                                    }
                                }
                                if(name == "TimeExpression") // Looking for class
                                {
                                    tree.Values.__TimeExpression = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "expr") // Looking for property
                                        {
                                            tree.Values.TimeExpression._expr = value;
                                        }
                                        if(name == "observation") // Looking for property
                                        {
                                            tree.Values.TimeExpression._observation = value;
                                        }
                                    }
                                }
                                if(name == "TimeInterval") // Looking for class
                                {
                                    tree.Values.__TimeInterval = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "max") // Looking for property
                                        {
                                            tree.Values.TimeInterval._max = value;
                                        }
                                        if(name == "min") // Looking for property
                                        {
                                            tree.Values.TimeInterval._min = value;
                                        }
                                    }
                                }
                                if(name == "TimeObservation") // Looking for class
                                {
                                    tree.Values.__TimeObservation = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "event") // Looking for property
                                        {
                                            tree.Values.TimeObservation._event = value;
                                        }
                                        if(name == "firstEvent") // Looking for property
                                        {
                                            tree.Values.TimeObservation._firstEvent = value;
                                        }
                                    }
                                }
                                if(name == "ValueSpecification") // Looking for class
                                {
                                    tree.Values.__ValueSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "UseCases") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Actor") // Looking for class
                                {
                                    tree.UseCases.__Actor = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Extend") // Looking for class
                                {
                                    tree.UseCases.__Extend = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "condition") // Looking for property
                                        {
                                            tree.UseCases.Extend._condition = value;
                                        }
                                        if(name == "extendedCase") // Looking for property
                                        {
                                            tree.UseCases.Extend._extendedCase = value;
                                        }
                                        if(name == "extension") // Looking for property
                                        {
                                            tree.UseCases.Extend._extension = value;
                                        }
                                        if(name == "extensionLocation") // Looking for property
                                        {
                                            tree.UseCases.Extend._extensionLocation = value;
                                        }
                                    }
                                }
                                if(name == "ExtensionPoint") // Looking for class
                                {
                                    tree.UseCases.__ExtensionPoint = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "useCase") // Looking for property
                                        {
                                            tree.UseCases.ExtensionPoint._useCase = value;
                                        }
                                    }
                                }
                                if(name == "Include") // Looking for class
                                {
                                    tree.UseCases.__Include = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "addition") // Looking for property
                                        {
                                            tree.UseCases.Include._addition = value;
                                        }
                                        if(name == "includingCase") // Looking for property
                                        {
                                            tree.UseCases.Include._includingCase = value;
                                        }
                                    }
                                }
                                if(name == "UseCase") // Looking for class
                                {
                                    tree.UseCases.__UseCase = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "extend") // Looking for property
                                        {
                                            tree.UseCases.UseCase._extend = value;
                                        }
                                        if(name == "extensionPoint") // Looking for property
                                        {
                                            tree.UseCases.UseCase._extensionPoint = value;
                                        }
                                        if(name == "include") // Looking for property
                                        {
                                            tree.UseCases.UseCase._include = value;
                                        }
                                        if(name == "subject") // Looking for property
                                        {
                                            tree.UseCases.UseCase._subject = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "StructuredClassifiers") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Association") // Looking for class
                                {
                                    tree.StructuredClassifiers.__Association = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "endType") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association._endType = value;
                                        }
                                        if(name == "isDerived") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association._isDerived = value;
                                        }
                                        if(name == "memberEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association._memberEnd = value;
                                        }
                                        if(name == "navigableOwnedEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association._navigableOwnedEnd = value;
                                        }
                                        if(name == "ownedEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Association._ownedEnd = value;
                                        }
                                    }
                                }
                                if(name == "AssociationClass") // Looking for class
                                {
                                    tree.StructuredClassifiers.__AssociationClass = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Class") // Looking for class
                                {
                                    tree.StructuredClassifiers.__Class = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "extension") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._extension = value;
                                        }
                                        if(name == "isAbstract") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._isAbstract = value;
                                        }
                                        if(name == "isActive") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._isActive = value;
                                        }
                                        if(name == "nestedClassifier") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._nestedClassifier = value;
                                        }
                                        if(name == "ownedAttribute") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._ownedAttribute = value;
                                        }
                                        if(name == "ownedOperation") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._ownedOperation = value;
                                        }
                                        if(name == "ownedReception") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._ownedReception = value;
                                        }
                                        if(name == "superClass") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Class._superClass = value;
                                        }
                                    }
                                }
                                if(name == "Collaboration") // Looking for class
                                {
                                    tree.StructuredClassifiers.__Collaboration = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "collaborationRole") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Collaboration._collaborationRole = value;
                                        }
                                    }
                                }
                                if(name == "CollaborationUse") // Looking for class
                                {
                                    tree.StructuredClassifiers.__CollaborationUse = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "roleBinding") // Looking for property
                                        {
                                            tree.StructuredClassifiers.CollaborationUse._roleBinding = value;
                                        }
                                        if(name == "type") // Looking for property
                                        {
                                            tree.StructuredClassifiers.CollaborationUse._type = value;
                                        }
                                    }
                                }
                                if(name == "Component") // Looking for class
                                {
                                    tree.StructuredClassifiers.__Component = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isIndirectlyInstantiated") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component._isIndirectlyInstantiated = value;
                                        }
                                        if(name == "packagedElement") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component._packagedElement = value;
                                        }
                                        if(name == "provided") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component._provided = value;
                                        }
                                        if(name == "realization") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component._realization = value;
                                        }
                                        if(name == "required") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Component._required = value;
                                        }
                                    }
                                }
                                if(name == "ComponentRealization") // Looking for class
                                {
                                    tree.StructuredClassifiers.__ComponentRealization = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "abstraction") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ComponentRealization._abstraction = value;
                                        }
                                        if(name == "realizingClassifier") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ComponentRealization._realizingClassifier = value;
                                        }
                                    }
                                }
                                if(name == "ConnectableElement") // Looking for class
                                {
                                    tree.StructuredClassifiers.__ConnectableElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "end") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectableElement._end = value;
                                        }
                                        if(name == "templateParameter") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectableElement._templateParameter = value;
                                        }
                                    }
                                }
                                if(name == "ConnectableElementTemplateParameter") // Looking for class
                                {
                                    tree.StructuredClassifiers.__ConnectableElementTemplateParameter = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "parameteredElement") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectableElementTemplateParameter._parameteredElement = value;
                                        }
                                    }
                                }
                                if(name == "Connector") // Looking for class
                                {
                                    tree.StructuredClassifiers.__Connector = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "contract") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector._contract = value;
                                        }
                                        if(name == "end") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector._end = value;
                                        }
                                        if(name == "kind") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector._kind = value;
                                        }
                                        if(name == "redefinedConnector") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector._redefinedConnector = value;
                                        }
                                        if(name == "type") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Connector._type = value;
                                        }
                                    }
                                }
                                if(name == "ConnectorEnd") // Looking for class
                                {
                                    tree.StructuredClassifiers.__ConnectorEnd = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "definingEnd") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectorEnd._definingEnd = value;
                                        }
                                        if(name == "partWithPort") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectorEnd._partWithPort = value;
                                        }
                                        if(name == "role") // Looking for property
                                        {
                                            tree.StructuredClassifiers.ConnectorEnd._role = value;
                                        }
                                    }
                                }
                                if(name == "EncapsulatedClassifier") // Looking for class
                                {
                                    tree.StructuredClassifiers.__EncapsulatedClassifier = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedPort") // Looking for property
                                        {
                                            tree.StructuredClassifiers.EncapsulatedClassifier._ownedPort = value;
                                        }
                                    }
                                }
                                if(name == "Port") // Looking for class
                                {
                                    tree.StructuredClassifiers.__Port = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isBehavior") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port._isBehavior = value;
                                        }
                                        if(name == "isConjugated") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port._isConjugated = value;
                                        }
                                        if(name == "isService") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port._isService = value;
                                        }
                                        if(name == "protocol") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port._protocol = value;
                                        }
                                        if(name == "provided") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port._provided = value;
                                        }
                                        if(name == "redefinedPort") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port._redefinedPort = value;
                                        }
                                        if(name == "required") // Looking for property
                                        {
                                            tree.StructuredClassifiers.Port._required = value;
                                        }
                                    }
                                }
                                if(name == "StructuredClassifier") // Looking for class
                                {
                                    tree.StructuredClassifiers.__StructuredClassifier = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedAttribute") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier._ownedAttribute = value;
                                        }
                                        if(name == "ownedConnector") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier._ownedConnector = value;
                                        }
                                        if(name == "part") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier._part = value;
                                        }
                                        if(name == "role") // Looking for property
                                        {
                                            tree.StructuredClassifiers.StructuredClassifier._role = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "StateMachines") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "ConnectionPointReference") // Looking for class
                                {
                                    tree.StateMachines.__ConnectionPointReference = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "entry") // Looking for property
                                        {
                                            tree.StateMachines.ConnectionPointReference._entry = value;
                                        }
                                        if(name == "exit") // Looking for property
                                        {
                                            tree.StateMachines.ConnectionPointReference._exit = value;
                                        }
                                        if(name == "state") // Looking for property
                                        {
                                            tree.StateMachines.ConnectionPointReference._state = value;
                                        }
                                    }
                                }
                                if(name == "FinalState") // Looking for class
                                {
                                    tree.StateMachines.__FinalState = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ProtocolConformance") // Looking for class
                                {
                                    tree.StateMachines.__ProtocolConformance = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "generalMachine") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolConformance._generalMachine = value;
                                        }
                                        if(name == "specificMachine") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolConformance._specificMachine = value;
                                        }
                                    }
                                }
                                if(name == "ProtocolStateMachine") // Looking for class
                                {
                                    tree.StateMachines.__ProtocolStateMachine = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "conformance") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolStateMachine._conformance = value;
                                        }
                                    }
                                }
                                if(name == "ProtocolTransition") // Looking for class
                                {
                                    tree.StateMachines.__ProtocolTransition = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "postCondition") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolTransition._postCondition = value;
                                        }
                                        if(name == "preCondition") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolTransition._preCondition = value;
                                        }
                                        if(name == "referred") // Looking for property
                                        {
                                            tree.StateMachines.ProtocolTransition._referred = value;
                                        }
                                    }
                                }
                                if(name == "Pseudostate") // Looking for class
                                {
                                    tree.StateMachines.__Pseudostate = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "kind") // Looking for property
                                        {
                                            tree.StateMachines.Pseudostate._kind = value;
                                        }
                                        if(name == "state") // Looking for property
                                        {
                                            tree.StateMachines.Pseudostate._state = value;
                                        }
                                        if(name == "stateMachine") // Looking for property
                                        {
                                            tree.StateMachines.Pseudostate._stateMachine = value;
                                        }
                                    }
                                }
                                if(name == "Region") // Looking for class
                                {
                                    tree.StateMachines.__Region = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "extendedRegion") // Looking for property
                                        {
                                            tree.StateMachines.Region._extendedRegion = value;
                                        }
                                        if(name == "redefinitionContext") // Looking for property
                                        {
                                            tree.StateMachines.Region._redefinitionContext = value;
                                        }
                                        if(name == "state") // Looking for property
                                        {
                                            tree.StateMachines.Region._state = value;
                                        }
                                        if(name == "stateMachine") // Looking for property
                                        {
                                            tree.StateMachines.Region._stateMachine = value;
                                        }
                                        if(name == "subvertex") // Looking for property
                                        {
                                            tree.StateMachines.Region._subvertex = value;
                                        }
                                        if(name == "transition") // Looking for property
                                        {
                                            tree.StateMachines.Region._transition = value;
                                        }
                                    }
                                }
                                if(name == "State") // Looking for class
                                {
                                    tree.StateMachines.__State = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "connection") // Looking for property
                                        {
                                            tree.StateMachines.State._connection = value;
                                        }
                                        if(name == "connectionPoint") // Looking for property
                                        {
                                            tree.StateMachines.State._connectionPoint = value;
                                        }
                                        if(name == "deferrableTrigger") // Looking for property
                                        {
                                            tree.StateMachines.State._deferrableTrigger = value;
                                        }
                                        if(name == "doActivity") // Looking for property
                                        {
                                            tree.StateMachines.State._doActivity = value;
                                        }
                                        if(name == "entry") // Looking for property
                                        {
                                            tree.StateMachines.State._entry = value;
                                        }
                                        if(name == "exit") // Looking for property
                                        {
                                            tree.StateMachines.State._exit = value;
                                        }
                                        if(name == "isComposite") // Looking for property
                                        {
                                            tree.StateMachines.State._isComposite = value;
                                        }
                                        if(name == "isOrthogonal") // Looking for property
                                        {
                                            tree.StateMachines.State._isOrthogonal = value;
                                        }
                                        if(name == "isSimple") // Looking for property
                                        {
                                            tree.StateMachines.State._isSimple = value;
                                        }
                                        if(name == "isSubmachineState") // Looking for property
                                        {
                                            tree.StateMachines.State._isSubmachineState = value;
                                        }
                                        if(name == "redefinedState") // Looking for property
                                        {
                                            tree.StateMachines.State._redefinedState = value;
                                        }
                                        if(name == "redefinitionContext") // Looking for property
                                        {
                                            tree.StateMachines.State._redefinitionContext = value;
                                        }
                                        if(name == "region") // Looking for property
                                        {
                                            tree.StateMachines.State._region = value;
                                        }
                                        if(name == "stateInvariant") // Looking for property
                                        {
                                            tree.StateMachines.State._stateInvariant = value;
                                        }
                                        if(name == "submachine") // Looking for property
                                        {
                                            tree.StateMachines.State._submachine = value;
                                        }
                                    }
                                }
                                if(name == "StateMachine") // Looking for class
                                {
                                    tree.StateMachines.__StateMachine = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "connectionPoint") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine._connectionPoint = value;
                                        }
                                        if(name == "extendedStateMachine") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine._extendedStateMachine = value;
                                        }
                                        if(name == "region") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine._region = value;
                                        }
                                        if(name == "submachineState") // Looking for property
                                        {
                                            tree.StateMachines.StateMachine._submachineState = value;
                                        }
                                    }
                                }
                                if(name == "Transition") // Looking for class
                                {
                                    tree.StateMachines.__Transition = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "container") // Looking for property
                                        {
                                            tree.StateMachines.Transition._container = value;
                                        }
                                        if(name == "effect") // Looking for property
                                        {
                                            tree.StateMachines.Transition._effect = value;
                                        }
                                        if(name == "guard") // Looking for property
                                        {
                                            tree.StateMachines.Transition._guard = value;
                                        }
                                        if(name == "kind") // Looking for property
                                        {
                                            tree.StateMachines.Transition._kind = value;
                                        }
                                        if(name == "redefinedTransition") // Looking for property
                                        {
                                            tree.StateMachines.Transition._redefinedTransition = value;
                                        }
                                        if(name == "redefinitionContext") // Looking for property
                                        {
                                            tree.StateMachines.Transition._redefinitionContext = value;
                                        }
                                        if(name == "source") // Looking for property
                                        {
                                            tree.StateMachines.Transition._source = value;
                                        }
                                        if(name == "target") // Looking for property
                                        {
                                            tree.StateMachines.Transition._target = value;
                                        }
                                        if(name == "trigger") // Looking for property
                                        {
                                            tree.StateMachines.Transition._trigger = value;
                                        }
                                    }
                                }
                                if(name == "Vertex") // Looking for class
                                {
                                    tree.StateMachines.__Vertex = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "container") // Looking for property
                                        {
                                            tree.StateMachines.Vertex._container = value;
                                        }
                                        if(name == "incoming") // Looking for property
                                        {
                                            tree.StateMachines.Vertex._incoming = value;
                                        }
                                        if(name == "outgoing") // Looking for property
                                        {
                                            tree.StateMachines.Vertex._outgoing = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "SimpleClassifiers") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "BehavioredClassifier") // Looking for class
                                {
                                    tree.SimpleClassifiers.__BehavioredClassifier = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "classifierBehavior") // Looking for property
                                        {
                                            tree.SimpleClassifiers.BehavioredClassifier._classifierBehavior = value;
                                        }
                                        if(name == "interfaceRealization") // Looking for property
                                        {
                                            tree.SimpleClassifiers.BehavioredClassifier._interfaceRealization = value;
                                        }
                                        if(name == "ownedBehavior") // Looking for property
                                        {
                                            tree.SimpleClassifiers.BehavioredClassifier._ownedBehavior = value;
                                        }
                                    }
                                }
                                if(name == "DataType") // Looking for class
                                {
                                    tree.SimpleClassifiers.__DataType = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedAttribute") // Looking for property
                                        {
                                            tree.SimpleClassifiers.DataType._ownedAttribute = value;
                                        }
                                        if(name == "ownedOperation") // Looking for property
                                        {
                                            tree.SimpleClassifiers.DataType._ownedOperation = value;
                                        }
                                    }
                                }
                                if(name == "Enumeration") // Looking for class
                                {
                                    tree.SimpleClassifiers.__Enumeration = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedLiteral") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Enumeration._ownedLiteral = value;
                                        }
                                    }
                                }
                                if(name == "EnumerationLiteral") // Looking for class
                                {
                                    tree.SimpleClassifiers.__EnumerationLiteral = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "classifier") // Looking for property
                                        {
                                            tree.SimpleClassifiers.EnumerationLiteral._classifier = value;
                                        }
                                        if(name == "enumeration") // Looking for property
                                        {
                                            tree.SimpleClassifiers.EnumerationLiteral._enumeration = value;
                                        }
                                    }
                                }
                                if(name == "Interface") // Looking for class
                                {
                                    tree.SimpleClassifiers.__Interface = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "nestedClassifier") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface._nestedClassifier = value;
                                        }
                                        if(name == "ownedAttribute") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface._ownedAttribute = value;
                                        }
                                        if(name == "ownedOperation") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface._ownedOperation = value;
                                        }
                                        if(name == "ownedReception") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface._ownedReception = value;
                                        }
                                        if(name == "protocol") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface._protocol = value;
                                        }
                                        if(name == "redefinedInterface") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Interface._redefinedInterface = value;
                                        }
                                    }
                                }
                                if(name == "InterfaceRealization") // Looking for class
                                {
                                    tree.SimpleClassifiers.__InterfaceRealization = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "contract") // Looking for property
                                        {
                                            tree.SimpleClassifiers.InterfaceRealization._contract = value;
                                        }
                                        if(name == "implementingClassifier") // Looking for property
                                        {
                                            tree.SimpleClassifiers.InterfaceRealization._implementingClassifier = value;
                                        }
                                    }
                                }
                                if(name == "PrimitiveType") // Looking for class
                                {
                                    tree.SimpleClassifiers.__PrimitiveType = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Reception") // Looking for class
                                {
                                    tree.SimpleClassifiers.__Reception = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "signal") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Reception._signal = value;
                                        }
                                    }
                                }
                                if(name == "Signal") // Looking for class
                                {
                                    tree.SimpleClassifiers.__Signal = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedAttribute") // Looking for property
                                        {
                                            tree.SimpleClassifiers.Signal._ownedAttribute = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Packages") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Extension") // Looking for class
                                {
                                    tree.Packages.__Extension = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isRequired") // Looking for property
                                        {
                                            tree.Packages.Extension._isRequired = value;
                                        }
                                        if(name == "metaclass") // Looking for property
                                        {
                                            tree.Packages.Extension._metaclass = value;
                                        }
                                        if(name == "ownedEnd") // Looking for property
                                        {
                                            tree.Packages.Extension._ownedEnd = value;
                                        }
                                    }
                                }
                                if(name == "ExtensionEnd") // Looking for class
                                {
                                    tree.Packages.__ExtensionEnd = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "lower") // Looking for property
                                        {
                                            tree.Packages.ExtensionEnd._lower = value;
                                        }
                                        if(name == "type") // Looking for property
                                        {
                                            tree.Packages.ExtensionEnd._type = value;
                                        }
                                    }
                                }
                                if(name == "Image") // Looking for class
                                {
                                    tree.Packages.__Image = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "content") // Looking for property
                                        {
                                            tree.Packages.Image._content = value;
                                        }
                                        if(name == "format") // Looking for property
                                        {
                                            tree.Packages.Image._format = value;
                                        }
                                        if(name == "location") // Looking for property
                                        {
                                            tree.Packages.Image._location = value;
                                        }
                                    }
                                }
                                if(name == "Model") // Looking for class
                                {
                                    tree.Packages.__Model = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "viewpoint") // Looking for property
                                        {
                                            tree.Packages.Model._viewpoint = value;
                                        }
                                    }
                                }
                                if(name == "Package") // Looking for class
                                {
                                    tree.Packages.__Package = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "URI") // Looking for property
                                        {
                                            tree.Packages.Package._URI = value;
                                        }
                                        if(name == "nestedPackage") // Looking for property
                                        {
                                            tree.Packages.Package._nestedPackage = value;
                                        }
                                        if(name == "nestingPackage") // Looking for property
                                        {
                                            tree.Packages.Package._nestingPackage = value;
                                        }
                                        if(name == "ownedStereotype") // Looking for property
                                        {
                                            tree.Packages.Package._ownedStereotype = value;
                                        }
                                        if(name == "ownedType") // Looking for property
                                        {
                                            tree.Packages.Package._ownedType = value;
                                        }
                                        if(name == "packageMerge") // Looking for property
                                        {
                                            tree.Packages.Package._packageMerge = value;
                                        }
                                        if(name == "packagedElement") // Looking for property
                                        {
                                            tree.Packages.Package._packagedElement = value;
                                        }
                                        if(name == "profileApplication") // Looking for property
                                        {
                                            tree.Packages.Package._profileApplication = value;
                                        }
                                    }
                                }
                                if(name == "PackageMerge") // Looking for class
                                {
                                    tree.Packages.__PackageMerge = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "mergedPackage") // Looking for property
                                        {
                                            tree.Packages.PackageMerge._mergedPackage = value;
                                        }
                                        if(name == "receivingPackage") // Looking for property
                                        {
                                            tree.Packages.PackageMerge._receivingPackage = value;
                                        }
                                    }
                                }
                                if(name == "Profile") // Looking for class
                                {
                                    tree.Packages.__Profile = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "metaclassReference") // Looking for property
                                        {
                                            tree.Packages.Profile._metaclassReference = value;
                                        }
                                        if(name == "metamodelReference") // Looking for property
                                        {
                                            tree.Packages.Profile._metamodelReference = value;
                                        }
                                    }
                                }
                                if(name == "ProfileApplication") // Looking for class
                                {
                                    tree.Packages.__ProfileApplication = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "appliedProfile") // Looking for property
                                        {
                                            tree.Packages.ProfileApplication._appliedProfile = value;
                                        }
                                        if(name == "applyingPackage") // Looking for property
                                        {
                                            tree.Packages.ProfileApplication._applyingPackage = value;
                                        }
                                        if(name == "isStrict") // Looking for property
                                        {
                                            tree.Packages.ProfileApplication._isStrict = value;
                                        }
                                    }
                                }
                                if(name == "Stereotype") // Looking for class
                                {
                                    tree.Packages.__Stereotype = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "icon") // Looking for property
                                        {
                                            tree.Packages.Stereotype._icon = value;
                                        }
                                        if(name == "profile") // Looking for property
                                        {
                                            tree.Packages.Stereotype._profile = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Interactions") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "ActionExecutionSpecification") // Looking for class
                                {
                                    tree.Interactions.__ActionExecutionSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "action") // Looking for property
                                        {
                                            tree.Interactions.ActionExecutionSpecification._action = value;
                                        }
                                    }
                                }
                                if(name == "BehaviorExecutionSpecification") // Looking for class
                                {
                                    tree.Interactions.__BehaviorExecutionSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "behavior") // Looking for property
                                        {
                                            tree.Interactions.BehaviorExecutionSpecification._behavior = value;
                                        }
                                    }
                                }
                                if(name == "CombinedFragment") // Looking for class
                                {
                                    tree.Interactions.__CombinedFragment = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "cfragmentGate") // Looking for property
                                        {
                                            tree.Interactions.CombinedFragment._cfragmentGate = value;
                                        }
                                        if(name == "interactionOperator") // Looking for property
                                        {
                                            tree.Interactions.CombinedFragment._interactionOperator = value;
                                        }
                                        if(name == "operand") // Looking for property
                                        {
                                            tree.Interactions.CombinedFragment._operand = value;
                                        }
                                    }
                                }
                                if(name == "ConsiderIgnoreFragment") // Looking for class
                                {
                                    tree.Interactions.__ConsiderIgnoreFragment = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "message") // Looking for property
                                        {
                                            tree.Interactions.ConsiderIgnoreFragment._message = value;
                                        }
                                    }
                                }
                                if(name == "Continuation") // Looking for class
                                {
                                    tree.Interactions.__Continuation = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "setting") // Looking for property
                                        {
                                            tree.Interactions.Continuation._setting = value;
                                        }
                                    }
                                }
                                if(name == "DestructionOccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.__DestructionOccurrenceSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ExecutionOccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.__ExecutionOccurrenceSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "execution") // Looking for property
                                        {
                                            tree.Interactions.ExecutionOccurrenceSpecification._execution = value;
                                        }
                                    }
                                }
                                if(name == "ExecutionSpecification") // Looking for class
                                {
                                    tree.Interactions.__ExecutionSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "finish") // Looking for property
                                        {
                                            tree.Interactions.ExecutionSpecification._finish = value;
                                        }
                                        if(name == "start") // Looking for property
                                        {
                                            tree.Interactions.ExecutionSpecification._start = value;
                                        }
                                    }
                                }
                                if(name == "Gate") // Looking for class
                                {
                                    tree.Interactions.__Gate = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "GeneralOrdering") // Looking for class
                                {
                                    tree.Interactions.__GeneralOrdering = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "after") // Looking for property
                                        {
                                            tree.Interactions.GeneralOrdering._after = value;
                                        }
                                        if(name == "before") // Looking for property
                                        {
                                            tree.Interactions.GeneralOrdering._before = value;
                                        }
                                    }
                                }
                                if(name == "Interaction") // Looking for class
                                {
                                    tree.Interactions.__Interaction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "action") // Looking for property
                                        {
                                            tree.Interactions.Interaction._action = value;
                                        }
                                        if(name == "formalGate") // Looking for property
                                        {
                                            tree.Interactions.Interaction._formalGate = value;
                                        }
                                        if(name == "fragment") // Looking for property
                                        {
                                            tree.Interactions.Interaction._fragment = value;
                                        }
                                        if(name == "lifeline") // Looking for property
                                        {
                                            tree.Interactions.Interaction._lifeline = value;
                                        }
                                        if(name == "message") // Looking for property
                                        {
                                            tree.Interactions.Interaction._message = value;
                                        }
                                    }
                                }
                                if(name == "InteractionConstraint") // Looking for class
                                {
                                    tree.Interactions.__InteractionConstraint = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "maxint") // Looking for property
                                        {
                                            tree.Interactions.InteractionConstraint._maxint = value;
                                        }
                                        if(name == "minint") // Looking for property
                                        {
                                            tree.Interactions.InteractionConstraint._minint = value;
                                        }
                                    }
                                }
                                if(name == "InteractionFragment") // Looking for class
                                {
                                    tree.Interactions.__InteractionFragment = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "covered") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment._covered = value;
                                        }
                                        if(name == "enclosingInteraction") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment._enclosingInteraction = value;
                                        }
                                        if(name == "enclosingOperand") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment._enclosingOperand = value;
                                        }
                                        if(name == "generalOrdering") // Looking for property
                                        {
                                            tree.Interactions.InteractionFragment._generalOrdering = value;
                                        }
                                    }
                                }
                                if(name == "InteractionOperand") // Looking for class
                                {
                                    tree.Interactions.__InteractionOperand = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "fragment") // Looking for property
                                        {
                                            tree.Interactions.InteractionOperand._fragment = value;
                                        }
                                        if(name == "guard") // Looking for property
                                        {
                                            tree.Interactions.InteractionOperand._guard = value;
                                        }
                                    }
                                }
                                if(name == "InteractionUse") // Looking for class
                                {
                                    tree.Interactions.__InteractionUse = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "actualGate") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse._actualGate = value;
                                        }
                                        if(name == "argument") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse._argument = value;
                                        }
                                        if(name == "refersTo") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse._refersTo = value;
                                        }
                                        if(name == "returnValue") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse._returnValue = value;
                                        }
                                        if(name == "returnValueRecipient") // Looking for property
                                        {
                                            tree.Interactions.InteractionUse._returnValueRecipient = value;
                                        }
                                    }
                                }
                                if(name == "Lifeline") // Looking for class
                                {
                                    tree.Interactions.__Lifeline = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "coveredBy") // Looking for property
                                        {
                                            tree.Interactions.Lifeline._coveredBy = value;
                                        }
                                        if(name == "decomposedAs") // Looking for property
                                        {
                                            tree.Interactions.Lifeline._decomposedAs = value;
                                        }
                                        if(name == "interaction") // Looking for property
                                        {
                                            tree.Interactions.Lifeline._interaction = value;
                                        }
                                        if(name == "represents") // Looking for property
                                        {
                                            tree.Interactions.Lifeline._represents = value;
                                        }
                                        if(name == "selector") // Looking for property
                                        {
                                            tree.Interactions.Lifeline._selector = value;
                                        }
                                    }
                                }
                                if(name == "Message") // Looking for class
                                {
                                    tree.Interactions.__Message = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "argument") // Looking for property
                                        {
                                            tree.Interactions.Message._argument = value;
                                        }
                                        if(name == "connector") // Looking for property
                                        {
                                            tree.Interactions.Message._connector = value;
                                        }
                                        if(name == "interaction") // Looking for property
                                        {
                                            tree.Interactions.Message._interaction = value;
                                        }
                                        if(name == "messageKind") // Looking for property
                                        {
                                            tree.Interactions.Message._messageKind = value;
                                        }
                                        if(name == "messageSort") // Looking for property
                                        {
                                            tree.Interactions.Message._messageSort = value;
                                        }
                                        if(name == "receiveEvent") // Looking for property
                                        {
                                            tree.Interactions.Message._receiveEvent = value;
                                        }
                                        if(name == "sendEvent") // Looking for property
                                        {
                                            tree.Interactions.Message._sendEvent = value;
                                        }
                                        if(name == "signature") // Looking for property
                                        {
                                            tree.Interactions.Message._signature = value;
                                        }
                                    }
                                }
                                if(name == "MessageEnd") // Looking for class
                                {
                                    tree.Interactions.__MessageEnd = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "message") // Looking for property
                                        {
                                            tree.Interactions.MessageEnd._message = value;
                                        }
                                    }
                                }
                                if(name == "MessageOccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.__MessageOccurrenceSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "OccurrenceSpecification") // Looking for class
                                {
                                    tree.Interactions.__OccurrenceSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "covered") // Looking for property
                                        {
                                            tree.Interactions.OccurrenceSpecification._covered = value;
                                        }
                                        if(name == "toAfter") // Looking for property
                                        {
                                            tree.Interactions.OccurrenceSpecification._toAfter = value;
                                        }
                                        if(name == "toBefore") // Looking for property
                                        {
                                            tree.Interactions.OccurrenceSpecification._toBefore = value;
                                        }
                                    }
                                }
                                if(name == "PartDecomposition") // Looking for class
                                {
                                    tree.Interactions.__PartDecomposition = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "StateInvariant") // Looking for class
                                {
                                    tree.Interactions.__StateInvariant = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "covered") // Looking for property
                                        {
                                            tree.Interactions.StateInvariant._covered = value;
                                        }
                                        if(name == "invariant") // Looking for property
                                        {
                                            tree.Interactions.StateInvariant._invariant = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "InformationFlows") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "InformationFlow") // Looking for class
                                {
                                    tree.InformationFlows.__InformationFlow = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "conveyed") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow._conveyed = value;
                                        }
                                        if(name == "informationSource") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow._informationSource = value;
                                        }
                                        if(name == "informationTarget") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow._informationTarget = value;
                                        }
                                        if(name == "realization") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow._realization = value;
                                        }
                                        if(name == "realizingActivityEdge") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow._realizingActivityEdge = value;
                                        }
                                        if(name == "realizingConnector") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow._realizingConnector = value;
                                        }
                                        if(name == "realizingMessage") // Looking for property
                                        {
                                            tree.InformationFlows.InformationFlow._realizingMessage = value;
                                        }
                                    }
                                }
                                if(name == "InformationItem") // Looking for class
                                {
                                    tree.InformationFlows.__InformationItem = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "represented") // Looking for property
                                        {
                                            tree.InformationFlows.InformationItem._represented = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Deployments") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Artifact") // Looking for class
                                {
                                    tree.Deployments.__Artifact = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "fileName") // Looking for property
                                        {
                                            tree.Deployments.Artifact._fileName = value;
                                        }
                                        if(name == "manifestation") // Looking for property
                                        {
                                            tree.Deployments.Artifact._manifestation = value;
                                        }
                                        if(name == "nestedArtifact") // Looking for property
                                        {
                                            tree.Deployments.Artifact._nestedArtifact = value;
                                        }
                                        if(name == "ownedAttribute") // Looking for property
                                        {
                                            tree.Deployments.Artifact._ownedAttribute = value;
                                        }
                                        if(name == "ownedOperation") // Looking for property
                                        {
                                            tree.Deployments.Artifact._ownedOperation = value;
                                        }
                                    }
                                }
                                if(name == "CommunicationPath") // Looking for class
                                {
                                    tree.Deployments.__CommunicationPath = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "DeployedArtifact") // Looking for class
                                {
                                    tree.Deployments.__DeployedArtifact = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Deployment") // Looking for class
                                {
                                    tree.Deployments.__Deployment = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "configuration") // Looking for property
                                        {
                                            tree.Deployments.Deployment._configuration = value;
                                        }
                                        if(name == "deployedArtifact") // Looking for property
                                        {
                                            tree.Deployments.Deployment._deployedArtifact = value;
                                        }
                                        if(name == "location") // Looking for property
                                        {
                                            tree.Deployments.Deployment._location = value;
                                        }
                                    }
                                }
                                if(name == "DeploymentSpecification") // Looking for class
                                {
                                    tree.Deployments.__DeploymentSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "deployment") // Looking for property
                                        {
                                            tree.Deployments.DeploymentSpecification._deployment = value;
                                        }
                                        if(name == "deploymentLocation") // Looking for property
                                        {
                                            tree.Deployments.DeploymentSpecification._deploymentLocation = value;
                                        }
                                        if(name == "executionLocation") // Looking for property
                                        {
                                            tree.Deployments.DeploymentSpecification._executionLocation = value;
                                        }
                                    }
                                }
                                if(name == "DeploymentTarget") // Looking for class
                                {
                                    tree.Deployments.__DeploymentTarget = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "deployedElement") // Looking for property
                                        {
                                            tree.Deployments.DeploymentTarget._deployedElement = value;
                                        }
                                        if(name == "deployment") // Looking for property
                                        {
                                            tree.Deployments.DeploymentTarget._deployment = value;
                                        }
                                    }
                                }
                                if(name == "Device") // Looking for class
                                {
                                    tree.Deployments.__Device = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ExecutionEnvironment") // Looking for class
                                {
                                    tree.Deployments.__ExecutionEnvironment = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Manifestation") // Looking for class
                                {
                                    tree.Deployments.__Manifestation = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "utilizedElement") // Looking for property
                                        {
                                            tree.Deployments.Manifestation._utilizedElement = value;
                                        }
                                    }
                                }
                                if(name == "Node") // Looking for class
                                {
                                    tree.Deployments.__Node = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "nestedNode") // Looking for property
                                        {
                                            tree.Deployments.Node._nestedNode = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "CommonStructure") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Abstraction") // Looking for class
                                {
                                    tree.CommonStructure.__Abstraction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "mapping") // Looking for property
                                        {
                                            tree.CommonStructure.Abstraction._mapping = value;
                                        }
                                    }
                                }
                                if(name == "Comment") // Looking for class
                                {
                                    tree.CommonStructure.__Comment = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "annotatedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Comment._annotatedElement = value;
                                        }
                                        if(name == "body") // Looking for property
                                        {
                                            tree.CommonStructure.Comment._body = value;
                                        }
                                    }
                                }
                                if(name == "Constraint") // Looking for class
                                {
                                    tree.CommonStructure.__Constraint = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "constrainedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Constraint._constrainedElement = value;
                                        }
                                        if(name == "context") // Looking for property
                                        {
                                            tree.CommonStructure.Constraint._context = value;
                                        }
                                        if(name == "specification") // Looking for property
                                        {
                                            tree.CommonStructure.Constraint._specification = value;
                                        }
                                    }
                                }
                                if(name == "Dependency") // Looking for class
                                {
                                    tree.CommonStructure.__Dependency = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "client") // Looking for property
                                        {
                                            tree.CommonStructure.Dependency._client = value;
                                        }
                                        if(name == "supplier") // Looking for property
                                        {
                                            tree.CommonStructure.Dependency._supplier = value;
                                        }
                                    }
                                }
                                if(name == "DirectedRelationship") // Looking for class
                                {
                                    tree.CommonStructure.__DirectedRelationship = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "source") // Looking for property
                                        {
                                            tree.CommonStructure.DirectedRelationship._source = value;
                                        }
                                        if(name == "target") // Looking for property
                                        {
                                            tree.CommonStructure.DirectedRelationship._target = value;
                                        }
                                    }
                                }
                                if(name == "Element") // Looking for class
                                {
                                    tree.CommonStructure.__Element = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedComment") // Looking for property
                                        {
                                            tree.CommonStructure.Element._ownedComment = value;
                                        }
                                        if(name == "ownedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Element._ownedElement = value;
                                        }
                                        if(name == "owner") // Looking for property
                                        {
                                            tree.CommonStructure.Element._owner = value;
                                        }
                                    }
                                }
                                if(name == "ElementImport") // Looking for class
                                {
                                    tree.CommonStructure.__ElementImport = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "alias") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport._alias = value;
                                        }
                                        if(name == "importedElement") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport._importedElement = value;
                                        }
                                        if(name == "importingNamespace") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport._importingNamespace = value;
                                        }
                                        if(name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.ElementImport._visibility = value;
                                        }
                                    }
                                }
                                if(name == "MultiplicityElement") // Looking for class
                                {
                                    tree.CommonStructure.__MultiplicityElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isOrdered") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement._isOrdered = value;
                                        }
                                        if(name == "isUnique") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement._isUnique = value;
                                        }
                                        if(name == "lower") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement._lower = value;
                                        }
                                        if(name == "lowerValue") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement._lowerValue = value;
                                        }
                                        if(name == "upper") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement._upper = value;
                                        }
                                        if(name == "upperValue") // Looking for property
                                        {
                                            tree.CommonStructure.MultiplicityElement._upperValue = value;
                                        }
                                    }
                                }
                                if(name == "NamedElement") // Looking for class
                                {
                                    tree.CommonStructure.__NamedElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "clientDependency") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement._clientDependency = value;
                                        }
                                        if(name == "name") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement._name = value;
                                        }
                                        if(name == "nameExpression") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement._nameExpression = value;
                                        }
                                        if(name == "namespace") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement._namespace = value;
                                        }
                                        if(name == "qualifiedName") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement._qualifiedName = value;
                                        }
                                        if(name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.NamedElement._visibility = value;
                                        }
                                    }
                                }
                                if(name == "Namespace") // Looking for class
                                {
                                    tree.CommonStructure.__Namespace = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "elementImport") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace._elementImport = value;
                                        }
                                        if(name == "importedMember") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace._importedMember = value;
                                        }
                                        if(name == "member") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace._member = value;
                                        }
                                        if(name == "ownedMember") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace._ownedMember = value;
                                        }
                                        if(name == "ownedRule") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace._ownedRule = value;
                                        }
                                        if(name == "packageImport") // Looking for property
                                        {
                                            tree.CommonStructure.Namespace._packageImport = value;
                                        }
                                    }
                                }
                                if(name == "PackageableElement") // Looking for class
                                {
                                    tree.CommonStructure.__PackageableElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.PackageableElement._visibility = value;
                                        }
                                    }
                                }
                                if(name == "PackageImport") // Looking for class
                                {
                                    tree.CommonStructure.__PackageImport = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "importedPackage") // Looking for property
                                        {
                                            tree.CommonStructure.PackageImport._importedPackage = value;
                                        }
                                        if(name == "importingNamespace") // Looking for property
                                        {
                                            tree.CommonStructure.PackageImport._importingNamespace = value;
                                        }
                                        if(name == "visibility") // Looking for property
                                        {
                                            tree.CommonStructure.PackageImport._visibility = value;
                                        }
                                    }
                                }
                                if(name == "ParameterableElement") // Looking for class
                                {
                                    tree.CommonStructure.__ParameterableElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "owningTemplateParameter") // Looking for property
                                        {
                                            tree.CommonStructure.ParameterableElement._owningTemplateParameter = value;
                                        }
                                        if(name == "templateParameter") // Looking for property
                                        {
                                            tree.CommonStructure.ParameterableElement._templateParameter = value;
                                        }
                                    }
                                }
                                if(name == "Realization") // Looking for class
                                {
                                    tree.CommonStructure.__Realization = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Relationship") // Looking for class
                                {
                                    tree.CommonStructure.__Relationship = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "relatedElement") // Looking for property
                                        {
                                            tree.CommonStructure.Relationship._relatedElement = value;
                                        }
                                    }
                                }
                                if(name == "TemplateableElement") // Looking for class
                                {
                                    tree.CommonStructure.__TemplateableElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedTemplateSignature") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateableElement._ownedTemplateSignature = value;
                                        }
                                        if(name == "templateBinding") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateableElement._templateBinding = value;
                                        }
                                    }
                                }
                                if(name == "TemplateBinding") // Looking for class
                                {
                                    tree.CommonStructure.__TemplateBinding = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "boundElement") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateBinding._boundElement = value;
                                        }
                                        if(name == "parameterSubstitution") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateBinding._parameterSubstitution = value;
                                        }
                                        if(name == "signature") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateBinding._signature = value;
                                        }
                                    }
                                }
                                if(name == "TemplateParameter") // Looking for class
                                {
                                    tree.CommonStructure.__TemplateParameter = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "default") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter._default = value;
                                        }
                                        if(name == "ownedDefault") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter._ownedDefault = value;
                                        }
                                        if(name == "ownedParameteredElement") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter._ownedParameteredElement = value;
                                        }
                                        if(name == "parameteredElement") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter._parameteredElement = value;
                                        }
                                        if(name == "signature") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameter._signature = value;
                                        }
                                    }
                                }
                                if(name == "TemplateParameterSubstitution") // Looking for class
                                {
                                    tree.CommonStructure.__TemplateParameterSubstitution = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "actual") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution._actual = value;
                                        }
                                        if(name == "formal") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution._formal = value;
                                        }
                                        if(name == "ownedActual") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution._ownedActual = value;
                                        }
                                        if(name == "templateBinding") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateParameterSubstitution._templateBinding = value;
                                        }
                                    }
                                }
                                if(name == "TemplateSignature") // Looking for class
                                {
                                    tree.CommonStructure.__TemplateSignature = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "ownedParameter") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateSignature._ownedParameter = value;
                                        }
                                        if(name == "parameter") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateSignature._parameter = value;
                                        }
                                        if(name == "template") // Looking for property
                                        {
                                            tree.CommonStructure.TemplateSignature._template = value;
                                        }
                                    }
                                }
                                if(name == "Type") // Looking for class
                                {
                                    tree.CommonStructure.__Type = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "package") // Looking for property
                                        {
                                            tree.CommonStructure.Type._package = value;
                                        }
                                    }
                                }
                                if(name == "TypedElement") // Looking for class
                                {
                                    tree.CommonStructure.__TypedElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "type") // Looking for property
                                        {
                                            tree.CommonStructure.TypedElement._type = value;
                                        }
                                    }
                                }
                                if(name == "Usage") // Looking for class
                                {
                                    tree.CommonStructure.__Usage = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "CommonBehavior") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "AnyReceiveEvent") // Looking for class
                                {
                                    tree.CommonBehavior.__AnyReceiveEvent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Behavior") // Looking for class
                                {
                                    tree.CommonBehavior.__Behavior = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "context") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._context = value;
                                        }
                                        if(name == "isReentrant") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._isReentrant = value;
                                        }
                                        if(name == "ownedParameter") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._ownedParameter = value;
                                        }
                                        if(name == "ownedParameterSet") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._ownedParameterSet = value;
                                        }
                                        if(name == "postcondition") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._postcondition = value;
                                        }
                                        if(name == "precondition") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._precondition = value;
                                        }
                                        if(name == "specification") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._specification = value;
                                        }
                                        if(name == "redefinedBehavior") // Looking for property
                                        {
                                            tree.CommonBehavior.Behavior._redefinedBehavior = value;
                                        }
                                    }
                                }
                                if(name == "CallEvent") // Looking for class
                                {
                                    tree.CommonBehavior.__CallEvent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "operation") // Looking for property
                                        {
                                            tree.CommonBehavior.CallEvent._operation = value;
                                        }
                                    }
                                }
                                if(name == "ChangeEvent") // Looking for class
                                {
                                    tree.CommonBehavior.__ChangeEvent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "changeExpression") // Looking for property
                                        {
                                            tree.CommonBehavior.ChangeEvent._changeExpression = value;
                                        }
                                    }
                                }
                                if(name == "Event") // Looking for class
                                {
                                    tree.CommonBehavior.__Event = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "FunctionBehavior") // Looking for class
                                {
                                    tree.CommonBehavior.__FunctionBehavior = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "MessageEvent") // Looking for class
                                {
                                    tree.CommonBehavior.__MessageEvent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "OpaqueBehavior") // Looking for class
                                {
                                    tree.CommonBehavior.__OpaqueBehavior = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "body") // Looking for property
                                        {
                                            tree.CommonBehavior.OpaqueBehavior._body = value;
                                        }
                                        if(name == "language") // Looking for property
                                        {
                                            tree.CommonBehavior.OpaqueBehavior._language = value;
                                        }
                                    }
                                }
                                if(name == "SignalEvent") // Looking for class
                                {
                                    tree.CommonBehavior.__SignalEvent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "signal") // Looking for property
                                        {
                                            tree.CommonBehavior.SignalEvent._signal = value;
                                        }
                                    }
                                }
                                if(name == "TimeEvent") // Looking for class
                                {
                                    tree.CommonBehavior.__TimeEvent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isRelative") // Looking for property
                                        {
                                            tree.CommonBehavior.TimeEvent._isRelative = value;
                                        }
                                        if(name == "when") // Looking for property
                                        {
                                            tree.CommonBehavior.TimeEvent._when = value;
                                        }
                                    }
                                }
                                if(name == "Trigger") // Looking for class
                                {
                                    tree.CommonBehavior.__Trigger = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "event") // Looking for property
                                        {
                                            tree.CommonBehavior.Trigger._event = value;
                                        }
                                        if(name == "port") // Looking for property
                                        {
                                            tree.CommonBehavior.Trigger._port = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Classification") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "Substitution") // Looking for class
                                {
                                    tree.Classification.__Substitution = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "contract") // Looking for property
                                        {
                                            tree.Classification.Substitution._contract = value;
                                        }
                                        if(name == "substitutingClassifier") // Looking for property
                                        {
                                            tree.Classification.Substitution._substitutingClassifier = value;
                                        }
                                    }
                                }
                                if(name == "BehavioralFeature") // Looking for class
                                {
                                    tree.Classification.__BehavioralFeature = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "concurrency") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature._concurrency = value;
                                        }
                                        if(name == "isAbstract") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature._isAbstract = value;
                                        }
                                        if(name == "method") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature._method = value;
                                        }
                                        if(name == "ownedParameter") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature._ownedParameter = value;
                                        }
                                        if(name == "ownedParameterSet") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature._ownedParameterSet = value;
                                        }
                                        if(name == "raisedException") // Looking for property
                                        {
                                            tree.Classification.BehavioralFeature._raisedException = value;
                                        }
                                    }
                                }
                                if(name == "Classifier") // Looking for class
                                {
                                    tree.Classification.__Classifier = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "attribute") // Looking for property
                                        {
                                            tree.Classification.Classifier._attribute = value;
                                        }
                                        if(name == "collaborationUse") // Looking for property
                                        {
                                            tree.Classification.Classifier._collaborationUse = value;
                                        }
                                        if(name == "feature") // Looking for property
                                        {
                                            tree.Classification.Classifier._feature = value;
                                        }
                                        if(name == "general") // Looking for property
                                        {
                                            tree.Classification.Classifier._general = value;
                                        }
                                        if(name == "generalization") // Looking for property
                                        {
                                            tree.Classification.Classifier._generalization = value;
                                        }
                                        if(name == "inheritedMember") // Looking for property
                                        {
                                            tree.Classification.Classifier._inheritedMember = value;
                                        }
                                        if(name == "isAbstract") // Looking for property
                                        {
                                            tree.Classification.Classifier._isAbstract = value;
                                        }
                                        if(name == "isFinalSpecialization") // Looking for property
                                        {
                                            tree.Classification.Classifier._isFinalSpecialization = value;
                                        }
                                        if(name == "ownedTemplateSignature") // Looking for property
                                        {
                                            tree.Classification.Classifier._ownedTemplateSignature = value;
                                        }
                                        if(name == "ownedUseCase") // Looking for property
                                        {
                                            tree.Classification.Classifier._ownedUseCase = value;
                                        }
                                        if(name == "powertypeExtent") // Looking for property
                                        {
                                            tree.Classification.Classifier._powertypeExtent = value;
                                        }
                                        if(name == "redefinedClassifier") // Looking for property
                                        {
                                            tree.Classification.Classifier._redefinedClassifier = value;
                                        }
                                        if(name == "representation") // Looking for property
                                        {
                                            tree.Classification.Classifier._representation = value;
                                        }
                                        if(name == "substitution") // Looking for property
                                        {
                                            tree.Classification.Classifier._substitution = value;
                                        }
                                        if(name == "templateParameter") // Looking for property
                                        {
                                            tree.Classification.Classifier._templateParameter = value;
                                        }
                                        if(name == "useCase") // Looking for property
                                        {
                                            tree.Classification.Classifier._useCase = value;
                                        }
                                    }
                                }
                                if(name == "ClassifierTemplateParameter") // Looking for class
                                {
                                    tree.Classification.__ClassifierTemplateParameter = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "allowSubstitutable") // Looking for property
                                        {
                                            tree.Classification.ClassifierTemplateParameter._allowSubstitutable = value;
                                        }
                                        if(name == "constrainingClassifier") // Looking for property
                                        {
                                            tree.Classification.ClassifierTemplateParameter._constrainingClassifier = value;
                                        }
                                        if(name == "parameteredElement") // Looking for property
                                        {
                                            tree.Classification.ClassifierTemplateParameter._parameteredElement = value;
                                        }
                                    }
                                }
                                if(name == "Feature") // Looking for class
                                {
                                    tree.Classification.__Feature = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "featuringClassifier") // Looking for property
                                        {
                                            tree.Classification.Feature._featuringClassifier = value;
                                        }
                                        if(name == "isStatic") // Looking for property
                                        {
                                            tree.Classification.Feature._isStatic = value;
                                        }
                                    }
                                }
                                if(name == "Generalization") // Looking for class
                                {
                                    tree.Classification.__Generalization = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "general") // Looking for property
                                        {
                                            tree.Classification.Generalization._general = value;
                                        }
                                        if(name == "generalizationSet") // Looking for property
                                        {
                                            tree.Classification.Generalization._generalizationSet = value;
                                        }
                                        if(name == "isSubstitutable") // Looking for property
                                        {
                                            tree.Classification.Generalization._isSubstitutable = value;
                                        }
                                        if(name == "specific") // Looking for property
                                        {
                                            tree.Classification.Generalization._specific = value;
                                        }
                                    }
                                }
                                if(name == "GeneralizationSet") // Looking for class
                                {
                                    tree.Classification.__GeneralizationSet = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "generalization") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet._generalization = value;
                                        }
                                        if(name == "isCovering") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet._isCovering = value;
                                        }
                                        if(name == "isDisjoint") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet._isDisjoint = value;
                                        }
                                        if(name == "powertype") // Looking for property
                                        {
                                            tree.Classification.GeneralizationSet._powertype = value;
                                        }
                                    }
                                }
                                if(name == "InstanceSpecification") // Looking for class
                                {
                                    tree.Classification.__InstanceSpecification = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "classifier") // Looking for property
                                        {
                                            tree.Classification.InstanceSpecification._classifier = value;
                                        }
                                        if(name == "slot") // Looking for property
                                        {
                                            tree.Classification.InstanceSpecification._slot = value;
                                        }
                                        if(name == "specification") // Looking for property
                                        {
                                            tree.Classification.InstanceSpecification._specification = value;
                                        }
                                    }
                                }
                                if(name == "InstanceValue") // Looking for class
                                {
                                    tree.Classification.__InstanceValue = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "instance") // Looking for property
                                        {
                                            tree.Classification.InstanceValue._instance = value;
                                        }
                                    }
                                }
                                if(name == "Operation") // Looking for class
                                {
                                    tree.Classification.__Operation = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "bodyCondition") // Looking for property
                                        {
                                            tree.Classification.Operation._bodyCondition = value;
                                        }
                                        if(name == "class") // Looking for property
                                        {
                                            tree.Classification.Operation._class = value;
                                        }
                                        if(name == "datatype") // Looking for property
                                        {
                                            tree.Classification.Operation._datatype = value;
                                        }
                                        if(name == "interface") // Looking for property
                                        {
                                            tree.Classification.Operation._interface = value;
                                        }
                                        if(name == "isOrdered") // Looking for property
                                        {
                                            tree.Classification.Operation._isOrdered = value;
                                        }
                                        if(name == "isQuery") // Looking for property
                                        {
                                            tree.Classification.Operation._isQuery = value;
                                        }
                                        if(name == "isUnique") // Looking for property
                                        {
                                            tree.Classification.Operation._isUnique = value;
                                        }
                                        if(name == "lower") // Looking for property
                                        {
                                            tree.Classification.Operation._lower = value;
                                        }
                                        if(name == "ownedParameter") // Looking for property
                                        {
                                            tree.Classification.Operation._ownedParameter = value;
                                        }
                                        if(name == "postcondition") // Looking for property
                                        {
                                            tree.Classification.Operation._postcondition = value;
                                        }
                                        if(name == "precondition") // Looking for property
                                        {
                                            tree.Classification.Operation._precondition = value;
                                        }
                                        if(name == "raisedException") // Looking for property
                                        {
                                            tree.Classification.Operation._raisedException = value;
                                        }
                                        if(name == "redefinedOperation") // Looking for property
                                        {
                                            tree.Classification.Operation._redefinedOperation = value;
                                        }
                                        if(name == "templateParameter") // Looking for property
                                        {
                                            tree.Classification.Operation._templateParameter = value;
                                        }
                                        if(name == "type") // Looking for property
                                        {
                                            tree.Classification.Operation._type = value;
                                        }
                                        if(name == "upper") // Looking for property
                                        {
                                            tree.Classification.Operation._upper = value;
                                        }
                                    }
                                }
                                if(name == "OperationTemplateParameter") // Looking for class
                                {
                                    tree.Classification.__OperationTemplateParameter = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "parameteredElement") // Looking for property
                                        {
                                            tree.Classification.OperationTemplateParameter._parameteredElement = value;
                                        }
                                    }
                                }
                                if(name == "Parameter") // Looking for class
                                {
                                    tree.Classification.__Parameter = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "default") // Looking for property
                                        {
                                            tree.Classification.Parameter._default = value;
                                        }
                                        if(name == "defaultValue") // Looking for property
                                        {
                                            tree.Classification.Parameter._defaultValue = value;
                                        }
                                        if(name == "direction") // Looking for property
                                        {
                                            tree.Classification.Parameter._direction = value;
                                        }
                                        if(name == "effect") // Looking for property
                                        {
                                            tree.Classification.Parameter._effect = value;
                                        }
                                        if(name == "isException") // Looking for property
                                        {
                                            tree.Classification.Parameter._isException = value;
                                        }
                                        if(name == "isStream") // Looking for property
                                        {
                                            tree.Classification.Parameter._isStream = value;
                                        }
                                        if(name == "operation") // Looking for property
                                        {
                                            tree.Classification.Parameter._operation = value;
                                        }
                                        if(name == "parameterSet") // Looking for property
                                        {
                                            tree.Classification.Parameter._parameterSet = value;
                                        }
                                    }
                                }
                                if(name == "ParameterSet") // Looking for class
                                {
                                    tree.Classification.__ParameterSet = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "condition") // Looking for property
                                        {
                                            tree.Classification.ParameterSet._condition = value;
                                        }
                                        if(name == "parameter") // Looking for property
                                        {
                                            tree.Classification.ParameterSet._parameter = value;
                                        }
                                    }
                                }
                                if(name == "Property") // Looking for class
                                {
                                    tree.Classification.__Property = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "aggregation") // Looking for property
                                        {
                                            tree.Classification.Property._aggregation = value;
                                        }
                                        if(name == "association") // Looking for property
                                        {
                                            tree.Classification.Property._association = value;
                                        }
                                        if(name == "associationEnd") // Looking for property
                                        {
                                            tree.Classification.Property._associationEnd = value;
                                        }
                                        if(name == "class") // Looking for property
                                        {
                                            tree.Classification.Property._class = value;
                                        }
                                        if(name == "datatype") // Looking for property
                                        {
                                            tree.Classification.Property._datatype = value;
                                        }
                                        if(name == "defaultValue") // Looking for property
                                        {
                                            tree.Classification.Property._defaultValue = value;
                                        }
                                        if(name == "interface") // Looking for property
                                        {
                                            tree.Classification.Property._interface = value;
                                        }
                                        if(name == "isComposite") // Looking for property
                                        {
                                            tree.Classification.Property._isComposite = value;
                                        }
                                        if(name == "isDerived") // Looking for property
                                        {
                                            tree.Classification.Property._isDerived = value;
                                        }
                                        if(name == "isDerivedUnion") // Looking for property
                                        {
                                            tree.Classification.Property._isDerivedUnion = value;
                                        }
                                        if(name == "isID") // Looking for property
                                        {
                                            tree.Classification.Property._isID = value;
                                        }
                                        if(name == "opposite") // Looking for property
                                        {
                                            tree.Classification.Property._opposite = value;
                                        }
                                        if(name == "owningAssociation") // Looking for property
                                        {
                                            tree.Classification.Property._owningAssociation = value;
                                        }
                                        if(name == "qualifier") // Looking for property
                                        {
                                            tree.Classification.Property._qualifier = value;
                                        }
                                        if(name == "redefinedProperty") // Looking for property
                                        {
                                            tree.Classification.Property._redefinedProperty = value;
                                        }
                                        if(name == "subsettedProperty") // Looking for property
                                        {
                                            tree.Classification.Property._subsettedProperty = value;
                                        }
                                    }
                                }
                                if(name == "RedefinableElement") // Looking for class
                                {
                                    tree.Classification.__RedefinableElement = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isLeaf") // Looking for property
                                        {
                                            tree.Classification.RedefinableElement._isLeaf = value;
                                        }
                                        if(name == "redefinedElement") // Looking for property
                                        {
                                            tree.Classification.RedefinableElement._redefinedElement = value;
                                        }
                                        if(name == "redefinitionContext") // Looking for property
                                        {
                                            tree.Classification.RedefinableElement._redefinitionContext = value;
                                        }
                                    }
                                }
                                if(name == "RedefinableTemplateSignature") // Looking for class
                                {
                                    tree.Classification.__RedefinableTemplateSignature = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "classifier") // Looking for property
                                        {
                                            tree.Classification.RedefinableTemplateSignature._classifier = value;
                                        }
                                        if(name == "extendedSignature") // Looking for property
                                        {
                                            tree.Classification.RedefinableTemplateSignature._extendedSignature = value;
                                        }
                                        if(name == "inheritedParameter") // Looking for property
                                        {
                                            tree.Classification.RedefinableTemplateSignature._inheritedParameter = value;
                                        }
                                    }
                                }
                                if(name == "Slot") // Looking for class
                                {
                                    tree.Classification.__Slot = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "definingFeature") // Looking for property
                                        {
                                            tree.Classification.Slot._definingFeature = value;
                                        }
                                        if(name == "owningInstance") // Looking for property
                                        {
                                            tree.Classification.Slot._owningInstance = value;
                                        }
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Classification.Slot._value = value;
                                        }
                                    }
                                }
                                if(name == "StructuralFeature") // Looking for class
                                {
                                    tree.Classification.__StructuralFeature = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isReadOnly") // Looking for property
                                        {
                                            tree.Classification.StructuralFeature._isReadOnly = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Actions") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException("value == null");
                                name = GetNameOfElement(value);
                                if(name == "ValueSpecificationAction") // Looking for class
                                {
                                    tree.Actions.__ValueSpecificationAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ValueSpecificationAction._result = value;
                                        }
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Actions.ValueSpecificationAction._value = value;
                                        }
                                    }
                                }
                                if(name == "VariableAction") // Looking for class
                                {
                                    tree.Actions.__VariableAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "variable") // Looking for property
                                        {
                                            tree.Actions.VariableAction._variable = value;
                                        }
                                    }
                                }
                                if(name == "WriteLinkAction") // Looking for class
                                {
                                    tree.Actions.__WriteLinkAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "WriteStructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.__WriteStructuralFeatureAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.WriteStructuralFeatureAction._result = value;
                                        }
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Actions.WriteStructuralFeatureAction._value = value;
                                        }
                                    }
                                }
                                if(name == "WriteVariableAction") // Looking for class
                                {
                                    tree.Actions.__WriteVariableAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Actions.WriteVariableAction._value = value;
                                        }
                                    }
                                }
                                if(name == "AcceptCallAction") // Looking for class
                                {
                                    tree.Actions.__AcceptCallAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "returnInformation") // Looking for property
                                        {
                                            tree.Actions.AcceptCallAction._returnInformation = value;
                                        }
                                    }
                                }
                                if(name == "AcceptEventAction") // Looking for class
                                {
                                    tree.Actions.__AcceptEventAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isUnmarshall") // Looking for property
                                        {
                                            tree.Actions.AcceptEventAction._isUnmarshall = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.AcceptEventAction._result = value;
                                        }
                                        if(name == "trigger") // Looking for property
                                        {
                                            tree.Actions.AcceptEventAction._trigger = value;
                                        }
                                    }
                                }
                                if(name == "Action") // Looking for class
                                {
                                    tree.Actions.__Action = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "context") // Looking for property
                                        {
                                            tree.Actions.Action._context = value;
                                        }
                                        if(name == "input") // Looking for property
                                        {
                                            tree.Actions.Action._input = value;
                                        }
                                        if(name == "isLocallyReentrant") // Looking for property
                                        {
                                            tree.Actions.Action._isLocallyReentrant = value;
                                        }
                                        if(name == "localPostcondition") // Looking for property
                                        {
                                            tree.Actions.Action._localPostcondition = value;
                                        }
                                        if(name == "localPrecondition") // Looking for property
                                        {
                                            tree.Actions.Action._localPrecondition = value;
                                        }
                                        if(name == "output") // Looking for property
                                        {
                                            tree.Actions.Action._output = value;
                                        }
                                    }
                                }
                                if(name == "ActionInputPin") // Looking for class
                                {
                                    tree.Actions.__ActionInputPin = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "fromAction") // Looking for property
                                        {
                                            tree.Actions.ActionInputPin._fromAction = value;
                                        }
                                    }
                                }
                                if(name == "AddStructuralFeatureValueAction") // Looking for class
                                {
                                    tree.Actions.__AddStructuralFeatureValueAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "insertAt") // Looking for property
                                        {
                                            tree.Actions.AddStructuralFeatureValueAction._insertAt = value;
                                        }
                                        if(name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.AddStructuralFeatureValueAction._isReplaceAll = value;
                                        }
                                    }
                                }
                                if(name == "AddVariableValueAction") // Looking for class
                                {
                                    tree.Actions.__AddVariableValueAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "insertAt") // Looking for property
                                        {
                                            tree.Actions.AddVariableValueAction._insertAt = value;
                                        }
                                        if(name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.AddVariableValueAction._isReplaceAll = value;
                                        }
                                    }
                                }
                                if(name == "BroadcastSignalAction") // Looking for class
                                {
                                    tree.Actions.__BroadcastSignalAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "signal") // Looking for property
                                        {
                                            tree.Actions.BroadcastSignalAction._signal = value;
                                        }
                                    }
                                }
                                if(name == "CallAction") // Looking for class
                                {
                                    tree.Actions.__CallAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isSynchronous") // Looking for property
                                        {
                                            tree.Actions.CallAction._isSynchronous = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.CallAction._result = value;
                                        }
                                    }
                                }
                                if(name == "CallBehaviorAction") // Looking for class
                                {
                                    tree.Actions.__CallBehaviorAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "behavior") // Looking for property
                                        {
                                            tree.Actions.CallBehaviorAction._behavior = value;
                                        }
                                    }
                                }
                                if(name == "CallOperationAction") // Looking for class
                                {
                                    tree.Actions.__CallOperationAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "operation") // Looking for property
                                        {
                                            tree.Actions.CallOperationAction._operation = value;
                                        }
                                        if(name == "target") // Looking for property
                                        {
                                            tree.Actions.CallOperationAction._target = value;
                                        }
                                    }
                                }
                                if(name == "Clause") // Looking for class
                                {
                                    tree.Actions.__Clause = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "body") // Looking for property
                                        {
                                            tree.Actions.Clause._body = value;
                                        }
                                        if(name == "bodyOutput") // Looking for property
                                        {
                                            tree.Actions.Clause._bodyOutput = value;
                                        }
                                        if(name == "decider") // Looking for property
                                        {
                                            tree.Actions.Clause._decider = value;
                                        }
                                        if(name == "predecessorClause") // Looking for property
                                        {
                                            tree.Actions.Clause._predecessorClause = value;
                                        }
                                        if(name == "successorClause") // Looking for property
                                        {
                                            tree.Actions.Clause._successorClause = value;
                                        }
                                        if(name == "test") // Looking for property
                                        {
                                            tree.Actions.Clause._test = value;
                                        }
                                    }
                                }
                                if(name == "ClearAssociationAction") // Looking for class
                                {
                                    tree.Actions.__ClearAssociationAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "association") // Looking for property
                                        {
                                            tree.Actions.ClearAssociationAction._association = value;
                                        }
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.ClearAssociationAction._object = value;
                                        }
                                    }
                                }
                                if(name == "ClearStructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.__ClearStructuralFeatureAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ClearStructuralFeatureAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ClearVariableAction") // Looking for class
                                {
                                    tree.Actions.__ClearVariableAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ConditionalNode") // Looking for class
                                {
                                    tree.Actions.__ConditionalNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "clause") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode._clause = value;
                                        }
                                        if(name == "isAssured") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode._isAssured = value;
                                        }
                                        if(name == "isDeterminate") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode._isDeterminate = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ConditionalNode._result = value;
                                        }
                                    }
                                }
                                if(name == "CreateLinkAction") // Looking for class
                                {
                                    tree.Actions.__CreateLinkAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "endData") // Looking for property
                                        {
                                            tree.Actions.CreateLinkAction._endData = value;
                                        }
                                    }
                                }
                                if(name == "CreateLinkObjectAction") // Looking for class
                                {
                                    tree.Actions.__CreateLinkObjectAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.CreateLinkObjectAction._result = value;
                                        }
                                    }
                                }
                                if(name == "CreateObjectAction") // Looking for class
                                {
                                    tree.Actions.__CreateObjectAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "classifier") // Looking for property
                                        {
                                            tree.Actions.CreateObjectAction._classifier = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.CreateObjectAction._result = value;
                                        }
                                    }
                                }
                                if(name == "DestroyLinkAction") // Looking for class
                                {
                                    tree.Actions.__DestroyLinkAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "endData") // Looking for property
                                        {
                                            tree.Actions.DestroyLinkAction._endData = value;
                                        }
                                    }
                                }
                                if(name == "DestroyObjectAction") // Looking for class
                                {
                                    tree.Actions.__DestroyObjectAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isDestroyLinks") // Looking for property
                                        {
                                            tree.Actions.DestroyObjectAction._isDestroyLinks = value;
                                        }
                                        if(name == "isDestroyOwnedObjects") // Looking for property
                                        {
                                            tree.Actions.DestroyObjectAction._isDestroyOwnedObjects = value;
                                        }
                                        if(name == "target") // Looking for property
                                        {
                                            tree.Actions.DestroyObjectAction._target = value;
                                        }
                                    }
                                }
                                if(name == "ExpansionNode") // Looking for class
                                {
                                    tree.Actions.__ExpansionNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "regionAsInput") // Looking for property
                                        {
                                            tree.Actions.ExpansionNode._regionAsInput = value;
                                        }
                                        if(name == "regionAsOutput") // Looking for property
                                        {
                                            tree.Actions.ExpansionNode._regionAsOutput = value;
                                        }
                                    }
                                }
                                if(name == "ExpansionRegion") // Looking for class
                                {
                                    tree.Actions.__ExpansionRegion = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "inputElement") // Looking for property
                                        {
                                            tree.Actions.ExpansionRegion._inputElement = value;
                                        }
                                        if(name == "mode") // Looking for property
                                        {
                                            tree.Actions.ExpansionRegion._mode = value;
                                        }
                                        if(name == "outputElement") // Looking for property
                                        {
                                            tree.Actions.ExpansionRegion._outputElement = value;
                                        }
                                    }
                                }
                                if(name == "InputPin") // Looking for class
                                {
                                    tree.Actions.__InputPin = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "InvocationAction") // Looking for class
                                {
                                    tree.Actions.__InvocationAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "argument") // Looking for property
                                        {
                                            tree.Actions.InvocationAction._argument = value;
                                        }
                                        if(name == "onPort") // Looking for property
                                        {
                                            tree.Actions.InvocationAction._onPort = value;
                                        }
                                    }
                                }
                                if(name == "LinkAction") // Looking for class
                                {
                                    tree.Actions.__LinkAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "endData") // Looking for property
                                        {
                                            tree.Actions.LinkAction._endData = value;
                                        }
                                        if(name == "inputValue") // Looking for property
                                        {
                                            tree.Actions.LinkAction._inputValue = value;
                                        }
                                    }
                                }
                                if(name == "LinkEndCreationData") // Looking for class
                                {
                                    tree.Actions.__LinkEndCreationData = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "insertAt") // Looking for property
                                        {
                                            tree.Actions.LinkEndCreationData._insertAt = value;
                                        }
                                        if(name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.LinkEndCreationData._isReplaceAll = value;
                                        }
                                    }
                                }
                                if(name == "LinkEndData") // Looking for class
                                {
                                    tree.Actions.__LinkEndData = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "end") // Looking for property
                                        {
                                            tree.Actions.LinkEndData._end = value;
                                        }
                                        if(name == "qualifier") // Looking for property
                                        {
                                            tree.Actions.LinkEndData._qualifier = value;
                                        }
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Actions.LinkEndData._value = value;
                                        }
                                    }
                                }
                                if(name == "LinkEndDestructionData") // Looking for class
                                {
                                    tree.Actions.__LinkEndDestructionData = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "destroyAt") // Looking for property
                                        {
                                            tree.Actions.LinkEndDestructionData._destroyAt = value;
                                        }
                                        if(name == "isDestroyDuplicates") // Looking for property
                                        {
                                            tree.Actions.LinkEndDestructionData._isDestroyDuplicates = value;
                                        }
                                    }
                                }
                                if(name == "LoopNode") // Looking for class
                                {
                                    tree.Actions.__LoopNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "bodyOutput") // Looking for property
                                        {
                                            tree.Actions.LoopNode._bodyOutput = value;
                                        }
                                        if(name == "bodyPart") // Looking for property
                                        {
                                            tree.Actions.LoopNode._bodyPart = value;
                                        }
                                        if(name == "decider") // Looking for property
                                        {
                                            tree.Actions.LoopNode._decider = value;
                                        }
                                        if(name == "isTestedFirst") // Looking for property
                                        {
                                            tree.Actions.LoopNode._isTestedFirst = value;
                                        }
                                        if(name == "loopVariable") // Looking for property
                                        {
                                            tree.Actions.LoopNode._loopVariable = value;
                                        }
                                        if(name == "loopVariableInput") // Looking for property
                                        {
                                            tree.Actions.LoopNode._loopVariableInput = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.LoopNode._result = value;
                                        }
                                        if(name == "setupPart") // Looking for property
                                        {
                                            tree.Actions.LoopNode._setupPart = value;
                                        }
                                        if(name == "test") // Looking for property
                                        {
                                            tree.Actions.LoopNode._test = value;
                                        }
                                    }
                                }
                                if(name == "OpaqueAction") // Looking for class
                                {
                                    tree.Actions.__OpaqueAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "body") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction._body = value;
                                        }
                                        if(name == "inputValue") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction._inputValue = value;
                                        }
                                        if(name == "language") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction._language = value;
                                        }
                                        if(name == "outputValue") // Looking for property
                                        {
                                            tree.Actions.OpaqueAction._outputValue = value;
                                        }
                                    }
                                }
                                if(name == "OutputPin") // Looking for class
                                {
                                    tree.Actions.__OutputPin = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Pin") // Looking for class
                                {
                                    tree.Actions.__Pin = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isControl") // Looking for property
                                        {
                                            tree.Actions.Pin._isControl = value;
                                        }
                                    }
                                }
                                if(name == "QualifierValue") // Looking for class
                                {
                                    tree.Actions.__QualifierValue = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "qualifier") // Looking for property
                                        {
                                            tree.Actions.QualifierValue._qualifier = value;
                                        }
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Actions.QualifierValue._value = value;
                                        }
                                    }
                                }
                                if(name == "RaiseExceptionAction") // Looking for class
                                {
                                    tree.Actions.__RaiseExceptionAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "exception") // Looking for property
                                        {
                                            tree.Actions.RaiseExceptionAction._exception = value;
                                        }
                                    }
                                }
                                if(name == "ReadExtentAction") // Looking for class
                                {
                                    tree.Actions.__ReadExtentAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "classifier") // Looking for property
                                        {
                                            tree.Actions.ReadExtentAction._classifier = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadExtentAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReadIsClassifiedObjectAction") // Looking for class
                                {
                                    tree.Actions.__ReadIsClassifiedObjectAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "classifier") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction._classifier = value;
                                        }
                                        if(name == "isDirect") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction._isDirect = value;
                                        }
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction._object = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadIsClassifiedObjectAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReadLinkAction") // Looking for class
                                {
                                    tree.Actions.__ReadLinkAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadLinkAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReadLinkObjectEndAction") // Looking for class
                                {
                                    tree.Actions.__ReadLinkObjectEndAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "end") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndAction._end = value;
                                        }
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndAction._object = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReadLinkObjectEndQualifierAction") // Looking for class
                                {
                                    tree.Actions.__ReadLinkObjectEndQualifierAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndQualifierAction._object = value;
                                        }
                                        if(name == "qualifier") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndQualifierAction._qualifier = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadLinkObjectEndQualifierAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReadSelfAction") // Looking for class
                                {
                                    tree.Actions.__ReadSelfAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadSelfAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReadStructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.__ReadStructuralFeatureAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadStructuralFeatureAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReadVariableAction") // Looking for class
                                {
                                    tree.Actions.__ReadVariableAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReadVariableAction._result = value;
                                        }
                                    }
                                }
                                if(name == "ReclassifyObjectAction") // Looking for class
                                {
                                    tree.Actions.__ReclassifyObjectAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isReplaceAll") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction._isReplaceAll = value;
                                        }
                                        if(name == "newClassifier") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction._newClassifier = value;
                                        }
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction._object = value;
                                        }
                                        if(name == "oldClassifier") // Looking for property
                                        {
                                            tree.Actions.ReclassifyObjectAction._oldClassifier = value;
                                        }
                                    }
                                }
                                if(name == "ReduceAction") // Looking for class
                                {
                                    tree.Actions.__ReduceAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "collection") // Looking for property
                                        {
                                            tree.Actions.ReduceAction._collection = value;
                                        }
                                        if(name == "isOrdered") // Looking for property
                                        {
                                            tree.Actions.ReduceAction._isOrdered = value;
                                        }
                                        if(name == "reducer") // Looking for property
                                        {
                                            tree.Actions.ReduceAction._reducer = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.ReduceAction._result = value;
                                        }
                                    }
                                }
                                if(name == "RemoveStructuralFeatureValueAction") // Looking for class
                                {
                                    tree.Actions.__RemoveStructuralFeatureValueAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isRemoveDuplicates") // Looking for property
                                        {
                                            tree.Actions.RemoveStructuralFeatureValueAction._isRemoveDuplicates = value;
                                        }
                                        if(name == "removeAt") // Looking for property
                                        {
                                            tree.Actions.RemoveStructuralFeatureValueAction._removeAt = value;
                                        }
                                    }
                                }
                                if(name == "RemoveVariableValueAction") // Looking for class
                                {
                                    tree.Actions.__RemoveVariableValueAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "isRemoveDuplicates") // Looking for property
                                        {
                                            tree.Actions.RemoveVariableValueAction._isRemoveDuplicates = value;
                                        }
                                        if(name == "removeAt") // Looking for property
                                        {
                                            tree.Actions.RemoveVariableValueAction._removeAt = value;
                                        }
                                    }
                                }
                                if(name == "ReplyAction") // Looking for class
                                {
                                    tree.Actions.__ReplyAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "replyToCall") // Looking for property
                                        {
                                            tree.Actions.ReplyAction._replyToCall = value;
                                        }
                                        if(name == "replyValue") // Looking for property
                                        {
                                            tree.Actions.ReplyAction._replyValue = value;
                                        }
                                        if(name == "returnInformation") // Looking for property
                                        {
                                            tree.Actions.ReplyAction._returnInformation = value;
                                        }
                                    }
                                }
                                if(name == "SendObjectAction") // Looking for class
                                {
                                    tree.Actions.__SendObjectAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "request") // Looking for property
                                        {
                                            tree.Actions.SendObjectAction._request = value;
                                        }
                                        if(name == "target") // Looking for property
                                        {
                                            tree.Actions.SendObjectAction._target = value;
                                        }
                                    }
                                }
                                if(name == "SendSignalAction") // Looking for class
                                {
                                    tree.Actions.__SendSignalAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "signal") // Looking for property
                                        {
                                            tree.Actions.SendSignalAction._signal = value;
                                        }
                                        if(name == "target") // Looking for property
                                        {
                                            tree.Actions.SendSignalAction._target = value;
                                        }
                                    }
                                }
                                if(name == "SequenceNode") // Looking for class
                                {
                                    tree.Actions.__SequenceNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "executableNode") // Looking for property
                                        {
                                            tree.Actions.SequenceNode._executableNode = value;
                                        }
                                    }
                                }
                                if(name == "StartClassifierBehaviorAction") // Looking for class
                                {
                                    tree.Actions.__StartClassifierBehaviorAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.StartClassifierBehaviorAction._object = value;
                                        }
                                    }
                                }
                                if(name == "StartObjectBehaviorAction") // Looking for class
                                {
                                    tree.Actions.__StartObjectBehaviorAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.StartObjectBehaviorAction._object = value;
                                        }
                                    }
                                }
                                if(name == "StructuralFeatureAction") // Looking for class
                                {
                                    tree.Actions.__StructuralFeatureAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.StructuralFeatureAction._object = value;
                                        }
                                        if(name == "structuralFeature") // Looking for property
                                        {
                                            tree.Actions.StructuralFeatureAction._structuralFeature = value;
                                        }
                                    }
                                }
                                if(name == "StructuredActivityNode") // Looking for class
                                {
                                    tree.Actions.__StructuredActivityNode = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "activity") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode._activity = value;
                                        }
                                        if(name == "edge") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode._edge = value;
                                        }
                                        if(name == "mustIsolate") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode._mustIsolate = value;
                                        }
                                        if(name == "node") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode._node = value;
                                        }
                                        if(name == "structuredNodeInput") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode._structuredNodeInput = value;
                                        }
                                        if(name == "structuredNodeOutput") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode._structuredNodeOutput = value;
                                        }
                                        if(name == "variable") // Looking for property
                                        {
                                            tree.Actions.StructuredActivityNode._variable = value;
                                        }
                                    }
                                }
                                if(name == "TestIdentityAction") // Looking for class
                                {
                                    tree.Actions.__TestIdentityAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "first") // Looking for property
                                        {
                                            tree.Actions.TestIdentityAction._first = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.TestIdentityAction._result = value;
                                        }
                                        if(name == "second") // Looking for property
                                        {
                                            tree.Actions.TestIdentityAction._second = value;
                                        }
                                    }
                                }
                                if(name == "UnmarshallAction") // Looking for class
                                {
                                    tree.Actions.__UnmarshallAction = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "object") // Looking for property
                                        {
                                            tree.Actions.UnmarshallAction._object = value;
                                        }
                                        if(name == "result") // Looking for property
                                        {
                                            tree.Actions.UnmarshallAction._result = value;
                                        }
                                        if(name == "unmarshallType") // Looking for property
                                        {
                                            tree.Actions.UnmarshallAction._unmarshallType = value;
                                        }
                                    }
                                }
                                if(name == "ValuePin") // Looking for class
                                {
                                    tree.Actions.__ValuePin = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Actions.ValuePin._value = value;
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
