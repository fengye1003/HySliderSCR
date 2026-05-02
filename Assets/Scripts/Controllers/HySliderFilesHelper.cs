using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class HySliderFilesHelper : MonoBehaviour
{
    public static string relativeImagesPath = "./Images/";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public static Dictionary<string, Texture2D> FetchRandomImages(int count, List<string> filteredFiles = null)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string ImagesPath = Path.Combine(basePath, relativeImagesPath);
        if (!Directory.Exists(ImagesPath))
        {
            Directory.CreateDirectory(ImagesPath);
        }
        
        List<string> files = new List<string>();
        foreach (var item in Directory.GetFiles(ImagesPath))
        {
            if (item.EndsWith(".png") ||
                item.EndsWith(".PNG") ||
                item.EndsWith(".jpg") ||
                item.EndsWith(".JPG"))
            {
                files.Add(item);
            }
        }
        if (filteredFiles != null) 
        {
            foreach (var item in filteredFiles)
            {
                if (files.Contains(item)) 
                {
                    files.Remove(item);
                }
            }
        }
        if (files.Count < count)
        {
            throw new Exception("No enough items error.");
        }
        files = ConfusionArray(files);
        var resultStrs = files.GetRange(0, count);
        Dictionary<string, Texture2D> result = new();
        foreach (var item in resultStrs)
        {

            //string path = Path.Combine(ImagesPath, item);
            string path = item;
            byte[] fileData = File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            result.Add(item, tex);
        }
        return result;
    }

    /// <summary>
    /// 重排列表（打乱列表）
    /// </summary>
    /// <param name="arr"></param>
    public static List<string> ConfusionArray(List<string> list)
    {
        System.Random random = new System.Random();
        return list.OrderBy(x => random.Next()).ToList();
    }

}
