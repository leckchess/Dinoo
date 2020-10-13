using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class Board : MonoBehaviour
{
    #region Serializable Fields
    [SerializeField]
    private GameObject _tilePrefab;
    #endregion

    #region Public Fields
    public UnityEvent onGenerationDone = new UnityEvent();
    public int numberOfReallocatingTiles;
    #endregion

    #region Private Fields
    private Level _level;
    private Dictionary<string, BoardTile> _tiles;
    private Vector2 _tilesize;
    private Vector3 _boardoffset = Vector3.zero;
    public BoardTile _hintTile;
    private BoardTile _searchtile;
    #endregion

    public void Init(Level level)
    {
        _level = level;
        _tilesize = new Vector2((float)_level.width / _level.columnsNumber, (float)_level.height / _level.rowsNumber);
        _boardoffset.x = -Mathf.Ceil((_level.columnsNumber / 2.0f) * _tilesize.x) + Mathf.Ceil((_tilesize.x * 0.5f) / _level.columnsNumber);
        _boardoffset.y = -Mathf.Ceil((_level.rowsNumber / 2.0f) * _tilesize.y) + Mathf.Ceil((_tilesize.y * 0.5f) / _level.rowsNumber);
        _tiles = new Dictionary<string, BoardTile>();
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
                _tiles.Add(id, tile);
                tileoffset.x += deltax;
            }
            tileoffset.y += deltay;
            tileoffset.x = startX;
        }

        onGenerationDone.Invoke();
        _searchtile = _tiles["00"];
        CheckForAvailableMoves(_searchtile, 1);
    }

    public string GetTile(int row, int col)
    {
        if (row < 0 || col < 0 || row >= _level.rowsNumber || col >= _level.columnsNumber)
            return null;


        return _tiles[row.ToString() + col.ToString()].id;
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
                _tiles[t.id] = t;
            }
            else
            {
                row = t.Row - 1;
                t.Init(row.ToString() + t.Column.ToString(), (int)_level.idleAnimation);
                _tiles[t.id] = t;
            }

            Vector3 offset = _boardoffset + new Vector3(t.Column * _tilesize.x, row * _tilesize.y);
            t.SetTransform(transform, _tilesize, offset);

        }

        numberOfReallocatingTiles--;
        if (numberOfReallocatingTiles == 0)
        {
            onGenerationDone.Invoke();
            _searchtile = _tiles["00"];
            CheckForAvailableMoves(_searchtile, 1);
        }
    }

    private List<BoardTile> GetReallocatingTiles(int row, int col)
    {
        List<BoardTile> reallocatingtiles = new List<BoardTile>();
        for (int i = row; i < _level.rowsNumber; i++)
        {
            reallocatingtiles.Add(_tiles[i.ToString() + col.ToString()]);
        }
        return reallocatingtiles;
    }

    private bool CheckForAvailableMoves(BoardTile tile, int n, string parenttile = "")
    {
        _hintTile = null;
        Color color = tile.TileColor;
        string last = "";
        foreach (string neighbour in tile.neighbours)
        {
            if (_tiles[neighbour].TileColor == color && neighbour != parenttile)
            {
                n++;
                last = neighbour;
            }
        }
        if (n >= 3)
        {
            _hintTile = tile;
            return true;
        }
        else if (n >= 2 && !string.IsNullOrEmpty(last))
        {
            return CheckForAvailableMoves(_tiles[last], n, tile.id);
        }
        else if (_searchtile.Column + 2 < _level.columnsNumber)
        {
            _searchtile = _tiles[_searchtile.Row.ToString() + (_searchtile.Column + 2).ToString()];
            return CheckForAvailableMoves(_searchtile, 1);
        }
        else if (_searchtile.Row + 2 < _level.rowsNumber)
        {
            _searchtile = _tiles[(_searchtile.Row + 2).ToString() + "0"];
            return CheckForAvailableMoves(_searchtile, 1);
        }

        Invoke("RandomizeTiles", 1);
        return false;
    }
    private void RandomizeTiles()
    {
        System.Random rnd = new System.Random();
        int[] rows = Enumerable.Range(0, _level.rowsNumber).OrderBy(x => x = rnd.Next()).ToArray();
        int[] cols = Enumerable.Range(0, _level.columnsNumber).OrderBy(x => x = rnd.Next()).ToArray();

        Monster monster0 = null;
        string monster0Tileid = "";
        for (int i = 0; i < rows.Length; i++)
            for (int j = 0; j < cols.Length; j++)
            {
                string id = rows[i].ToString() + cols[j].ToString();
                Monster monster1 = _tiles[id].monster;
                if (monster0 == null)
                {
                    monster0Tileid = id;
                    monster0 = _tiles[id].monster;
                }
                else
                {
                    _tiles[monster0Tileid].SetMonster(monster1);
                    _tiles[id].SetMonster(monster0);
                    monster0 = null;
                }
            }

        onGenerationDone.Invoke();
        _searchtile = _tiles["00"];
        CheckForAvailableMoves(_searchtile, 0);
    }

    // for testing
    private void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            RandomizeTiles();
            //onGenerationDone.Invoke();
            //_searchtile = _tiles["00"];
            //CheckForAvailableMoves(_searchtile, 0);
        }
    }

}
