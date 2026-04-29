using System;
using UnityEngine;
using UnityEngine.UI;
using static TransparentSharedAnim;
using static ResizeSharedAnim;

using static FitImageHelper;

public class FlashInAnim : BaseAnim
{
    CanvasGroup canvasGroup;
    public Image img;

    public Canvas MainCanvas;
    public RectTransform panel;
    public RectTransform image;

    public float initSizeRatio = 0.7f;
    public float initImageRatio = 1.5f;
    public float TransparencyAnimDuration = 0.5f;
    public float ResizeDuration = 0.4f;
    public float AfterAnimDuration = 15;
    public bool EnableTransparencyAnim = true;
    public bool EnableResizeAnim = true;
    public bool EnableAfterAnim = true;
    public bool UseSmoothFormula = true;
    public bool ReverseAfterAnimDirection = false;
    //public bool startAnimation = false;
    bool isOnAnimStart = true;
    //public bool isInitialized = true;
    float timer = 0;

    //public bool animFinished = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeAnim();
    }

    void InitializeAnim()
    {
        startAnimation = false;
        isOnAnimStart = true;
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        gameObject.transform.localScale = new Vector3(initSizeRatio, initSizeRatio, initSizeRatio);
        img.transform.localScale = ReverseAfterAnimDirection?
            new Vector3(1f, 1f, 1f):
            new Vector3(initImageRatio, initImageRatio, initImageRatio);
        FitImage(panel, img, image);
        animFinished = false;
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            InitializeAnim();
        }
        if (!startAnimation)
        {
            return;
        }
        else
        {
            if (isOnAnimStart)
            {
                InitializeAnim();
                isOnAnimStart = false;
                //gameObject.transform.localPosition = ApplyRealtimePosition();
            }
        }
        timer += UnityEngine.Time.deltaTime;

        // Transparency
        ExecuteTransparentAnimUpdate(
            true,
            UseSmoothFormula,
            timer,
            TransparencyAnimDuration,
            canvasGroup);

        // Resize
        //resize
        ExecuteResizeAnimUpdate(
            UseSmoothFormula,
            EnableResizeAnim,
            EnableAfterAnim,
            ReverseAfterAnimDirection,
            initSizeRatio,
            initImageRatio,
            ResizeDuration,
            AfterAnimDuration,
            timer,
            gameObject,
            img);

        if (
                ((timer > AfterAnimDuration && !EnableResizeAnim)  //不启用resize,启用after
                || (timer > ResizeDuration + AfterAnimDuration
                && EnableResizeAnim) //启用resize和after
                || (!EnableResizeAnim && !EnableAfterAnim))//均不启用
                &&
                ((timer > TransparencyAnimDuration && EnableTransparencyAnim) //启用transparent
                || !EnableTransparencyAnim)//不启用transparent
                )
            animFinished = true;
    }

}
