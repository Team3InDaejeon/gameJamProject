using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveNPC : MonoBehaviour
{
    [SerializeField]
    ScriptableNPC NPCDataInfo;
    [SerializeField]
    int CurrentStage = 0;
    [SerializeField]
    DialogueUI UI_Dialogue;

    void Start()
    {
        UI_Dialogue = GameObject.FindWithTag("NPCUI").GetComponent<DialogueUI>();
        CurrentStage = GameManager.Inst.CurrentStage;
        if (UI_Dialogue) 
        {
            UI_Dialogue.Initialize(NPCDataInfo);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        StartDialogue();
    }

    void StartDialogue() 
    {
        int numOfChild = this.transform.childCount;
        for (int i = 0; i < numOfChild; i++){
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
        UI_Dialogue.StartDialogue();
    }
}
