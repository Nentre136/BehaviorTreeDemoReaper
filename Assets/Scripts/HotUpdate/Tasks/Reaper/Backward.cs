using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class Backward : Action
{
    private Animator animator;
    private NavMeshAgent agent;

    public float distance = 18.0f;
    public float sprintSpeed = 18.0f;
    private float originSpeed;
    //移动方向
    private Vector3 movePosition;
    //目标位置向量
    private Vector3 targetPosition;
    private float sprintTime = 0.8f;
    private float clock;

    private float curSprintProgress;
    public override void OnStart()
    {
        animator = Reaper.Instance.animator;
        agent = Reaper.Instance.navMeshAgent;

        //冲刺期间navMash不转向
        agent.updateRotation = false;
        originSpeed = agent.speed;
        agent.speed = sprintSpeed;
        clock = 0;


       animator.SetTrigger("backward");
       movePosition = -transform.forward * distance;

        //冲刺目标位置: 当前向量 + 移动向量
        targetPosition = transform.position + movePosition;
    }
    public override TaskStatus OnUpdate()
    {
        if (clock >= sprintTime)
        {
            //归还设置
            agent.SetDestination(transform.position);
            agent.updateRotation = true;
            agent.speed = originSpeed;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnFixedUpdate()
    {
        if (clock <= sprintTime)
        {
            clock += Time.fixedDeltaTime;
            agent.SetDestination(targetPosition);
        }
    }
}
