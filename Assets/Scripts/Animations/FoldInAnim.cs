using System;
using UnityEngine;
using UnityEngine.UI;
using static TransparentSharedAnim;
using static ResizeSharedAnim;
using static FitImageHelper;


public class FoldInAnim : BaseAnim
{
    CanvasGroup canvasGroup;
    public Image img;
    public Camera targetCamera;
    Quaternion camRot;

    public RectTransform panel;
    public RectTransform image;

    public float initOffset = 30;
    float offset;
    public float initSizeRatio = 1.2f;
    public float initImageRatio = 1.5f;
    public float TransparencyAnimDuration = 0.5f;
    public float ResizeDuration = 0.4f;
    public float FoldAnimDuration = 0.5f;
    public float AfterAnimDuration = 15;
    public bool EnableResizeAnim = true;
    public bool EnableAfterAnim = true;
    public bool UseSmoothFormula = true;
    public bool ReverseAfterAnimDirection = false;
    //public bool startAnimation = false;
    bool isOnAnimStart = true;
    public FoldDirection foldDirection = FoldDirection.Clockreverse;
    //public bool isInitialized = true;
    float timer = 0;

    //public bool animFinished = false;

    public enum FoldDirection
    {
        Clockwise,
        Clockreverse,
        Down,
        Up,
        //UpClockwise,
        //UpClockreverse,
        //DownClockwise,
        //DownClockreverse
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeAnim();
    }

    void InitializeAnim()
    {
        offset = initOffset;
        startAnimation = false;
        isOnAnimStart = true;
        timer = 0;
        camRot = targetCamera.transform.rotation;
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        //gameObject.transform.localRotation = new(0, 45, 0, 0);
        //gameObject.transform.forward = new(0, -1, 0);
        //Debug.Log(gameObject.transform.rotation);
        //Debug.Log(gameObject.transform.forward);
        gameObject.transform.localScale = new Vector3(initSizeRatio, initSizeRatio, initSizeRatio);
        img.transform.localScale = ReverseAfterAnimDirection ?
            new Vector3(1f, 1f, 1f) :
            new Vector3(initImageRatio, initImageRatio, initImageRatio);
        //transform.rotation = camRot * Quaternion.Euler(0, offset, 0);
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
                DoOffsetSync();
                isOnAnimStart = false;
            }
        }
        timer += UnityEngine.Time.deltaTime;

        //Transparency
        ExecuteTransparentAnimUpdate(
            true,
            UseSmoothFormula,
            timer, 
            TransparencyAnimDuration, 
            canvasGroup);

        //Main Fold
        if (timer <= FoldAnimDuration) 
        {
            if (UseSmoothFormula)
            {
                offset = Convert.ToSingle(MathRepo.SmoothMovePhysicFormula(initOffset, 0f, FoldAnimDuration, timer));
            }
            else
            {
                float percentage = Mathf.Clamp01(timer / FoldAnimDuration);
                offset = Mathf.Lerp(initOffset, 0f, percentage);
                //Debug.Log($"offset = {percentage}");
            }
        }
        else
        {
            offset = 0f;
        }

        DoOffsetSync();

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
                ((timer > FoldAnimDuration)
                ))
            animFinished = true;
    }

    void DoOffsetSync()
    {
        switch (foldDirection)
        {
            case FoldDirection.Clockwise:
                transform.rotation = camRot * Quaternion.Euler(0, -offset, 0);
                break;
            case FoldDirection.Clockreverse:
                transform.rotation = camRot * Quaternion.Euler(0, offset, 0);
                break;
            case FoldDirection.Down:
                transform.rotation = camRot * Quaternion.Euler(-offset, 0, 0);
                break;
            case FoldDirection.Up:
                transform.rotation = camRot * Quaternion.Euler(offset, 0, 0);
                break;
            //case FoldDirection.UpClockwise:
            //    transform.rotation = camRot * Quaternion.Euler(offset, -offset, 0);
            //    break;
            //case FoldDirection.UpClockreverse:
            //    transform.rotation = camRot * Quaternion.Euler(offset, offset, 0);
            //    break;
            //case FoldDirection.DownClockwise:
            //    transform.rotation = camRot * Quaternion.Euler(-offset, -offset, 0);
            //    break;
            //case FoldDirection.DownClockreverse:
            //    transform.rotation = camRot * Quaternion.Euler(-offset, offset, 0);
            //    break;
            default:
                break;
        }
        
    }
}
