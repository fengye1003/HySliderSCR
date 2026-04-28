using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FoldInAnim : MonoBehaviour
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
    public bool UseSmoothFormula = true;
    public bool startAnimation = false;
    bool isOnAnimStart = true;
    public FoldDirection foldDirection = FoldDirection.Clockreverse;
    public bool isInitialized = true;
    float timer = 0;

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
        img.transform.localScale = new Vector3(initImageRatio, initImageRatio, initImageRatio);
        //transform.rotation = camRot * Quaternion.Euler(0, offset, 0);
        FitImage();
        isInitialized = true;
    }

    public void FitImage()
    {
        float panelW = panel.rect.width;
        float panelH = panel.rect.height;

        // 用 sprite 原始尺寸，避免被 layout 污染
        float imgW = img.sprite.rect.width;
        float imgH = img.sprite.rect.height;

        float scale = Mathf.Max(panelW / imgW, panelH / imgH);

        float targetW = imgW * scale;
        float targetH = imgH * scale;

        image.sizeDelta = new Vector2(targetW, targetH);
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
                DoOffsetSync();
                isOnAnimStart = false;
            }
        }
        timer += UnityEngine.Time.deltaTime;
        if (timer <= TransparencyAnimDuration)
        {
            
            if (UseSmoothFormula)
            {
                canvasGroup.alpha = Convert.ToSingle(MathRepo.SmoothMovePhysicFormula(0f, 1f, TransparencyAnimDuration, timer));
            }
            else
            {
                float percentage = Mathf.Clamp01(timer / TransparencyAnimDuration);
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, percentage);
                //Debug.Log($"alpha = {percentage}");
            }
        }
        else
        {
            canvasGroup.alpha = 1f;
        }

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

        if (timer <= ResizeDuration)
        {
            float value;
            if (!UseSmoothFormula)
            {
                value = Convert.ToSingle(MathRepo.SmoothMovePhysicFormula(initSizeRatio, 1f, ResizeDuration, timer));
            }
            else
            {
                float percentage = Mathf.Clamp01(timer / ResizeDuration);
                value = Mathf.Lerp(initSizeRatio, 1f, percentage);
                //Debug.Log($"resize = {percentage}");
            }
            gameObject.transform.localScale = new(value, value, value);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            if (timer <= ResizeDuration + AfterAnimDuration)
            {
                float value;
                if (UseSmoothFormula)
                {
                    value = Convert.ToSingle(MathRepo.SmoothMovePhysicFormula(initImageRatio, 1f, AfterAnimDuration, (timer - ResizeDuration)));
                }
                else
                {
                    float percentage = Mathf.Clamp01((timer - ResizeDuration) / AfterAnimDuration);
                    value = Mathf.Lerp(initImageRatio, 1f, percentage);
                    //Debug.Log($"after = {percentage}");
                    //Debug.Log($"time = {timer -  ResizeDuration}");
                }
                img.transform.localScale = new(value, value, value);
            }
            else
            {
                img.transform.localScale = new(1f, 1f, 1f);
            }
        }
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
