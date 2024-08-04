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
    GameObject Button_Next;

    [SerializeField]
    List<GameObject> UI_Ingame;

    List<string> Dialogues = new List<string>();

    int MaxIndex = 0;
    int CurrentIndex = 0;

    public void Initialize(ScriptableNPC NPCDataInfo) 
    {
        if (NPCDataInfo.Script1 != "0")
        {
            Dialogues.Add(NPCDataInfo.Script1);
        }
        if (NPCDataInfo.Script2 != "0")
        {
            Dialogues.Add(NPCDataInfo.Script2);
        }
        if (NPCDataInfo.Script3 != "0")
        {
            Dialogues.Add(NPCDataInfo.Script3);
        }

        MaxIndex = Dialogues.Count;
        Text_Dialogue.text = NPCDataInfo.Name;  
    }


    public void StartDialogue() 
    {
        for (int i = 0; i < UI_Ingame.Count; ++i)
        {
            UI_Ingame[i].SetActive(false);
        }
        Image_Background.gameObject.SetActive(true);
        Image_Name.gameObject.SetActive(true);
        Text_Dialogue.gameObject.SetActive(true);
        Text_Name.gameObject.SetActive(true);
        Button_Next.SetActive(true);

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
            for (int i = 0; i < UI_Ingame.Count; ++i) 
            {
                UI_Ingame[i].SetActive(true);
            }
            Image_Background.gameObject.SetActive(false);
            Image_Name.gameObject.SetActive(false);
            Text_Dialogue.gameObject.SetActive(false);
            Text_Name.gameObject.SetActive(false);
            Button_Next.SetActive(false);

            GameManager.Inst.NextStage();
        }
        else 
        {
            SetDialogue();
        }
    }
}
