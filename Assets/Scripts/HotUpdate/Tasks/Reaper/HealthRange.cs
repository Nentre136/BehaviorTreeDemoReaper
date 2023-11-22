using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
[TaskDescription("确定生命范围.healthRange：表示满足Value以下的返回Success，" +
    "否则返回Failure;取值表示血量的百分比")]
public class HealthRange : Conditional
{
    public float healthRange;
    public override TaskStatus OnUpdate()
    {
        if ((Reaper.Instance.Health / Reaper.Instance.maxHealth)*100 <= healthRange)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
