using System;
using UnityEngine;

public class PrefabPageCommon : MonoBehaviour
{
    public bool StartAnim = false;
    public virtual Canvas MainCanvas { get; set; }
    public virtual GameObject ImgPrefab { get; set; }
    public virtual Transform parent { get; set; }
    public virtual Camera targetCamera { get; set; }
    public virtual int COUNT => 0;
    public virtual float Delay => 0.2f;

    protected float _timer = 0f;
    public virtual float timer
    {
        get => _timer;
        set => _timer = value;
    }

    public virtual GameObject[] gameObjects { get; set; }
    public virtual BaseAnim[] components { get; set; }

    public virtual Type AnimType { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        //Debug.Log("start.");
        if (gameObjects == null || components == null)
        {
            InitAllLists();
        }
    }

    public virtual void InitAllLists()
    {
        if (AnimType == null)
            AnimType = typeof(FlashInAnim);
        //if (ImgPrefab == null)
        //    ImgPrefab = HySliderDaemon.Instance.ImgPrefab;
        //if (parent == null)
        //    parent = HySliderDaemon.Instance.ScreenPanel.GetComponent<Transform>();
        //if (targetCamera == null)
        //    targetCamera = HySliderDaemon.Instance.camera;
        //if (MainCanvas == null)
        //    MainCanvas = HySliderDaemon.Instance.MainCanvas;
        gameObjects = new GameObject[COUNT];
        components = new BaseAnim[COUNT];

        for (int i = 0; i < COUNT; i++)
        {
            gameObjects[i] = InstantiateImgPrefab();
            components[i] = gameObjects[i].GetComponent(AnimType) as BaseAnim;
        }
    }

    public virtual void Awake()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!StartAnim)
            return;
        timer += Time.deltaTime;
        //Debug.Log(timer);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (!components[i].startAnimation && timer >= Delay * i)
            {
                components[i].startAnimation = true;
            }
        }
    }

    public GameObject InstantiateImgPrefab()
    {
        var result = Instantiate(ImgPrefab);
        result.transform.SetParent(parent);
        result.GetComponent<FoldInAnim>().targetCamera = targetCamera;
        result.GetComponent<SlideInAnim>().MainCanvas = MainCanvas;
        return result;
    }

    public bool AllCompleted()
    {
        for (int i = 0; i < COUNT; i++)
        {
            if (!components[i].animFinished)
            {
                return false;
            }
        }
        return true;
    }

    public void SetAnim<T>() where T : BaseAnim
    {
        if (gameObjects == null || components == null)
        {
            InitAllLists();
        }
        AnimType = typeof(T);
        for (int i = 0; i < COUNT; i++)
        {
            //Debug.Log(i);
            //Debug.Log(gameObjects[i] == null);
            //Debug.Log(gameObjects[i].GetComponent(AnimType) == null);
            components[i] = gameObjects[i].GetComponent(AnimType) as BaseAnim;
            
        }
    }

    protected void ConfigImgPrefab(GameObject gameObject, Vector2 size,
        Vector2 position)
    {
        var rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        gameObject.transform.localPosition = new(position.x,
            position.y,
            0);
    }
}
