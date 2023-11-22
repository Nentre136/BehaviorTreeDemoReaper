using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
[TaskDescription("ȷ��������Χ.healthRange����ʾ����Value���µķ���Success��" +
    "���򷵻�Failure;ȡֵ��ʾѪ���İٷֱ�")]
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
