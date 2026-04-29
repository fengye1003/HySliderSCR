using System;
using UnityEngine;

public class PrefabPageCommon : MonoBehaviour
{
    public virtual Canvas MainCanvas { get; set; }
    public virtual GameObject ImgPrefab { get; set; }
    public virtual Transform parent { get; set; }
    public virtual Camera targetCamera { get; set; }
    public virtual Canvas MainCanva { get; set; }
    public virtual int COUNT => 0;
    public virtual GameObject[] gameObjects { get; set; }
    public virtual BaseAnim[] components { get; set; }

    public virtual Type AnimType { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject InstantiateImgPrefab()
    {
        var result = Instantiate(ImgPrefab);
        result.transform.SetParent(parent);
        result.GetComponent<FoldInAnim>().targetCamera = targetCamera;
        result.GetComponent<SlideInAnim>().MainCanvas = MainCanvas;
        return result;
    }

    public bool AllCompleted()
    {
        for (int i = 0; i < COUNT; i++)
        {
            if (!components[i].animFinished)
            {
                return false;
            }
        }
        return true;
    }

    public void SetAnim<T>() where T : BaseAnim
    {
        AnimType = typeof(T);
        for (int i = 0; i < COUNT; i++)
        {
            components[i] = gameObjects[i].GetComponent(AnimType) as BaseAnim;
        }
    }
}
