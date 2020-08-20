using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHP : MonoBehaviour
{
    Scrollbar _scrollbar;
    // Start is called before the first frame update
    void Start()
    {
        _scrollbar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        _scrollbar.transform.GetChild(1).GetComponent<Text>().text = GameManager.Instance.HP + "/" + GameManager.Instance.HPMax;
        _scrollbar.size = (GameManager.Instance.HP)/ (float)GameManager.Instance.HPMax;
        _scrollbar.value = 0;
    }
}
