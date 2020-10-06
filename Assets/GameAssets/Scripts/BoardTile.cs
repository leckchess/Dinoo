using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BoardTile : MonoBehaviour
{
    public string id { get; private set; }
    public Monster monster;
    public bool IsChicked { get; set; }
    UnityAction<BoardTile> OnTilePressed;
    UnityAction<BoardTile> OnTileReleased;
    UnityAction<BoardTile> OnTileEntered;
    UnityAction<BoardTile> OnTileExit;

    public string[] neighbours = new string[8];

    public Color TileColor { get { return monster.color; } }

    public string PrevTileId { get; internal set; }
    public string NextTileId { get; internal set; }

    internal void Init(GameObject tilemonster, string tileid)
    {
        id = name = tileid;

        monster = tilemonster.GetComponent<Monster>();
        monster.SetTransform(transform, Vector3.zero, Vector3.one);
        OnTilePressed += GameManager.instance.OnTilePressed;
        OnTileEntered += GameManager.instance.OnTileEntered;
        OnTileReleased += GameManager.instance.OnTileReleased;
        OnTileExit += GameManager.instance.OnTileExit;
        GameManager.instance.Board.onGenerationDone.AddListener(GetNeighbours);
    }

    public void SetTransform(Transform parent, Vector3 size, Vector2 offset)
    {
        transform.parent = parent;
        if (size.x < size.y)
        {
            size.y = size.z = size.x;
        }
        else
        {
            size.x = size.z = size.y;
        }
        transform.localScale = new Vector3(size.x, size.y, size.z);
        transform.position = new Vector3(offset.x, offset.y, 1);
    }

    private void OnMouseEnter()
    {
        OnTileEntered.Invoke(this);
    }
    private void OnMouseExit()
    {
        OnTileExit.Invoke(this);
    }
    private void OnMouseDown()
    {
        OnTilePressed.Invoke(this);
    }
    private void OnMouseUp()
    {
        OnTileReleased.Invoke(this);
    }

    private void GetNeighbours()
    {
        int row = int.Parse(id[0].ToString());
        int col = int.Parse(id[1].ToString());
        int index = 0;
        for (int i = row - 1; i <= row + 1;  i++)
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (i != row || j != col)
                {
                    neighbours[index] = GameManager.instance.Board.GetTile(i, j);
                    index++;
                }
            }

        GameManager.instance.Board.onGenerationDone.RemoveAllListeners();
    }

    public bool IsNeighbour(string id)
    {
        return neighbours.Contains(id);
    }
}
