using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeNode : Node
{
    private float range;
    private Transform[] target;
    private Transform origin;

    public InRangeNode(float range, Transform[] target, Transform origin)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        foreach(var tar in target)
        {
            float dist = Vector3.Distance(tar.position, origin.position);
            _nodeState = dist < range ? NodeState.SUCCESS : NodeState.FAILURE;

            if(_nodeState == NodeState.SUCCESS)
                return _nodeState;
        }
       
        return _nodeState;
    }
}
