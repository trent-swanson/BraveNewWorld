using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor.AnimatedValues;
using UnityEditor.Rendering;

namespace StylizedGrass
{
    public class StylizedGrassGUI : Editor
    {
        private const string AssetIconData = "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";
        private static Texture m_AssetIcon;
        public static Texture AssetIcon
        {
            get
            {
                if(m_AssetIcon == null) m_AssetIcon = CreateIcon(AssetIconData);
                return m_AssetIcon;
            }
        }
        
        private static Texture CreateIcon(string data)
        {
            byte[] bytes = System.Convert.FromBase64String(data);

            Texture2D icon = new Texture2D(32, 32, TextureFormat.RGBA32, false, false);
            icon.LoadImage(bytes, true);
            return icon;
        }

        
        private static GUIStyle _Header;
        public static GUIStyle Header
        {
            get
            {
                if (_Header == null)
                {
                    _Header = new GUIStyle(GUI.skin.label)
                    {
                        richText = true,
                        alignment = TextAnchor.MiddleCenter,
                        wordWrap = true,
                        fontSize = 18,
                        fontStyle = FontStyle.Normal
                    };
                }

                return _Header;
            }
        }

        private static GUIStyle _Tab;
        public static GUIStyle Tab
        {
            get
            {
                if (_Tab == null)
                {
                    _Tab = new GUIStyle(EditorStyles.miniButtonMid)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = true,
                        fontSize = 12,
                        fixedHeight = 27.5f,
                        fontStyle = FontStyle.Bold,
                        padding = new RectOffset()
                        {
                            left = 14,
                            right = 14,
                            top = 8,
                            bottom = 8
                        }
                    };
                }

                return _Tab;
            }
        }

