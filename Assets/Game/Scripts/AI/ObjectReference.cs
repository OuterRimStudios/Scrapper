using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class ObjectReference : MonoBehaviour
{
    public BehaviorTree behaviorTree;
    public string variableName;

    public void SetReference(Object reference)
    {
        behaviorTree.SetVariableValue(variableName, reference);
    }
}
