using System.Runtime.InteropServices;
using UnityEngine;

public class HoldLoginStatusHelper : MonoBehaviour
{
    [DllImport("kernel32.dll")]
    private static extern uint SetThreadExecutionState(uint esFlags);

    private const uint ES_CONTINUOUS = 0x80000000;
    private const uint ES_DISPLAY_REQUIRED = 0x00000002;
    private const uint ES_SYSTEM_REQUIRED = 0x00000001;

    void Start()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        // 防止系统休眠 + 防止关闭显示器
        SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
#endif
    }

}
