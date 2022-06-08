using UnityEngine;
using UnityEditor;
using MistplaySDK;

[CustomEditor(typeof(MistplayHexManager))]
public class MistplayHexManagerEditor : Editor
{
    bool foldout;
    Texture icon;
    Color mainColor = Colors.NicePurple;

    Layout.Category settingsCategory;

    void OnEnable()
    {
        icon = Resources.Load("Icon_Hex") as Texture;

        settingsCategory = new Layout.Category(
            "Hex Settings", icon,
            "overlaySide",
            "overlayPadding",
            "initializeByDefault",
            "dontDestroyOnLoad"
        );

        settingsCategory.color = mainColor;
    }

    public override void OnInspectorGUI()
    {
        if(Layout.DrawMistplayHeader("Hex", mainColor)) return;

        settingsCategory.Draw(serializedObject);

        Layout.Footer();
        Layout.RepaintIfMouseHover(this);
    }
}