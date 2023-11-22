using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
public class RouseEnemy : MonoBehaviour
{
    private BehaviorTree bt;
    private void Start()
    {
        bt = Reaper.Instance.bt;
    }
    private void OnTriggerEnter(Collider other)
    {
        //发现玩家
        if (other.tag == "Player")
        {
            //首次发现
            if ((bool)bt.GetVariable("isIdle").GetValue())
            {
                bt.SetVariableValue("isIdle", false);
                bt.SetVariableValue("attackTarget",GameObject.FindGameObjectWithTag("Player"));
                //发送事件 唤醒Enemy
                bt.SendEvent("Wake");
                SphereCollider sc = GetComponent<SphereCollider>();
                sc.enabled = false;
            } 
        }
    }
}
