#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class LevelEditor : ScriptableWizard
{
    public int LevelID;
    [Header("Level")]
    public Color BackgroundColor;
    public int TileScore;
    public int TileStars;
    [Header("Board")]
    public int Width;
    public int Height;
    public int ColumnsNumber;
    public int RowsNumber;
    public Vector2 BoardOffset;
    [Header("Monsters")]
    public List<GameObject> Monsters = new List<GameObject>();
    public Animations idleAnimation = Animations.Idle;
    public Animations startAnimation = Animations.Hello;
    public Animations rightSelectedAnimation = Animations.Yes;
    public Animations wrongSelectedAnimation = Animations.No;
    public Animations exploadAnimation = Animations.DieFront;
    public Animations winAnimation = Animations.Happy;
    public Animations losAnimation = Animations.Sick;
    [Header("Gameplay")]
    public int numberOfMoves;
    public int targetStars;
    [Header("Music")]
    public AudioClip gameBackgroundMusic;
    [Header("Sounds")]
    public AudioClip startSound;
    public AudioClip rightSelectedSound;
    public AudioClip wrongSelectedSound;
    public AudioClip exploadSound;
    public AudioClip winSound;

    [MenuItem("Levels/Create New Level")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<LevelCreator>("Create Level", "Create");
    }

    void OnWizardCreate()
    {
        // create scriptable object here
    }

    void OnWizardUpdate()
    {
        helpString = "Please setup level's data";
    }
}
#endif
