using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    #region Serializable Fields
    [SerializeField]
    private GameObject _tilePrefab;
    [SerializeField]
    private Level _level;
    #endregion

    #region Private Fields
    private BoardTile[,] tiles;
    #endregion

    public void Init()
    {
        tiles = new BoardTile[_level.rowsNumber, _level.columnsNumber];
        GenerateMonsters();
    }
    private void GenerateMonsters()
    {
        Vector2 size = new Vector2((float)_level.width / _level.columnsNumber, (float)_level.height / _level.rowsNumber);
        Vector3 offset = Vector3.zero;
        offset.x = -Mathf.Ceil((_level.columnsNumber / 2.0f) * size.x) + Mathf.Ceil((size.x * 0.5f) / _level.columnsNumber);
        offset.y = -Mathf.Ceil((_level.rowsNumber / 2.0f) * size.y) + Mathf.Ceil((size.y * 0.5f) / _level.rowsNumber);
        float startX = offset.x;
        float deltay = size.y, deltax = size.x;

        for (int i = 0; i < _level.rowsNumber; i++)
        {
            for (int j = 0; j < _level.columnsNumber; j++)
            {
                BoardTile tile = Instantiate(_tilePrefab, new Vector3(i * size.x, j * size.y, 0), Quaternion.identity).GetComponent<BoardTile>();
                tile.SetTransform(transform, size, offset);
                tile.Init(_level.GetRandomMonster(), i.ToString() + j.ToString());
                tiles[i, j] = tile;
                offset.x += deltax;
            }
            offset.y += deltay;
            offset.x = startX;
        }
    }
}
