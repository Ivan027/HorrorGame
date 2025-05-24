using System;
using TMPro;
using UnityEngine;
using Zenject;

public class GameTaskDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskText;
    
    private GameTaskController _gameTaskController;
    
    [Inject]
    public void Construct(GameTaskController gameTaskController)
    {
        _gameTaskController = gameTaskController;
    }

    private void Awake()
    {
        DisplaySubtask(_gameTaskController.GetCurrentSubtask());
    }

    private void OnEnable()
    {
        _gameTaskController.OnSubtaskChanged += DisplaySubtask;
    }

    private void OnDisable()
    {
        _gameTaskController.OnSubtaskChanged -= DisplaySubtask;
    }

    private void DisplaySubtask(Subtask subtask)
    {
        _taskText.text = subtask == null ? "" : subtask.SubtaskDescription;
    }
}
