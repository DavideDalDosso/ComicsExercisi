using UnityEditor;
using UnityEngine;

public class CharacterEditorWindow : EditorWindow
{
    private Character selectedCharacter;
    private Vector2 scrollPos;

    [MenuItem("Window/Character Manager")]
    public static void ShowWindow()
    {
        GetWindow<CharacterEditorWindow>("Character Manager");
    }

    private void Update()
    {
        this.Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Detected Characters", EditorStyles.boldLabel);

        Character[] characters = FindObjectsByType<Character>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (characters.Length == 0)
        {
            EditorGUILayout.HelpBox("No characters detected.", MessageType.Info);
            return;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));

        foreach (Character c in characters)
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle nameStyle = new GUIStyle(EditorStyles.label);
            nameStyle.normal.textColor = c.characterColor;

            EditorGUILayout.LabelField(c.characterName, nameStyle, GUILayout.Width(100));
            EditorGUILayout.LabelField($"Pos: {c.transform.position:F2}", GUILayout.Width(160));
            string activeText = c.isActive ? "Active" : "Inactive";
            EditorGUILayout.LabelField(activeText, GUILayout.Width(100));

            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                selectedCharacter = c;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(10);
        DrawCharacterEditor();
    }

    private void DrawCharacterEditor()
    {
        if (!selectedCharacter)
        {
            EditorGUILayout.HelpBox("Select a Character to edit.", MessageType.Info);
            return;
        }

        EditorGUILayout.LabelField("Character Properties", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        var name = EditorGUILayout.TextField("Name", selectedCharacter.characterName);
        selectedCharacter.characterName = name;
        selectedCharacter.gameObject.name = name;

        selectedCharacter.age = EditorGUILayout.IntField("Age", selectedCharacter.age);
        EditorGUILayout.EndHorizontal();

        if (selectedCharacter.age < 18)
        {
            EditorGUILayout.HelpBox("Warning: Character is under 18!", MessageType.Warning);
        }

        string toggleLabel = selectedCharacter.isActive ? "Deactivate" : "Activate";
        if (GUILayout.Button(toggleLabel))
        {
            selectedCharacter.isActive = !selectedCharacter.isActive;
            selectedCharacter.gameObject.SetActive(selectedCharacter.isActive);
        }

        var color = EditorGUILayout.ColorField("Color", selectedCharacter.characterColor);
        selectedCharacter.characterColor = color;
        var renderer = selectedCharacter.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = color;
        }
        
        var position = EditorGUILayout.Vector3Field("Position", selectedCharacter.transform.position);

        selectedCharacter.transform.position = position;
        selectedCharacter.position = position;
            

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Print Data in Console"))
        {
            string data = "";
            data += "Name: " + selectedCharacter.characterName + "\n";
            data += "Age: " + selectedCharacter.age + "\n";
            data += "Active: " + selectedCharacter.isActive + "\n";
            data += "Color: " + selectedCharacter.characterColor + "\n";
            data += "Position: " + selectedCharacter.transform.position + "\n";
            Debug.Log(data);
        }

    }
}
