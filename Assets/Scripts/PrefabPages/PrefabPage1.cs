using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage1 : PrefabPageCommon
{
    public override int COUNT => 1;

    float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObjects = new GameObject[COUNT];
        components = new BaseAnim[COUNT];

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
    void Update()
    {
        timer += Time.deltaTime;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            var comp = components[i];
            if (!comp.startAnimation && timer >= 0.2f * i)
            {
                comp.startAnimation = true;
            }
        }
    }
}
