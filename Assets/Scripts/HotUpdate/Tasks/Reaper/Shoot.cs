using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class Shoot : Action
{
    private Animator animator;
    private BehaviorTree bt;

    public float rotaSpeed = 5.0f;
    private GameObject attackTarget;
    public override void OnStart()
    {
        animator = Reaper.Instance.animator;
        bt = Reaper.Instance.bt;
        animator.SetTrigger("shoot");
        attackTarget = (GameObject)bt.GetVariable("attackTarget").GetValue();

    }
    public override TaskStatus OnUpdate()
    {
        if (Reaper.Instance.shootComplete)
        {   
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnFixedUpdate()
    {
        if (!Reaper.Instance.shootComplete)
        {
            //始终面向Player
            Vector3 positionOffset = attackTarget.transform.position - transform.position;
            positionOffset.y = 0;

            //确定目标角度
            Quaternion targetRotation = Quaternion.LookRotation(positionOffset);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotaSpeed
            );
        }
    }
}
