using System;
using UnityEngine;
using UnityEngine.UI;
using static TransparentSharedAnim;
using static ResizeSharedAnim;
using static FitImageHelper;

public class SlideInAnim : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public Image img;

    public Canvas MainCanvas;
    public RectTransform panel;
    public RectTransform image;

    public float initSizeRatio = 1.2f;
    public float initImageRatio = 1.5f;
    public float TransparencyAnimDuration = 0.5f;
    public float ResizeDuration = 0.4f;
    public float SlideDuration = 0.4f;
    public float AfterAnimDuration = 15;
    public bool EnableTransparencyAnim = true;
    public bool EnableAfterAnim = true;
    public bool EnableResizeAnim = true;
    public bool ReverseAfterAnimDirection = false;
    public bool UseSmoothFormula = true;
    public bool startAnimation = false;
    bool isOnAnimStart = true;
    public SlideDirection slideDirection = SlideDirection.LogicalRandom;
    public bool isInitialized = true;
    float timer = 0;

    Vector3 dest;

    public bool animFinished = false;

    public enum SlideDirection
    {
        LeftToRight,
        RightToLeft,
        UpToDown,
        DownToUp,
        LogicalRandom
    }
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
        img.transform.localScale = ReverseAfterAnimDirection ?
            new Vector3(1f, 1f, 1f) :
            new Vector3(initImageRatio, initImageRatio, initImageRatio);
        FitImage(panel, img, image);
        dest = gameObject.transform.localPosition;
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
                gameObject.transform.localPosition = ApplyRealtimePosition();
            }
        }
        timer += UnityEngine.Time.deltaTime;

        // Transparency
        ExecuteTransparentAnimUpdate(
            EnableTransparencyAnim,
            UseSmoothFormula,
            timer,
            TransparencyAnimDuration,
            canvasGroup);

        // Slide
        if (timer <= SlideDuration)
        {
            gameObject.transform.localPosition = ApplyRealtimePosition();
        }
        else
        {
            gameObject.transform.localPosition = dest;
        }

        // Resize
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
                ((timer > SlideDuration)
                )
                &&
                ((timer > TransparencyAnimDuration && EnableTransparencyAnim) //启用transparent
                || !EnableTransparencyAnim)//不启用transparent
            )
            animFinished = true;

    }

    Vector3 ApplyRealtimePosition()
    {
        var x = transform.localPosition.x;
        var y = transform.localPosition.y;
        var z = transform.localPosition.z;
        var leftOutBound = -MainCanvas.
            GetComponent<RectTransform>().
            sizeDelta.x / 2
                        - panel.sizeDelta.x / 2;
        var rightOutBound = MainCanvas.
            GetComponent<RectTransform>().
            sizeDelta.x / 2
                        + panel.sizeDelta.x / 2;
        var upOutBound = MainCanvas.
            GetComponent<RectTransform>().
            sizeDelta.y / 2
                        + panel.sizeDelta.y / 2;
        var downOutBound = -MainCanvas.
            GetComponent<RectTransform>().
            sizeDelta.y / 2
                        - panel.sizeDelta.y / 2;
        float percentage = Mathf.Clamp01(timer / SlideDuration);
        switch (slideDirection)
        {
            case SlideDirection.LeftToRight:
                if (UseSmoothFormula)
                {
                    x = Convert.ToSingle(
                        MathRepo.SmoothMovePhysicFormula(
                            leftOutBound, 
                            dest.x, 
                            SlideDuration, 
                            timer));
                }
                else
                {
                    x = Mathf.Lerp(
                        leftOutBound,
                        dest.x,
                        percentage);
                }
                break;
            case SlideDirection.RightToLeft:
                if (UseSmoothFormula)
                {
                    x = Convert.ToSingle(
                        MathRepo.SmoothMovePhysicFormula(
                            rightOutBound,
                            dest.x,
                            SlideDuration,
                            timer));
                }
                else
                {
                    x = Mathf.Lerp(
                        rightOutBound,
                        dest.x,
                        percentage);
                }
                break;
            case SlideDirection.UpToDown:
                if (UseSmoothFormula)
                {
                    y = Convert.ToSingle(
                        MathRepo.SmoothMovePhysicFormula(
                            upOutBound,
                            dest.y,
                            SlideDuration,
                            timer));
                }
                else
                {
                    y = Mathf.Lerp(
                        upOutBound,
                        dest.y,
                        percentage);
                }
                break;
            case SlideDirection.DownToUp:
                if (UseSmoothFormula)
                {
                    y = Convert.ToSingle(
                        MathRepo.SmoothMovePhysicFormula(
                            downOutBound,
                            dest.y,
                            SlideDuration,
                            timer));
                }
                else
                {
                    y = Mathf.Lerp(
                        downOutBound,
                        dest.y,
                        percentage);
                }
                break;
            case SlideDirection.LogicalRandom:
                System.Random r = new();
                if (dest.x <= 0 && dest.y >= 0)
                {
                    slideDirection = r.Next(2) == 0 ?
                        SlideDirection.LeftToRight :
                        SlideDirection.UpToDown;
                }
                else if (dest.x > 0 && dest.y > 0) 
                {
                    slideDirection = r.Next(2) == 0 ?
                        SlideDirection.RightToLeft :
                        SlideDirection.UpToDown;
                }
                else if (dest.x < 0 && dest.y < 0)
                {
                    slideDirection = r.Next(2) == 0 ?
                        SlideDirection.LeftToRight :
                        SlideDirection.DownToUp;
                }
                else
                {
                    slideDirection = r.Next(2) == 0 ?
                        SlideDirection.RightToLeft :
                        SlideDirection.DownToUp;
                }
                break;
            default:
                throw new Exception("Undefined behavior");
                //break;
                
        }
        return new Vector3(x, y, z);
    }
}
