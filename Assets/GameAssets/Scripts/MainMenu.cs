using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;
    [Header("settings")]
    [SerializeField]
    private Button _soundButtons;
    [SerializeField]
    private Button _musicButtons;
    [SerializeField]
    private Button _exitButtons;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayClicked);
        _soundButtons.onClick.AddListener(OnSoundClicked);
        _musicButtons.onClick.AddListener(OnMusicClicked);
        _exitButtons.onClick.AddListener(OnExitClicked);
    }

    private void OnPlayClicked()
    {
        SceneManager.LoadScene(1);
    }
    private void OnSoundClicked()
    {
        GameObject disabled = _soundButtons.transform.GetChild(0).gameObject;
        disabled.SetActive(!disabled.activeSelf);
        SoundManager.instance.MuteSounds();
    }
    private void OnMusicClicked()
    {
        GameObject disabled = _musicButtons.transform.GetChild(0).gameObject;
        disabled.SetActive(!disabled.activeSelf);
        SoundManager.instance.MuteMusic();
    }
    private void OnExitClicked()
    {
        Application.Quit();
    }
}
