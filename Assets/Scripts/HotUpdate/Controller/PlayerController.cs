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
    //��̼�ʱ��
    private float sprintClock;
    private float sprintTime=0.8f;
    private ParticleSystem sprintLight;
    //�޵м�ʱ��
    public float godClock { get; private set; }
    private float rollGodTime = 3.0f;
    private float hitGodTime = 6.0f;
    private bool isJumping;
    //��Ծ�ƶ�����
    private float jumpMoveObstruct= 0.4f;
    //��׹����
    private float fallMultip = 1.9f;
    private float _energy;
    public float maxEnergy = 70;
    //�ܲ����پ����ٶ�
    private float runReduceEnergySpeed = 4.0f;
    //�����ָ��ٶ�
    private float energyRecoverSpeed = 8.0f;
    //ʹ�þ�����ָ���ʱ��
    private float energyRecoverClock;
    private float energyRecoverTime = 1.3f;


    //ˮƽ����ӳ��
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
        //��
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            Jump();
        }
        //���
        if(!isTired && !isJumping && sprintClock<=0 && Energy - 10 > 0 
            && Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprintClock = sprintTime;
            Sprint();
        }
        //�ܲ�
        if(!isJumping && !isTired && Input.GetKey(KeyCode.LeftShift))
        {
            RunReduceEnergy();
        }
        //����
        if((currentMoveSpeed==runSpeed && Input.GetKeyUp(KeyCode.LeftShift)) || isTired)
            currentMoveSpeed = moveSpeed;
            

        //�����ȴ
        if (sprintClock > 0)
            sprintClock -= Time.deltaTime;
        //�޵�ʱ���ʱ
        if (godClock > 0)
            godClock -= Time.fixedDeltaTime;
        //ʹ�þ�����ָ���ʱ��
        if (energyRecoverClock > 0)
            energyRecoverClock -= Time.deltaTime;

        //�������
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultip - 1) * Time.deltaTime;
        }

        //�ָ�����
        if (energyRecoverClock <= 0 && Energy < maxEnergy)
        {
            RecoverEnergy();
        }
        //����ƣ��״̬
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
        //��Camere��ǰ��Ϊ��ɫ������
        //��Ԫ * ����  =  ��ת�������
        move = camereGrapher.GetComponent<GrapherController>().rota * move;
        //��ֹ��y����ƶ�����
        move.y = 0;
        if (keyX != 0 || keyY != 0)
        {
            float jumpMoveSpeed = 1.0f;
            //����Ծ�����У��ƶ�������
            if (isJumping)
                jumpMoveSpeed = jumpMoveObstruct;
            Vector3 movePosition = move * currentMoveSpeed * jumpMoveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movePosition);

            //����movePosition��ȡplayer��ת�Ƕ�
            Quaternion rota = Quaternion.LookRotation(movePosition);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rota,
                rotaSpeed * Time.deltaTime
            );
        }
    }
    //���Ӽ��ܶ�������
    private void OnTriggerStay(Collider other)
    {
        //û���޵�״̬
        if (other.tag == "Hit" && godClock <= 0)
        {
            StartCoroutine(_OnGetHit());
            //�ܻ���ȴʱ��
            godClock = hitGodTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //���
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
            //��ȡ�����ӽ��µ����ǰ������
            sprintDirec = camereGrapher.GetComponent<GrapherController>().rota * sprintDirec;
            sprintDirec.y = 0;
            rb.AddForce(sprintDirec * sprintForce, ForceMode.Impulse);
            //�����Ч ֹͣ�������
            sprintLight.Stop();
            sprintLight.Clear();
            sprintLight.Play();
            godClock = rollGodTime;
            Energy -= 10;
            //�ܲ�״̬���ɻָ�����
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
        //��
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
            //�ܲ�״̬���ɻָ�����
            energyRecoverClock = energyRecoverTime;
        }
    }
    private void OnTired()
    {
        StartCoroutine(GameManager.Instance.RecoverTired());
    }
}
