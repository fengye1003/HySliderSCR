using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage1 : PrefabPageCommon
{
    public override int COUNT => 1;

    //float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start(); 

        for (int i = 0; i < COUNT; i++)
        {
            gameObjects[i] = InstantiateImgPrefab();
            components[i] = gameObjects[i].GetComponent(AnimType) as BaseAnim;
            var rect = gameObjects[i].GetComponent<RectTransform>();
            rect.sizeDelta = new(
                MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 1,
                MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 1);
        }
        gameObjects[0].transform.localPosition = new(0, 0, 0);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
