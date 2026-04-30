using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage9 : PrefabPageCommon
{
    public override int COUNT => 3;

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
                (canvasSize.x / 3 -
                canvasSize.x / 2),
                0)
                );

        for (int i = 0; i < 2; i++)
        {
            ConfigImgPrefab(gameObjects[i + 1],

                new(
                canvasSize.x / 3,
                canvasSize.y / 2),

                new(
                canvasSize.x / 6 * 5 -
                canvasSize.x / 2,
                -(canvasSize.y / 4 * (1 + i * 2) -
                canvasSize.y / 2))
                );

        }

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    
}
