using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapherController : MonoBehaviour
{
    [Tooltip("��Ӱʦ�۾�λ��")]
    public GameObject followTarget;
    private Camera mainCamara;
    private Vector3 originPosition;
    // ���������
    public float MouseSensitivity = 20.0f;
    public Quaternion rota { get; private set; }
    private float rotaX = 0;
    private float rotaY = 0;

    [Tooltip("��Ļ��������")]
    public float shakeForce = 0.1f;
    [Tooltip("��Ļ����ʱ��")]
    public float shakeTime = 0.5f;
    void Start()
    {
        mainCamara = transform.Find("Main Camera").GetComponent<Camera>();
        originPosition = mainCamara.transform.localPosition;
    }

    private void FixedUpdate()
    {
        rotaX += Input.GetAxis("Mouse X") * MouseSensitivity;
        rotaY -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        //����������ת������90��
        rotaY = Mathf.Clamp(rotaY, -90, 90);
        //��ȡ��ת������Ԫֵ
        rota = Quaternion.Euler(rotaY,rotaX, 0);
        transform.rotation = rota;
        //transform.rotation = Quaternion.Slerp(
        //    transform.rotation,
        //    rota,
        //    Time.deltaTime
        //);
        //�������
        transform.position = followTarget.transform.position;

    }
    //��Ļ����
    public void Shake()
    {
        StartCoroutine(OnShake());
    }
    IEnumerator OnShake()
    {
        float clock = 0f;

        while (clock < shakeTime)
        {
            //����𶯽Ƕ�
            Vector3 shakeOffset = Random.insideUnitSphere * shakeForce;
            //��ǰλ�øı�Ƕ�
            mainCamara.transform.localPosition = originPosition + shakeOffset;

            clock += Time.deltaTime;
            // �ȴ�һ֡
            yield return null;
        }
        //��ԭ
        mainCamara.transform.localPosition = originPosition;
    }
}
