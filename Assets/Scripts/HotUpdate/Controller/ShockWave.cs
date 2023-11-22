using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private ParticleSystem mainPartic;

    private SphereCollider sphereCollider;
    public float speed = 10.8f;
    private void Start()
    {
        mainPartic = GetComponent<ParticleSystem>();
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        //粒子不再播放
        if (!mainPartic.IsAlive())
            Destroy(mainPartic.gameObject);
        if(sphereCollider.radius<=21.5f)
            sphereCollider.radius += speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        //该技能可以跳跃躲避
        if (other.tag == "Player" && other.GetComponent<PlayerController>().godClock <= 0)
        {
            if (other.transform.position.y <= 1.0f)
            {
                other.GetComponent<PlayerController>().GetHit();
            }
        }
    }
}
