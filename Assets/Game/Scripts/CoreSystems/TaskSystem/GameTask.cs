using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Task System/GameTask", fileName = "New GameTask")]
public class GameTask : ScriptableObject
{
    [SerializeField] private List<Subtask> _subtasks;
    
    [field: SerializeField] public string TaskName { get; private set; }
    [field: SerializeField] public GameTask NextTask { get; private set; }
    public List<Subtask> Subtasks => new List<Subtask>(_subtasks);
}

[System.Serializable]
public class Subtask
{
    [field: SerializeField] public string SubtaskDescription { get; private set; }
    [field: SerializeField] public GameTaskTypes GameTaskType { get; private set; }
    
    [field: SerializeField] public Item RequiredItem { get; private set; }
    [field: SerializeField] public string Location { get; private set; }
}

public enum GameTaskTypes
{
    TakeItem,
    GoTo
}