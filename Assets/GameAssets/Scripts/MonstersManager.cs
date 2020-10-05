using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _monsters;

    public GameObject GetRandomMonster(int numberOfMonsters)
    {
        numberOfMonsters = numberOfMonsters > _monsters.Length ? _monsters.Length : numberOfMonsters;
        int randomIndex = Random.Range(0, numberOfMonsters);
        return _monsters[randomIndex];
    }
}
