using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BoardTile : MonoBehaviour
{
    public string id { get; private set; }
    public Monster monster;
    public bool IsChicked { get; set; }
    public UnityAction<BoardTile> onMouseDown;
    public UnityAction<BoardTile> onMouseUp;
    public UnityAction<BoardTile> onMouseEnter;
    public UnityAction<BoardTile> onMouseExit;
    internal void Init(GameObject tilemonster, string tileid)
    {
        id = name = tileid;
        monster = tilemonster.AddComponent<Monster>();
        monster.SetTransform(transform, Vector3.zero, Vector3.one);
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
        onMouseEnter.Invoke(this);
    }
    private void OnMouseExit()
    {
        onMouseEnter.Invoke(this);
          }
    private void OnMouseDown()
    {
        onMouseDown.Invoke(this);
    }
    private void OnMouseUp()
    {
        onMouseUp.Invoke(this);
    }
}
