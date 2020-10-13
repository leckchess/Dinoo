using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _levelNumber;

    private Level _level;
    public void Init(Level level)
    {        
        _levelNumber.text = level.name[level.name.Length - 1].ToString();
        GetComponent<Button>().onClick.AddListener(OnLevelClick);
        _level = level;
    }
    private void OnLevelClick()
    {
        LevelsManager.instance.CurrentLevel = _level;
        SceneManager.LoadScene(2);
    }
}
