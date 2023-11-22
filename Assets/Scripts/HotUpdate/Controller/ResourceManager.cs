using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��Դ������
public class ResourceManager: MonoBehaviour
{
    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            return _instance;
        }
    }
    public bool LoadComplete
    {
        private set
        {
            _loadingComplete = value;
            //ÿ��������϶���رռ���ҳ��
            if (value)
                UnLoad_LoadPage();
        }
        get
        {
            return _loadingComplete;
        }
    }
    private bool _loadingComplete;
    private GameObject loadPage;
    private void Start()
    {
        _instance = this;
        LoadComplete = false;
        StartCoroutine(AsyncLoadingUI());
        StartCoroutine(AsyncLoadingCombat());
    }
    //��Ϸ���� �첽
    IEnumerator AsyncLoadingCombat()
    {
        AssetBundleCreateRequest 
            ab_combat = AssetBundle.LoadFromFileAsync(Config.ABPath + "/combat");
        yield return ab_combat;
        LoadComplete = true;

        GameObject fps = new GameObject("fps");
        fps.AddComponent<GetFPS>();

        //UI
        GameObject canvas = Instantiate(ab_combat.assetBundle.LoadAsset<GameObject>("CombatUI"));
        canvas.name = "Canvas";

        //Player
        GameObject player = Instantiate(ab_combat.assetBundle.LoadAsset<GameObject>("Player"));
        player.name = "Player";

        //PhotoGrapher
        GameObject photoGrapher = Instantiate(ab_combat.assetBundle.LoadAsset<GameObject>("PhotoGrapher"));
        photoGrapher.name = "PhotoGrapher";
        photoGrapher.GetComponent<GrapherController>().followTarget = player.transform.Find("Eyes").gameObject;

        //Reaper
        GameObject reaperInstance = Instantiate(ab_combat.assetBundle.LoadAsset<GameObject>("Reaper"));
        reaperInstance.name = "Reaper";
        Reaper.Instance.laser = ab_combat.assetBundle.LoadAsset<GameObject>("MagicRay");
        Reaper.Instance.shockWave = ab_combat.assetBundle.LoadAsset<GameObject>("ShockWave");
        
        //GameManager
        GameObject gameManager = Instantiate(ab_combat.assetBundle.LoadAsset<GameObject>("GameManager"));
        gameManager.name = "GameManager";

        //ж��AB��������������Դ
        ab_combat.assetBundle.Unload(false);
        Resources.UnloadUnusedAssets();
        yield break;
    }
    IEnumerator AsyncLoadingUI()
    {
        AssetBundleCreateRequest
            ab_load = AssetBundle.LoadFromFileAsync(Config.ABPath + "/load");
        yield return ab_load;

        loadPage = Instantiate(ab_load.assetBundle.LoadAsset<GameObject>("LoadPage"));
        loadPage.name = "LoadPage";

        ab_load.assetBundle.Unload(false);
        Resources.UnloadUnusedAssets();
        yield break;
    }

    private void UnLoad_LoadPage()
    {
        loadPage.SetActive(false);
    }
}
