using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTaskController
{
    private GameTask _currentGameTask;
    private Queue<Subtask> _activeSubtasks = new Queue<Subtask>();
    private InventoryController _inventoryController;
    
    public event Action<Subtask> OnSubtaskChanged; 
    
    public GameTaskController(InventoryController inventoryController)
    {
        _inventoryController = inventoryController;
    }

    #region Tasks

    public void SetGameTask(GameTask task)
    {
        if (task == null) return;
        
        Debug.Log($"Установлено задание {task.TaskName}");
        
        _currentGameTask = task;
        _activeSubtasks = new Queue<Subtask>(_currentGameTask.Subtasks);
        SettingNextSubtask();

        // EventManager.TriggerTaskChanged(_currentGameTask);
    }
    
    private void CompleteTask()
    {
        SetGameTask(_currentGameTask.NextTask); 
        // EventManager.TriggerTaskCompleted(_currentGameTask);
    }
    
    #endregion

    #region Subtasks

    private void SettingNextSubtask()
    {
        Subtask currentSubtask = GetCurrentSubtask();
        OnSubtaskChanged?.Invoke(currentSubtask);

        if (currentSubtask == null) { CompleteTask(); return; } // Если больше нет сабтасков
        
        switch (currentSubtask.GameTaskType)
        {
            case GameTaskTypes.TakeItem:
                _inventoryController.OnItemAdded += CompleteSubtask;
                break;
        }
    }
    
    public Subtask GetCurrentSubtask()
    {
        return _activeSubtasks.Count <= 0 ? null : _activeSubtasks.Peek() ;
    }
    
    #endregion

    #region CheckConditions

    private void CompleteSubtask()
    {
        Debug.Log($"Задание \"{GetCurrentSubtask().SubtaskDescription}\" выполнено");
        
        _activeSubtasks.Dequeue();
        SettingNextSubtask();
    }
    
    private void CompleteSubtask(Item item)
    {
        if (item == GetCurrentSubtask().RequiredItem)
        {
            CompleteSubtask();
        }
    }

    #endregion
}