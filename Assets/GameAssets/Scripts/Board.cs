using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    #region Serializable Fields
    [SerializeField]
    private GameObject _tilePrefab;
    #endregion

    #region Private Fields
    private Level _level;
    private Dictionary<string, BoardTile> tiles;
    private Vector2 _tilesize;
    private Vector3 _boardoffset = Vector3.zero;
    public UnityEvent onGenerationDone = new UnityEvent();
    public BoardTile hintTile;
    private BoardTile searchtile;
    #endregion

    public void Init(Level level)
    {
        _level = level;
        _tilesize = new Vector2((float)_level.width / _level.columnsNumber, (float)_level.height / _level.rowsNumber);
        _boardoffset.x = -Mathf.Ceil((_level.columnsNumber / 2.0f) * _tilesize.x) + Mathf.Ceil((_tilesize.x * 0.5f) / _level.columnsNumber);
        _boardoffset.y = -Mathf.Ceil((_level.rowsNumber / 2.0f) * _tilesize.y) + Mathf.Ceil((_tilesize.y * 0.5f) / _level.rowsNumber);
        tiles = new Dictionary<string, BoardTile>();
        GenerateTiles();
    }
    private void GenerateTiles()
    {
        float startX = _boardoffset.x;
        float deltay = _tilesize.y, deltax = _tilesize.x;
        Vector2 tileoffset = _boardoffset;

        for (int i = 0; i < _level.rowsNumber; i++)
        {
            for (int j = 0; j < _level.columnsNumber; j++)
            {
                BoardTile tile = Instantiate(_tilePrefab, Random.insideUnitSphere * 50, Quaternion.identity).GetComponent<BoardTile>();
                tile.SetTransform(transform, _tilesize, tileoffset);
                string id = i.ToString() + j.ToString();
                tile.Init(_level.GetRandomMonster(), id, (int)_level.idleAnimation);
                tiles.Add(id, tile);
                tileoffset.x += deltax;
            }
            tileoffset.y += deltay;
            tileoffset.x = startX;
        }

        onGenerationDone.Invoke();
        searchtile = tiles["00"];
        if (!CheckForAvailableMoves(searchtile, 0))
            RandomizeTiles();
    }

    public string GetTile(int row, int col)
    {
        if (row < 0 || col < 0 || row >= _level.rowsNumber || col >= _level.columnsNumber)
            return null;


        return tiles[row.ToString() + col.ToString()].id;
    }

    public void OnTileReinit(BoardTile tile)
    {
        List<BoardTile> samecoltiles = GetReallocatingTiles(tile.Row, tile.Column);
        int row;
        string tileid = tile.id;
        foreach (BoardTile t in samecoltiles)
        {
            if (t.id == tileid)
            {
                row = _level.rowsNumber - 1;
                t.transform.position = Random.insideUnitSphere * 50;
                t.Init(_level.GetRandomMonster(), row.ToString() + t.Column.ToString(), (int)_level.idleAnimation);
                tiles[t.id] = t;
            }
            else
            {
                row = t.Row - 1;
                t.Init(row.ToString() + t.Column.ToString(), (int)_level.idleAnimation);
                tiles[t.id] = t;
            }

            Vector3 offset = _boardoffset + new Vector3(t.Column * _tilesize.x, row * _tilesize.y);
            t.SetTransform(transform, _tilesize, offset);

        }
        onGenerationDone.Invoke();

        searchtile = tiles["00"];
        if (!CheckForAvailableMoves(searchtile, 1))
            RandomizeTiles();
    }

    private List<BoardTile> GetReallocatingTiles(int row, int col)
    {
        List<BoardTile> reallocatingtiles = new List<BoardTile>();
        for (int i = row; i < _level.rowsNumber; i++)
        {
            reallocatingtiles.Add(tiles[i.ToString() + col.ToString()]);
        }
        return reallocatingtiles;
    }

    private bool CheckForAvailableMoves(BoardTile tile,int n, string parenttile = "")
    {
        Color color = tile.TileColor;
        string last = "";
        foreach (string neighbour in tile.neighbours)
        {
            if (tiles[neighbour].TileColor == color && neighbour != parenttile)
            {
                n++; 
                last = neighbour;
            }
        }
        if (n >= 3)
        {
            hintTile = tile;
            StopCoroutine("CheckForAvailableMoves");
            return true;
        }
        else if (n >= 1 && !string.IsNullOrEmpty(last))
            return CheckForAvailableMoves(tiles[last], n, tile.id);
        else if (searchtile.Column + 1 < _level.columnsNumber)
        {
            searchtile = tiles[searchtile.Row.ToString() + (searchtile.Column + 1).ToString()];
            return CheckForAvailableMoves(searchtile, 0);
        }
        else if (searchtile.Row + 1 < _level.rowsNumber)
        {
            searchtile = tiles[(searchtile.Row + 1).ToString() + "0"];
            return CheckForAvailableMoves(searchtile, 0);
        }

        return false;
    }
    private void RandomizeTiles()
    {
        print("rand");
    }
}
