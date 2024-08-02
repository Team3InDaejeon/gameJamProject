using UnityEngine;


[CreateAssetMenu(fileName = "NewNPC", menuName = "GameJam/NPCInfo", order = 4)]
public class ScriptableNPC : ScriptableObject
{
    public string Index;
    public string Name;
    public int Stage;
    public string Script1;
    public string Script2;
    public string Script3;
    public string UnlockSkill;
}
