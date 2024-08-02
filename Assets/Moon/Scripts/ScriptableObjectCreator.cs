// #if UNITY_EDITOR
// using UnityEditor;
// #endif
// using UnityEngine;
// using System.Collections.Generic;
// using System.Linq;
// using System;

// public static class ScriptableObjectCreator
// {
//     [MenuItem("SOAutoCreate/CreateEnemy")]
//     public static void CreateScriptableEnemyObjects()
//     {
//         MonsterInfo.Data.Load();
//         Team5DataTable_Value.FoodValueData.Load();
//         List<Team5DataTable_Type.FoodTypeData> foodTypes = Team5DataTable_Type.FoodTypeData.FoodTypeDataList;
//         List<Team5DataTable_Value.FoodValueData> foodValues = Team5DataTable_Value.FoodValueData.FoodValueDataList;
//         Debug.Log($"Food Types Count: {foodTypes.Count}");
//         Debug.Log($"Food Values Count: {foodValues.Count}");
//         string foodDataListPath = "Assets/Resources/FoodDataList.asset";
//         FoodDataList foodDataList = AssetDatabase.LoadAssetAtPath<FoodDataList>(foodDataListPath);
//         foodDataList.cleanList();
//         foreach (var foodType in foodTypes)
//         {
//             string assetPath = $"Assets/2.Scripts/ScriptableObject/Foods/{foodType.stageToUse}Stage/SO_{foodType.foodName}.asset";
// #if UNITY_EDITOR
//             ScriptableFood dataObject = AssetDatabase.LoadAssetAtPath<ScriptableFood>(assetPath);
// #endif
//             if (dataObject == null)
//             {
//                 dataObject = ScriptableObject.CreateInstance<ScriptableFood>();
//                 AssetDatabase.CreateAsset(dataObject, assetPath);
//             }

//             dataObject.index = foodType.index;
//             dataObject.foodName = foodType.foodName;
//             dataObject.foodNameInKorean = foodType.foodNameInKorean;
//             dataObject.stageToUse = foodType.stageToUse;

//             // foodValues를 필터링하여 dataObject에 저장
//             var filteredValues = foodValues.Where(v => v.foodName.StartsWith(foodType.foodName)).ToList();
//             dataObject.foodPrice = new int[filteredValues.Count];
//             dataObject.upgradeMoney = new int[filteredValues.Count];

//             foreach (var value in filteredValues)
//             {
//                 string[] split = value.foodName.Split('_');
//                 int levelIndex = int.Parse(split[1]) - 1;
//                 dataObject.foodPrice[levelIndex] = value.saleValue;
//                 dataObject.upgradeMoney[levelIndex] = value.upgradeValue;
//             }

//             EditorUtility.SetDirty(dataObject);
//             foodDataList.AddFood(dataObject);
//         }
//         EditorUtility.SetDirty(foodDataList);
// #if UNITY_EDITOR
//         AssetDatabase.SaveAssets();
//         Debug.Log("save Completed");
// #endif
//     }
// }