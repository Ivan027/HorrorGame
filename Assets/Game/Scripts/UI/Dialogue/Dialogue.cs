using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private List<DialogueNode> _dialogueNodes = new List<DialogueNode>();
    
    public List<DialogueNode> DialogueNodes => new List<DialogueNode>(_dialogueNodes);
    
    private void OnValidate()
    {
        for (int i = 0; i < _dialogueNodes.Count; i++)
        {
            _dialogueNodes[i].SetId(i);
        }
    }
}

[System.Serializable]
public class DialogueNode
{
    [SerializeField] private int _id;
    [SerializeField] private string _nodeName;
    [SerializeField] private int _nextNode;
    
    [SerializeField] private DialogueNodeType _nodeType;
    
    [SerializeField] private List<Phrase> _phrases = new List<Phrase>();
    [SerializeField] private List<Answer> _answers = new List<Answer>();

    public int NextNode => _nextNode;
    public DialogueNodeType NodeType => _nodeType;
    public List<Phrase> Phrases => new List<Phrase>(_phrases);
    public List<Answer> Answers => new List<Answer>(_answers);
    
    public void SetId(int id)
    {
        _id = id;
    }
}

[System.Serializable]
public class Phrase
{
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public Speaker Speaker{ get; private set; }
    [field: SerializeField] public AudioClip Voice { get; private set; }
}

[System.Serializable]
public class Answer
{
    [field: SerializeField] public string AnswerText { get; private set; }
    [Tooltip("Индекс нода, на который ведет ответ.")]
    [field: SerializeField] public int NextNodeIndex { get; private set; }
}

public enum DialogueNodeType
{
    Phrase,
    AnswerTab
}