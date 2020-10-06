using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        Board.Init();
    }

    public void OnTilePressed(BoardTile headtile)
    {
        linkedMonsters.Clear();
        headtile.PrevTileId = "";
        linkedMonsters.Add(headtile);
    }
    public void OnTileReleased(BoardTile tile)
    {
        if (linkedMonsters.Count == 0)
            return;
        // if there is more than 3 n destroy / n kill
        linkedMonsters.Clear();
    }
    public void OnTileEntered(BoardTile tile)
    {
        if (linkedMonsters.Count == 0)
            return;

        if (linkedMonsters.Contains(tile))
        {
            if (tile.NextTileId == linkedMonsters[linkedMonsters.Count - 1].id && linkedMonsters[linkedMonsters.Count - 1].id == _prevTileId)
            {
                linkedMonsters.Remove(linkedMonsters[linkedMonsters.Count - 1]);
            }
            return;
        }

        if (tile.TileColor == linkedMonsters[linkedMonsters.Count - 1].TileColor && linkedMonsters[linkedMonsters.Count - 1].IsNeighbour(tile.id))
        {
            linkedMonsters[linkedMonsters.Count - 1].NextTileId = tile.id;
            tile.PrevTileId = linkedMonsters[linkedMonsters.Count - 1].id;
            linkedMonsters.Add(tile);
            // play yes animation
        }
        else
        {
            //play no animation
        }
    }
    public void OnTileExit(BoardTile tile)
    {
        if (linkedMonsters.Count == 0)
            return;

        _prevTileId = tile.id;
    }

}
