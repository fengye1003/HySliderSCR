using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FlashInAnim : MonoBehaviour
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
    public bool UseSmoothFormula = true;
    public bool startAnimation = false;
    bool isOnAnimStart = true;
    public bool isInitialized = true;
    float timer = 0;

    public bool animFinished = false;
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
        img.transform.localScale = new Vector3(initImageRatio, initImageRatio, initImageRatio);
        FitImage();
        animFinished = false;
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
                isOnAnimStart = false;
                //gameObject.transform.localPosition = ApplyRealtimePosition();
            }
        }
        timer += UnityEngine.Time.deltaTime;

        // Transparency
        if (EnableTransparencyAnim && timer <= TransparencyAnimDuration)
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

        // Resize
        if (timer <= ResizeDuration)
        {
            if (EnableResizeAnim)
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
                animFinished = true;
            }
        }
    }
}
