#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System;

public static class ScriptableObjectCreator
{
    [MenuItem("SOAutoCreate/CreateEnemy")]
    public static void CreateScriptableEnemyObjects()
    {
        MonsterInfo.Mob.Load();
        List<MonsterInfo.Mob> mobTypes = MonsterInfo.Mob.MobList;
        Debug.Log($"Mob Types Count: {mobTypes.Count}");
        foreach (var mobType in mobTypes)
        {
            string assetPath = $"Assets/Moon/Scripts/Mobs/SO_{mobType.Index}.asset";
#if UNITY_EDITOR
            ScriptableEnemy dataObject = AssetDatabase.LoadAssetAtPath<ScriptableEnemy>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableEnemy>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.Index = mobType.Index;
            dataObject.Name = mobType.Name;
            dataObject.Type = (EnemyType)Enum.Parse(typeof(EnemyType), mobType.EnemyType);
            dataObject.Health = mobType.Hp;
            dataObject.StrikingPower = mobType.Damage;

            EditorUtility.SetDirty(dataObject);
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed");
#endif
    }
    [MenuItem("SOAutoCreate/CreateBoss")]
    public static void CreateScriptableBossObjects()
    {
        MonsterInfo.Boss.Load();
        List<MonsterInfo.Boss> bossTypes = MonsterInfo.Boss.BossList;
        Debug.Log($"Boss Types Count: {bossTypes.Count}");
        foreach (var bossType in bossTypes)
        {
            string assetPath = $"Assets/Moon/Scripts/Mobs/SO_{bossType.Index}.asset";
#if UNITY_EDITOR
            ScriptableBoss dataObject = AssetDatabase.LoadAssetAtPath<ScriptableBoss>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableBoss>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.Index = bossType.Index;
            dataObject.Name = bossType.Name;
            dataObject.Type = (EnemyType)Enum.Parse(typeof(EnemyType), bossType.EnemyType);
            dataObject.Health = bossType.Hp;
            dataObject.Atk1Damage = bossType.Atk1Damage;
            dataObject.Atk2Damage = bossType.Atk2Damage;
            dataObject.Atk3Damage = bossType.Atk3Damage;

            EditorUtility.SetDirty(dataObject);
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed");
#endif
    }
    [MenuItem("SOAutoCreate/CreateSkill")]
    public static void CreateScriptableSkillObjects()
    {
        SkillInfo.CharacterSkill.Load();
        List<SkillInfo.CharacterSkill> skillTypes = SkillInfo.CharacterSkill.CharacterSkillList;
        Debug.Log($"Skill Types Count: {skillTypes.Count}");
        foreach (var skillType in skillTypes)
        {
            string assetPath = $"Assets/Moon/Scripts/Skills/SO_{skillType.Index}.asset";
#if UNITY_EDITOR
            ScriptableSkill dataObject = AssetDatabase.LoadAssetAtPath<ScriptableSkill>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableSkill>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.Index = skillType.Index;
            dataObject.Name = skillType.Name;
            dataObject.Condition = skillType.Condition;
            dataObject.Effect = skillType.Effect;
            dataObject.Cooltime = skillType.Cooltime;

            EditorUtility.SetDirty(dataObject);
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed");
#endif
    }
    [MenuItem("SOAutoCreate/CreateNPC")]
    public static void CreateScriptableNPCObjects()
    {
        MonsterInfo.Npc.Load();
        List<MonsterInfo.Npc> npcTypes = MonsterInfo.Npc.NpcList;
        Debug.Log($"Npc Types Count: {npcTypes.Count}");
        foreach (var npcType in npcTypes)
        {
            string assetPath = $"Assets/Moon/Scripts/Npcs/SO_{npcType.Index}.asset";
#if UNITY_EDITOR
            ScriptableNPC dataObject = AssetDatabase.LoadAssetAtPath<ScriptableNPC>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableNPC>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.Index = npcType.Index;
            dataObject.Name = npcType.Name;
            dataObject.Stage = npcType.Stage;
            dataObject.Script1 = npcType.Script1;
            dataObject.Script2 = npcType.Script2;
            dataObject.Script3 = npcType.Script3;
            dataObject.UnlockSkill = npcType.UnlockSkill;

            EditorUtility.SetDirty(dataObject);
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed");
#endif
    }
}