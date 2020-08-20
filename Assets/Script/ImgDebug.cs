using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgDebug : MonoBehaviour
{
    public float _start;
    // Start is called before the first frame update
    void Start()
    {
        _start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void de()
    {
        DestroyImmediate(gameObject);
    }
}
