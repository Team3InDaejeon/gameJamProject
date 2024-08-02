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
}