using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private Level _level;

    public List<BoardTile> linkedMonsters = new List<BoardTile>();
    public Board Board { get; private set; }
    string _prevTileId = "";


    #region testing
    private void Start()
    {
        StartGame();
    }
    #endregion

    private void Awake()
    {
        instance = this;
    }
    void StartGame()
    {
        if (Board == null)
            Board = FindObjectOfType<Board>();
        Board.Init(_level);
    }

    public void OnTilePressed(BoardTile headtile, Action<int> callback)
    {
        linkedMonsters.Clear();
        headtile.PrevTileId = "";
        linkedMonsters.Add(headtile);
        callback.Invoke((int)_level.rightSelectedAnimation);
    }
    public void OnTileReleased()
    {
        if (linkedMonsters.Count == 0)
            return;

        Board.numberOfReallocatingTiles = linkedMonsters.Count;
        foreach (BoardTile tile in linkedMonsters)
        {
            if (linkedMonsters.Count >= 3)
            {
                tile.monster.PlayAnimation((int)_level.exploadAnimation);
                tile.ReInitTile();
            }
            else
                tile.monster.PlayAnimation((int)_level.idleAnimation);
        }

        linkedMonsters.Clear(); 
    }
    public void OnTileEntered(BoardTile tile, Action<int> callback)
    {
        if (linkedMonsters.Count == 0)
            return;

        if (linkedMonsters.Contains(tile))
        {
            if (tile.NextTileId == linkedMonsters[linkedMonsters.Count - 1].id && linkedMonsters[linkedMonsters.Count - 1].id == _prevTileId)
            {
                linkedMonsters.Remove(linkedMonsters[linkedMonsters.Count - 1]);
                callback.Invoke((int)_level.idleAnimation);
            }
            return;
        }

        if (tile.TileColor == linkedMonsters[linkedMonsters.Count - 1].TileColor && linkedMonsters[linkedMonsters.Count - 1].IsNeighbour(tile.id))
        {
            linkedMonsters[linkedMonsters.Count - 1].NextTileId = tile.id;
            tile.PrevTileId = linkedMonsters[linkedMonsters.Count - 1].id;
            linkedMonsters.Add(tile);
            callback.Invoke((int)_level.rightSelectedAnimation);
        }
        else
        {
            callback.Invoke((int)_level.wrongSelectedAnimation);
        }
    }
    public void OnTileExit(BoardTile tile, Action<int> callback)
    {
        if (linkedMonsters.Count == 0)
            return;

        _prevTileId = tile.id;

        if (!linkedMonsters.Contains(tile))
            callback.Invoke((int)_level.idleAnimation);

    }

}
