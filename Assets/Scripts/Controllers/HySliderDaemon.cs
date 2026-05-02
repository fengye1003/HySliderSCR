using Playsis.Essencial_Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HySliderDaemon : MonoBehaviour
{
    public static HySliderDaemon Instance;
    public Canvas MainCanvas;
    public GameObject ImgPrefab;
    public GameObject ScreenPanel;
    public new Camera camera;

    public List<PrefabPageCommon> PagePrefabs = new();

    PrefabPageCommon lastPage;
    //Point
    PrefabPageCommon nextPage;

    List<string> filteredStrs;

    bool switching = false;

    float timer = 0f;

    string Path = AppDomain.CurrentDomain.BaseDirectory;

    public static Hashtable MainConfig;
    public static Hashtable AnimConfig;
    public static Hashtable PageConfig;

    public static Hashtable mainConfigStandard = new()
    {
        { "type", "hySliderDaemon.Main" },
        { "reverseAfterAnim" , "false" },
        { "maxImgScale", "1.2" },
        { "maxBlockScale", "1.2" },
        { "maxAngleDeg", "30" },
        { "transparencyDuration", "0.5" },
        { "resizeDuration", "0.4" },
        { "foldDuration", "0.5" },
        { "slideDuration", "0.4" },
        { "useSmoothFormula", "true" },
    };

    public static Hashtable animWeightsConfigStandard = new()
    {
        { "type", "hySliderDaemon.AnimWeights" },
        { "SlideInAnimWeight" , "5" },
        { "FlashInAnimWeight", "5" },
        { "FoldInAnimWeight", "10" },
    };

    public static Hashtable pageWeightsConfigStandard = new()
    {
        { "type", "hySliderDaemon.AnimWeights" },
        { "prefab0" , "intExample" },
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        


#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
#else
            KillOtherInstances();
#endif
        if (!Directory.Exists(Path + "/Properties"))
        {
            Directory.CreateDirectory(Path + "./Properties");
        }
        MainConfig = 
            PropertiesHelper.AutoCheck(
                mainConfigStandard, 
                Path + "/Properties/main.properties");
        AnimConfig = 
            PropertiesHelper.AutoCheck(
                animWeightsConfigStandard, 
                Path + "/Properties/animations.properties");
        PageConfig =
            PropertiesHelper.AutoCheck(
                pageWeightsConfigStandard,
                Path + "/Properties/pages.properties");
    }

    

    // Update is called once per frame
    void Update()
    {

        System.Random r = new();
        if (lastPage == null || lastPage.AllCompleted())
        {
            if (!switching)
            {
                switching = true;
                int index = r.Next(PagePrefabs.Count);
                switch (r.Next(3))
                {
                    case 0:
                        nextPage = InstantiatePagePrefab<SlideInAnim>(index);
                        break;
                    case 1:
                        nextPage = InstantiatePagePrefab<FlashInAnim>(index);
                        break;
                    case 2:
                        nextPage = InstantiatePagePrefab<FoldInAnim>(index);
                        break;
                    default:
                        nextPage = InstantiatePagePrefab<FlashInAnim>(index);
                        break;
                }
            }
            else
            {
                if (nextPage.AllCompleted())
                {
                    if (lastPage != null) DestroyPage(lastPage);
                    lastPage = nextPage;
                    switching = false;
                }
            }
        }
    }

    PrefabPageCommon InstantiatePagePrefab<T>(int index) where T : BaseAnim
    {
        var obj = Instantiate<PrefabPageCommon>(PagePrefabs[index]);
        obj.parent = ScreenPanel.transform;
        obj.ImgPrefab = ImgPrefab;
        obj.MainCanvas = MainCanvas;
        obj.targetCamera = Camera.main;
        obj.SetAnim<T>();

        var images = HySliderFilesHelper.FetchRandomImages(obj.COUNT, filteredStrs);
        filteredStrs = images.Keys.ToList<string>();
        for (int i = 0; i < obj.COUNT; i++)
        {
            var tex = images.Values.ToList<Texture2D>()[i];
            obj.gameObjects[i].transform.GetChild(0)
                .GetComponent<Image>().sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f)
                );
            //Debug.Log(obj.gameObjects[i].transform.GetChild(0).name);
            //
        }

        obj.StartAnim = true;
        return obj;
    }

    void DestroyPage(PrefabPageCommon page)
    {
        foreach (var item in page.gameObjects)
        {
            Destroy(item);
        }
        Destroy(page.gameObject);
        //Debug.Log("Destroyed last page.");
    }

    void KillOtherInstances()
    {
        Process current = Process.GetCurrentProcess();
        string processName = current.ProcessName;

        Process[] processes = Process.GetProcessesByName(processName);

        foreach (Process proc in processes)
        {
            try
            {
                // 跳过当前进程
                if (proc.Id == current.Id)
                    continue;

                // 可选：进一步校验路径，避免误杀同名程序
                if (proc.MainModule.FileName != current.MainModule.FileName)
                    continue;

                UnityEngine.Debug.Log($"Killing duplicate process: {proc.Id}");

                proc.Kill();
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogWarning($"Failed to kill process {proc.Id}: {e.Message}");
            }
        }
    }
}
