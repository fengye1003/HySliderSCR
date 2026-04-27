using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

[InitializeOnLoad]
public static class GitVersionWriter
{
    static GitVersionWriter()
    {
        EditorApplication.playModeStateChanged += OnPlay;
    }

    static void OnPlay(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            WriteVersion();
        }
    }

    [MenuItem("Tools/Update Version")]
    public static void WriteVersion()
    {
        string hash = RunGit("rev-parse --short HEAD");
        string count = RunGit("rev-list --count HEAD");

        if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(count))
            return;

        string version = $"1.1.{count}";

        // 写 PlayerSettings（安全）
        PlayerSettings.bundleVersion = version;

        var data = new VersionData
        {
            version = version,
            gitHash = hash,
            buildTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string json = JsonUtility.ToJson(data, true);
        string path = "Assets/Resources/version.json";

        // 防止无意义写入（关键）
        if (File.Exists(path))
        {
            if (File.ReadAllText(path) == json)
                return;
        }

        File.WriteAllText(path, json);

        // 可选：这里 Refresh 是安全的（不会触发编译）
        AssetDatabase.Refresh();
    }

    static string RunGit(string args)
    {
        try
        {
            var p = new Process();
            p.StartInfo.FileName = "git";
            p.StartInfo.Arguments = args;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            return p.StandardOutput.ReadToEnd().Trim();
        }
        catch
        {
            return "";
        }
    }
}