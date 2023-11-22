using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SKillA : Action
{
    private Animator animator;

    private bool isStart;
    private float progress;
    public override void OnStart()
    {
        animator = Reaper.Instance.animator;
        animator.SetTrigger("shockWave");
        isStart = false;
    }
    public override TaskStatus OnUpdate()
    {
        AnimatorClipInfo[] clipInfoArray = animator.GetCurrentAnimatorClipInfo(0);
        //避免访问为空
        if (clipInfoArray.Length > 0)
        {
            // 获取当前动画名字
            string clipName = clipInfoArray[0].clip.name;
            if(clipName== "castSpellA")
            {
                isStart = true;
            }
        }
        if (isStart)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            progress = stateInfo.normalizedTime;
            if (progress >= 0.89)
                return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
