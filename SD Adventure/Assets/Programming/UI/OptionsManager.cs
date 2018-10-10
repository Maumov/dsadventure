﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : GenericMenu
{
    [Header("UI components")]
    public Toggle BgmToggle;
    public Toggle VfxToggle;
    public Toggle VoiceToggle;

    public Toggle[] FontOptions;
    public Toggle[] ColorOptions;

    [Header("Settings Options")]
    public AudioMixer Mixer;
    public TextColor[] ColorSetting;
    public Font[] Fonts;

    OptionsSave config;
    public static event UnityAction<Font, TextColor> TextChange;
    static OptionsManager instance;
    public static void ManualUpdate(out Font f, out TextColor c)
    {
        f = instance.Fonts[instance.config.FontId];
        c = instance.ColorSetting[instance.config.ColorId];
    }

    void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Load();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        instance = this;
        Hide();
        Mixer.SetFloat("BGMVol", config.BgmState ? 0 : -80);
        Mixer.SetFloat("VFXVol", config.VfxState ? 0 : -80);
        Mixer.SetFloat("VoiceVol", config.VoicesState ? 0 : -80);
    }

    public override void Show()
    {
        BgmToggle.isOn = config.BgmState;
        VfxToggle.isOn = config.VfxState;
        VoiceToggle.isOn = config.VoicesState;

        for(int i = 0; i < FontOptions.Length; i++)
            FontOptions[i].isOn = false;
        FontOptions[config.FontId].isOn = true;

        for(int i = 0; i < ColorOptions.Length; i++)
            ColorOptions[i].isOn = false;
        ColorOptions[config.ColorId].isOn = true;

        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }


    public void SetBgm(bool state)
    {
        config.BgmState = state;
        Mixer.SetFloat("BGMVol", state ? 0 : -80);
        Save();
    }

    public void SetVfx(bool state)
    {
        config.VfxState = state;
        Mixer.SetFloat("VFXVol", state ? 0 : -80);
        Save();
    }

    public void SetVoices(bool state)
    {
        config.VoicesState = state;
        Mixer.SetFloat("VoiceVol", state ? 0 : -80);
        Save();
    }

    public void SetFont(Toggle t)
    {
        if(!t.isOn)
            return;

        config.FontId = t.transform.GetSiblingIndex() - 1;

        if(TextChange != null)
            TextChange(Fonts[config.FontId], ColorSetting[config.ColorId]);

        Save();
    }

    public void SetColor(Toggle t)
    {
        if(!t.isOn)
            return;

        config.ColorId = t.transform.GetSiblingIndex() - 1;

        if(TextChange != null)
            TextChange(Fonts[config.FontId], ColorSetting[config.ColorId]);

        Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(TextChange != null)
            TextChange(Fonts[config.FontId], ColorSetting[config.ColorId]);
    }

    void Save()
    {
        PlayerPrefs.SetString("OptionSave", JsonUtility.ToJson(config));
    }

    void Load()
    {
        config = JsonUtility.FromJson<OptionsSave>(PlayerPrefs.GetString("OptionSave"));
        if(config == null)
            config = new OptionsSave();

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    [System.Serializable]
    public struct TextColor
    {
        public Color BackColor;
        public Color FontColor;
    }
}

[System.Serializable]
public class OptionsSave
{
    public bool BgmState;
    public bool VfxState;
    public bool VoicesState;
    public int FontId;
    public int ColorId;

    public OptionsSave()
    {
        BgmState = true;
        VfxState = true;
        VoicesState = true;
        FontId = 0;
        ColorId = 0;
    }
}