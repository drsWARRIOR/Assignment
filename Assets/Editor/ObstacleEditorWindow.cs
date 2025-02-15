using UnityEngine;
using UnityEditor;

// Custom Editor Window for modifying the obstacle placement on the grid
public class ObstacleEditorWindow : EditorWindow
{
    SerializedObject obj;           
    SerializedProperty property;    

    // Opens the grid editor window and initializes it with the given serialized object and property
    public static void OpenWindow(SerializedObject obj, SerializedProperty property)
    {
        ObstacleEditorWindow win = GetWindow<ObstacleEditorWindow>("Obstacle Settings");
        win.obj = obj;
        win.property = property;

        win.Show();
    }

    //Unity built-in method to render UI element inside unity editor
    void OnGUI()
    {
        // Close the window if serialized object or property is missing
        if (obj == null || property == null)
        {
            Close();
            return;
        }

        obj.Update(); // Update the data before modifying

        EditorGUILayout.LabelField("Obstacle Editor", EditorStyles.boldLabel); // title of the window
        EditorGUILayout.Space(); //Space for better ui layout

        Rect newPos = GUILayoutUtility.GetRect(position.width, 18f); //Rectangle for grid element

        //Iterate through the grid
        for (int j = 0; j < 10; j++)
        {
            SerializedProperty row = property.GetArrayElementAtIndex(j).FindPropertyRelative("rows"); // Get row
            if (row.arraySize != 10) row.arraySize = 10; // it make sure each row has exactly 10 columns

            //adjustment for better ui layout
            newPos.width = position.width / 10;
            newPos.height = 18f;

            //Creating toggle field for every grid cell
            for (int i = 0; i < 10; i++)
            {
                EditorGUI.PropertyField(newPos, row.GetArrayElementAtIndex(i), GUIContent.none);
                newPos.x += newPos.width;
            }

            newPos.x = 0; //Reset x position for new row
            newPos.y += 18f; // Move to the next row
        }

        obj.ApplyModifiedProperties(); // Changes applied to the serialized object
    }
}
