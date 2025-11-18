using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public enum GameScreenMode
{
    Fullscreen=0,
    Windowed=1
}

public class SettingData : MonoBehaviour
{
    public static SettingData Instance;
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public float BGMVolume = 0.8f;
    public float SFXVolume = 0.8f;
    public GameScreenMode ScreenMode = GameScreenMode.Fullscreen;
    public int QualityLevel = 2;
    public float MouseSensitivity = 0.5f;  //cameracontroller의 sensitivity와 매칭

    public void ResetToDefault()
    {
        BGMVolume = 0.8f;
        SFXVolume = 0.8f;
        ScreenMode = GameScreenMode.Fullscreen;
        QualityLevel = 2;
        MouseSensitivity = 0.5f;
    }

    #region Set
    public void SetScreenMode(GameScreenMode mode)
    {
        if (mode == GameScreenMode.Fullscreen)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void SetQuality(int level)
    {
        QualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(QualityLevel);
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        MouseSensitivity = Mathf.Clamp01(sensitivity);
    }

    // set bgm ? set sfx? soundmanager에서만 관리할지 

    #endregion

    #region get
    public float GetMouseSensitivity() => MouseSensitivity;
    #endregion


    // Save & Load는 SettingManager에서 처리?
}