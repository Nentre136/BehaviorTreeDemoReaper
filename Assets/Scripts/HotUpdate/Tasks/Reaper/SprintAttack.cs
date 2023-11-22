using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class SprintAttack : Action
{
    private BehaviorTree bt;
    private NavMeshAgent agent;
    private Animator animator;


    public float space = 3.1f;
    public float sprintSpeed = 25.0f;
    private float originSpeed;
    private Vector3 targetPosition;
    private bool attack;
    public override void OnStart()
    {
        bt = Reaper.Instance.bt;
        agent = Reaper.Instance.navMeshAgent;
        animator = Reaper.Instance.animator;

        originSpeed = agent.speed;
        agent.speed = sprintSpeed;
        attack = false;
        animator.SetTrigger("forward");
        targetPosition = ((GameObject)bt.GetVariable("attackTarget").GetValue())
            .transform.position;
        agent.SetDestination(targetPosition);
        //随机攻击模式
        int attackModel = Random.Range(1, 4);
        animator.SetInteger("attackModel", attackModel);
    }

    public override TaskStatus OnUpdate()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance<= space)
        {
            agent.isStopped = true;
            agent.speed = originSpeed;

            if (!attack)
            {
                animator.SetTrigger("attack");
                attack = true;
            }
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //当前为攻击动画
        if (stateInfo.IsName("scytheAttack1") || stateInfo.IsName("scytheAttack2")
            || stateInfo.IsName("scytheAttack3"))
        {
            // 获取当前动画的播放进度
            float progress = stateInfo.normalizedTime;
            if (progress >= 0.86)
            {
                agent.SetDestination(transform.position);
                agent.isStopped = false;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Running;
    }
}

