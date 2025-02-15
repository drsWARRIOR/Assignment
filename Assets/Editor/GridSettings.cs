using UnityEditor;
using UnityEngine;

//this class provides a ui button in the unity inspector to open the appropriate grid editor
[CustomPropertyDrawer(typeof(ArraySetup))]
public class GridSettings : PropertyDrawer
{
    //override the default unity property gui to add custom button for opening editor
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Draw the default property label
        EditorGUI.PrefixLabel(position, label);

        // defines the button position
        Rect buttonRect = new Rect(position.x, position.y + 20f, position.width, 20f);

        //get the serialized object containing this property
        SerializedObject obj = property.serializedObject;

        //get the actual object the property belongs to
        Object targetObj = obj.targetObject; 

        //check if the target object is an obstacle manager
        if (targetObj is ObstacleManager)
        {
            //Display Obstacle Editor open button
            if (GUI.Button(buttonRect, "Open Obstacle Editor"))
            {
                ObstacleEditorWindow.OpenWindow(property.serializedObject, property.FindPropertyRelative("table"));
            }
        }
        else
        {
            //Display Grid accessibility Editor open button
            if (GUI.Button(buttonRect, "Open Grid Editor"))
            {
                GridEditorWindow.OpenWindow(property.serializedObject, property.FindPropertyRelative("table"));
            }
        }
    }

    //Sets the height of the custom property in inspector
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 50f; // increase the height to make layout better
    }
}