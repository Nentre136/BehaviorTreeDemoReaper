using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapherController : MonoBehaviour
{
    [Tooltip("摄影师眼睛位置")]
    public GameObject followTarget;
    private Camera mainCamara;
    private Vector3 originPosition;
    // 鼠标灵敏度
    public float MouseSensitivity = 20.0f;
    public Quaternion rota { get; private set; }
    private float rotaX = 0;
    private float rotaY = 0;

    [Tooltip("屏幕抖动幅度")]
    public float shakeForce = 0.1f;
    [Tooltip("屏幕抖动时间")]
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
        //限制上下旋转不超过90度
        rotaY = Mathf.Clamp(rotaY, -90, 90);
        //获取旋转到的四元值
        rota = Quaternion.Euler(rotaY,rotaX, 0);
        transform.rotation = rota;
        //transform.rotation = Quaternion.Slerp(
        //    transform.rotation,
        //    rota,
        //    Time.deltaTime
        //);
        //相机跟随
        transform.position = followTarget.transform.position;

    }
    //屏幕抖动
    public void Shake()
    {
        StartCoroutine(OnShake());
    }
    IEnumerator OnShake()
    {
        float clock = 0f;

        while (clock < shakeTime)
        {
            //随机震动角度
            Vector3 shakeOffset = Random.insideUnitSphere * shakeForce;
            //当前位置改变角度
            mainCamara.transform.localPosition = originPosition + shakeOffset;

            clock += Time.deltaTime;
            // 等待一帧
            yield return null;
        }
        //复原
        mainCamara.transform.localPosition = originPosition;
    }
}
