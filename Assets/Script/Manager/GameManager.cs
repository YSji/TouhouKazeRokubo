using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

using System.IO;
using NAudio;
using NAudio.Wave;
using UnityEngine.Networking;

using UnityEngine.UI;
using System.Diagnostics;

public class GameManager : Singleton<GameManager>
{
    public int HP;
    public int HPMax;
    public GameObject _bori;
    public  GameObject _mori;
    AudioSource audioS;
    public Button buttonPause;
    public Button buttonStart;
    public GameObject progressBar;
    public bool _isStart;
    public bool _isPause;

    public float _clipTime;
    public float _clipTimeCount;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        _isStart = false;
        _isPause = false;
        HP = 10;
        HPMax = 10;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GamePause();
        }
        if (_isStart)
        {
            if (HP <= 0)
            {
                GameEnd();
            }
            if (!_isPause)
            {
                _clipTimeCount += Time.deltaTime;
                if (_clipTime <= _clipTimeCount)
                {
                    GameEnd();
                }
            }
        }
    }
    public void HPDec()
    {
        HP -= 1;
    }


    public void GamePause()
    {
            if (!_isPause)
            {
                buttonPause.transform.GetChild(0).GetComponent<Text>().text = "继续";
                _isPause = true;

                audioS.Pause();
            }
            else
            {
                buttonPause.transform.GetChild(0).GetComponent<Text>().text = "暂停";
                _isPause = false;
                audioS.Play();
            }
        

    }
    public void GameStart()
    {
        if (!_isStart)
        {
            if (_isPause)
            {
                buttonPause.transform.GetChild(0).GetComponent<Text>().text = "暂停";
                _isPause = false;
            }
            StartCoroutine(GameStartWait());
            buttonStart.transform.GetChild(0).GetComponent<Text>().text = "结束";
        }
        else
        {
            GameEnd();
        }
        
    }
    IEnumerator GameStartWait()
    {
        HP = HPMax;
        _mori.SetActive(true);
        _bori.GetComponent<BossController>().Reset();
        _mori.GetComponent<PlayerContraller>().Reset();
        audioS.Stop();
        yield return new WaitForSeconds(0.3f);
        Debug.print("开始");
        _clipTime = audioS.clip.length;
        _clipTimeCount = 0;
        progressBar.SetActive(true);
        audioS.loop = false;
        audioS.Play();
        _isStart = true;
        //StartCoroutine(AudioPlayFinished(audioS.clip.length));
        //StartCoroutine("AudioPlayFinished",audioS.clip.length);

    }
    public void GameEnd()
    {
        _isStart = false;
        _isPause = false;
        buttonStart.transform.GetChild(0).GetComponent<Text>().text = "开始";
        progressBar.SetActive(false);
        _mori.SetActive(false);
        audioS.loop = true;

        if (!audioS.isPlaying)
        {
            audioS.Play();
            
        }
        Debug.print("结束");

    }
    public void QuiteGame()
    {
        Application.Quit();
    }

    public void ChangeMusic()
    {
        if (!_isStart)
        {
            OpenFileName fileInfo = GetFile();
            if (fileInfo != null)
            {
                //string path = fileInfo.file;
                switch (fileInfo.filterIndex)
                {
                    case 2:
                        StartCoroutine(IELoadExternalAudioWebRequest2(fileInfo));

                        break;
                    default:
                        StartCoroutine(GetAudioClip( fileInfo));
                        break;


                }
            }
        }
    }
    /// <summary>
    /// 加载文件为AudioClip
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    IEnumerator GetAudioClip(OpenFileName file)
    {
        string path = file.file;
        Debug.print(path);
        audioS.Stop();
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(path,AudioType.WAV))

        {

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)

            { Debug.print("uwrERROR:" + uwr.error); }

            else

            {
                audioS.clip =  DownloadHandlerAudioClip.GetContent(uwr);
                Audio.ResetHighest();
                audioS.Play();
            }

        }

    }
    
    public OpenFileName GetFile()
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "(*.wav)\0*.wav\0(*.mp3)\0*.mp3";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        openFileName.title = "选择音频";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetSaveFileName(openFileName))
        {
            Debug.print(openFileName.file);
            Debug.print(openFileName.fileTitle);
            return openFileName;
        }
        return null;
    }
    private IEnumerator IELoadExternalAudioWebRequest2(OpenFileName file)
    {
        string _url = file.file;
        audioS.Stop();
        string _tempURL = _url.Replace(".mp3", ".wav");
        Debug.print(_tempURL);
        if (!File.Exists(_tempURL))
        {
            FileStream _fileStream = File.Open(_url, FileMode.Open);
            Mp3FileReader _mp3FileReader = new Mp3FileReader(_fileStream);
            WaveFileWriter.CreateWaveFile(_tempURL, _mp3FileReader);
        }
        yield return null;
        UnityWebRequest _unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(_tempURL, AudioType.WAV);
        yield return _unityWebRequest.SendWebRequest();
        if (_unityWebRequest.isHttpError || _unityWebRequest.isNetworkError)
        {
            Debug.print(_unityWebRequest.error.ToString());
        }
        else
        {

            audioS.clip = DownloadHandlerAudioClip.GetContent(_unityWebRequest);
            Audio.ResetHighest();
            audioS.Play();
        }
    }
    /*
        private IEnumerator AudioPlayFinished(float time)
        {

                yield return new WaitForSeconds(time);

                #region   播放完成后执行的代码

                print("播放完毕");
                GameEnd();

                #endregion
        }
    */

}
