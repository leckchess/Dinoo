using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardTile : MonoBehaviour
{
    public string id { get; private set; }
    public int Column
    {
        get
        {
            string[] splittedid = id.Split('-');
            return int.Parse(splittedid[1].ToString());
        }
        private set { }
    }
    public int Row
    {
        get
        {
            string[] splittedid = id.Split('-');
            return int.Parse(splittedid[0].ToString());
        }
        private set { }
    }
    public Color TileColor { get { return monster.color; } }
    public string PrevTileId { get; internal set; }
    public string NextTileId { get; internal set; }

    public Monster monster;
    public List<string> neighbours = new List<string>();

    private UnityAction<BoardTile> OnTilePressed;
    private UnityEvent OnTileReleased = new UnityEvent();
    private UnityAction<BoardTile> OnTileEntered;
    private UnityAction<BoardTile> OnTileExit;
    private UnityAction<BoardTile> onReInit;


    private AudioSource _audioSource;

    private void Start()
    {
        ListenToEvents();
    }
    internal void Init(GameObject tilemonster, string tileid, int idleanimation, AudioClip startsound)
    {
        id = name = tileid;
        monster = tilemonster.GetComponent<Monster>();
        monster.SetTransform(transform, Vector3.zero, Vector3.one);
        monster.PlayAnimation(idleanimation);
        GameManager.instance.Board.onGenerationDone?.AddListener(GetNeighbours);
        _audioSource = gameObject.AddComponent<AudioSource>();
        SoundManager.instance.PlayDelaiedSound(_audioSource, startsound,Random.Range(0,0.5f));
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
        OnTileEntered?.Invoke(this);
    }
    private void OnMouseExit()
    {
        OnTileExit?.Invoke(this);
    }
    private void OnMouseDown()
    {
        OnTilePressed?.Invoke(this);
    }
    private void OnMouseUp()
    {
        OnTileReleased?.Invoke();
    }
    private void GetNeighbours()
    {
        // neigbours from row -1 to row +1 and col-1 to col +1
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
    public void Expload(AudioClip exploadSound)
    {
        // destroy the monster and rechange position and get new monster
        Destroy(monster.gameObject);
        SoundManager.instance.PlaySound(_audioSource, exploadSound);
        onReInit.Invoke(this);
    }

    public void PlayAnimation(int animation)
    {
        monster.PlayAnimation(animation);
    }
}
