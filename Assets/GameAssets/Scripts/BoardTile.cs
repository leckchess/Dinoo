using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BoardTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler,IPointerDownHandler
{
    private MonstersManager _monstersManager;
    private UnityEvent _onSelect = new UnityEvent();
    private UnityEvent _onDeselect = new UnityEvent();
    public bool IsChicked { get; set; }

    internal void Init(int numberOfMonsters)
    {
        if (_monstersManager == null)
            _monstersManager = FindObjectOfType<MonstersManager>();

        GameObject monster = Instantiate(_monstersManager.GetRandomMonster(numberOfMonsters));
        monster.transform.parent = transform;
        monster.transform.localPosition = Vector3.zero;
        monster.transform.localScale = Vector3.one;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if same color zi previous play yes else play no
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // play sad aw 7aga kda
        // lw prev == next yb2a clear this else add next lw same color
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // lw awl 7d yb2a al head
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // if there is more than 3 n destroy / n kill
    }


}
