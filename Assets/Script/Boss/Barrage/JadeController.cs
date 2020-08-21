using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeController : MonoBehaviour
{
    public  GameObject _bullet;
    

    Vector3 _target;

    bool _isMove;

    float _speed;

    float _speedPri;

    // Start is called before the first frame update
    void Start()
    {
        _speedPri = 8;
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance._isPause)
            MoveTo();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            //CreateCircleBarrage();
            CreateAngleBarrage();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            CreateCircleBarrage();
        }
        Vector3 v;
        if (GameManager.Instance._isStart)
            v = (GetPlayerTransform.tf.position - transform. position).normalized;
            //v = (GetPlayerTransform.tf.position - GetBossTransform.tf.position).normalized;
        else v = new Vector3(-1, 0, 0);
        transform.up = v;
    }
    public void SetTarget(Vector3 tPos)
    {
        Vector2 tfv = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 ttfv = Camera.main.WorldToViewportPoint(tPos);
        _speed = _speedPri * ( new Vector2((tfv.x - ttfv.x), (tfv.y - ttfv.y)).magnitude + 0.5f);
        _target =new Vector3( tPos.x,tPos.y,0);
    }
    public void MoveTo()
    {
        Vector3 pos = Vector2.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        transform.position = pos;
    }


   public void CreateCircleBarrage(int n=12)
    {
        Vector3 fireDirection =transform.forward;
        //Quaternion stepQuaternion = Quaternion.AngleAxis(360f / n, fireDirection);
        float rotaStep = 360f / n;
        for (int i = 0; i < n; i++)
        {
            //transform.eulerAngles = new Vector3(0, 0, rotaStep * i);
            GameObject _instanceCube = ObjectPool.Instance.GetGObject("Prefabs/dan-1");
            _instanceCube.transform.localPosition = transform.position;
            _instanceCube.transform.localRotation = transform.localRotation;
            _instanceCube.transform.localRotation = Quaternion.AngleAxis(rotaStep * i, fireDirection);
            //fireDirection = stepQuaternion*fireDirection;
            //_instanceCube.transform.position = this.transform.position;
            //_instanceCube.transform.parent = this.transform;
            //_instanceCube.name = "CircleCube" + i;
            Color color = Color.HSVToRGB((float)(i / n) + (1 / 2 * n), 0.6f, 1f);
            color.a = 0.6f;
            _instanceCube.GetComponent<SpriteRenderer>().color = color;
            //_instanceCube.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.HSVToRGB(rotaStep * i / 360f, 0.6f, 0.6f));

        }
    }

   public void CreateAngleBarrage(int n=3, float rotaStep=10)
    {
        //Vector3 fireDirection = transform.forward;
        float rotaStart = -10*((n-1)/2f);
        for (int i = 0; i < n; i++)
        {

            GameObject _instanceCube = ObjectPool.Instance.GetGObject("Prefabs/dan-1");
            _instanceCube.transform.position = transform.position;
            _instanceCube.transform.localRotation = transform.localRotation;
            _instanceCube.transform.Rotate(new Vector3(0, 0, rotaStart + rotaStep * i));
            //_instanceCube.transform.localRotation = Quaternion.AngleAxis(rotaStart + rotaStep * i, fireDirection);
            //GameObject _instanceCube = Instantiate(_bullet, transform.position, Quaternion.AngleAxis(rotaStart+ rotaStep * i, fireDirection));
            //_instanceCube.transform.position += Vector3.up * 5;
            Color color = Color.HSVToRGB(((float)i) / n, 0.6f, 1f);
            color.a = 0.8f;
            _instanceCube.GetComponent<SpriteRenderer>().color = color;
            _instanceCube.GetComponent<danController>()._speed = 4f;
            //_instanceCube.GetComponent<SpriteRenderer>().color = Color.HSVToRGB((float)i/n, 0.6f, 0.6f);
        }
    }

   public void CreateRandomAngleBarrage(float angleRange)
    {

    }
   public void CreateBigBarrage()
    {
        GameObject _instanceCube = ObjectPool.Instance.GetGObject("Prefabs/dan-2");
        _instanceCube.transform.position = transform.position;
        _instanceCube.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _instanceCube.transform.Rotate(new Vector3(0, 0,Random.Range(75f,105f)));
        _instanceCube.GetComponent<danController>()._speed = 6f;
    } 
    public void CreateRandomSingleBarrage()
    {
        GameObject _instanceCube = ObjectPool.Instance.GetGObject("Prefabs/dan-3");
        _instanceCube.transform.position = transform.position;
        _instanceCube.transform.localRotation = transform.localRotation;
        _instanceCube.transform.Rotate(new Vector3(0, 0,Random.Range(0f,5f)));
        _instanceCube.GetComponent<danController>()._speed = 6f;
    }
}
