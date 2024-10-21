using UnityEngine;


[CreateAssetMenu(fileName = "NewSkill", menuName = "GameJam/SkillInfo", order = 2)]
public class ScriptableSkill : ScriptableObject
{
    public string Index;
    public string Name;
    public string Condition;
    public string Effect;
    public int Cooltime;
}
