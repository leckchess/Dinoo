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
    private Level _level;
    #endregion

    #region Private Fields
    private Dictionary<string, BoardTile> tiles;
    private Vector2 _tilesize;
    Vector3 _boardoffset = Vector3.zero;
    public UnityEvent onGenerationDone = new UnityEvent();
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
        if (!CheckForAvailableMoves(0, _level.columnsNumber, 0, _level.rowsNumber))
        RandomizeTiles();
    }

    //public BoardTile GetTile(string id)
    //{
    //    return tiles[id];
    //}

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

        //if (!CheckForAvailableMoves())
        //    RandomizeTiles();
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

    private bool CheckForAvailableMoves()// int left, int right, int up, int down)
    {
        //int midx = left + (right - left) / 2;
        //int midy = down + (up - down) / 2;
        //print(midy.ToString() + midx.ToString());

        // if (GetSimilarNeighbours(midy.ToString() + midx.ToString()) >= 2)
        //    return true;

        //return CheckForAvailableMoves(midx + 1, right, up, midy);

        //int count = 0;

        //do
        //{
        //    string id = (_level.rowsNumber / 2).ToString() + (_level.columnsNumber / 2).ToString();
        //    BoardTile tile = tiles[id];
        //    count = tile.GetSimilarNeighbours();

        //} while (count < 2);
        //int n = 0;
        //Color color = Color.white;
        //foreach (BoardTile tile in tiles.Values)
        //{
        //    if (n == 0)
        //    {
        //        color = tile.TileColor;
        //        n++;
        //    }
        //    else
        //    {
        //        if (tile.TileColor == color)
        //        {
        //            n++;
        //            if (n >= 3)
        //                return true;
        //        }
        //        else
        //        {
        //            color = tile.TileColor;
        //            n = 1;
        //        }
        //    }
        //}
    }

    private int GetSimilarNeighbours(string id)
    {
        throw new System.NotImplementedException();
    }

    private void RandomizeTiles()
    {
        print("rand");
    }
}
