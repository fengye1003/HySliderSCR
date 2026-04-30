using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage12 : PrefabPageCommon
{
    public override int COUNT => 6;

    //float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        var canvasSize = MainCanvas.GetComponent<RectTransform>().sizeDelta;
        for (int i = 0; i < 3; i++)
        {
            ConfigImgPrefab(gameObjects[i],

                new(
                canvasSize.x / 4,
                canvasSize.y / 3),

                new(
                canvasSize.x / 8 * (i * 2 + 1) -
                canvasSize.x / 2,
                -(canvasSize.y / 6 -
                canvasSize.y / 2))
                );
        }

        ConfigImgPrefab(gameObjects[3],

                new(
                canvasSize.x / 4 * 3,
                canvasSize.y / 3 * 2),

                new(
                canvasSize.x / 8 * 3 -
                canvasSize.x / 2,
                -(canvasSize.y / 6 * 4 -
                canvasSize.y / 2))
                );

        ConfigImgPrefab(gameObjects[4],

                new(
                canvasSize.x / 4,
                canvasSize.y / 2),

                new(
                canvasSize.x / 8 * (3 * 2 + 1) -
                canvasSize.x / 2,
                -(canvasSize.y / 4 -
                canvasSize.y / 2))
                );

        ConfigImgPrefab(gameObjects[5],

                new(
                canvasSize.x / 4 * 1,
                canvasSize.y / 2),

                new(
                canvasSize.x / 8 * 7 -
                canvasSize.x / 2,
                -(canvasSize.y / 4 * 3 -
                canvasSize.y / 2))
                );
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    
}
