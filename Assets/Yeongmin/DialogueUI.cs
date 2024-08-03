using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    Image Image_Backgorund;
    [SerializeField]
    TextMeshProUGUI Text_Dialogue;

    List<string> Dialogues = new List<string>();

    int MaxIndex = 0;
    int CurrentIndex = 0;

    public void Initialize(ScriptableNPC NPCDataInfo) 
    {
        if (!string.IsNullOrEmpty(NPCDataInfo.Script1))
        {
            Dialogues.Add(NPCDataInfo.Script1);
        }
        if (!string.IsNullOrEmpty(NPCDataInfo.Script2))
        {
            Dialogues.Add(NPCDataInfo.Script2);
        }
        if (!string.IsNullOrEmpty(NPCDataInfo.Script3))
        {
            Dialogues.Add(NPCDataInfo.Script3);
        }

        MaxIndex = Dialogues.Count;
    }

    public void StartDialogue() 
    {
        if (Dialogues.Count > 0)
        {
            Text_Dialogue.text = Dialogues[CurrentIndex];
        }
    }

    public void NextButton() 
    {
        ++CurrentIndex;
        if (CurrentIndex >= MaxIndex)
        {
            GameManager.Inst.NextStage();
        }
        else 
        {
            StartDialogue();
        }
    }
}
