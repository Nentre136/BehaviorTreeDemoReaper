using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotEntrance : MonoBehaviour
{
    public static void Start()
    {
        Debug.Log("热更新开始入口,Loading");
        Screen.SetResolution(1850, 1000, false);
        //加载
        GameObject loading = new GameObject("Loading");
        loading.AddComponent<ResourceManager>();
    }
}
