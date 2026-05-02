using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int GetRandomPagePrefabId()
    {
        System.Random r = new();
        List<int> l = new();
        int[] counts = new int[HySliderDaemon.Instance.PagePrefabs.Count];

        for (int i = 0; i < counts.Length; i++)
        {
            if (!HySliderDaemon.PageConfig.ContainsKey($"prefab{i + 1}"))
            {
                counts[i] = 1;
            }
            else
            try
            {
                counts[i] =
                    Convert.ToInt32(
                        (string)HySliderDaemon.
                        PageConfig[$"prefab{i + 1}"]);
            }
            catch
            {
                counts[i] = 1;
            }
        }

        for (int i = 0; i < counts.Length; i++)
        {
            for (int j = 0; j < counts[i]; j++)
            {
                l.Add(i);
            }
        }
        return l[r.Next(l.Count)];
    }

    public static int GetRandomAnimId()
    {
        System.Random r = new();
        List<int> l = new();
        int[] counts = new int[3];



        counts[0] =
            Convert.ToInt32(
                (string)HySliderDaemon.
                AnimConfig["SlideInAnimWeight"]);
        counts[1] =
            Convert.ToInt32(
                (string)HySliderDaemon.
                AnimConfig["FlashInAnimWeight"]);
        counts[2] =
            Convert.ToInt32(
                (string)HySliderDaemon.
                AnimConfig["FoldInAnimWeight"]);



        for (int i = 0; i < counts.Length; i++)
        {
            for (int j = 0; j < counts[i]; j++)
            {
                l.Add(i);
            }
        }

        return l[r.Next(l.Count)];
    }
}
