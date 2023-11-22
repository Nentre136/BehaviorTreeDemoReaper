using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
[TaskDescription("waitTime：等待一段时间再开始转向  转向成功后会给目标位置赋值")]
public class FaceToPlayer : Action
{
    private GameObject target;
    private BehaviorTree bt;

    private Vector3 targetPosition;
    public float rotaSpeed = 8.0f;
    public float waitTime = 0f;
    private bool isStart;

    public override void OnStart()
    {
        bt = Reaper.Instance.bt;
        target = bt.GetVariable("attackTarget").GetValue() as GameObject;
        targetPosition = target.transform.position;
        isStart = false;
        StartCoroutine(DelayRotate());
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 positionOffset = targetPosition - transform.position;
        positionOffset.y = 0;

        //确定目标角度
        Quaternion targetRotation = Quaternion.LookRotation(positionOffset);
        float offsetRotation = Quaternion.Angle(transform.rotation, targetRotation);
        //偏角小于0.2度 则旋转成功
        if (isStart && offsetRotation <= 0.2f)
        {
            bt.SetVariableValue("targetPosition", targetPosition);
            return TaskStatus.Success;
        }
        else if(isStart)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotaSpeed
            );
        }
        return TaskStatus.Running;
    }
    IEnumerator DelayRotate()
    {
        yield return new WaitForSeconds(waitTime);
        isStart = true;
    }
}
