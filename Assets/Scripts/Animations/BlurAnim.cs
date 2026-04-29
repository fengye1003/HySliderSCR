using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.UI;
using static FitImageHelper;
using static ResizeSharedAnim;
using static TransparentSharedAnim;

public class BlurAnim : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public Image img;

    public RectTransform panel;
    public RectTransform image;
    Material runtimeBlurMatShader;

    public float initSizeRatio = 1.2f;
    public float initImageRatio = 1.5f;
    public float initBlurStrength = 10f;
    public float TransparencyAnimDuration = 0.5f;
    public float ResizeDuration = 0.4f;
    public float BlurLiftDuration = 10f;
    public float AfterAnimDuration = 15;
    public bool EnableTransparencyAnim = true;
    public bool EnableAfterAnim = true;
    public bool EnableResizeAnim = true;
    public bool ReverseAfterAnimDirection = false;
    public bool ReverseBlurAnim = false;
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
        runtimeBlurMatShader = Instantiate(img.material);
        img.material = runtimeBlurMatShader;
        SetBlur(ReverseBlurAnim ? 0f : initBlurStrength);
        startAnimation = false;
        isOnAnimStart = true;
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        gameObject.transform.localScale = new Vector3(initSizeRatio, initSizeRatio, initSizeRatio);
        img.transform.localScale = ReverseAfterAnimDirection ?
            new Vector3(1f, 1f, 1f) :
            new Vector3(initImageRatio, initImageRatio, initImageRatio);
        FitImage(panel, img, image);
        animFinished = false;
        isInitialized = true;
    }

    //void Awake()
    //{
    //    // 用 shader 创建一个独立材质（关键）
    //    runtimeBlurMatShader = new Material(img.material.shader);
    //    runtimeBlurMatShader.CopyPropertiesFromMaterial(img.material);

    //    img.material = runtimeBlurMatShader;
    //}

    public void SetBlur(float value)
    {
        runtimeBlurMatShader.SetFloat("_BlurSize", value);
        //Debug.Log(runtimeBlurMatShader.GetFloat("_BlurSize"));
        img.SetMaterialDirty();
        Debug.Log(img.materialForRendering == runtimeBlurMatShader);
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

        float blur;
        //Main Blur
        if (timer <= BlurLiftDuration)
        {
            if (UseSmoothFormula)
            {
                blur = ReverseBlurAnim ?
                    Convert.ToSingle(
                        MathRepo.SmoothMovePhysicFormula(
                            0f,
                            initBlurStrength,
                            BlurLiftDuration,
                            timer)) :
                    Convert.ToSingle(
                        MathRepo.SmoothMovePhysicFormula(
                            initBlurStrength,
                            0f,
                            BlurLiftDuration,
                            timer));
            }
            else
            {
                float percentage = Mathf.Clamp01(timer / BlurLiftDuration);
                blur = ReverseBlurAnim ?
                    Mathf.Lerp(0f, initBlurStrength, percentage) :
                    Mathf.Lerp(initBlurStrength, 0f, percentage);
                //Debug.Log($"offset = {percentage}");
            }
        }
        else
        {
            blur = 0f;
        }
        SetBlur(blur);

        if (
                ((timer > AfterAnimDuration && !EnableResizeAnim)  //不启用resize,启用after
                || (timer > ResizeDuration + AfterAnimDuration
                && EnableResizeAnim) //启用resize和after
                || (!EnableResizeAnim && !EnableAfterAnim))//均不启用
                &&
                ((timer > BlurLiftDuration)
                )
                &&
                ((timer > TransparencyAnimDuration && EnableTransparencyAnim) //启用transparent
                || !EnableTransparencyAnim)//不启用transparent
            )
            animFinished = true;
    }
}
