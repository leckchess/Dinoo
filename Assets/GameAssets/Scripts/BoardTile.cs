using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardTile : MonoBehaviour
{
    public string id { get; private set; }
    public int Column { get { return int.Parse(id[1].ToString()); } private set { } }
    public int Row { get { return int.Parse(id[0].ToString()); } private set { } }
    public Monster monster;
    public bool IsChicked { get; set; }
    UnityAction<BoardTile, Action<int>> OnTilePressed;
    UnityEvent OnTileReleased = new UnityEvent();
    UnityAction<BoardTile, Action<int>> OnTileEntered;
    UnityAction<BoardTile, Action<int>> OnTileExit;
    UnityAction<BoardTile> onReInit;

    public List<string> neighbours = new List<string>();
    public Color TileColor { get { return monster.color; } }

    public string PrevTileId { get; internal set; }
    public string NextTileId { get; internal set; }

    private void Start()
    {
        ListenToEvents();
    }

    internal void Init(GameObject tilemonster, string tileid, int idleanimation)
    {
        id = name = tileid;
        monster = tilemonster.GetComponent<Monster>();
        monster.SetTransform(transform, Vector3.zero, Vector3.one);
        monster.PlayAnimation(idleanimation);
        GameManager.instance.Board.onGenerationDone?.AddListener(GetNeighbours);
    }

    internal void Init(string tileid, int idleanimation)
    {
        id = name = tileid;
        monster.PlayAnimation(idleanimation);
    }

    private void ListenToEvents()
    {
        OnTilePressed += GameManager.instance.OnTilePressed;
        OnTileEntered += GameManager.instance.OnTileEntered;
        OnTileReleased.AddListener(GameManager.instance.OnTileReleased);
        OnTileExit += GameManager.instance.OnTileExit;
        onReInit += GameManager.instance.Board.OnTileReinit;
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
        transform.DOMove(new Vector3(offset.x, offset.y, 1), 1);
    }

    public void SetMonster(Monster newmonster)
    {
        monster = newmonster;
        monster.SetTransform(transform, UnityEngine.Random.insideUnitCircle * 50, Vector3.one);
        monster.transform.DOLocalMove(Vector3.zero, 1);
    }
    private void OnMouseEnter()
    {
        OnTileEntered.Invoke(this, (animation) =>
        {
            monster.PlayAnimation(animation);
        });
    }
    private void OnMouseExit()
    {
        OnTileExit.Invoke(this, (animation) =>
        {
            monster.PlayAnimation(animation);
        });
    }
    private void OnMouseDown()
    {
        OnTilePressed.Invoke(this, (animation) =>
        {
            monster.PlayAnimation(animation);
        });
    }
    private void OnMouseUp()
    {
        OnTileReleased.Invoke();
    }
    private void GetNeighbours()
    {
        neighbours.Clear();
        for (int i = Row - 1; i <= Row + 1; i++)
            for (int j = Column - 1; j <= Column + 1; j++)
            {
                if (i != Row || j != Column)
                {
                    if (GameManager.instance.Board.GetTile(i, j) != null)
                        neighbours.Add(GameManager.instance.Board.GetTile(i, j));
                }
            }
    }
    public bool IsNeighbour(string id)
    {
        return neighbours.Contains(id);
    }
    public void ReInitTile()
    {
        Destroy(monster.gameObject);
        onReInit.Invoke(this);
    }
}
