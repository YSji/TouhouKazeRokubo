using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using NAudio;
using NAudio.Wave;
using UnityEngine.Networking;

public class NAudioTest
{
    private IEnumerator IELoadExternalAudioWebRequest2(string _url, AudioType _audioType)
    {
        string _tempURL = _url.Replace(".mp3", ".wav");
        Debug.print(_tempURL);
        if (!File.Exists(_tempURL))
        {
            FileStream _fileStream = File.Open(_url, FileMode.Open);
            Mp3FileReader _mp3FileReader = new Mp3FileReader(_fileStream);
            WaveFileWriter.CreateWaveFile(_tempURL, _mp3FileReader);
        }
        yield return null;
        UnityWebRequest _unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(_tempURL, _audioType);
        yield return _unityWebRequest.SendWebRequest();
        if (_unityWebRequest.isHttpError || _unityWebRequest.isNetworkError)
        {
            Debug.print(_unityWebRequest.error.ToString());
        }
        else
        {
            AudioClip _audioClip = DownloadHandlerAudioClip.GetContent(_unityWebRequest);
            
        }
    }
}