        private static GUIStyle _Button;
        public static GUIStyle Button
        {
            get
            {
                if (_Button == null)
                {
                    _Button = new GUIStyle(UnityEngine.GUI.skin.button)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = true,
                        padding = new RectOffset()
                        {
                            left = 14,
                            right = 14,
                            top = 8,
                            bottom = 8
                        }
                    };
                }

                return _Button;
            }
        }

        public static void DrawActionBox(string text, string label, MessageType messageType, Action action)
        {
            Assert.IsNotNull(action);

            EditorGUILayout.HelpBox(text, messageType);

            GUILayout.Space(-32);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(label, GUILayout.Width(60)))
                    action();

                GUILayout.Space(8);
            }
            GUILayout.Space(11);
        }

        public static class Material
        {
            //Section toggles
            public class Section
            {
                public bool Expanded
                {
                    get { return SessionState.GetBool(id, false); }
                    set { SessionState.SetBool(id, value); }
                }
                public bool showHelp;
                public const float animSpeed = 16f;
                public AnimBool anim;

                public readonly string id;
                public string title;

                public Section(MaterialEditor target, string id, string title)
                {
                    this.id = "SGS_" + id + "_SECTION";
                    this.title = title;

                    anim = new AnimBool(true);
                    anim.valueChanged.AddListener(target.Repaint);
                    anim.speed = animSpeed;
                }

                public void SetTarget()
                {
                    anim.target = Expanded;
                }
            }

            //https://github.com/Unity-Technologies/Graphics/blob/d0473769091ff202422ad13b7b764c7b6a7ef0be/com.unity.render-pipelines.core/Editor/CoreEditorUtils.cs#L460
            public static bool DrawHeader(string title, bool isExpanded, Action clickAction = null)
            {
#if URP
                CoreEditorUtils.DrawSplitter();
#endif

                var backgroundRect = GUILayoutUtility.GetRect(1f, 25f);

                var labelRect = backgroundRect;
                labelRect.xMin += 8f;
                labelRect.xMax -= 20f + 16 + 5;

                var foldoutRect = backgroundRect;
                foldoutRect.xMin -= 8f;
                foldoutRect.y += 0f;
                foldoutRect.width = 25f;
                foldoutRect.height = 25f;

                // Background rect should be full-width
                backgroundRect.xMin = 0f;
                backgroundRect.width += 4f;

                // Background
                float backgroundTint = EditorGUIUtility.isProSkin ? 0.1f : 1f;
                EditorGUI.DrawRect(backgroundRect, new Color(backgroundTint, backgroundTint, backgroundTint, 0.2f));

                // Title
                EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);

                // Foldout
                isExpanded = GUI.Toggle(foldoutRect, isExpanded, new GUIContent(isExpanded ? "−" : "≡"), EditorStyles.boldLabel);

                // Context menu
                #if URP
                var menuIcon = CoreEditorStyles.paneOptionsIcon;
#else
                Texture menuIcon = null;
#endif
                var menuRect = new Rect(labelRect.xMax + 3f + 16 + 5, labelRect.y + 1f, menuIcon.width, menuIcon.height);

                //if (clickAction != null)
                //GUI.DrawTexture(menuRect, menuIcon);

                // Handle events
                var e = Event.current;

                if (e.type == EventType.MouseDown)
                {
                    if (clickAction != null && menuRect.Contains(e.mousePosition))
                    {
                        e.Use();
                    }
                    else if (labelRect.Contains(e.mousePosition))
                    {
                        if (e.button == 0)
                        {
                            isExpanded = !isExpanded;
                            if (clickAction != null) clickAction.Invoke();
                        }

                        e.Use();
                    }
                }

#if URP
                //CoreEditorUtils.DrawSplitter();
#endif

                //GUILayout.Space(5f);

                return isExpanded;
            }

            public static void DrawMinMaxSlider(string label, ref float min, ref float max)
            {
                float minVal = min;
                float maxVal = max;

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(label, GUILayout.MaxWidth(EditorGUIUtility.labelWidth - 30));
                    EditorGUILayout.LabelField(System.Math.Round(minVal, 2).ToString(), GUILayout.Width(75f));
                    EditorGUILayout.MinMaxSlider(ref minVal, ref maxVal, 0f, 500f);
                    EditorGUILayout.LabelField(System.Math.Round(maxVal, 2).ToString(), GUILayout.Width(75f));
                }

                min = minVal;
                max = maxVal;
            }

            public static void DrawFloatField(MaterialProperty prop, string label = null, string tooltip = null)
            {
                prop.floatValue = EditorGUILayout.FloatField(new GUIContent(label ?? prop.displayName, null, tooltip), prop.floatValue, GUILayout.MaxWidth(EditorGUIUtility.labelWidth + 50f));
            }

            public static void DrawFloatTicker(MaterialProperty prop, string label = null, string tooltip = null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(new GUIContent(label ?? prop.displayName, null, tooltip));
                if (GUILayout.Button("«", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
                {
                    prop.floatValue -= 1f;
                }
                if (GUILayout.Button("‹", EditorStyles.miniButtonMid, GUILayout.MaxWidth(17)))
                {
                    prop.floatValue -= .1f;
                }
                prop.floatValue = EditorGUILayout.FloatField(prop.floatValue, GUILayout.MaxWidth(45));
                if (GUILayout.Button("›", EditorStyles.miniButtonMid, GUILayout.MaxWidth(17)))
                {
                    prop.floatValue += .1f;
                }
                if (GUILayout.Button("»", EditorStyles.miniButtonRight, GUILayout.MaxWidth(20)))
                {
                    prop.floatValue += 1f;
                }
                EditorGUILayout.EndHorizontal();
            }

            public static void DrawSlider(MaterialProperty prop, string label = null, string tooltip = null)
            {
                prop.floatValue = EditorGUILayout.Slider(new GUIContent(label ?? prop.displayName, null, tooltip), prop.floatValue, prop.rangeLimits.x, prop.rangeLimits.y);
            }

            public static void Toggle(MaterialProperty prop, string label = null, string tooltip = null)
            {
                prop.floatValue = EditorGUILayout.Toggle(new GUIContent(label ?? prop.displayName, null, tooltip), prop.floatValue == 1f ? true : false) ? 1f : 0f;
            }

            public static bool Toggle(bool value, string label, string tooltip = null)
            {
                return EditorGUILayout.Toggle(new GUIContent(label, null, tooltip), value);
            }

            public static void DrawColorField(MaterialProperty prop, bool hdr, string name = null, string tooltip = null)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.PrefixLabel(new GUIContent(name ?? prop.displayName, tooltip));
                    prop.colorValue = EditorGUILayout.ColorField(new GUIContent("", null, tooltip), prop.colorValue, true, true, hdr, GUILayout.MaxWidth(60f));
                }
            }

            public static void DrawVector3(MaterialProperty prop, string name, string tooltip = null)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.PrefixLabel(new GUIContent(name, tooltip));
                    GUILayout.Space(-15f);
                    prop.vectorValue = EditorGUILayout.Vector3Field(new GUIContent("", null, tooltip), prop.vectorValue);
                }
            }
        }
        
        public class ParameterGroup
        {
            static ParameterGroup()
            {
                Section = new GUIStyle(EditorStyles.helpBox)
                {
                    margin = new RectOffset(0, 0, -10, 10),
                    padding = new RectOffset(10, 10, 10, 10),
                    clipping = TextClipping.Clip,
                };

                headerLabel = new GUIStyle(EditorStyles.miniLabel);
                headerBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
                headerBackgroundLight = new Color(1f, 1f, 1f, 0.2f);
                paneOptionsIconDark = (Texture2D)EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/pane options.png");
                paneOptionsIconLight = (Texture2D)EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/pane options.png");
                splitterDark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
                splitterLight = new Color(0.6f, 0.6f, 0.6f, 1.333f);
            }

            public static readonly GUIStyle headerLabel;
            public static GUIStyle Section;
            static readonly Texture2D paneOptionsIconDark;
            static readonly Texture2D paneOptionsIconLight;
            public static Texture2D paneOptionsIcon { get { return EditorGUIUtility.isProSkin ? paneOptionsIconDark : paneOptionsIconLight; } }
            static readonly Color headerBackgroundDark;
            static readonly Color headerBackgroundLight;
            public static Color headerBackground { get { return EditorGUIUtility.isProSkin ? headerBackgroundDark : headerBackgroundLight; } }

            static readonly Color splitterDark;
            static readonly Color splitterLight;
            public static Color splitter { get { return EditorGUIUtility.isProSkin ? splitterDark : splitterLight; } }

            public static void DrawHeader(GUIContent content)
            {
                //DrawSplitter();
                Rect backgroundRect = GUILayoutUtility.GetRect(1f, 20f);

                if (content.image)
                {
                    content.text = " " + content.text;
                }

                Rect labelRect = backgroundRect;
                labelRect.y += 0f;
                labelRect.xMin += 5f;
                labelRect.xMax -= 20f;

                // Background rect should be full-width
                backgroundRect.xMin = 10f;
                //backgroundRect.width -= 10f;

                // Background
                EditorGUI.DrawRect(backgroundRect, headerBackground);

                // Title
                EditorGUI.LabelField(labelRect, content, EditorStyles.boldLabel);

                DrawSplitter();
            }

            public static void DrawSplitter()
            {
                var rect = GUILayoutUtility.GetRect(1f, 1f);

                // Splitter rect should be full-width
                rect.xMin = 10f;
                //rect.width -= 10f;

                if (Event.current.type != EventType.Repaint)
                    return;

                EditorGUI.DrawRect(rect, splitter);
            }


        }

    }
}