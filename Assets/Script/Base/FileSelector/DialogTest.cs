using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class DialogTest : MonoBehaviour
{

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Open"))
        {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = "音频文件(*.mp3;*.wav;*.ogg)\0*.mp3;*.wav;*.ogg\0All files(*.*)\0*.*";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
            openFileName.title = "窗口标题";
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

            if (LocalDialog.GetSaveFileName(openFileName))
            {
                Debug.print(openFileName.file);
                Debug.print(openFileName.fileTitle);
            }
        }
    }
}