using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class AdvanceAttack : Action
{
    private BehaviorTree bt;
    private NavMeshAgent agent;
    private Animator animator;

    private GameObject attackTarget;
    private bool moving;
    private bool attack;
    private float _rotaSpeed = 9.0f;
    public override void OnStart()
    {
        bt = Reaper.Instance.bt;
        agent = Reaper.Instance.navMeshAgent;
        animator = Reaper.Instance.animator;
        attackTarget = (GameObject)bt.GetVariable("attackTarget").GetValue();
        moving = false;
        attack = false;
        //随机攻击模式
        int attackModel = Random.Range(1, 4);
        animator.SetInteger("attackModel", attackModel);
    }

    public override TaskStatus OnUpdate()
    {
        if (!moving && !attack &&
           Vector3.Distance(attackTarget.transform.position, transform.position) > 3.0f)
        {
            animator.SetTrigger("forward");
            moving = true;
        }

        if (Vector3.Distance(attackTarget.transform.position, transform.position) <= 3.0f)
        {
            if (!attack)
            {
                animator.SetTrigger("attack");
                attack = true;
            }
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //当前为攻击动画
        if ((stateInfo.IsName("scytheAttack1") || stateInfo.IsName("scytheAttack2")
            || stateInfo.IsName("scytheAttack3")) && attack)
        {
            // 获取当前动画的播放进度
            float progress = stateInfo.normalizedTime;
            if (progress >= 0.86)
            {
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Running;
    }

    public override void OnFixedUpdate()
    {
        if (Reaper.Instance.isTurn)
        {
            //获取指向目标方向
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.y = 0;
            //获取指向目标的角度
            Quaternion targetRota = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRota,
                Time.deltaTime * _rotaSpeed
            );
        }
        agent.SetDestination(attackTarget.transform.position);
    }
}
