using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private Level _level;

    private string _prevTileId = "";
    private int _coins;
    private int _stars;
    private UIManager _uiManager;
    private List<BoardTile> _linkedMonsters = new List<BoardTile>();
    private bool _useHummer;
    public Board Board { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        if (_uiManager == null)
            _uiManager = FindObjectOfType<UIManager>();
        if (Board == null)
            Board = FindObjectOfType<Board>();

        _level = LevelsManager.instance.CurrentLevel;
        Board.Init(_level);
        Camera.main.backgroundColor = _level.bgColor;
        _uiManager.StartGame(_level.numberOfMoves, _level.bgColor);
        _uiManager.onGameOver.AddListener(OnGameOver);
        SoundManager.instance.PlayMusic(_level.gameBackgroundMusic, true);
    }
    public void UseHummer()
    {
        if (_useHummer)
            return;

        _useHummer = true;
    }
    public void OnTilePressed(BoardTile headtile)
    {
        if (_useHummer)
        {
            // when play hummer as if we reallocated 1 tile
            Board.numberOfReallocatingTiles = 1;
            headtile.monster.PlayAnimation((int)_level.exploadAnimation);
            headtile.Expload(_level.exploadSound);
            _useHummer = false;
        }
        else
        {
            _linkedMonsters.Clear();
            headtile.PrevTileId = "";
            _linkedMonsters.Add(headtile);
            headtile.PlayAnimation((int)_level.rightSelectedAnimation);
            SoundManager.instance.PlaySound(_level.rightSelectedSound);
        }
    }
    public void OnTileReleased()
    {
        if (_linkedMonsters.Count == 0)
            return;
        if (_linkedMonsters.Count >= 3)
            _uiManager.OnMove();

        // to know how many tiles reallocated (to know when all reallocation finished)
        Board.numberOfReallocatingTiles = _linkedMonsters.Count;
        foreach (BoardTile tile in _linkedMonsters)
        {
            if (_linkedMonsters.Count >= 3)
            {
                tile.monster.PlayAnimation((int)_level.exploadAnimation);
                tile.Expload(_level.exploadSound);
                _coins += _level.tileScore;
                _stars += _level.tileStars;
                _uiManager.Score = _coins;
                _uiManager.Stars = _stars + "/" + _level.targetStars;
            }
            else
                tile.monster.PlayAnimation((int)_level.idleAnimation);
        }

        _linkedMonsters.Clear();
    }
    public void OnTileEntered(BoardTile tile)
    {
        if (_linkedMonsters.Count == 0)
            return;

        if (_linkedMonsters.Contains(tile))
        {
            // when going back from tile in the link to its previous tile in the link (we should remove it)
            if (tile.NextTileId == _linkedMonsters[_linkedMonsters.Count - 1].id && _linkedMonsters[_linkedMonsters.Count - 1].id == _prevTileId)
            {
                _linkedMonsters[_linkedMonsters.Count - 1].PlayAnimation((int)_level.idleAnimation);
                _linkedMonsters.Remove(_linkedMonsters[_linkedMonsters.Count - 1]);
            }
            return;
        }

        // if the entered tile id neighbour to the previous tile in head and has the same color (we should add it)
        if (tile.TileColor == _linkedMonsters[_linkedMonsters.Count - 1].TileColor && _linkedMonsters[_linkedMonsters.Count - 1].IsNeighbour(tile.id))
        {
            _linkedMonsters[_linkedMonsters.Count - 1].NextTileId = tile.id;
            tile.PrevTileId = _linkedMonsters[_linkedMonsters.Count - 1].id;
            _linkedMonsters.Add(tile);
            tile.PlayAnimation((int)_level.rightSelectedAnimation);
            SoundManager.instance.PlaySound(_level.rightSelectedSound);
        }
        else
        {
            tile.PlayAnimation((int)_level.wrongSelectedAnimation);
            SoundManager.instance.PlaySound(_level.wrongSelectedSound);
        }
    }
    public void OnTileExit(BoardTile tile)
    {
        if (_linkedMonsters.Count == 0)
            return;

        // keep track of the previous tile for the unlinking check
        _prevTileId = tile.id;

        if (!_linkedMonsters.Contains(tile))
            tile.monster.PlayAnimation((int)_level.idleAnimation);
    }
    private void OnGameOver()
    {
        SoundManager.instance.PlayMusic(_level.winSound,true);
    }
}
