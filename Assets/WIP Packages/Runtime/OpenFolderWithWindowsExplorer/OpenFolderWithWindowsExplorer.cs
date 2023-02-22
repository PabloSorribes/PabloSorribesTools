using UnityEngine;
using System.Diagnostics;

public class OpenFolderWithWindowsExplorer : MonoBehaviour
{
    public void OpenLogFileFolder()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            Arguments = Application.persistentDataPath,
            FileName = "explorer.exe"
        };
        UnityEngine.Debug.Log(startInfo.Arguments);

        string path = Application.persistentDataPath;
        path = path.Replace(@"/", @"\");
        //path = "/select," + path;
        Process.Start("explorer.exe",path);
    }
}
