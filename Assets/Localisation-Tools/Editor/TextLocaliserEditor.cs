//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class TextLocaliserEditor : EditorWindow 
//{
//   public static void Open(string key)
//    {
//        TextLocaliserEditor window = new TextLocaliserEditor();
//        window.titleContent = new GUIContent("Localiser Window");
//        window.ShowUtility();
//        window.key = key;
//    }

//    public string key;
//    public string value;

//    public void OnGUI()
//    {
//        key = EditorGUILayout.TextField("Key :", key);
//        EditorGUILayout.BeginHorizontal();
//        EditorGUILayout.LabelField("Value:", GUILayout.MaxWidth(50));

//        EditorStyles.textArea.wordWrap = true;
//        value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
//        EditorGUILayout.EndHorizontal();

//        if(GUILayout.Button("Add"))
//        {
//            if(LocalisationSystem.GetLocalisedValue(key)!=string.Empty)
//            {
//                LocalisationSystem.Replace(key, value);
//            }
//            else
//            {
//                LocalisationSystem.Add(key, value);
//            }
//        }

//        minSize = new Vector2(460, 250);
//        maxSize = minSize;
//    }
//}