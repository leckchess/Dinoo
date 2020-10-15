#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class LevelCreator : ScriptableWizard
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
    public GameObject[] Monsters;
    public Animations idleAnimation = Animations.Idle;
    public Animations startAnimation = Animations.Hello;
    public Animations rightSelectedAnimation = Animations.Yes;
    public Animations wrongSelectedAnimation = Animations.No;
    public Animations exploadAnimation = Animations.DieFront;
    public Animations winAnimation = Animations.Happy;
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
        ScriptableWizard.DisplayWizard<LevelCreator>("Create Level", "Close", "Create");
    }

    void OnWizardCreate()
    {
        
    }
    void OnWizardOtherButton()
    {
        if (AssetsAlreadyExists())
        {
            int option = EditorUtility.DisplayDialogComplex("Level Already Exists", "Level" + LevelID + " already exists", "Load", "Replace", "Cancel");
            switch (option)
            {
                case 0:
                    LoadLevel();
                    break;
                case 1:
                    CreateLevel();
                    break;
                case 2:
                    break;
            }
        }
        else
            CreateLevel();
    }

    void OnWizardUpdate()
    {
        isValid = CheckIfValidTooCreateLevel();
        helpString = "Please setup level's data";
    }

    bool AssetsAlreadyExists()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Levels/", "*.asset");
        foreach (string s in files)
        {
            if (s.Contains("Level" + LevelID + ".asset"))
                return true;
        }

        return false;
    }

    void CreateLevel()
    {
        Level level = new Level();
        level.bgColor = BackgroundColor;
        level.tileScore = TileScore;
        level.tileStars = TileStars;

        level.width = Width;
        level.height = Height;
        level.columnsNumber = ColumnsNumber;
        level.rowsNumber = RowsNumber;
        level.offset = BoardOffset;

        level.monsters = new List<GameObject>();
        foreach (GameObject m in Monsters)
            level.monsters.Add(m);

        level.idleAnimation = idleAnimation;
        level.startAnimation = startAnimation;
        level.rightSelectedAnimation = rightSelectedAnimation;
        level.wrongSelectedAnimation = wrongSelectedAnimation;
        level.exploadAnimation = exploadAnimation;
        level.winAnimation = winAnimation;

        level.numberOfMoves = numberOfMoves;
        level.targetStars = targetStars;

        level.gameBackgroundMusic = gameBackgroundMusic;

        level.startSound = startSound;
        level.rightSelectedSound = rightSelectedSound;
        level.wrongSelectedSound = wrongSelectedSound;
        level.exploadSound = exploadSound;
        level.winSound = winSound;


        AssetDatabase.CreateAsset(level, "Assets/Resources/Levels/Level" + LevelID + ".asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = level;
    }
    void LoadLevel()
    {
        Level level = Resources.Load<Level>("Levels/Level" + LevelID);
        BackgroundColor = level.bgColor;
        TileScore = level.tileScore;
        TileStars = level.tileStars;

        Width = level.width;
        Height = level.height;
        ColumnsNumber = level.columnsNumber;
        RowsNumber = level.rowsNumber;
        BoardOffset = level.offset;

        Monsters = new GameObject[level.monsters.Count];
        for (int i = 0; i < level.monsters.Count; i++)
            Monsters[i] = level.monsters[i];

        idleAnimation = level.idleAnimation;
        startAnimation = level.startAnimation;
        rightSelectedAnimation = level.rightSelectedAnimation;
        wrongSelectedAnimation = level.wrongSelectedAnimation;
        exploadAnimation = level.exploadAnimation;
        winAnimation = level.winAnimation;

        numberOfMoves = level.numberOfMoves;
        targetStars = level.targetStars;

        gameBackgroundMusic = level.gameBackgroundMusic;

        startSound = level.startSound;
        rightSelectedSound = level.rightSelectedSound;
        wrongSelectedSound = level.wrongSelectedSound;
        exploadSound = level.exploadSound;
        winSound = level.winSound;
    }
    bool CheckIfValidTooCreateLevel()
    {
        if (TileScore == 0 ||
            TileStars == 0 ||
            Width == 0 ||
            Height == 0 ||
            ColumnsNumber == 0 ||
            RowsNumber == 0 ||
            numberOfMoves == 0 ||
            targetStars == 0 ||
            (Monsters.Length == 0 || Monsters[0] == null))
            return false;

        return true;
    }
}
#endif
