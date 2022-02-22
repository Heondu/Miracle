using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject quitPanel;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private bool isOpen = false;
    private float currentBGMVolume;
    private float currentSFXVolume;

    private void Start()
    {
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();
        bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen)
                OpenMenu();
            else
                CloseMenu();
        }
    }

    private void UpdateBGMVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    private void UpdateSFXVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    public void OpenMenu()
    {
        isOpen = true;
        UIManager.IsUIControl = true;
        menuPanel.SetActive(true);
        settingPanel.SetActive(false);
        quitPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        isOpen = false;
        UIManager.IsUIControl = false;
        menuPanel.SetActive(false);
        settingPanel.SetActive(false);
        quitPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        menuPanel.SetActive(false);
        settingPanel.SetActive(true);
        quitPanel.SetActive(false);
        currentBGMVolume = SoundManager.Instance.GetBGMVolume();
        currentSFXVolume = SoundManager.Instance.GetSFXVolume();
    }

    public void ApplySettings()
    {
        UpdateBGMVolume(bgmSlider.value);
        UpdateSFXVolume(sfxSlider.value);
        OpenMenu();
    }

    public void CancelSettings()
    {
        UpdateBGMVolume(currentBGMVolume);
        UpdateSFXVolume(currentSFXVolume);
        OpenMenu();
    }

    public void OpenQuit()
    {
        menuPanel.SetActive(false);
        settingPanel.SetActive(false);
        quitPanel.SetActive(true);
    }

    public void QuitYes()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void QuitNo()
    {
        OpenMenu();
    }
}
