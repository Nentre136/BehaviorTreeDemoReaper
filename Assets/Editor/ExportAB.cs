using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Editor;
using UnityEngine.UI;
public class ExportAB
{
    [MenuItem("ExportAB/Windows")]
    public static void CreatAB_Windows()
    {
        string abpath = Config.ABPath;
        
        //创建abPath目录 判断文件夹是否存在
        if (!Directory.Exists(abpath))
        {
            Directory.CreateDirectory(abpath);
        }
        //(路径，导出选项，平台) 不同平台的AB包格式不同
        BuildPipeline.BuildAssetBundles(abpath,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows);

        Debug.Log("导出成功AB包，存放路径：" + abpath);
        
    }
}
