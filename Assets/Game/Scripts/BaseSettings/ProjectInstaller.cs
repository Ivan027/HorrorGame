using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private int _startInventorySize;
    [SerializeField] private List<Item> _startItems = new List<Item>();
    
    public override void InstallBindings()
    {
        InstallInputBindings();
        Container.Bind<InventoryController>().AsSingle().WithArguments(_startInventorySize, _startItems).NonLazy();
        Container.Bind<GameTaskController>().AsSingle().Lazy();
        Container.Bind<DialogueController>().AsSingle();
    }
    
    private void InstallInputBindings()
    {
        var input = new GameInput();
        input.Enable();
        Container.Bind<GameInput>().FromInstance(input).AsSingle().NonLazy();
    }
}
