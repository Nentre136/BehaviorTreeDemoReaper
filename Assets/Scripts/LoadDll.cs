using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public class LoadDll : MonoBehaviour
{
    void Start()
    {
        // Editor�����£�HotUpdate.dll.bytes�Ѿ����Զ����أ�����Ҫ���أ��ظ����ط���������⡣
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdateDef.dll.bytes"));
#else
        // Editor��������أ�ֱ�Ӳ��һ��HotUpdate����
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdateDef");
#endif

        Type type = hotUpdateAss.GetType("HotEntrance");
        type.GetMethod("Start").Invoke(null, null);
        DontDestroyOnLoad(this);
        
    }
}
