using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("�ж�bool����;Ĭ��isReverseΪfalseʱ��variablesΪtrue����Success;" +
    "isReverseΪtrueʱ��Ч����ת")]
public class IsVariables : Conditional
{
    public SharedBool variables;
    //�Ƿ��ת
    public bool isReverse = false;
    public override TaskStatus OnUpdate()
    {
        if(!isReverse)
            return variables.Value ? TaskStatus.Success : TaskStatus.Failure; 
        else
            return variables.Value ? TaskStatus.Failure : TaskStatus.Success;
    }
}
