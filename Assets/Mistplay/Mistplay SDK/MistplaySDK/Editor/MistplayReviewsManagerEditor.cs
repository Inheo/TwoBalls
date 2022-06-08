using UnityEngine;
using UnityEditor;
using MistplaySDK;

[CustomEditor(typeof(MistplayReviewsManager))]
public class MistplayReviewsManagerEditor : Editor
{
    bool foldout;
    Texture icon;
    Color mainColor = Colors.NiceYellow;

    Layout.Category settingsCategory;

    void OnEnable()
    {
        icon = Resources.Load("Icon_Star") as Texture;

        settingsCategory = new Layout.Category("Reviews Settings", icon, "MinimumRating");
        settingsCategory.color = mainColor;
    }

    public override void OnInspectorGUI()
    {
        if(Layout.DrawMistplayHeader("Reviews", mainColor)) return;

        settingsCategory.Draw(serializedObject);

        Layout.Footer();
        Layout.RepaintIfMouseHover(this);
    }
}