using UnityEngine;

[System.Serializable]
public class VersionData
{
    public string version;
    public string gitHash;
    public string buildTime;
}

public static class BuildVersion
{
    private static VersionData _data;

    public static string Version => Load().version;
    public static string GitHash => Load().gitHash;
    public static string BuildTime => Load().buildTime;

    static VersionData Load()
    {
        if (_data != null) return _data;

        var text = Resources.Load<TextAsset>("version");
        _data = JsonUtility.FromJson<VersionData>(text.text);
        return _data;
    }
}