using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage8 : PrefabPageCommon
{
    public override int COUNT => 4;

    //float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        var canvasSize = MainCanvas.GetComponent<RectTransform>().sizeDelta;

        for (int i = 0; i < 2; i++)
        {
            ConfigImgPrefab(gameObjects[i],

                new(
                canvasSize.x / 4,
                canvasSize.y / 2),

                new(
                -(canvasSize.x / 8 * 7 -
                canvasSize.x / 2),
                -(canvasSize.y / 4 * (1 + i * 2) -
                canvasSize.y / 2))
                );
        }

        for (int i = 0; i < 2; i++)
        {
            ConfigImgPrefab(gameObjects[i + 2],

                new(
                canvasSize.x / 8 * 3,
                canvasSize.y),

                new(
                canvasSize.x / 16 * 3 * (i * 2 + 1) +
                canvasSize.x / 4 -
                canvasSize.x / 2,
                0)
                );
        }

        

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    
}
