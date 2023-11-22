using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadPage : MonoBehaviour
{
    private Image loadBar;
    public float loadSpeed = 10f;
    private float offsetZ;
    void Start()
    {
        loadBar = transform.Find("LoadBar").GetComponent<Image>();
        offsetZ = 0;
    }

    void Update()
    {
        if(!ResourceManager.Instance.LoadComplete)
        {
            offsetZ -= loadSpeed * 80.0f * Time.deltaTime;
            offsetZ %= 360.0f;
            Vector3 rotationOffset = new Vector3(0, 0, offsetZ);
            loadBar.rectTransform.rotation = Quaternion.Euler(rotationOffset);
        }
    }
}
