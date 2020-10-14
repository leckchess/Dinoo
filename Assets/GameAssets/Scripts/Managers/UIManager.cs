using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Ingame UI")]
    [SerializeField]
    private Image _movesImage;
    [SerializeField]
    private TMP_Text _numberOfMovesText;
    [SerializeField]
    private TMP_Text _ingameCoinsText;
    [SerializeField]
    private TMP_Text _ingameStarsText;
    [SerializeField]
    private Button _hintsButton;
    [SerializeField]
    private TMP_Text _hintsCooldownText;
    [SerializeField]
    private Button _hummerButton;
    [SerializeField]
    private TMP_Text _hummerCooldownText;
    [SerializeField]
    private int _hintCooldownTime;
    [SerializeField]
    private int _hummerCooldownTime;

    [Header("Gameover UI")]
    [SerializeField]
    private CanvasGroup gameoverScreen;
    [SerializeField]
    private TMP_Text _gameoverCoinsText;
    [SerializeField]
    private TMP_Text _gameoverStarsText;
    [SerializeField]
    private GameObject _stars;
    [SerializeField]
    private Button _nextLevelButton;
    [SerializeField]
    private Button _homeButton;
    [SerializeField]
    private Button _levelsButton;


    private int _numberOfMoves = 50;
    private int _currentNumberOfMoves;

    public int Score { set { _ingameCoinsText.text = value.ToString(); } }
    public string Stars { set { _ingameStarsText.text = value; } }
    public void StartGame(int numberofmoves, Color bgcolor)
    {
        _hintsButton.onClick.AddListener(OnHintButtonClick);
        _hummerButton.onClick.AddListener(OnHummerButtonClick);
        _nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
        _homeButton.onClick.AddListener(OnHomeButtonPressed);
        _levelsButton.onClick.AddListener(OnLevelsButtonPressed);
        _numberOfMoves = numberofmoves;
        _numberOfMovesText.text = numberofmoves.ToString();
        _currentNumberOfMoves = numberofmoves;
        _movesImage.color = bgcolor;
        _movesImage.fillAmount = 1;
        _hintsCooldownText.text = "";
        _hummerCooldownText.text = "";
    }
    public void OnMove()
    {
        _movesImage.fillAmount -= (1.0f / _numberOfMoves);
        _currentNumberOfMoves--;
        _numberOfMovesText.text = _currentNumberOfMoves.ToString();
        if (_currentNumberOfMoves == 0)
            GameOver();
    }
    public void GameOver()
    {
        gameoverScreen.DOFade(1, 1);
        gameoverScreen.blocksRaycasts = true;
        gameoverScreen.interactable = true;
        _gameoverCoinsText.text = _ingameCoinsText.text;
        _gameoverStarsText.text = _ingameStarsText.text;
        string[] stars = _ingameStarsText.text.Split('/');
        int starsnumber = Mathf.Clamp((int.Parse(stars[0]) / int.Parse(stars[1])), 0, 3);
        for (int i = 0; i < starsnumber; i++)
            _stars.transform.GetChild(i).DOScale(Vector3.one, 1);
        if (LevelsManager.instance.CurrentLevel.ID == LevelsManager.instance.levels.Length - 1)
            _nextLevelButton.interactable = false;

    }
    private void OnHintButtonClick()
    {
        _hintsButton.interactable = false;
        StartCoroutine(HintCooldown(_hintCooldownTime));
        GameManager.instance.Board.SHowHint();
    }
    private void OnHummerButtonClick()
    {
        _hummerButton.interactable = false;
        StartCoroutine(HummerCooldown(_hummerCooldownTime));
        GameManager.instance.UseHummer();
    }
    private void OnLevelsButtonPressed()
    {
        _levelsButton.interactable = false;
        SceneManager.LoadScene(1);
    }
    private void OnHomeButtonPressed()
    {
        _homeButton.interactable = false;
        SceneManager.LoadScene(0);
    }
    private void OnNextLevelButtonClick()
    {
        int id = LevelsManager.instance.CurrentLevel.ID + 1;
        LevelsManager.instance.CurrentLevel = LevelsManager.instance.levels[id];
        SceneManager.LoadScene(2);
    }
    private IEnumerator HintCooldown(int hintcooldown)
    {
        if (hintcooldown == 0)
        {
            _hintsButton.interactable = true;
            _hintsCooldownText.text = "";
            yield break;
        }

        yield return new WaitForSeconds(1);
        hintcooldown--;
        _hintsCooldownText.text = hintcooldown.ToString();
        StartCoroutine(HintCooldown(hintcooldown));
    }
    private IEnumerator HummerCooldown(int hummercooldown)
    {
        if (hummercooldown == 0)
        {
            _hummerButton.interactable = true;
            _hummerCooldownText.text = "";
            yield break;
        }

        yield return new WaitForSeconds(1);
        hummercooldown--;
        _hummerCooldownText.text = hummercooldown.ToString();
        StartCoroutine(HummerCooldown(hummercooldown));
    }
}
