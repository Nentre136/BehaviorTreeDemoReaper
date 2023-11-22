using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("判断bool类型;默认isReverse为false时，variables为true返回Success;" +
    "isReverse为true时则效果反转")]
public class IsVariables : Conditional
{
    public SharedBool variables;
    //是否调转
    public bool isReverse = false;
    public override TaskStatus OnUpdate()
    {
        if(!isReverse)
            return variables.Value ? TaskStatus.Success : TaskStatus.Failure; 
        else
            return variables.Value ? TaskStatus.Failure : TaskStatus.Success;
    }
}
