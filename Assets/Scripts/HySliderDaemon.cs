using System;
using System.Collections.Generic;
using System.Linq;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        
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
}
