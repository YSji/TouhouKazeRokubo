using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeTargetContraller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance._isPause)
            //transform.RotateAround(transform.parent.position, new Vector3(1, 0, 1), Audio._bufferBand[2]*2+1);
            transform.RotateAround(transform.parent.position, new Vector3(0, 0, 1), Audio._bufferBand[2]*4+1);
    }
}
