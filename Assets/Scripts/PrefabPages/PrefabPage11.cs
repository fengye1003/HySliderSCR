using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage11 : PrefabPageCommon
{
    public override int COUNT => 2;

    //float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        var canvasSize = MainCanvas.GetComponent<RectTransform>().sizeDelta;


        ConfigImgPrefab(gameObjects[0],

                new(
                canvasSize.x / 3 * 2,
                canvasSize.y),

                new(
                canvasSize.x / 3 * 1 -
                canvasSize.x / 2,
                0)
                );

        ConfigImgPrefab(gameObjects[1],

                new(
                canvasSize.x / 3,
                canvasSize.y),

                new(
                (canvasSize.x / 6 * 5 -
                canvasSize.x / 2),
                0)
                );
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    
}
