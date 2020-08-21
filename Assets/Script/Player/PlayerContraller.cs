using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerContraller : MonoBehaviour
{
    public float speedPrf;
    public float shiftRate;
    float speed;
    float _getHitCD;
    public GameObject check;
    Vector3 leftBtm_cornerPos;
    Vector3 rightTop_cornerPos;
    float leftBorder;
    float rightBorder;
    float topBorder;
    float bottomBorder;
    // Start is called before the first frame update
    void Start()
    {
        leftBtm_cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        rightTop_cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 1f, 0f));
        leftBorder = leftBtm_cornerPos.x;
        rightBorder = rightTop_cornerPos.x;
        topBorder = rightTop_cornerPos.y;
        bottomBorder = leftBtm_cornerPos.y;
        speed = speedPrf;
        check.SetActive(false);
        _getHitCD = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance._isPause)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                speed /= shiftRate;
                check.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                speed = speedPrf;
                check.SetActive(false);
            }
            _getHitCD -= Time.deltaTime;
            Movement();
        }
    }
    void Movement()
    {
        Vector3 pos = transform.position;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log(horizontal);
        //Debug.Log(vertical);
        Vector3 position =Vector3.zero;
        position.x = horizontal * Mathf.Sqrt(1 - (vertical * vertical) / 2.0f);
        position.y = vertical * Mathf.Sqrt(1 - (horizontal * horizontal) / 2.0f);
        position.x *= speed * Time.deltaTime;
        position.y *= speed * Time.deltaTime;
        pos += position;
        if (pos.x <= leftBorder)
        {
            pos.x = leftBorder;
        }
        else if (pos.x >= rightBorder)
        {
            pos.x = rightBorder;
        }

        if (pos.y <= bottomBorder)
        {
            pos.y = bottomBorder;
        }
        else if (pos.y >= topBorder)
        {
            pos.y = topBorder;
        }

        transform.position = pos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Barrage")
        {
            ObjectPool.Instance.ReCircleObj(collision.gameObject.name, collision.gameObject);
            //Destroy(other.gameObject);
            if (_getHitCD <= 0)
            {
                GameManager.Instance.HPDec();
                _getHitCD = 1;
            }
        }
    }
    public void Reset()
    {
        transform.position = new Vector3(-7,0,0);

        _getHitCD = 1;
    }
}
