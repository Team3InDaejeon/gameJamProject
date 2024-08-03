using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    Image Image_Background;
    [SerializeField]
    Image Image_Name;
    [SerializeField]
    TextMeshProUGUI Text_Dialogue;
    [SerializeField]
    TextMeshProUGUI Text_Name;

    [SerializeField]
    GameObject UI_PlayerSkill;

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
        Text_Dialogue.text = NPCDataInfo.Name;
    }

    void Start()
    {
        this.SetActive(false);
    }

    public void StartDialogue() 
    {
        UI_PlayerSkill.SetActive(false);
        SetDialogue();
    }

    private void SetDialogue() 
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
            UI_PlayerSkill.SetActive(true);
            GameManager.Inst.NextStage();
        }
        else 
        {
            StartDialogue();
        }
    }
}
