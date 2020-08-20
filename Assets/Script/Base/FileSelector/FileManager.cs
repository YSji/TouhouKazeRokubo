using System.Runtime.InteropServices;
//脚本FileManager
public static class FileManager
{
    private static OpenFileName OpenFileManager(string str)
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        //文件类型 config配置文件,"Excel文件(*.xlsx)\0*.xlsx" ,"Txt文件(*.txt)\0*.txt"...
        openFileName.filter = "config文件(*.conf)\0*.conf";
        openFileName.file = new string(new char[256]);//new一个256字符的string
        openFileName.maxFile = openFileName.file.Length;//获取256字符的string的长度作为最大
        openFileName.fileTitle = new string(new char[64]);//64字符的string
        openFileName.maxFileTitle = openFileName.fileTitle.Length;//文件标题的最大长度
        openFileName.initialDir = UnityEngine.Application.dataPath;//默认路径
        openFileName.title = str;//文件标题
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        return openFileName;
    }
 
    public static void OpenFile()
    {
        OpenFileName openFileName = OpenFileManager("OpenFile");
        if (LocalDialog.GetOpenFileName(openFileName))
        {
 
        }
    }
 
    public static void SaveFile()
    {
        OpenFileName openFileName = OpenFileManager("SaveFile");
        if (LocalDialog.GetSaveFileName(openFileName))
        {
            var nowPath = openFileName.file;
            if (!nowPath.EndsWith(".conf"))//是不是以".conf"结尾
            {
                nowPath = openFileName.file + ".conf";
            }
        }
    }
}
 