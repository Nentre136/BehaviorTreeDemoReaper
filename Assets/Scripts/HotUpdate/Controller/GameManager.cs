//using BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime.Tasks;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private PlayerController player;

    public Button hit;
    private Image energyBar;
    private Image energyBuffer;
    private Image healthBar;
    private Image healthBuffer;
    private Image tiredBar;
    private float uiChangeTime = 0.3f;
    private float bufferChangeTime = 1.0f;
    private float tiredRecoverTime = 5.0f;
    private float energyMaxBarLength;
    private float healthMaxBarLength;
    //������Э�� ȷ��ֻ��һ��Э�̴�������
    private Coroutine energyBufferCorou;
    private Coroutine healthBufferCorou;

    private void Awake()
    {
        _instance = this;
        //�л���������
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        hit = GameObject.Find("Canvas/HitButton").GetComponent<Button>();
        hit.onClick.AddListener(() => {
            if(Reaper.Instance.Health - 10 >= 0)
            {
                Reaper.Instance.GetHit(10);
            }
        });
        energyBar = GameObject.Find("Canvas/EnergyBar/Energy").GetComponent<Image>();
        energyBuffer = GameObject.Find("Canvas/EnergyBar/Buffer").GetComponent<Image>();
        healthBar = GameObject.Find("Canvas/BossHealthBar/Health").GetComponent<Image>();
        healthBuffer = GameObject.Find("Canvas/BossHealthBar/Buffer").GetComponent<Image>();
        tiredBar = GameObject.Find("Canvas/TiredBar/Tired").GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        energyMaxBarLength = energyBar.transform.parent.GetComponent<Image>().
            rectTransform.sizeDelta.x;
        healthMaxBarLength = healthBar.transform.parent.GetComponent<Image>().
            rectTransform.sizeDelta.x;
    }
    //���ľ�����
    public IEnumerator ChangeEnergyBar(float oldValue, float targetValue)
    {
        float oldLength = energyMaxBarLength * (oldValue / player.maxEnergy);
        float targetLength = energyMaxBarLength * (targetValue / player.maxEnergy);

        //�仯����Ҫ����
        if (Mathf.Abs(oldValue - targetValue) >= 5 && energyBufferCorou==null)
        {
            energyBufferCorou = StartCoroutine(ChangeEnergyBuffer(oldValue));
        }//�仯С ��û��Э�����У������뾫����ͬ�����軺��
        else if(Mathf.Abs(oldValue - targetValue) < 5 && energyBufferCorou == null)
        {
            
            energyBuffer.rectTransform.sizeDelta = new Vector2(
                targetLength,
                energyBuffer.rectTransform.sizeDelta.y
            );
        }

        float clock = 0;
        while (clock < uiChangeTime)
        {
            //���������仯���˳�
            if (targetValue != player.Energy)
                yield break;
            //���㵱ǰ��ʱ���Ľ���
            float progess = Mathf.Clamp01(clock / uiChangeTime);

            float offsetLength = Mathf.Lerp(
                oldLength,
                targetLength,
                progess
            );

            //ÿ��ˢ�¸���һ�γ���
            energyBar.rectTransform.sizeDelta = new Vector2(
                offsetLength,
                energyBar.rectTransform.sizeDelta.y
            );

            clock += Time.deltaTime;
            //�ȴ�һ֡
            yield return null;
        }

        energyBar.rectTransform.sizeDelta = new Vector2(
            targetLength,
            energyBar.rectTransform.sizeDelta.y
        );
        energyBar.transform.parent.Find("Value").GetComponent<Text>().
            text = targetValue.ToString("F0");
    }
    //����������
    IEnumerator ChangeEnergyBuffer(float oldValue)
    {
        yield return new WaitForSeconds(1.2f);

        float oldLength = energyMaxBarLength * (oldValue / player.maxEnergy);
        float targetLength = 0;
        float clock = 0;
        while (clock < bufferChangeTime)
        {
            //Ŀ�곤����Ҫʵʱ���µ���ǰ��������λ��
            targetLength = energyMaxBarLength * (player.Energy / player.maxEnergy);
            //���㵱ǰ��ʱ���Ľ���
            float progess = Mathf.Clamp01(clock / uiChangeTime);

            float offsetLength = Mathf.Lerp(
                oldLength,
                targetLength,
                progess
            );

            //ÿ��ˢ�¸���һ�γ��� energyBuffer
            energyBuffer.rectTransform.sizeDelta = new Vector2(
                offsetLength,
                energyBuffer.rectTransform.sizeDelta.y
            );
            //�ӽ�Ŀ�����˳�
            if (Mathf.Abs(energyBuffer.rectTransform.sizeDelta.x - targetLength) <= 0.1f)
                break;
            clock += Time.deltaTime;
            //�ȴ�һ֡
            yield return null;
        }

        energyBuffer.rectTransform.sizeDelta = new Vector2(
            targetLength,
            energyBuffer.rectTransform.sizeDelta.y
        );
        //Э������ÿ�
        energyBufferCorou = null;
    }
    //����Ѫ����
    public IEnumerator ChangeHealthBar(float oldValue, float targetValue)
    {
        //�仯����Ҫ����
        if (Mathf.Abs(oldValue - targetValue) >= 5 && healthBufferCorou == null)
        {
            healthBufferCorou = StartCoroutine(ChangeHealthBuffer(oldValue));
        }

        float oldLength = healthMaxBarLength * (oldValue / Reaper.Instance.maxHealth);
        float targetLength = healthMaxBarLength * (targetValue / Reaper.Instance.maxHealth);

        float clock = 0;
        while (clock < uiChangeTime)
        {
            if (targetValue != Reaper.Instance.Health)
                yield break;
            //���㵱ǰ��ʱ���Ľ���
            float progess = Mathf.Clamp01(clock / uiChangeTime);

            float offsetLength = Mathf.Lerp(
                oldLength,
                targetLength,
                progess
            );

            //ÿ��ˢ�¸���һ�γ���
            healthBar.rectTransform.sizeDelta = new Vector2(
                offsetLength,
                healthBar.rectTransform.sizeDelta.y
            );

            clock += Time.deltaTime;
            //�ȴ�һ֡
            yield return null;
        }

        healthBar.rectTransform.sizeDelta = new Vector2(
            targetLength,
            healthBar.rectTransform.sizeDelta.y
        );
    }
    IEnumerator ChangeHealthBuffer(float oldValue)
    {
        yield return new WaitForSeconds(1.2f);

        float oldLength = healthMaxBarLength * (oldValue / Reaper.Instance.maxHealth);
        float targetLength = 0;
        float clock = 0;
        while (clock < bufferChangeTime)
        {
            //Ŀ�곤����Ҫʵʱ���µ���ǰѪ������λ��
            targetLength = healthMaxBarLength * (Reaper.Instance.Health / Reaper.Instance.maxHealth);
            //���㵱ǰ��ʱ���Ľ���
            float progess = Mathf.Clamp01(clock / uiChangeTime);

            float offsetLength = Mathf.Lerp(
                oldLength,
                targetLength,
                progess
            );

            //ÿ��ˢ�¸���һ�γ��� healthBuffer
            healthBuffer.rectTransform.sizeDelta = new Vector2(
                offsetLength,
                healthBuffer.rectTransform.sizeDelta.y
            );
            //�ӽ�Ŀ�����˳�
            if(Mathf.Abs(healthBuffer.rectTransform.sizeDelta.x-targetLength)<=0.1f)
                break;
            clock += Time.deltaTime;
            //�ȴ�һ֡
            yield return null;
        }

        healthBuffer.rectTransform.sizeDelta = new Vector2(
            targetLength,
            healthBuffer.rectTransform.sizeDelta.y
        );
        //Э������ÿ�
        healthBufferCorou = null;
    }
    //ƣ�ͻָ�
    public IEnumerator RecoverTired()
    {
        tiredBar.enabled = true;
        tiredBar.transform.parent.GetComponent<Image>().enabled = true;
        energyBar.enabled = false;
        tiredBar.fillAmount = 0;
        float clock = 0;
        while (clock < tiredRecoverTime)
        {
            //���㵱ǰ��ʱ���Ľ���
            float progess = Mathf.Clamp01(clock / tiredRecoverTime);
            tiredBar.fillAmount = progess;
            clock += Time.deltaTime;
            //�ȴ�һ֡
            yield return null;
        }
        tiredBar.fillAmount = 1;
        energyBar.enabled = true;
        player.isTired = false;
        tiredBar.enabled = false;
        tiredBar.transform.parent.GetComponent<Image>().enabled = false;
    }
}
