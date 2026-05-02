using Playsis.Essencial_Repos;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    float timer;
    string Path => Application.persistentDataPath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Debug.Log(Application.persistentDataPath);
#if UNITY_EDITOR
		return;
#endif
        LockScreenHelper.ParseArgs();
        if (!LockScreenHelper.isPreview && !LockScreenHelper.isConfig && !LockScreenHelper.isSmall)
        {

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            LockScreenHelper.ForceTopMost();
#endif
        }
        if (LockScreenHelper.isPreview)
        {
            var r = Screen.currentResolution;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(r.width, r.height, true);
            //Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        if (LockScreenHelper.isSmall)
        {

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            LockScreenHelper.AttachToPreviewWindow();
#endif
        }
        if (LockScreenHelper.isConfig)
        {
            string path = Path;
            path = System.IO.Path.Combine(Path, "Properties/main.properties").Replace("/", "\\");
            if (!System.IO.File.Exists(path))
            {
                PropertiesHelper.FixProperties(HySliderDaemon.mainConfigStandard, path);
            }
            
            Log.SaveLog(path);
            Process.Start("explorer.exe", "/select,\"" + path + "\"");
#if !UNITY_EDITOR

            Application.Quit();
#endif
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        
        if (hasFocus)
        {
            return;
        }
        //if (timer <= 2f)//fxxk you nvidia//sorry its my fault
        //    return;

//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif

        if (!LockScreenHelper.isPreview && !LockScreenHelper.isConfig && !LockScreenHelper.isSmall)
        {

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        LockScreenHelper.Lock();
#endif

#if !UNITY_EDITOR

            Application.Quit();
#endif
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.anyKeyDown)
        {
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        if (!LockScreenHelper.isPreview && !LockScreenHelper.isConfig && !LockScreenHelper.isSmall)
        {
            LockScreenHelper.Lock();

            
        }
#endif
#if !UNITY_EDITOR

            Application.Quit();
#endif
        }
    }
}
