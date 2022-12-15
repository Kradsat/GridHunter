using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TextLocaliserSearchWindow : EditorWindow
{
    public string value = "key";
    public Vector2 scroll;
    public Dictionary<string, string> dictionary;

    private string searchResult = string.Empty;
    private bool isSearching = false;
    private bool isShowingResult = false;

    [MenuItem("Tool/Search Window")]
    public static void Init()
    {
        TextLocaliserSearchWindow window = (TextLocaliserSearchWindow)EditorWindow.GetWindow(typeof(TextLocaliserSearchWindow));
        window.titleContent = new GUIContent("Search Window");
        window.Show();
    }

    public void OnGUI()
    {
        if (!isSearching)
        {
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
            value = EditorGUILayout.TextField(value);

            if (GUILayout.Button("Search"))
            {
                GetSearchResults();
            }
        }
        else
        {
            if (isShowingResult)
            {
                EditorGUILayout.LabelField("Result: ", searchResult);

                if (searchResult != "NO SUCH KEY")
                { 
                    Texture closeIcon = (Texture)Resources.Load("close");
                    GUIContent content = new GUIContent(closeIcon);

                    if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                    {
                        if (EditorUtility.DisplayDialog("Remove Key " + value + "?", "This will remove the element from the localisation, are you sure?", "Do it"))
                        {
                            LocalisationSystem.Remove(value);
                            AssetDatabase.Refresh();
                            LocalisationSystem.Init();
                            dictionary = LocalisationSystem.GetDictionaryForEditor();
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("Searching...");
            }
        }
    }

    private void GetSearchResults()
    {
        if (value == "") { return; }

        isSearching = true;

        dictionary = LocalisationSystem.GetDictionaryForEditor();

        if (dictionary.ContainsKey(value))
        {
            searchResult = dictionary[value];
        }
        else
        {
            searchResult = "NO SUCH KEY";
        }
        
        isShowingResult = true;
    }
}