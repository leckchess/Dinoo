using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    #region Serializable Fields
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    [SerializeField]
    private int _columnsNumber;
    [SerializeField]
    private int _rowsNumber;
    [SerializeField]
    private GameObject _tilePrefab;
    [SerializeField]
    private int _numberOfMonsters;
    #endregion

    #region Private Fields
    private BoardTile[,] tiles;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Init();
    }
    #endregion

    #region Private Methods
    private void Init()
    {
        tiles = new BoardTile[_rowsNumber, _columnsNumber];
        GenerateMonsters();
    }
    private void GenerateMonsters()
    {
        Vector2 size = new Vector2((float)_width / _columnsNumber, (float)_height / _rowsNumber);
        Vector3 offset = Vector3.zero;
        offset.x = -Mathf.Ceil((_columnsNumber / 2.0f) * size.x) + Mathf.Ceil((size.x * 0.5f) / _columnsNumber);
        offset.y = -Mathf.Ceil((_rowsNumber / 2.0f) * size.y) + Mathf.Ceil((size.y * 0.5f) / _rowsNumber);
        float startX = offset.x;
        float deltay = size.y, deltax = size.x;

        for (int i = 0; i < _rowsNumber; i++)
        {
            for (int j = 0; j < _columnsNumber; j++)
            {
                BoardTile tile = Instantiate(_tilePrefab, new Vector3(i * size.x, j * size.y, 0), Quaternion.identity).GetComponent<BoardTile>();
                tile.SetTransform(transform, size, offset);
                tile.Init(_numberOfMonsters);
                tiles[i, j] = tile;
                offset.x += deltax;
            }
            offset.y += deltay;
            offset.x = startX;
        }
    }
    #endregion
}
