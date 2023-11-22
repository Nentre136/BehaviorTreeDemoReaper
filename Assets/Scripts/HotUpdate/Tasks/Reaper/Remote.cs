using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("距离为'远'时，判定为Success;距离为'近'时，判定为Failure")]
public class Remote : Conditional
{
    private Vector3 targetPosition;
    private BehaviorTree bt;
    public override void OnStart()
    {
        bt = GetComponent<BehaviorTree>();
        targetPosition = (bt.GetVariable("attackTarget").GetValue() as GameObject).transform.position;
    }
    public override TaskStatus OnUpdate()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= 15.0f)
        {
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }

    }
}
