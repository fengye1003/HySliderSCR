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

    Dictionary<string, Texture2D> lastTextures;
    Dictionary<string, Texture2D> nextTextures;


    List<string> filteredStrs;

    bool switching = false;

    float timer = 0f;

    string Path => Application.persistentDataPath;

    public static Hashtable MainConfig;
    public static Hashtable AnimConfig;
    public static Hashtable PageConfig;

    static bool reverseAfterAnim = false;
    public static string imgPath = "D:/imgs/";
    static float maxImgScale = 1.2f;
    static float maxBlockScale = 1.2f;
    static float maxAngleDeg = 30;
    static float afterAnimDuration = 15;
    static float transparencyDuration = 0.5f;
    static float resizeDuration = 0.4f;
    static float foldDuration = 0.5f;
    static float slideDuration = 0.4f;
    static bool useSmoothFormula = true;

    public static Hashtable mainConfigStandard = new()
    {
        { "type", "hySliderDaemon.Main" },
        { "imgPath", "D:/imgs/" },
        { "reverseAfterAnim" , "false" },
        { "maxImgScale", "1.2" },
        { "maxBlockScale", "1.2" },
        { "maxAngleDeg", "30" },
        { "transparencyDuration", "0.5" },
        { "afterAnimDuration", "15" },
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
            Directory.CreateDirectory(Path + "/Properties");
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

        reverseAfterAnim =
            (string)MainConfig["reverseAfterAnim"] == "true";
        useSmoothFormula =
            (string)MainConfig["useSmoothFormula"] == "true";
        maxImgScale =
            Convert.ToSingle(
                (string)MainConfig["maxImgScale"]);
        maxBlockScale =
            Convert.ToSingle(
                (string)MainConfig["maxBlockScale"]);
        maxAngleDeg =
            Convert.ToSingle(
                (string)MainConfig["maxAngleDeg"]);
        transparencyDuration =
            Convert.ToSingle(
                (string)MainConfig["transparencyDuration"]);
        resizeDuration =
            Convert.ToSingle(
                (string)MainConfig["resizeDuration"]);
        foldDuration =
            Convert.ToSingle(
                (string)MainConfig["foldDuration"]);
        slideDuration =
            Convert.ToSingle(
                (string)MainConfig["slideDuration"]);
        afterAnimDuration =
            Convert.ToSingle(
                (string)MainConfig["afterAnimDuration"]);
        imgPath = (string)MainConfig["imgPath"];
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
                //int index = r.Next(PagePrefabs.Count);
                int index = RandomController.GetRandomPagePrefabId();
                lastTextures = nextTextures;
                switch (RandomController.GetRandomAnimId())
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
                    if (lastTextures != null)
                    {
                        foreach (var item in lastTextures)
                        {
                            Destroy(item.Value);
                        }
                    }
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

        if (typeof(T) == typeof(SlideInAnim))
        {
            foreach (SlideInAnim sia in obj.components)
            {

                sia.initImageRatio = maxImgScale;
                sia.initSizeRatio = maxBlockScale;
                sia.SlideDuration = slideDuration;
                sia.ResizeDuration = resizeDuration;
                sia.AfterAnimDuration = afterAnimDuration;
                sia.ReverseAfterAnimDirection = reverseAfterAnim;
                sia.UseSmoothFormula = useSmoothFormula;
            }
        }
        else if (typeof(T) == typeof(FoldInAnim))
        {
            foreach (FoldInAnim foia in obj.components)
            {

                foia.initImageRatio = maxImgScale;
                foia.initSizeRatio = maxBlockScale;
                foia.FoldAnimDuration = foldDuration;
                foia.ResizeDuration = resizeDuration;
                foia.AfterAnimDuration = afterAnimDuration;
                foia.ReverseAfterAnimDirection = reverseAfterAnim;
                foia.UseSmoothFormula = useSmoothFormula;
            }
        }
        else if (typeof(T) == typeof(FlashInAnim))
        {
            foreach (FlashInAnim flia in obj.components)
            {

                flia.initImageRatio = maxImgScale;
                flia.initSizeRatio = maxBlockScale;
                flia.ResizeDuration = resizeDuration;
                flia.AfterAnimDuration = afterAnimDuration;
                flia.ReverseAfterAnimDirection = reverseAfterAnim;
                flia.UseSmoothFormula = useSmoothFormula;
            }
        }



        var images = HySliderFilesHelper.FetchRandomImages(obj.COUNT, filteredStrs);
        nextTextures = images;
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
            obj.gameObjects[i].transform.GetChild(0)
                .GetComponent<Image>().name = images.Keys.ToList<string>()[i];
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

    public void KillOtherInstances()
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
