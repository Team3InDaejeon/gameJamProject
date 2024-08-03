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

            // 8, 9, 10 NPCType == BossType
            if ((int)NPCType > 7 && (int)NPCType < 11)
            {
                GameManager.Inst.EnterBossCombat();
            }
            Destroy(this);
        }
       
    }

    // 1, 2, 3 // 11,12,13,14
    public void SpawnInteractiveNPC(int CurrentStage)
    {
        int NPCIndex = CurrentStage + 10;
        Instantiate(monsterEntries[NPCIndex], transform.position, Quaternion.identity);
        Destroy(this);
        // InteractiveNPC if (npc.GetComponent<InteractiveNPC>()) 
    }
}
