using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveNPC : MonoBehaviour
{
    ScriptableNPC NPCDataInfo;

    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        StartDialogue();
    }

    void StartDialogue() 
    {
    
    }
}
