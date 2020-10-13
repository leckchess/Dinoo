using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Dino/Level")]
public class Level : ScriptableObject
{
    [Header("level")]
    public Color bgColor;
    public Color bgPhoto;
    [Header("board")]
    public int width;
    public int height;
    public int columnsNumber;
    public int rowsNumber;
    [Header("monsters")]
    public List<GameObject> _monsters = new List<GameObject>();
    public Animations idleAnimation = Animations.Idle;
    public Animations startAnimation = Animations.Hello;
    public Animations rightSelectedAnimation = Animations.Yes;
    public Animations wrongSelectedAnimation = Animations.No;
    public Animations exploadAnimation = Animations.DieFront;
    public Animations winAnimation = Animations.Happy;
    public Animations losAnimation = Animations.Sick;
    [Header("gameplay")]
    public int numberOfMoves;
    public int time;
    public int numberOfHints;
    public bool timeBased;
    public void AddMonster(GameObject monster)
    {
        _monsters.Add(monster);
    }
    public void RemoveMonster(GameObject monster)
    {
        _monsters.Remove(monster);
    }
    public GameObject GetRandomMonster()
    {
        int randomIndex = Random.Range(0, _monsters.Count);
        return Instantiate(_monsters[randomIndex]);
    }
}
