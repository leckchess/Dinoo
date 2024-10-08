﻿using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
    private bool _showhint;
    #endregion

    public void Init(Level level)
    {
        // do some calculations to center the board (based on its offset) and resizing the tiles
        _level = level;
        _tilesize = new Vector2((float)_level.width / _level.columnsNumber, (float)_level.height / _level.rowsNumber);
        _boardoffset.x = -Mathf.Ceil((_level.columnsNumber / 2.0f) * _tilesize.x) + Mathf.Ceil((_tilesize.x * 0.5f) / _level.columnsNumber) + _level.offset.x;
        _boardoffset.y = -Mathf.Ceil((_level.rowsNumber / 2.0f) * _tilesize.y) + Mathf.Ceil((_tilesize.y * 0.5f) / _level.rowsNumber) + _level.offset.y;
        _tiles = new Dictionary<string, BoardTile>();
        GenerateTiles();
    }

    // to make sure if the tile exists and doesnt exceed the limits
    public string GetTile(int row, int col)
    {
        if (row < 0 || col < 0 || row >= _level.rowsNumber || col >= _level.columnsNumber)
            return null;


        return _tiles[row.ToString() + "-" + col.ToString()].id;
    }
    // when destroy tile to reallocate  (we reinit dont destroy)
    public void OnTileReinit(BoardTile tile)
    {
        // get tiles in the same column and greater rows to reallocate
        List<BoardTile> samecoltiles = GetReallocatingTiles(tile.Row, tile.Column);
        int row;
        string tileid = tile.id;
        foreach (BoardTile t in samecoltiles)
        {
            if (t.id == tileid)
            {
                row = _level.rowsNumber - 1;
                t.transform.position = Random.insideUnitSphere * 50;
                t.Init(_level.GetRandomMonster(), row.ToString() + "-" + t.Column.ToString(), (int)_level.idleAnimation);
                _tiles[t.id] = t;
            }
            else
            {
                row = t.Row - 1;
                t.Init(row.ToString() + "-" + t.Column.ToString(), (int)_level.idleAnimation);
                _tiles[t.id] = t;
            }

            Vector3 offset = _boardoffset + new Vector3(t.Column * _tilesize.x, row * _tilesize.y);
            t.SetTransform(transform, _tilesize, offset);

        }

        // check when all the reallocations are done recalculate neighbours, check for available moves and allocate the next hint
        numberOfReallocatingTiles--;
        if (numberOfReallocatingTiles == 0)
        {
            onGenerationDone.Invoke();
            _searchtile = _tiles["0-0"];
            CheckForAvailableMoves(_searchtile, 1);
        }
    }

    public void SHowHint()
    {
        _showhint = true;
        StartCoroutine(ShowHint());
    }
    private IEnumerator ShowHint()
    {
        if (!_showhint)
            yield break;
        _hintTile?.monster.transform.DOScale(new Vector3(2f, 2f, 2f), 1);
        yield return new WaitForSeconds(0.2f);
        _hintTile?.monster.transform.DOScale(Vector3.one, 1);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShowHint());
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
                string id = i.ToString() + "-" + j.ToString();
                tile.Init(_level.GetRandomMonster(), id, (int)_level.idleAnimation, _level.startSound);
                _tiles.Add(id, tile);
                tileoffset.x += deltax;
            }
            tileoffset.y += deltay;
            tileoffset.x = startX;
        }

        // for the tiles to calculate neighbours 
        onGenerationDone.Invoke();
        _searchtile = _tiles["0-0"];
        CheckForAvailableMoves(_searchtile, 1);
    }
    private List<BoardTile> GetReallocatingTiles(int row, int col)
    {
        _showhint = false;
        List<BoardTile> reallocatingtiles = new List<BoardTile>();
        for (int i = row; i < _level.rowsNumber; i++)
        {
            reallocatingtiles.Add(_tiles[i.ToString() + "-" + col.ToString()]);
        }
        return reallocatingtiles;
    }
    private bool CheckForAvailableMoves(BoardTile tile, int n, string parenttile = "")
    {
        // check neighbours if more than 2 has sae color the true
        // if  has same color check its neighbours if another one has same color then its true
        // else get the next tile in column (neighbours are execluded) and restart the process
        // else there is no match and call randomize (delay to give the player time to see the board before randomization)

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
            _searchtile = _tiles[_searchtile.Row.ToString() + "-" + (_searchtile.Column + 2).ToString()];
            return CheckForAvailableMoves(_searchtile, 1);
        }
        else if (_searchtile.Row + 2 < _level.rowsNumber)
        {
            _searchtile = _tiles[(_searchtile.Row + 2).ToString() + "-0"];
            return CheckForAvailableMoves(_searchtile, 1);
        }

        Invoke("RandomizeTiles", 1);
        return false;
    }
    private void RandomizeTiles()
    {
        // get randomized rows and col
        // reallocate the tiles' monsters based on the randomized numbers (not the tiles itself)

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

        // recalculate neighbours and recheck after randomization
        onGenerationDone.Invoke();
        _searchtile = _tiles["0-0"];
        CheckForAvailableMoves(_searchtile, 0);
    }
}
