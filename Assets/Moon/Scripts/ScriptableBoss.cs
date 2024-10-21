using UnityEngine;


[CreateAssetMenu(fileName = "NewBoss", menuName = "GameJam/BossInfo", order = 0)]
public class ScriptableBoss : ScriptableObject
{
    public string Index;
    public string Name;
    public EnemyType Type;
    public float moveSpeed;
    public int Health;
    public int Atk1Damage;
    public int Atk2Damage;
    public int Atk3Damage;
}
