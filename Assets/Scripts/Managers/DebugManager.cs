using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("References")]
    public GameMode debugGameMode;

    [Header("Settings")]
    public bool debugEnabled = true;
    public bool debugGameModeEnabled = true;
    public bool debugLogsEnabled = true;
    public List<string> debugNames;

#if !UNITY_EDITOR
    private void Awake()
    {
        debugEnabled = false;
        debugGameModeEnabled = false;
        debugLogsEnabled = false;
    }
#endif
}
