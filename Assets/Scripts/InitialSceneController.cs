using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;

public class InitialSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage AnimFrame;
    public static bool animCompleted = false;
    CanvasGroup imgCanvasGroup;

    float timer = 0f;
    float fadeDuration = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imgCanvasGroup = AnimFrame.GetComponent<CanvasGroup>();
        StartCoroutine(DelayPlay());
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Update is called once per frame
    void Update()
    {

        if (animCompleted)
        {
            var alpha = imgCanvasGroup.alpha;
            if (timer <= fadeDuration)
            {
                float percentage =
                        Mathf.Clamp01(
                            (timer)
                            / fadeDuration);
                imgCanvasGroup.alpha = Mathf.Lerp(1f, 0f, percentage);
                timer += Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene("SliderScene");
            }
        }
    }

    IEnumerator DelayPlay()
    {
        yield return new WaitForSeconds(1f);
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Logo animation play accomplished.");
        //SetIniTextTransparency(1f);
        animCompleted = true;
    }
}
