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
    public GameObject _jadeTargetPrefabe;
    public float _jadeDir;
    public GameObject _jadePrefabe;
    int _jade_Count=2;
    GameObject[] _jadeTargetList;
    GameObject[] _jadeList;

    int _bandLength;
    public float[] _oldBuffBands;
    public float[] _onsetCheckList;
    public float _onsetCD;

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

        _bandLength = Audio._normBufferBand.Length;
        _oldBuffBands = new float[_bandLength];
        _onsetCheckList = new float[_bandLength];
        for (int i = 0; i < Audio._normBufferBand.Length; i++)
        {
            _oldBuffBands[i] = 0;
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
        float x = 0.8f - ((Audio._normBufferBand[6] + 0.5f) % 1 * 0.8f + 0.1f)*0.2f;
        float y = (Audio._normBufferBand[6] + 0.5f)%1*0.8f+0.1f;
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
        _jadeTargetList = new GameObject[_jade_Count];
        float rotaStep = 360f / _jade_Count;
        for (int i = 0; i < _jade_Count; i++)
        {
            
            GameObject _instanceCube = Instantiate(_jadeTargetPrefabe);
            Transform tf = _instanceCube.transform;
            tf.parent = transform;
            _instanceCube.transform.position = transform.position;
            //_instanceCube.transform.rotation = transform.rotation;
            //tf.Rotate(new Vector3(0, 45, 0), Space.Self);
            tf.Rotate(new Vector3(0, 0, i * rotaStep), Space.Self);
            tf.localPosition += tf.up * _jadeDir;
            _jadeTargetList[i]=_instanceCube;

        }
    }
    public void UpdateJade()
    {
        for (int i = 0; i < _jade_Count; i++)
        {
            _jadeList[i].GetComponent<JadeController>().SetTarget(_jadeTargetList[i].transform.position);
        }
    }
    public void CreateJade()
    {
        _jadeList = new GameObject[_jade_Count];
        float rotaStep = 360f / _jade_Count;
        for (int i = 0; i < _jade_Count; i++)
        {

            GameObject _instanceCube = Instantiate(_jadePrefabe);
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
            _jadeList[i] = _instanceCube;
        }
    }

    public void CheackOnSet()
    {
        for (int i = 0; i < _bandLength; i++)
        {
            _onsetCheckList[i] -= Time.deltaTime;
            if (_onsetCheckList[i] <= 0)
            {

                if (_oldBuffBands[i] > 0.05)
                {
                    float check = _oldBuffBands[i];
                    if (Audio._normBufferBand[i] - _oldBuffBands[i] >(check>0.5? check*0.2:0.1))
                    {
                        _onsetCheckList[i] = _onsetCD;
                        OnOnSet(i);
                    }
                }
            }
        }

        Audio._normBufferBand.CopyTo(_oldBuffBands, 0);
    }
    void OnOnSet(int i)
    {

        //if (!_isMove)
        //{
        switch (i)
        {
            case (2):
                _jadeList[0].GetComponent<JadeController>().CreateBigBarrage();
                break;
            case (1):
                _jadeList[0].GetComponent<JadeController>().CreateBigBarrage();
                break;
            case (9):
                _jadeList[1].GetComponent<JadeController>().CreateAngleBarrage(6, 10);
                break;
            //case (10):
            //    _jadeList[1].GetComponent<JadeController>().CreateAngleBarrage(6, 10);
            //    break;
            default:
                break;
        }
        //}

        if (i>2&&i<8)
        {
                CreateSingleBarrage();

        }
    }
    void CreateSingleBarrage()
    {
        Vector3 v;
        if (GameManager.Instance._isStart)
            v = (GetPlayerTransform.tf.position - transform.position).normalized;
        //v = (GetPlayerTransform.tf.position - GetBossTransform.tf.position).normalized;
        else v = new Vector3(-1, 0, 0);

        GameObject _instanceCube = ObjectPool.Instance.GetGObject("Prefabs/dan-3");
        _instanceCube.transform.position = transform.position;
        _instanceCube.transform.localRotation = transform.localRotation;
        _instanceCube.transform.up=v;
        _instanceCube.transform.Rotate(new Vector3(0,0, Random.Range(-5f, 5f)));
        _instanceCube.GetComponent<danController>()._speed = 7f;
    }
    //public void TransitionToState(BossBaseState state)
    //{
    //    _currentState = state;
    //    _currentState.StateStart(this);
    //}
    public void Reset()
    {
        transform.position = new Vector3(3.8f,0,0);
        _target = Vector3.zero;
        _isMove = false;
        for (int i = 0; i < _jade_Count; i++)
        {
            _jadeList[i].transform.localPosition = Vector3.zero;
        }
    }
}
