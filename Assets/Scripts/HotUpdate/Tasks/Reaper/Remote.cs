using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("����Ϊ'Զ'ʱ���ж�ΪSuccess;����Ϊ'��'ʱ���ж�ΪFailure")]
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
