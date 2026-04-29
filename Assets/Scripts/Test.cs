using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Test : MonoBehaviour
{
    public Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material.SetFloat("_BlurSize", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
