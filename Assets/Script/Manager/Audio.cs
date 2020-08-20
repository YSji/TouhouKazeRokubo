using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class Audio : MonoBehaviour
{
    public static int[] size = { 1, 2, 3, 6, 12, 18, 46, 94, 186, 282, 512 };
    //public static int[] size = { 2, 6, 14, 30, 62, 126, 254, 512 };

    public static float[] _samples;
    public static float[] _freqBand;
    readonly int n_Sample = 512;
    static int n_freq;
    public float _k;
    public float _b;

    //buffer
    public static float[] _bufferBand;
    float[] _bufferDec;
    public float _decInitial;
    public float _decIncrease;
    float _bufferDecAmp;
    public float _decInitialAmp;
    public float _decIncreaseAmp;
    //normalize
    public static float[] _normBand;
    public static float[] _normBufferBand;
    public static float[] _highestBand;
    public static float _prfHighest;
    //amplitude
    public static float _amplitude, _amplitudeBuffer;
    static float _amplitudeHighest;
    int _sampleGetCount=0;
    int _sampleGetStep=3;
    float[,] _sampleTemp;

    AudioSource _audioSource;
    
    

    private void Awake()
    {
        n_freq = size.Length;
        _samples = new float[n_Sample];
        _freqBand = new float[n_freq];
        _bufferBand = new float[n_freq];
        _bufferDec = new float[n_freq];
        _normBand = new float[n_freq];
        _normBufferBand = new float[n_freq];
        _highestBand = new float[n_freq];
        _sampleTemp = new float[_sampleGetStep,n_freq];
        _amplitudeBuffer = 0;
        _amplitude = 0;
        _audioSource = GetComponent<AudioSource>();
        _prfHighest = 0.2f;
        ResetHighest();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequenceBands();
        MakeBufferBand();
        MakeNormalizationBands();
        GetAmplitude();

    }
    public static void ResetHighest()
    {

        for (int i = 0; i < n_freq; i++)
        {
            _highestBand[i] = _prfHighest;
        }
        _amplitudeHighest = _prfHighest * n_freq * 4;
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples,0,FFTWindow.BlackmanHarris);
    }

    void MakeFrequenceBandsOld()
    {
        int start = 0;
        int end;
        int correntI = _sampleGetCount % _sampleGetStep;
        if (correntI == 0)
        {
            for (int i = 0; i < size.Length; i++)
            {
                float average = 0;
                for (int j = 0; j < _sampleGetStep; j++)
                {

                    average += _sampleTemp[j, i];
                }
                _freqBand[i] = average / _sampleGetStep;
            }
        }

        for (int i = 0; i < n_freq; i++)
        {
            end = size[i];
            float average = 0;

            for (int j = start; j < end; j++)
            {
                average += _samples[j] * (_k * (j + 1) + _b);
            }
            start = end;
            average /= end;
            _sampleTemp[correntI ,i] = average;
        }

        _sampleGetCount++;


    }
    void MakeFrequenceBands()
    {
        int start = 0;
        int end;

        for (int i = 0; i < size.Length; i++)
        {
            end = size[i];
            float average = 0;

            for (int j = start; j < end; j++)
            {
                average += _samples[j] * (_k * (j + 1) + _b);
            }
            start = end;
            average /= end;
            _freqBand[i] = average;
        }

    }

    void MakeBufferBand()
    {
        for (int i = 0; i < n_freq; i++)
        {
            if (_freqBand[i] > _bufferBand[i])
            {
                _bufferBand[i] = _freqBand[i];
                _bufferDec[i] = _decInitial;
            }
            if (_freqBand[i] < _bufferBand[i])
            {
                _bufferBand[i] =_bufferBand[i]>_bufferDec[i]? (_bufferBand[i] - _bufferDec[i]):0;
                _bufferDec[i] *= _decIncrease;
            }
        }
    }

    void MakeNormalizationBands()
    {
        for (int i = 0; i < n_freq; i++)
        {
            if (_freqBand[i]> _highestBand[i])
            {
                _highestBand[i] = _freqBand[i];
            }
            _normBand[i] = _freqBand[i] / _highestBand[i];
            _normBufferBand[i] = _bufferBand[i] / _highestBand[i];
        }
    }

    //Amplitude
    void GetAmplitude()
    {
        float temp = 0;
        for (int i = 0; i < n_freq; i++)
        {
            temp += _normBufferBand[i];
            //temp += _normBand[i];


            //temp_Buffer += _normBufferBand[i];
        }


        if (temp > _amplitudeHighest)
            _amplitudeHighest = temp;

        _amplitude = temp / _amplitudeHighest;
        if (_amplitude >= _amplitudeBuffer)
        {
            _amplitudeBuffer = _amplitude;
            _bufferDecAmp = _decInitialAmp;
        }
        else
        {
            _amplitudeBuffer = _amplitudeBuffer > _bufferDecAmp ? (_amplitudeBuffer - _bufferDecAmp) : 0;
            _bufferDecAmp *= _decIncreaseAmp;
        }
    }
    
}
