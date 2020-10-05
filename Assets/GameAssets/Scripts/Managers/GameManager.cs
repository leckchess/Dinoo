using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<BoardTile> linkedMonsters = new List<BoardTile>();
    private Board _board;

    private void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        if (_board == null)
            _board = FindObjectOfType<Board>();
        _board.Init();
    }

    void OnTilePressed(BoardTile tile)
    {
        // lw awl 7d yb2a al head
    }
    void OnTileReleased(BoardTile tile)
    {
        // if there is more than 3 n destroy / n kill
    }
    void OnTileEntered(BoardTile tile)
    {
        // if same color zi previous play yes else play no
    }
    void OnTileExit(BoardTile tile)
    {
        // play sad aw 7aga kda
        // lw prev == next yb2a clear this else add next lw same color
    }
}
