using UnityEngine;


[CreateAssetMenu(fileName = "NewCharacter", menuName = "GameJam/CharacterInfo", order = 1)]
public class ScriptableCharacter : ScriptableObject
{
    public string Index;
    public string Name;
    public string Description;
    public EnemyType Type;
    public int Health;
    public int StrikingPower;
}
