using System;
using UnityEngine;
using UnityEngine.UI;

public class ResizeSharedAnim
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ExecuteResizeAnimUpdate(
        bool UseSmoothFormula,
        bool EnableResizeAnim,
        bool EnableAfterAnim,
        bool ReverseAfterAnimDirection,
        float initSizeRatio,
        float initImageRatio,
        float ResizeDuration,
        float AfterAnimDuration,
        float timer,
        GameObject gameObject,
        Image img)
    {
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        //Resize
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
        //After
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            if (EnableAfterAnim && timer <= ResizeDuration + AfterAnimDuration)
            {
                float value;
                if (UseSmoothFormula)
                {
                    value =
                        ReverseAfterAnimDirection ?
                        Convert.ToSingle(
                            MathRepo.SmoothMovePhysicFormula(
                                1f,
                                initImageRatio,
                                AfterAnimDuration,
                                (timer - ResizeDuration))) :
                        Convert.ToSingle(
                            MathRepo.SmoothMovePhysicFormula(
                                initImageRatio,
                                1f,
                                AfterAnimDuration,
                                (timer - ResizeDuration)));
                }
                else
                {
                    float percentage =
                        Mathf.Clamp01(
                            (timer - ResizeDuration)
                            / AfterAnimDuration);
                    value = ReverseAfterAnimDirection ?
                        Mathf.Lerp(1f, initImageRatio, percentage) :
                        Mathf.Lerp(initImageRatio, 1f, percentage);
                    //Debug.Log($"after = {percentage}");
                    //Debug.Log($"time = {timer -  ResizeDuration}");
                }
                img.transform.localScale = new(value, value, value);
            }
            else
            {
                img.transform.localScale = ReverseAfterAnimDirection ?
                    new(initImageRatio, initImageRatio, initImageRatio) :
                    new(1f, 1f, 1f);
            }

        }
    }
}
