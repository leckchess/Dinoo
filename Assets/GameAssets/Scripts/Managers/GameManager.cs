using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private Level _level;

    private string _prevTileId = "";
    private int _coins;
    private UIManager _uiManager;
    private List<BoardTile> _linkedMonsters = new List<BoardTile>();
    private bool _useHummer;
    public Board Board { get; private set; }

    #region testing
    private void Start()
    {
        StartGame(_level);
    }
    #endregion

    private void Awake()
    {
        instance = this;
    }
    void StartGame(Level level)
    {
        if (_uiManager == null)
            _uiManager = FindObjectOfType<UIManager>();
        if (Board == null)
            Board = FindObjectOfType<Board>();
        Board.Init(_level);
        Camera.main.backgroundColor = _level.bgColor;
        _uiManager.StartGame(_level.numberOfMoves, _level.bgColor);
    }
    public void UseHummer()
    {
        _useHummer = !_useHummer;
    }
    public void OnTilePressed(BoardTile headtile, Action<int> callback)
    {
        if (_useHummer)
        {
            headtile.ReInitTile();
        }
        else
        {
            _linkedMonsters.Clear();
            headtile.PrevTileId = "";
            _linkedMonsters.Add(headtile);
            callback.Invoke((int)_level.rightSelectedAnimation);
        }
    }
    public void OnTileReleased()
    {
        if (_linkedMonsters.Count == 0)
            return;
        if (_linkedMonsters.Count >= 3)
            _uiManager.OnMove();

        Board.numberOfReallocatingTiles = _linkedMonsters.Count;
        foreach (BoardTile tile in _linkedMonsters)
        {
            if (_linkedMonsters.Count >= 3)
            {
                tile.monster.PlayAnimation((int)_level.exploadAnimation);
                tile.ReInitTile();
                _coins += _level.tileScore;
                _uiManager.Score = _coins;
            }
            else
                tile.monster.PlayAnimation((int)_level.idleAnimation);
        }

        _linkedMonsters.Clear();
    }
    public void OnTileEntered(BoardTile tile, Action<int> callback)
    {
        if (_linkedMonsters.Count == 0)
            return;

        if (_linkedMonsters.Contains(tile))
        {
            if (tile.NextTileId == _linkedMonsters[_linkedMonsters.Count - 1].id && _linkedMonsters[_linkedMonsters.Count - 1].id == _prevTileId)
            {
                _linkedMonsters.Remove(_linkedMonsters[_linkedMonsters.Count - 1]);
                callback.Invoke((int)_level.idleAnimation);
            }
            return;
        }

        if (tile.TileColor == _linkedMonsters[_linkedMonsters.Count - 1].TileColor && _linkedMonsters[_linkedMonsters.Count - 1].IsNeighbour(tile.id))
        {
            _linkedMonsters[_linkedMonsters.Count - 1].NextTileId = tile.id;
            tile.PrevTileId = _linkedMonsters[_linkedMonsters.Count - 1].id;
            _linkedMonsters.Add(tile);
            callback.Invoke((int)_level.rightSelectedAnimation);
        }
        else
        {
            callback.Invoke((int)_level.wrongSelectedAnimation);
        }
    }
    public void OnTileExit(BoardTile tile, Action<int> callback)
    {
        if (_linkedMonsters.Count == 0)
            return;

        _prevTileId = tile.id;

        if (!_linkedMonsters.Contains(tile))
            callback.Invoke((int)_level.idleAnimation);

    }

}
