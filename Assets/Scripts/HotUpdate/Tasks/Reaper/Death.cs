using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Death : Action
{
    private Animator animator;
    private BehaviorTree bt;
    public override void OnStart()
    {
        animator = Reaper.Instance.animator;
        bt = Reaper.Instance.bt;
        animator.SetTrigger("death");
        bt.SetVariableValue("death", false);
        
        GameObject.Find("Canvas/HitButton").SetActive(false);
        GameObject.Find("Canvas/BossHealthBar").SetActive(false);
    }
}
