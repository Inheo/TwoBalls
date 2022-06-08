using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
namespace MistplaySDK 
{
    public class Layout
    {
        public class Styles
        {
            public static GUIStyle borders = new GUIStyle()
            {
                margin = new RectOffset(0, 10, 5, 10)
            };

            public static GUIStyle box = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(15, 15, 15, 15),
                margin = new RectOffset(10, 10, 10, 10)
            };

            public static GUIStyle boxTight = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(5, 5, 5, 5),
                margin = new RectOffset(10, 10, 10, 10)
            };

            public static GUIStyle boxTightNoBg = new GUIStyle()
            {
                padding = new RectOffset(5, 5, 5, 5),
                margin = new RectOffset(10, 10, 10, 10)
            };

            public static GUIStyle headerBox = new GUIStyle()
            {
                padding = new RectOffset(5, 5, 0, 5),
                margin = new RectOffset(5, 5, 5, 5)
            };

            public static GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12,
                padding = new RectOffset(5, 0, 0, 0),
                fontStyle = FontStyle.Bold
            };

            public static GUIStyle foldoutHeaderStyle = new GUIStyle()
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(5,5,5,5),
                normal = new GUIStyleState() 
                {
                    textColor = Color.white.Alpha(.8f)
                }
            };

            public static GUIStyle subtitleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.white
                }
            };
        };

        public class Category
        {
            public string name;
            public bool enabled;
            public Color color;
            public Texture icon;
            public string[] variables;

            public Category(string name, Texture icon, params string[] variables)
            {
                this.name       = name;
                this.color      = Colors.NiceRed;
                this.variables  = variables;
                this.icon       = icon;
            }

            public void Draw(SerializedObject obj)
            {
                enabled = icon == null ? 
                Layout.DrawHeaderFoldout(enabled, name, color) :
                Layout.DrawHeaderFoldoutWithIcon(enabled, name, color, icon);

                if(enabled)
                {
                    EditorGUILayout.BeginVertical();
                    Layout.Space(5);

                    foreach(var v in variables)
                    {
                        var prop = obj.FindProperty(v);
                        if(prop != null)
                        EditorGUILayout.PropertyField(prop);
                        else EditorGUILayout.LabelField("Couldnt find property : " + v);
                    }

                    Layout.Space();
                    EditorGUILayout.EndVertical();
                    
                    var r = GUILayoutUtility.GetLastRect();
                        r.Set(r.x - 18, r.y, 3, r.height);

                    EditorGUI.DrawRect(r, color.Alpha(.3f));
                }

                obj.ApplyModifiedProperties();
            }
        }

        public static Texture logoImage, sdkImage;

        public static void Footer()
        {
            Space(20);
        }

        public static void RepaintIfMouseHover(Editor editor)
        {
            if(new Rect(0, 0, Screen.width, Screen.height).Contains(Event.current.mousePosition))
            {
                editor.Repaint();
            }   
        }

        public static void DrawCategoryBorder(Color color)
        {
            var r = GUILayoutUtility.GetLastRect();
                r.Set(r.x - 18, r.y, 3, r.height);

            EditorGUI.DrawRect(r, color.Alpha(.3f));
        }

        public static bool DrawMistplayHeader(string subtitle, Color subtitleColor)
        {
            if(logoImage == null) 
            {
                logoImage = Resources.Load("Mistplay_Logo_Tiny") as Texture2D; 
            }
            if(sdkImage == null) sdkImage = Resources.Load("Mistplay_Logo_Sdk") as Texture2D; 

            int height = 30;
            var fill = new Rect(0, 0, Screen.width, height);
            EditorGUI.DrawRect(fill, new Color(28/255f, 28/255f, 84/255f, 255).Alpha(.2f));

            var border = new Rect(fill);
            border.width = 3;
            EditorGUI.DrawRect(border, subtitleColor);

            var r = new Rect(10, 5, 85, 17);
            GUI.DrawTexture(r, logoImage);

            var sdkRect = r;
            sdkRect.x += sdkRect.width + 5;
            sdkRect.width = 28;
            sdkRect.height = 12;
            sdkRect.y = height * .5f - sdkRect.height * .5f - 1;
            GUI.color = subtitleColor;
            GUI.DrawTexture(sdkRect, sdkImage);
            GUI.color = Color.white;    

            var rr = new Rect(Screen.width - 20, 0, 30, height - 3);
            // debug = GUI.Toggle(rr, debug, "");
            var labelWidth = EditorStyles.boldLabel.CalcSize(new GUIContent(subtitle)).x;
            rr.x -= labelWidth;
            rr.width = labelWidth;

            GUI.color = subtitleColor;
            EditorGUI.LabelField(rr, subtitle, Styles.subtitleStyle);
            GUI.color = Color.white;

            fill.y += fill.height - 1;
            fill.height = 2;
            // EditorGUI.DrawRect(fill, subtitleColor);

            // if(debug) 
            // {
            //     Layout.Space(30);
            //     base.OnInspectorGUI();
            //     return true;
            // }

            Space(height - 5);
            
            return false;
        }

        public static bool DrawHeaderFoldoutWithIcon(bool value, string label, Color color, Texture icon)
        {
            EditorGUI.indentLevel++;
            var r = GUILayoutUtility.GetRect(new GUIContent(label), Styles.foldoutHeaderStyle);
            r.x -= 15;
            r.width = Screen.width;
            EditorGUI.DrawRect(r, value ? Colors.Grey(.3f) : Colors.Grey(.25f));
            
            var rr = r;
            rr.y += rr.height;
            rr.y -= 1;
            rr.height = 1;
            //EditorGUI.DrawRect(rr, Colors.Grey(.3f));

            rr.y += 1;
            rr.x -= 3;
            rr.width += 3;
            EditorGUI.DrawRect(rr, Colors.Grey(.2f));

            rr = r;
            rr.height = 1;
            rr.y -= 1;
            rr.x -= 3;
            rr.width += 3;
            EditorGUI.DrawRect(rr, Colors.Grey(.2f));

            int iconWidth = 16;
            int iconHeight = 16;
            if(icon != null)
            {
                var iconRect = r;
                iconRect.width = iconWidth;
                iconRect.x += 5;
                iconRect.height = iconHeight;
                iconRect.y += (r.height - iconHeight) / 2;

                GUI.color = value ? color : Color.grey;
                GUI.DrawTexture(iconRect, icon);
                GUI.color = Color.white;

                r.x += iconWidth + 14;

                iconRect.x += iconWidth + 5;
                iconRect.width = 1;
                iconRect.y = r.y;
                iconRect.height = r.height;
                EditorGUI.DrawRect(iconRect, Colors.Grey(.2f));
            }

            GUI.color = value ? Color.white : new Color(.9f, .9f, .9f, 1f);
            value = EditorGUI.Foldout(r, value, label, true, Styles.foldoutHeaderStyle);
            GUI.color = Color.white;
            r.x -= iconWidth + 14;

            EditorGUI.indentLevel--;

            r.width = 3;
            r.x -= 3;
            EditorGUI.DrawRect(r, value ? color : Color.grey);

            var rect = GUILayoutUtility.GetLastRect();
            if(rect.Contains(Event.current.mousePosition))
            {
                rect.x = 0;
                rect.width = Screen.width;
                EditorGUI.DrawRect(rect, Color.white.Alpha(.1f));
            }

            return value;
        }

        public static bool DrawHeaderFoldout(bool value, string label, Color color)
        {
            EditorGUI.indentLevel++;
            var r = GUILayoutUtility.GetRect(new GUIContent(label), Styles.foldoutHeaderStyle);
            r.x -= 15;
            r.width += 18;
            EditorGUI.DrawRect(r, value ? Colors.Grey(.3f) : Colors.Grey(.25f));
            
            var rr = r;
            rr.y += rr.height;
            rr.y -= 1;
            rr.height = 1;
            //EditorGUI.DrawRect(rr, Colors.Grey(.3f));

            rr.y += 1;
            rr.x -= 3;
            rr.width += 3;
            EditorGUI.DrawRect(rr, Colors.Grey(.2f));

            rr = r;
            rr.height = 1;
            rr.y -= 1;
            rr.x -= 3;
            rr.width += 3;
            EditorGUI.DrawRect(rr, Colors.Grey(.2f));

            GUI.color = value ? Color.white : new Color(.9f, .9f, .9f, 1f);
            value = EditorGUI.Foldout(r, value, label, true, Styles.foldoutHeaderStyle);
            GUI.color = Color.white;
            EditorGUI.indentLevel--;

            r.width = 3;
            r.x -= 3;
            EditorGUI.DrawRect(r, value ? color : Color.grey);

            var rect = GUILayoutUtility.GetLastRect();
            if(rect.Contains(Event.current.mousePosition))
            {
                rect.x = 0;
                rect.width = Screen.width;
                EditorGUI.DrawRect(rect, Color.white.Alpha(.1f));
            }

            return value;
        }

        public static void Space(float amount = 10)
        {
            EditorGUILayout.Space(amount);
        }
    }
}
#endif
