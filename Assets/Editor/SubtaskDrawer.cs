using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Subtask))]
public class SubtaskDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = 2; // TaskDescription и GameTaskType

        SerializedProperty typeProp = property.FindPropertyRelative("<GameTaskType>k__BackingField");

        if (typeProp != null)
        {
            GameTaskTypes type = (GameTaskTypes)typeProp.enumValueIndex;
            if (type == GameTaskTypes.TakeItem)
                lines++;
            else if (type == GameTaskTypes.GoTo)
                lines++;

            // ƒќЅј¬№ «ƒ≈—№: если по€в€тс€ новые типы задач Ч добавь нужное количество строк
        }

        return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines - 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);

        SerializedProperty descriptionProp = property.FindPropertyRelative("<SubtaskDescription>k__BackingField");
        SerializedProperty typeProp = property.FindPropertyRelative("<GameTaskType>k__BackingField");
        SerializedProperty itemProp = property.FindPropertyRelative("<RequiredItem>k__BackingField");
        SerializedProperty locationProp = property.FindPropertyRelative("<Location>k__BackingField");

        // ƒќЅј¬№ «ƒ≈—№: если добавишь новые пол€ в Subtask, добавь их SerializedProperty

        EditorGUI.PropertyField(currentRect, descriptionProp, new GUIContent("Description"));
        currentRect.y += lineHeight + spacing;

        EditorGUI.PropertyField(currentRect, typeProp, new GUIContent("Task Type"));
        currentRect.y += lineHeight + spacing;

        GameTaskTypes type = (GameTaskTypes)typeProp.enumValueIndex;

        if (type == GameTaskTypes.TakeItem)
        {
            EditorGUI.PropertyField(currentRect, itemProp, new GUIContent("Required Item"));
        }
        else if (type == GameTaskTypes.GoTo)
        {
            EditorGUI.PropertyField(currentRect, locationProp, new GUIContent("Location"));
        }

        // ƒќЅј¬№ «ƒ≈—№: отображение полей под новый тип задачи

        EditorGUI.EndProperty();
    }
}