using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueNode))]
public class DialogueNodeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        var nodeTypeProp = property.FindPropertyRelative("_nodeType");
        var phrasesProp = property.FindPropertyRelative("_phrases");
        var answersProp = property.FindPropertyRelative("_answers");

        float height = 0f;

        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // foldout
        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // id
        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // name
        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 10f; // next node
        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // node type

        if ((DialogueNodeType)nodeTypeProp.enumValueIndex == DialogueNodeType.Phrase)
        {
            height += EditorGUI.GetPropertyHeight(phrasesProp, true);
        }
        else
        {
            height += EditorGUI.GetPropertyHeight(answersProp, true);
        }

        return height + EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, property.displayName, true);

        if (!property.isExpanded)
            return;

        var idProp = property.FindPropertyRelative("_id");
        var nameProp = property.FindPropertyRelative("_nodeName");
        var nextNodeProp = property.FindPropertyRelative("_nextNode");
        var nodeTypeProp = property.FindPropertyRelative("_nodeType");
        var phrasesProp = property.FindPropertyRelative("_phrases");
        var answersProp = property.FindPropertyRelative("_answers");

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        EditorGUI.BeginProperty(position, label, property);

        Rect idRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(idRect, "Node ID", idProp.intValue.ToString());
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect nameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        nameProp.stringValue = EditorGUI.TextField(nameRect, "Node Name", nameProp.stringValue);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        Rect nextNodeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(nextNodeRect, nextNodeProp, new GUIContent("Next Node"));
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 10f;

        Rect typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typeRect, nodeTypeProp, new GUIContent("Node Type"));
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if ((DialogueNodeType)nodeTypeProp.enumValueIndex == DialogueNodeType.Phrase)
        {
            EditorGUI.PropertyField(position, phrasesProp, new GUIContent("Phrases"), true);
        }
        else
        {
            EditorGUI.PropertyField(position, answersProp, new GUIContent("Answers"), true);
        }

        EditorGUI.EndProperty();
    }
}