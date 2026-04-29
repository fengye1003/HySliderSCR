using System;
using UnityEngine;

public class TransparentSharedAnim : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ExecuteTransparentAnimUpdate(
        bool EnableTransparencyAnim,
        bool UseSmoothFormula,
        float timer,
        float maxt,
        CanvasGroup canvasGroup)
    {
        if (EnableTransparencyAnim && timer <= maxt)
        {

            if (UseSmoothFormula)
            {
                canvasGroup.alpha = 
                    Convert.ToSingle(
                        MathRepo.SmoothMovePhysicFormula(
                            0f, 1f, maxt, timer));
            }
            else
            {
                float percentage = Mathf.Clamp01(timer / maxt);
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, percentage);
                //Debug.Log($"alpha = {percentage}");
            }
        }
        else
        {
            canvasGroup.alpha = 1f;
        }
    }
}
