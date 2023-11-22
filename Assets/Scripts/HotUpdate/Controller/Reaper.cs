using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

public class Reaper : MonoBehaviour
{
    private static Reaper _instance;
    //����һ������
    public static Reaper Instance
    {
        get
        {
            return _instance;
        }
    }
    public BehaviorTree bt;
    public Rigidbody rb;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    //����������perfab
    public GameObject laser;
    public GameObject shockWave;

    public bool shootComplete { get; private set; }
    public bool isTurn { get; private set; }
    public Transform build { get; private set; }
    public Transform foot { get; private set; }
    public float maxHealth = 100;
    private float _health;
    //�������Է�����
    public float Health
    {
        get
        {
            return _health;
        }
        private set
        {
            float oldValue = _health;
            _health = value;
            StartCoroutine(GameManager.Instance.ChangeHealthBar(oldValue, _health));
            if (_health <= 0)
                bt.SetVariableValue("death", true);
        }
    }
    private void Awake()
    {
        _instance = this;
        bt = GetComponent<BehaviorTree>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        _health = maxHealth;
    }
    private void Start()
    {
        shootComplete = false;
        isTurn = false;
        build = transform.Find("build").transform;
        foot = transform.Find("foot").transform;
    }


    public void ShootComplete(int value) {

        if (value == 1)
            OnLaser();
        shootComplete = value == 1 ? true : false;
    }
    //�����Ƿ��ܹ�ת��
    public void FaceToPlayer(int value)
    {
        isTurn = value == 1 ? true : false;
    }
    //����Laser
    private void OnLaser()
    {
        //����parent ��֤����һ��
        GameObject laserInstan = GameObject.Instantiate(laser, build);
        laserInstan.transform.localPosition = Vector3.zero;
        laserInstan.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        laserInstan.transform.parent = null;
    }
    //����ShockWave
    public void CreateShockWave()
    {
        GameObject waveInstan = GameObject.Instantiate(shockWave, foot);
        waveInstan.transform.localPosition = new Vector3(0,0.1f,0);
        waveInstan.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        waveInstan.transform.parent = null;
    }

    public void OnAttackHit(int model)
    {
        string hitName = "hit" + model.ToString();
        BoxCollider hitBox = transform.Find(hitName).GetComponent<BoxCollider>();
        hitBox.enabled = true;
    }
    public void OffAttackHit(int model)
    {
        string hitName = "hit" + model.ToString();
        BoxCollider hitBox = transform.Find(hitName).GetComponent<BoxCollider>();
        hitBox.enabled = false;
    }
    public void GetHit(float value)
    {
        if (Health - value < 0)
            return;
        Health -= value;
    }
}
