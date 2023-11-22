using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject camereGrapher;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    private float currentMoveSpeed;
    public float moveSpeed = 5.0f;
    public float runSpeed = 8.0f;
    public float rotaSpeed = 5.0f;
    public float jumpForce = 50.0f;
    public float sprintForce = 100.0f;
    public bool isTired;
    //冲刺计时器
    private float sprintClock;
    private float sprintTime=0.8f;
    private ParticleSystem sprintLight;
    //无敌计时器
    public float godClock { get; private set; }
    private float rollGodTime = 3.0f;
    private float hitGodTime = 6.0f;
    private bool isJumping;
    //跳跃移动阻力
    private float jumpMoveObstruct= 0.4f;
    //下坠加速
    private float fallMultip = 1.9f;
    private float _energy;
    public float maxEnergy = 70;
    //跑步减少精力速度
    private float runReduceEnergySpeed = 4.0f;
    //精力恢复速度
    private float energyRecoverSpeed = 8.0f;
    //使用精力后恢复计时器
    private float energyRecoverClock;
    private float energyRecoverTime = 1.3f;


    //水平按键映射
    private float keyX;
    private float keyY;
   public float Energy
    {
        get
        {
            return _energy;
        }
        set
        {
            float oldValue = _energy;
            _energy = value;
            StartCoroutine(GameManager.Instance.ChangeEnergyBar(oldValue,_energy));
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        camereGrapher = GameObject.Find("PhotoGrapher");
        sprintLight = transform.Find("SprintLight").GetComponent<ParticleSystem>();
        currentMoveSpeed = moveSpeed;
        _energy = maxEnergy;
        isJumping = false;
        isTired = false;
        sprintClock = 0;
        godClock = 0;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        //跳
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            Jump();
        }
        //冲刺
        if(!isTired && !isJumping && sprintClock<=0 && Energy - 10 > 0 
            && Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprintClock = sprintTime;
            Sprint();
        }
        //跑步
        if(!isJumping && !isTired && Input.GetKey(KeyCode.LeftShift))
        {
            RunReduceEnergy();
        }
        //行走
        if((currentMoveSpeed==runSpeed && Input.GetKeyUp(KeyCode.LeftShift)) || isTired)
            currentMoveSpeed = moveSpeed;
            

        //冲刺冷却
        if (sprintClock > 0)
            sprintClock -= Time.deltaTime;
        //无敌时间计时
        if (godClock > 0)
            godClock -= Time.fixedDeltaTime;
        //使用精力后恢复计时器
        if (energyRecoverClock > 0)
            energyRecoverClock -= Time.deltaTime;

        //下落加速
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultip - 1) * Time.deltaTime;
        }

        //恢复精力
        if (energyRecoverClock <= 0 && Energy < maxEnergy)
        {
            RecoverEnergy();
        }
        //进入疲劳状态
        if (Energy < 1 && !isTired)
        {
            isTired = true;
            OnTired();
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        keyX = Input.GetAxisRaw("Horizontal");
        keyY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(keyX, 0, keyY).normalized;
        //设Camere正前方为角色正方向
        //四元 * 向量  =  旋转后的向量
        move = camereGrapher.GetComponent<GrapherController>().rota * move;
        //防止有y轴的移动向量
        move.y = 0;
        if (keyX != 0 || keyY != 0)
        {
            float jumpMoveSpeed = 1.0f;
            //在跳跃过程中，移动有阻力
            if (isJumping)
                jumpMoveSpeed = jumpMoveObstruct;
            Vector3 movePosition = move * currentMoveSpeed * jumpMoveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movePosition);

            //根据movePosition获取player旋转角度
            Quaternion rota = Quaternion.LookRotation(movePosition);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rota,
                rotaSpeed * Time.deltaTime
            );
        }
    }
    //粒子技能独立处理
    private void OnTriggerStay(Collider other)
    {
        //没有无敌状态
        if (other.tag == "Hit" && godClock <= 0)
        {
            StartCoroutine(_OnGetHit());
            //受击冷却时间
            godClock = hitGodTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //落地
        if(isJumping && LayerMask.LayerToName(collision.gameObject.layer) == "Plane")
        {
            isJumping = false;
        }
    }
    private void Jump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }
    private void Sprint()
    {
        if(Energy - 10 > 0)
        {
            Vector3 sprintDirec = new Vector3(keyX, 0, keyY).normalized;
            //获取自由视角下的相对前后左右
            sprintDirec = camereGrapher.GetComponent<GrapherController>().rota * sprintDirec;
            sprintDirec.y = 0;
            rb.AddForce(sprintDirec * sprintForce, ForceMode.Impulse);
            //冲刺特效 停止清除播放
            sprintLight.Stop();
            sprintLight.Clear();
            sprintLight.Play();
            godClock = rollGodTime;
            Energy -= 10;
            //跑步状态不可恢复精力
            energyRecoverClock = energyRecoverTime;
        }
    }
    public void GetHit()
    {
        StartCoroutine(_OnGetHit());
    }
    IEnumerator _OnGetHit()
    {
        Material bodyMater = meshRenderer.material;
        Color hitRed;
        ColorUtility.TryParseHtmlString("#E54F4F", out hitRed);
        bodyMater.color = hitRed;
        //震动
        camereGrapher.GetComponent<GrapherController>().Shake();
        yield return new WaitForSeconds(0.1f);
        bodyMater.color = Color.white;
    }
    
    private void RecoverEnergy()
    {
        float recoverValue = Energy + energyRecoverSpeed * Time.deltaTime;
        StartCoroutine(GameManager.Instance.ChangeEnergyBar(Energy, recoverValue));
        Energy = recoverValue;
    }

    private void RunReduceEnergy()
    {
        float reduceValue = Energy - runReduceEnergySpeed * Time.deltaTime;
        if (reduceValue >= 0)
        {
            StartCoroutine(GameManager.Instance.ChangeEnergyBar(Energy, reduceValue));
            Energy = reduceValue;
            currentMoveSpeed = runSpeed;
            //跑步状态不可恢复精力
            energyRecoverClock = energyRecoverTime;
        }
    }
    private void OnTired()
    {
        StartCoroutine(GameManager.Instance.RecoverTired());
    }
}
