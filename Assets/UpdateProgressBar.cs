using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateProgressBar : MonoBehaviour
{
    Scrollbar scb;
    // Start is called before the first frame update
    void Start()
    {
        scb = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        scb.size = GameManager.Instance._clipTimeCount / GameManager.Instance._clipTime;
        scb.value = 0;
    }
}
