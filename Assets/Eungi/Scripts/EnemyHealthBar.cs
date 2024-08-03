using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider EnemyHPbar;

    private int EnemyMaxHealth;
    private int EnemyCurrentHealth;

    CharacterStat stat;

    void Start()
    {
        stat = GetComponent<CharacterStat>();
        //EnemyMaxHealth = this.GetComponent<CharacterStat>().GetMaxHealth();
        //EnemyCurrentHealth = this.GetComponent<CharacterStat>().GetHealth();
        //EnemyHPbar.value = (float)EnemyCurrentHealth / (float)EnemyMaxHealth;
    }

    void Update()
    {
        EnemyMaxHealth = stat.GetMaxHealth();
        EnemyCurrentHealth = stat.GetHealth();
        UpdateHP();
    }

    private void UpdateHP()
    {
        EnemyHPbar.value = Mathf.Lerp(EnemyHPbar.value, (float)EnemyCurrentHealth / (float)EnemyMaxHealth,
        Time.deltaTime * 10);
    }
}
