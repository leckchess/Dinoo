using UnityEngine;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _level;
    [SerializeField]
    private Transform _levelsParent;

    private void Start()
    {
        LoadLevels();
    }

    private void LoadLevels()
    {
        LevelsManager.instance.LoadLevels(()=> {
            foreach (Level level in LevelsManager.instance.levels)
            {
                GameObject levelobj = Instantiate(_level);
                levelobj.transform.SetParent(_levelsParent);
                levelobj.GetComponent<LevelItem>().Init(level);
            }
        });
    }
}
