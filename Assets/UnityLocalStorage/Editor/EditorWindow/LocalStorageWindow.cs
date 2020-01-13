using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

namespace NetworkConsole
{
    public class LocalStorageWindow : EditorWindow
    {
        bool foldoutLoggingSettings = true;
        bool foldoutSavedDataSettings = true;

        int showLocalStorageLog;
        string httpPrefixPath;

        Vector2 scrollPos = Vector2.zero;

        // オブジェクトがロードされたとき、この関数は呼び出されます。
        void OnEnable()
        {
            showLocalStorageLog = PlayerPrefs.GetInt("ShowLocalStorageLogKey", 1);
        }

        // メニューのWindowにEditorExという項目を追加。
        [MenuItem("Tools/LocalStorageWindow")]
        static void Open()
        {
            // メニューのWindow/EditorExを選択するとOpen()が呼ばれる。
            // 表示させたいウィンドウは基本的にGetWindow()で表示＆取得する。
            EditorWindow.GetWindow<LocalStorageWindow>("LocalStorageWindow");
        }

        // Windowのクライアント領域のGUI処理を記述
        void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            WithInFoldoutBlock("Logging", ref foldoutLoggingSettings, () =>
            {
                int _showLocalStorageLog = EditorGUILayout.Toggle("LocalStorage", PlayerPrefs.GetInt("ShowLocalStorageLogKey", showLocalStorageLog) == 1) ? 1 : 0;
                if (showLocalStorageLog != _showLocalStorageLog)
                {
                    PlayerPrefs.SetInt("ShowLocalStorageLogKey", showLocalStorageLog = _showLocalStorageLog);
                }
            });

            WithInFoldoutBlock("Stored Data", ref foldoutSavedDataSettings, () =>
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Persistent Directory");
                if (GUILayout.Button("Reveal In Finder"))
                {
                    Debug.Log(Application.persistentDataPath);
                    EditorUtility.RevealInFinder(Application.persistentDataPath);
                }
                if (GUILayout.Button("Clear"))
                {
                    Debug.Log("Deleted everything in " + Application.persistentDataPath);
                    Directory.Delete(Application.persistentDataPath, true);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("LocalStorage");
                if (GUILayout.Button("Clear"))
                {
                    UnityLocalStorage.LocalStorage.Clear();
                    Debug.Log("LocalStorage Cleared");
                }
                EditorGUILayout.EndHorizontal();
            });

            EditorGUILayout.EndScrollView();
        }

        void WithInFoldoutBlock(string title, ref bool foldout, Action callback)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            foldout = EditorGUILayout.Foldout(foldout, title);
            if (foldout)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.Space();
                callback();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }
}