using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject _panel;
    Transform _ts_Panel;

    float _panelWidth;
    float _panelHeight;

    public float _scale_Band;
    public float _scale_Amp;
    public float _height_Bands;
    int _count_bands=0;

    public float _offsetX, _offsetY;

    public GameObject _sampleRectangle;
    public GameObject _sampleAmpBand;

    public GameObject _ampBand;
    public GameObject _ampBandBuff;
    public GameObject _ampNormBandBuff;

     ArrayList _Rectangles_Freq=new ArrayList();
     ArrayList _Rectangles_Buff = new ArrayList();
     ArrayList _Rectangles_NormBuff = new ArrayList();


    //int count = 0;

    protected override void OnAwake()
    {
        _panelWidth = _panel.GetComponent<RectTransform>().rect.width;
        _panelHeight = _panel.GetComponent<RectTransform>().rect.height;
        _ts_Panel = _panel.GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //_ampBand =CreateRectangles(Audio._freqBand.Length, _Rectangles_Freq, _offsetX, _offsetY,true);
        //_ampBandBuff= CreateRectangles(Audio._bufferBand.Length, _Rectangles_Buff, _offsetX, _offsetY,true);
        _ampNormBandBuff= CreateRectangles(Audio._normBufferBand.Length, _Rectangles_NormBuff, _offsetX, _offsetY,true);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateRectangles(Audio._freqBand, _Rectangles_Freq,Audio._amplitude,_ampBand);
        //UpdateRectangles(Audio._bufferBand, _Rectangles_Buff,Audio._amplitudeBuffer,_ampBandBuff);
        UpdateRectangles(Audio._normBufferBand, _Rectangles_NormBuff, Audio._amplitudeBuffer, _ampNormBandBuff);

    }

    void CreateRectangles(int n_Rectangles,ArrayList rectangls,float offsetX,float offsetY)
    {
        //_count_bands++;        
        float sampleWidth = _sampleRectangle.GetComponent<RectTransform>().rect.width;
        float spacing = (_panelWidth - 2 * offsetX - (n_Rectangles - 1) * sampleWidth) / (n_Rectangles - 1);
        float posX = spacing + sampleWidth;
        float posY = _height_Bands*_count_bands+offsetY;
        

        //rectangls = new GameObject[n_Rectangles];
        for (int i = 0; i < n_Rectangles; i++)
        {
            
            GameObject insReatangl = Instantiate(_sampleRectangle);
            
            Transform tf = insReatangl.GetComponent<Transform>();
            RectTransform rt = insReatangl.GetComponent<RectTransform>();
            tf.SetParent(_ts_Panel);
            //insReatangl.GetComponent<RectTransform>().pivot = new Vector2(offsetX, offsetY);
            rt.anchoredPosition3D=new Vector3(posX * i + offsetX*(i+1), posY, 0);
            //tf.SetPositionAndRotation(new Vector3(posX * i + offsetX, posY, 0),new Quaternion(0,0,0,0));
            insReatangl.name = "Rectangle"+"_"+_count_bands+"_" + i;
            rectangls.Add( insReatangl);
            //rectangls[i] = insReatangl;
        }
        _count_bands++;
    }
    GameObject CreateRectangles(int n_Rectangles, ArrayList rectangls, float offsetX, float offsetY, bool hasAmp)
    {
        //_count_bands++;
        float sampleWidth = _sampleRectangle.GetComponent<RectTransform>().rect.width;
        float spacing = (_panelWidth - 2 * offsetX - (n_Rectangles -1)* sampleWidth) / (n_Rectangles - 1);
        float posX = spacing+sampleWidth;
        float posY = _height_Bands * _count_bands + offsetY;

        GameObject ampBand=null;
        if (hasAmp)
        {
            ampBand = Instantiate(_sampleAmpBand);
            Transform tf_a = ampBand.GetComponent<Transform>();
            RectTransform rt_a = ampBand.GetComponent<RectTransform>();
            tf_a.SetParent(_ts_Panel);
            rt_a.anchoredPosition3D = new Vector3(0, posY, 0);
            rt_a.localScale=new Vector3(1, rt_a.localScale.y, 1);
            rt_a.sizeDelta = new Vector2(_panelWidth-2*offsetX, 0);
            ampBand.name = "AmpBand" + "_" + _count_bands ;
        }


        //rectangls = new GameObject[n_Rectangles];
        for (int i = 0; i < n_Rectangles; i++)
        {

            GameObject insReatangl = Instantiate(_sampleRectangle);

            Transform tf_r = insReatangl.GetComponent<Transform>();
            RectTransform rt_r = insReatangl.GetComponent<RectTransform>();
            tf_r.SetParent(_ts_Panel);
            //insReatangl.GetComponent<RectTransform>().pivot = new Vector2(offsetX, offsetY);
            rt_r.anchoredPosition3D = new Vector3(posX * i + offsetX, posY, 0);
            //tf.SetPositionAndRotation(new Vector3(posX * i + offsetX, posY, 0),new Quaternion(0,0,0,0));
            insReatangl.name = "Rectangle" + "_" + _count_bands + "_" + i;
            rectangls.Add(insReatangl);
            //rectangls[i] = insReatangl;
        }
        _count_bands++;

        return ampBand;
    }
    

    void UpdateRectangles(float[] data, ArrayList rectangls)
    {
        int n_Rectangles =Mathf.Min( rectangls.Count,data.Length);


        for (int i = 0; i < n_Rectangles; i++)
        {
            RectTransform rt = ((GameObject)rectangls[i]).GetComponent<RectTransform>();
            //RectTransform rt = rectangls[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.rect.width, data[i]*_scale_Band);
        }
        
    }
    void UpdateRectangles(float[] data, ArrayList rectangls,float amp,GameObject ampBand)
    {
        int n_Rectangles =Mathf.Min( rectangls.Count,data.Length);

        RectTransform rt_a = ampBand.GetComponent<RectTransform>();
        rt_a.sizeDelta = new Vector2(rt_a.rect.width, amp*_scale_Band*_scale_Amp);

        for (int i = 0; i < n_Rectangles; i++)
        {
            RectTransform rt = ((GameObject)rectangls[i]).GetComponent<RectTransform>();
            //RectTransform rt = rectangls[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.rect.width, data[i]*_scale_Band);
        }

    }

}
