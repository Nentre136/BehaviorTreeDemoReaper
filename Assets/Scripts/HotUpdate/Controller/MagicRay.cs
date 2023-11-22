using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRay : MonoBehaviour
{
    private BoxCollider hitBox;

    private float clock;
    private void Awake()
    {
        hitBox = GetComponent<BoxCollider>();
        clock = 2.0f;
        hitBox.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(OnStartMagic());
    }
    private void Update()
    {
        if (clock >= 0)
            clock -= Time.deltaTime;
        else
            Destroy(gameObject);
    }
    IEnumerator OnStartMagic()
    {
        yield return new WaitForSeconds(0.25f);
        hitBox.enabled = true;
    }
}
