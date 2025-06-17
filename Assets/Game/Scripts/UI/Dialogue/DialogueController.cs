using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController
{
    private Dialogue _currentDialogue;
    private int _currentNodeIndex;
    private Queue<Phrase> _currentNodePhrases = new Queue<Phrase>();

    public event Action<Dialogue> OnDialogueStarted;
    public event Action OnDialogueEnded;
    public event Action<string, Speaker, AudioClip> OnPhraseUpdated;
    public event Action<List<Answer>> OnAnswerPresented;

    public bool IsDialogueActive => _currentDialogue != null;

    public void StartDialogue(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
        _currentNodeIndex = 0;

        OnDialogueStarted?.Invoke(dialogue);
        EnterNode(_currentNodeIndex);
    }

    public void EndDialogue()
    {
        _currentDialogue = null;
        _currentNodePhrases.Clear();
        OnDialogueEnded?.Invoke();
    }

    public void ContinueDialogue(int answerIndex = -1)
    {
        if (_currentDialogue == null) return;

        if (_currentNodePhrases.Count > 0)
        {
            var phrase = _currentNodePhrases.Dequeue();
            OnPhraseUpdated?.Invoke(phrase.Text, phrase.Speaker, phrase.Voice);
            return;
        }

        var node = _currentDialogue.DialogueNodes[_currentNodeIndex];

        if (node.NodeType == DialogueNodeType.Phrase)
        {
            if (node.NextNode < 0)
            {
                EndDialogue();
            }
            else
            {
                _currentNodeIndex = node.NextNode;
                EnterNode(_currentNodeIndex);
            }
        }
        else if (node.NodeType == DialogueNodeType.AnswerTab && answerIndex >= 0)
        {
            var selectedAnswer = node.Answers[answerIndex];
            _currentNodeIndex = selectedAnswer.NextNodeIndex;
            EnterNode(_currentNodeIndex);
        }
    }

    private void EnterNode(int index)
    {
        if (_currentDialogue == null || index < 0 || index >= _currentDialogue.DialogueNodes.Count)
        {
            EndDialogue();
            return;
        }

        var node = _currentDialogue.DialogueNodes[index];

        if (node.NodeType == DialogueNodeType.Phrase)
        {
            _currentNodePhrases = new Queue<Phrase>(node.Phrases);
            ContinueDialogue();
        }
        else if (node.NodeType == DialogueNodeType.AnswerTab)
        {
            OnAnswerPresented?.Invoke(node.Answers);
        }
    }
}