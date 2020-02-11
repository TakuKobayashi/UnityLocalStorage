using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

namespace NetworkConsole
{
    public class LocalStorageWindow : EditorWindow
    {
        bool foldoutSavedDataSettings = true;
        string httpPrefixPath;
        Vector2 scrollPos = Vector2.zero;

        void OnEnable()
        {
        }

        [MenuItem("Tools/LocalStorageWindow")]
        static void Open()
        {
            EditorWindow.GetWindow<LocalStorageWindow>("LocalStorageWindow");
        }

        void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
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
            EditorGUILayout.EndScrollView();
        }
    }
}