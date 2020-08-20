using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Cut {all, left,right}
public class Debug : MonoBehaviour
{
    public Cut cutSetting;
    int count = 0;
    int _length;
    int _start;
    public int _width;
    public int _speed;

    public float _rectWidth;
    public float _imgK;
    public float _imgB;
    public float _lowLimit;

    public float _stepX;
    public float _stepY;
    public float _offsetX;
    public float _offsetY;
    //public float _scaleX;
    public float _scaleY;

    public GameObject _sampleRectangle;
     ArrayList _Buff = new ArrayList();
    Transform _tf;
    // Start is called before the first frame update
    GameObject[,] Lines;


    void Start()
    {
        _length = Audio._normBufferBand.Length;
        _start = 0;
        if (cutSetting == Cut.left)
        {
            _length /= 2;
        }else if (cutSetting == Cut.right)
        {
            _start = _length / 2;
            _length /= 2;
        }
        _tf = gameObject.GetComponent<Transform>();
        Lines = new GameObject[_length, 50];
        CreateLines();
    }
    private void FixedUpdate()
    {
        UpdateLines();
    }
    // Update is called once per frame
    void Update()
    {

    }
    void CreateLines()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j <_length; j++)
            {
                //Debug.DrawLine(new Vector3(count, j * 20, 0), new Vector3(count, j * 20 + Audio._bufferBand[j] * 10, 0), Color.red);
                GameObject insReatangl = Instantiate(_sampleRectangle);

                Transform tf_r = insReatangl.GetComponent<Transform>();
                RectTransform rt_r = insReatangl.GetComponent<RectTransform>();
                Image image = insReatangl.GetComponent<Image>();
                //image.color += new Color(0, 0, 0, _imgK *Mathf.Pow( _imgB,j));
                image.color += new Color(0, 0, 0, (j * _imgK + _imgB) / 255f);
                tf_r.SetParent(_tf);
                //insReatangl.GetComponent<RectTransform>().pivot = new Vector2(offsetX, offsetY);
                rt_r.anchoredPosition3D = new Vector3(i*_stepX+_offsetX, j *_stepY+_offsetY, 0);
                rt_r.sizeDelta = new Vector2(_rectWidth, 0);
                rt_r.localScale = Vector3.one;
                //tf.SetPositionAndRotation(new Vector3(posX * i + offsetX, posY, 0),new Quaternion(0,0,0,0));
                insReatangl.name = "Rectangle" + "_" +j + "_" + i;
                Lines[j, i] = insReatangl;

            }
        }
    }
    void UpdateLines()
    {
        if (count % _speed == 0)
        {
            float curVlue;
            for (int j = 0; j < _length; j++)
            {
                curVlue = Audio._normBufferBand[j+_start];
                GameObject ins = Lines[j, _width - 1];
                RectTransform rt_i = ins.GetComponent<RectTransform>();
                rt_i.sizeDelta = new Vector2(_rectWidth, (curVlue>_lowLimit? curVlue:0) * _scaleY);
            }
            for (int i = 0; i < _width - 1; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    GameObject ins = Lines[j, i];
                    GameObject fow = Lines[j, i + 1];
                    RectTransform rt_i = ins.GetComponent<RectTransform>();
                    RectTransform rt_f = fow.GetComponent<RectTransform>();
                    //rt_i.sizeDelta = new Vector2(rt_i.rect.width, Audio._bufferBand[j] * 10);
                    rt_i.sizeDelta = new Vector2(_rectWidth, rt_f.rect.height);
                }
            }
        }
        count++;
    }
    //void vvv()
    //{
    //    if (count % 10 == 0)
    //    {
    //        for (int i = 0; i < Audio._bufferBand.Length; i++)
    //        {
    //            //Debug.DrawLine(new Vector3(count, i * 20, 0), new Vector3(count, i * 20 + Audio._bufferBand[i] * 10, 0), Color.red);
    //            GameObject insReatangl = Instantiate(_sampleRectangle);

    //            Transform tf_r = insReatangl.GetComponent<Transform>();
    //            RectTransform rt_r = insReatangl.GetComponent<RectTransform>();
    //            tf_r.SetParent(_tf);
    //            //insReatangl.GetComponent<RectTransform>().pivot = new Vector2(offsetX, offsetY);
    //            rt_r.anchoredPosition3D = new Vector3(count / 10, i * 20, 0);
    //            rt_r.sizeDelta = new Vector2(rt_r.rect.width, Audio._bufferBand[i] * 10);
    //            //tf.SetPositionAndRotation(new Vector3(posX * i + offsetX, posY, 0),new Quaternion(0,0,0,0));
    //            insReatangl.name = "Rectangle" + "_" + count / 10 + "_" + i;
    //            _Buff.Add(insReatangl);

    //        }
    //    }
    //    count++;

    //    for (int i = 0; i < _Buff.Count; i++)
    //    {
    //        ImgDebug img = ((GameObject)_Buff[i]).GetComponent<ImgDebug>();
    //        if (Time.time > (float)img._start + 4)
    //        {
    //            _Buff.Remove(i);
    //            Transform tf_r = ((GameObject)_Buff[i]).GetComponent<Transform>();
    //            tf_r.SetParent(null);
    //            //img.de();
    //        }
    //        else if (Time.time > (float)img._start + 5)
    //        {
    //            img.de();
    //        }
    //        else
    //        {
    //            RectTransform rt_r = ((GameObject)_Buff[i]).GetComponent<RectTransform>();
    //            //RectTransform rt = rectangls[i].GetComponent<RectTransform>();
    //            rt_r.anchoredPosition3D -= new Vector3(1, 0, 0);
    //        }
    //    }
    //}
}
