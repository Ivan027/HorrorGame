using Unity.Cinemachine;
using Unity.Mathematics;
using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{ 
    [Header("Player Object")]
    [SerializeField] private PlayerMovement _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    
    [Header("Inventory Display")]
    [SerializeField] private InventoryDisplay _inventoryDisplay;
    
    [Header("Dialogue Display")]
    [SerializeField] private DialogueDisplay _dialogueDisplay;
    
    [Header("Game Task Display")]
    [SerializeField] private GameTaskDisplay _gameTaskDisplay;
    
    [Header("Start Scene Task")]
    [SerializeField] private GameTask _startTask;
    
    public override void InstallBindings()
    {
        InstallPlayerBindings();
        InstallDisplayBindings();
    }
    
    private void InstallPlayerBindings()
    {
        var spawnPosition = _playerSpawnPoint?.position ?? Vector3.zero;
        PlayerMovement player = Container.InstantiatePrefabForComponent<PlayerMovement>(_playerPrefab, spawnPosition,
            quaternion.identity, null);
        
        Container.Bind<PlayerCamera>().FromInstance(player.GetComponent<PlayerCamera>()).AsSingle();
        Container.Bind<PlayerMovement>().FromInstance(player).AsSingle();
        Container.Inject(player);
    }
    
    private void InstallDisplayBindings()
    {
        Container.Bind<DialogueDisplay>().FromInstance(_dialogueDisplay);
        Container.Bind<GameTaskDisplay>().FromInstance(_gameTaskDisplay);
    }
}