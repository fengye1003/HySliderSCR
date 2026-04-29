using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PrefabPage2 : PrefabPageCommon
{
    public override int COUNT => 5;

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
        }
        for (int i = 0; i < 4; i++)
        {
            var rect = gameObjects[i].GetComponent<RectTransform>();
            rect.sizeDelta = new(
                MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 3,
                MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 2);
            gameObjects[i].transform.localPosition = new(
                MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 6 * ((i % 2) * 2 + 1) -
                MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 2,
                -(MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 4 * ((i / 2) * 2 + 1) -
                MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 2),
                0);
        }
        gameObjects[4].GetComponent<RectTransform>().sizeDelta = new(
            MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 3,
            MainCanvas.GetComponent<RectTransform>().sizeDelta.y);
        gameObjects[4].transform.localPosition = new(
            MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 6 * 2,
            0,
            0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (!gameObjects[i].GetComponent<SlideInAnim>().startAnimation && timer >= 0.2f * i)
            {
                gameObjects[i].GetComponent<SlideInAnim>().startAnimation = true;
            }
        }
    }
    
}
