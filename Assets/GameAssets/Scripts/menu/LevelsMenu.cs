using UnityEngine;
/// <summary>
/// Creats Levels Items Based on Created Levels in Resources
/// </summary>
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
                levelobj.transform.localScale = Vector3.one;
                levelobj.GetComponent<LevelUIItem>().Init(level);
            }
        });
    }
}
