using UnityEngine;
using UnityEditor;
using MistplaySDK;

[CustomEditor(typeof(MistplayFeedbackManager))]
public class MistplayFeedbackManagerEditor : Editor
{
    bool foldout;
    Texture icon;
    Color mainColor = Colors.NiceOrange;

    void OnEnable()
    {
        icon = Resources.Load("Icon_Feedback") as Texture;
    }

    public override void OnInspectorGUI()
    {
        if(Layout.DrawMistplayHeader("Feedback", mainColor)) return;

        Layout.Footer();
        Layout.RepaintIfMouseHover(this);
    }
}