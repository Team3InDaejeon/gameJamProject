using UnityEngine;


[CreateAssetMenu(fileName = "NewEnemy", menuName = "GameJam/EnemyInfo", order = 0)]
public class ScriptableEnemy : ScriptableObject
{
    public string Index;
    public string Name;
    public EnemyType Type;
    public int Health;
    public int StrikingPower;
}
