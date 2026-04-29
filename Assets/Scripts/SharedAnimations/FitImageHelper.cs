using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FitImageHelper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void FitImage(
        RectTransform panel, 
        Image img,
        RectTransform imageRect)
    {
        float panelW = panel.rect.width;
        float panelH = panel.rect.height;

        // 用 sprite 原始尺寸，避免被 layout 污染
        float imgW = img.sprite.rect.width;
        float imgH = img.sprite.rect.height;

        float scale = Mathf.Max(panelW / imgW, panelH / imgH);

        float targetW = imgW * scale;
        float targetH = imgH * scale;

        imageRect.sizeDelta = new Vector2(targetW, targetH);
    }
}
