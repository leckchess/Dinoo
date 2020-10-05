using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Dino/Level")]
public class Level : ScriptableObject
{
    public int width;
    public int height;
    public int columnsNumber;
    public int rowsNumber;
    public List<GameObject> _monsters = new List<GameObject>();

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
