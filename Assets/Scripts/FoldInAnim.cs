using UnityEngine;
using UnityEngine.UI;

public class FoldInAnim : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public Image img;
    public Camera targetCamera;
    Quaternion camRot;
    public float offset = 45;
    public bool startAnimation = false;
    bool isOnAnimStart = true;
    public FoldDirection foldDirection = FoldDirection.Clockreverse;

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
        camRot = targetCamera.transform.rotation;
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        //gameObject.transform.localRotation = new(0, 45, 0, 0);
        //gameObject.transform.forward = new(0, -1, 0);
        Debug.Log(gameObject.transform.rotation);
        Debug.Log(gameObject.transform.forward);
        gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        img.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //transform.rotation = camRot * Quaternion.Euler(0, offset, 0);
    }

    // Update is called once per frame
    void Update()
    {
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
        if (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += 0.005f;
        }
        if (offset > 0)
        {
            offset -= 1f;
        }
        DoOffsetSync();
        if (gameObject.transform.localScale.x >= 1.0f)
        {
            gameObject.transform.localScale = new(
                gameObject.transform.localScale.x - 0.01f,
                gameObject.transform.localScale.x - 0.01f,
                gameObject.transform.localScale.x - 0.01f);
        }
        else
        {

            if (img.transform.localScale.x >= 1.0f)
            {
                img.transform.localScale = new(
                    img.transform.localScale.x - 0.01f,
                    img.transform.localScale.x - 0.01f,
                    img.transform.localScale.x - 0.01f);
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
