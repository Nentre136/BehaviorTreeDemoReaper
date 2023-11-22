using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class AdvanceCombo : Action
{
    private BehaviorTree bt;
    private NavMeshAgent agent;
    private Animator animator;

    private GameObject attackTarget;
    private int _comboRange;
    private bool moving;
    private bool combo;
    private float _rotaSpeed = 9.0f;
    public override void OnStart()
    {
        bt = Reaper.Instance.bt;
        agent = Reaper.Instance.navMeshAgent;
        animator = Reaper.Instance.animator;

        attackTarget = (GameObject)bt.GetVariable("attackTarget").GetValue();
        
        _comboRange = 2;
        if ((Reaper.Instance.Health / Reaper.Instance.maxHealth) * 100 <= 20)
            _comboRange++;
        int comboModel = Random.Range(1, _comboRange+1);
        animator.SetInteger("comboModel", comboModel);
        moving = false;
        combo = false;
    }

    public override TaskStatus OnUpdate()
    {
        // 距离远才需要移动
        if (!moving && !combo &&
           Vector3.Distance(attackTarget.transform.position, transform.position) > 3.3f)
        {
            animator.SetTrigger("forward");
            moving = true;
        }

        if (Vector3.Distance(attackTarget.transform.position, transform.position) <= 3.0f)
        {
            if (!combo)
            {
                animator.SetTrigger("combo");
                combo = true;
            }
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //当前为攻击动画
        if ((stateInfo.IsName("scythe2HitComboA") || stateInfo.IsName("scythe2HitComboB")
            || stateInfo.IsName("scythe3HitCombo")) && combo)
        {
            // 播放进度
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
