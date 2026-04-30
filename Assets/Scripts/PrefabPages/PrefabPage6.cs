using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage6 : PrefabPageCommon
{
    public override int COUNT => 6;

    //float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        var canvasSize = MainCanvas.GetComponent<RectTransform>().sizeDelta;
        
        for (int i = 0; i < 6; i++)
        {
            ConfigImgPrefab(gameObjects[i],

                new(
                canvasSize.x / 3,
                canvasSize.y / 2),

                new(
                canvasSize.x / 6 * (i % 3 * 2 + 1) -
                canvasSize.x / 2,
                -(canvasSize.y / 4 * (i / 3 * 2 + 1) -
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
