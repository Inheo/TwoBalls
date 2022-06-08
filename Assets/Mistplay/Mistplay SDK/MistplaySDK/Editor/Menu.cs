using UnityEngine;
using UnityEditor;
using GameAnalyticsSDK;

namespace MistplaySDK
{
    static class Menu
    {
        [MenuItem("Mistplay/Create Analytics Manager", false, 200)]
        static void AddAnalytics() => AddManager<GameAnalytics>("Analytics");

        [MenuItem("Mistplay/Create Advertising Manager", false, 400)]
        static void AddAdvertising() => AddManager<MistplayAdvertisingManager>("Advertising");

        [MenuItem("Mistplay/Create Hex Manager", false, 500)]
        static void AddHex() => AddManager<MistplayHexManager>("Hex");

        [MenuItem("Mistplay/Create Reviews Manager", false, 600)]
        static void AddReviews() => AddManager<MistplayReviewsManager>("Reviews");

        // [MenuItem("Mistplay/Create Session Manager", false, 700)]
        // static void AddSession() => AddManager<MistplaySessionManager>("Session");

        [MenuItem("Mistplay/Create All Managers", false, 0)]
        static void AddEverything()
        {
            AddAnalytics();
            AddAdvertising();
            AddHex();
            AddReviews();
            //AddSession();
        }
        
        static void AddManager<T>(string name)
        {
            if(Object.FindObjectOfType(typeof(T)) == null)
            {
                GameObject go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(GameAnalytics.WhereIs($"Mistplay {name} Manager.prefab", "Prefab"), typeof(GameObject))) as GameObject;
                go.name = "Mistplay - " + name;
                Selection.activeObject = go;
                Undo.RegisterCreatedObjectUndo(go, "Created " + name); 
            }
            else Debug.LogWarning($"A Mistplay {name} Manager already exists in this scene - you should never have more than one per scene!");
        }
    }
}
