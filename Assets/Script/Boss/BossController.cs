using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossController : MonoBehaviour
{
    public bool _isMove;
    public float _speedPri;
    float _speed;
    public Vector3 _target;
    float _stayTimeCount;
    public float _stayTime;
    //BossBaseState _currentState;
    //public BossStandbyState standbyState= new BossStandbyState();
    public GameObject jadeTargetPrefabe;
    public GameObject jadePrefabe;
    int _jade_Count=6;
    GameObject[] jadeTargetList;
    GameObject[] jadeList;

    int bandLength;
    public float[] oldBuffBands;
    public float[] Onset;
    public float OnsetCD;

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
        rightTop_cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f,1f, 0f));
        leftBorder = leftBtm_cornerPos.x;
        rightBorder = rightTop_cornerPos.x;
        topBorder = rightTop_cornerPos.y;
        bottomBorder = leftBtm_cornerPos.y;
        _speed = 10;

         //_target = Vector3.zero;
        _target = transform.position;
        _isMove = true;


        CreateJadeTarget();
        CreateJade();

        bandLength = Audio._normBufferBand.Length;
        oldBuffBands = new float[bandLength];
        Onset = new float[bandLength];
        for (int i = 0; i < Audio._normBufferBand.Length; i++)
        {
            oldBuffBands[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.Instance._isPause)
        {
            if (GameManager.Instance._isStart)
            {
                CheackOnSet();
            }
            MoveTo();
            UpdateJade();
        }
        //_currentState.StateUpdate(this);
    }
    IEnumerator SetTargetWait(float time=1)
    {
        //while (true)
        //{
        //    if (_isMove)
        //        yield return null;
        //    else
        //    {
        //        yield return new WaitForSeconds(1f);
        //        SetTarget();
        //    }
        //}
        yield return new WaitForSeconds(time);
        SetTarget();
    }
    public void SetTarget()
    {
        float x = 0.7f - Mathf.Pow(Audio._normBufferBand[4] - 0.5f, 2);
        float y = (Audio._normBufferBand[3] + 0.5f)%1*0.8f+0.1f;
       _target = Camera.main.ViewportToWorldPoint(new Vector3(x,y,10));
        _isMove = true;
    }
    public void MoveTo()
    {
        if (_isMove)
        {
            Vector2 tv = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 ttv = Camera.main.WorldToViewportPoint(_target);
            float d = new Vector2((ttv.x - tv.x), (ttv.y - tv.y)).magnitude;
            _speed = _speedPri*(1-(d - 0.4f) * (d-0.4f));
            Vector3 pos = Vector2.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

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

            if (new Vector2(pos.x-_target.x,pos.y-_target.y).magnitude == 0f)
            {
                _isMove = false;
                _stayTimeCount = _stayTime;
                //SetTarget();
                //IEnumerator enumerable = SetTargetWait();
                //StartCoroutine(SetTargetWait(1));
            }
        }
        else
        {
            _stayTimeCount -= Time.deltaTime;
            if (_stayTimeCount <= 0)
            {
                SetTarget();
                _isMove = true;
            }
        }

    }

    public void CreateJadeTarget()
    {
        jadeTargetList = new GameObject[_jade_Count];
        float rotaStep = 360f / _jade_Count;
        for (int i = 0; i < _jade_Count; i++)
        {
            
            GameObject _instanceCube = Instantiate(jadeTargetPrefabe);
            Transform tf = _instanceCube.transform;
            tf.parent = transform;
            _instanceCube.transform.position = transform.position;
            //_instanceCube.transform.rotation = transform.rotation;
            //tf.Rotate(new Vector3(0, 45, 0), Space.Self);
            tf.Rotate(new Vector3(0, 0, i * rotaStep), Space.Self);
            tf.localPosition += tf.up * 9;
            jadeTargetList[i]=_instanceCube;

        }
    }
    public void UpdateJade()
    {
        for (int i = 0; i < _jade_Count; i++)
        {
            jadeList[i].GetComponent<JadeController>().SetTarget(jadeTargetList[i].transform.position);
        }
    }
    public void CreateJade()
    {
        jadeList = new GameObject[_jade_Count];
        float rotaStep = 360f / _jade_Count;
        for (int i = 0; i < _jade_Count; i++)
        {

            GameObject _instanceCube = Instantiate(jadePrefabe);
            Transform tf = _instanceCube.transform;
            //tf.parent = transform;
            _instanceCube.transform.position = transform.position;
            _instanceCube.transform.rotation = transform.rotation;
            //tf.Rotate(new Vector3(0, 45, 0), Space.Self);
            //tf.Rotate(new Vector3(0, 0, i * rotaStep), Space.Self);
            //tf.localPosition += tf.up * 10;
            //tf.rotation = Quaternion.Euler(0,0,90);
            //tf.position = new Vector3(tf.position.x, tf.position.y, 0);
            //_instanceCube.GetComponent<SpriteRenderer>().color = Color.HSVToRGB((float)i / _jade_Count, 0.6f, 0.6f);
            jadeList[i] = _instanceCube;
        }
    }

    public void CheackOnSet()
    {
        for (int i = 0; i < bandLength; i++)
        {
            Onset[i] -= Time.deltaTime;
            if (Onset[i] <= 0)
            {

                if (oldBuffBands[i] > 0.1)
                {
                    float check = oldBuffBands[i];
                    if (Audio._normBufferBand[i] - oldBuffBands[i] > (check>0.4?check*0.2:0.08) )
                    {
                        Onset[i] = OnsetCD;
                        OnOnSet(i);
                    }
                }
            }
        }

        Audio._normBufferBand.CopyTo(oldBuffBands, 0);
    }
    void OnOnSet(int i)
    {

        if (!_isMove)
        {
            switch (i)
            {
                case (1):
                    jadeList[0].GetComponent<JadeController>().CreateAngleBarrage(6, 10);
                    break;
                case (9):
                    jadeList[3].GetComponent<JadeController>().CreateAngleBarrage(6, 10);
                    break;
                default:
                    break;
            }
        }

        switch (i)
        {
            case (3):
                jadeList[1].GetComponent<JadeController>().CreateAngleBarrage(1, 0);
                break;
            case (4):
                jadeList[2].GetComponent<JadeController>().CreateAngleBarrage(1, 0);
                break;
            case (5):
                jadeList[3].GetComponent<JadeController>().CreateAngleBarrage(1, 0);
                break;
            case (6):
                jadeList[4].GetComponent<JadeController>().CreateAngleBarrage(1, 0);
                break;
            case (7):
                jadeList[5].GetComponent<JadeController>().CreateAngleBarrage(1, 0);
                break;
            default:
                break;
        }
    }
    //public void TransitionToState(BossBaseState state)
    //{
    //    _currentState = state;
    //    _currentState.StateStart(this);
    //}
    public void Reset()
    {
        transform.position = Vector3.zero;
        _target = Vector3.zero;
        _isMove = true;
        for (int i = 0; i < _jade_Count; i++)
        {
            jadeList[i].transform.localPosition = Vector3.zero;
        }
    }
}
