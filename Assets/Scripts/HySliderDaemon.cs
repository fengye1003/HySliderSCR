using NUnit.Framework;
using System.Collections.Generic;
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

    float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        var obj = Instantiate<PrefabPageCommon>(PagePrefabs[0]);
        obj.parent = ScreenPanel.transform;
        obj.ImgPrefab = ImgPrefab;
        obj.MainCanvas = MainCanvas;
        obj.targetCamera = Camera.main;
        obj.SetAnim<SlideInAnim>();
        obj.StartAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
