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
        var canvasSize = MainCanvas.GetComponent<RectTransform>().sizeDelta;

        ConfigImgPrefab(gameObjects[0],

                new(
                canvasSize.x / 1,
                canvasSize.y / 1),

                new(
                0,
                0)
                );

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
