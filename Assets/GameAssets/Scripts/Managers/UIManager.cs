using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image _movesImage;
    [SerializeField]
    private TMP_Text _numberOfMovesText;
    [SerializeField]
    private TMP_Text _coinsText;
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


    private int _numberOfMoves = 50;
    private int _currentNumberOfMoves;

    public int Score{ set { _coinsText.text = value.ToString(); } }
    public void StartGame(int numberofmoves, Color bgcolor)
    {
        _hintsButton.onClick.AddListener(OnHintButtonClick);
        _hummerButton.onClick.AddListener(OnHummerButtonClick);
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
