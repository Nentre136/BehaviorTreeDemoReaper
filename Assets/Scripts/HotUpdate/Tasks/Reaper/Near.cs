using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("����Ϊ'Զ'ʱ���ж�ΪFailure;����Ϊ'��'ʱ���ж�ΪSuccess")]
public class Near : Conditional
{
    private Vector3 targetPosition;
    private BehaviorTree bt;
    public override void OnStart()
    {
        bt = Reaper.Instance.bt;
        targetPosition = (bt.GetVariable("attackTarget").GetValue() as GameObject).transform.position;
    }
    public override TaskStatus OnUpdate()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= 15.0f)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
