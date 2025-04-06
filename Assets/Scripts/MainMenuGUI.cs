using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Data;

public class MainMenuGUI : MonoBehaviour
{
    public static MainMenuGUI Instance { get; private set; }
    [SerializeField] private Button main_Play;
    [SerializeField] private Button main_Settings;
    [SerializeField] private Button main_Credits;
    [SerializeField] private Button main_Exit;
    [SerializeField] private Button settings_Return;
    [SerializeField] private Slider settings_VolumeSlider;
    [SerializeField] private TMP_Text settings_VolumeValue;
    [SerializeField] private GameObject mainObject;
    [SerializeField] private GameObject settingsObject;
    [SerializeField] private GameObject playMenuObject;
    [SerializeField] private GameObject singleplayerMenuObject;
    private MenuState state = MenuState.Main;
    delegate void StateChangedDelegate();
    event StateChangedDelegate stateChanged;
    private void Start()
    {
        stateChanged += MainStateChanged;
        stateChanged += SettingsStateChanged;
        stateChanged += CreditsStateChanged;
        stateChanged += PlayMenuStateChanged;
        stateChanged += SingleplayerMenuStateChanged;
    }
    private void MainStateChanged()
    {
        if(state == MenuState.Main)
        {
            mainObject.SetActive(true);
        }
        else
        {
            mainObject.SetActive(false);
        }
    }
    private void SettingsStateChanged()
    {
        if (state == MenuState.Settings)
        {
            settingsObject.SetActive(true);
        }
        else
        {
            settingsObject.SetActive(false);
        }
    }
    private void CreditsStateChanged()
    {

    }
    private void PlayMenuStateChanged()
    {
        if (state == MenuState.Play)
        {
            playMenuObject.SetActive(true);
        }
        else
        {
            playMenuObject.SetActive(false);
        }
    }
    private void SingleplayerMenuStateChanged()
    {
        if (state == MenuState.PlaySingle)
        {
            singleplayerMenuObject.SetActive(true);
        }
        else
        {
            singleplayerMenuObject.SetActive(false);
        }
    }
    public void ChangeState(MainMenuStateComponent newState)
    {
        state = newState.state;
        stateChanged?.Invoke();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
