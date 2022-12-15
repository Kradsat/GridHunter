using UnityEngine;
using UnityEditor;

public class TestWindow : EditorWindow
{
    public string key = "key";
    public string jp_value = "jp value";
    public string en_value = "en value";

    [MenuItem("Tool/Test Window")]
    public static void Init()
    {
        TestWindow window = (TestWindow)EditorWindow.GetWindow(typeof(TestWindow));
        window.titleContent = new GUIContent("Localiser Window");
        window.Show();
    }

    public void OnGUI()
    {
        key = EditorGUILayout.TextField("Key :", key);
        jp_value = EditorGUILayout.TextField("jp :", jp_value);
        en_value = EditorGUILayout.TextField("en :", en_value);

        if (GUILayout.Button("Add"))
        {
            if (LocalisationSystem.GetLocalisedValue(key) != string.Empty)
            {
                LocalisationSystem.Replace(key, jp_value, en_value);
            }
            else
            {
                LocalisationSystem.Add(key, jp_value, en_value );
            }
        }
    }
}
