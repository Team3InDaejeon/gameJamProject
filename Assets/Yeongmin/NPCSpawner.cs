using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    NonPlayerType NPCType;
    [Header("순서대로 넣어주세요")]
    public List<GameObject> monsterEntries = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterPlayer Player = other.GetComponent<CharacterPlayer>();
        if (Player != null)
        {
            GameObject npc = Instantiate(monsterEntries[(int)NPCType], transform.position, Quaternion.identity);
            MonsterManager.Inst.AddMonster(npc);
        }
    }
}
