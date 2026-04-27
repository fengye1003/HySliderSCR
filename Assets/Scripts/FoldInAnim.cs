using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FoldInAnim : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public Image img;
    public Camera targetCamera;
    Quaternion camRot;
    public float initOffset = 45;
    float offset;
    public float initSizeRatio = 1.3f;
    public float initImageRatio = 1.5f;
    public float TransparencyAnimDuration = 2.0f;
    public float ResizeDuration = 2.0f;
    public float FoldAnimDuration = 2.0f;
    public float AfterAnimDuration = 10.0f;
    public bool UseSmoothFormula = false;
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
                DoOffsetSync();
                isOnAnimStart = false;
            }
        }
        timer += UnityEngine.Time.deltaTime;
        if (timer <= TransparencyAnimDuration)
        {
            
            if (UseSmoothFormula)
            {

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
            float percentage = Mathf.Clamp01(timer / ResizeDuration);
            var value = Mathf.Lerp(initSizeRatio, 1f, percentage);
            gameObject.transform.localScale = new(value, value, value);
            //Debug.Log($"resize = {percentage}");
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            if (timer <= ResizeDuration + AfterAnimDuration)
            {
                float percentage = Mathf.Clamp01((timer - ResizeDuration) / AfterAnimDuration);
                var value = Mathf.Lerp(initImageRatio, 1f, percentage);
                img.transform.localScale = new(value, value, value);
                //Debug.Log($"after = {percentage}");
                //Debug.Log($"time = {timer -  ResizeDuration}");
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
