using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class danController : MonoBehaviour,IResetable
{
    float _start ;
    float _speed=20;
    float _lifeTime = 10;

    public void Reset()
    {
        _start = Time.time;
        _lifeTime = 12;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    // Start is called before the first frame update
    void Start()
    {
        _start = Time.time;
        //Vector2 vector2 = new Vector2(transform.rotation.z, transform.rotation.w) * 2;
        //rb.velocity =vector2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance._isPause){
            //Destroy(gameObject);
            _lifeTime -= Time.deltaTime;
            if (_lifeTime<=0)
                ReCircleSelf();
            Move();
        }
        if (!GameManager.Instance._isStart)
        {
            ReCircleSelf();
        }
    }
    void ReCircleSelf()
    {
            ObjectPool.Instance.ReCircleObj(name, gameObject);
    }
    void Move()
    {
        transform.Translate(transform.up * _speed * Time.deltaTime, Space.World);
    }
}
