using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage3 : PrefabPageCommon
{
    public override int COUNT => 5;

    //float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        var canvasSize = MainCanvas.GetComponent<RectTransform>().sizeDelta;
        ConfigImgPrefab(gameObjects[0],

                new(
                canvasSize.x / 3,
                canvasSize.y),

                new(
                -canvasSize.x / 6 * 2,
                0)
                );

        for (int i = 1; i < 5; i++)
        {
            ConfigImgPrefab(gameObjects[i],

                new(
                canvasSize.x / 3,
                canvasSize.y / 2),

                new(
                canvasSize.x / 6 * ((i % 2) * 2 + 3) -
                canvasSize.x / 2,
                -(canvasSize.y / 4 * ((i / 2) * 2 + 1) -
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
