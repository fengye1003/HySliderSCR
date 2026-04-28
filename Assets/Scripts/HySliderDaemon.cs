using UnityEngine;
using UnityEngine.UI;

public class HySliderDaemon : MonoBehaviour
{
    public Canvas MainCanvas;
    int Width = Screen.width;
    int Height= Screen.height;
    public GameObject ImgPrefab;
    public GameObject ScreenPanel;
    public new Camera camera;
    GameObject[] gameObjects = new GameObject[5];

    float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            gameObjects[i] = Instantiate(ImgPrefab);
            gameObjects[i].transform.SetParent(ScreenPanel.transform);
            gameObjects[i].GetComponent<FoldInAnim>().targetCamera = camera;
            gameObjects[i].GetComponent<SlideInAnim>().MainCanvas = MainCanvas;
        }
        for (int i = 0; i < 4; i++)
        {
            var rect = gameObjects[i].GetComponent<RectTransform>();
            gameObjects[i].GetComponent<RectTransform>().sizeDelta = new(
                MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 3, 
                MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 2);
            gameObjects[i].transform.localPosition = new(
                MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 6 * ((i % 2) * 2 + 1) - 
                MainCanvas.GetComponent<RectTransform>().sizeDelta.x / 2,
                MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 4 * ((i / 2) * 2 + 1) -
                MainCanvas.GetComponent<RectTransform>().sizeDelta.y / 2, 
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
            if (!gameObjects[i].GetComponent<FlashInAnim>().startAnimation && timer >= 0.2f * i)
            {
                gameObjects[i].GetComponent<FlashInAnim>().startAnimation = true;
            }
        }
    }
}
