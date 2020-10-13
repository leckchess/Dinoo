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
    [SerializeField]
    private Button _infoButtons;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayClicked);
        _soundButtons.onClick.AddListener(OnSoundClicked);
        _musicButtons.onClick.AddListener(OnMusicClicked);
        _exitButtons.onClick.AddListener(OnExitClicked);
        _infoButtons.onClick.AddListener(OnInfoClicked);
    }

    private void OnPlayClicked()
    {
        SceneManager.LoadScene(1);
    }
    private void OnSoundClicked()
    {
    }
    private void OnMusicClicked()
    {
    }
    private void OnExitClicked()
    {
    }
    private void OnInfoClicked()
    {
    }
}
