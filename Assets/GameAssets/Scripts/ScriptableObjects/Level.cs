using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Dino/Level")]
public class Level : ScriptableObject
{
    public int ID { get; set; }
    [Header("level")]
    public Color bgColor;
    public int tileScore;
    public int tileStars;
    [Header("board")]
    public int width;
    public int height;
    public int columnsNumber;
    public int rowsNumber;
    public Vector2 offset;
    [Header("monsters")]
    public List<GameObject> monsters = new List<GameObject>();
    public Animations idleAnimation = Animations.Idle;
    public Animations startAnimation = Animations.Hello;
    public Animations rightSelectedAnimation = Animations.Yes;
    public Animations wrongSelectedAnimation = Animations.No;
    public Animations exploadAnimation = Animations.DieFront;
    public Animations winAnimation = Animations.Happy;
    [Header("gameplay")]
    public int numberOfMoves;
    public int targetStars;
    [Header("music")]
    public AudioClip gameBackgroundMusic;
    [Header("sounds")]
    public AudioClip startSound;
    public AudioClip rightSelectedSound;
    public AudioClip wrongSelectedSound;
    public AudioClip exploadSound;
    public AudioClip winSound;

    public void AddMonster(GameObject monster)
    {
        monsters.Add(monster);
    }
    public void RemoveMonster(GameObject monster)
    {
        monsters.Remove(monster);
    }
    public GameObject GetRandomMonster()
    {
        int randomIndex = Random.Range(0, monsters.Count);
        return Instantiate(monsters[randomIndex]);
    }
}
