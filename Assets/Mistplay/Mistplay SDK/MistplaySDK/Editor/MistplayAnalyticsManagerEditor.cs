using UnityEngine;
using UnityEditor;
using MistplaySDK;
using System.Collections.Generic;

#if mistplay_gameanalytics_enabled
[CustomEditor(typeof(MistplayAnalyticsManager))]
[CanEditMultipleObjects]
public class MistplayAnalyticsManagerEditor : Editor
{
    GameAnalyticsSDK.Setup.Settings settings;
    Editor editor;
    bool gaFold, smartlookFold, gaSettingsFold;
    int viewIndex, selectedPlatformIndex;

    GUIContent gameKeyLabel = new GUIContent("Game Key", "Your GameAnalytics Game Key - copy/paste from the GA website.");
    GUIContent secretKeyLabel = new GUIContent("Secret Key", "Your GameAnalytics Secret Key - copy/paste from the GA website.");

    Texture iconSmartlook;
    Texture iconGameAnalytics;

    List<string> gameKeys, secretKeys;

    void OnEnable()
    {
        iconSmartlook = Resources.Load("Icon_Smartlook") as Texture;
        iconGameAnalytics = Resources.Load("Icon_GameAnalytics") as Texture;

        var assets = AssetDatabase.FindAssets("t:GameAnalyticsSDK.Setup.Settings");
        if(assets.Length > 0)
        {
            settings = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(GameAnalyticsSDK.Setup.Settings)) as GameAnalyticsSDK.Setup.Settings;
            editor = Editor.CreateEditor(settings);
        }
        else Debug.LogError("No Game Analytics settings found!");
    }

    public override void OnInspectorGUI()
    {
        Layout.DrawMistplayHeader("Analytics", Colors.NicePink);

        // GameAnalytics Foldout
        gaFold = Layout.DrawHeaderFoldoutWithIcon(gaFold, "GameAnalytics", Colors.NicePink, iconGameAnalytics);
        if(gaFold) 
        {
            using (new EditorGUILayout.VerticalScope())
            {
                Layout.Space();

                // Views Buttons
                using(new EditorGUILayout.HorizontalScope())
                {
                    viewIndex = GUILayout.Toolbar(viewIndex, new string[]{"Simple", "Advanced"});
                    GUILayout.FlexibleSpace();
                }

                Layout.Space();

                switch(viewIndex)
                {
                    case 0:
                    {
                        // Display Game/Secret keys
                        using(new EditorGUILayout.VerticalScope())
                        {
                            gameKeys = new List<string>();
                            secretKeys = new List<string>();

                            for (int i = 0; i < settings.Platforms.Count; i++)
                            {
                                if(i > gameKeys.Count || i == 0) 
                                {
                                    gameKeys.Add("");
                                    secretKeys.Add("");
                                }

                                // Platform Header
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUILayout.LabelField(settings.Platforms[i].ToString(), EditorStyles.boldLabel);
                                    GUILayout.FlexibleSpace();
                                    if(GUILayout.Button("x"))
                                    {
                                        settings.RemovePlatformAtIndex(i);
                                        return;
                                    }
                                }

                                // Game Key Text Field
                                EditorGUI.BeginChangeCheck();

                                Texture l = EditorGUIUtility.IconContent("InspectorLock").image;
                                gameKeyLabel.image = l;
                                secretKeyLabel.image = l;

                                gameKeys[gameKeys.Count - 1] = EditorGUILayout.TextField(gameKeyLabel, settings.GetGameKey(i));
                                if(EditorGUI.EndChangeCheck())
                                {
                                    //if(gameKeys[gameKeys.Count - 1] != "")
                                    settings.UpdateGameKey(i, gameKeys[gameKeys.Count - 1]);
                                }

                                // Secret Key Text Field
                                EditorGUI.BeginChangeCheck();
                                secretKeys[secretKeys.Count - 1] = EditorGUILayout.TextField(secretKeyLabel, settings.GetSecretKey(i));
                                if(EditorGUI.EndChangeCheck())
                                {
                                    if(secretKeys[secretKeys.Count - 1] != "")
                                    settings.UpdateSecretKey(i, secretKeys[secretKeys.Count - 1]);
                                }

                                Layout.Space();
                            }

                            var platforms = settings.GetAvailablePlatforms();
                            selectedPlatformIndex = EditorGUILayout.Popup("Platform to add", selectedPlatformIndex, platforms);
                            if (GUILayout.Button("Add platform"))
                            {
                                if (platforms[this.selectedPlatformIndex].Equals("WSA"))
                                {
                                    settings.AddPlatform(RuntimePlatform.WSAPlayerARM);
                                }
                                else
                                {
                                    settings.AddPlatform((RuntimePlatform)System.Enum.Parse(typeof(RuntimePlatform), platforms[this.selectedPlatformIndex]));
                                }
                                this.selectedPlatformIndex = 0;
                            }
                        }
                        break;
                    }
                    case 1:
                    {
                        editor.OnInspectorGUI();
                        break;
                    }
                }
            }

            Layout.DrawCategoryBorder(Colors.NicePink);
        }
        
        if(serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
        }

        Layout.Footer();
        Layout.RepaintIfMouseHover(this);
    }
}
#endif